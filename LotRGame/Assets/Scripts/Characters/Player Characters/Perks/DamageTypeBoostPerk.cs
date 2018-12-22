using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DamageTypeBoostPerk : Perk
{
    //The damage type that this perk boosts
    public CombatManager.DamageType damageTypeToBoost = CombatManager.DamageType.Slashing;

    //Bool for if this perk only works during a crit. If not it works all the time
    public bool onlyWorksOnCrit = false;

    //Bool for if this perk only works for attack actions (not DoTs or HoTs)
    public bool onlyWorksOnAttackAct = false;

    //Bool for if this perk only works for DoTs and HoTs
    public bool onlyWorksOnDoTAndHoT = false;

    //Damage multiplier if the attack that we're boosting crits
    [Range(1, 10)]
    public float critMultiplier = 1;

    [Space(8)]

    //The base amount of added damage
    public int baseAddedDamage = 0;

    [Space(8)]

    //The number of damage dice to roll
    public int numberOfDamageDiceToRoll = 0;
    //The number of sides on the damage dice
    public int damageDiceSideNumber = 6;
    //If this die roll is negative
    public bool dieRollIsNegative = false;



    //Function called from AttackAction.cs to get the amount of bonus damage this perk awards
    public int GetDamageBoostAmount(Character perkOwner_, bool isCrit_, bool isDoTOrHot_, CombatManager.DamageType type_)
    {
        //The total amount of bonus damage returned
        int totalDamage = 0;

        //If the damage type doesn't match the type we're boosting, nothing happens
        if (this.damageTypeToBoost != type_)
        {
            return totalDamage;
        }

        //If this perk only activates during a crit and the attack didn't crit, nothing happens
        if (this.onlyWorksOnCrit && !isCrit_)
        {
            return totalDamage;
        }

        //If this perk only activates for Attack Actions and the current action isn't, nothing happens
        if (this.onlyWorksOnAttackAct && isDoTOrHot_)
        {
            return totalDamage;
        }

        //If this perk only activates for DoTs and HoTs and the current action isn't, nothing happens
        if (this.onlyWorksOnDoTAndHoT && !isDoTOrHot_)
        {
            return totalDamage;
        }

        //Adding the base damage amount
        totalDamage += this.baseAddedDamage;

        //Multiplier for the dice rolls to see if they're negative or positive
        int diePositiveNegative = 1;
        if (this.dieRollIsNegative)
        {
            diePositiveNegative = -1;
        }

        //Looping through and adding bonus damage for each damage die
        for (int d = 0; d < this.numberOfDamageDiceToRoll; ++d)
        {
            totalDamage += diePositiveNegative * Random.Range(1, this.damageDiceSideNumber + 1);
        }


        //If the attack was a crit, we multiply the total damage
        if (isCrit_)
        {
            totalDamage = Mathf.FloorToInt(totalDamage * this.critMultiplier);
        }

        //Returning the total damage
        return totalDamage;
    }
}
