using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    GameObject player;
    Transform goal;
    GameObject controller;
    EnemyController controllerScript;
    float spaceBetween;
    PlayerController pCont;
    public int health;



    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        pCont = player.GetComponent<PlayerController>();
        goal = player.transform;
        controller = GameObject.Find("EnemyController");
        controllerScript = controller.GetComponent<EnemyController>();
        health = 5;

        if (gameObject.tag == "basicEnemy")
        {
            spaceBetween = Random.Range(10, 20);
        }
        else if (gameObject.tag == "armourEnemy")
        {
            spaceBetween = Random.Range(10, 15);
        }
        else
        {
            spaceBetween = Random.Range(5, 15);
        }
        StartCoroutine(Shuffle());
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(goal.position, transform.position) >= spaceBetween)
        {
            Vector2 direction = goal.position - transform.position;
            transform.Translate(direction * Time.deltaTime);
        }
        else
        {
            Vector2 direction = transform.position - goal.transform.position;
            transform.Translate(direction * Time.deltaTime);
        }

        if (spaceBetween > 20f)
        {
            spaceBetween = 20f;
        }

        if (health <=0)
        {
            controllerScript.enemies.Remove(this.gameObject);

            FindObjectOfType<AudioManager>().Play("EnemyDeath");

            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (gameObject.tag == "basicEnemy")
        {
            if (other.gameObject.CompareTag("basicAttack"))
            {
                health -= 5;
            }
            else
            {
                health -= 1;
            }
        }
        else if (gameObject.tag == "armourEnemy")
        {
            if (other.gameObject.CompareTag("APAttack"))
            {
                health -= 5;
            }
            else
            {
                health -= 1;
            }
        }
        else if(this.gameObject.tag == "spikyEnemy")
        {
            if (other.gameObject.CompareTag("rangedAttack"))
            {
                health -= 5;
            }
            else
            {
                pCont.health -= 1;
                health -= 1;
            }
        }

        if (other.gameObject.CompareTag("Respawn"))
        {
            transform.position = new Vector2(0,0);
        }
    }

    IEnumerator Shuffle()
    {
        yield return new WaitForSecondsRealtime(5.0f);
        spaceBetween += Random.Range(-5f, 5f);
        StartCoroutine(Shuffle());
    }
}
