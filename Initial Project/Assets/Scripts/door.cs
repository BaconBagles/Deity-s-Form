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
            GameController gCont = FindObjectOfType<GameController>();

            gCont.NewRoom();

            //collision.gameObject.transform.position = new Vector2(0, 0);
            PlayerController player = collision.GetComponent<PlayerController>();
        }
    }
}
