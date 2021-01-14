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
        Direction = targetPos - (Vector2)transform.position;
        GetComponent<Rigidbody2D>().AddForce(Direction * force);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            Audio.Play("PlayerDamage");
            player.GetComponent<PlayerController>().health --; 
        }

        Destroy(this.gameObject);
    }
}
