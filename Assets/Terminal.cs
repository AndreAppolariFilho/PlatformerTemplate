using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Terminal : MonoBehaviour
{
    // Start is called before the first frame update
    public Button[] buttonsInTerminal;
    public Button[] buttonsToChoice;
    public Button deleteButton;
    public GameObject[] positionsToChoice;
    public GameObject[] postionsInTerminal;
    public ProcesserInput processerInput;
    public GameObject selectingObjectButtonsHud;
    public GameObject selectingInTerminalHud;
    public GameObject deletingInTerminalHud;
    public Modal tutorialPanel;
    public void AllocTerminalSize(int size)
    {
        buttonsInTerminal = new Button[size];
        processerInput.buttonsInTerminal = new Button[size];
    }
    private void Start()
    {
        if(buttonsInTerminal.Length <= 0)
            AllocTerminalSize(postionsInTerminal.Length);

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
    }
    public void DiselectPlatform()
    {
        processerInput = null;
    }
    public void UpdatePlatform()
    {
        string word = "";
        
        foreach(Button button in buttonsInTerminal)
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
        buttonsToChoice[position] = button;
        positionsToChoice[position].GetComponent<Image>().sprite = button.buttonImage;
        Color c = positionsToChoice[position].GetComponent<Image>().color;
        c.a = 1;
        positionsToChoice[position].GetComponent<Image>().color = c;
    }
    public void AddToTerminal(Button button, int position)
    {
        if (position < buttonsInTerminal.Length)
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
