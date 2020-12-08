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
    bool attacking;

    Vector2 movement;
    public float moveSpeed;
    public Rigidbody2D rb;
    public int formNumber;
    public GameObject[] forms;
    public GameObject[] formOneAttacks;
    public GameObject[] formTwoAttacks;
    public GameObject[] formThreeAttacks;
    public AudioSource source;

    private Animation anim;

    public float attackDuration;

    void Start()
    {
        attacking = false;
        formNumber = Random.Range(0, 3);
        SwitchForm();
        health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        anim = gameObject.GetComponent<Animation>();

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
        /* //Update for Input
         movement.x = Input.GetAxisRaw("Horizontal");
         movement.y = Input.GetAxisRaw("Vertical"); */

        //New Movement Code, no longer uses rigidbody
        //All inputs call playerpref 'keys' dictionary that carry between scenes
        if (Input.GetKey(keys["Up"]))
        {
            transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
        }

        if (Input.GetKey(keys["Down"]))
        {
            transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);
        }

        if (Input.GetKey(keys["Left"]))
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
        }

        if (Input.GetKey(keys["Right"]))
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
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
            StartCoroutine (AttackRight());
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

        healthBar.SetHealth(health);

        if (health  <= 0)
        {
            SceneManager.LoadScene(0);
        }

        if (anim.isPlaying)
        {
            return;
        }
    }

  /*  void FixedUpdate()
    {
        //FUpdate for movement physics
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    } */

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

        foreach (GameObject form in forms)
        {
            form.SetActive(false);
        }

        forms[formNumber].SetActive(true);
        FindObjectOfType<AudioManager>().Play("FormChange");
    }

    IEnumerator AttackRight()
    {
        attacking = true;
        FindObjectOfType<AudioManager>().Play("PlayerAttack");

        if (formNumber == 0)
        {
            formOneAttacks[0].SetActive(true);
            anim.Play("BaseAttackRight");
            yield return new WaitForSeconds(attackDuration);
            formOneAttacks[0].SetActive(false);
            attacking = false;
        }
        else if (formNumber == 1)
        {
            formTwoAttacks[0].SetActive(true);
            anim.Play("ApAttackRight");
            yield return new WaitForSeconds(attackDuration);
            formTwoAttacks[0].SetActive(false);
            attacking = false;
        }
        else
        {
            formThreeAttacks[0].SetActive(true);
            anim.Play("RangedAttackRight");
            yield return new WaitForSeconds(attackDuration);
            formThreeAttacks[0].SetActive(false);
            attacking = false;
        }
    }

    IEnumerator AttackLeft()
    {
        attacking = true;
        FindObjectOfType<AudioManager>().Play("PlayerAttack");

        if (formNumber == 0)
        {
            formOneAttacks[1].SetActive(true);
            anim.Play("BaseAttackLeft");
            yield return new WaitForSeconds(attackDuration);
            formOneAttacks[1].SetActive(false);
            attacking = false;
        }
        else if (formNumber == 1)
        {
            formTwoAttacks[1].SetActive(true);
            anim.Play("ApAttackLeft");
            yield return new WaitForSeconds(attackDuration);
            formTwoAttacks[1].SetActive(false);
            attacking = false;
        }
        else
        {
            formThreeAttacks[1].SetActive(true);
            anim.Play("RangedAttackLeft");
            yield return new WaitForSeconds(attackDuration);
            formThreeAttacks[1].SetActive(false);
            attacking = false;
        }
    }

    IEnumerator AttackUp()
    {
        attacking = true;
        FindObjectOfType<AudioManager>().Play("PlayerAttack");

        if (formNumber == 0)
        {
            formOneAttacks[2].SetActive(true);
            anim.Play("BaseAttackUp");
            yield return new WaitForSeconds(attackDuration);
            formOneAttacks[2].SetActive(false);
            attacking = false;
        }
        else if (formNumber == 1)
        {
            formTwoAttacks[2].SetActive(true);
            anim.Play("ApAttackUp");
            yield return new WaitForSeconds(attackDuration);
            formTwoAttacks[2].SetActive(false);
            attacking = false;
        }
        else
        {
            formThreeAttacks[2].SetActive(true);
            anim.Play("RangedAttackUp");
            yield return new WaitForSeconds(attackDuration);
            formThreeAttacks[2].SetActive(false);
            attacking = false;
        }
    }

    IEnumerator AttackDown()
    {
        attacking = true;
        FindObjectOfType<AudioManager>().Play("PlayerAttack");

        if (formNumber == 0)
        {
            formOneAttacks[3].SetActive(true);
            anim.Play("BaseAttackDown");
            yield return new WaitForSeconds(attackDuration);
            formOneAttacks[3].SetActive(false);
            attacking = false;
        }
        else if (formNumber == 1)
        {
            formTwoAttacks[3].SetActive(true);
            anim.Play("ApAttackDown");
            yield return new WaitForSeconds(attackDuration);
            formTwoAttacks[3].SetActive(false);
            attacking = false;
        }
        else
        {
            formThreeAttacks[3].SetActive(true);
            anim.Play("RangedAttackDown");
            yield return new WaitForSeconds(attackDuration);
            formThreeAttacks[3].SetActive(false);
            attacking = false;
        }
    }
}
