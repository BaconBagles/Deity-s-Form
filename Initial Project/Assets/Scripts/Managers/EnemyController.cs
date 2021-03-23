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
    public AudioManager Audio;
    public PlayerController player;
    public GameController gameController;
    public List<Enemy> enemies = new List<Enemy>(); 
    public Enemy[] EnemyType; 
    public Enemy Boss; 
    Vector2 rndPos;
    public bool spawning;
    public int spawnTime;
    bool bossSpawned;
    Vector2 spawnPoint;

    void Start()
    {
        attackTimer = PlayerPrefs.GetInt("turnTimer", 5);
        spawning = true;
       StartCoroutine(SpawnEnemies());
    }


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
        foreach (Enemy enemy in enemies)
        {
          enemy.enemyAnim.SetTrigger("Attack");
        }
        yield return new WaitForSeconds(0.3f);
        foreach (Enemy enemy in enemies)
        {
            yield return new WaitForSeconds(0.1f);
            enemy.Attack();
        }

        StartCoroutine(EnemyAttack());
    }

    public IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(spawnTime);
        Audio.Play("EnemySpawn");

        spawnPoint = gameController.eSpawn.transform.position;

        if (gameController.currentRoom == 8 && bossSpawned == false)
        {
             bossSpawned = true;
             rndPos = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10));
             Enemy boss = Instantiate(Boss, spawnPoint + rndPos, Quaternion.identity, transform);
             enemies.Add(boss);
         }
        else
        {
            for (int i = 0; i < diffLevel; i++)
            {
                rndPos = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10));
                int enemyType = Random.Range(0, EnemyType.Length);
                Enemy enemy = Instantiate(EnemyType[enemyType], spawnPoint + rndPos , Quaternion.identity, transform);
                enemies.Add(enemy);
            }
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
