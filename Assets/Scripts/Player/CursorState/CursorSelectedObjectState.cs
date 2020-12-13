using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CursorSelectedObjectState : CursorState
{
    // Start is called before the first frame update

    public float currentTime = 0;
    public float waitTime = 0.5f;
    public CursorSelectedObjectState(Cursor player, CursorStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        player.SetSize(player.bounds.x, player.bounds.y);
        player.transform.position = player.position;
        currentTime = waitTime;
        player.buttonsObjects[1].GetComponentInChildren<Image>().sprite = player.inputsButtons[1].image;
        player.buttonsObjects[1].GetComponentInChildren<TMP_Text>().text = player.inputsButtons[1].name;
        player.buttonsObjects[1].SetActive(true);
        if (player.gameManager.HasPlatformWithCommand())
        {
            player.buttonsObjects[2].GetComponentInChildren<Image>().sprite = player.inputsButtons[2].image;
            player.buttonsObjects[2].GetComponentInChildren<TMP_Text>().text = player.inputsButtons[2].name;
            player.buttonsObjects[2].SetActive(true);

        }


    }
    public override void Exit()
    {

        player.buttonsObjects[1].SetActive(false);
        if (player.gameManager.HasPlatformWithCommand())
        {

            player.buttonsObjects[1].GetComponentInChildren<Image>().sprite = player.inputsButtons[2].image;
            player.buttonsObjects[1].GetComponentInChildren<TMP_Text>().text = player.inputsButtons[2].name;
            player.buttonsObjects[1].SetActive(true);
            
            player.buttonsObjects[2].SetActive(false);

        }
        player.SetSize(1, 1);
        
    }
    public override void LogicUpdate()
    {
        if (currentTime <= 0)
        {
            int x = player.InputHandler.NormInputX;
            int y = player.InputHandler.NormInputY;
            bool selected = player.InputHandler.JumpInput;

            if (x != 0 || y != 0)
            {
                player.transform.position = new Vector2(player.middlePoint.transform.position.x + x * ((player.bounds.x / 2) + (player.upLeft.transform.localScale.x / 2)), player.middlePoint.transform.position.y + y * ((player.bounds.y / 2) + (player.upLeft.transform.localScale.y / 2)));
                stateMachine.ChangeState(player.CursorNormalMovement);
            }
            if(selected)
            {
                
                player.InputHandler.UseJumpInput();
                stateMachine.ChangeState(player.CursorNormalMovement);
                Button[] buttons = player.SelectedPlatform.GetComponent<ProcesserInput>().allowedButtons;
                GameObject.FindObjectOfType<GameManager>().ChangeState(GameManager.GameState.TerminalMode);
                player.terminal.SetQuantityOfButtonToChoice(buttons.Length);
                
                foreach(ProcesserInput.Connection c in player.SelectedPlatform.GetComponent<ProcesserInput>().connections)
                {
                    if(c.up)
                    {
                        player.terminal.SetConnection(c.button, c.up, "up");
                    }
                    if(c.left)
                    {
                        player.terminal.SetConnection(c.button, c.left, "left");
                    }
                    if(c.down)
                    {
                        player.terminal.SetConnection(c.button, c.down, "down");
                    }
                    if(c.right)
                    {
                        player.terminal.SetConnection(c.button, c.right, "right");
                    }
                }
                Debug.LogError("Quantidade de botões "+buttons.Length);
                for (int i = 0; i < buttons.Length; i++)
                { 
                    player.terminal.SetButtonToChoice(i, buttons[i]);
                }
                player.terminal.SetActualPlatform(player.SelectedPlatform.GetComponent<ProcesserInput>());
                Button[] buttonsInTerminal = player.SelectedPlatform.GetComponent<ProcesserInput>().buttonsInTerminal;
                /*
                for(int i = 0; i < player.terminal.buttonsInTerminal.Length;i++)
                {
                    player.terminal.DeleteButtonInTerminal(i);
                    player.terminal.postionsInTerminal[i].SetActive(false);
                }
                */
                for (int i = 0; i < buttonsInTerminal.Length; i++)
                {
                    //player.terminal.postionsInTerminal[i].GetComponent<Image>().sprite = null;
                    if (buttonsInTerminal[i]){ 
                        //Color c = player.terminal.postionsInTerminal[i].GetComponent<Image>().color;
                        //c.a = 1;
                        //player.terminal.postionsInTerminal[i].GetComponent<Image>().color = c;
                        player.terminal.postionsInTerminal[i].SetActive(true);
                        Debug.Log(buttonsInTerminal[i]);
                    
                        //player.terminal.postionsInTerminal[i].GetComponent<Image>().sprite = buttonsInTerminal[i].buttonImage;
                    }
                    player.terminal.buttonsInTerminal[i] = buttonsInTerminal[i];
                }
            }
        }
        else
        {
            currentTime -= Time.deltaTime;
        }
        if (player.InputHandler.ActionInput)
        {
            player.InputHandler.UseActionInput();
            stateMachine.ChangeState(player.CursorPlayPreview);
        }
        if (player.InputHandler.CancelInput)
        {
            player.InputHandler.UseCancelInput();
            Debug.Log("To Player Mode");
            GameObject.FindObjectOfType<GameManager>().ChangeState(GameManager.GameState.PlayerMode);
            GameObject.FindObjectOfType<GameManager>().ActivatePlatforms();
        }

    }
}
