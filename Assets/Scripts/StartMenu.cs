using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public UIManager uiManager;
    
    public void StartGame()
    {
        AudioManager.Instance.Play("button click");
        AudioManager.Instance.Play("music");
        uiManager.SetShouldGameStart(true);
    }

    public void QuitGame()
    {
        AudioManager.Instance.Play("button click");
        Debug.Log("QuitGame() called");
        Application.Quit();
    }
}
