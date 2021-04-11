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
            BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();
            MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
            collider.isTrigger = false;
            renderer.enabled = false;
            gCont.pointer.isDoor = false;
            gCont.pointer.gameObject.SetActive(false);
            gCont.StartCoroutine(gCont.FadeOut());
            
            PlayerController player = collision.GetComponent<PlayerController>();
        }
    }
}
