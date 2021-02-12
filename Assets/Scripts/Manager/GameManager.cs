using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
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
        TerminalMode,
        PauseMode
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
    public GameObject postProcessingEffect;
    public Button[] allowedButtons;
    #endregion
    #region CameraEffects
    public Animator CameraAnim;
    public GameObject blackScreen;
    public GameObject screenEffect;
    #endregion
    #region PauseState
    public GameObject pauseScreen;
    #endregion
    #region ScalableObjects
    public SpriteRenderer levelBg;
    public GameObject parent;
    public GameObject[] resizableObjects;
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
        //ScaleObjects();
        CheckRespawn();
        currentTimeStateChanged -= Time.deltaTime;
        
        foreach (ProcesserInput platform in platforms)
        {
                platform.Execute();
        }
        //if(AllPlatformsFinishedAction())
        //{
        //    FinishAllPlatformsAction();
        //}
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
    public void SwitchToCursorCamera()
    {
        if(currentState == GameState.CursorMode)
        {
            cameraPlayer.SetActive(false);
            cameraCursor.SetActive(true);
            cursor.gameObject.SetActive(true);
            screenEffect.SetActive(true);
            //postProcessingEffect.SetActive(true);
            cursorHUD.SetActive(true);
            terminalHUD.SetActive(false);
        }
        if(currentState == GameState.PlayerMode)
        {
            cameraPlayer.SetActive(true);
            cameraCursor.SetActive(false);
            cursor.gameObject.SetActive(false);
            //postProcessingEffect.SetActive(false);
            screenEffect.SetActive(false);
            cursorHUD.SetActive(false);
            terminalHUD.SetActive(false);
        }
    }
    public void ChangeState(GameState state)
    {
        if(currentTimeStateChanged <= 0)
        {
            //Debug.Log(currentState + " " + state);
        }
        if(state == GameState.CursorMode )
        {
            //CameraAnim.Play("camera_glitch");
            if(currentState != GameState.TerminalMode)
            {
                cameraPlayer.SetActive(false);
                cameraCursor.SetActive(true);
                CameraAnim.SetTrigger("Glitch");
            }
            else
            {
                currentState = GameState.CursorMode;
                SwitchToCursorCamera();
            }
            currentTimeStateChanged = timeBetweenChangeStates;
            currentState = GameState.CursorMode;
            
        }
        else if(state == GameState.PlayerMode)
        {   
            currentTimeStateChanged = timeBetweenChangeStates;
            
            if(currentState == GameState.CursorMode)
            {
                currentState = GameState.PlayerMode;
                SwitchToCursorCamera();
                CameraAnim.SetTrigger("Glitch");
            }
            else{
                currentState = GameState.PlayerMode;
                SwitchToCursorCamera();
            }
            currentState = GameState.PlayerMode;
            
        }
        else if(state == GameState.TerminalMode)
        {   
            currentTimeStateChanged = timeBetweenChangeStates;
            //postProcessingEffect.SetActive(false);
            cameraPlayer.SetActive(false);
            cameraCursor.SetActive(true);
            currentState = GameState.TerminalMode;
            cursorHUD.SetActive(false);
            screenEffect.SetActive(false);
            terminalHUD.SetActive(true);
        }
        else if(state == GameState.PauseMode)
        {
            currentTimeStateChanged = timeBetweenChangeStates;
            //postProcessingEffect.SetActive(false);
            cameraPlayer.SetActive(false);
            cameraCursor.SetActive(false);
            currentState = GameState.PauseMode;
            cursorHUD.SetActive(false);
            screenEffect.SetActive(false);
            terminalHUD.SetActive(false);
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
    public bool AllPlatformsCanDoAction()
    {
        foreach(ProcesserInput platform in platforms)
        {
            if(!platform.canExecute)
            {
                return false;
            }
        }
        return true;
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
    public bool IsValid(ProcesserInput pI, Waypoint startPoint, Waypoint endPoint)
    {
        foreach(ProcesserInput platform in platforms)
        {
            
            if(platform != pI && platform.m_currWaypoint && platform.m_objectiveWaypoint)
            { 
                if (platform.m_currWaypoint.transform.position == endPoint.transform.position && platform.m_objectiveWaypoint.transform.position == startPoint.transform.position)
                {
                    return false;
                }
                if (platform.m_currWaypoint.transform.position == endPoint.transform.position && platform.word.Length == 0)
                {
                    return false;
                }
                if(platform.m_currWaypoint.transform.position == endPoint.transform.position && platform.HasError())
                {
                    return false;
                }
                if(platform.m_objectiveWaypoint.transform.position == endPoint.transform.position)
                {
                    return false;
                }
            }
            else
            {
                if (platform != pI && platform.m_currWaypoint.transform.position == endPoint.transform.position)
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
    public bool AllPlatformsInAction()
    {
        foreach (ProcesserInput platform in platforms)
        {
            if(!platform.HasError())
            {
                if(!platform.inAction && platform.cpu < platform.word.Length)
                {
                    return false;
                }
            }
        }
        return true;
    }
    public bool AllPlatformsFinishedAction()
    {
        foreach (ProcesserInput platform in platforms)
        {
            if (!platform.HasError())
            {
                if (!platform.turnFinished )
                {
                    //Debug.Log("Não finalizado "+platform.gameObject.name);
                    allFinished = false;
                    return false;
                }
            }
        }
        allFinished = true;
        return true;
    }
    IEnumerator highlightBtn()
     {
         EventSystem myEventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
         myEventSystem.SetSelectedGameObject(null);
         yield return null;
         myEventSystem.SetSelectedGameObject(myEventSystem.firstSelectedGameObject);
     }
    public void PauseGame()
    {
        if(currentState != GameState.PauseMode)
        {
            Time.timeScale = 0;
            StartCoroutine("highlightBtn");
            pauseScreen.SetActive(true);
            ChangeState(GameState.PauseMode);
        }
    }
    IEnumerator ChangeToPlayerMode()
    {
        yield return new WaitForSeconds(0.1f);
        ChangeState(GameState.PlayerMode);
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseScreen.SetActive(false);
        StartCoroutine("ChangeToPlayerMode");
    }
    public void FinishAllPlatformsAction()
    {
        foreach (ProcesserInput platform in platforms)
        {
            //if(!platform.HasError())
            platform.FinishAction();
        }
        
    }
    public void ScaleObjects()
    {
        levelBg.gameObject.transform.localScale = new Vector3(1,1,1);
     
        var width = levelBg.sprite.bounds.size.x;
        var height = levelBg.sprite.bounds.size.y;

        var worldScreenHeight = Camera.main.orthographicSize * 2.0;
        var worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        levelBg.gameObject.transform.parent = this.parent.transform;
        foreach(GameObject o in resizableObjects)
        {
            o.transform.parent = parent.transform;
        }
        parent.transform.localScale = new Vector3((float)worldScreenWidth / width,(float)worldScreenHeight / height, 0);
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
