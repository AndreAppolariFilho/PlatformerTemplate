// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/spriteShader"
{
    Properties
    {
        [NoScaleOffset] _MainTex ("Sprite Atlas", 2D) = "white" {}
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _TextureRect ("Base Texture Rect", Vector) = (0, 0, 0, 0)
        // The position and size of this sprite on the original texture atlas
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #pragma multi_compile_instancing
            #pragma multi_compile _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            #pragma instancing_options assumeuniformscaling
            // Sprites will not scale in my game

            #pragma instancing_options nolightmap
            #pragma instancing_options nolightprobe
            // My game does not use lighting (it's all pixel art)

            #include "UnitySprites.cginc"

            UNITY_INSTANCING_BUFFER_START(Props)
                UNITY_DEFINE_INSTANCED_PROP(float4, _TextureRect)
                // Sprites are GPU-instanced where supported
            UNITY_INSTANCING_BUFFER_END(Props)

            // These four properties are set once, when the game is first loaded
            uniform float4 _MainTex_TexelSize;
            uniform float2 _InitialSeed;
            uniform float4 _SpriteRectChunks[128];
            // Coordinates of each 8x8 sprite chunk I want to use in the shader
            // x,y are the positions (in pixels) of the chunk on the original atlas, z,w are unused and set to 0
            uniform int _SpriteRectChunkCount = 0;
            // Not all of these vectors are used, so I also pass in how many actually are


            struct vert2frag_img
            {
                float4 pos : SV_POSITION;
                half2 uv : TEXCOORD0;
                nointerpolation float4 chunk : POSITION1;
                // The chunk selection is done in the vertex shader

                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

            // Taken from https://stackoverflow.com/a/3451607/1089957
            float2 remap(float2 value, float2 low1, float2 high1, float2 low2, float2 high2)
            {
                return low2 + (value - low1) * (high2 - low2) / (high1 - low1);
            }

            // Given a vec2 as a seed, return a number from [0, 1)
            // Taken from https://stackoverflow.com/a/10625698/1089957
            float random(float2 seed)
            {
                const float2 r = float2(
                    23.1406926327792690,
                    2.6651441426902251
                );
                return frac(cos(fmod(123456789., 1e-7 + 256. * dot(seed, r))));  
            }

            int random(float2 seed, int limit) {
                return (int)round(random(seed) * limit);
            }

            vert2frag_img vert(appdata_img IN)
            {
                vert2frag_img OUT;

                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                OUT.pos = UnityFlipSprite(IN.vertex, _Flip);
                OUT.pos = UnityObjectToClipPos(OUT.pos);
                OUT.uv = IN.texcoord;

                float2 position = mul(unity_ObjectToWorld, float4(0, 0, 0, 1)).xy;

                #ifdef UNITY_INSTANCING_ENABLED
                int index = random(round(position * _InitialSeed * 8 + UNITY_GET_INSTANCE_ID(IN)), _SpriteRectChunkCount);
                #else
                int index = random(round(position * _InitialSeed * 8), _SpriteRectChunkCount);
                #endif
                OUT.chunk = _SpriteRectChunks[index];

                #ifdef PIXELSNAP_ON
                OUT.pos = UnityPixelSnap(OUT.pos);
                #endif

                return OUT;
            }

            fixed4 frag(vert2frag_img IN) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(IN);

                float2 pixels = remap(IN.uv, 0, 1, 0, _MainTex_TexelSize.zw);
                // texture coords (range [0, 1]) -> texture pixels (range [0, TextureSize])

                pixels = pixels - UNITY_ACCESS_INSTANCED_PROP(Props, _TextureRect).xy;
                // Sprite pixel coordinate, relative to (0, 0)

                pixels = fmod(pixels, 8);
                // Display glitched sprites in 8x8 chunks

                pixels = pixels + IN.chunk.xy;
                // Back to the sprite

                pixels = remap(pixels, 0, _MainTex_TexelSize.zw, 0, 1);
                // Remap to normalized coordinates

                fixed4 c = tex2D(_MainTex, clamp(pixels, 0, 1));

                c.rgb *= c.a;
                return c;
            }

        ENDCG
        }
    }
}
