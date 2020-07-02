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
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("EndMenu.QuitGame()");
    }
}
