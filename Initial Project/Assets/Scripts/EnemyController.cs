using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int diffLevel;
    public int attackTimer;
    public PlayerController player;
    public List<GameObject> enemies = new List<GameObject>();
    public GameObject basicEnemy;
    public GameObject armourEnemy;
    public GameObject spikeEnemy;
    Vector2 rndPos;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemyAttack());
    }

    // Update is called once per frame
    void Update()
    {
        if (enemies.Count == 0)
        {
            SpawnEnemies();
        }
    }

    IEnumerator EnemyAttack()
    {
        yield return new WaitForSecondsRealtime(attackTimer);
        foreach (GameObject enemy in enemies)
        {
            player.health -= 1;
        }
        StartCoroutine(EnemyAttack());
    }

    void SpawnEnemies()
    {

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
    }
}
