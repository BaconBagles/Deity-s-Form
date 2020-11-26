using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
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
    }

    // Update is called once per frame
    void Update()
    {
        //Update for Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        if (Input.GetKeyDown("1"))
        {
            formNumber = 0;
            SwitchForm();
        }
        if (Input.GetKeyDown("2"))
        {
            formNumber = 1;
            SwitchForm();
        }
        if (Input.GetKeyDown("3"))
        {
            formNumber = 2;
            SwitchForm();
        }
        if (Input.GetKeyDown("q"))
        {
            formNumber -= 1;
            SwitchForm();
        }
        if (Input.GetKeyDown("e"))
        {
            formNumber += 1;
            SwitchForm();
        }

        //attack code
        if (Input.GetKeyDown("right") && attacking == false)
        {
            StartCoroutine (AttackRight());
        }
        if (Input.GetKeyDown("left") && attacking == false)
        {
            StartCoroutine(AttackLeft());
        }
        if (Input.GetKeyDown("up") && attacking == false)
        {
            StartCoroutine(AttackUp());
        }
        if (Input.GetKeyDown("down") && attacking == false)
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

    void FixedUpdate()
    {
        //FUpdate for movement physics
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
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

        foreach (GameObject form in forms)
        {
            form.SetActive(false);
        }

        forms[formNumber].SetActive(true);
        source.Play();
    }

    IEnumerator AttackRight()
    {
        attacking = true;

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
