using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public UIManager uiManager;
    public AudioSource buttonSound;
    
    public void StartGame()
    {
        buttonSound.Play();
        uiManager.SetShouldGameStart(true);
    }

    public void QuitGame()
    {
        buttonSound.Play();
        Debug.Log("QuitGame() called");
        Application.Quit();
    }
}
