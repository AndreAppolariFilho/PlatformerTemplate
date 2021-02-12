using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarsManager : MonoBehaviour
{
    public GameObject[] m_starsInScene;
    public GameObject[] m_starsParents;
    public float m_percentageOfStarsAnimating = 0.3f;
    public float m_timeBetweenAnimations1 = 2;
    public float m_timeBetweenAnimations2 = 5;
    public float m_currentTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        int total_elements = 0;
        foreach(GameObject m_starsParent in m_starsParents)
        {
            total_elements += m_starsParent.GetComponentsInChildren<SpriteRenderer>().Length;
            
        }
        m_starsInScene = new GameObject[total_elements];
        int index = 0;
        foreach(GameObject m_starsParent in m_starsParents)
        {
            if(m_starsParent)
            {   
                SpriteRenderer [] sprites = m_starsParent.GetComponentsInChildren<SpriteRenderer>();
                for(int i = 0; i < sprites.Length; i++)
                {
                    
                    m_starsInScene[index] = sprites[i].gameObject;
                    index++;
                }
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
        int quantity =(int) (m_percentageOfStarsAnimating * m_starsInScene.Length);
        if(m_currentTime <= 0)
        {
            for(int i = 0; i < quantity; i++)
            {
                int random = Random.Range(0, m_starsInScene.Length);
                m_starsInScene[random].GetComponent<Animator>().SetTrigger("animate");
            }
            m_currentTime = Random.Range(m_timeBetweenAnimations1, m_timeBetweenAnimations2);
        }
        else
        {
                m_currentTime -= Time.deltaTime;
        }
    }
}
