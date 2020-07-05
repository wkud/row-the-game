using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using BayatGames.SaveGameFree;

public class EndMenu : MonoBehaviour
{
    public void RestartGame()
    {
        SaveGame.Save<bool>("restartInProgress", true);
        SceneManager.LoadScene("MainScene");
        AudioManager.Instance.Play("button click");
        AudioManager.Instance.Play("music");
    }

    public void QuitGame()
    {
        AudioManager.Instance.Play("button click");
        Application.Quit();
        Debug.Log("EndMenu.QuitGame()");
    }
}
