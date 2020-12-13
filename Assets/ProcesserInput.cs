using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcesserInput : MonoBehaviour
{
    // Start is called before the first frame update
    public int cpu = 0;
    int beginForLoop = 0;
    int endForLoop = 0;
    int actualForLoop = 0;
    int totalForLoop = 0;
    public bool finished = false;
    public Button[] allowedButtons;
    [Serializable]
    public class Connection
    {
        public Button button;
        public Button up;
        public Button down;
        public Button left;
        public Button right;
    };
    
    public Connection[] connections;
    public string word = "U}{{}}}{}}{DRL[5[5UD]]";
    public bool inLoop = false;
    public Dictionary<int, List<int>> for_loop_state;
    public Dictionary<int, int> to_begin_for_loop;
    public Dictionary<int, int> to_end_for_loop;
    public Waypoint m_firstWaypoint;
    public Waypoint m_currWaypoint;
    public Waypoint m_objectiveWaypoint;
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
    void Start()
    {
        processerMode = ProcesserMode.InGameMode;
        gameManager = FindObjectOfType<GameManager>();
        m_firstWaypoint = m_currWaypoint;
        to_end_for_loop = LanguageParser.GetTokensForLoop(word);
        to_begin_for_loop = new Dictionary<int, int>();
        for_loop_state = new Dictionary<int, List<int>>();
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
    }
    public void ResetPosition()
    {
        m_currWaypoint = m_firstWaypoint;
        m_objectiveWaypoint = null;
        transform.position = m_firstWaypoint.transform.position;
        cpu = 0;
        inAction = false;
        finished = false;
        currentTime = 0;
    }
    // Update is called once per frame
    void Update()
    {
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
                            
                        }
                        else
                        {
                            withError = true;
                        }
                        selectedMoviment = true;
                        inAction = true;
                        turnFinished = false;
                        break;
                    case LanguageEnums.Types.upArrow:
                        if (m_currWaypoint.up)
                        {
                            m_objectiveWaypoint = m_currWaypoint.up;
                            
                        }
                        else
                        {
                            withError = true;
                        }
                        selectedMoviment = true;
                        inAction = true;
                        turnFinished = false;
                        break;
                    case LanguageEnums.Types.leftArrow:
                        if (m_currWaypoint.left)
                        {
                            m_objectiveWaypoint = m_currWaypoint.left;
                            
                        }
                        else
                        {
                            withError = true;
                        }
                        selectedMoviment = true;
                        inAction = true;
                        turnFinished = false;
                        break;
                    case LanguageEnums.Types.rightArrow:
                        if (m_currWaypoint.right)
                        {
                            m_objectiveWaypoint = m_currWaypoint.right;
                            
                        }
                        else
                        {
                            withError = true;
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
                        break;
                    case LanguageEnums.Types.turnAntiClockWise:
                        inAction = true;
                        inRotation = true;
                        endAngle = (beginAngle - 90)%360;
                        m_objectiveWaypoint = m_currWaypoint;
                        turnFinished = false;
                        break;
                }
            }
            else
            {
                if(gameManager.AllPlatformSelectedMovement())
                { 
                    switch (type)
                    {
                        case LanguageEnums.Types.downArrow:
                            if(!withError)
                            {
                                withError = !gameManager.IsValid(m_currWaypoint, m_objectiveWaypoint);
                            }
                            if(!withError)
                            { 
                                this.transform.position = new Vector2(transform.position.x, transform.position.y - speed * Time.deltaTime);
                            }
                            else
                            {
                                if (currentTime == 0)
                                    currentTime = 0.5f;
                            }
                            break;
                        case LanguageEnums.Types.upArrow:
                            if (!withError)
                            {
                                withError = !gameManager.IsValid(m_currWaypoint, m_objectiveWaypoint);
                            }
                            if (!withError)
                            {
                                this.transform.position = new Vector2(transform.position.x, transform.position.y + speed * Time.deltaTime);
                            }
                            else
                            {
                                if (currentTime == 0)
                                    currentTime = 0.5f;
                            }
                            break;
                        case LanguageEnums.Types.leftArrow:
                            if (!withError)
                            {
                                withError = !gameManager.IsValid(m_currWaypoint, m_objectiveWaypoint);
                            }
                            if (!withError)
                            {
                                this.transform.position = new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y);
                            }
                            else
                            {
                                if (currentTime == 0)
                                    currentTime = 0.5f;
                            }
                            break;
                        case LanguageEnums.Types.rightArrow:
                            if (!withError)
                            {
                                withError = !gameManager.IsValid(m_currWaypoint, m_objectiveWaypoint);
                            }
                            if (!withError)
                            {
                                this.transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
                            }
                            else
                            {
                                if (currentTime == 0)
                                    currentTime = 0.5f;
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
                        if(Mathf.Abs(Mathf.DeltaAngle(endAngle,this.transform.eulerAngles.z)) < 1)
                        {
                            this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, this.transform.eulerAngles.y, endAngle);
                            beginAngle = endAngle;
                            if (currentTime == 0)
                                currentTime = 0.5f;
                        }
                    }
                    else {
                        //Debug.Log(Vector2.Distance(transform.position, m_objectiveWaypoint.transform.position));
                        //Debug.Log(m_currWaypoint.transform.position);
                    
                        if(Vector2.Distance(transform.position, m_objectiveWaypoint.transform.position) < 0.08)
                        {
                            Debug.Log(transform.position + " " + m_objectiveWaypoint.gameObject.name + " " + m_objectiveWaypoint.gameObject.transform.position + " " + m_currWaypoint.transform.position);
                            this.transform.position = m_objectiveWaypoint.transform.position;
                            m_currWaypoint = m_objectiveWaypoint;
                            if(processerMode == ProcesserMode.InGameMode)
                            {
                                m_firstWaypoint = m_currWaypoint;
                            }
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
                    
                }
            }
            if(turnFinished)
            {
                inRotation = false;
                inAction = false;
                withError = false;
                turnFinished = false;
                selectedMoviment = false;
                if (cpu == word.Length)
                {
                    finished = true;
                }
            }
        }
        
    }
    
    void ProcessInput()
    {
        inLoop = false;

        //Debug.Log(word[cpu]);
        Debug.Log(cpu);
        Debug.Log(word);
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
