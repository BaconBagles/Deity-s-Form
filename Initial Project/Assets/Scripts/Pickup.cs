using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    GameObject eContObj;
    EnemyController eCont;
    GameObject player;
    PlayerController pCont;

    void Start()
    {
        eContObj = GameObject.Find("EnemyController");
        eCont = eContObj.GetComponent<EnemyController>();
        player = GameObject.Find("Player");
        pCont = player.GetComponent<PlayerController>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            eCont.StartCoroutine(eCont.SpawnEnemies());
            Debug.Log("Doot Doot");
            Destroy(this.gameObject);
        }
    }
}
