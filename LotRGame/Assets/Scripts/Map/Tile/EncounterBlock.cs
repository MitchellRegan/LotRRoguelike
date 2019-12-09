using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class used in RegionInfo.cs. Represents a collection of enemies in a group that have a chance to be spawned together
[System.Serializable]
public class EncounterBlock
{
    //The skill roll that the player party has to reach to encounter
    [Range(10, 200)]
    public int skillCheck = 10;

    //The likelihood that this encounter block will be chosen
    [Range(0.01f, 1f)]
    public float encounterChance = 0.1f;

    //The list of enemies in this encounter
    public EnemyEncounter encounterEnemies;
}