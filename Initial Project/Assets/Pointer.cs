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
    //Vector3 v3Pos;
    float angle;
    float distance = .24f;
    public float xPos;
    public float yPos;

    public bool isPickup;
    public bool isDoor;

    // Update is called once per frame
    void Update()
    {
        //project the mouse point into the worldspace at the distance of the player
        //v3Pos = Input.mousePosition;
        //v3Pos.z = (player.transform.position.z - Camera.main.transform.position.z);
        //v3Pos = Camera.main.ScreenToWorldPoint(v3Pos);
        //v3Pos = v3Pos - player.transform.position;

        pickup = gCont.pickup;
        door = gCont.door;

        if (isPickup == true)
        {
            pointTo = pickup.transform.position;
        }
        if (isDoor == true)
        {
            pointTo = door.transform.position;
        }
        //pointTo = pointTo - player.transform.position;
        angle = Mathf.Atan2(pointTo.y, pointTo.x) * Mathf.Rad2Deg;
        if (angle < 0.0f) angle += 360.0f;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //transform.localEulerAngles = new Vector3(0, 0, angle);
        //as pos is transformed, maintain .24 radius;
        xPos = Mathf.Cos(Mathf.Deg2Rad * angle) + distance;
        yPos = Mathf.Sin(Mathf.Deg2Rad * angle) + distance - 1;
        transform.localPosition = new Vector3(player.transform.position.x + xPos * 4, player.transform.position.y + yPos * 4, 0);
    }
}
