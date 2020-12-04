using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public EnemyController eCont;
    public PlayerController player;
    public GameObject[] rooms;
    public GameObject[] pickups;
    int roomNumber;
    public int pickupNumber;
    public bool pickupSpawned;

    void Start()
    {
        roomNumber = Random.Range(0, rooms.Length);
        for (int i = 0; i< rooms.Length; i++)
        {
            rooms[i].SetActive(false);
        }
        rooms[roomNumber].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (eCont.enemies.Count == 0 && pickupSpawned == false && eCont.spawning == false)
        {
            pickupSpawned = true;
            eCont.spawning = true;
            RandomPickup();
            Instantiate(pickups[pickupNumber], new Vector2(0, 0), Quaternion.identity);
        }
    }

    void RandomPickup()
    {
        int randomiser = Random.Range(0, 100);
        if(randomiser >= 50 && randomiser < 60)
        {
            pickupNumber = 1;
        }
        else if (randomiser >= 60 && randomiser < 70)
        {
            pickupNumber = 2;
        }
        else if (randomiser >= 70 && randomiser < 80)
        {
            pickupNumber = 3;
        }
        else if (randomiser >= 80 && randomiser < 90)
        {
            pickupNumber = 4;
        }
        else if (randomiser >= 90 && randomiser < 100)
        {
            pickupNumber = 5;
        }
        else
        {
            pickupNumber = 0;
        }
        
    }
}
