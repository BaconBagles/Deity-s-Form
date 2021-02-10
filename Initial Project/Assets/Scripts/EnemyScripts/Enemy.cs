using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    GameObject player;
    Transform goal;
    GameObject controller;
    EnemyController controllerScript;
    public float spaceBetween;
    PlayerController pCont;
    public int health;
    public Animator enemyAnim;
    public GameObject projectile;
    public int currentDamage;
    public float currentForce;
    public float currentSize;
    public GameObject healthPickup;


    private void Awake()
    {
        health = PlayerPrefs.GetInt("enemyHealth", 5);
        currentForce = 400;
        currentDamage = 1;
        currentSize = 0f;
    }

    // Start is called before the first frame update
    void Start()
    {

        player = GameObject.Find("Player");
        pCont = player.GetComponent<PlayerController>();
        goal = player.transform;
        controller = GameObject.Find("EnemyController");
        controllerScript = controller.GetComponent<EnemyController>();
        

        if (gameObject.tag == "basicEnemy")
        {
            enemyAnim.SetInteger("EnemyType", 2);
            spaceBetween = Random.Range(15, 25);
        }
        else if (gameObject.tag == "armourEnemy")
        {
            health += 2;
            enemyAnim.SetInteger("EnemyType", 0);
            spaceBetween = Random.Range(15, 20);
        }
        else
        {
            enemyAnim.SetInteger("EnemyType", 1);
            spaceBetween = Random.Range(10, 15);
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

        if (spaceBetween > 30f)
        {
            spaceBetween = 30f;
        }

        if (spaceBetween < 0f)
        {
            spaceBetween = 0f;
        }

        if (health <= 0)
        {
            float randomNum = Random.Range(0, 100);
            if (randomNum > 95)
            {
                Instantiate(healthPickup, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), Quaternion.identity);
            }
            controllerScript.enemies.Remove(this.gameObject);
            FindObjectOfType<AudioManager>().Play("EnemyDeath");
            Destroy(this.gameObject);
        }
    }

    public void Attack()
    {
        GameObject enemyProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
        projectileScript proj = enemyProjectile.GetComponent<projectileScript>();
        proj.force = currentForce;
        proj.damage = currentDamage;
        proj.bossSize = currentSize;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Respawn"))
        {
            transform.position = new Vector2(0, 0);
        }
        if (pCont.superForm == true)
        {
            health -= 10;
        }
        else if (other.gameObject.CompareTag("JackalSpecial"))
        {
            health -= 1;
            pCont.health += 1;
            FindObjectOfType<AudioManager>().Play("EnemyDamaged");
        }
        else if (other.gameObject.CompareTag("BullSpecial"))
        {
            health -= 1;
            spaceBetween += 5;
        }
        else if (other.gameObject.CompareTag("HawkSpecial"))
        {
            spaceBetween = 0;
        }


    }


    IEnumerator Shuffle()
    {
        yield return new WaitForSecondsRealtime(4.0f);
        if (spaceBetween < 5f)
        {
            spaceBetween += 10f;
        }
        else
        {
            spaceBetween += Random.Range(-7.5f, 7.5f);
        }
        StartCoroutine(Shuffle());
    }
}
