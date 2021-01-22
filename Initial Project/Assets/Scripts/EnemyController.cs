﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int diffLevel;
    private int attackTimer;
    float timeLeft;
    public attackTimer timeBar;
    bool attacking;
    public OptionsMenu Options;

    public Enemy enemyScript;
    public AudioManager Audio;
    public PlayerController player;
    public GameController gameController;
    public List<GameObject> enemies = new List<GameObject>();
    public GameObject basicEnemy;
    public GameObject armourEnemy;
    public GameObject spikeEnemy;
    Vector2 rndPos;
    public bool spawning;
    public int spawnTime;

    void Start()
    {
        attackTimer = PlayerPrefs.GetInt("attackTimer", 5);
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
        yield return new WaitForSeconds(attackTimer);
        foreach (GameObject enemy in enemies)
        {
            enemyScript = enemy.GetComponent<Enemy>();
            enemyScript.Attack();
        }
       Audio.Play("EnemyAttack");
        StartCoroutine(EnemyAttack());
    }

    public IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(spawnTime);
        Audio.Play("EnemySpawn");

        for (int i = 0; i < diffLevel; i++)
        {
            rndPos = new Vector2(Random.Range(-20, 20), Random.Range(-20, 20));
            enemies.Add((GameObject)Instantiate(basicEnemy, rndPos, Quaternion.identity));
        }
        for (int i = 0; i < diffLevel/2; i++)
        {
            rndPos = new Vector2(Random.Range(-20, 20), Random.Range(-20, 20));
            enemies.Add((GameObject)Instantiate(armourEnemy, rndPos, Quaternion.identity));
        }
        for (int i = 0; i <diffLevel/4; i++)
        {
            rndPos = new Vector2(Random.Range(-20, 20), Random.Range(-20, 20));
            enemies.Add((GameObject)Instantiate(spikeEnemy, rndPos, Quaternion.identity));
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
