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
    public int firstForm;
    public int secondForm;
    public bool isFinalBoss;
    public int bossHealth;
    public int bossDamage;

    // Start is called before the first frame update
    void Start()
    {
        spriteR = GetComponent<SpriteRenderer>();
        enemyScript = GetComponent<Enemy>();
        controller = GameObject.Find("EnemyController");
        Audio = FindObjectOfType<AudioManager>();
        controllerScript = controller.GetComponent<EnemyController>();
        enemyScript.health = bossHealth;
        switchthreshold = 15;
        FormList = firstForm;
        SwitchForm();
        enemyScript.currentDamage = bossDamage;
        enemyScript.currentSize = 3f;
        enemyScript.isBoss = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (enemyScript.health <= switchthreshold && isFinalBoss == false)
        {
            Audio.Play("BossShieldBreak");
            FormList = secondForm;
            SwitchForm();
            switchthreshold = enemyScript.health / 2;
            StartCoroutine(SwitchBack());
        }
        else if (enemyScript.health <= switchthreshold && FormList <= 2)
        {
            Audio.Play("BossShieldBreak");
            FormList++;
            SwitchForm();
            switchthreshold = enemyScript.health / 2;
        } 
        
       if (controllerScript.attacking == true && controllerScript.enemies.Count == 1)
       {
          controllerScript.StopAllCoroutines();
          StartCoroutine(controllerScript.SpawnEnemies());
          controllerScript.attacking = false;
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
               
                break;
            case 1:
                gameObject.tag = "armourEnemy";
                spriteR.sprite = spriteList[0];
                enemyAnim.SetInteger("EnemyType", 0);
                
                
                break;
            case 2:
                gameObject.tag = "spikyEnemy";
                spriteR.sprite = spriteList[2];
                enemyAnim.SetInteger("EnemyType", 1);
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

