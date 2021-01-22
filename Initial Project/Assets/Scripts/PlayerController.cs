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

    SpriteRenderer sr;

    public Camera cam;

    //New Primary Attack Stuff
    public Transform firePoint;
    public GameObject[] attackPrefabs;
    public float attackIncrease;
    public float attackCooldown;
    float currentCooldown;

    void Start()
    {
        currentCooldown = attackCooldown;
        attacking = false;
        formNumber = 0;
        SwitchForm();
        maxHealth = PlayerPrefs.GetInt("playerHealth", 100);
        health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        sr = GetComponent<SpriteRenderer>();

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

            if (Input.GetKeyDown(keys["secondaryAttack"]) && attacking == false)
            {
                StartCoroutine(SecondaryAttack());
            }

            if(currentCooldown <= attackCooldown)
            {
                currentCooldown -= Time.deltaTime;
            }
            
        }

        healthBar.SetHealth(health);

        if (health > maxHealth)
        {
            health = maxHealth;
        }

        if (health <= 0)
        {
            health = 0;
        }

        if (health == 0)
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
            }
            else if (formNumber == 1)
            {
                GameObject HawkAttack = Instantiate(attackPrefabs[2], firePoint.position, firePoint.rotation);
            }
            else if (formNumber == 2)
            {
                GameObject BullAttack = Instantiate(attackPrefabs[1], firePoint.position, firePoint.rotation);
            }
            currentCooldown = attackCooldown;
        }
       
    }

    /*IEnumerator BasicAttack()
    {
        attacking = true;

        if (powerAttack)
        {
            attacks[3].SetActive(true);
            ParticleSystem aEffect = Instantiate(attackEffect, transform.position, Quaternion.Euler(0, 0, 0));
            SetParticleColour();
            aEffect.transform.parent = gameObject.transform;
            yield return new WaitForSeconds(attackDuration);
            attacks[3].SetActive(false);
            Destroy(aEffect.gameObject);
            attacking = false;
        }
        else
        {
            attacks[formNumber].SetActive(true);
            ParticleSystem aEffect = Instantiate(attackEffect, transform.position, Quaternion.Euler(0, 0, 0));
            SetParticleColour();
            aEffect.transform.parent = gameObject.transform;
            yield return new WaitForSeconds(attackDuration);
            attacks[formNumber].SetActive(false);
            Destroy(aEffect.gameObject);
            attacking = false;
        }
    } */
    
    IEnumerator SecondaryAttack()
    {
        attacking = true;

        if (formNumber == 2)
        {
           /* while (Input.GetKeyDown(keys["secondaryAttack"]) == true)
            {
                secondaryAttacks[formNumber].SetActive(true);
                secondaryAttacks[formNumber].gameObject.transform.localScale += new Vector3(0.1f, 0f, 0f) * Time.deltaTime;
                if (secondaryAttacks[formNumber].gameObject.transform.localScale.x >= 10)
                {
                    secondaryAttacks[formNumber].gameObject.transform.localScale = new Vector3(10f, 1f, 0f);
                }
            }
            if (Input.GetKeyUp(keys["secondaryAttack"]))
            {
                this.gameObject.transform.Translate(Vector3.forward * (Time.deltaTime * secondaryAttacks[formNumber].gameObject.transform.localScale.x));
                secondaryAttacks[formNumber].gameObject.transform.localScale = new Vector3(0f, 1f, 0f);
                secondaryAttacks[formNumber].SetActive(false);
                attacking = false;
            }*/
            yield return null;
        }

        else if (formNumber == 1)
        {
            secondaryAttacks[formNumber].SetActive(true);
            yield return new WaitForSeconds(attackDuration);
            secondaryAttacks[formNumber].SetActive(false);
            attacking = false;
        }

        else
        {
            attackOrbit orbitPos = attackOrbiter.GetComponent<attackOrbit>();
            secondaryAttacks[formNumber].SetActive(true);
            transform.localPosition = new Vector3(transform.position.x + orbitPos.xPos * 4, (transform.position.y + 3) + orbitPos.yPos * 4, 0);
            yield return new WaitForSeconds(attackDuration);
            secondaryAttacks[formNumber].SetActive(false);
            attacking = false;
        }
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
