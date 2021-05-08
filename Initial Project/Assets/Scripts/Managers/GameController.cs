using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Rendering.Universal;


public class GameController : MonoBehaviour
{
    public EnemyController eCont;
    public PlayerController player;
    public DifficultyManager dCont;
    public AudioManager aMan;
    public Pointer pointer;
    public GameObject[] rooms;
    public GameObject pickup;
    public GameObject memory;
    public TMPro.TextMeshProUGUI PickupText;
    public int roomNumber;
    public ProgressBar progressBar;

    public int pickupNumber;
    public bool pickupSpawned;
    public bool allMemories;
    public int deleteSave;

    public int currentRoom;
    public int waveMax;
    public int waveNum;
    public bool roomComplete;
    public bool bossRoom;
    public int bRoomNum;
    public DialogueTrigger[] bossConversations;
    public GameObject door;
    public GameObject pSpawn;
    public GameObject eSpawn;
    public GameObject memSpawn;
    new BoxCollider2D collider;
    bool spawnMemory;
    public int currentScene;
    public Image sceneFader;
    public GameObject hourglass;
    public Animator hourAnim;
    public float animSpeed;

    public List<GameObject> pickups = new List<GameObject>();
    Vector3 pickupDist = new Vector3(10, 0, 0);

    void Awake()
    {
        deleteSave = PlayerPrefs.GetInt("deleteSave", 0);
        PlayerPrefs.Save();
        hourAnim = hourglass.GetComponent<Animator>();
        if (PlayerPrefs.GetInt("Memory3", 0) == 1)
        {
            allMemories = true;
        }
        else
        {
            allMemories = false;
        }
        if (deleteSave == 1)
        {
            LoadGame();
        }
        else if (deleteSave == 2)
        {
            LoadReset();
        }
        dCont.CheckDiff();
        FadeIn();
    }

    void Start()
    {
        

        animSpeed = 1f / eCont.attackTimer;
        hourAnim.SetFloat("speed", animSpeed);

        RandomRoom();
        

        int random = Random.Range(0, bossConversations.Length);
        for (int i = 0; i< bossConversations.Length; i++)
        {
            bossConversations[i].gameObject.SetActive(false);
        }
        bossConversations[random].gameObject.SetActive(true);

        progressBar.SetMaxProgress(bRoomNum);
        progressBar.SetProgress(currentRoom);
    }

    // Update is called once per frame
    void Update()
    {

        if (waveNum >= waveMax)
        {
            roomComplete = true;
        }

        if (roomComplete == true && eCont.enemies.Count == 0 && pickupSpawned == false)
        {
            StartCoroutine(SpawnPickups());
            pointer.gameObject.SetActive(true);
            pointer.isPickup = true;
        }

        if (currentRoom == bRoomNum)
        {
            bossRoom = true;
        }

        progressBar.SetProgress(currentRoom);
    }

    IEnumerator SpawnPickups()
    {
        pickupSpawned = true;
        RandomPickup();
        aMan.Play("Glimmer");
        GameObject pickup1 = Instantiate(pickup, eSpawn.transform.position + pickupDist, Quaternion.identity);
        pickup1.GetComponent<Pickup>().pickupNumber = pickupNumber;
        pickups.Add(pickup1);
        yield return new WaitForSeconds(0.02f);
        while (pickupNumber == pickup1.GetComponent<Pickup>().pickupNumber)
        {
            RandomPickup();
        }
        GameObject pickup2 = Instantiate(pickup, eSpawn.transform.position - pickupDist, Quaternion.identity);
        pickup2.GetComponent<Pickup>().pickupNumber = pickupNumber;
        pickups.Add(pickup2);
    }

