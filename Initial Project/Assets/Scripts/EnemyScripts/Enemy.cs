using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour
{

    GameObject player;
    Transform goal;
    Vector2 raycastOrigin;
    Vector3 raycastDirection;
    int enemysecondaryType;
    public int switchthreshold;
    public int enemyType;
    public float spaceBetween;
    PlayerController pCont;
    public EnemyHealthBar healthBar;
    public int health;
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
    public bool isBoss;
    public int bossNum;
    BossEnemy boss;
    string secondarytag;
    int latDirRnd;
    bool lateralDirection;
    public EnemyController eCont;
    public SpriteRenderer projRenderer;
    public Sprite[] projSprite;
    public int currentSprite;

    Material mat;
    float fade;
    bool isDying;

    float time;
    float currentTime;
    public bool inLava;
    

    Collider2D agentCollider;
    public Collider2D AgentCollider { get { return agentCollider; } }

    private void Awake()
    {
        maxHealth = PlayerPrefs.GetInt("enemyHealth", 5);
        health = maxHealth;
        currentDamage = 2;
        currentSize = 0f;
        currentSprite = 0;

    }

    void Start()
    {
        agentCollider = GetComponent<Collider2D>();
        Audio = FindObjectOfType<AudioManager>();
        eCont = FindObjectOfType<EnemyController>();
        player = GameObject.Find("Player");
        pCont = player.GetComponent<PlayerController>();
        boss = gameObject.GetComponent<BossEnemy>();
        currentForce = eCont.Force;
        currentKnockback = eCont.Knockback;
        goal = player.transform;
        isDead = false;

        latDirRnd = Random.Range(0, 2);

        fade = 1f;
        mat = GetComponent<SpriteRenderer>().material;
        mat.SetFloat("_Fade", fade);

        time = 1;
        currentTime = time;

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
        else if (enemyType == 3)
        { //heavybase
            maxHealth += 7;
            health = maxHealth;
            switchthreshold = (maxHealth / 2);
            enemyAnim.SetInteger("EnemyType", 0);
            enemysecondaryType = 2;
            secondarytag = "basicEnemy";
            spaceBetween = Random.Range(10, 15);
        }
        else if (enemyType == 4)
        { //spikebase
            maxHealth += 5;
            health = maxHealth;
            switchthreshold = (maxHealth / 2);
            enemyAnim.SetInteger("EnemyType", 1);
            enemysecondaryType = 2;
            secondarytag = "basicEnemy";
            spaceBetween = Random.Range(5, 10);
        }
        else if (enemyType == 5)
        { //spikeheavy
            maxHealth += 5;
            health = maxHealth;
            switchthreshold = (maxHealth / 2);
            enemyAnim.SetInteger("EnemyType", 1);
            enemysecondaryType = 0;
            secondarytag = "armourEnemy";
            spaceBetween = Random.Range(5, 10);
        }

        StartCoroutine(Shuffle());
    }

    void Update()
    {
        
        if (isDead != true)
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
        }

        if (health <= switchthreshold && enemyType > 2)
        {
            gameObject.tag = secondarytag;
            enemyAnim.SetInteger("EnemyType", enemysecondaryType);
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

        if (health <= 0 && isDead != true)
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

        if (isDying == true)
        {
            fade -= Time.deltaTime;
        }
        mat.SetFloat("_Fade", fade);

        if (inLava == true)
        {
            currentTime -= Time.deltaTime;
        }

        if (currentTime < 0)
        {
            health -= 2;
            currentTime = time;
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

    public IEnumerator Attack()
    {
        GameObject enemyProjectile;
        projectileScript proj;
        if (isDead != true)
        {
            if (isBoss != true)
            {
                Audio.Play("EnemyAttack");
                enemyProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
                proj = enemyProjectile.GetComponent<projectileScript>();
                projRenderer = proj.GetComponent<SpriteRenderer>();
                projRenderer.sprite = projSprite[currentSprite];
                proj.damage = currentDamage;
                proj.bossSize = currentSize;
                proj.knockbackPower = currentKnockback;
                yield return new WaitForSeconds(0.1f);
                proj.force = currentForce;
            }
            else
            {
                switch(bossNum)
                {
                    case 0:
                        for (int i = 0; i < 2; i++)
                        {
                            Audio.Play("EnemyAttack");
                            enemyProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
                            proj = enemyProjectile.GetComponent<projectileScript>();
                            projRenderer = proj.GetComponent<SpriteRenderer>();
                            projRenderer.sprite = projSprite[currentSprite];
                            proj.damage = currentDamage;
                            proj.bossSize = currentSize;
                            proj.knockbackPower = currentKnockback;
                            yield return new WaitForSeconds(0.3f);
                            proj.force = currentForce;
                        }
                        break;
                    case 1:
                        Audio.Play("EnemyAttack");
                        enemyProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
                        proj = enemyProjectile.GetComponent<projectileScript>();
                        projRenderer = proj.GetComponent<SpriteRenderer>();
                        projRenderer.sprite = projSprite[currentSprite];
                        proj.damage = currentDamage;
                        proj.bossSize = currentSize;
                        proj.knockbackPower = (currentKnockback * 2);
                        yield return new WaitForSeconds(0.1f);
                        proj.force = (currentForce/2);
                        break;
                    case 2:
                        for (int i = 0; i < 3; i++)
                        {
                            Audio.Play("EnemyAttack");
                            enemyProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
                            proj = enemyProjectile.GetComponent<projectileScript>();
                            projRenderer = proj.GetComponent<SpriteRenderer>();
                            projRenderer.sprite = projSprite[currentSprite];
                            proj.damage = currentDamage;
                            proj.bossSize = currentSize/2;
                            proj.knockbackPower = currentKnockback/2;
                            yield return new WaitForSeconds(0.1f);
                            if (i == 0)
                            {
                                proj.Direction = Quaternion.Euler(0, 0, 60) * (proj.target.position - transform.position);
                            }
                            if (i == 2)
                            {
                                proj.Direction = Quaternion.Euler(0, 0, -60) * (proj.target.position - transform.position);
                            }
                            proj.force = currentForce;
                        }
                        break;
                    case 3:
                        Audio.Play("EnemyAttack");
                        enemyProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
                        proj = enemyProjectile.GetComponent<projectileScript>();
                        projRenderer = proj.GetComponent<SpriteRenderer>();
                        projRenderer.sprite = projSprite[currentSprite];
                        proj.damage = currentDamage;
                        proj.bossSize = currentSize;
                        proj.knockbackPower = currentKnockback;
                        proj.finalBoss = true;
                        yield return new WaitForSeconds(0.1f);
                        proj.force = currentForce;
                        break;
                }
            }
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

        yield return new WaitForSecondsRealtime(0.5f);

        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        isDying = true;
        FindObjectOfType<AudioManager>().Play("EnemyDeath");

        yield return new WaitForSecondsRealtime(1f);

        float randomNum = Random.Range(0, 100);
        if (randomNum > 90 || boss == true)
        {
            Instantiate(healthPickup, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), Quaternion.identity);
        }
        eCont.enemies.Remove(this);

        if (isBoss == true)
        {
            eCont.gameController.roomComplete = true;
            if (boss.isFinalBoss == true)
            {
                Instantiate(boss.bossCrown, transform.position, Quaternion.identity);
            }
        }

        
        Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Respawn"))
        {
            transform.position = eCont.spawnPoint;
        }
        else if (other.gameObject.CompareTag("JackalSpecial"))
        {
            health -= 2;
            pCont.health += 2;
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
            health -= 1;
            FindObjectOfType<AudioManager>().Play("EnemyDamaged");
            StartCoroutine(Knockback(2f, 200f, other.gameObject.transform));
            spaceBetween += 10;
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
