using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public UIManager uiManager;

    public void StartGame()
    {
        uiManager.SetShouldGameStart(true);
    }

    public void QuitGame()
    {
        Debug.Log("QuitGame() called");
        Application.Quit();
    }
}
