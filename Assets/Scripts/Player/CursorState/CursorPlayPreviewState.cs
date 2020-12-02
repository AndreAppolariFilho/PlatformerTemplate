using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorPlayPreviewState : CursorState
{
    public CursorPlayPreviewState(Cursor player, CursorStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        player.SetSize(player.bounds.x, player.bounds.y);
        player.transform.position = player.position;
        player.gameManager.ActivatePlatforms();
        player.gameManager.ChangeAllToPreviewMode();

    }
    public override void Exit()
    {

        player.gameManager.DeactivatePlatforms();
        player.gameManager.ResetPlatformsPositions();
        player.SetSize(1, 1);
        player.gameManager.ChangeAllToInGameMode();

    }
    public override void LogicUpdate()
    {

        if (player.InputHandler.CancelInput)
        {
            player.InputHandler.UseCancelInput();
            stateMachine.ChangeState(player.CursorNormalMovement);
        }
    }
}
