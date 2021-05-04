using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialTipsScript : MonoBehaviour
{
    public GameObject groundText;
    public GameObject previousTip;

    public GameObject nextDots;

    public tutorialScript tutorialWave;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == ("Player"))
        {
            tutorialWave.TutorialStep();
          //  tutorialWave.tutorialStage++;
            groundText.SetActive(true);
            previousTip.SetActive(false);

        }
    }
}
