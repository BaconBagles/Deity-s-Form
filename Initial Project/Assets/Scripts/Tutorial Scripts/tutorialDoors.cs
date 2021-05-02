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
            collider.isTrigger = false;
            tCont.pointer.isDoor = false;
            tCont.pointer.gameObject.SetActive(false);
            tCont.StartCoroutine(tCont.FadeOut());

            PlayerController player = collision.GetComponent<PlayerController>();
        }
    }
}

