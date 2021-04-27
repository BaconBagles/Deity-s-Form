using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class door : MonoBehaviour
{
    GameController gCont;

    void Start()
    {
        gCont = FindObjectOfType<GameController>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Collider2D collider = gameObject.GetComponent<Collider2D>();
            collider.isTrigger = false;
            gCont.pointer.isDoor = false;
            gCont.pointer.gameObject.SetActive(false);
            gCont.StartCoroutine(gCont.FadeOut());
            
            PlayerController player = collision.GetComponent<PlayerController>();
        }
    }
}
