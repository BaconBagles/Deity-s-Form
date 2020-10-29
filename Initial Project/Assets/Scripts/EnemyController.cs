using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public int attackTimer;
    public PlayerController player;
    public GameObject[] enemies;

    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (IsArrayEmpty())
        {
        }
    }

    IEnumerator EnemyAttack()
    {
        yield return new WaitForSeconds(attackTimer);
        foreach (GameObject enemy in enemies)
        {
            player.health -= 1;
        }
        StartCoroutine(EnemyAttack());
    }

    private bool IsArrayEmpty()
    {
        if (enemies == null || enemies.Length == 0)
        {
            return true;
        }
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                return false;
            }
        }
    }
}
