using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    GameObject player;
    Transform goal;
    GameObject controller;
    public EnemyController controllerScript;
    float spaceBetween;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        goal = player.transform;
        controller = GameObject.Find("EnemyController");
        controllerScript = controller.GetComponent<EnemyController>();

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
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("basicAttack"))
        {
            if (gameObject.tag == "basicEnemy")
            {
                controllerScript.enemies.Remove(this.gameObject);
                Destroy(this.gameObject);
            }
        }
        else if (other.gameObject.CompareTag("APAttack"))
        {
            if (gameObject.tag == "armourEnemy")
            {
                controllerScript.enemies.Remove(this.gameObject);
                Destroy(this.gameObject);
            }
        }
        else if(other.gameObject.CompareTag("rangedAttack"))
        {
            if (this.gameObject.tag == "spikyEnemy")
            {
                controllerScript.enemies.Remove(this.gameObject);
                Destroy(this.gameObject);
            }
        }
    }

    IEnumerator Shuffle()
    {
        yield return new WaitForSecondsRealtime(5.0f);
        spaceBetween += Random.Range(-5f, 5f);
        StartCoroutine(Shuffle());
    }
}
