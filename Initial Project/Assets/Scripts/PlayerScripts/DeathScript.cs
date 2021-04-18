using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScript : MonoBehaviour
{
    public int LastScene;

    public void Start()
    {
        LastScene = PlayerPrefs.GetInt("lastScene", 3);
    }

    public void Dead()
    {
        PlayerPrefs.SetInt("tutorialDone", 0);
        SceneManager.LoadScene(1);
    }

    public void Restart()
    {
        PlayerPrefs.SetInt("deleteSave", 0);
        PlayerPrefs.SetInt("saveExists", 0);
        PlayerPrefs.SetInt("lastScene", 3);
        PlayerPrefs.Save();
        SceneManager.LoadScene(0);
    }

    public void Continue()
    {
        PlayerPrefs.SetInt("deleteSave", 0);
        PlayerPrefs.SetInt("saveExists", 0);
        PlayerPrefs.SetInt("lastScene", 3);
        PlayerPrefs.Save();
        SceneManager.LoadScene(3);
    }

    public void ContinueWithGear()
    {
        PlayerPrefs.SetInt("lastScene", LastScene);
        PlayerPrefs.SetInt("deleteSave", 2);
        PlayerPrefs.SetInt("saveExists", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene(3);
    }
}
