using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*The characters that the player is able to control in and out of combat*/

[RequireComponent(typeof(Level))]
[RequireComponent(typeof(Equipment))]
[RequireComponent(typeof(AbilityList))]
[RequireComponent(typeof(PerkList))]
[RequireComponent(typeof(CombatStats))]
//[RequireComponent(typeof(Health))] Already applied from CombatObject
public class PlayerCharacter : CombatObject
{
    //This character's gender and race
    public Races race = Races.Human;
    public Genders gender = Genders.Male;

    //The amount of inventory space this character can hold
    public int inventorySize = 3;
}
