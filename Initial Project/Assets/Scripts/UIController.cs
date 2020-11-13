using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Text healthText;
    public Text formText;

    GameObject player;
    PlayerController pController;

    string formName;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        pController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(pController.formNumber)
        {
            case 0:
                formName = "BASE";
                break;
            case 1:
                formName = "CRUSHER";
                break;
            case 2:
                formName = "THRESHER";
                break;
        }

        healthText.text = "HEALTH: " + pController.health;
        formText.text = "FORM: " + formName;
    }
}
