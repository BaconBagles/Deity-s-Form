using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthGain : MonoBehaviour
{
    PlayerController pCont;
    GameController gCont;

    public Sprite image;
    
    void Start()
    {
        pCont = FindObjectOfType<PlayerController>();
        gCont = FindObjectOfType<GameController>();
        SpriteRenderer render = gameObject.GetComponent<SpriteRenderer>();
        render.sprite = image;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Heal();
        FindObjectOfType<AudioManager>().Play("Pickup");
        Destroy(gameObject);
    }

    void Heal()
    {
        pCont.health += 4;
        gCont.StartCoroutine(gCont.HealthAdded());
    }
}
