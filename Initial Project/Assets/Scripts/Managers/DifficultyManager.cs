using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public GameController gCont;
    public EnemyController eCont;
    public int diffLevel;
    public int diffScale;

    // Start is called before the first frame update
    void Start()
    {
        gCont = FindObjectOfType<GameController>();
        eCont = FindObjectOfType<EnemyController>();
    }

    public void CheckDiff()
    {
        if (diffLevel <= 8)
        {
            diffScale = 1;
            gCont.waveMax = 2;
            eCont.maxEnemies = 5;
            eCont.diffHealthMod = 0;
            eCont.diffDamageMod = 0;
        }
        else if (diffLevel > 8 && diffLevel <= 16)
        {
            diffScale = 2;
            gCont.waveMax = 3;
            eCont.maxEnemies = 5;
            eCont.diffHealthMod = 1;
            eCont.diffDamageMod = 0;
        }
        else if (diffLevel > 16 && diffLevel <=24)
        {
            diffScale = 3;
            gCont.waveMax = 3;
            eCont.maxEnemies = 6;
            eCont.diffHealthMod = 1;
            eCont.diffDamageMod = 1;
        }
        else if (diffLevel > 24 && diffLevel <= 32)
        {
            diffScale = 4;
            gCont.waveMax = 3;
            eCont.maxEnemies = 6;
            eCont.diffHealthMod = 2;
            eCont.diffDamageMod = 1;
        }
        else if (diffLevel > 32 && diffLevel <= 40)
        {
            diffScale = 5;
            gCont.waveMax = 4;
            eCont.maxEnemies = 6;
            eCont.diffHealthMod = 2;
            eCont.diffDamageMod = 1;
        }
        else if (diffLevel > 40 && diffLevel <= 48)
        {
            diffScale = 6;
            gCont.waveMax = 4;
            eCont.maxEnemies = 7;
            eCont.diffHealthMod = 2;
            eCont.diffDamageMod = 2;
        }
        else if (diffLevel > 48 && diffLevel <= 56)
        {
            diffScale = 7;
            gCont.waveMax = 4;
            eCont.maxEnemies = 7;
            eCont.diffHealthMod = 3;
            eCont.diffDamageMod = 2;
        }
        else if (diffLevel > 56 && diffLevel <= 64)
        {
            diffScale = 8;
            gCont.waveMax = 4;
            eCont.maxEnemies = 7;
            eCont.diffHealthMod = 3;
            eCont.diffDamageMod = 3;
        }
        else if (diffLevel > 64 && diffLevel <= 72)
        {
            diffScale = 9;
            gCont.waveMax = 4;
            eCont.maxEnemies = 8;
            eCont.diffHealthMod = 3;
            eCont.diffDamageMod = 3;
        }
        else if (diffLevel > 72 && diffLevel <= 80)
        {
            diffScale = 10;
            gCont.waveMax = 5;
            eCont.maxEnemies = 8;
            eCont.diffHealthMod = 4;
            eCont.diffDamageMod = 3;
        }
        else if (diffLevel > 80)
        {
            diffScale = 11;
            gCont.waveMax = 5;
            eCont.maxEnemies = 8;
            eCont.diffHealthMod = 5;
            eCont.diffDamageMod = 4;
        }
    }
}
