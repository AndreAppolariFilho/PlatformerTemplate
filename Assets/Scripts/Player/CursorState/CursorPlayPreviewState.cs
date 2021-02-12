using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CursorPlayPreviewState : CursorState
{
    public CursorPlayPreviewState(Cursor player, CursorStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
    string cancel_text = "";
    public override void Enter()
    {
        base.Enter();
        player.ActivateCursor();
        player.DeactivateCursorSelected();
        player.SetSize(1, 1);
        player.transform.position = player.position;
        player.gameManager.ActivatePlatforms();
        player.gameManager.ChangeAllToPreviewMode();
        cancel_text = player.buttonsObjects[0].GetComponentInChildren<TMP_Text>().text;
        player.buttonsObjects[0].GetComponentInChildren<TMP_Text>().text = "Stop";

    }
    public override void Exit()
    {

        player.gameManager.DeactivatePlatforms();
        player.gameManager.ResetPlatformsPositions();
        player.SetSize(1, 1);
        player.gameManager.ChangeAllToInGameMode();
        player.buttonsObjects[0].GetComponentInChildren<TMP_Text>().text = cancel_text;

    }
    public override void LogicUpdate()
    {
            
        if (player.InputHandler.CancelInput)
        {
            player.InputHandler.UseCancelInput();
            stateMachine.ChangeState(player.CursorNormalMovement);
        }
        int x = player.InputHandler.NormInputX;
        int y = player.InputHandler.NormInputY;
        player.transform.position += new Vector3(x * player.speed *Time.deltaTime, y * player.speed *Time.deltaTime, 0);
    }
}
