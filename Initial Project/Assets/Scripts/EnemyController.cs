using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int diffLevel;
    public int attackTimer;
    float timeLeft;
    public attackTimer timeBar;
    bool attacking;

    public PlayerController player;
    public List<GameObject> enemies = new List<GameObject>();
    public GameObject basicEnemy;
    public GameObject armourEnemy;
    public GameObject spikeEnemy;
    Vector2 rndPos;
    bool spawning;
    public int spawnTime;
    

    // Update is called once per frame
    void Update()
    {
        if (enemies.Count == 0 && spawning == false)
        {
            StopAllCoroutines();
            attacking = false;
            spawning = true;
            StartCoroutine(SpawnEnemies());
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
        yield return new WaitForSecondsRealtime(attackTimer);
        foreach (GameObject enemy in enemies)
        {
            player.health -= 1;
        }
        StartCoroutine(EnemyAttack());
    }

    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSecondsRealtime(spawnTime);

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
            enemies.Add((GameObject)Instantiate(spikeEnemy, rndPos, Quaternion.Euler(0,0,45)));
        }

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
