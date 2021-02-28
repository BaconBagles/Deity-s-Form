using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSceneControl : MonoBehaviour
{

    public int isTutorialDone;

    private void Start()
    {
        isTutorialDone = PlayerPrefs.GetInt("tutorialDone", 0);
    }

    public void StartGame()
    {
        if(isTutorialDone == 0)
        {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2); // Will load player into tutorial scene, only works once
        }
        else
        {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3); //Will load player into main game
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame() // Will quit the game
    {
        Application.Quit();
    }

}
