using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal; 


public class ProcesserInput : MonoBehaviour
{
    // Start is called before the first frame update
    public int cpu = 0;
    int beginForLoop = 0;
    int endForLoop = 0;
    int actualForLoop = 0;
    int totalForLoop = 0;
    public bool finished = false;    
    public Color neutralColor;
    public Color errorColor;
    public Color acceptedColor;
    public Light2D light;
    [Serializable]
    public class Connection
    {
        public Button button;
        public Button up;
        public Button down;
        public Button left;
        public Button right;
    };
    [Serializable]
    public enum ConnectionProfile
    {
        one,
        two
    };
    public ConnectionProfile profile;
    public Connection[] connections;
    public string word = "U}{{}}}{}}{DRL[5[5UD]]";
    public bool inLoop = false;
    public Dictionary<int, List<int>> for_loop_state;
    public Dictionary<int, int> to_begin_for_loop;
    public Dictionary<int, int> to_end_for_loop;
    public Waypoint m_firstWaypoint;
    public Waypoint m_currWaypoint;
    public Waypoint m_objectiveWaypoint;
    public SpriteRenderer spriteR;
    public SpriteRenderer lcdDisplay;
    public float beginAngle;
    public float endAngle;
    public LanguageEnums.Types type;
    public bool inAction;
    public bool inRotation = false;
    public float speed = 1;
    public float angularSpeed = 5;
    public float currentTime = 0;
    public bool isActive = false;
    public bool withError = false;
    public bool selectedMoviment = false;
    public bool turnFinished = true;
    public GameManager gameManager;
    public Button[] buttonsInTerminal;
    public GameObject actualTrail;
    public Light2D actualTrailLight;
    public PlatformExecuteState m_ExecuteState;
    public PlatformInErrorState m_ErrorState;
    public PlatformStoppedState m_StoppedState;
    public PlatformProcessWordState m_ProcessWord;
    public PlatformStateMachine m_StateMachine;
    public bool canExecute = true;
    public enum ProcesserMode
    {
        PreviewMode,
        InGameMode
    }
    public ProcesserMode processerMode;
    public void ChangeToInGameMode()
    {
        processerMode = ProcesserMode.InGameMode;
    }
    public void ChangeToPreviewMode()
    {
        processerMode = ProcesserMode.PreviewMode;
    }
    public void SetButton(int index, Button b, Button down, Button up, Button right, Button left) {
        connections[index] = new Connection();
        connections[index].button = b;
        connections[index].down  = down;
        connections[index].up = up;
        connections[index].right = right;
        connections[index].left = left;
    }
    public void LoadProfile(ConnectionProfile profile)
    {
        switch(profile)
        {
            case ConnectionProfile.one: break;
            case ConnectionProfile.two:
            connections =  new Connection[5];
            SetButton(0,gameManager.allowedButtons[0] ,gameManager.allowedButtons[1], null, gameManager.allowedButtons[4], null);
            SetButton(1,gameManager.allowedButtons[1] ,gameManager.allowedButtons[2], gameManager.allowedButtons[0], gameManager.allowedButtons[4], null);
            SetButton(2,gameManager.allowedButtons[2] ,gameManager.allowedButtons[3], gameManager.allowedButtons[1], gameManager.allowedButtons[4], null);
            SetButton(3,gameManager.allowedButtons[3] ,gameManager.allowedButtons[4], gameManager.allowedButtons[2], gameManager.allowedButtons[4], null);
            SetButton(4,gameManager.allowedButtons[4] ,null, gameManager.allowedButtons[3], null, gameManager.allowedButtons[3]);
            break;
        }
    }
    private void Awake() {
        m_StateMachine = new PlatformStateMachine();
        m_ProcessWord = new PlatformProcessWordState(this, m_StateMachine);
        m_ExecuteState = new PlatformExecuteState(this, m_StateMachine);
        m_ErrorState = new PlatformInErrorState(this, m_StateMachine);
        m_StoppedState = new PlatformStoppedState(this, m_StateMachine);
        m_StateMachine.Initialize(m_ProcessWord);
    }
    void Start()
    {
        spriteR = GetComponent<SpriteRenderer>();
        processerMode = ProcesserMode.InGameMode;
        gameManager = FindObjectOfType<GameManager>();
        m_firstWaypoint = m_currWaypoint;
        to_end_for_loop = LanguageParser.GetTokensForLoop(word);
        to_begin_for_loop = new Dictionary<int, int>();
        for_loop_state = new Dictionary<int, List<int>>();
        LoadProfile(profile);
        foreach (KeyValuePair<int, int> dict in to_end_for_loop)
        {
            to_begin_for_loop[dict.Value] = dict.Key;
            for_loop_state[dict.Key] = new List<int>();
            int key = 0;
            int max = LanguageParser.GetNumberOfTimes(word, dict.Key, out key);
            for_loop_state[dict.Key].Add(0);
            for_loop_state[dict.Key].Add(max);
        }
    }
    public void SetWord(string word)
    {
        this.word = word;
    }
    public bool HasError()
    {
        return this.withError;
    }
    public bool SelectedMovement()
    {
        return this.selectedMoviment;
    }
    public void Activate()
    {
        isActive = true;
    }
    public void Deactivate()
    {
        isActive = false;
        actualTrail = null;
        if(actualTrailLight != null)
        {
            actualTrailLight.color = neutralColor;
        }
    }
    public void ResetPosition()
    {
        if(m_currWaypoint)
            m_currWaypoint.gameObject.GetComponentInChildren<SpriteRenderer>().material.SetColor("_Color",neutralColor);
        if(m_objectiveWaypoint)
            m_objectiveWaypoint.gameObject.GetComponentInChildren<SpriteRenderer>().material.SetColor("_Color",neutralColor);
        if(m_currWaypoint)
        {
            if(m_currWaypoint.up)
            {
                m_currWaypoint.light_up.color = neutralColor;
                m_currWaypoint.trail_up.GetComponentInChildren<SpriteRenderer>().material.SetColor("_Color",neutralColor);
            }
            if(m_currWaypoint.down)
            {
                m_currWaypoint.light_down.color = neutralColor;
                m_currWaypoint.trail_down.GetComponentInChildren<SpriteRenderer>().material.SetColor("_Color",neutralColor);
            }
            if(m_currWaypoint.left)
            {
                m_currWaypoint.light_left.color = neutralColor;
                m_currWaypoint.trail_left.GetComponentInChildren<SpriteRenderer>().material.SetColor("_Color",neutralColor);
            }
            if(m_currWaypoint.right!= null)
            {
                //Debug.Log(m_currWaypoint.right);
                m_currWaypoint.light_right.color = neutralColor;
                m_currWaypoint.trail_right.GetComponentInChildren<SpriteRenderer>().material.SetColor("_Color",neutralColor);
            }
        }
        m_currWaypoint = m_firstWaypoint;
        m_objectiveWaypoint = null;
        
        transform.position = m_firstWaypoint.transform.position;
        cpu = 0;
        inAction = false;
        finished = false;
        currentTime = 0;
        canExecute = true;
        m_StateMachine.Initialize(m_ProcessWord);
    }
    // Update is called once per frame
    public void Execute()
    {
        m_StateMachine.CurrentState.LogicUpdate();
        /*
        if(inAction && !turnFinished)
        {
            if(withError)
            {
                light.color = errorColor;
                spriteR.material.SetColor("_Color", errorColor);
                if(actualTrail)
                {
                    m_currWaypoint.gameObject.GetComponentInChildren<SpriteRenderer>().material.SetColor("_Color",errorColor);                    
                    m_objectiveWaypoint.gameObject.GetComponentInChildren<SpriteRenderer>().material.SetColor("_Color",errorColor);
                    actualTrail.GetComponent<SpriteRenderer>().material.SetColor("_Color",errorColor);
                    actualTrailLight.color = errorColor;
                }
            }
            else
            {
                light.color = acceptedColor;
                spriteR.material.SetColor("_Color", acceptedColor);
                if(actualTrail)
                {
                    m_currWaypoint.gameObject.GetComponentInChildren<SpriteRenderer>().material.SetColor("_Color",acceptedColor);
                    m_objectiveWaypoint.gameObject.GetComponentInChildren<SpriteRenderer>().material.SetColor("_Color",acceptedColor);
                    actualTrail.GetComponent<SpriteRenderer>().material.SetColor("_Color",acceptedColor);
                    actualTrailLight.color = acceptedColor;
                }
            }
        }
        if(finished)
        {
            if(processerMode == ProcesserMode.InGameMode)
            {
                word = "";
                cpu = 0;
            }
        }
        
        if(isActive && !finished && word.Length > 0)
        {
            if(!inAction)
            { 
                ProcessInput();
                switch(type)
                {
                    case LanguageEnums.Types.downArrow:
                        if(m_currWaypoint.down)
                        {
                            m_objectiveWaypoint = m_currWaypoint.down;
                            actualTrail = m_currWaypoint.trail_down;
                            actualTrailLight = m_currWaypoint.light_down;
                            m_currWaypoint.ActivateEmissionDown();
                            m_objectiveWaypoint.ActivateEmissionUp();
                        }
                        else
                        {
                            actualTrailLight = null;
                            actualTrail = null;
                            withError = true;
                        }
                        
                        foreach(Button button in gameManager.allowedButtons)
                        {
                            if(button.buttonValue == "D")
                            {
                                lcdDisplay.sprite = button.lcdDisplayImage;
                                break;
                            }
                        }
                        selectedMoviment = true;
                        inAction = true;
                        turnFinished = false;
                        break;
                    case LanguageEnums.Types.upArrow:
                        if (m_currWaypoint.up)
                        {
                            m_objectiveWaypoint = m_currWaypoint.up;
                            actualTrail = m_currWaypoint.trail_up;
                            actualTrailLight = m_currWaypoint.light_up;
                            m_currWaypoint.ActivateEmissionUp();
                            m_objectiveWaypoint.ActivateEmissionDown();
                        }
                        else
                        {
                            actualTrailLight = null;
                            actualTrail = null;
                            withError = true;
                        }
                        foreach(Button button in gameManager.allowedButtons)
                        {
                            if(button.buttonValue == "U")
                            {
                                lcdDisplay.sprite = button.lcdDisplayImage;
                                break;
                            }
                        }
                        selectedMoviment = true;
                        inAction = true;
                        turnFinished = false;
                        break;
                    case LanguageEnums.Types.leftArrow:
                        if (m_currWaypoint.left)
                        {
                            m_objectiveWaypoint = m_currWaypoint.left;
                            actualTrail = m_currWaypoint.trail_left;
                            actualTrailLight = m_currWaypoint.light_left;
                            m_currWaypoint.ActivateEmissionLeft();
                            m_objectiveWaypoint.ActivateEmissionRight();
                        }
                        else
                        {
                            withError = true;
                            actualTrail = null;
                            actualTrailLight = null;
                        }
                        foreach(Button button in gameManager.allowedButtons)
                        { 
                            if(button.buttonValue == "L")
                            {
                                lcdDisplay.sprite = button.lcdDisplayImage;
                                break;
                            }
                        }
                        selectedMoviment = true;
                        inAction = true;
                        turnFinished = false;
                        break;
                    case LanguageEnums.Types.rightArrow:
                        if (m_currWaypoint.right)
                        {
                            m_objectiveWaypoint = m_currWaypoint.right;
                            actualTrail = m_currWaypoint.trail_right;
                            actualTrailLight = m_currWaypoint.light_right;
                            m_currWaypoint.ActivateEmissionRight();
                            m_objectiveWaypoint.ActivateEmissionLeft();
                        }
                        else
                        {
                            withError = true;
                            actualTrail = null;
                            actualTrailLight = null;
                        }
                        foreach(Button button in gameManager.allowedButtons)
                        {
                            if(button.buttonValue == "R")
                            {
                                lcdDisplay.sprite = button.lcdDisplayImage;
                                break;
                            }
                        }
                        selectedMoviment = true;
                        inAction = true;
                        turnFinished = false;
                        break;
                    case LanguageEnums.Types.turnClockWise:
                        inAction = true;
                        inRotation = true;
                        endAngle = (beginAngle + 90) % 360;
                        m_objectiveWaypoint = m_currWaypoint;
                        turnFinished = false;
                        actualTrail = null;
                        break;
                    case LanguageEnums.Types.turnAntiClockWise:
                        inAction = true;
                        inRotation = true;
                        endAngle = (beginAngle - 90)%360;
                        actualTrail = null;
                        m_objectiveWaypoint = m_currWaypoint;
                        turnFinished = false;
                        break;
                }
            }
            else
            {
                if( !turnFinished && gameManager.AllPlatformSelectedMovement())
                { 
                    switch (type)
                    {
                        case LanguageEnums.Types.downArrow:
                            if(!withError)
                            {
                                withError = !gameManager.IsValid(this, m_currWaypoint, m_objectiveWaypoint);
                            }
                            if(!withError)
                            { 
                                if(currentTime  == 0)
                                    this.transform.position = new Vector2(transform.position.x, transform.position.y - speed * Time.deltaTime);
                            }
                            else
                            {
                                if (currentTime == 0)
                                    currentTime = 3;
                            }
                            break;
                        case LanguageEnums.Types.upArrow:
                            if (!withError)
                            {
                                withError = !gameManager.IsValid(this, m_currWaypoint, m_objectiveWaypoint);
                            }
                            if (!withError)
                            {
                                if(currentTime  == 0)
                                    this.transform.position = new Vector2(transform.position.x, transform.position.y + speed * Time.deltaTime);
                            }
                            else
                            {
                                if (currentTime == 0)
                                    currentTime = 3;
                            }
                            break;
                        case LanguageEnums.Types.leftArrow:
                            if (!withError)
                            {
                                withError = !gameManager.IsValid(this, m_currWaypoint, m_objectiveWaypoint);
                            }
                            if (!withError)
                            {
                                if(currentTime  == 0)
                                    this.transform.position = new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y);
                            }
                            else
                            {
                                if (currentTime == 0)
                                    currentTime = 3;
                            }
                            break;
                        case LanguageEnums.Types.rightArrow:
                            if (!withError)
                            {
                                withError = !gameManager.IsValid(this, m_currWaypoint, m_objectiveWaypoint);
                            }
                            if (!withError)
                            {
                                if(currentTime  == 0)
                                    this.transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
                            }
                            else
                            {
                                if (currentTime == 0)
                                    currentTime = 3;
                            }
                            break;

                    }
                    switch (type)
                    {
                        case LanguageEnums.Types.turnClockWise:
                            this.transform.Rotate(Vector3.forward * angularSpeed * Time.deltaTime);
                            break;
                        case LanguageEnums.Types.turnAntiClockWise:
                            this.transform.Rotate(-1 * Vector3.forward * angularSpeed * Time.deltaTime);
                            break;
                    }
                }
                if(!withError)
                { 
                    if (inRotation)
                    {   
                        if(Mathf.Abs(Mathf.DeltaAngle(endAngle,this.transform.eulerAngles.z)) < 1.5f)
                        {
                            this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, this.transform.eulerAngles.y, endAngle);
                            beginAngle = endAngle;
                            if (currentTime == 0)
                                currentTime = 0.5f;
                        }
                    }
                    else {
                        Debug.Log(Vector2.Distance(transform.position, m_objectiveWaypoint.transform.position));
                        Debug.Log(m_currWaypoint.transform.position);
                    
                        if(Vector2.Distance(transform.position, m_objectiveWaypoint.transform.position) < 0.2)
                        {
                            //Debug.LogError(transform.position + " " + m_objectiveWaypoint.gameObject.name + " " + m_objectiveWaypoint.gameObject.transform.position + " " + m_currWaypoint.transform.position);
                            this.transform.position = m_objectiveWaypoint.transform.position;
                            
                            
                            if(currentTime == 0)
                                currentTime = 0.5f;
                        
                        }
                    }
                }
            }
            if(currentTime > 0)
            {
                currentTime -= Time.deltaTime;
                if(currentTime <= 0)
                {
                    currentTime = 0;
                    
                    turnFinished = true;
                    
                    m_currWaypoint.gameObject.GetComponentInChildren<SpriteRenderer>().material.SetColor("_Color",neutralColor);
                    if(!withError)
                    m_currWaypoint = m_objectiveWaypoint;
                    
                }
            }
            //if(turnFinished && !withError)
            ////if(gameManager.AllPlatformsFinishedAction())
            //{
            //    FinishAction();
            //    //gameManager.FinishAllPlatformsAction();
            //}
        }
        else
        {
            m_objectiveWaypoint = null;
        }
        */
    }
    public void FinishAction()
    {
        inRotation = false;
        inAction = false;
        
        turnFinished = false;
        selectedMoviment = false;
        light.color = neutralColor;
        //spriteR.material.SetColor("_Color", neutralColor);
        if(actualTrail)
        {
            m_firstWaypoint.gameObject.GetComponentInChildren<SpriteRenderer>().material.SetColor("_Color",neutralColor);
            m_firstWaypoint.TurnOff();
            m_currWaypoint.TurnOff();
            if(processerMode == ProcesserMode.InGameMode)
            {

                m_firstWaypoint = m_currWaypoint;

            }
            actualTrailLight.color = neutralColor;
            m_firstWaypoint.gameObject.GetComponentInChildren<SpriteRenderer>().material.SetColor("_Color",neutralColor);
            m_currWaypoint.gameObject.GetComponentInChildren<SpriteRenderer>().material.SetColor("_Color",neutralColor);
            m_objectiveWaypoint.gameObject.GetComponentInChildren<SpriteRenderer>().material.SetColor("_Color",neutralColor);
            
            actualTrail.GetComponent<SpriteRenderer>().material.SetColor("_Color",neutralColor);
            if(withError)
                m_objectiveWaypoint = null;
            actualTrail = null;
        }
        if (cpu == word.Length)
        {
            finished = true;
        }
        withError = false;
    }    
    public void ProcessInput()
    {
        inLoop = false;

        //Debug.Log(word[cpu]);
        //Debug.Log(cpu);
        //Debug.Log(word);
        type = LanguageParser.GetExecution(word, cpu);

        while (type == LanguageEnums.Types.beginForLoop)
        {
            beginForLoop = cpu;
            totalForLoop = LanguageParser.GetNumberOfTimes(word, beginForLoop, out beginForLoop);
            cpu = beginForLoop;
            type = LanguageParser.GetExecution(word, cpu);

        }
        while (type == LanguageEnums.Types.endForLoop)
        {

            for_loop_state[to_begin_for_loop[cpu]][0]++;

            if (for_loop_state[to_begin_for_loop[cpu]][0] >= for_loop_state[to_begin_for_loop[cpu]][1])
            {
                for_loop_state[to_begin_for_loop[cpu]][0] = 0;
                cpu++;
            }
            else
            {

                cpu = to_begin_for_loop[cpu];
                LanguageParser.GetNumberOfTimes(word, cpu, out beginForLoop);
                cpu = beginForLoop;
                type = LanguageParser.GetExecution(word, cpu);
                while (type == LanguageEnums.Types.beginForLoop)
                {
                    //Debug.Log(cpu);
                    LanguageParser.GetNumberOfTimes(word, cpu, out beginForLoop);
                    cpu = beginForLoop;

                    type = LanguageParser.GetExecution(word, cpu);
                }
            }
            
            if (cpu < word.Length)
                type = LanguageParser.GetExecution(word, cpu);
            else
            {
                type = LanguageEnums.Types.doNothing;
            }
        }
        /*
        if (cpu < word.Length)
            Debug.Log(cpu + " Executed " + LanguageParser.GetExecution(word, cpu));
        */
        if (!inLoop)
            cpu++;

        
    }
}
