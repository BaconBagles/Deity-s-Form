using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoguelikeTester : MonoBehaviour
{
    public int rNumber;
    public GameObject[] Rooms;

    // Start is called before the first frame update
    void Start()
    {
        rNumber = Random.Range(0, 10);
        RoomSwitch();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            rNumber = Random.Range(0, Rooms.Length);
            RoomSwitch();
        }
    }

    void RoomSwitch()
    {
        foreach (GameObject room in Rooms)
        {
            room.SetActive(false);
        }
        switch (rNumber)
        {
            case 9:
                Rooms[9].gameObject.SetActive(true);
                break;
            case 8:
                Rooms[8].gameObject.SetActive(true);
                break;
            case 7:
                Rooms[7].gameObject.SetActive(true);
                break;
            case 6:
                Rooms[6].gameObject.SetActive(true);
                break;
            case 5:
                Rooms[5].gameObject.SetActive(true);
                break;
            case 4:
                Rooms[4].gameObject.SetActive(true);
                break;
            case 3:
                Rooms[3].gameObject.SetActive(true);
                break;
            case 2:
                Rooms[2].gameObject.SetActive(true);
                break;
            case 1:
                Rooms[1].gameObject.SetActive(true);
                break;
            case 0:
                Rooms[0].gameObject.SetActive(true);
                break;
        }
    }
}
