using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameMenuClose : MonoBehaviour, ICancelHandler
{
    public bool changeScene;
    public string sceneName;

    public GameObject oldPanel;
    public GameObject nextPanel;
    public Selectable nextCursorPosition;
    public void OnCancel(BaseEventData eventData)
    {
        Debug.Log("Cancel");
        if(changeScene)
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            if(nextPanel != null)
            {
                nextPanel.SetActive(true);
            }
            if(nextCursorPosition != null)
            {
                nextCursorPosition.Select();
            }
            if(oldPanel != null)
            {
                oldPanel.SetActive(false);
            }
        }
    }
}
