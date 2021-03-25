using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyT : MonoBehaviour
{
    GameObject player;
    Transform goal;
    GameObject controller;
    Vector2 raycastOrigin;
    Vector3 raycastDirection;
    public int enemyType;
    tutorialScript controllerScript;
    public float spaceBetween;
    PlayerController pCont;
    public EnemyHealthBar healthBar;
    public float health;
    public int maxHealth;
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
    string secondarytag;
    int latDirRnd;
    bool lateralDirection;

    Collider2D agentCollider;
    public Collider2D AgentCollider { get { return agentCollider; } }



    private void Awake()
    {
        health = PlayerPrefs.GetInt("enemyHealth", 5);
        health = maxHealth;
        currentForce = 5.5f;
        currentDamage = 2;
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
        controller = GameObject.Find("TUTORIAL");
        controllerScript = controller.GetComponent<tutorialScript>();

        latDirRnd = Random.Range(0, 2);

        rb = GetComponent<Rigidbody2D>();

        if (enemyType == 0)
        { //Base
            enemyAnim.SetInteger("EnemyType", 2);
            spaceBetween = Random.Range(10, 20);
        }
        else if (enemyType == 1)
        { //heavy
            maxHealth += 2;
            health = maxHealth;
            enemyAnim.SetInteger("EnemyType", 0);
            spaceBetween = Random.Range(10, 15);
        }
        else if (enemyType == 2)
        { //spike
            enemyAnim.SetInteger("EnemyType", 1);
            spaceBetween = Random.Range(5, 10);
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

        if (lateralDirection == true)
        {
            transform.RotateAround(goal.transform.position, Vector3.forward, 50 * Time.deltaTime);
        }
        else
        {
            transform.RotateAround(goal.transform.position, Vector3.forward, -50 * Time.deltaTime);
        }
        transform.rotation = Quaternion.identity;

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

        if (latDirRnd == 0)
        {
            lateralDirection = true;
        }
        else
        {
            lateralDirection = false;
        }
        
        
    }

    void FixedUpdate()
    {
        raycastOrigin = new Vector2(transform.position.x, transform.position.y - 2);
        raycastDirection = new Vector2(raycastOrigin.x + Random.Range(-180, 180), raycastOrigin.y + Random.Range(-180, 180));
        //raycastDirection = transform.position - goal.transform.position;
        //Debug.DrawRay(raycastOrigin, raycastDirection, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, raycastDirection, 5f);

        if (hit.collider)
        {
            if (hit.collider.tag == "Wall" && hit.distance < 2f)
            {
                spaceBetween -= 2;
            }
            //Debug.Log("Hit: " + hit.collider.tag);// + ", distance: " + hit.distance);
            // Draw line in Scene view from `origin` to `hit.point` for 3 seconds.
            //Debug.DrawLine(raycastOrigin, hit.point, Color.red, 3f);
        }
    }

    public void Attack()
    {
        if (isDead != true)
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
        controllerScript.enemies.Remove(this);

        Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Respawn"))
        {
            transform.position = new Vector2(0, 0);
        }
        /*if (pCont.superForm == true)
        {
            health -= 10;
        }*/
        else if (other.gameObject.CompareTag("JackalSpecial"))
        {
            health -= 2;
            pCont.health += 1;
            FindObjectOfType<AudioManager>().Play("EnemyDamaged");
        }
        else if (other.gameObject.CompareTag("BullSpecial"))
        {
            health -= 3;
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
        latDirRnd = Random.Range(0, 2);
        StartCoroutine(Shuffle());
    }
}
