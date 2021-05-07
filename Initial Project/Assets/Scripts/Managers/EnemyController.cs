using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int attackTimer;
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
    public float Force;
    public float Knockback;
    public Vector2 spawnPoint;
    public int enemyNumber;
    public int maxEnemies;
    public int diffHealthMod;
    public int diffDamageMod;

    void Awake()
    {
        Force = 5.5f;
        Knockback = 175;
        enemyNumber = PlayerPrefs.GetInt("lastScene", 3);
        attackTimer = PlayerPrefs.GetInt("turnTimer", 5);
        spawning = true;
        gameController.dCont.CheckDiff();
        StartCoroutine(SpawnEnemies());
    }


    void Update()
    {
        if (enemies.Count == 0 && spawning == false)
        {
            StopAllCoroutines();
            gameController.hourAnim.SetBool("fightTime", false);
            if (gameController.roomComplete == false)
            {
                attacking = false;
                spawning = true;
                StartCoroutine(SpawnEnemies());
            }
        }
    }

    IEnumerator EnemyAttack()
    {
        attacking = true;
        yield return new WaitForSeconds(attackTimer - 0.25f);
        
        foreach (Enemy enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.enemyAnim.SetTrigger("Attack");
                yield return new WaitForSeconds(0.25f);
                enemy.StartCoroutine(enemy.Attack());
            }
        }

        StartCoroutine(EnemyAttack());
    }

    public IEnumerator SpawnEnemies()
    {
        spawning = true;
        yield return new WaitForSeconds(spawnTime);
        Audio.Play("EnemySpawn");

        spawnPoint = gameController.eSpawn.transform.position;

        if (gameController.currentRoom == gameController.bRoomNum && bossSpawned == false)
        {
            bossSpawned = true;
            Enemy boss = Instantiate(Boss, spawnPoint, Quaternion.identity, transform);
            enemies.Add(boss);
        }
        else
        {
            if(enemyNumber == 3)
            {
                for (int i = 0; i < maxEnemies; i++)
                {
                    int enemyType = Random.Range(0, EnemyType.Length - 3);
                    Enemy enemy = Instantiate(EnemyType[enemyType], spawnPoint, Quaternion.identity, transform);
                    enemy.maxHealth += diffHealthMod;
                    enemy.health += diffHealthMod;
                    enemy.currentDamage += diffDamageMod;
                    enemies.Add(enemy);
                }
            }
            else if (enemyNumber == 4)
            {
                for (int i = 0; i < maxEnemies; i++)
                {
                    int enemyType = Random.Range(0, EnemyType.Length - 2);
                    Enemy enemy = Instantiate(EnemyType[enemyType], spawnPoint, Quaternion.identity, transform);
                    enemy.maxHealth += diffHealthMod;
                    enemy.health += diffHealthMod;
                    enemy.currentDamage += diffDamageMod;
                    enemies.Add(enemy);
                }
            }
            else if (enemyNumber == 5)
            {
                for (int i = 0; i < maxEnemies; i++)
                {
                    int enemyType = Random.Range(0, EnemyType.Length - 1);
                    Enemy enemy = Instantiate(EnemyType[enemyType], spawnPoint, Quaternion.identity, transform);
                    enemy.maxHealth += diffHealthMod;
                    enemy.health += diffHealthMod;
                    enemy.currentDamage += diffDamageMod;
                    enemies.Add(enemy);
                }
            }
            else if (enemyNumber == 6)
            {
                for (int i = 0; i < maxEnemies; i++)
                {
                    int enemyType = Random.Range(0, EnemyType.Length);
                    Enemy enemy = Instantiate(EnemyType[enemyType], spawnPoint, Quaternion.identity, transform);
                    enemy.maxHealth += diffHealthMod;
                    enemy.health += diffHealthMod;
                    enemy.currentDamage += diffDamageMod;
                    enemies.Add(enemy);
                }
            }
        }

        gameController.waveNum += 1;
        StartCoroutine(EnemyAttack());
        gameController.hourAnim.SetBool("fightTime", true);
        spawning = false;
    }
    
}
