using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScript : MonoBehaviour
{

    public void Dead()
    {
        PlayerPrefs.SetInt("tutorialDone", 0);
        PlayerPrefs.Save();
        SceneManager.LoadScene(1);
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

}
