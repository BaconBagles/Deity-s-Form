using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSceneControl : MonoBehaviour
{

    public int isTutorialDone;
    public int lastScene;
    public int saveExists;
    public GameObject Resumebutton;

    private void Start()
    {
        isTutorialDone = PlayerPrefs.GetInt("tutorialDone", 0);
        lastScene = PlayerPrefs.GetInt("lastScene", 3);
        saveExists = PlayerPrefs.GetInt("saveExists", 0);
        if (saveExists == 1)
        {
            Resumebutton.SetActive(true);
        }
        else
        {
            Resumebutton.SetActive(false);
        }
    }

    public void StartGame()
    {
        PlayerPrefs.SetInt("deleteSave", 0);

        /*if (isTutorialDone == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2); // Will load player into tutorial scene, only works once
        }
        else if (isTutorialDone == 1)
        { */
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3); //Will load player into main game
   //     }
    }

    public void ResumeGame()
    {
        if(saveExists == 1)
        {
            SceneManager.LoadScene(lastScene);
        }
        else
        {
            return;
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
