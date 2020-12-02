using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursosNormalMovementState : CursorState
{
    // Start is called before the first frame update
    public CursosNormalMovementState(Cursor player, CursorStateMachine stateMachine, string animBoolName):base(player,stateMachine, animBoolName)
    {   
    }
    public  override void  Enter()
    {
        base.Enter();
        player.SetSize(0, 0);
        player.gameManager.ResetPlatformsPositions();
        player.gameManager.DeactivatePlatforms();
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void LogicUpdate()
    {
        
        int x = player.InputHandler.NormInputX;
        int y = player.InputHandler.NormInputY;
        player.transform.position += new Vector3(x * player.speed *Time.deltaTime, y * player.speed *Time.deltaTime, 0);
        if(player.IsColliding())
        {
            stateMachine.ChangeState(player.CursorSelectedMovement);
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
