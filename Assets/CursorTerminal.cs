using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorTerminal : MonoBehaviour
{
    // Start is called before the first frame update
    #region States
    public CursorTerminalStateMachine StateMachine;
    public CursorTerminalStopped CursorStoppedState;
    public CursorTerminalPlacing CursorPlacingState;
    public CursorTerminalSelecting CursorSelectingState;
    public CursorTerminalDeletingState CursorTerminalDeleting;
    public CursorShowInformation CursorShowInfo;
    #endregion
    public Button selectedButton;
    public GameObject upLeft;
    public GameObject upRight;
    public GameObject bottomRight;
    public GameObject bottomLeft;
    public GameObject middlePoint;
    public Terminal terminal;
    public PlayerInputHandler InputHandler;
    public int cursorInSelectPosition = 0;
    public int cursorInTerminalPosition = 0;
    public float timeToMove = 0.2f;
    public float currentTimeToMove = 0;
    public void Awake()
    {
        StateMachine = new CursorTerminalStateMachine();
        CursorStoppedState = new CursorTerminalStopped(this, StateMachine, "");
        CursorPlacingState = new CursorTerminalPlacing(this, StateMachine, "");
        CursorSelectingState = new CursorTerminalSelecting(this, StateMachine, "");
        CursorTerminalDeleting = new CursorTerminalDeletingState(this, StateMachine, "");
        CursorShowInfo = new CursorShowInformation(this, StateMachine, "");
        StateMachine.Initialize(CursorStoppedState);

    }
    void Start()
    {
        
        this.SetPositionInSelectPosition(0);
        
        SetSize(terminal.positionsToChoice[0].GetComponent<Image>().sprite.rect.width * 2, terminal.positionsToChoice[0].GetComponent<Image>().sprite.rect.height * 2);
    }

    public void SetSize(float w, float h)
    {
        upLeft.transform.position = new Vector2(middlePoint.transform.position.x - (w / 2.0f), middlePoint.transform.position.y + (h / 2.0f));
        upRight.transform.position = new Vector2(middlePoint.transform.position.x + (w / 2.0f), middlePoint.transform.position.y + (h / 2.0f));
        bottomRight.transform.position = new Vector2(middlePoint.transform.position.x + (w / 2.0f), middlePoint.transform.position.y - (h / 2.0f));
        bottomLeft.transform.position = new Vector2(middlePoint.transform.position.x - (w / 2.0f), middlePoint.transform.position.y - (h / 2.0f));
    }
    public void SelectButton(Button button)
    {
        selectedButton = button;
    }
    public void DiselectButton()
    {
        selectedButton = null;
    }
    public void SetButtonInTerminal(int position, Button image)
    {
        terminal.postionsInTerminal[position].GetComponent<Image>().sprite = null;
        Color c = terminal.postionsInTerminal[position].GetComponent<Image>().color;
        c.a = 1;
        terminal.postionsInTerminal[position].GetComponent<Image>().color = c;
        terminal.postionsInTerminal[position].GetComponent<Image>().sprite = image.buttonImage;
        terminal.buttonsInTerminal[position] = image;
        if(terminal.processerInput.buttonsInTerminal.Length <= 0)
        {
            terminal.processerInput.buttonsInTerminal = new Button[terminal.postionsInTerminal.Length];
        }
        terminal.processerInput.buttonsInTerminal[position] = image;
    }
    public void deleteButtonInTerminal(int position)
    {
        terminal.postionsInTerminal[position].GetComponent<Image>().sprite = null;
        Color c = terminal.postionsInTerminal[position].GetComponent<Image>().color;
        c.a = 0;
        terminal.postionsInTerminal[position].GetComponent<Image>().color = c;
        terminal.buttonsInTerminal[position] = null;
    }
    public void SetPositionInSelectPosition(int input_x)
    {
        if (currentTimeToMove <= 0)
        {
            if (input_x > 0 && cursorInSelectPosition < terminal.buttonsToChoice.Length - 1)
            {
                cursorInSelectPosition++;
                currentTimeToMove = timeToMove;
            }
            else if(input_x > 0 && cursorInSelectPosition >= terminal.buttonsToChoice.Length - 1)
            {
                cursorInSelectPosition = terminal.positionsToChoice.Length - 1;
                currentTimeToMove = timeToMove;
            }
            if(input_x < 0 && cursorInSelectPosition > terminal.buttonsToChoice.Length - 1)
            {
                cursorInSelectPosition = terminal.buttonsToChoice.Length - 1;
                currentTimeToMove = timeToMove;
            }
            else if (input_x < 0 && cursorInSelectPosition > 0)
            {
                cursorInSelectPosition--;
                currentTimeToMove = timeToMove;
            }
            //middlePoint.transform.position = terminal.positionsToChoice[cursorInSelectPosition].transform.position;
        }
        this.transform.parent = terminal.positionsToChoice[cursorInSelectPosition].transform;
        this.transform.localPosition = Vector3.zero;
        middlePoint.transform.localPosition = Vector3.zero;
    }
    
    public void SetPositionInTerminalPosition(int input_x)
    {
        if(currentTimeToMove <= 0)
        { 
            if(input_x > 0 && cursorInTerminalPosition < terminal.postionsInTerminal.Length - 1)
            {
                cursorInTerminalPosition++;
                currentTimeToMove = timeToMove;
            }
            if (input_x < 0 && cursorInTerminalPosition > 0)
            {
                cursorInTerminalPosition--;
                currentTimeToMove = timeToMove;
            }
        }
        this.transform.parent = terminal.postionsInTerminal[cursorInTerminalPosition].transform;
        this.transform.localPosition = Vector3.zero;
        middlePoint.transform.localPosition = Vector3.zero;
    }
    // Update is called once per frame
    void Update()
    {
        
        StateMachine.CurrentState.LogicUpdate();
        Debug.Log(StateMachine.CurrentState.ToString());
        if (currentTimeToMove > 0)
        {
            currentTimeToMove -= Time.deltaTime;
        }
        if(selectedButton)
        {
            Color c = middlePoint.GetComponent<Image>().color;
            c.a = 1;
            middlePoint.GetComponent<Image>().color = c;
            middlePoint.GetComponent<Image>().sprite = selectedButton.buttonImage;
        }
        else
        {
            Color c = middlePoint.GetComponent<Image>().color;
            c.a = 0;
            middlePoint.GetComponent<Image>().color = c;
        }
        
    }
}
