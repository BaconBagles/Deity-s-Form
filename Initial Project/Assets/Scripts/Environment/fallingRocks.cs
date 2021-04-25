using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallingRocks : MonoBehaviour
{
     CircleCollider2D mycollider;
    public GameObject landEffect;
    Enemy enemy;

    void Start()
    {
        mycollider = GetComponent<CircleCollider2D>();
        mycollider.enabled = false;
        StartCoroutine(ScaleUp(2f));
    }


    public IEnumerator ScaleUp(float time)
    {
        float i = 0;
        float rate = 1 / time;

        Vector3 oldScale = transform.localScale;
        Vector3 newScale = new Vector3(15, 15, 0);

        while (i < 1)
        {
            i += Time.deltaTime * rate;
            transform.localScale = Vector3.Lerp(oldScale, newScale, i);
            yield return 0;
        }

        mycollider.enabled = true;
        GameObject effect = Instantiate(landEffect, transform.position, Quaternion.identity);
        Destroy(effect, 0.5f);
        CameraShake cam = FindObjectOfType<CameraShake>();
        cam.StartCoroutine(cam.Shake(0.5f, 1.5f));
        Destroy(this.gameObject);


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController player = PlayerController.FindObjectOfType<PlayerController>();
            player.health -= 4;
        }
        else
        {
            enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.health -= 5;
        }
    }

}
