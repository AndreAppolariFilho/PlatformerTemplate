using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorTerminalDeletingState : CursorTerminalState
{
    // Start is called before the first frame update
    public CursorTerminalDeletingState(CursorTerminal player, CursorTerminalStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        player.cursorInTerminalPosition = 0;
        player.SetPositionInTerminalPosition(0);
        player.terminal.ActivateDeletingInTerminalHud();
    }
    public override void Exit()
    {
        player.cursorInTerminalPosition = 0;
        player.terminal.DeactivateDeletingInTerminalHud();
        player.terminal.terminalController.ActivateScrollBar();
    }
    public override void LogicUpdate()
    {
        int inputX = player.InputHandler.NormInputY;
        bool cancel = player.InputHandler.CancelInput;
        bool confirm = player.InputHandler.JumpInput;
        player.SetPositionInTerminalPosition(inputX);
        if (confirm)
        {
            player.InputHandler.UseJumpInput();
            player.deleteButtonInTerminal(player.cursorInTerminalPosition);
            //player.SetButtonInTerminal(player.cursorInTerminalPosition, player.selectedButton);
            stateMachine.ChangeState(player.CursorSelectingState);
        }
        if (cancel)
        {
            player.InputHandler.UseCancelInput();
            stateMachine.ChangeState(player.CursorSelectingState);
        }

    }
}
