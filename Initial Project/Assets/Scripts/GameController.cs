using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public EnemyController eCont;
    public PlayerController player;
    public GameObject[] rooms;
    public GameObject pickup;
    public GameObject MemoryPickupText;
    int roomNumber;

    public int pickupNumber;
    public bool pickupSpawned;
    public bool allMemories;

    public int currentRoom;
    public int waveMax;
    public int waveNum;
    public bool roomComplete;
    public GameObject door;
    new BoxCollider2D collider;

    int currentScene;

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
        RandomRoom();
        currentRoom = 1;
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
            pickupSpawned = true;
            RandomPickup();
            Instantiate(pickup, new Vector2(0, 0), Quaternion.identity);
        }
    }

    public void RandomRoom()
    {
        roomComplete = false;
        waveNum = 0;
        roomNumber = Random.Range(0, rooms.Length);
        for (int i = 0; i < rooms.Length; i++)
        {
            rooms[i].SetActive(false);
        }
        rooms[roomNumber].SetActive(true);
        door = rooms[roomNumber].transform.Find("Door").gameObject;
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
        int randomiser = Random.Range(0, 100);
        if(randomiser >= 50 && randomiser < 60)
        {
            pickupNumber = 1;
        }
        else if (randomiser >= 61 && randomiser < 70)
        {
            pickupNumber = 2;
        }
        else if (randomiser >= 71 && randomiser < 80)
        {
            pickupNumber = 3;
        }
        else if (randomiser >= 81 && randomiser < 90)
        {
            pickupNumber = 4;
        }
        else if (randomiser >= 91 && randomiser < 100 && !allMemories)
        {
            pickupNumber = 5;
        }
        else
        {
            pickupNumber = 0;
        }
       
    }

    public IEnumerator MemoryAdded()
    {
        MemoryPickupText.SetActive(true);

        yield return new WaitForSeconds(2);

        MemoryPickupText.SetActive(false);
    }
}
