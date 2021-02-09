using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //Dictionary for Storing Keybinds
    private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();

    public int maxHealth;
    public int health;
    public HealthBar healthBar;
    public OptionsMenu Options;
    public AudioManager Audio;
    public DeathScript death;

    bool attacking;

    public int shieldCount;

    Vector2 movement;
    public float moveSpeed;
    public float speedBonus;
    public Rigidbody2D rb;
    public int formNumber;
    public ParticleSystem attackEffect;
    public GameObject attackOrbiter;
    public GameObject[] attacks;
    public GameObject[] secondaryAttacks;

    public Animator anim;

    public float attackDuration;

    public bool powerAttack;
    public bool superForm;
    public bool tempFormActive;

    bool jackalSndAtk;
    bool hawkSndAtk;
    bool bullSndAtk;
    Vector3 mousePos;
    private Vector3 normaliseDir;

    attackOrbit orbitPos;

    SpriteRenderer sr;

    public Camera cam;

    //New Primary Attack Stuff
    public Transform firePoint;
    public GameObject[] attackPrefabs;
    public float attackIncrease;
    public float rangeIncrease;
    public float attackCooldown;
    float currentCooldown;

    bool sndActive;
    float sndCurrentCooldown;
    public float sndCooldown;

    void Start()
    {
        currentCooldown = attackCooldown;
        attacking = false;
        formNumber = 0;
        SwitchForm();
        maxHealth = PlayerPrefs.GetInt("playerHealth", 100);
        health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        sndCooldown = 5;

        sr = GetComponent<SpriteRenderer>();

        orbitPos = attackOrbiter.GetComponent<attackOrbit>();

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

    void Update()
    {
        if (superForm == true)
        {
            sr.color = Color.yellow;
        }
        else
        {
            sr.color = Color.white;
        }



        //Update for Input
        if (Options.GameIsPaused == false)
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
                MainAttack();
            }

            if (Input.GetKeyDown(keys["secondaryAttack"]) && attacking == false && sndActive == false)
            {
                StartCoroutine(SecondaryAttack());
            }

            if(currentCooldown >= 0)
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

            mousePos = Input.mousePosition;
            mousePos.z = 0;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            normaliseDir = (mousePos - transform.position).normalized;

            if (bullSndAtk == true)
            {
                transform.Translate(normaliseDir * (moveSpeed * 2) * Time.deltaTime);
                if (currentCooldown <= 0f)
                {
                    attacking = false;
                    secondaryAttacks[formNumber].SetActive(false);
                    bullSndAtk = false;
                    sndCurrentCooldown = sndCooldown;
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

        healthBar.SetHealth(health);

        if (health > maxHealth)
        {
            health = maxHealth;
        }

        if (health <= 0)
        {
            death.Dead();
        }


    }

    void playFootstepSound()
    {
        Audio.Play("PlayerWalk");
    }


    void SwitchForm()
    {
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

        switch (formNumber)
        {
            case 0:
                moveSpeed = 20 + speedBonus;
                break;
            default:
                moveSpeed = 15 + speedBonus;
                break;
        }
    }

    void MainAttack()
    {
        if (currentCooldown <= 0)
        {
            if (formNumber == 0)
            {
                GameObject JackalAttack = Instantiate(attackPrefabs[0], firePoint.position, Quaternion.identity);
               // playerProjectileScript Proj = JackalAttack.GetComponent<playerProjectileScript>();
            }
            else if (formNumber == 1)
            {
                GameObject HawkAttack = Instantiate(attackPrefabs[2], firePoint.position, firePoint.rotation);
               // playerProjectileScript Proj = HawkAttack.GetComponent<playerProjectileScript>();
            }
            else if (formNumber == 2)
            {
                GameObject BullAttack = Instantiate(attackPrefabs[1], firePoint.position, firePoint.rotation);
              //  playerProjectileScript Proj = BullAttack.GetComponent<playerProjectileScript>();
            }
            currentCooldown = attackCooldown;
        }
       
    }

    public void IncreaseSecondarySize()
    {
        foreach (GameObject attack in secondaryAttacks)
        {
            attack.transform.localScale = new Vector2(attack.gameObject.transform.localScale.x + 1, attack.gameObject.transform.localScale.y);
        }
    }
    
    IEnumerator SecondaryAttack()
    {
         attacking = true;

        if (formNumber == 2)
        {
            secondaryAttacks[formNumber].SetActive(true);
            currentCooldown = attackCooldown * 4;
            bullSndAtk = true;
        }
        else if (formNumber == 1)
        {
            secondaryAttacks[formNumber].SetActive(true);
            currentCooldown = attackCooldown;
            hawkSndAtk = true;
        }
        else
        {
            secondaryAttacks[formNumber].SetActive(true);
            currentCooldown = attackCooldown/2;
            jackalSndAtk = true;
        }

        yield return null;
       
    }

    public void IncreaseAttackRange()
    {
        rangeIncrease += 0.4f;
    }

    public void IncreaseAttackSize()
    {
        attackIncrease += 1f;
    }

    void SetParticleColour()
    {
        ParticleSystem pObj = GameObject.FindObjectOfType<ParticleSystem>();
        ParticleSystem ps = pObj.GetComponent<ParticleSystem>();
        var main = ps.main;
        switch (formNumber)
        {
            case 1:
                main.startColor = Color.black;
                break;
            case 2:
                main.startColor = Color.yellow;
                break;
            default:
                main.startColor = Color.blue;
                break;
        }
    }

    
}
