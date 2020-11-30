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
    int pickupNumber;
    bool pickupSpawned;

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
            pickupNumber = Random.Range(0, pickups.Length);
            Instantiate(pickups[pickupNumber], new Vector2(0, 0), Quaternion.identity);
        }
    }
}
