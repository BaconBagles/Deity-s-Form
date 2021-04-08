using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    public GameObject player;
    public GameController gCont;
    public GameObject door;
    public GameObject pickup;
    public Vector3 pointTo;
    Vector3 pointFrom;

    public bool isPickup;
    public bool isDoor;
    

    // Update is called once per frame
    void Update()
    {
        pickup = gCont.eSpawn;
        door = gCont.door;

        if (isPickup == true)
        {
            pointTo = pickup.transform.position;
        }
        if (isDoor == true)
        {
            pointTo = door.transform.position;
        }
        transform.up = (pointTo - player.transform.position).normalized;
    }

}
