using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBoostPerk : Perk
{
    //The number of stages that this perk owner's health is modified
    public int healthStageBoost = 0;

    [Space(8)]

    //The base amount of bonus health granted when this perk owner's health is increased
    public int baseHealthBoostOnIncrease = 0;

    [Space(8)]

    //The number of damage dice to roll
    public int numberOfHealthDiceToRoll = 0;
    //The number of sides on the damage dice
    public int healthDiceSideNumber = 6;
    //If this die roll is negative
    public bool dieRollIsNegative = false;
}
