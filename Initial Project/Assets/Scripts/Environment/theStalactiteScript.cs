using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class theStalactiteScript : MonoBehaviour
{
    public thePillarScript pillar;
    public GameObject stalactite;
    public GameObject[] list;
    bool triggered;
    Vector2 pillarposition;
    Vector2 spawnposition;

    void Start()
    {
        pillar = GetComponent<thePillarScript>();
        pillarposition = transform.position;

        list = new GameObject[Random.Range(1, 6)];
        triggered = false;
    }

    void Update()
    {
        if (pillar.pillarState == 0 && triggered == false)
        {
            for(int i = 0; i < list.Length; i++)
            {
                spawnposition = pillarposition + Random.insideUnitCircle * 5;
                GameObject clone = (GameObject)Instantiate(stalactite, spawnposition, Quaternion.identity);

                list[i] = clone;
            }
            triggered = true;
        }

        if( pillar.pillarState == 3)
        {
            triggered = false;
        }
    }
}
