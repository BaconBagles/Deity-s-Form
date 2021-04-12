using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    // List all variables here, needs to be int, float, bool or string
    //pCont
    public int playerCurrentHealth;
    public int playerMaxHealth;
    public int playerShieldCount;
    public float playerSpeedBonus;
    public float playerForce;
    public float playerAttackIncrease;
    public float playerRangeIncrease;
    public float playerattackCooldown;
    public float playerKnockbackIncrease;
    public float playerSndCooldown;

    //eCont
    public int gameDifficultyLevel;
    public int enemyAttackTimer;
    public float enemyForce;
    public float enemyKnockback;

    //gCont
    public int roomNumber;
    public int currentRoom;
    public int currentScene;
    public int bossRoomNum;
    public bool roomComplete;
    public bool bossRoom;
    public bool pickupSpawned;

    //aMan
    public bool bossStageOne;

    public PlayerData ( EnemyController eCont, PlayerController pCont, GameController gCont, AudioManager aMan)
    {
        //variable = script.variable;
        playerCurrentHealth = pCont.health;
        playerMaxHealth = pCont.maxHealth;
        playerShieldCount = pCont.shieldCount;
        playerSpeedBonus = pCont.speedBonus;
        playerForce = pCont.force;
        playerAttackIncrease = pCont.attackIncrease;
        playerRangeIncrease = pCont.rangeIncrease;
        playerattackCooldown = pCont.attackCooldown;
        playerKnockbackIncrease = pCont.knockbackIncrease;
        playerSndCooldown = pCont.sndCooldown;

        gameDifficultyLevel = eCont.diffLevel;
        enemyAttackTimer = eCont.attackTimer;
        enemyForce = eCont.Force;
        enemyKnockback = eCont.Knockback;

        roomNumber = gCont.roomNumber;
        currentRoom = gCont.currentRoom;
        currentScene = gCont.currentScene;
        bossRoomNum = gCont.bRoomNum;
        //roomComplete = gCont.roomComplete;
        bossRoom = gCont.bossRoom;
        //pickupSpawned = gCont.pickupSpawned;

        bossStageOne = aMan.bossStageOne;
    }
}
