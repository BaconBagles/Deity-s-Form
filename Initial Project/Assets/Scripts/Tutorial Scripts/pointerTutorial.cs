using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pointerTutorial : MonoBehaviour
{
    public GameObject player;
    public tutorialScript gCont;
    public GameObject door;
    public Vector3 pointTo;
    Vector3 pointFrom;

    public bool isDoor;


    // Update is called once per frame
    void Update()
    {
        door = gCont.door[gCont.currentRoom];

        if (isDoor == true)
        {
            pointTo = door.transform.position;
        }
        transform.up = (pointTo - player.transform.position).normalized;
    }
}
