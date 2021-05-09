using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSceneControl : MonoBehaviour
{

    public int lastScene;
    public int saveExists;
    public int firstGameComplete;
    public GameObject Resumebutton;
    public GameObject Crown;

    private void Start()
    {
        lastScene = PlayerPrefs.GetInt("lastScene", 3);
        saveExists = PlayerPrefs.GetInt("saveExists", 0);
        firstGameComplete = PlayerPrefs.GetInt("firstGameComplete", 0);

        if (saveExists == 1)
        {
            Resumebutton.SetActive(true);
        }
        else
        {
            Resumebutton.SetActive(false);
        }

        if (firstGameComplete == 1)
        {
            Crown.SetActive(true);
        }
        else
        {
            Crown.SetActive(false);
        }
    }
    public void PlayTutorial()
    {
        PlayerPrefs.SetInt("deleteSave", 0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }

    public void StartGame()
    {
        PlayerPrefs.SetInt("deleteSave", 0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3); //Will load player into main game
    }

    public void ResumeGame()
    {
        if (saveExists == 1)
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
