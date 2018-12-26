using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
[System.Serializable]
public class EnemyEncounter : MonoBehaviour
{
    //Reference to the prefab that this object was instantiated from. Set from CharacterManager.CreateEnemyEncounter
    [HideInInspector]
    public GameObject encounterPrefab;

    //The list of all enemies that are spawned when combat begins
    public List<EncounterEnemy> enemies;

    //Enum for the different ranges that this encounter group can be spawned
    public enum EnemyCombatPosition { MeleeFront, MeleeFlanking, MeleeBehind, RangedFront, RangedFlanking, RangedBehind, MiddleFront, MiddleFlanking, MiddleBehind };

    //The distance that this enemy encounter engages the player party at by default
    public EnemyCombatPosition defaultPosition = EnemyCombatPosition.MiddleFront;

    //The distance that this enemy encounter engages the player party when ambushing
    public EnemyCombatPosition ambushPosition = EnemyCombatPosition.MiddleFlanking;

    [Space(8)]

    //The odds that the enemies in this encounter will ambush the player party
    [Range(0, 1)]
    public float ambushChance = 0.05f;

    [Space(8)]

    //The loot table for this encounter
    public List<EncounterLoot> lootTable;
}

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
    [Range(0,3)]
    public int specificCol = 0;
    [Range(0,7)]
    public int specificRow = 0;
}

//Class used in EnemyEncounter.cs and CombatManager.cs to let the player get item drops from enemy encounters
[System.Serializable]
public class EncounterLoot
{
    //The item object that is dropped
    public Item lootItem;
    //The range of items in this stack
    public Vector2 stackSizeMinMax = new Vector2(1, 1);
    //The likelihood that this item is dropped
    [Range(0.01f, 1)]
    public float dropChance = 0.2f;
}