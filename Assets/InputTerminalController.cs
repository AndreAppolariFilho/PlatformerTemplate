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
            postionsInTerminal.Add(instantiate);
            buttonsInTerminal.Add(null);
            postionsInTerminal[0].GetComponentInChildren<TMP_Text>().text = "";
        }

    }
    public void AddNewPosition()
    {
        postionsInTerminal.Add(Instantiate(rowPrefab, firstPositionInTerminal.parent.transform));
        buttonsInTerminal.Add(new Button());
        int i = postionsInTerminal.Count - 1;
        postionsInTerminal[i].GetComponentInChildren<TMP_Text>().text = "";
        postionsInTerminal[i].transform.localPosition = new Vector3(
            firstPositionInTerminal.transform.localPosition.x, 
            postionsInTerminal[i - 1].transform.localPosition.y - 51 , 
            0
        );
    }
    public void SetText(int position, string text)
    {
        postionsInTerminal[position].GetComponentInChildren<TMP_Text>().text = text;
    }
    public void ResetFirstPosition()
    {
        actualFistPositionInTerminal = firstPositionInTerminal;
    }
    public void ReassignToUpPositions()
    {
        postionsInTerminal[0].transform.localPosition = new Vector3(
            postionsInTerminal[0].transform.localPosition.x,
            postionsInTerminal[0].transform.localPosition.y + 51,
            0
        );
        //postionsInTerminal[0].transform.localPosition = actualFistPositionInTerminal.transform.localPosition;
        for(int i = 1; i < postionsInTerminal.Count; i++)
        {
            postionsInTerminal[i].transform.localPosition = new Vector3(
                firstPositionInTerminal.transform.localPosition.x, 
                postionsInTerminal[i - 1].transform.localPosition.y - 51 , 
                0
            );
        }
    }
    public void ReassingToDownPositions()
    {
        Debug.LogError(actualFistPositionInTerminal.transform.localPosition.y - 51);
        postionsInTerminal[0].transform.localPosition = new Vector3(
            postionsInTerminal[0].transform.localPosition.x,
            postionsInTerminal[0].transform.localPosition.y - 51,
            0
        );
        //postionsInTerminal[0].transform.localPosition = actualFistPositionInTerminal.transform.localPosition;
        
        Debug.LogError("Position "+postionsInTerminal[0].transform.localPosition.y);
        for(int i = 1; i < postionsInTerminal.Count; i++)
        {
            postionsInTerminal[i].transform.localPosition = new Vector3(
                firstPositionInTerminal.transform.localPosition.x, 
                postionsInTerminal[i - 1].transform.localPosition.y - 51 , 
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
        return postionsInTerminal[index].transform.localPosition.y >= firstPositionInTerminal.transform.localPosition.y;
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
