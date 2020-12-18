using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*The enemies that the player characters fight in combat*/

[RequireComponent(typeof(Equipment))]
[RequireComponent(typeof(AbilityList))]
[RequireComponent(typeof(PerkList))]
[RequireComponent(typeof(CombatStats))]
//[RequireComponent(typeof(Health))] Already applied from CombatObject
public class EnemyCharacter : CombatObject
{
    //This enemy's gender and race (for the purpose of race/gender-specific effects)
    public Races race = Races.Human;
    public Genders gender = Genders.Male;
}
