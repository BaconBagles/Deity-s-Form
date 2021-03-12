using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerProjectileScript : MonoBehaviour
{
    public GameObject hitEffect;
    public float range = 0.2f;
    PlayerController pContd;
    Enemy enemy;
    BossEnemy bossEnemy;
    thePillarScript pillar;

    //knockback stuff
    public float knockbackPower = 175;
    public float knockbackDuration = 2f;

    void Start()
    {
        pContd = FindObjectOfType<PlayerController>();

        range += pContd.rangeIncrease;
        knockbackPower += pContd.knockbackIncrease;
        gameObject.transform.localScale = new Vector3(transform.localScale.x + pContd.attackIncrease, transform.localScale.y, 0);

        if (this.gameObject.tag == "basicAttack")
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
        if (other.gameObject.CompareTag("Pillar"))
        {
            //damage pillar
            pillar = other.gameObject.GetComponent<thePillarScript>();
           // GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
         //   Destroy(effect, 0.5f);
            pillar.pillarState -= 1;
            pillar.PillarDamage();
            Destroy(this.gameObject);
        }
        else if (other.gameObject.CompareTag("Wall"))
        {
          //  GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
           // Destroy(effect, 0.5f);
            Destroy(this.gameObject);
        }
        else
        {
            enemy = other.gameObject.GetComponent<Enemy>();
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(effect, 0.5f);
            StartCoroutine(enemy.GetComponent<Enemy>().Knockback(knockbackDuration, knockbackPower, this.transform));
            if (gameObject.tag == "basicAttack")
            {
                if (other.gameObject.CompareTag("basicEnemy"))
                {
                    enemy.health -= 5;
                }
                else
                {
                    enemy.health -= 1;
                }
            }
            else if (gameObject.tag == "APAttack")
            {
                if (other.gameObject.CompareTag("armourEnemy"))
                {
                    enemy.health -= 7;
                }
                else
                {
                    enemy.health -= 1;
                }
            }
            else if (gameObject.tag == "rangedAttack")
            {
                if (other.gameObject.CompareTag("spikyEnemy"))
                {
                    enemy.health -= 5;
                }
                else
                {
                    enemy.health -= 1;
                }
            }
        }
    
    
        Destroy(this.gameObject);
    }

    IEnumerator JackalSelfDestruct()
    {
        yield return new WaitForSeconds(range);

        Destroy(this.gameObject);
    }

    IEnumerator HawkSelfDestruct()
    {
        yield return new WaitForSeconds(range * 2);

        Destroy(this.gameObject);
    }

    IEnumerator BullSelfDestruct()
    {
        yield return new WaitForSeconds(range);

        Destroy(this.gameObject);
    }
}
