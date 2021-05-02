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
    public PlayerController player;
    public List<EnemyT> enemies = new List<EnemyT>();
    public EnemyT[] EnemyType;
    public Vector2 spawnPoint;
    Vector2 rndPos;
    public bool spawning;
    public int spawnTime;
    public int wave;
    public GameObject door;
    public GameObject hourglass;
    public Animator hourAnim;
    public float animSpeed;
    public float Force;
    public float Knockback;
    public int enemyNumber;
    public Pointer pointer;
    public GameObject[] rooms;
    public int roomNumber;
    public int currentRoom;
    public bool roomComplete;
    public GameObject pSpawn;
    public GameObject eSpawn;
    new BoxCollider2D collider;
    public Image sceneFader;
    public TextMeshProUGUI PickupText;

    void Awake()
    {
        PlayerPrefs.SetInt("tutorialDone", 1);
        PlayerPrefs.Save();

        Force = 5.5f;
        Knockback = 175;
        enemyNumber = PlayerPrefs.GetInt("lastScene", 3);
        attackTimer = PlayerPrefs.GetInt("turnTimer", 5);
        spawning = true;
        hourAnim = hourglass.GetComponent<Animator>();

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
        if (enemies.Count == 0 && spawning == false)
        {
            StopAllCoroutines();
            hourAnim.SetBool("fightTime", false);
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

    public IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(spawnTime);
        Audio.Play("EnemySpawn");

        spawnPoint = eSpawn.transform.position;

        // Spawn enemy here

        StartCoroutine(EnemyAttack());
        hourAnim.SetBool("fightTime", true);
        spawning = false;
    }

    public void SetUpNextRoom()
    {
        Collider2D collider = door.GetComponent<Collider2D>();
        collider.isTrigger = true;
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
