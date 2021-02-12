﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool playerEntered = false;
    public PlayerInputHandler inputHandler;
    GameManager gameManager;
    public Transform playerPosition;
    Player player;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player)
        {
            if(playerEntered)
            {
                if(player.canDisplayInteractPanel)
                { 
                    player.ActivatePanel();
                    if (inputHandler.InteractInput)
                    {
                        inputHandler.UseInteractInput();
                        if (gameManager.cameraPlayer)
                        {
                            //Debug.Log(gameManager.cameraPlayer.name);
                        }
                        playerEntered = false;
                        player.InitCutscene();

                    }
                }
                else
                {
                    player.DeactivatePanel();
                }
            }
            else
            {
                player.DeactivatePanel();
            }
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.GetComponent<Player>();
            playerEntered = true;
            if(player.canDisplayInteractPanel)
            { 
                player.ActivatePanel();
                if (inputHandler.InteractInput)
                {
                    inputHandler.UseInteractInput();
                    if (gameManager.cameraPlayer)
                    {
                        //Debug.Log(gameManager.cameraPlayer.name);
                    }
                    player.InitCutscene();

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
            playerEntered = false;
            player = null;
            collision.GetComponent<Player>().DeactivatePanel();
        }
    }
}