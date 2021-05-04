using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class tutorialUiController : MonoBehaviour
{
    public TextMeshProUGUI healthText, formText;

    GameObject player;
    playerTutorial pController;

    string formName;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        pController = player.GetComponent<playerTutorial>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (pController.formNumber)
        {
            case 0:
                formName = "JACKAL";
                break;
            case 1:
                formName = "HAWK";
                break;
            case 2:
                formName = "BULL";
                break;
        }

        healthText.text = "Health: " + pController.health;
        formText.text = "Form: " + formName;
    }
}
