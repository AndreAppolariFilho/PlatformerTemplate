using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformStoppedState : PlatformState
{
    public PlatformStoppedState(ProcesserInput player, PlatformStateMachine stateMachine): base(player, stateMachine)
    {
        
    }
    public override void Enter() 
    {
        player.m_firstWaypoint.gameObject.GetComponentInChildren<SpriteRenderer>().material.SetColor("_Color",player.neutralColor);
        if(player.m_firstWaypoint)
            player.m_firstWaypoint.TurnOff();
        if(player.m_currWaypoint)
            player.m_currWaypoint.TurnOff();
        if(player.m_objectiveWaypoint)
            player.m_objectiveWaypoint.TurnOff();
        if(player.processerMode == ProcesserInput.ProcesserMode.InGameMode)
        {
            player.m_firstWaypoint = player.m_currWaypoint;
        }
        if(player.actualTrailLight)
            player.actualTrailLight.color = player.neutralColor;
        if(player.m_firstWaypoint)
            player.m_firstWaypoint.gameObject.GetComponentInChildren<SpriteRenderer>().material.SetColor("_Color",player.neutralColor);
        if(player.m_currWaypoint)
            player.m_currWaypoint.gameObject.GetComponentInChildren<SpriteRenderer>().material.SetColor("_Color",player.neutralColor);
        if(player.m_objectiveWaypoint)
            player.m_objectiveWaypoint.gameObject.GetComponentInChildren<SpriteRenderer>().material.SetColor("_Color",player.neutralColor);
        if(player.actualTrail)
        {
            player.actualTrail.GetComponent<SpriteRenderer>().material.SetColor("_Color",player.neutralColor);
            player.spriteR.material.SetColor("_Color", player.neutralColor);
            player.actualTrail = null;
        }
        
        
    }
    public override void Exit()
    {
        
    }
    public override void LogicUpdate()
    {
        if(player.currentTime > 0)
        {
            player.currentTime -= Time.deltaTime;
            if(player.currentTime <= 0)
            {
                player.currentTime = 0;
                
                player.turnFinished = true;
                
                player.m_currWaypoint.gameObject.GetComponentInChildren<SpriteRenderer>().material.SetColor("_Color",player.neutralColor);
                
                player.m_currWaypoint = player.m_objectiveWaypoint;

                player.canExecute = true;

                player.m_objectiveWaypoint = null;
                
            }
        }
        if(player.gameManager.AllPlatformsCanDoAction())
        {
            if (player.cpu == player.word.Length)
            {
                if(player.processerMode == ProcesserInput.ProcesserMode.InGameMode)
                {
                    player.word = "";
                    player.cpu = 0;
                    stateMachine.ChangeState(player.m_ProcessWord);
                }
            }
            else{
                stateMachine.ChangeState(player.m_ProcessWord);
            }
        }
    }
}
