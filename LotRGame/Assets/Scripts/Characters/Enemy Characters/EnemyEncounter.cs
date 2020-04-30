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
