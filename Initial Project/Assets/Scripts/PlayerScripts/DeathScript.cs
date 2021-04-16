using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScript : MonoBehaviour
{
    public int LastScene;

    private void Start()
    {

    }

    public void Dead()
    {
        PlayerPrefs.SetInt("tutorialDone", 0);
        LastScene = PlayerPrefs.GetInt("lastScene", 0);
        PlayerPrefs.Save();
        SceneManager.LoadScene(1);
    }

    public void Restart()
    {
        PlayerPrefs.SetInt("deleteSave", 0);
        PlayerPrefs.SetInt("saveExists", 0);
        PlayerPrefs.SetInt("lastScene", 0);
        PlayerPrefs.Save();
        SceneManager.LoadScene(0);
    }

    public void Continue()
    {
        PlayerPrefs.SetInt("deleteSave", 0);
        PlayerPrefs.SetInt("saveExists", 0);
        PlayerPrefs.SetInt("lastScene", 0);
        PlayerPrefs.Save();
        SceneManager.LoadScene(3);
    }

    public void ContinueWithGear()
    {
        SceneManager.LoadScene(3);
    }
}
