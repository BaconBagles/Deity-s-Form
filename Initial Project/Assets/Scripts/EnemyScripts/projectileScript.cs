using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileScript : MonoBehaviour
{
    GameObject player;
    GameObject AudioManager;
    private Transform target;
    Vector2 Direction;
    public float force;
    public AudioManager Audio;
    public int damage;
    public float bossSize;
    public GameObject hitEffect;
    public int projType;

    //knockback stuff
    public float knockbackPower = 150;
    public float knockbackDuration = 1.5f;

    void Start()
    {
        player = GameObject.Find("Player");
        AudioManager = GameObject.Find("AudioManager");
        Audio = AudioManager.GetComponent<AudioManager>();
        target = player.transform;
        transform.right = target.position - transform.position;
        Vector2 targetPos = target.position;
        targetPos.y -= 3;
        Direction = targetPos - (Vector2)transform.position;
        GetComponent<Rigidbody2D>().AddForce(Direction * force, ForceMode2D.Impulse);
        StartCoroutine(SelfDestruct());
        gameObject.transform.localScale = new Vector3(transform.localScale.x + bossSize, transform.localScale.y + bossSize, 0);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            Audio.Play("PlayerDamage");
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(effect, 0.5f);
            StartCoroutine(player.GetComponent<PlayerController>().Knockback(knockbackDuration, knockbackPower, this.transform));
            if (player.GetComponent<PlayerController>().shieldCount == 0)
            {
                player.GetComponent<PlayerController>().health -= damage;
                CameraShake cam = FindObjectOfType<CameraShake>();
                cam.StartCoroutine(cam.Shake(.5f, 1f));
            }
            else
            {
                player.GetComponent<PlayerController>().shieldCount -= damage;
            }
        }

        Destroy(this.gameObject);
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(1f);

        Destroy(this.gameObject);
    }
}