    public void SaveGame()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("lastScene", currentScene);
        SaveSystem.SaveGame(eCont, player, this, aMan, dCont);
        PlayerPrefs.SetInt("saveExists", 1);
        PlayerPrefs.SetInt("deleteSave", 1);
        PlayerPrefs.Save();
    }

    public void LoadReset()
    {
        PlayerData data = SaveSystem.LoadGame();

        player.health = data.playerCurrentHealth;
        player.maxHealth = data.playerMaxHealth;
        player.shieldCount = data.playerShieldCount;
        player.speedBonus = data.playerSpeedBonus;
        player.force = data.playerForce;
        player.attackIncrease = data.playerAttackIncrease;
        player.rangeIncrease = data.playerRangeIncrease;
        player.attackCooldown = data.playerattackCooldown;
        player.knockbackIncrease = data.playerKnockbackIncrease;
        player.sndCooldown = data.playerSndCooldown;

      //  dCont.diffLevel = data.gameDifficultyLevel;
      //  dCont.diffScale = data.gameDifficultyScale;
        eCont.attackTimer = data.enemyAttackTimer;
        eCont.Force = data.enemyForce;
        eCont.Knockback = data.enemyKnockback;

       /* roomNumber = data.roomNumber;
        currentRoom = data.currentRoom;
        currentScene = data.currentScene;
        bRoomNum = data.bossRoomNum;
        bossRoom = data.bossRoom; */
    }

    public void LoadGame()
    {
        PlayerData data = SaveSystem.LoadGame();

        player.health = data.playerCurrentHealth;
        player.maxHealth = data.playerMaxHealth;
        player.shieldCount = data.playerShieldCount;
        player.speedBonus = data.playerSpeedBonus;
        player.force = data.playerForce;
        player.attackIncrease = data.playerAttackIncrease;
        player.rangeIncrease = data.playerRangeIncrease;
        player.attackCooldown = data.playerattackCooldown;
        player.knockbackIncrease = data.playerKnockbackIncrease;
        player.sndCooldown = data.playerSndCooldown;

        dCont.diffLevel = data.gameDifficultyLevel;
        dCont.diffScale = data.gameDifficultyScale;
        eCont.attackTimer = data.enemyAttackTimer;
        eCont.Force = data.enemyForce;
        eCont.Knockback = data.enemyKnockback;

        roomNumber = data.roomNumber;
        currentRoom = data.currentRoom;
        currentScene = data.currentScene;
        bRoomNum = data.bossRoomNum;
       // roomComplete = data.roomComplete;
        bossRoom = data.bossRoom;
       // pickupSpawned = data.pickupSpawned;

        aMan.bossStageOne = data.bossStageOne;
    }


    public void RandomRoom()
    {
        roomComplete = false;
        waveNum = 0;
        if (currentRoom == (bRoomNum))
        {
            roomNumber = 0;
            eCont.maxEnemies = 8;
        }
        else
        {
            switch (currentRoom)
            {
                case 0:
                    roomNumber = 1;
                    break;
                default:
                    roomNumber = Random.Range(2, rooms.Length);
                    break;
            }
        }
        CheckMemory();
        pSpawn = rooms[roomNumber].transform.Find("PlayerSpawn").gameObject;
        eSpawn = rooms[roomNumber].transform.Find("EnemySpawn").gameObject;
        door = rooms[roomNumber].transform.Find("Door").gameObject;
        memSpawn = rooms[roomNumber].transform.Find("MemorySpawn").gameObject;
        if (memSpawn)
        {
            Debug.Log("spawn point confirmed");
        }
        else
        {
            Debug.Log("No spawn point detected");
        }
        player.transform.position = pSpawn.transform.position;
    }

    public void SetUpNextRoom()
    {
        Collider2D collider = door.GetComponent<Collider2D>();
        collider.isTrigger = true;
    }

    void RandomPickup()
    {
        int randomiser = Random.Range(0, 60);
        if (randomiser >= 0 && randomiser < 5)
        {
            pickupNumber = 0;
        }
        if (randomiser >= 5 && randomiser < 10)
        {
            pickupNumber = 1;
        }
        if (randomiser >= 10 && randomiser < 15)
        {
            pickupNumber = 2;
        }
        if (randomiser >= 15 && randomiser < 20)
        {
            pickupNumber = 3;
        }
        if (randomiser >= 20 && randomiser < 25)
        {
            pickupNumber = 4;
        }
        if (randomiser >= 25 && randomiser < 30)
        {
            pickupNumber = 5;
        }
        if (randomiser >= 30 && randomiser < 35)
        {
            pickupNumber = 6;
        }
        if (randomiser >= 35 && randomiser < 40)
        {
            pickupNumber = 7;
        }
        if (randomiser >= 40 && randomiser < 45)
        {
            pickupNumber = 8;
        }
        if (randomiser >= 45 && randomiser < 50)
        {
            pickupNumber = 9;
        }
        if (randomiser >= 50 && randomiser < 55)
        {
            pickupNumber = 10;
        }
        if (randomiser >= 55 && randomiser < 60)
        {
            pickupNumber = 11;
        }
    }

    public IEnumerator MemoryAdded()
    {
        PickupText.text = "Memory regained";
        PickupText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        PickupText.gameObject.SetActive(false);
    }

    public IEnumerator HealthAdded()
    {
        PickupText.text = "Health regained";
        PickupText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        PickupText.gameObject.SetActive(false);
    }

    public IEnumerator PickupGained(int pNum)
    {
        switch (pNum)
        {
            case 0:
                PickupText.text = "Attack Range Increased";
                break;
            case 1:
                PickupText.text = "Attack Speed Increased";
                break;
            case 2:
                PickupText.text = "Attack Size Increased";
                break;
            case 3:
                PickupText.text = "Secondary Attack Cooldown Reduced";
                break;
            case 4:
                PickupText.text = "Enemy Projectile Speed Reduced";
                break;
            case 5:
                PickupText.text = "Enemy Attack Timer Increased";
                hourAnim.SetFloat("speed", animSpeed);
                break;
            case 6:
                PickupText.text = "Move Speed Increased";
                break;
            case 7:
                PickupText.text = "Armour Gained";
                break;
            case 8:
                PickupText.text = "Attack Knockback Increased";
                break;
            case 9:
                PickupText.text = "Secondary Attack Range Increased";
                break;
            case 10:
                PickupText.text = "Enemy Knockback Reduced";
                break;
            case 11:
                PickupText.text = "Max Health Increased";
                break;
        }
        PickupText.gameObject.SetActive(true);

        yield return new WaitForSeconds(2);

        PickupText.gameObject.SetActive(false);
    }

    public void NewRoom()
    {
       
        if (bossRoom == false)
        {
            currentRoom++;
            dCont.diffLevel++;
            dCont.CheckDiff();
            roomComplete = false;
            ResetSounds();
            RandomRoom();
            progressBar.SetProgress(currentRoom);
            pickupSpawned = false;
            thePillarScript[] allPillars;
            theTorchScript[] alltorches;
            allPillars = FindObjectsOfType<thePillarScript>();
            alltorches = FindObjectsOfType<theTorchScript>();
            for (int i = 0; i < allPillars.Length; i++)
            {
                allPillars[i].pillarState = allPillars[i].state.Length-1;
                allPillars[i].mylight.SetActive(true);
                allPillars[i].mycollider.enabled = true;
                allPillars[i].PillarDamage();
            }
            for (int i = 0; i < alltorches.Length; i++)
            {
                alltorches[i].LightsOn();
            } 
            if (spawnMemory == true)
            {
                Instantiate(memory, memSpawn.transform.position, Quaternion.identity);
            }
            SaveGame();
        }
        else if (bossRoom == true)
        {
            aMan.bossStageOne = false;
            SaveGame();
            PlayerPrefs.SetInt("deleteSave", 2);
            PlayerPrefs.Save();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

    }

    void ResetSounds()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        int buildIndex = currentScene.buildIndex;
        aMan.StopSound("Water");
        aMan.StopSound("Lava");
        if (buildIndex == 4)
        {
            aMan.Play("Water");
        }
        if (buildIndex == 6)
        {
            aMan.Play("Lava");
        }
    }

    void CheckMemory()
    {
        int randomizer = Random.Range(0, 100);
        if (randomizer <= 5)
        {
            spawnMemory = true;
        }
        else
        {
            spawnMemory = false;
        }
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

    void FadeIn()
    {
        sceneFader.color = Color.black;
        sceneFader.canvasRenderer.SetAlpha(1.0f);
        sceneFader.CrossFadeAlpha(0.0f, 1f, false);
    }
}
