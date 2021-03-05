using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int diffLevel;
    public int attackTimer;
    float timeLeft;
    public attackTimer timeBar;
    public bool attacking;
    public OptionsMenu Options;

    public Enemy enemyScript;
    public AudioManager Audio;
    public PlayerController player;
    public GameController gameController;
    public List<GameObject> enemies = new List<GameObject>();
    public GameObject basicEnemy;
    public GameObject armourEnemy;
    public GameObject spikeEnemy;
    public GameObject bossEnemy;
    Vector2 rndPos;
    public bool spawning;
    public int spawnTime;
    bool bossSpawned;

    void Start()
    {
        attackTimer = PlayerPrefs.GetInt("turnTimer", 5);

        spawning = true;
       StartCoroutine(SpawnEnemies());
    }

    // Update is called once per frame
    void Update()
    {

        if (enemies.Count == 0 && spawning == false)
        {
            StopAllCoroutines();
            if (gameController.roomComplete == false)
            {
                attacking = false;
                spawning = true;
                StartCoroutine(SpawnEnemies());
            }
        }
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
            enemyScript = enemy.GetComponent<Enemy>();
            enemyScript.enemyAnim.SetTrigger("Attack");
            
        }
        yield return new WaitForSeconds(0.3f);
        foreach (GameObject enemy in enemies)
        {
            enemyScript = enemy.GetComponent<Enemy>();
            yield return new WaitForSeconds(0.1f);
            enemyScript.Attack();
        } 

        StartCoroutine(EnemyAttack());
    }

    public IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(spawnTime);
        Audio.Play("EnemySpawn");

        if (gameController.currentRoom == 8 && bossSpawned == false)
        {
            bossSpawned = true;
            rndPos = new Vector2(Random.Range(-20, 20), Random.Range(-20, 20));
            enemies.Add((GameObject)Instantiate(bossEnemy, rndPos, Quaternion.identity));
        }
        else
        {
            for (int i = 0; i < diffLevel; i++)
            {
                rndPos = new Vector2(gameController.eSpawn.transform.position.x + Random.Range(-5, 5), gameController.eSpawn.transform.position.y + Random.Range(-5, 5));
                int enemyType = Random.Range(0,3);
                switch (enemyType)
                {
                    case 0:
                        enemies.Add((GameObject)Instantiate(basicEnemy, rndPos, Quaternion.identity));
                        break;
                    case 1:
                        enemies.Add((GameObject)Instantiate(armourEnemy, rndPos, Quaternion.identity));
                        break;
                    case 2:
                        enemies.Add((GameObject)Instantiate(spikeEnemy, rndPos, Quaternion.identity));
                        break;
                }
                
            }
            /*for (int i = 0; i < diffLevel / 2; i++)
            {
                rndPos = new Vector2(Random.Range(-20, 20), Random.Range(-20, 20));
                enemies.Add((GameObject)Instantiate(armourEnemy, rndPos, Quaternion.identity));
            }
            for (int i = 0; i < diffLevel / 4; i++)
            {
                rndPos = new Vector2(Random.Range(-20, 20), Random.Range(-20, 20));
                enemies.Add((GameObject)Instantiate(spikeEnemy, rndPos, Quaternion.identity));
            }*/
        }

        gameController.waveNum += 1;
        StartCoroutine(EnemyAttack());

        spawning = false;
    }

    void StartTimer()
    {
        timeLeft = attackTimer;
        timeBar.SetMaxTime(attackTimer);
        attacking = true;
    }
}
