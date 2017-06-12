using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class EnemyEncounter : MonoBehaviour
{
    //The list of all enemies that are spawned when combat begins
    public List<EncounterEnemy> enemies;
}

[System.Serializable]
public class EncounterEnemy
{
    //The enemy character
    public Character enemyCreature;

    //Enums that determine what section of the combat grid this enemy starts in
    public enum colPositionAreas { Front, Middle, Back, Random, SpecificPos };
    public enum rowPositionAreas { Top, Middle, Bottom, Random, SpecificPos };
    public colPositionAreas colArea = colPositionAreas.Random;
    public rowPositionAreas rowArea = rowPositionAreas.Random;

    //Ints for specific row/col positions. If negative, they're ignored and set to randomized
    public int specificCol = -1;
    public int specificRow = -1;
}
