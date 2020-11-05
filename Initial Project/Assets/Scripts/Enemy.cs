using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    GameObject controller;
    public EnemyController controllerScript;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("EnemyController");
        controllerScript = controller.GetComponent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("basicAttack"))
        {
            if (this.gameObject.tag == "basicEnemy")
            {
                controllerScript.enemies.Remove(this.gameObject);
                Destroy(this.gameObject);
            }
        }
        else if (other.gameObject.CompareTag("APAttack"))
        {
            if (this.gameObject.tag == "armourEnemy")
            {
                Destroy(this.gameObject);
            }
        }
        else if(other.gameObject.CompareTag("rangedAttack"))
        {
            if (this.gameObject.tag == "spikyEnemy")
            {
                Destroy(this.gameObject);
            }
        }
    }
}
