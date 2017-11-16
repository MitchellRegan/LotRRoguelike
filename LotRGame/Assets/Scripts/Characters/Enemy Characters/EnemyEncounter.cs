using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
[System.Serializable]
public class EnemyEncounter : MonoBehaviour
{
    //The list of all enemies that are spawned when combat begins
    public List<EncounterEnemy> enemies;

    //Enum for the different ranges that this encounter group can be spawned
    public enum EnemyCombatPosition { MeleeFront, MeleeFlanking, MeleeBehind, RangedFront, RangedFlanking, RangedBehind, MiddleFront, MiddleFlanking, MiddleBehind };

    //The distance that this enemy encounter engages the player party at by default
    public EnemyCombatPosition defaultPosition = EnemyCombatPosition.MiddleFront;

    //The distance that this enemy encounter engages the player party when ambushing
    public EnemyCombatPosition ambushPosition = EnemyCombatPosition.MiddleFlanking;
}

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
