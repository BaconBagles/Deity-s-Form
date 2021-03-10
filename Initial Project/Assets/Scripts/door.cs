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
            gCont.FadeOut();
            BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();
            MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
            collider.isTrigger = false;
            renderer.enabled = false;
            
            gCont.NewRoom();
            
            PlayerController player = collision.GetComponent<PlayerController>();
        }
    }
}
