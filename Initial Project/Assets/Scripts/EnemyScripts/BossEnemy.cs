using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    GameObject controller;
    GameObject audioManager;
    EnemyController controllerScript;
    public float switchthreshold;
    private int FormList;
    Enemy enemyScript;
    public Sprite[] spriteList;
    private SpriteRenderer spriteR;
    public Animator enemyAnim;
    public AudioManager Audio;

    // Start is called before the first frame update
    void Start()
    {
        spriteR = GetComponent<SpriteRenderer>();
        enemyScript = GetComponent<Enemy>();
        controller = GameObject.Find("EnemyController");
        audioManager = GameObject.Find("AudioManager");
        Audio = audioManager.GetComponent<AudioManager>();
        controllerScript = controller.GetComponent<EnemyController>();
        enemyScript.health = 25;
        switchthreshold = 15;
        FormList = 1;
        SwitchForm();
        enemyScript.currentDamage = 5;
        enemyScript.currentSize = 3f;
        
    }

    // Update is called once per frame
    void Update()
    {
       
        if (enemyScript.health <= switchthreshold)
        {
            Audio.Play("BossShieldBreak");
            FormList = 0;
            SwitchForm();
            switchthreshold = enemyScript.health / 2;
            StartCoroutine(SwitchBack());
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
           /* case 2:
                gameObject.tag = "spikyEnemy";
                break; */
        }
    }

    IEnumerator SwitchBack()
    {
        yield return new WaitForSeconds(10f);
        FormList = 1;
        Audio.Play("BossReapplyingArmour");
        SwitchForm();
    }

    
      
 
}

