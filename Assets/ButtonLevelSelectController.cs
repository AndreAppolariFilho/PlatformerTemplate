using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

using TMPro;

public class ButtonLevelSelectController : MonoBehaviour

{
    // Start is called before the first frame update
    public Color colorSelected;
    public Color colorNormal;
    Button button;
    void Start()
    {
        //button = this.GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        
        TMP_Text text = (TMP_Text)this.GetComponentInChildren<TMP_Text>();
        if(text)
        {
           if(EventSystem.current.currentSelectedGameObject==this.gameObject)
           {
               text.color = colorSelected;
           }
           else{
               text.color = colorNormal;
           }
        }
        
    }
}
