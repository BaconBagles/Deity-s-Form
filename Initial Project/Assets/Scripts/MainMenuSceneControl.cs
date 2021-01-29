using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSceneControl : MonoBehaviour
{
    
    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2); // Will load player into next scene, (only works in menu)
    }

    public void QuitGame() // Will quit the game
    {
        Application.Quit();
    }

}
