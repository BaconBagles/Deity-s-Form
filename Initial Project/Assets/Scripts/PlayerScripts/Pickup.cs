﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    EnemyController eCont;
    PlayerController pCont;
    GameController gCont;


    public Sprite[] images;

    public int pickupNumber;

    void Start()
    {
        eCont = FindObjectOfType<EnemyController>();
        pCont = FindObjectOfType<PlayerController>();
        gCont = FindObjectOfType<GameController>();

        pickupNumber = gCont.pickupNumber;
        SpriteRenderer render = gameObject.GetComponent<SpriteRenderer>();
        render.sprite = images[pickupNumber];
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        PickupBonus();
        FindObjectOfType<AudioManager>().Play("Pickup");
        gCont.SetUpNextRoom();
        gCont.pointer.isPickup = false;
        gCont.pointer.isDoor = true;
        Destroy(this.gameObject);
    }

    void PickupBonus()
    {
        gCont.StartCoroutine(gCont.PickupGained());
        switch (pickupNumber)
        {
            case 0:
                //attack range up
                pCont.IncreaseAttackRange();
                break;
            case 1:
                //attack speed up
                pCont.attackCooldown *= 0.75f;
                break;
            case 2:
                //attack size up
                pCont.IncreaseAttackSize();
                break;
            case 3:
                //Secondary attack cooldown reduced
                pCont.sndCooldown *= 0.75f;
                break;
            case 4:
                //enemy projectile speed down
                 eCont.Force *= 0.75f;
                break;
            case 5:
                //attack timer up
                eCont.attackTimer += 1;
                break;
            case 6:
                //move speed up
                pCont.speedBonus += 10;
                break;
            case 7:
                //armoured
                pCont.shieldCount = 10;
                break;
            case 8:
                //Attack Knockback/mass up
                pCont.IncreaseKnockback();
                break;
            case 9:
                //secondary attack range up
                pCont.IncreaseSecondarySize();
                break;
            case 10:
                //knockback reduced
                 eCont.Knockback *= 0.75f;
                
                break;
            case 11:
                //Max health increased
                pCont.maxHealth += 10;
                pCont.health += 10;
                break;
        }
    }
}
