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
    public AudioManager Audio;
    public PlayerController player;
    public List<EnemyT> enemies = new List<EnemyT>();
    public EnemyT[] EnemyType;
    public Vector2 enemySpawnPoint;
    Vector2 rndPos;
    public int spawnTime;
    public int wave;
    public GameObject door;
    public GameObject hourglass;
    public Animator hourAnim;
    public float animSpeed;


    void Start()
    {
        PlayerPrefs.SetInt("tutorialDone", 1);
        PlayerPrefs.Save();

        wave = -1;
        attackTimer = PlayerPrefs.GetInt("turnTimer", 5);
        hourAnim = hourglass.GetComponent<Animator>();
        animSpeed = 1f / attackTimer;
        hourAnim.SetFloat("speed", animSpeed);
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

        if(enemies.Count == 0)
        {
            hourAnim.SetBool("fightTime", false);
        }
    }

    IEnumerator EnemyAttack()
    {
        StartTimer();
        yield return new WaitForSeconds(attackTimer - 0.3f);
        foreach (EnemyT enemy in enemies)
        {
            enemy.enemyAnim.SetTrigger("Attack");

        }
        yield return new WaitForSeconds(0.3f);
        foreach (EnemyT enemy in enemies)
        {
            yield return new WaitForSeconds(0.1f);
            enemy.Attack();
        }

        StartCoroutine(EnemyAttack());
    }

    public IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(0.5f);
        hourAnim.SetBool("fightTime", true);

        if (wave < 4)
            {
                Audio.Play("EnemySpawn"); 

                if (wave == 0)
                {
                rndPos = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10));
                EnemyT enemy = Instantiate(EnemyType[0], enemySpawnPoint + rndPos, Quaternion.identity, transform);
                enemies.Add(enemy);
                wave += 1;
                }

                else if (wave == 1)
                {
                rndPos = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10));
                EnemyT enemy = Instantiate(EnemyType[1], enemySpawnPoint + rndPos, Quaternion.identity, transform);
                enemies.Add(enemy);
                wave += 1;
                }

                else if (wave == 2)
                {
                rndPos = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10));
                EnemyT enemy = Instantiate(EnemyType[2], enemySpawnPoint + rndPos, Quaternion.identity, transform);
                enemies.Add(enemy);
                wave += 1;
                }

                else if (wave == 3)
                {
                  rndPos = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10));
                  EnemyT enemy = Instantiate(EnemyType[0], enemySpawnPoint + rndPos, Quaternion.identity, transform);
                  enemies.Add(enemy);

                  EnemyT enemy1 = Instantiate(EnemyType[1], enemySpawnPoint + rndPos, Quaternion.identity, transform);
                  enemies.Add(enemy1);

                  EnemyT enemy2 = Instantiate(EnemyType[2], enemySpawnPoint + rndPos, Quaternion.identity, transform);
                  enemies.Add(enemy2);

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
