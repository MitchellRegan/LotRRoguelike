using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
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



    //Function called from PlayerHealthManager.cs when this character's health increases
    public int GetHealthBoostAmount()
    {
        //The amount of health granted
        int healthBoost = 0;

        //Adding the base health boost amount
        healthBoost += this.baseHealthBoostOnIncrease;

        //Multiplier for the dice rolls to see if they're negative or positive
        int diePositiveNegative = 1;
        if (this.dieRollIsNegative)
        {
            diePositiveNegative = -1;
        }
        //Looping through for each health die to roll for random health
        for (int d = 0; d < this.numberOfHealthDiceToRoll; ++d)
        {
            healthBoost += diePositiveNegative * Random.Range(1, this.healthDiceSideNumber + 1);
        }

        return healthBoost;
    }
}
