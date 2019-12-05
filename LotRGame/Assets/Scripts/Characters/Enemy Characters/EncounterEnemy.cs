using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class used in EnemyEncounter.cs to set an enemy character and their position in the encounter
[System.Serializable]
public class EncounterEnemy
{
    //The enemy character
    public Character enemyCreature;

    //Bools that determine if this enemy's position is randomized or not
    public bool randomCol = true;
    public bool randomRow = true;

    //Ints for specific row/col positions
    [Range(0, 3)]
    public int specificCol = 0;
    [Range(0, 7)]
    public int specificRow = 0;
}