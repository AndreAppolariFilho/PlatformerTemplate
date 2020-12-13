using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorTerminalStopped : CursorTerminalState
{
    public CursorTerminalStopped(CursorTerminal player, CursorTerminalStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    
    public override void Enter()
    {
        //if (GameObject.FindObjectOfType<GameManager>().currentState == GameManager.GameState.TerminalMode)
        //    GameObject.FindObjectOfType<GameManager>().ChangeState(GameManager.GameState.CursorMode);
        player.terminal.ActivateObjectButtonsHud();
        player.terminal.terminalController.DeactivateScrollBar();

    }
    public override void Exit()
    {
        player.terminal.DeactivateObjectButtonsHud();
    }
    public override void LogicUpdate()
    {
        int inputX = player.InputHandler.NormInputX;
        int inputY = player.InputHandler.NormInputY;
        bool cancel = player.InputHandler.CancelInput;
        bool confirm = player.InputHandler.JumpInput;
        bool showInfo = player.InputHandler.InteractInput;
        if (inputX != 0 || inputY != 0)
        {
            stateMachine.ChangeState(player.CursorSelectingState);
        }
        if(cancel)
        {
            player.cursorInTerminalPosition = 0;
            player.InputHandler.UseCancelInput();
            GameObject.FindObjectOfType<GameManager>().ChangeState(GameManager.GameState.CursorMode);
        }
        if(confirm)
        {
            player.InputHandler.UseJumpInput();
            player.SelectButton(player.terminal.buttonsToChoice[player.cursorInSelectPosition]);
            stateMachine.ChangeState(player.CursorPlacingState);
        }
        if (showInfo)
        {
            player.InputHandler.UseInteractInput();
            player.SelectButton(player.terminal.buttonsToChoice[player.cursorInSelectPosition]);
            stateMachine.ChangeState(player.CursorShowInfo);
        }

    }
}
