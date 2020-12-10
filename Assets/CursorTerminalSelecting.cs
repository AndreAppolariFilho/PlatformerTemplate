using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorTerminalSelecting : CursorTerminalState
{
    public CursorTerminalSelecting(CursorTerminal player, CursorTerminalStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    // Start is called before the first frame update

    public override void Enter()
    {
        player.DiselectButton();
        player.terminal.ActivateObjectButtonsHud();
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
        player.SetPositionInSelectPosition(inputX, inputY);
        if(cancel)
        {
            player.InputHandler.UseCancelInput();
            player.cursorInTerminalPosition = 0;
            stateMachine.ChangeState(player.CursorStoppedState);
            GameObject.FindObjectOfType<GameManager>().ChangeState(GameManager.GameState.CursorMode);
        }
        if(confirm)
        {
            player.InputHandler.UseJumpInput();
            if(player.terminal.positionsToChoice[player.actualButtonInSelectPosition].CompareTag("DeleteButton"))
            {
                stateMachine.ChangeState(player.CursorTerminalDeleting);
            }
            else { 
                player.SelectButton(player.actualButtonInSelectPosition);
                stateMachine.ChangeState(player.CursorPlacingState);
            }
        }
        if(showInfo)
        {
            player.InputHandler.UseInteractInput();
            if (player.terminal.positionsToChoice[player.actualButtonInSelectPosition].CompareTag("DeleteButton"))
            {
                player.SelectButton(player.terminal.deleteButton);
            }
            else { 
                player.SelectButton(player.actualButtonInSelectPosition);
            }
            stateMachine.ChangeState(player.CursorShowInfo);
        }

    }
}
