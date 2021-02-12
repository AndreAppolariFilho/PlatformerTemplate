using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class InputTerminalController : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform firstPositionInTerminal;
    public Transform actualFistPositionInTerminal;
    public int actualPosition = 0;
    public Transform endPosition;
    public GameObject rowPrefab;
    public bool scrobarIsActive = false;
    public GameObject upScrollBarArrow;
    public GameObject downScrollBarArrow;
    public List<GameObject> postionsInTerminal = new List<GameObject>();
    public List<Button> buttonsInTerminal = new List<Button>();
    public int quantity = 0;
    public float offset = 600;
    void Start()
    {
        if(firstPositionInTerminal)
        {
            actualFistPositionInTerminal = firstPositionInTerminal;
            GameObject instantiate = Instantiate(
                rowPrefab, 
                firstPositionInTerminal.position, 
                Quaternion.identity,
                firstPositionInTerminal.parent.transform
            );
            if(postionsInTerminal.Count == 0)
            {
            postionsInTerminal.Add(instantiate);
            //buttonsInTerminal.Add(new Button());
            postionsInTerminal[0].GetComponentInChildren<TMP_Text>().text = "";
            }
        }

    }
    public void ResetInputTerminalController()
    {
        for(int i = 0; i < postionsInTerminal.Count; i++)
        {
            Destroy(postionsInTerminal[i]);
        }
        postionsInTerminal = new List<GameObject>();
        for(int i = 0; i < buttonsInTerminal.Count; i++)
        {
            buttonsInTerminal[i] = new Button();
        }
        buttonsInTerminal = new List<Button>();
        if(firstPositionInTerminal)
        {
            actualFistPositionInTerminal = firstPositionInTerminal;
            GameObject instantiate = Instantiate(
                rowPrefab, 
                firstPositionInTerminal.position, 
                Quaternion.identity,
                firstPositionInTerminal.parent.transform
            );
            postionsInTerminal.Add(instantiate);
            buttonsInTerminal.Add(new Button());
            postionsInTerminal[0].GetComponentInChildren<TMP_Text>().text = "";
        }
    }
    public int GetLastIndex()
    {
        int index = 0;
        
        if(buttonsInTerminal[0] && buttonsInTerminal[0].funtionName == "")
        {
            return index;
        }
        for(int i = 1; i < buttonsInTerminal.Count; i++)
        {
            //if(buttonsInTerminal[i] == null || buttonsInTerminal[i].funtionName == "")
            
            if(buttonsInTerminal[i] && string.IsNullOrEmpty(buttonsInTerminal[i].funtionName))
            {
                
                return i;
            }
        }
        return index;
    } 
    public void AddNewPosition()
    {
        postionsInTerminal.Add(Instantiate(rowPrefab, firstPositionInTerminal.parent.transform));
        buttonsInTerminal.Add(new Button());
        int i = postionsInTerminal.Count - 1;
        postionsInTerminal[i].GetComponentInChildren<TMP_Text>().text = "";
        postionsInTerminal[i].transform.localPosition = new Vector3(
            firstPositionInTerminal.transform.localPosition.x, 
            postionsInTerminal[i - 1].transform.localPosition.y - offset , 
            0
        );
    }
    public void SetText(int position, string text)
    {
        if(postionsInTerminal[position])
        {
            TMP_Text[] texts = postionsInTerminal[position].GetComponentsInChildren<TMP_Text>();
            foreach(TMP_Text tmp_text in texts)
            {
                if(!tmp_text.CompareTag("CursorTerminal"))
                {
                    tmp_text.text = text;
                }
            }
        }
    }
    public void ResetFirstPosition()
    {
        actualFistPositionInTerminal = firstPositionInTerminal;
    }
    public void ReassignToUpPositions()
    {
        postionsInTerminal[0].transform.localPosition = new Vector3(
            postionsInTerminal[0].transform.localPosition.x,
            postionsInTerminal[0].transform.localPosition.y + offset ,
            0
        );
        //postionsInTerminal[0].transform.localPosition = actualFistPositionInTerminal.transform.localPosition;
        for(int i = 1; i < postionsInTerminal.Count; i++)
        {
            postionsInTerminal[i].transform.localPosition = new Vector3(
                firstPositionInTerminal.transform.localPosition.x, 
                postionsInTerminal[i - 1].transform.localPosition.y - offset , 
                0
            );
        }
    }
    public void ReassingToDownPositions()
    {
        postionsInTerminal[0].transform.localPosition = new Vector3(
            postionsInTerminal[0].transform.localPosition.x,
            postionsInTerminal[0].transform.localPosition.y - offset,
            0
        );
        
        //postionsInTerminal[0].transform.localPosition = actualFistPositionInTerminal.transform.localPosition;
        
        for(int i = 1; i < postionsInTerminal.Count; i++)
        {
            postionsInTerminal[i].transform.localPosition = new Vector3(
                firstPositionInTerminal.transform.localPosition.x, 
                postionsInTerminal[i - 1].transform.localPosition.y - offset , 
                0
            );
        }
    }
    public bool isBottonPosition(int index)
    {
        return postionsInTerminal[index].transform.localPosition.y <= endPosition.transform.localPosition.y; 
    }
    public bool isTopPosition(int index)
    {
        return postionsInTerminal[index].transform.localPosition.y > firstPositionInTerminal.transform.localPosition.y;
    }
    public void SetButton(int position, Button button)
    {
        buttonsInTerminal[position] = button;
    }
    public void SetActualIndexPosition(int position)
    {
        actualPosition = position;        
    }
    public void ActivateScrollBar()
    {
        scrobarIsActive = true;
    }
    public void DeactivateScrollBar()
    {
        scrobarIsActive = false;
    }
    void Update()
    {
        if(scrobarIsActive)
        {
            if(isBottonPosition(postionsInTerminal.Count - 1))
            {
                downScrollBarArrow.SetActive(true);
            }
            if(isTopPosition(0))
            {
                upScrollBarArrow.SetActive(true);
            }
            if(actualPosition == 0)
            {
                upScrollBarArrow.SetActive(false);
            }
            if(actualPosition == postionsInTerminal.Count - 1)
            {
                downScrollBarArrow.SetActive(false);
            }
        }
        else
        {
            upScrollBarArrow.SetActive(false);
            downScrollBarArrow.SetActive(false);
        }
    }
}
