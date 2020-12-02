using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : Manager<GameManager>
{
    [SerializeField]
    private Transform respawnPoint;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private float respawnTime;

    private float respawnTimeStart;

    private bool respawn;
    public bool allFinished = false;
    public bool allSelected = false;
    
    private CinemachineVirtualCamera CVC;
    #region GameState
    public enum GameState
    {
        PlayerMode,
        CursorMode,
        TerminalMode
    }
    public GameState currentState;
    public float timeBetweenChangeStates = 0.2f;
    public float currentTimeStateChanged;
    #endregion
    #region Cameras
    public GameObject cameraPlayer;
    public GameObject cameraCursor;
    #endregion
    #region Player
    //public Player player;
    #endregion
    #region HUD
    public GameObject cursorHUD;
    public GameObject terminalHUD;
    public Cursor cursor;
    #endregion
    #region MovingPlatforms
    public ProcesserInput[] platforms;
    #endregion
    private void Awake()
    {
        currentState = GameState.PlayerMode;
        cursor.gameObject.SetActive(false);
    }
    private void Start()
    {
        //CVC = GameObject.Find("Player Camera").GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        //Debug.Log(Time.time+" "+cameraPlayer);
        CheckRespawn();
        currentTimeStateChanged -= Time.deltaTime;
    }
    public void Respawn()
    {
        respawnTimeStart = Time.time;
        respawn = true;
    }
    public void ChangeAllToInGameMode()
    {
        foreach(ProcesserInput p in platforms)
            p.ChangeToInGameMode();
    }
    public void ChangeAllToPreviewMode()
    {
        foreach (ProcesserInput p in platforms)
            p.ChangeToPreviewMode();
    }
    public void ChangeState(GameState state)
    {
        if(currentTimeStateChanged <= 0)
        {
            Debug.Log(currentState + " " + state);
        }
        if(state == GameState.CursorMode )
        {
            cursor.gameObject.SetActive(true);
            
            currentTimeStateChanged = timeBetweenChangeStates;
            Debug.Log(cameraPlayer);
            cameraPlayer.SetActive(false);
            cameraCursor.SetActive(true);
            currentState = GameState.CursorMode;
            cursorHUD.SetActive(true);
            terminalHUD.SetActive(false);
        }
        else if(state == GameState.PlayerMode)
        {   
            currentTimeStateChanged = timeBetweenChangeStates;
            cameraPlayer.SetActive(true);
            cameraCursor.SetActive(false);
            currentState = GameState.PlayerMode;
            cursorHUD.SetActive(false);
            terminalHUD.SetActive(false);
        }
        else if(state == GameState.TerminalMode)
        {   
            currentTimeStateChanged = timeBetweenChangeStates;
            cameraPlayer.SetActive(false);
            cameraCursor.SetActive(true);
            currentState = GameState.TerminalMode;
            cursorHUD.SetActive(false);
            terminalHUD.SetActive(true);
        }
    }
    public void ActivatePlatforms()
    {
        foreach (ProcesserInput platform in platforms)
        {
            platform.Activate();
        }
    }
    public void DeactivatePlatforms()
    {
        foreach (ProcesserInput platform in platforms)
        {
            platform.Deactivate();
        }
    }
    public void ResetPlatformsPositions()
    {
        foreach(ProcesserInput platform in platforms)
        {
            platform.ResetPosition();
        }
    }
    public bool AllPlatformSelectedMovement()
    {
        foreach(ProcesserInput platform in platforms)
        {
            if(((!platform.SelectedMovement() && !platform.finished)) && platform.word.Length > 0)
            {
                allSelected = false;
                return false;
            }
        }
        allSelected = true;
        return true;
    }
    public bool HasPlatformWithCommand()
    {
        foreach (ProcesserInput platform in platforms)
        {
            if (platform.word.Length > 0)
                return true;
        }
        return false;
    }
    public bool IsValid(Waypoint startPoint, Waypoint endPoint)
    {
        foreach(ProcesserInput platform in platforms)
        {
            
            if(platform.m_currWaypoint && platform.m_objectiveWaypoint)
            { 
                if (platform.m_currWaypoint.transform.position == endPoint.transform.position && platform.m_objectiveWaypoint.transform.position == startPoint.transform.position)
                {
                    return false;
                }
            }
            else
            {
                if (platform.m_currWaypoint.transform.position == endPoint.transform.position)
                {
                    return false;
                }
            }

        }
        return true;
    }
    public bool PlatformInAction()
    {
        foreach (ProcesserInput platform in platforms)
        {
            if(platform.inAction)
            {
                return true;
            }
        }
        return false;
    }
    public bool AllPlatformsFinishedAction()
    {
        foreach (ProcesserInput platform in platforms)
        {
            if (!platform.HasError())
            {
                if (!platform.turnFinished || !platform.finished)
                {
                    allFinished = false;
                    return false;
                }
            }
        }
        allFinished = true;
        return true;
    }
    private void CheckRespawn()
    {
        //if(Time.time >= respawnTimeStart + respawnTime && respawn)
        //{
        //    var playerTemp = Instantiate(player, respawnPoint);
        //    CVC.m_Follow = playerTemp.transform;
        //    respawn = false;
        //}
    }
}
