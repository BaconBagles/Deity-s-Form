using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour
{
    GameObject player;
    Transform goal;
   
   // public float spaceBetween;
    PlayerController pCont;
    public float health;
    public Animator enemyAnim;
    public GameObject projectile;
    public int currentDamage;
    public float currentForce;
    public float currentSize;
    public float currentKnockback;
    public GameObject healthPickup;
    public Rigidbody2D rb;
    AudioManager Audio;
    public bool isBoss;

    //flockingAI stuff
    EnemyController ECont;
    public EnemyController eCont { get { return ECont; } }

    Collider2D agentCollider;
    public Collider2D AgentCollider { get { return agentCollider; } }

    private void Awake()
    {
        health = PlayerPrefs.GetInt("enemyHealth", 5);
        currentForce = 350;
        currentDamage = 1;
        currentSize = 0f;
        currentKnockback = 175;
    }

    // Start is called before the first frame update
    void Start()
    {
        agentCollider = GetComponent<Collider2D>();
        Audio = FindObjectOfType<AudioManager>();
        player = GameObject.Find("Player");
        pCont = player.GetComponent<PlayerController>();
        goal = player.transform;

        rb = GetComponent<Rigidbody2D>();

        if (gameObject.tag == "basicEnemy")
        {
            enemyAnim.SetInteger("EnemyType", 2);
          //  spaceBetween = Random.Range(10, 20);
        }
        else if (gameObject.tag == "armourEnemy")
        {
            health += 2;
            enemyAnim.SetInteger("EnemyType", 0);
           // spaceBetween = Random.Range(10, 15);
        }
        else
        {
            enemyAnim.SetInteger("EnemyType", 1);
          //  spaceBetween = Random.Range(5, 10);
        }
        //StartCoroutine(Shuffle());
    }

    public void Initialize(EnemyController eContFlock)
    {
        ECont = eContFlock;
    }

    // Update is called once per frame
    void Update()
    {
        /* if (Vector2.Distance(goal.position, transform.position) >= 5)
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
          */
        if (health <= 0)
        {
            float randomNum = Random.Range(0, 100);
            if (randomNum > 95)
            {
                Instantiate(healthPickup, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), Quaternion.identity);
            }
            eCont.enemies.Remove(this);
            FindObjectOfType<AudioManager>().Play("EnemyDeath");
            if (isBoss == true)
            {
                eCont.gameController.roomComplete = true;
            }
            Destroy(this.gameObject);
        }
    }

    public void Move(Vector2 velocity)
    {
        transform.up = velocity; //be prepared to crush this.
        transform.position += (Vector3)velocity * Time.deltaTime;
    }

    public void Attack()
    {
        Audio.Play("EnemyAttack");
        GameObject enemyProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
        projectileScript proj = enemyProjectile.GetComponent<projectileScript>();
        proj.force = currentForce;
        proj.damage = currentDamage;
        proj.bossSize = currentSize;
        proj.knockbackPower = currentKnockback;
    }

    public IEnumerator Knockback(float knockBackDuration, float knockbackPower, Transform obj)
    {
        float timer = 0;

        while (knockBackDuration > timer)
        {
            timer += Time.deltaTime;
            Vector2 direction = (obj.transform.position - this.transform.position).normalized;
            rb.AddForce(-direction * knockbackPower);
        }

        yield return 0;
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
            health -= 3;
            pCont.health += 1;
            FindObjectOfType<AudioManager>().Play("EnemyDamaged");
        }
        else if (other.gameObject.CompareTag("BullSpecial"))
        {
            health -= 3;
          //  spaceBetween += 5;
        }
        else if (other.gameObject.CompareTag("HawkSpecial"))
        {
           // spaceBetween = 0;
        }


    }


  /*  IEnumerator Shuffle()
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
    } */
}
