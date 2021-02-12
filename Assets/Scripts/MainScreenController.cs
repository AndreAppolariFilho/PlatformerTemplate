using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainScreenController : MonoBehaviour
{
    public void LoadContinue()
    {
        SceneManager.LoadScene("LoadSelect");
    }
    public void LoadNewGame()
    {
        SceneManager.LoadScene("SlotSelect");  
    }
    public void LoadSettings()
    {
        SceneManager.LoadScene("Options");  
    }
    public void QuitGame()
    {

    }
}
