using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseGameController : MonoBehaviour
{
    public void ReturnMainScreen()
    {
        SceneManager.LoadScene("MainScreen");
    }
    public void ReturnToLevelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
    }
    public void ResetLevel(string level)
    {
        FindObjectOfType<GameManager>().ResumeGame();
        Application.LoadLevel(Application.loadedLevel);
    }
}
