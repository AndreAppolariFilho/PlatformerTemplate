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
    public GameObject arrow;
    public Terminal terminal;
    public PlayerInputHandler InputHandler;
    public Button actualButtonInSelectPosition;
    
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
        
        actualButtonInSelectPosition = terminal.buttonsToChoice[0];
        this.SetPositionInSelectPosition(0, 0);
        //SetSize(terminal.positionsToChoice[0].GetComponent<Image>().sprite.rect.width * 2, terminal.positionsToChoice[0].GetComponent<Image>().sprite.rect.height * 2);
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
        terminal.terminalController.SetText(position, image.funtionName);
        if(position == terminal.GetPositionsInTerminal().Count - 1)
        {
            terminal.terminalController.AddNewPosition();
        }
        terminal.terminalController.SetButton(position, image);
        
    }
    public void deleteButtonInTerminal(int position)
    {
        terminal.terminalController.SetText(position, "");
        
        if(terminal.terminalController.buttonsInTerminal.Count > 1)
        {
            if(position == terminal.terminalController.buttonsInTerminal.Count - 2)
            {
                terminal.terminalController.buttonsInTerminal[position] = null;
                terminal.terminalController.buttonsInTerminal.RemoveAt(terminal.terminalController.buttonsInTerminal.Count - 1);
                terminal.terminalController.postionsInTerminal.RemoveAt(terminal.terminalController.postionsInTerminal.Count - 1);
            }
        }
    }
    public void SetPositionInSelectPosition(int input_x, int input_y)
    {
        if (currentTimeToMove <= 0)
        {
            if(input_x > 0)
            {
                if(terminal.connections[actualButtonInSelectPosition].right)
                {
                    actualButtonInSelectPosition = terminal.connections[actualButtonInSelectPosition].right;
                    currentTimeToMove = timeToMove;
                }
            }
            if(input_x < 0)
            {
                if(terminal.connections[actualButtonInSelectPosition].left)
                {
                    actualButtonInSelectPosition = terminal.connections[actualButtonInSelectPosition].left;
                    currentTimeToMove = timeToMove;
                }
            }
            if(input_y > 0)
            {
                if(terminal.connections[actualButtonInSelectPosition].up)
                {
                    actualButtonInSelectPosition = terminal.connections[actualButtonInSelectPosition].up;
                    currentTimeToMove = timeToMove;
                }
            }
            if(input_y < 0)
            {
                if(terminal.connections[actualButtonInSelectPosition].down)
                {
                    actualButtonInSelectPosition = terminal.connections[actualButtonInSelectPosition].down;
                    currentTimeToMove = timeToMove;
                }
            }
        }
        if(terminal.positionsToChoice.ContainsKey(actualButtonInSelectPosition))
        {
            for(int i = 0; i < terminal.positionsToChoice[actualButtonInSelectPosition].transform.childCount; i++)
            {
                Transform child = terminal.positionsToChoice[actualButtonInSelectPosition].transform.GetChild(i);
                if(child.tag == "CursorPosition")
                {
                    this.transform.parent = child;
                    break;
                }
                
            }
        }
        this.transform.localPosition = Vector3.zero;
        arrow.transform.localPosition = Vector3.zero;
    }
    
    public void SetPositionInTerminalPosition(int input_y)
    {
        if(!selectedButton.isRemovalNode)
        {
            if(!terminal.terminalController.buttonsInTerminal[cursorInTerminalPosition])
                terminal.terminalController.SetText(cursorInTerminalPosition, "");
            else
                terminal.terminalController.SetText(cursorInTerminalPosition, terminal.terminalController.buttonsInTerminal[cursorInTerminalPosition].funtionName);
        }
        if(input_y > 0 && terminal.terminalController.isTopPosition(cursorInTerminalPosition))
        {
            terminal.terminalController.ReassingToDownPositions();
        }
        if(terminal.terminalController.isBottonPosition(cursorInTerminalPosition))
        {
            Debug.LogError(terminal.terminalController.postionsInTerminal[cursorInTerminalPosition].transform.localPosition.y);
            Debug.LogError(terminal.terminalController.firstPositionInTerminal.transform.localPosition.y);
            Debug.LogError("To Up");
            terminal.terminalController.ReassignToUpPositions();
        }
        
        
        if(currentTimeToMove <= 0)
        {
            
            if(input_y < 0 && cursorInTerminalPosition < terminal.GetPositionsInTerminal().Count - 1)
            {
                
                cursorInTerminalPosition++;
                currentTimeToMove = timeToMove;
            }
            if (input_y > 0 && cursorInTerminalPosition > 0)
            {
                
                cursorInTerminalPosition--;
                currentTimeToMove = timeToMove;
            }
        }
        terminal.terminalController.SetActualIndexPosition(cursorInTerminalPosition);        
        if(!selectedButton.isRemovalNode)
        {
            terminal.terminalController.SetText(cursorInTerminalPosition, selectedButton.funtionName);
        }
        SetCursor(cursorInTerminalPosition, arrow);
        
    }
    public void SetCursor(int position, GameObject cursor)
    {
        for(int i = 0; i < terminal.terminalController.postionsInTerminal[position].transform.childCount; i++)
        {
            Transform child = terminal.terminalController.postionsInTerminal[position].transform.GetChild(i);
            if(child.tag == "CursorPosition")
            {
                this.transform.parent = child;
                break;
            }
            
        }
        this.transform.localPosition = Vector3.zero;
        cursor.transform.localPosition = Vector3.zero;
    }
    // Update is called once per frame
    void Update()
    {
        
        StateMachine.CurrentState.LogicUpdate();
        if (currentTimeToMove > 0)
        {
            currentTimeToMove -= Time.deltaTime;
        }
        //if(selectedButton)
        //{
        //    Color c = middlePoint.GetComponent<Image>().color;
        //    c.a = 1;
        //    middlePoint.GetComponent<Image>().color = c;
        //    middlePoint.GetComponent<Image>().sprite = selectedButton.buttonImage;
        //}
        //else
        //{
        //    Color c = middlePoint.GetComponent<Image>().color;
        //    c.a = 0;
        //    middlePoint.GetComponent<Image>().color = c;
        //}
        
    }
}
