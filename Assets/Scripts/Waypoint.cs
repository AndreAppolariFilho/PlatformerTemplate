using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Waypoint : MonoBehaviour
{
    // Start is called before the first frame update
    public Waypoint right;
    public Waypoint left;
    public Waypoint down;
    public Waypoint up;
    public GameObject trail_right;
    public GameObject trail_left;
    public GameObject trail_down;
    public GameObject trail_up;
    public Light2D light_down;
    public Light2D light_up;
    public Light2D light_left;
    public Light2D light_right;

    public Texture2D m_EmissionLeft;
    public Texture2D m_EmissionLeftUp;
    public Texture2D m_EmissionLeftDown;
    public Texture2D m_EmissionLeftRight;
    public Texture2D m_EmissionLeftRightUp;
    public Texture2D m_EmissionLeftRightUpDown;
    public Texture2D m_EmissionLeftUpDown;
    public Texture2D m_EmissionLeftRightDown;
    public Texture2D m_EmissionRight;
    public Texture2D m_EmissionRightUp;
    public Texture2D m_EmissionRightDown;
    public Texture2D m_EmissionRightUpDown;
    public Texture2D m_EmissionUp;
    public Texture2D m_EmissionUpDown;
    
    public Texture2D m_EmissionDown;

    public enum ConnectionType{
        Right,
        Left,
        Up,
        Down,
        LeftRight,
        LeftUp,
        LeftDown,
        LeftRightUp,
        LeftRightUpDown,
        LeftUpDown,
        LeftRightDown,
        RightUp,
        RightDown,
        RightUpDown,
        UpDown,
        None
    };
    public ConnectionType m_ActualConnection;
    public void ActivateEmissionLeft()
    {
        if(m_EmissionLeft)
        {
            switch(m_ActualConnection)
            {
                case ConnectionType.None:
                    m_ActualConnection = ConnectionType.Left;
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.EnableKeyword("_Emission");
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.SetTexture("_Emission",m_EmissionLeft);
                    break;
                case ConnectionType.Left:break;
                case ConnectionType.Right:
                    m_ActualConnection = ConnectionType.LeftRight;
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.EnableKeyword("_Emission");
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.SetTexture("_Emission",m_EmissionLeftRight);
                    break;
                case ConnectionType.Up:
                    m_ActualConnection = ConnectionType.LeftUp;
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.EnableKeyword("_Emission");
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.SetTexture("_Emission",m_EmissionLeftUp);
                    break;
                case ConnectionType.Down:
                    m_ActualConnection = ConnectionType.LeftDown;
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.EnableKeyword("_Emission");
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.SetTexture("_Emission",m_EmissionLeftDown);
                    break;
                case ConnectionType.LeftRight:break;
                case ConnectionType.LeftUp:break;
                case ConnectionType.LeftDown:break;
                case ConnectionType.LeftRightUpDown:break;
                case ConnectionType.RightUp:
                    m_ActualConnection = ConnectionType.LeftRightUp;
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.EnableKeyword("_Emission");
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.SetTexture("_Emission",m_EmissionLeftRightUp);
                    break;
                case ConnectionType.RightDown:
                    m_ActualConnection = ConnectionType.LeftRightDown;
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.EnableKeyword("_Emission");
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.SetTexture("_Emission",m_EmissionLeftRightDown);
                    break;
                case ConnectionType.RightUpDown:
                    m_ActualConnection = ConnectionType.LeftRightUpDown;
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.EnableKeyword("_Emission");
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.SetTexture("_Emission",m_EmissionLeftRightUpDown);
                    break;
                case ConnectionType.LeftUpDown:
                    break;
                case ConnectionType.LeftRightDown:
                    break;
                case ConnectionType.LeftRightUp:
                    break;
                case ConnectionType.UpDown:
                    m_ActualConnection = ConnectionType.LeftUpDown;
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.EnableKeyword("_Emission");
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.SetTexture("_Emission",m_EmissionLeftUpDown);
                    break;
            }       
            
        }
    }
    public void ActivateEmissionRight()
    {
        if(m_EmissionRight)
        {
            
            switch(m_ActualConnection)
            {
                case ConnectionType.None:
                    m_ActualConnection = ConnectionType.Right;
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.EnableKeyword("_Emission");
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.SetTexture("_Emission",m_EmissionRight);
                    break;
                case ConnectionType.Left:
                    m_ActualConnection = ConnectionType.LeftRight;
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.EnableKeyword("_Emission");
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.SetTexture("_Emission",m_EmissionLeftRight);
                    break;
                case ConnectionType.Right:
                    break;
                case ConnectionType.Up:
                    m_ActualConnection = ConnectionType.RightUp;
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.EnableKeyword("_Emission");
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.SetTexture("_Emission",m_EmissionRightUp);
                    break;
                case ConnectionType.Down:
                    m_ActualConnection = ConnectionType.RightDown;
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.EnableKeyword("_Emission");
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.SetTexture("_Emission",m_EmissionRightDown);
                    break;
                case ConnectionType.LeftRight:break;
                case ConnectionType.LeftUp:
                    m_ActualConnection = ConnectionType.LeftRightUp;
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.EnableKeyword("_Emission");
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.SetTexture("_Emission",m_EmissionLeftRightUp);
                    break;
                case ConnectionType.LeftDown:
                    m_ActualConnection = ConnectionType.LeftRightDown;
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.EnableKeyword("_Emission");
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.SetTexture("_Emission",m_EmissionLeftRightDown);
                    break;
                
                case ConnectionType.LeftRightUpDown:break;
                case ConnectionType.RightUp:
                    break;
                case ConnectionType.RightDown:
                    break;
                case ConnectionType.RightUpDown:
                    break;
                case ConnectionType.LeftUpDown:
                    m_ActualConnection = ConnectionType.LeftRightUpDown;
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.EnableKeyword("_Emission");
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.SetTexture("_Emission",m_EmissionLeftRightUpDown);
                    break;
                case ConnectionType.LeftRightDown:
                    break;
                case ConnectionType.LeftRightUp:
                    break;
                case ConnectionType.UpDown:
                    m_ActualConnection = ConnectionType.RightUpDown;
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.EnableKeyword("_Emission");
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.SetTexture("_Emission",m_EmissionRightUpDown);
                    break;
            }
        }
    }
    public void ActivateEmissionDown()
    {
        if(m_EmissionDown)
        {
            switch(m_ActualConnection)
            {
                case ConnectionType.None:
                    m_ActualConnection = ConnectionType.Down;
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.EnableKeyword("_Emission");
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.SetTexture("_Emission",m_EmissionDown);
                    break;
                case ConnectionType.Left:
                    m_ActualConnection = ConnectionType.LeftDown;
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.EnableKeyword("_Emission");
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.SetTexture("_Emission",m_EmissionLeftDown);
                    break;
                case ConnectionType.Right:
                    m_ActualConnection = ConnectionType.RightDown;
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.EnableKeyword("_Emission");
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.SetTexture("_Emission",m_EmissionRightDown);
                    break;
                case ConnectionType.Up:
                    m_ActualConnection = ConnectionType.UpDown;
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.EnableKeyword("_Emission");
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.SetTexture("_Emission",m_EmissionUpDown);
                    break;
                case ConnectionType.Down:
                    break;
                case ConnectionType.LeftRight:
                    m_ActualConnection = ConnectionType.LeftRightDown;
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.EnableKeyword("_Emission");
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.SetTexture("_Emission",m_EmissionLeftRightDown);
                    break;
                case ConnectionType.LeftUp:
                    m_ActualConnection = ConnectionType.LeftUpDown;
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.EnableKeyword("_Emission");
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.SetTexture("_Emission",m_EmissionLeftUpDown);
                    break;
                case ConnectionType.LeftDown:break;
                case ConnectionType.LeftRightUpDown:
                    break;
                case ConnectionType.LeftUpDown:
                    
                    break;
                case ConnectionType.LeftRightDown:
                    break;
                
                case ConnectionType.LeftRightUp:
                    m_ActualConnection = ConnectionType.LeftRightUpDown;
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.EnableKeyword("_Emission");
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.SetTexture("_Emission",m_EmissionLeftRightUpDown);
                    break;
                case ConnectionType.RightUp:
                    m_ActualConnection = ConnectionType.RightUpDown;
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.EnableKeyword("_Emission");
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.SetTexture("_Emission",m_EmissionRightUpDown);
                    break;
                case ConnectionType.RightDown:
                    break;
                    
                case ConnectionType.RightUpDown:
                    break;
                case ConnectionType.UpDown:
                    break;
            }
            
        }
    }
    public void ActivateEmissionUp()
    {
        if(m_EmissionUp)
        {
            switch(m_ActualConnection)
            {
                case ConnectionType.None:
                    m_ActualConnection = ConnectionType.Up;
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.EnableKeyword("_Emission");
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.SetTexture("_Emission",m_EmissionUp);
                    break;
                case ConnectionType.Left:
                    m_ActualConnection = ConnectionType.LeftUp;
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.EnableKeyword("_Emission");
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.SetTexture("_Emission",m_EmissionLeftUp);
                    break;
                case ConnectionType.Right:
                    m_ActualConnection = ConnectionType.RightUp;
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.EnableKeyword("_Emission");
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.SetTexture("_Emission",m_EmissionRightUp);
                    break;
                case ConnectionType.Up:
                    break;
                case ConnectionType.Down:
                    m_ActualConnection = ConnectionType.UpDown;
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.EnableKeyword("_Emission");
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.SetTexture("_Emission",m_EmissionUpDown);
                    break;
                case ConnectionType.LeftRight:
                    m_ActualConnection = ConnectionType.LeftRightUp;
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.EnableKeyword("_Emission");
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.SetTexture("_Emission",m_EmissionLeftRightUp);
                    break;
                case ConnectionType.LeftUp:
                    break;
                case ConnectionType.LeftDown:break;
                case ConnectionType.LeftRightUpDown:
                    break;
                case ConnectionType.LeftUpDown:
                   
                    break;
                case ConnectionType.LeftRightDown:
                m_ActualConnection = ConnectionType.LeftRightUpDown;
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.EnableKeyword("_Emission");
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.SetTexture("_Emission",m_EmissionLeftRightUpDown);
                    break;
                    break;
                case ConnectionType.LeftRightUp:
                    break;
                case ConnectionType.RightUp:
                    break;
                case ConnectionType.RightDown:
                    m_ActualConnection = ConnectionType.RightUpDown;
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.EnableKeyword("_Emission");
                    gameObject.GetComponentInChildren<SpriteRenderer>().material.SetTexture("_Emission",m_EmissionRightUpDown);
                    break;
                    
                case ConnectionType.RightUpDown:
                    break;
                case ConnectionType.UpDown:
                    break;
            }
            
        }
    }
    public void TurnOff()
    {
        m_ActualConnection = ConnectionType.None;
    }
    void Start()
    {
        m_ActualConnection = ConnectionType.None;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
