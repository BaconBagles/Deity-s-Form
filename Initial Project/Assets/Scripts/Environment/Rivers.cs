using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rivers : MonoBehaviour
{

    bool isRiver;
    bool isLava;

    float time;
    float currentTime;

    bool playerColliding;
    
    public float slowdown;

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.tag == "River")
        {
            isRiver = true;
        }

        if (gameObject.tag == "Lava")
        {
            isLava = true;
        }

        time = 1;

        slowdown = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerColliding && currentTime <= 0)
        {
            if (isLava)
            {
                DamagePlayer();
            }
            currentTime = time;
        }

        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            playerColliding = true;

            if (isRiver)
            {
                PlayerController player = PlayerController.FindObjectOfType<PlayerController>();
                player.moveSpeed  = player.moveSpeed/2f;
            }
        }

        if(isLava == true)
        {
            if (coll.gameObject.tag == "basicEnemy")
            {
                Enemy enemy = coll.GetComponent<Enemy>();
                enemy.inLava = true;
            }

            if (coll.gameObject.tag == "armourEnemy")
            {
                Enemy enemy = coll.GetComponent<Enemy>();
                enemy.inLava = true;
            }

            if (coll.gameObject.tag == "spikyEnemy")
            {
                Enemy enemy = coll.GetComponent<Enemy>();
                enemy.inLava = true;
            }
        }
        
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            playerColliding = false;
            if(isRiver)
            {
                PlayerController player = PlayerController.FindObjectOfType<PlayerController>();
                switch (player.formNumber)
                {
                    case 0:
                        player.moveSpeed = 20 + player.speedBonus;
                        break;
                    case 1:
                        player.moveSpeed = 15 + player.speedBonus;
                        break;
                    default:
                        player.moveSpeed = 15 + player.speedBonus;
                        break;
                }
            }
        }
        if (isLava == true)
        {
            if (coll.gameObject.tag == "basicEnemy")
            {
                Enemy enemy = coll.GetComponent<Enemy>();
                enemy.inLava = false;
            }

            if (coll.gameObject.tag == "armourEnemy")
            {
                Enemy enemy = coll.GetComponent<Enemy>();
                enemy.inLava = false;
            }

            if (coll.gameObject.tag == "spikyEnemy")
            {
                Enemy enemy = coll.GetComponent<Enemy>();
                enemy.inLava = false;
            }
        }
    }

    void DamagePlayer()
    {
        PlayerController player = PlayerController.FindObjectOfType<PlayerController>();
        player.health -= 4;
    }
}
