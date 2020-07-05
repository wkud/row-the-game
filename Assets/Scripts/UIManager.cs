using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BayatGames.SaveGameFree;

public class UIManager : MonoBehaviour
{
    private Player player;
    private Camera camera;
    public Canvas startMenu;
    public Canvas gameMenu;
    public Canvas endMenu;

    enum GameState
    {
        StartMenu,
        Game,
        EndMenu
    }
    GameState currentGameState;
    bool shouldGameStart = false;
    bool restartInProgress = false;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        camera = FindObjectOfType<Camera>();
    }

    public void SetShouldGameStart(bool newValue)
    {
        shouldGameStart = newValue;
    }

    void Start()
    {
        if (SaveGame.Exists("restartInProgress"))
        {
            restartInProgress = SaveGame.Load<bool>("restartInProgress");
            if (restartInProgress)
            {
                SaveGame.Save<bool>("restartInProgress", false);
            }
        }
        else
        {
            restartInProgress = false;
        }
        currentGameState = GameState.StartMenu;
    }

    void Update()
    {
        bool playerCrashed = player.CrashOccured();

        if (currentGameState == GameState.StartMenu && (shouldGameStart || restartInProgress))
        {
            SetupGameComponents(false, false, false, true, false);
            currentGameState = GameState.Game;
            shouldGameStart = false;
            restartInProgress = false;
        }

        if (currentGameState == GameState.Game && playerCrashed)
        {
            SetupGameComponents(true, true, false, false, true);
            currentGameState = GameState.EndMenu;
            shouldGameStart = false;
            restartInProgress = false;
            AudioManager.Instance.Stop("music");
            AudioManager.Instance.Play("game over");
        }
    }

    void SetupGameComponents(bool playerSkip, bool cameraSkip, bool startMenuVisible, bool gameUIVisible,
        bool endMenuVisible)
    {
        player.SetSkipUpdate(playerSkip);
        camera.SetSkipUpdate(cameraSkip);
        startMenu.gameObject.SetActive(startMenuVisible);
        gameMenu.gameObject.SetActive(gameUIVisible);
        endMenu.gameObject.SetActive(endMenuVisible);
    }
}
