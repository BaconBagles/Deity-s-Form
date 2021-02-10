using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class tutorialScript : MonoBehaviour
{
    public GameObject[] tutorialText;
    public GameObject tutorialParent;
    public int currentTip;

    private void Awake()
    {
        Time.timeScale = 0f;
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
        tutorialParent.SetActive(false);
    }
}
