using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class tutorialScript : MonoBehaviour
{
   
    public int attackTimer;
    float timeLeft;
    public attackTimer timeBar;
    public bool attacking;
    public OptionsMenu Options;
    public AudioManager Audio;
    public playerTutorial player;
    public tutorialTipsScript[] tTips;
    public List<EnemyT> enemies = new List<EnemyT>();
    public EnemyT[] EnemyType;
    public GameObject[] spawnPoint;
    Vector2 rndPos;
    public bool spawning;
    public int spawnTime;
    public int wave;
    public GameObject[] door;
    public GameObject hourglass;
    public Animator hourAnim;
    public float animSpeed;
    public float Force;
    public float Knockback;
    public int enemyNumber;
    public pointerTutorial pointer;
    public GameObject[] rooms;
    public int roomNumber;
    public int currentRoom;
    public bool roomComplete;
    public GameObject[] pSpawn;
    new BoxCollider2D collider;
    public Image sceneFader;
    public TextMeshProUGUI PickupText;
    public bool enemySpawn;
    public int tutorialStage;
    public int tutorialWave;
    public bool fightOver;

    void Awake()
    {
        PlayerPrefs.SetInt("tutorialDone", 1);
        PlayerPrefs.Save();

        Force = 5.5f;
        Knockback = 175;
        enemyNumber = PlayerPrefs.GetInt("lastScene", 3);
        attackTimer = PlayerPrefs.GetInt("turnTimer", 5);
        spawning = true;
        roomComplete = false;
        fightOver = true;
        hourAnim = hourglass.GetComponent<Animator>();
        spawnTime = 1;
        tutorialStage = 0;
        tutorialWave = 0;
        currentRoom = 0;
        FadeIn();
    }

    void Start()
    {
        attackTimer = PlayerPrefs.GetInt("turnTimer", 5);
        hourAnim = hourglass.GetComponent<Animator>();
        animSpeed = 1f / attackTimer;

        hourAnim.SetFloat("speed", animSpeed);
        Audio.Play("BossReapplyingArmour");
    }

    // Update is called once per frame
    void Update()
    {
        if (enemies.Count == 0 && spawning == false && fightOver == false)
        {
            StopAllCoroutines();
            tutorialStage++;
            TutorialStep();
            hourAnim.SetBool("fightTime", false);
            fightOver = true;
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

        if (roomComplete == true && enemies.Count == 0)
        {
            pointer.gameObject.SetActive(true);
            pointer.isDoor = true;
        }
    }

    IEnumerator EnemyAttack()
    {
        StartTimer();
        yield return new WaitForSeconds(attackTimer - 0.25f);

        foreach (EnemyT enemy in enemies)
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

    public void TutorialStep()
    {

        switch (tutorialStage)
        {
            case 0:
                {
                    tTips[0].nextDots.gameObject.SetActive(true);
                    tutorialStage = 1;
                }
                break;
            case 1:
                tutorialWave = 0;
                StartCoroutine(SpawnEnemies());

                break;
            case 2:
                tTips[1].nextDots.gameObject.SetActive(true);
                tutorialStage = 3;
                break;
            case 3:
                tutorialWave = 1;
                StartCoroutine(SpawnEnemies());
                break;
            case 4:
                tTips[2].nextDots.gameObject.SetActive(true);
                tutorialStage = 5;
                break;
            case 5:
                tutorialWave = 2;
                StartCoroutine(SpawnEnemies());
                break;
            case 6:
                tTips[3].nextDots.gameObject.SetActive(true);
                tutorialStage = 7;
                break;
            case 7:
                roomComplete = true;
                door[0].SetActive(true);
                tutorialStage = 8;
                break;
            case 8:
                player.formlocked = false;
                tTips[5].nextDots.gameObject.SetActive(true);
                tutorialStage = 9;
                break;
            case 9:
                tutorialWave = 3;
                StartCoroutine(SpawnEnemies());
                break;
            case 10:
                tTips[6].nextDots.gameObject.SetActive(true);
                tutorialStage = 11;
                break;
            case 11:
                tutorialWave = 4;
                StartCoroutine(SpawnEnemies());
                break;
            case 12:
                tTips[7].nextDots.gameObject.SetActive(true);
                tutorialStage = 13;
                break;
            case 13:
                tutorialWave = 5;
                StartCoroutine(SpawnEnemies());
                break;
            case 14:
                tTips[8].nextDots.gameObject.SetActive(true);
                tutorialStage = 15;
                break;
            case 15:
                roomComplete = true;
                door[1].SetActive(true);
                tutorialStage = 16;
                break;
            case 16:
                player.hawklocked = false;
                tutorialWave = 6;
                StartCoroutine(SpawnEnemies());
                break;
            case 17:
                tTips[10].nextDots.gameObject.SetActive(true);
                tutorialStage = 18;
                break;
            case 18:
                tutorialWave = 7;
                StartCoroutine(SpawnEnemies());
                break;
            case 19:
                tTips[11].nextDots.gameObject.SetActive(true);
                tutorialStage = 20;
                break;
            case 20:
                roomComplete = true;
                door[2].SetActive(true);
                break;
        }
    }

    public IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(spawnTime);
        Audio.Play("EnemySpawn");


        switch (tutorialWave)
        {
            case 0:
                EnemyT enemy = Instantiate(EnemyType[0], spawnPoint[0].transform.position, Quaternion.identity, transform);
                enemies.Add(enemy);
                break;
            case 1:
                EnemyT enemy1 = Instantiate(EnemyType[0], spawnPoint[0].transform.position, Quaternion.identity, transform);
                enemies.Add(enemy1);
                EnemyT enemy2 = Instantiate(EnemyType[0], spawnPoint[0].transform.position, Quaternion.identity, transform);
                enemies.Add(enemy2);
                break;
            case 2:
                EnemyT enemy3 = Instantiate(EnemyType[1], spawnPoint[0].transform.position, Quaternion.identity, transform);
                enemies.Add(enemy3);
                break;
            case 3:
                EnemyT enemy4 = Instantiate(EnemyType[1], spawnPoint[1].transform.position, Quaternion.identity, transform);
                enemies.Add(enemy4);
                break;
            case 4:
                EnemyT enemy5 = Instantiate(EnemyType[0], spawnPoint[1].transform.position, Quaternion.identity, transform);
                enemies.Add(enemy5);
                break;
            case 5:
                EnemyT enemy6 = Instantiate(EnemyType[0], spawnPoint[1].transform.position, Quaternion.identity, transform);
                enemies.Add(enemy6);
                EnemyT enemy7 = Instantiate(EnemyType[1], spawnPoint[1].transform.position, Quaternion.identity, transform);
                enemies.Add(enemy7);
                EnemyT enemy8 = Instantiate(EnemyType[0], spawnPoint[1].transform.position, Quaternion.identity, transform);
                enemies.Add(enemy8);
                EnemyT enemy9 = Instantiate(EnemyType[1], spawnPoint[1].transform.position, Quaternion.identity, transform);
                enemies.Add(enemy9);
                break;
            case 6:
                EnemyT enemy10 = Instantiate(EnemyType[2], spawnPoint[2].transform.position, Quaternion.identity, transform);
                enemies.Add(enemy10);
                EnemyT enemy11 = Instantiate(EnemyType[2], spawnPoint[2].transform.position, Quaternion.identity, transform);
                enemies.Add(enemy11);
                break;
            case 7:
                EnemyT enemy12 = Instantiate(EnemyType[0], spawnPoint[2].transform.position, Quaternion.identity, transform);
                enemies.Add(enemy12);
                EnemyT enemy13 = Instantiate(EnemyType[1], spawnPoint[2].transform.position, Quaternion.identity, transform);
                enemies.Add(enemy13);
                EnemyT enemy14 = Instantiate(EnemyType[2], spawnPoint[2].transform.position, Quaternion.identity, transform);
                enemies.Add(enemy14);
                EnemyT enemy15 = Instantiate(EnemyType[0], spawnPoint[2].transform.position, Quaternion.identity, transform);
                enemies.Add(enemy15);
                EnemyT enemy16 = Instantiate(EnemyType[1], spawnPoint[2].transform.position, Quaternion.identity, transform);
                enemies.Add(enemy16);
                EnemyT enemy17 = Instantiate(EnemyType[2], spawnPoint[2].transform.position, Quaternion.identity, transform);
                enemies.Add(enemy17);
                break;
        }


        StartCoroutine(EnemyAttack());
        fightOver = false;
        hourAnim.SetBool("fightTime", true);
        spawning = false;
    }


    public IEnumerator HealthAdded()
    {
        PickupText.text = "Health regained";
        PickupText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        PickupText.gameObject.SetActive(false);
    }

    public IEnumerator FadeOut()
    {
        sceneFader.color = Color.black;
        sceneFader.canvasRenderer.SetAlpha(0.0f);
        sceneFader.CrossFadeAlpha(1.0f, 1f, false);
        yield return new WaitForSeconds(1);
        NewRoom();
        FadeIn();
    }

    public void NewRoom()
    {
        currentRoom++;
        roomComplete = false;
        pointer.gameObject.SetActive(false);
        pointer.isDoor = false;
        player.transform.position = pSpawn[currentRoom].transform.position;

    }

    void FadeIn()
    {
        sceneFader.color = Color.black;
        sceneFader.canvasRenderer.SetAlpha(1.0f);
        sceneFader.CrossFadeAlpha(0.0f, 1f, false);

    }

    void StartTimer()
    {
        timeLeft = attackTimer;
        timeBar.SetMaxTime(attackTimer);
        attacking = true;
    }
}
