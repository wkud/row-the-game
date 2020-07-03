using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using BayatGames.SaveGameFree;

public class EndMenu : MonoBehaviour
{
    public AudioSource buttonSound;
    
    public void RestartGame()
    {
        buttonSound.Play();
        SaveGame.Save<bool>("restartInProgress", true);
        SceneManager.LoadScene("MainScene");
    }

    public void QuitGame()
    {
        buttonSound.Play();
        Application.Quit();
        Debug.Log("EndMenu.QuitGame()");
    }
}
