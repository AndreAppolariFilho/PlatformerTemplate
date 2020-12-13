using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Terminal : MonoBehaviour
{
    // Start is called before the first frame update
    public Button[] buttonsInTerminal;
    public Button[] buttonsToChoice;
    public Button deleteButton;
    public GameObject[] internalPositionsToChoice;
    public Dictionary<Button, GameObject>  positionsToChoice = new Dictionary<Button, GameObject>();
    public InputTerminalController terminalController;
    public List<GameObject> postionsInTerminal = new List<GameObject>();
    public ProcesserInput processerInput;
    public GameObject selectingObjectButtonsHud;
    public GameObject selectingInTerminalHud;
    public GameObject deletingInTerminalHud;
    public Modal tutorialPanel;
    public GameObject cursor;
    public struct Connection
    {
        public Button up;
        public Button down;
        public Button left;
        public Button right;
    }
    [SerializeField] public Dictionary<Button, Connection> connections = new Dictionary<Button, Connection>();
    public void AllocTerminalSize()
    {
        int size = internalPositionsToChoice.Length;
        buttonsInTerminal = new Button[size];
        processerInput.buttonsInTerminal = new Button[size];
    }
    public List<GameObject> GetPositionsInTerminal()
    {
        return terminalController.postionsInTerminal;
    }
    public void SetConnection(Button button1, Button button2, string type)
    {
        Debug.LogError(button1.name);
        if(!connections.ContainsKey(button1))
        {
            connections[button1] = new Connection();
        }
        Connection c = connections[button1];
        if (type == "up")
        {   
            c.up = button2;
        }
        if(type == "down")
        {
            c.down = button2;
        }
        if(type == "left")
        {
            c.left = button2;
        }
        if(type == "right")
        {
            c.right = button2;
        }
        connections[button1] = c;

    }
    private void Start()
    {
        

    }
    public void SetQuantityOfButtonToChoice(int size)
    {
        buttonsToChoice = new Button[size];
        
    }
    public void ActivateObjectButtonsHud()
    {
        if (selectingObjectButtonsHud)
            selectingObjectButtonsHud.SetActive(true);
    }
    public void ActivateSelectingInTerminalHud()
    {
        if (selectingInTerminalHud)
            selectingInTerminalHud.SetActive(true);
    }

    public void ActivateDeletingInTerminalHud()
    {
        if (deletingInTerminalHud)
            deletingInTerminalHud.SetActive(true);
    }
    public void DeactivateObjectButtonsHud()
    {
        if (selectingObjectButtonsHud)
            selectingObjectButtonsHud.SetActive(false);
    }
    public void DeactivateSelectingInTerminalHud()
    {
        if (selectingInTerminalHud)
            selectingInTerminalHud.SetActive(false);
    }

    public void DeactivateDeletingInTerminalHud()
    {
        if (deletingInTerminalHud)
            deletingInTerminalHud.SetActive(false);
    }
    public void SetActualPlatform(ProcesserInput actualPlatform)
    {
        processerInput = actualPlatform;
        AllocTerminalSize();
    }
    public void DiselectPlatform()
    {
        processerInput = null;
    }
    public void UpdatePlatform()
    {
        string word = "";
        
        foreach(Button button in terminalController.buttonsInTerminal)
        {
            if (button)
                word += button.buttonValue;
            else
                word += " ";
        }
        word = word.TrimEnd(' ');
        word = word.Replace(' ', 'O');
        processerInput.SetWord(word);
    }
    public void SetButtonToChoice(int position, Button button)
    {
        Debug.LogError("Button To Choice "+button.name);
        buttonsToChoice[position] = button;
        Debug.Log(position);
        Debug.Log(internalPositionsToChoice.Length);
        if(!positionsToChoice.ContainsKey(button))
        {
            positionsToChoice[button] = internalPositionsToChoice[position];
        }
        positionsToChoice[button].GetComponentInChildren<TMP_Text>().text = button.funtionName;
        positionsToChoice[button].SetActive(true);
    }
    public void AddToTerminal(Button button, int position)
    {
        if (position < terminalController.buttonsInTerminal.Count)
        { 
            buttonsInTerminal[position] = button;
            processerInput.buttonsInTerminal[position] = button;
        }
    }
    public void DeleteButtonInTerminal(int position)
    {
        if (position < buttonsInTerminal.Length)
        {
            buttonsInTerminal[position] = null;
            //processerInput.buttonsInTerminal[position] = null;

        }
    }
    public Button GetButton(int position)
    {
        if(position < buttonsToChoice.Length)
            return buttonsToChoice[position];
        return buttonsToChoice[buttonsToChoice.Length - 1];
    }
    public Button GetButtonInTerminal(int position)
    {
        if (position < buttonsInTerminal.Length)
            return buttonsInTerminal[position];
        return buttonsInTerminal[buttonsInTerminal.Length - 1];
    }
    
    public void Validate()
    {

    }
    public void Update()
    {
        if(processerInput)
        {
            UpdatePlatform();
        }
    }
}
