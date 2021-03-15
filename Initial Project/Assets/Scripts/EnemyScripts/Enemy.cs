using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour
{
    GameObject player;
    Transform goal;
   
    public float spaceBetween;
    PlayerController pCont;
    public EnemyHealthBar healthBar;
    public float health;
    float maxHealth;
    private bool isDead;
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

    public EnemyController eCont;

    Collider2D agentCollider;
    public Collider2D AgentCollider { get { return agentCollider; } }

    private void Awake()
    {
        maxHealth = PlayerPrefs.GetInt("enemyHealth", 5);
        health = maxHealth;
        currentForce = 350;
        currentDamage = 2;
        currentSize = 0f;
        currentKnockback = 175;
    }

    void Start()
    {
        agentCollider = GetComponent<Collider2D>();
        Audio = FindObjectOfType<AudioManager>();
        eCont = FindObjectOfType<EnemyController>();
        player = GameObject.Find("Player");
        pCont = player.GetComponent<PlayerController>();
        goal = player.transform;
        isDead = false;

        rb = GetComponent<Rigidbody2D>();

        if (gameObject.tag == "basicEnemy")
        {
            enemyAnim.SetInteger("EnemyType", 2);
            spaceBetween = Random.Range(10, 20);
        }
        else if (gameObject.tag == "armourEnemy")
        {
            health += 2;
            enemyAnim.SetInteger("EnemyType", 0);
            spaceBetween = Random.Range(10, 15);
        }
        else
        {
            enemyAnim.SetInteger("EnemyType", 1);
            spaceBetween = Random.Range(5, 10);
        }
        StartCoroutine(Shuffle());
    }

  /*  public void Initialize(EnemyController eContFlock)
    {
        ECont = eContFlock;
    } */

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


        healthBar.SetHealth(health, maxHealth);

        if (health <= 0)
        {
            StartCoroutine(Death());
        }
    }

   /* public void Move(Vector2 velocity)
    {
        if(isDead != true)
        {
            transform.up = velocity; //THE PROBLEM LINE
            transform.position += (Vector3)velocity * Time.deltaTime;
        }
        
    } */

    public void Attack()
    {
        if(isDead != true)
        {
            Audio.Play("EnemyAttack");
            GameObject enemyProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
            projectileScript proj = enemyProjectile.GetComponent<projectileScript>();
            proj.force = currentForce;
            proj.damage = currentDamage;
            proj.bossSize = currentSize;
            proj.knockbackPower = currentKnockback;
        }
       
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

    public IEnumerator Death()
    {
        isDead = true;
        enemyAnim.SetTrigger("Death");
        FindObjectOfType<AudioManager>().Play("EnemyDeath");

        yield return new WaitForSecondsRealtime(1f);

        float randomNum = Random.Range(0, 100);
        if (randomNum > 95)
        {
            Instantiate(healthPickup, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), Quaternion.identity);
        }
        eCont.enemies.Remove(this);

        if (isBoss == true)
        {
            eCont.gameController.roomComplete = true;
        }

        Destroy(this.gameObject);
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
            health -= 4;
            FindObjectOfType<AudioManager>().Play("EnemyDamaged");
            StartCoroutine(Knockback(2f, 200f, other.gameObject.transform));

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
