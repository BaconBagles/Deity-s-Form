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

   // public Enemy enemyScript;
    public AudioManager Audio;
    public PlayerController player;
    public GameController gameController;
    public List<Enemy> enemies = new List<Enemy>(); // use for Agents
    public Enemy basicEnemy; // use for agentPrefab
    public Enemy armourEnemy; // use for agentPrefab
    public Enemy spikeEnemy; // use for agentPrefab
    public Enemy bossEnemy; // use for agentPrefab
    //Vector2 rndPos;
    public bool spawning;
    public int spawnTime;
    bool bossSpawned;

    //Enemy AI stuff
    public EnemyFlockBehaviour behaviour;
    const float AgentDensity = 0.08f;
    [Range(1f, 100f)]
    public float driveFactor = 10f;
    [Range(1f, 100f)]
    public float maxSpeed = 5f;
    [Range(1f, 10f)]
    public float neightbourRadius = 1.5f;
    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplier = 0.5f;
    float squareMaxSpeed;
    float squareNeighbourRadius;
    float squareAvoidanceRadius;
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }
    public Vector3 spawnPoint;

    void Start()
    {
        attackTimer = PlayerPrefs.GetInt("turnTimer", 5);
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighbourRadius = neightbourRadius * neightbourRadius;
        squareAvoidanceRadius = squareAvoidanceRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

       spawning = true;
       StartCoroutine(SpawnEnemies());
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Enemy enemy in enemies)
        {
            List<Transform> context = GetNearbyObjects(enemy);


            Vector2 move = behaviour.CalculateMove(enemy, context, this);
            move *= driveFactor;
            if(move.sqrMagnitude > squareMaxSpeed)
            {
                move = move.normalized * maxSpeed;
            }
            enemy.Move(move);
        }

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

    List<Transform> GetNearbyObjects(Enemy enemy)
    {
        List<Transform> context = new List<Transform>();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(enemy.transform.position, neightbourRadius);
        foreach(Collider2D c in contextColliders)
        {
            if (c != enemy.AgentCollider)
            {
                context.Add(c.transform);
            }
        }

        return context;
    }

    IEnumerator EnemyAttack()
    {
        StartTimer();
        yield return new WaitForSeconds(attackTimer - 0.3f);
        foreach (Enemy enemy in enemies)
        {
          //  enemyScript = enemy.GetComponent<Enemy>();
            enemy.enemyAnim.SetTrigger("Attack");
            
        }
        yield return new WaitForSeconds(0.3f);
        foreach (Enemy enemy in enemies)
        {
          //  enemyScript = enemy.GetComponent<Enemy>();
            yield return new WaitForSeconds(0.1f);
            enemy.Attack();
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
           // rndPos = new Vector2(Random.Range(-20, 20), Random.Range(-20, 20));
            Enemy boss = Instantiate(bossEnemy, Random.insideUnitCircle * diffLevel * AgentDensity, Quaternion.identity, transform);
            enemies.Add(boss);
        }
        else
        {
            for (int i = 0; i < diffLevel; i++)
            {
              //  rndPos = new Vector2(Random.Range(-20, 20), Random.Range(-20, 20));
                int enemyType = Random.Range(0,3);
                switch (enemyType)
                {
                    case 0:
                       Enemy basic = Instantiate(basicEnemy, Random.insideUnitCircle * diffLevel * AgentDensity, Quaternion.identity, transform);
                       enemies.Add(basic);
                       break;
                    case 1:
                        Enemy armour = Instantiate(armourEnemy, Random.insideUnitCircle * diffLevel * AgentDensity, Quaternion.identity, transform);
                        enemies.Add(armour);
                        break;
                    case 2:
                        Enemy spike =  Instantiate(spikeEnemy, Random.insideUnitCircle * diffLevel * AgentDensity, Quaternion.identity, transform);
                        enemies.Add(spike);
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
