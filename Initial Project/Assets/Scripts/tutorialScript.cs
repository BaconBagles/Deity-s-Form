using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class tutorialScript : MonoBehaviour
{
    public OptionsMenu Options;
    public GameObject[] tutorialText;
    public GameObject tutorialParent;
    public int currentTip;

    private void Awake()
    {
        Time.timeScale = 0f;
        Options.GameIsPaused = true;
    }

    public void NextTip()
    {
        if(currentTip != tutorialText.Length -1)
        {
            tutorialText[currentTip].SetActive(false);
            tutorialText[currentTip + 1].SetActive(true);
            currentTip++;
        }
       else
        {
            FinishTutorial();
        }
    }

    public void FinishTutorial()
    {
        Time.timeScale = 1f;
        Options.GameIsPaused = false;
        tutorialParent.SetActive(false);
    } 


}
