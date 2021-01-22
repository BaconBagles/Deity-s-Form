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
        GetComponent<Rigidbody2D>().AddForce(Direction * force);
        StartCoroutine(SelfDestruct());
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            Audio.Play("PlayerDamage");
            if (player.GetComponent<PlayerController>().shieldCount == 0)
            {
                player.GetComponent<PlayerController>().health--;

            }
            else
            {
                player.GetComponent<PlayerController>().shieldCount -= 1;
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
