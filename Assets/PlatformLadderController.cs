using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformLadderController : MonoBehaviour
{
    // Start is called before the first frame update
    PlatformEffector2D effector2D;
    Collider2D m_collider;
    [SerializeField] Player player;
    void Start()
    {
        effector2D = GetComponent<PlatformEffector2D>();
        m_collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapAreaAll(m_collider.bounds.min, m_collider.bounds.max);
        bool touchingPlayer = false;
        foreach(Collider2D collider in colliders)
        {
            if(collider.gameObject.CompareTag("Player"))
            {
                touchingPlayer = true;
            }
        }
        if(!touchingPlayer)
        {
            effector2D.rotationalOffset = 0; 
        }
    }
}
