using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerProjectileScript : MonoBehaviour
{
    public Camera cam;
    Vector2 mousePos;
    Rigidbody2D rb;
    public float force;

    void Start()
    {
        cam = FindObjectOfType<Camera>();
        rb = GetComponent<Rigidbody2D>();
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
        rb.AddForce(lookDir * force);

        if(this.gameObject.tag == "basicAttack")
        {
            StartCoroutine(JackalSelfDestruct());
        }
        else if (this.gameObject.tag == "APAttack")
        {
            StartCoroutine(BullSelfDestruct());
        }
        if (this.gameObject.tag == "rangedAttack")
        {
            StartCoroutine(HawkSelfDestruct());
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(this.gameObject);
    }

    IEnumerator JackalSelfDestruct()
    {
        yield return new WaitForSeconds(1f);

        Destroy(this.gameObject);
    }

    IEnumerator HawkSelfDestruct()
    {
        yield return new WaitForSeconds(0.5f);

        Destroy(this.gameObject);
    }

    IEnumerator BullSelfDestruct()
    {
        yield return new WaitForSeconds(0.5f);

        Destroy(this.gameObject);
    }
}
