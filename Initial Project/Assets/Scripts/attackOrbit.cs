using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackOrbit : MonoBehaviour
{
    public GameObject player;
    Vector3 v3Pos;
    float angle;
    float distance = .24f;
    public float xPos;
    public float yPos;

    // Update is called once per frame
    void Update()
    {
        //project the mouse point into the worldspace at the distance of the player
        v3Pos = Input.mousePosition;
        v3Pos.z = (player.transform.position.z - Camera.main.transform.position.z);
        v3Pos = Camera.main.ScreenToWorldPoint(v3Pos);
        v3Pos = v3Pos - player.transform.position;
        angle = Mathf.Atan2(v3Pos.y, v3Pos.x) * Mathf.Rad2Deg;
        if (angle < 0.0f) angle += 360.0f;
        transform.localEulerAngles = new Vector3(0, 0, angle);
        //as pos is transformed, maintain .24 radius;
        xPos = Mathf.Cos(Mathf.Deg2Rad * angle) + distance;
        yPos = Mathf.Sin(Mathf.Deg2Rad * angle) + distance - 1;
        transform.localPosition = new Vector3(player.transform.position.x + xPos * 4, player.transform.position.y + yPos * 4, 0);
    }
}
