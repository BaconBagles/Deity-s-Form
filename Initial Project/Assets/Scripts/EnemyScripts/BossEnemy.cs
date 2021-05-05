using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    GameObject controller;
    EnemyController controllerScript;
    public float switchthreshold;
    private int FormList;
    Enemy enemyScript;
    public Sprite[] spriteList;
    private SpriteRenderer spriteR;
    public Animator enemyAnim;
    public AudioManager Audio;
    public int bossNum;
    public int firstForm;
    public int secondForm;
    public bool isFinalBoss;
    public int bossHealth;
    public int bossDamage;
    public ParticleSystem deathEffect;

    // Start is called before the first frame update
    void Start()
    {
        spriteR = GetComponent<SpriteRenderer>();
        enemyScript = GetComponent<Enemy>();
        controller = GameObject.Find("EnemyController");
        Audio = FindObjectOfType<AudioManager>();
        controllerScript = controller.GetComponent<EnemyController>();
        enemyScript.maxHealth = bossHealth;
        enemyScript.health = bossHealth;
        enemyScript.spaceBetween = Random.Range(15, 20);
        switchthreshold = enemyScript.health * 0.75f;
        FormList = firstForm;
        SwitchForm();
        enemyScript.currentDamage = bossDamage;
        enemyScript.currentSize = 3f;
        enemyScript.isBoss = true;
        enemyScript.bossNum = bossNum;
    }

    // Update is called once per frame
    void Update()
    {

        if (enemyScript.health <= switchthreshold && isFinalBoss == false)
        {
            Audio.Play("BossShieldBreak");
            FormList = secondForm;
            SwitchForm();
            switchthreshold = enemyScript.health * 0.75f;
            StartCoroutine(SwitchBack());
        }
        else if (enemyScript.health <= switchthreshold && FormList <= 2)
        {
            Audio.Play("BossShieldBreak");
            FormList++;
            SwitchForm();
            switchthreshold = enemyScript.health * 0.75f;
        } 
        
       if (controllerScript.attacking == true && controllerScript.enemies.Count <= 1)
       {
          controllerScript.StopAllCoroutines();
          StartCoroutine(controllerScript.SpawnEnemies());
          controllerScript.attacking = false;
       }

       if (enemyScript.health == 0)
       {
           Instantiate(deathEffect, transform.position, Quaternion.identity);
       }

       if(FormList > 2)
        {
            FormList = 0;
        }
    }

    public void SwitchForm()
    {

        switch (FormList)
        {
            case 0:
                gameObject.tag = "basicEnemy";
                spriteR.sprite = spriteList[1];
                enemyAnim.SetInteger("EnemyType", 2);
                enemyScript.spaceBetween = Random.Range(15, 20);
                if (isFinalBoss == true)
                {
                    enemyScript.currentSprite = 0;
                }

                break;
            case 1:
                gameObject.tag = "armourEnemy";
                spriteR.sprite = spriteList[0];
                enemyAnim.SetInteger("EnemyType", 0);
                enemyScript.spaceBetween = Random.Range(15, 20);
                if (isFinalBoss == true)
                {
                    enemyScript.currentSprite = 1;
                }
                                
                break;
            case 2:
                gameObject.tag = "spikyEnemy";
                spriteR.sprite = spriteList[2];
                enemyAnim.SetInteger("EnemyType", 1);
                enemyScript.spaceBetween = Random.Range(15, 20);
                if (isFinalBoss == true)
                {
                    enemyScript.currentSprite = 2;
                }
                break; 
        }
    }

    IEnumerator SwitchBack()
    {
        yield return new WaitForSeconds(10f);
        FormList = firstForm;
        Audio.Play("BossReapplyingArmour");
        SwitchForm();
    }

    
      
 
}

