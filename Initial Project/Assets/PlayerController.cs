using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector2 movement;
    public float moveSpeed;
    public Rigidbody2D rb;
    public GameObject[] forms;
    int formNumber;
    public AudioSource source;

    void Start()
    {
        formNumber = Random.Range(0, 3);
        SwitchForm();
        source = GetComponent<AudioSource>();
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



        switch (formNumber)
        {
            case 2:
                forms[2].SetActive(true);
                source.Play();
                break;
            case 1:
                forms[1].SetActive(true);
                source.Play();
                break;
            default:
                forms[0].SetActive(true);
                source.Play();
                break;
        }
    }
}
