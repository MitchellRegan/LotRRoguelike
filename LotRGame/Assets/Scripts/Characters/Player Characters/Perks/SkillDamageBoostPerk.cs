using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDamageBoostPerk : Perk
{
    //The skill that this perk is used for
    public SkillList skillToBoost = SkillList.Unarmed;

    //Bool for if this perk only works during a crit. If not, it works all the time
    public bool onlyWorksOnCrit = false;

    //Bool for if this perk only works for attack actions (not DoTs or HoTs)
    public bool onlyWorksOnAttackAct = false;

    //Bool for if this perk only works for DoTs and HoTs
    public bool onlyWorksOnDoTAndHoT = false;

    //Damage multiplier if the attack that we're boosting is a crit
    [Range(1, 10)]
    public float critMultiplier = 1;

    [Space(8)]

    //The base amount of added damage
    public int baseAddedDamage = 0;

    [Space(8)]

    //The amount of added damage based on the character's skill level
    [Range(-1f, 1)]
    public float percentOfSkillLevelDamage = 0;
    //The multiplier for the amount that the percentOfSkillLevelDamage gets
    public float percentDamageMultiplier = 1;

    [Space(8)]

    //The number of damage dice to roll
    public int numberOfDamageDiceToRoll = 0;
    //The number of sides on the damage dice
    public int damageDiceSideNumber = 6;



	//Function called from AttackAction.cs to get the amount of bonus damage this perk awards
    public int GetDamageBoostAmount(Character perkOwner_, bool isCrit_, bool isDoTOrHot_)
    {
        //The total amount of bonus damage returned
        int totalDamage = 0;

        //If this perk only activates during a crit and the attack didn't crit, nothing happens
        if(this.onlyWorksOnCrit && !isCrit_)
        {
            return totalDamage;
        }

        //If this perk only activates for Attack Actions and the current action isn't, nothing happens
        if(this.onlyWorksOnAttackAct && isDoTOrHot_)
        {
            return totalDamage;
        }

        //If this perk only activates for DoTs and HoTs and the current action isn't, nothing happens
        if(this.onlyWorksOnDoTAndHoT && !isDoTOrHot_)
        {
            return totalDamage;
        }

        //Adding the base damage amount
        totalDamage += this.baseAddedDamage;

        //Adding the bonus damage from the player's skill level
        totalDamage += Mathf.FloorToInt(this.percentOfSkillLevelDamage * 
                                        perkOwner_.charSkills.GetSkillLevelValueWithMod(this.skillToBoost) * 
                                        this.percentDamageMultiplier);

        //Looping through and adding bonus damage for each damage die
        for(int d = 0; d < this.numberOfDamageDiceToRoll; ++d)
        {
            totalDamage += Random.Range(1, this.damageDiceSideNumber + 1);
        }


        //If the attack was a crit, we multiply the total damage
        if(isCrit_)
        {
            totalDamage = Mathf.FloorToInt(totalDamage * this.critMultiplier);
        }

        //Returning the total damage
        return totalDamage;
    }
}
