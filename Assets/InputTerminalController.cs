using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class InputTerminalController : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform firstPositionInTerminal;
    public GameObject rowPrefab;
    public List<GameObject> postionsInTerminal = new List<GameObject>();
    public List<Button> buttonsInTerminal = new List<Button>();
    public int quantity = 0;
    void Start()
    {
        if(firstPositionInTerminal)
        {
            GameObject instantiate = Instantiate(rowPrefab, firstPositionInTerminal.position, Quaternion.identity, firstPositionInTerminal.parent.transform);
            postionsInTerminal.Add(instantiate);
            buttonsInTerminal.Add(null);
            postionsInTerminal[0].GetComponentInChildren<TMP_Text>().text = "";
        }

    }
    public void AddNewPosition()
    {
        postionsInTerminal.Add(Instantiate(rowPrefab, transform));
        buttonsInTerminal.Add(new Button());
        int i = postionsInTerminal.Count - 1;
        postionsInTerminal[i].GetComponentInChildren<TMP_Text>().text = "";
        postionsInTerminal[i].transform.localPosition = new Vector3(firstPositionInTerminal.transform.localPosition.x, postionsInTerminal[i - 1].transform.localPosition.y - 51 , 0);
    }
    public void SetText(int position, string text)
    {
        postionsInTerminal[position].GetComponentInChildren<TMP_Text>().text = text;
    }
    public void SetButton(int position, Button button)
    {
        buttonsInTerminal[position] = button;
    }
    
    void Update()
    {
        
    }
}
