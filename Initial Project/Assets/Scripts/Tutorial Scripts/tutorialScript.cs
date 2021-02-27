using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class tutorialScript : MonoBehaviour
{
   
    public int attackTimer;
    float timeLeft;
    public attackTimer timeBar;
    public bool attacking;
    public OptionsMenu Options;

    public EnemyT enemyScript;
    public AudioManager Audio;
    public PlayerController player;
    public List<GameObject> enemies = new List<GameObject>();
    public GameObject basicEnemy;
    public GameObject armourEnemy;
    public GameObject spikeEnemy;
    public Vector2 enemySpawnPoint;
    public int spawnTime;
    public int wave;
    public GameObject door;
    

    void Start()
    {
        PlayerPrefs.SetInt("tutorialDone", 1);
        PlayerPrefs.Save();

        wave = -1;
        attackTimer = PlayerPrefs.GetInt("turnTimer", 5);

        Audio.Play("BossReapplyingArmour");
    }

    // Update is called once per frame
    void Update()
    {
        if (attacking == true && timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timeBar.SetTime(timeLeft);
        }

        if (attacking == false)
        {
            timeLeft = attackTimer;
            timeBar.SetMaxTime(attackTimer);
        }
    }

    IEnumerator EnemyAttack()
    {
        StartTimer();
        yield return new WaitForSeconds(attackTimer - 0.3f);
        foreach (GameObject enemy in enemies)
        {
            enemyScript = enemy.GetComponent<EnemyT>();
            enemyScript.enemyAnim.SetTrigger("Attack");

        }
        yield return new WaitForSeconds(0.3f);
        foreach (GameObject enemy in enemies)
        {
            enemyScript = enemy.GetComponent<EnemyT>();
            yield return new WaitForSeconds(0.1f);
            enemyScript.Attack();
        }

        StartCoroutine(EnemyAttack());
    }

    public IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(0.5f);
        
        if (wave < 4)
            {

                Audio.Play("EnemySpawn"); 

                if (wave == 0)
                {
                    enemies.Add((GameObject)Instantiate(basicEnemy, enemySpawnPoint, Quaternion.identity));
                    wave += 1;
                }

                else if (wave == 1)
                {
                    enemies.Add((GameObject)Instantiate(armourEnemy, enemySpawnPoint, Quaternion.identity));
                    wave += 1;
                }

                else if (wave == 2)
                {
                    enemies.Add((GameObject)Instantiate(spikeEnemy, enemySpawnPoint, Quaternion.identity));
                    wave += 1;
                }

                else if (wave == 3)
                {
                    enemies.Add((GameObject)Instantiate(basicEnemy, enemySpawnPoint, Quaternion.identity));

                    enemies.Add((GameObject)Instantiate(armourEnemy, enemySpawnPoint, Quaternion.identity));

                    enemies.Add((GameObject)Instantiate(spikeEnemy, enemySpawnPoint, Quaternion.identity));
                    wave += 1;
                }

                StartCoroutine(EnemyAttack());
        }
            else
            {
                door.SetActive(true);
            }
        
    }

    void StartTimer()
    {
        timeLeft = attackTimer;
        timeBar.SetMaxTime(attackTimer);
        attacking = true;
    }
}
