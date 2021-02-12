using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeGame : MonoBehaviour
{
    // Start is called before the first frame update
    GameManager gameManager;
    
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    private void OnDisable()
    {
        if(FindObjectOfType<GameManager>())
            gameManager.ResumeGame();
    }
    
}
