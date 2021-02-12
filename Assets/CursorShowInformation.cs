using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorShowInformation : CursorTerminalState
{
    public CursorShowInformation(CursorTerminal player, CursorTerminalStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        player.terminal.tutorialPanel.functionName.text = player.selectedButton.funtionName;
        player.terminal.tutorialPanel.text.text = player.selectedButton.description;
        player.terminal.tutorialPanel.gameObject.SetActive(true);
    }

    public override void Exit()
    {
        player.terminal.tutorialPanel.gameObject.SetActive(false);
    }

    public override void LogicUpdate()
    {
        bool confirm = player.InputHandler.JumpInput;
        
        if (confirm)
        {
            player.InputHandler.UseJumpInput();
            stateMachine.ChangeState(player.CursorSelectingState);
            
        }
    }
    
}
