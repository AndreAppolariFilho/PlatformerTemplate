using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformExecuteState : PlatformState
{
   public PlatformExecuteState(ProcesserInput player, PlatformStateMachine stateMachine): base(player, stateMachine)
    {
       
    }
    public override void Enter()
    {
        player.light.color = player.acceptedColor;
        player.spriteR.material.SetColor("_Color", player.acceptedColor);
        if(player.actualTrail)
        {
            player.m_currWaypoint.gameObject.GetComponentInChildren<SpriteRenderer>().material.SetColor("_Color",player.acceptedColor);
            player.m_objectiveWaypoint.gameObject.GetComponentInChildren<SpriteRenderer>().material.SetColor("_Color",player.acceptedColor);
            player.actualTrail.GetComponent<SpriteRenderer>().material.SetColor("_Color",player.acceptedColor);
            player.actualTrailLight.color = player.acceptedColor;
        }
        
    }
    public override void Exit()
    {
        
    }
    public override void LogicUpdate()
    {
        
        if( player.gameManager.AllPlatformSelectedMovement())
        { 
        switch (player.type)
            {
                case LanguageEnums.Types.downArrow:
                    //if(!withError)
                    {
                        //withError = !gameManager.IsValid(this, m_currWaypoint, m_objectiveWaypoint);
                    }
                    player.canExecute = false;
                    if(player.gameManager.IsValid(player, player.m_currWaypoint, player.m_objectiveWaypoint))
                    { 
                        if(player.currentTime  == 0)
                        player.transform.position = new Vector2(player.transform.position.x, player.transform.position.y - player.speed * Time.deltaTime);
                    }
                    else
                    {
                        player.currentTime = 1f;
                        player.canExecute = false;
                        stateMachine.ChangeState(player.m_ErrorState);
                        return;
                    }
                    break;
                case LanguageEnums.Types.upArrow:
                    player.canExecute = false;
                    if (player.gameManager.IsValid(player, player.m_currWaypoint, player.m_objectiveWaypoint))
                    {
                        if(player.currentTime  == 0)
                        player.transform.position = new Vector2(player.transform.position.x, player.transform.position.y + player.speed * Time.deltaTime);
                    }
                    else
                    {
                        player.currentTime = 1f;
                        player.canExecute = false;
                        stateMachine.ChangeState(player.m_ErrorState);
                        return;
                    }
                    break;
                case LanguageEnums.Types.leftArrow:
                    player.canExecute = false;
                    if (player.gameManager.IsValid(player, player.m_currWaypoint, player.m_objectiveWaypoint))
                    {
                        //if(player.currentTime  == 0)                        
                        player.transform.position = new Vector2(player.transform.position.x - player.speed * Time.deltaTime, player.transform.position.y);
                    }
                    else
                    {
                        player.currentTime = 1f;
                        player.canExecute = false;
                        stateMachine.ChangeState(player.m_ErrorState);
                        return;
                    }
                    break;
                case LanguageEnums.Types.rightArrow:
                    player.canExecute = false;
                    if (player.gameManager.IsValid(player, player.m_currWaypoint, player.m_objectiveWaypoint))
                    {
                        if(player.currentTime  == 0)
                            player.transform.position = new Vector2(player.transform.position.x + player.speed * Time.deltaTime, player.transform.position.y);
                    }
                    else
                    {
                        player.currentTime = 1f;
                        player.canExecute = false;
                        stateMachine.ChangeState(player.m_ErrorState);
                        return;
                    }
                    break;
                    
                }
                switch (player.type)
                {
                    case LanguageEnums.Types.turnClockWise:
                        //palyer.transform.Rotate(Vector3.forward * angularSpeed * Time.deltaTime);
                        break;
                    case LanguageEnums.Types.turnAntiClockWise:
                        //this.transform.Rotate(-1 * Vector3.forward * angularSpeed * Time.deltaTime);
                        break;
                }
                
                if(Vector2.Distance(player.transform.position, player.m_objectiveWaypoint.transform.position) < 0.2)
                {
                    player.transform.position = player.m_objectiveWaypoint.transform.position;
                    
                    if(player.currentTime == 0)
                        player.currentTime = 0.5f;
                    
                    stateMachine.ChangeState(player.m_StoppedState);
                
                }
                //if(!withError)
                //{ 
                //    if (inRotation)
                //    {   
                //        if(Mathf.Abs(Mathf.DeltaAngle(endAngle,this.transform.eulerAngles.z)) < 1.5f)
                //        {
                //            this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, this.transform.eulerAngles.y, endAngle);
                //            beginAngle = endAngle;
                //            if (currentTime == 0)
                //                currentTime = 0.5f;
                //        }
                //    }
                //    else {
                //        Debug.Log(Vector2.Distance(transform.position, m_objectiveWaypoint.transform.position));
                //        Debug.Log(m_currWaypoint.transform.position);
                //    
                //        if(Vector2.Distance(transform.position, m_objectiveWaypoint.transform.position) < 0.2)
                //        {
                //            //Debug.LogError(transform.position + " " + m_objectiveWaypoint.gameObject.name + " " + m_objectiveWaypoint.gameObject.transform.position + " " + m_currWaypoint.transform.position);
                //            this.transform.position = m_objectiveWaypoint.transform.position;
                //            
                //            
                //            if(currentTime == 0)
                //                currentTime = 0.5f;
                //        
                //        }
                //    }
                //}
            }
            
    }
}
