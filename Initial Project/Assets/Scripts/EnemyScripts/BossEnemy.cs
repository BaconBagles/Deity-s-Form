using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    GameObject controller;
    EnemyController controllerScript;
    public int currentHealth;
    private int FormList;
    Enemy enemyScript;
    public Sprite[] spriteList;
    private SpriteRenderer spriteR;
    public Animator enemyAnim;

    // Start is called before the first frame update
    void Start()
    {
        spriteR = GetComponent<SpriteRenderer>();
        enemyScript = GetComponent<Enemy>();
        controller = GameObject.Find("EnemyController");
        controllerScript = controller.GetComponent<EnemyController>();
        controllerScript.enemies.Add(this.gameObject);
        enemyScript.health = 25;
        currentHealth = enemyScript.health;
        FormList = 1;
        SwitchForm();
        enemyScript.currentDamage = 5;
        enemyScript.currentSize = 3f;
        
    }

    // Update is called once per frame
    void Update()
    {
       
        if (enemyScript.health <= currentHealth-5)
        {
            SwitchForm();
            currentHealth -= 5;
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
                FormList = 1;
                break;
            case 1:
                gameObject.tag = "armourEnemy";
                enemyAnim.SetInteger("EnemyType", 0);
                spriteR.sprite = spriteList[0];
                FormList = 0;
                break;
           /* case 2:
                gameObject.tag = "spikyEnemy";
                break; */
        }
    }

    
      
 
}

