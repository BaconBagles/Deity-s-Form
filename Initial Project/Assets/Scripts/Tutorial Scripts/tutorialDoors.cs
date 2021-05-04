using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialDoors : MonoBehaviour
{
    tutorialScript tCont;

    void Start()
    {
        tCont = FindObjectOfType<tutorialScript>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Collider2D collider = gameObject.GetComponent<Collider2D>();

            tCont.pointer.isDoor = false;
            tCont.pointer.gameObject.SetActive(false);
            tCont.StartCoroutine(tCont.FadeOut());


            playerTutorial player = collision.GetComponent<playerTutorial>();
            collider.isTrigger = false;
        }
    }
}

