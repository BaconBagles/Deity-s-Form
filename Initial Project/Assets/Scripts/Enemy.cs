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
    public Animator enemyAnim;
    public GameObject projectile;




    // Start is called before the first frame update
    void Start()
    {

        player = GameObject.Find("Player");
        pCont = player.GetComponent<PlayerController>();
        goal = player.transform;
        controller = GameObject.Find("EnemyController");
        controllerScript = controller.GetComponent<EnemyController>();
        health = PlayerPrefs.GetInt("enemyHealth", 5);

        if (gameObject.tag == "basicEnemy")
        {
            enemyAnim.SetInteger("EnemyType", 0);
            spaceBetween = Random.Range(10, 20);
        }
        else if (gameObject.tag == "armourEnemy")
        {
            health += 2;
            enemyAnim.SetInteger("EnemyType", 1);
            spaceBetween = Random.Range(10, 15);
        }
        else
        {
            enemyAnim.SetInteger("EnemyType", 2);
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
            enemyAnim.SetFloat("Horizontal", direction.x);
            enemyAnim.SetFloat("Vertical", direction.y);
            transform.Translate(direction * Time.deltaTime);
        }
        else
        {
            Vector2 direction = transform.position - goal.transform.position;
            enemyAnim.SetFloat("Horizontal", -direction.x);
            enemyAnim.SetFloat("Vertical", -direction.y);
            transform.Translate(direction * Time.deltaTime);
        }

        if (spaceBetween > 20f)
        {
            spaceBetween = 20f;
        }

        if (spaceBetween < 1f)
        {
            spaceBetween = 1f;
        }

        if (health <= 0)
        {
            controllerScript.enemies.Remove(this.gameObject);

            FindObjectOfType<AudioManager>().Play("EnemyDeath");

            Destroy(this.gameObject);
        }
    }

    public void Attack()
    {
        Instantiate(projectile.transform, transform.position, Quaternion.identity);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (pCont.superForm == true)
        {
            health -= 10;
        }
       /* else if (gameObject.tag == "basicEnemy")
        {
            if (other.gameObject.CompareTag("BullSpecial") || other.gameObject.CompareTag("HawkSpecial") || other.gameObject.CompareTag("JackalSpecial"))
            {
                SpecialAttack(other.gameObject.tag);
            }
            if (other.gameObject.CompareTag("basicAttack"))
            {
                health -= 5;
            }
            else
            {
                health -= 1;
                FindObjectOfType<AudioManager>().Play("EnemyDamaged");
            }
        }
        else if (gameObject.tag == "armourEnemy")
        {
            if (other.gameObject.CompareTag("BullSpecial") || other.gameObject.CompareTag("HawkSpecial") || other.gameObject.CompareTag("JackalSpecial"))
            {
                SpecialAttack(other.gameObject.tag);
            }
            if (other.gameObject.CompareTag("APAttack"))
            {
                health -= 7;
            }
            else
            {
                health -= 1;
                FindObjectOfType<AudioManager>().Play("EnemyDamaged");
            }
        }
        else if (this.gameObject.tag == "spikyEnemy")
        {
            if (other.gameObject.CompareTag("BullSpecial") || other.gameObject.CompareTag("HawkSpecial") || other.gameObject.CompareTag("JackalSpecial"))
            {
                SpecialAttack(other.gameObject.tag);
            }
            if (other.gameObject.CompareTag("rangedAttack"))
            {
                health -= 5;
            }
            else
            {
                pCont.health -= 1;
                health -= 1;
                FindObjectOfType<AudioManager>().Play("EnemyDamaged");
            }
        } */

            if (other.gameObject.CompareTag("Respawn"))
            {
                transform.position = new Vector2(0, 0);
            }
        }

    void SpecialAttack(string attackTag)
    {
        if (attackTag == "JackalSpecial")
        {
            health -= 2;
            pCont.health += 1;
        }
        else if (attackTag =="HawkSpecial")
        {
            spaceBetween -= 5f;
        }
        else if (attackTag == "BullSpecial")
        {
            health -= 3;
        }
    }

        IEnumerator Shuffle()
        {
            yield return new WaitForSecondsRealtime(4.0f);
            spaceBetween += Random.Range(-5f, 5f);
            StartCoroutine(Shuffle());
        }
}
