using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    GameObject eContObj;
    EnemyController eCont;
    GameObject player;
    PlayerController pCont;
    GameObject gContObj;
    GameController gCont;

    public Sprite[] images;

    public int pickupNumber;

    void Start()
    {
        eContObj = GameObject.Find("EnemyController");
        eCont = eContObj.GetComponent<EnemyController>();
        player = GameObject.Find("Player");
        pCont = player.GetComponent<PlayerController>();
        gContObj = GameObject.Find("GameController");
        gCont = gContObj.GetComponent<GameController>();

        pickupNumber = gCont.pickupNumber;
        SpriteRenderer render = gameObject.GetComponent<SpriteRenderer>();
        render.sprite = images[pickupNumber];
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PickupBonus();
            FindObjectOfType<AudioManager>().Play("Pickup");
            gCont.SetUpNextRoom();
            Destroy(this.gameObject);
        }
    }

    void PickupBonus()
    {
        switch (pickupNumber)
        {
            case 0:
                pCont.health += 5;
                break;
            case 1:
                pCont.IncreaseAttackSize();
                break;
            case 2:
                pCont.speedBonus += 1.5f;
                pCont.moveSpeed += 1.5f;
                break;
            case 3:
                pCont.maxHealth += 5;
                pCont.health += 5;
                break;
            case 4:
                pCont.shieldCount = 2;
                break;
        }
    }
}
