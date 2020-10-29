using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int health;
    Vector2 movement;
    public float moveSpeed;
    public Rigidbody2D rb;
    int formNumber;
    public GameObject[] forms;
    public GameObject[] formOneAttacks;
    public GameObject[] formTwoAttacks;
    public GameObject[] formThreeAttacks;
    public AudioSource source;

    public float attackDuration;

    void Start()
    {
        formNumber = Random.Range(0, 3);
        SwitchForm();
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
        if (Input.GetKeyDown("right"))
        {
            StartCoroutine (AttackRight());
        }
        if (Input.GetKeyDown("left"))
        {
            StartCoroutine(AttackLeft());
        }
        if (Input.GetKeyDown("up"))
        {
            StartCoroutine(AttackUp());
        }
        if (Input.GetKeyDown("down"))
        {
            StartCoroutine(AttackDown());
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
        if (formNumber == 0)
        {
            formOneAttacks[0].SetActive(true);
            yield return new WaitForSeconds(attackDuration);
            formOneAttacks[0].SetActive(false);
        }
        else if (formNumber == 1)
        {
            formTwoAttacks[0].SetActive(true);
            yield return new WaitForSeconds(attackDuration);
            formTwoAttacks[0].SetActive(false);
        }
        else
        {
            formThreeAttacks[0].SetActive(true);
            yield return new WaitForSeconds(attackDuration);
            formThreeAttacks[0].SetActive(false);
        }
    }

    IEnumerator AttackLeft()
    {
        if (formNumber == 0)
        {
            formOneAttacks[1].SetActive(true);
            yield return new WaitForSeconds(attackDuration);
            formOneAttacks[1].SetActive(false);
        }
        else if (formNumber == 1)
        {
            formTwoAttacks[1].SetActive(true);
            yield return new WaitForSeconds(attackDuration);
            formTwoAttacks[1].SetActive(false);
        }
        else
        {
            formThreeAttacks[1].SetActive(true);
            yield return new WaitForSeconds(attackDuration);
            formThreeAttacks[1].SetActive(false);
        }
    }

    IEnumerator AttackUp()
    {
        if (formNumber == 0)
        {
            formOneAttacks[2].SetActive(true);
            yield return new WaitForSeconds(attackDuration);
            formOneAttacks[2].SetActive(false);
        }
        else if (formNumber == 1)
        {
            formTwoAttacks[2].SetActive(true);
            yield return new WaitForSeconds(attackDuration);
            formTwoAttacks[2].SetActive(false);
        }
        else
        {
            formThreeAttacks[2].SetActive(true);
            yield return new WaitForSeconds(attackDuration);
            formThreeAttacks[2].SetActive(false);
        }
    }

    IEnumerator AttackDown()
    {
        if (formNumber == 0)
        {
            formOneAttacks[3].SetActive(true);
            yield return new WaitForSeconds(attackDuration);
            formOneAttacks[3].SetActive(false);
        }
        else if (formNumber == 1)
        {
            formTwoAttacks[3].SetActive(true);
            yield return new WaitForSeconds(attackDuration);
            formTwoAttacks[3].SetActive(false);
        }
        else
        {
            formThreeAttacks[3].SetActive(true);
            yield return new WaitForSeconds(attackDuration);
            formThreeAttacks[3].SetActive(false);
        }
    }
}
