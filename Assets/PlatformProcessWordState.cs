using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformProcessWordState : PlatformState
{
    public PlatformProcessWordState(ProcesserInput player, PlatformStateMachine stateMachine): base(player, stateMachine)
    {
        this.player = player;
        this.stateMachine = stateMachine;
    }
    public override void Enter()
    {

    }
    public override void Exit()
    {

    }
    public override void LogicUpdate()
    {

        if(player.isActive && player.word.Length > 0)
        {
            player.ProcessInput();
            switch(player.type)
            {
                case LanguageEnums.Types.downArrow:
                    player.selectedMoviment = true;
                    foreach(Button button in player.gameManager.allowedButtons)
                    {
                        if(button.buttonValue == "D")
                        {
                            player.lcdDisplay.sprite = button.lcdDisplayImage;
                            break;
                        }
                    }
                    if(player.m_currWaypoint.down)
                    {
                        player.m_objectiveWaypoint = player.m_currWaypoint.down;
                        player.actualTrail = player.m_currWaypoint.trail_down;
                        player.actualTrailLight = player.m_currWaypoint.light_down;
                        player.m_currWaypoint.ActivateEmissionDown();
                        player.m_objectiveWaypoint.ActivateEmissionUp();
                        stateMachine.ChangeState(player.m_ExecuteState);
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
                    player.selectedMoviment = true;
                    foreach(Button button in player.gameManager.allowedButtons)
                    {
                        if(button.buttonValue == "U")
                        {
                            player.lcdDisplay.sprite = button.lcdDisplayImage;
                            break;
                        }
                    }
                    if (player.m_currWaypoint.up)
                    {
                        player.m_objectiveWaypoint = player.m_currWaypoint.up;
                        player.actualTrail = player.m_currWaypoint.trail_up;
                        player.actualTrailLight = player.m_currWaypoint.light_up;
                        player.m_currWaypoint.ActivateEmissionUp();
                        player.m_objectiveWaypoint.ActivateEmissionDown();
                        stateMachine.ChangeState(player.m_ExecuteState);
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
                    player.selectedMoviment = true;
                    foreach(Button button in player.gameManager.allowedButtons)
                    { 
                        if(button.buttonValue == "L")
                        {
                            player.lcdDisplay.sprite = button.lcdDisplayImage;
                            break;
                        }
                    }
                    if (player.m_currWaypoint.left)
                    {
                        player.m_objectiveWaypoint = player.m_currWaypoint.left;
                        player.actualTrail = player.m_currWaypoint.trail_left;
                        player.actualTrailLight = player.m_currWaypoint.light_left;
                        player.m_currWaypoint.ActivateEmissionLeft();
                        player.m_objectiveWaypoint.ActivateEmissionRight();
                        stateMachine.ChangeState(player.m_ExecuteState);
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
                    player.selectedMoviment = true;
                    foreach(Button button in player.gameManager.allowedButtons)
                    {
                        if(button.buttonValue == "R")
                        {
                            player.lcdDisplay.sprite = button.lcdDisplayImage;
                            break;
                        }
                    }
                    if (player.m_currWaypoint.right)
                    {
                        player.m_objectiveWaypoint = player.m_currWaypoint.right;
                        player.actualTrail = player.m_currWaypoint.trail_right;
                        player.actualTrailLight = player.m_currWaypoint.light_right;
                        player.m_currWaypoint.ActivateEmissionRight();
                        player.m_objectiveWaypoint.ActivateEmissionLeft();
                        stateMachine.ChangeState(player.m_ExecuteState);
                    }
                    else
                    {
                        player.currentTime = 1f;
                        player.canExecute = false;
                        stateMachine.ChangeState(player.m_ErrorState);
                        return;
                    }
                    break;
                case LanguageEnums.Types.turnClockWise:
                    //inAction = true;
                    //inRotation = true;
                    //endAngle = (beginAngle + 90) % 360;
                    //m_objectiveWaypoint = m_currWaypoint;
                    //turnFinished = false;
                    //actualTrail = null;
                    break;
                case LanguageEnums.Types.turnAntiClockWise:
                    //inAction = true;
                    //inRotation = true;
                    //endAngle = (beginAngle - 90)%360;
                    //actualTrail = null;
                    //m_objectiveWaypoint = m_currWaypoint;
                    //turnFinished = false;
                    break;
            }
        }
    }
}
