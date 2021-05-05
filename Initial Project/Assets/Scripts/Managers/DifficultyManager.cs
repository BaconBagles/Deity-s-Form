using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public GameController gCont;
    public EnemyController eCont;
    public int diffLevel;
    public int diffScale;
    public int enemyNum;

    // Start is called before the first frame update
    void Awake()
    {
        gCont = FindObjectOfType<GameController>();
        eCont = FindObjectOfType<EnemyController>();
        CheckDiff();
    }

    public void CheckDiff()
    {
        enemyNum = gCont.currentRoom;
        switch (enemyNum)
        {
            case 1:
                eCont.maxEnemies = 4;
                break;
            case 2:
                eCont.maxEnemies = 4;
                break;
            case 3:
                eCont.maxEnemies = 5;
                break;
            case 4:
                eCont.maxEnemies = 5;
                break;
            case 5:
                eCont.maxEnemies = 6;
                break;
            case 6:
                eCont.maxEnemies = 6;
                break;
            case 7:
                eCont.maxEnemies = 7;
                break;
            case 8:
                eCont.maxEnemies = 7;
                break;
            default:
                eCont.maxEnemies = 0;
                break;
        }

        switch (diffLevel)
        {
            case 1:
            diffScale = 1;
            gCont.waveMax = 2;
            eCont.diffHealthMod = 0;
            eCont.diffDamageMod = 0;
            enemyNum = 1;
                break;
            case 8:
            diffScale = 2;
            gCont.waveMax = 3;
            eCont.diffHealthMod = 1;
            eCont.diffDamageMod = 0;
            enemyNum = 1;
                break;
            case 16:
            diffScale = 3;
            gCont.waveMax = 3;
            eCont.diffHealthMod = 1;
            eCont.diffDamageMod = 1;
            enemyNum = 1;
                break;
            case 24:
            diffScale = 4;
            gCont.waveMax = 3;
            eCont.diffHealthMod = 2;
            eCont.diffDamageMod = 1;
            enemyNum = 1;
                break;
            case 32:
            diffScale = 5;
            gCont.waveMax = 4;
            eCont.diffHealthMod = 2;
            eCont.diffDamageMod = 1;
            enemyNum = 1;
                break;
            case 40:
            diffScale = 6;
            gCont.waveMax = 4;
            eCont.diffHealthMod = 2;
            eCont.diffDamageMod = 2;
            enemyNum = 1;
                break;
            case 48:
            diffScale = 7;
            gCont.waveMax = 4;
            eCont.diffHealthMod = 3;
            eCont.diffDamageMod = 2;
            enemyNum = 1;
                break;
            case 56:
            diffScale = 8;
            gCont.waveMax = 4;
            eCont.diffHealthMod = 3;
            eCont.diffDamageMod = 3;
            enemyNum = 1;
                break;
            case 64:
            diffScale = 9;
            gCont.waveMax = 4;
            eCont.diffHealthMod = 3;
            eCont.diffDamageMod = 3;
            enemyNum = 1;
                break;
            case 72:
            diffScale = 10;
            gCont.waveMax = 5;
            eCont.diffHealthMod = 4;
            eCont.diffDamageMod = 3;
            enemyNum = 1;
                break;
            case 80:
            diffScale = 11;
            gCont.waveMax = 5;
            eCont.diffHealthMod = 5;
            eCont.diffDamageMod = 4;
            enemyNum = 1;
                break;
        }

        
    }
}
