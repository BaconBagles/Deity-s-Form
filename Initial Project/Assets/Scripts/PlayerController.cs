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
    //public GameObject[] forms;
    public ParticleSystem attackEffect;
    public GameObject[] formOneAttacks;
    public GameObject[] formTwoAttacks;
    public GameObject[] formThreeAttacks;

    public Animator anim;

    public float attackDuration;

    void Start()
    {
        attacking = false;
        formNumber = Random.Range(0, 3);
        SwitchForm();
        maxHealth = PlayerPrefs.GetInt("playerHealth", 100);
        health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        

        //Adds our stored keys to the dictionary
        //This will need to be done again if the player changes keybinds during game
        keys.Add("Up", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Up", "W")));
        keys.Add("Down", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Down", "S")));
        keys.Add("Left", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Left", "A")));
        keys.Add("Right", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Right", "D")));
        keys.Add("Escape", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Escape", "Escape")));
        keys.Add("AttackUp", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("AttackUp", "UpArrow")));
        keys.Add("AttackDown", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("AttackDown", "DownArrow")));
        keys.Add("AttackLeft", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("AttackLeft", "LeftArrow")));
        keys.Add("AttackRight", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("AttackRight", "RightArrow")));
        keys.Add("form1", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("form1", "1")));
        keys.Add("form2", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("form2", "2")));
        keys.Add("form3", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("form3", "3")));
        keys.Add("switchA", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("switchA", "Q")));
        keys.Add("switchB", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("switchB", "E")));
    }

    void Update()
    {
        //Update for Input
        //anim.SetFloat("speed", anim.GetFloat("vertical")*anim.GetFloat("horizontal"));
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

            if (Input.GetKey(keys["Right"]))
            {

                transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
                anim.SetFloat("vertical", 0);
                anim.SetFloat("horizontal", 1);
            }

            if (Input.GetKeyDown(keys["form1"]))
            {
                formNumber = 0;
                SwitchForm();
            }
            if (Input.GetKeyDown(keys["form2"]))
            {
                formNumber = 1;
                SwitchForm();
            }
            if (Input.GetKeyDown(keys["form3"]))
            {
                formNumber = 2;
                SwitchForm();
            }
            if (Input.GetKeyDown(keys["switchA"]))
            {
                formNumber -= 1;
                SwitchForm();
            }
            if (Input.GetKeyDown(keys["switchB"]))
            {
                formNumber += 1;
                SwitchForm();
            }

            //attack code
            if (Input.GetKeyDown(keys["AttackRight"]) && attacking == false)
            {
                StartCoroutine(AttackRight());
            }
            if (Input.GetKeyDown(keys["AttackLeft"]) && attacking == false)
            {
                StartCoroutine(AttackLeft());
            }
            if (Input.GetKeyDown(keys["AttackUp"]) && attacking == false)
            {
                StartCoroutine(AttackUp());
            }
            if (Input.GetKeyDown(keys["AttackDown"]) && attacking == false)
            {
                StartCoroutine(AttackDown());
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
            death.dead = true;
            health = 1;
        }


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
        /*foreach (GameObject form in forms)
        {
            form.SetActive(false);
        }

        forms[formNumber].SetActive(true);*/
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

    IEnumerator AttackRight()
    {
        attacking = true;
       Audio.Play("PlayerAttack");
        StartCoroutine(ParticleRight());

        if (formNumber == 0)
        {
            formOneAttacks[0].SetActive(true);
            //anim.Play("BaseAttackRight");
            yield return new WaitForSeconds(attackDuration);
            formOneAttacks[0].SetActive(false);
            attacking = false;
        }
        else if (formNumber == 1)
        {
            formTwoAttacks[0].SetActive(true);
            //anim.Play("ApAttackRight");
            yield return new WaitForSeconds(attackDuration);
            formTwoAttacks[0].SetActive(false);
            attacking = false;
        }
        else
        {
            formThreeAttacks[0].SetActive(true);
            //anim.Play("RangedAttackRight");
            yield return new WaitForSeconds(attackDuration);
            formThreeAttacks[0].SetActive(false);
            attacking = false;
        }
    }

    IEnumerator ParticleRight()
    {
        ParticleSystem aEffect = Instantiate(attackEffect, transform.position, Quaternion.Euler(0, 90, 0));
        SetParticleColour();
        aEffect.transform.parent = gameObject.transform;
        yield return new WaitForSeconds(attackDuration + .5f);
        Destroy(aEffect.gameObject);
    }

    IEnumerator AttackLeft()
    {
        attacking = true;
        Audio.Play("PlayerAttack");
        StartCoroutine(ParticleLeft());

        if (formNumber == 0)
        {
            formOneAttacks[1].SetActive(true);
            //anim.Play("BaseAttackLeft");
            yield return new WaitForSeconds(attackDuration);
            formOneAttacks[1].SetActive(false);
            attacking = false;
        }
        else if (formNumber == 1)
        {
            formTwoAttacks[1].SetActive(true);
            //anim.Play("ApAttackLeft");
            yield return new WaitForSeconds(attackDuration);
            formTwoAttacks[1].SetActive(false);
            attacking = false;
        }
        else
        {
            formThreeAttacks[1].SetActive(true);
            //anim.Play("RangedAttackLeft");
            yield return new WaitForSeconds(attackDuration);
            formThreeAttacks[1].SetActive(false);
            attacking = false;
        }
    }

    IEnumerator ParticleLeft()
    {
        ParticleSystem aEffect = Instantiate(attackEffect, transform.position, Quaternion.Euler(0, -90, 0));
        SetParticleColour();
        aEffect.transform.parent = gameObject.transform;
        yield return new WaitForSeconds(attackDuration + .5f);
        Destroy(aEffect.gameObject);
    }

    IEnumerator AttackUp()
    {
        attacking = true;
        Audio.Play("PlayerAttack");
        StartCoroutine(ParticleUp());

        if (formNumber == 0)
        {
            formOneAttacks[2].SetActive(true);
            //anim.Play("BaseAttackUp");
            yield return new WaitForSeconds(attackDuration);
            formOneAttacks[2].SetActive(false);
            attacking = false;
        }
        else if (formNumber == 1)
        {
            formTwoAttacks[2].SetActive(true);
            //anim.Play("ApAttackUp");
            yield return new WaitForSeconds(attackDuration);
            formTwoAttacks[2].SetActive(false);
            attacking = false;
        }
        else
        {
            formThreeAttacks[2].SetActive(true);
            //anim.Play("RangedAttackUp");
            yield return new WaitForSeconds(attackDuration);
            formThreeAttacks[2].SetActive(false);
            attacking = false;
        }
    }

    IEnumerator ParticleUp()
    {
        ParticleSystem aEffect = Instantiate(attackEffect, transform.position, Quaternion.Euler(-90, 0, 0));
        SetParticleColour();
        aEffect.transform.parent = gameObject.transform;
        yield return new WaitForSeconds(attackDuration + .5f);
        Destroy(aEffect.gameObject);
    }

    IEnumerator AttackDown()
    {
        attacking = true;
        Audio.Play("PlayerAttack");
        StartCoroutine(ParticleDown());

        if (formNumber == 0)
        {
            formOneAttacks[3].SetActive(true);
            //anim.Play("BaseAttackDown");
            yield return new WaitForSeconds(attackDuration);
            formOneAttacks[3].SetActive(false);
            attacking = false;
        }
        else if (formNumber == 1)
        {
            formTwoAttacks[3].SetActive(true);
            //anim.Play("ApAttackDown");
            yield return new WaitForSeconds(attackDuration);
            formTwoAttacks[3].SetActive(false);
            attacking = false;
        }
        else
        {
            formThreeAttacks[3].SetActive(true);
            //anim.Play("RangedAttackDown");
            yield return new WaitForSeconds(attackDuration);
            formThreeAttacks[3].SetActive(false);
            attacking = false;
        }
    }

    IEnumerator ParticleDown()
    {
        ParticleSystem aEffect = Instantiate(attackEffect, transform.position, Quaternion.Euler(90, 0, 0));
        SetParticleColour();
        aEffect.transform.parent = gameObject.transform;
        yield return new WaitForSeconds(attackDuration + .5f);
        Destroy(aEffect.gameObject);
    }

    public void IncreaseAttackSize()
    {
        foreach (GameObject attack in formOneAttacks)
        {
            attack.gameObject.transform.localScale += new Vector3(0, 1f, 0);
        }
        foreach (GameObject attack in formTwoAttacks)
        {
            attack.gameObject.transform.localScale += new Vector3(.5f, .5f, 0);
        }
        foreach (GameObject attack in formThreeAttacks)
        {
            attack.gameObject.transform.localScale += new Vector3(1f, 0, 0);
        }
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
