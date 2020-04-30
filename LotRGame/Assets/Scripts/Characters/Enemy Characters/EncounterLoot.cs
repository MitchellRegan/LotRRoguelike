using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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