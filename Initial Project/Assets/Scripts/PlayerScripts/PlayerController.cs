using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //Dictionary for Storing Keybinds
    private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();
    public int maxHealth;
    public int health;
    public HealthBar healthBar;
    public SecondaryCooldown secondaryCooldown;
    public OptionsMenu Options;
    public AudioManager Audio;
    public DeathScript death;
    bool attacking;
    public int shieldCount;
    Vector2 movement;
    public float moveSpeed;
    public float speedBonus;
    public Rigidbody2D rb;
    public Rigidbody2D frb;
    public int formNumber;
    public ParticleSystem auraObj;
    public ParticleSystem attackEffect;
    public GameObject attackOrbiter;
    public GameObject[] secondaryAttacks;
    public GameObject[] secondaryEffects;
    public Animator anim;
    public bool playerDead;
    bool jackalSndAtk;
    bool hawkSndAtk;
    bool bullSndAtk;
    Vector2 mousePos;
    private Vector3 normaliseDir;
    attackOrbit orbitPos;
    SpriteRenderer sr;
    public Camera cam;
    public GameController gCont;

    //New Primary Attack Stuff
    public Transform firePoint;
    public GameObject fireObject;
    public GameObject[] attackPrefabs;
    public float force = 75;
    public float attackIncrease;
    public float rangeIncrease;
    public float attackCooldown;
    float currentCooldown;
    public float knockbackIncrease;

    bool sndActive;
    float sndCurrentCooldown;
    public float sndCooldown;

    Material mat;
    float fade = 1f;

    public GameObject healthBox;
    Material healthBoxMat;
    float outline = 0f;

    public bool finalBossAnim;

    void Awake()
    {
        currentCooldown = attackCooldown;
        attacking = false;
        playerDead = false;
        formNumber = 0;
        maxHealth = PlayerPrefs.GetInt("playerHealth", 60);
        sndCooldown = 5;

        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        frb = fireObject.GetComponent<Rigidbody2D>();
        orbitPos = attackOrbiter.GetComponent<attackOrbit>();
        mat = GetComponent<SpriteRenderer>().material;
        healthBoxMat = healthBox.GetComponent<Image>().material;
        

        //Adds our stored keys to the dictionary
        //This will need to be done again if the player changes keybinds during game
        keys.Add("Up", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Up", "W")));
        keys.Add("Down", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Down", "S")));
        keys.Add("Left", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Left", "A")));
        keys.Add("Right", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Right", "D")));
        keys.Add("Escape", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Escape", "Escape")));
        keys.Add("switchA", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("switchA", "Q")));
        keys.Add("switchB", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("switchB", "E")));
        keys.Add("basicAttack", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("basicAttack", "Mouse0")));
        keys.Add("secondaryAttack", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("secondaryAttack", "Mouse1")));
    }

     void Start()
    {
        healthBar.SetMaxHealth(maxHealth);
        //SwitchForm();
    }

    void Update()
    {
        
        fade += Time.deltaTime * 2;
        if (fade >= 1f)
        {
            fade = 1f;
        }
        mat.SetFloat("_Fade", fade);


        if (shieldCount > 0)
        {
            outline = 1;
        }
        else
        {
            outline = 0;
        }
        healthBoxMat.SetFloat("_OutlineThickness", outline);

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        fireObject.transform.position = rb.transform.position;

        //Update for Input
        if (Options.GameIsPaused == false && playerDead == false)
        {
            if (!finalBossAnim)
            {
                //New Movement Code, no longer uses rigidbody
                //All inputs call playerpref 'keys' dictionary that carry between scenes
                if (Input.GetKey(keys["Up"]))
                {

                    transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
                    anim.SetFloat("horizontal", 0);
                    anim.SetFloat("vertical", 1);
                }

                if (Input.GetKey(keys["Down"]))
                {

                    transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);
                    anim.SetFloat("horizontal", -0);
                    anim.SetFloat("vertical", -1);
                }

                if (Input.GetKey(keys["Left"]))
                {

                    transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
                    anim.SetFloat("vertical", 0);
                    anim.SetFloat("horizontal", -1);
                }

                if (!Input.GetKey(keys["Left"]) && !Input.GetKey(keys["Right"]) && !Input.GetKey(keys["Up"]) && !Input.GetKey(keys["Down"]))
                {
                    anim.SetFloat("vertical", 0);
                    anim.SetFloat("horizontal", 0);
                }

                if (Input.GetKey(keys["Right"]))
                {

                    transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
                    anim.SetFloat("vertical", 0);
                    anim.SetFloat("horizontal", 1);
                }

                if (Input.GetKeyDown(keys["switchA"]) && attacking == false)
                {
                    formNumber -= 1;
                    SwitchForm();
                }
                if (Input.GetKeyDown(keys["switchB"]) && attacking == false)
                {
                    formNumber += 1;
                    SwitchForm();
                }

                //attackCode (Mouse)
                if (Input.GetKeyDown(keys["basicAttack"]) && attacking == false)
                {
                    //StartCoroutine(BasicAttack());
                    StartCoroutine(MainAttack());
                }

                if (Input.GetKeyDown(keys["secondaryAttack"]) && attacking == false && sndActive == false)
                {
                    StartCoroutine(SecondaryAttack());
                }

                if (currentCooldown >= 0)
                {
                    currentCooldown -= Time.deltaTime;
                }

                if (sndCurrentCooldown >= 0)
                {
                    sndCurrentCooldown -= Time.deltaTime;
                }

                if (sndCurrentCooldown < 0)
                {
                    sndActive = false;
                }


                /* mousePos = Input.mousePosition;
                 mousePos.z = 0;
                 mousePos = Camera.main.ScreenToWorldPoint(mousePos); */
                normaliseDir = (mousePos - rb.position).normalized;

                if (bullSndAtk == true)
                {
                    transform.Translate(normaliseDir * (moveSpeed * 2) * Time.deltaTime);
                    if (currentCooldown <= 0f)
                    {
                        attacking = false;
                        secondaryAttacks[formNumber].SetActive(false);
                        bullSndAtk = false;
                        sndCurrentCooldown = sndCooldown;
                        anim.SetBool("Charging", false);
                        sndActive = true;
                    }
                }
                if (jackalSndAtk == true)
                {
                    transform.Translate(normaliseDir * (moveSpeed * 2) * Time.deltaTime);
                    if (currentCooldown <= 0f)
                    {
                        attacking = false;
                        secondaryAttacks[formNumber].SetActive(false);
                        jackalSndAtk = false;
                        sndCurrentCooldown = sndCooldown;
                        sndActive = true;
                    }
                }
                if (hawkSndAtk == true)
                {
                    if (currentCooldown <= 0f)
                    {
                        attacking = false;
                        secondaryAttacks[formNumber].SetActive(false);
                        hawkSndAtk = false;
                        sndCurrentCooldown = sndCooldown;
                        sndActive = true;
                    }
                }
            }

        }

        healthBar.SetHealth(health);

        if (health > maxHealth)
        {
            health = maxHealth;
        }

        if (health <= 0)
        {
            StartCoroutine(Death());
            
        }

        secondaryCooldown.SetTime(sndCurrentCooldown);

    }

    private void FixedUpdate()
    {
        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        frb.rotation = angle;
    }

    void playFootstepSound()
    {
        Audio.Play("PlayerWalk");
    }


    void SwitchForm()
    {
        fade = 0.5f;
        mat.SetFloat("_Fade", fade);
        if (formNumber > 2)
        {
            formNumber = 0;
        }
        if (formNumber < 0)
        {
            formNumber = 2;
        }

        anim.SetInteger("form", formNumber);
        Audio.Play("FormChange");
        ParticleSystem aura = auraObj.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule auraMain = aura.main;

        switch (formNumber)
        {
            case 0:
                moveSpeed = 20 + speedBonus;
                auraMain.startColor = Color.magenta;
                break;
            case 1:
                moveSpeed = 15 + speedBonus;
                auraMain.startColor = Color.yellow;
                break;
            default:
                moveSpeed = 15 + speedBonus;
                auraMain.startColor = Color.blue;
                break;
        }
    }
    public IEnumerator Death()
    {
        anim.SetTrigger("Death");
        playerDead = true;
        yield return new WaitForSecondsRealtime(1f);
        health = maxHealth;
        gCont.SaveGame();
        death.Dead();
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

    public IEnumerator MainAttack()
    {

        if (currentCooldown <= 0)
        {
            anim.SetTrigger("Attack");
            yield return new WaitForSeconds(0.25f);
            if (formNumber == 0)
            {
                GameObject JackalAttack = Instantiate(attackPrefabs[0], firePoint.position, firePoint.rotation * Quaternion.Euler(0,0,-90));
                Rigidbody2D brb = JackalAttack.GetComponent<Rigidbody2D>();
                playerProjectileScript jProj = JackalAttack.GetComponent<playerProjectileScript>();
                brb.AddForce(firePoint.up * force, ForceMode2D.Impulse);

            }
            else if (formNumber == 1)
            {
                GameObject HawkAttack = Instantiate(attackPrefabs[2], firePoint.position, firePoint.rotation * Quaternion.Euler(0, 0, -90));
                Rigidbody2D brb = HawkAttack.GetComponent<Rigidbody2D>();
                playerProjectileScript hProj = HawkAttack.GetComponent<playerProjectileScript>();
                brb.AddForce(firePoint.up * force, ForceMode2D.Impulse);
            }
            else if (formNumber == 2)
            {
                GameObject BullAttack = Instantiate(attackPrefabs[1], firePoint.position, firePoint.rotation * Quaternion.Euler(0, 0, -90));
                Rigidbody2D brb = BullAttack.GetComponent<Rigidbody2D>();
                playerProjectileScript bProj = BullAttack.GetComponent<playerProjectileScript>();
                brb.AddForce(firePoint.up * force, ForceMode2D.Impulse);
            }
            currentCooldown = attackCooldown;
        }
       
    }

    public void IncreaseSecondarySize()
    {
        foreach (GameObject attack in secondaryAttacks)
        {
            attack.transform.localScale = new Vector2(attack.gameObject.transform.localScale.x * 1.40f, attack.gameObject.transform.localScale.y);
        }
    }
    
    IEnumerator SecondaryAttack()
    {
         attacking = true;

        if (formNumber == 2)
        {
            anim.SetTrigger("SpecialAttack");
            anim.SetBool("Charging", true);
            secondaryAttacks[formNumber].SetActive(true);
            currentCooldown = attackCooldown * 4;
            GameObject effect = Instantiate(secondaryEffects[2], transform.position, Quaternion.identity);
            effect.transform.parent = this.transform;
            Destroy(effect, 1f);
            bullSndAtk = true;
        }
        else if (formNumber == 1)
        {
            anim.SetTrigger("SpecialAttack");
            yield return new WaitForSeconds(0.35f);
            secondaryAttacks[formNumber].SetActive(true);
            currentCooldown = attackCooldown;
            GameObject effect = Instantiate(secondaryEffects[1], transform.position, Quaternion.identity);
            effect.transform.parent = this.transform;
            Destroy(effect, 0.5f);
            hawkSndAtk = true;
        }
        else
        {
            anim.SetTrigger("SpecialAttack");
            yield return new WaitForSeconds(0.25f);
            secondaryAttacks[formNumber].SetActive(true);
            currentCooldown = attackCooldown / 2;
            GameObject effect = Instantiate(secondaryEffects[0], transform.position, Quaternion.identity);
            effect.transform.parent = this.transform;
            Destroy(effect, 1f);
            jackalSndAtk = true;
        }

        yield return null;
       
    }

    public void IncreaseAttackRange()
    {
        rangeIncrease *= 1.40f;
    }

    public void IncreaseKnockback()
    {
        knockbackIncrease *= 1.40f;
    }

    public void IncreaseAttackSize()
    {
        attackIncrease *= 1.40f;
    }
    
    
}
