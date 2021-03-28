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
    public GameObject[] rooms;
    public GameObject pickup;
    public TMPro.TextMeshProUGUI PickupText;
    public int roomNumber;

    public int pickupNumber;
    public bool pickupSpawned;
    public bool allMemories;

    public int currentRoom;
    public int waveMax;
    public int waveNum;
    public bool roomComplete;
    public bool bossRoom;
    public int bRoomNum;
    public GameObject door;
    public GameObject pSpawn;
    public GameObject eSpawn;
    new BoxCollider2D collider;

    int currentScene;

    public Image sceneFader;

    void Start()
    {
        if (PlayerPrefs.GetInt("Memory3", 0) == 1)
        {
            allMemories = true;
        }
        else
        {
            allMemories = false;
        }
        currentRoom = 1;
        RandomRoom();
        FadeIn();
    }

    // Update is called once per frame
    void Update()
    {
        waveMax = eCont.diffLevel / 2;

        if (waveNum >= waveMax)
        {
            roomComplete = true;
        }

        if (roomComplete == true && eCont.enemies.Count == 0 && pickupSpawned == false)
        {
            pickupSpawned = true;
            RandomPickup();
            Instantiate(pickup, eSpawn.transform.position, Quaternion.identity);
        }

        if (currentRoom == bRoomNum)
        {
            bossRoom = true;
        }
    }

    public void RandomRoom()
    {
        roomComplete = false;
        waveNum = 0;
        if (currentRoom == (bRoomNum-1))
        {
            roomNumber = 0;
        }
        else
        {
            switch (currentRoom)
            {
                case 1:
                    roomNumber = 1;
                    break;
                default:
                    roomNumber = Random.Range(2, rooms.Length - 1);
                    break;
            }
        }
        pSpawn = rooms[roomNumber].transform.Find("PlayerSpawn").gameObject;
        eSpawn = rooms[roomNumber].transform.Find("EnemySpawn").gameObject;
        door = rooms[roomNumber].transform.Find("Door").gameObject;
        player.transform.position = pSpawn.transform.position;
    }

    public void SetUpNextRoom()
    {
        BoxCollider2D collider = door.GetComponent<BoxCollider2D>();
        MeshRenderer renderer = door.GetComponent<MeshRenderer>();
        collider.isTrigger = true;
        renderer.enabled = true;
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

    public IEnumerator PickupGained()
    {
        switch (pickupNumber)
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
            if (currentRoom % 2 == 0)
            {
                eCont.diffLevel++;
            }
            roomComplete = false;
            RandomRoom();
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
        }

        if (bossRoom == true)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
