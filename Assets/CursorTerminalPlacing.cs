using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorTerminalPlacing : CursorTerminalState
{
    public CursorTerminalPlacing(CursorTerminal player, CursorTerminalStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    // Start is called before the first frame update
    public override void Enter()
    {
        player.cursorInTerminalPosition = player.terminal.GetPositionsInTerminal().Count - 1;
        player.SetPositionInTerminalPosition(0);
        player.terminal.ActivateSelectingInTerminalHud();
        player.terminal.terminalController.ActivateScrollBar();
    }
    public override void Exit()
    {
        player.cursorInTerminalPosition = 0;
        player.cursorInSelectPosition = 0;
        player.terminal.DeactivateSelectingInTerminalHud();
    }
    public override void LogicUpdate()
    {
        int inputX = player.InputHandler.NormInputY;
        bool cancel = player.InputHandler.CancelInput;
        bool confirm = player.InputHandler.JumpInput;
        player.SetPositionInTerminalPosition(inputX);
        if(confirm)
        {
            player.InputHandler.UseJumpInput();
            player.SetButtonInTerminal(player.cursorInTerminalPosition, player.selectedButton);
            stateMachine.ChangeState(player.CursorSelectingState);
        }
        if(cancel)
        {
            player.InputHandler.UseCancelInput();
            stateMachine.ChangeState(player.CursorSelectingState);
        }

    }
}
