using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

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

    }

    // Update is called once per frame
    void Update()
    {
        if (enemies.Count == 0)
        {
            SpawnEnemies();
        }
        rndPos = new Vector2(Random.Range(-20, 20), Random.Range(-20, 20));
    }

    IEnumerator EnemyAttack()
    {
        yield return new WaitForSeconds(attackTimer);
        foreach (GameObject enemy in enemies)
        {
            player.health -= 1;
        }
        StartCoroutine(EnemyAttack());
    }

    void SpawnEnemies()
    {
            enemies.Add((GameObject)Instantiate(basicEnemy,rndPos, Quaternion.identity));
    }

    public void CheckNull()
    {
        enemies.RemoveAll(item => item == null);
    }
}
