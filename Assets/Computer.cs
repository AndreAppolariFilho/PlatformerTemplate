using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour
{
    // Start is called before the first frame update
    public bool playerEntered = false;
    public PlayerInputHandler inputHandler;
    GameManager gameManager;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if(player.canDisplayInteractPanel)
            { 
                player.ActivatePanel();
                if (inputHandler.InteractInput)
                {
                    inputHandler.UseInteractInput();
                    if (gameManager.cameraPlayer)
                    {
                        Debug.Log(gameManager.cameraPlayer.name);
                    }
                    gameManager.ChangeState(GameManager.GameState.CursorMode);

                }
            }
            else
            {
                player.DeactivatePanel();
            }
        }
    }
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player.canDisplayInteractPanel)
            {
                player.ActivatePanel();
                if (inputHandler.InteractInput)
                {
                    inputHandler.UseInteractInput();
                    if (gameManager.cameraPlayer)
                    {
                        Debug.Log(gameManager.cameraPlayer.name);
                    }
                    gameManager.ChangeState(GameManager.GameState.CursorMode);

                }
            }
            else
            {
                player.DeactivatePanel();
            }
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().DeactivatePanel();
        }
    }
}
