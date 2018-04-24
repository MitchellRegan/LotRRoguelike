using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ThreatBoostPerk : MonoBehaviour
{
    //Damage type to boost threat on
    public CombatManager.DamageType damageTypeToThreaten = CombatManager.DamageType.Slashing;

    //Bool for if this perk only works during a crit. If not, it works all the time
    public bool onlyWorksOnCrit = false;

    //Bool for if this perk works for ALL damage types
    public bool threatenAllDamageTypes = false;

    //Bool for if this perk applies at the start of combat
    public bool increaseThreatAtStartOfCombat = false;

    //Bool for if this perk only applies to instant attacks
    public bool onlyWorksOnAttackAct = false;

    //Bool for if this perk only applies to DoTs and HoTs
    public bool onlyWorksOnDoTAndHoT = false;

    [Space(8)]

    //The base amount of threat to add
    public int baseThreatToAdd = 0;

    [Space(8)]

    //The multiplier for this perk owner's threat
    [Range(-1, 3)]
    public float threatMultiplier = 1f;

    [Space(8)]

    //The number of damage dice to roll
    public int numberOfDamageDiceToRoll = 0;
    //The number of sides on the damage dice
    public int damageDiceSideNumber = 6;
    //If this die roll is negative
    public bool dieRollIsNegative = false;



    //Function called externally to return the amount of threat for a given action
    public int GetAddedActionThreat(int defaultThreat_, bool isCrit_, bool isDoTOrHoT_)
    {
        //The total amount of threat returned
        int totalThreat = 0;

        //If this perk only activates during a crit and the attack didn't crit, nothing happens
        if(this.onlyWorksOnCrit && !isCrit_)
        {
            return totalThreat;
        }

        //If this perk only activates for Attack actions and the current action is a DoT or HoT, nothing happens
        if(this.onlyWorksOnAttackAct && isDoTOrHoT_)
        {
            return totalThreat;
        }

        //If this perk only activates for DoTs and HoTs and the current action is an attack action, nothing happens
        if(this.onlyWorksOnDoTAndHoT && !isDoTOrHoT_)
        {
            return totalThreat;
        }

        //Adding the base amount of threat
        totalThreat += this.baseThreatToAdd;

        //Adding the amount of threat equal to the default threat times the threat multiplier
        totalThreat += Mathf.FloorToInt(defaultThreat_ * this.threatMultiplier);

        //Multiplier for the dice rolls to see if they're negative or positive
        int diePositiveNegative = 1;
        if (this.dieRollIsNegative)
        {
            diePositiveNegative = -1;
        }

        //Looping through and adding bonus damage for each damage die
        for (int d = 0; d < this.numberOfDamageDiceToRoll; ++d)
        {
            totalThreat += diePositiveNegative * Random.Range(1, this.damageDiceSideNumber + 1);
        }

        //Returning the total threat
        return totalThreat;
    }
}
