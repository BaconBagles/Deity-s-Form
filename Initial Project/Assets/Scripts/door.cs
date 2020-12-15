using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameController gCont = FindObjectOfType<GameController>();
            gCont.roomComplete = false;
            gCont.RandomRoom();
            gCont.pickupSpawned = false;
            collision.gameObject.transform.position = new Vector2(0, 0);
        }
    }
}
