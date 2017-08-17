using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantDamageEffect : Effect
{
    //The type of damage that's inflicted
    public CombatManager.DamageType type = CombatManager.DamageType.Physical;

    //The base amount of damage dealt
    public int baseDamage = 0;

    //The number of dice that are rolled
    public int diceRolled = 1;

    //The highest value of the type of die rolled
    public int diceSides = 6;

    //The crit chance for this damage
    [Range(0,1)]
    public float critChance = 0.1f;

    //The damage multiplier when this crits
    public int critMultiplier = 2;



    //Overrided function from Effect.cs to trigger this damage effect
    public override void TriggerEffect(Character usingCharacter_, Character targetCharacter_, float timeDelay_ = 0)
    {
        //Int to hold all of the damage for the attack
        int totalDamage = 0;

        //Adding the base damage
        totalDamage += this.baseDamage;

        //Looping through each individual die rolled
        for(int d = 0; d < this.diceRolled; ++d)
        {
            //Finding the value rolled on the current die
            totalDamage += Random.Range(1, this.diceSides);
        }

        //Rolling to see if this effect crits
        float critRoll = Random.Range(0, 1);
        bool isCrit = false;
        if(critRoll < this.critChance)
        {
            totalDamage = totalDamage * this.critMultiplier;
        }

        //Subtracting the target character's magic resistances
        switch(this.type)
        {
            case CombatManager.DamageType.Fire:
                if(targetCharacter_.charInventory.totalFireResist > 0)
                {
                    totalDamage -= targetCharacter_.charInventory.totalFireResist;
                }
                break;

            case CombatManager.DamageType.Water:
                if (targetCharacter_.charInventory.totalWaterResist > 0)
                {
                    totalDamage -= targetCharacter_.charInventory.totalWaterResist;
                }
                break;

            case CombatManager.DamageType.Electric:
                if (targetCharacter_.charInventory.totalElectricResist > 0)
                {
                    totalDamage -= targetCharacter_.charInventory.totalElectricResist;
                }
                break;

            case CombatManager.DamageType.Wind:
                if (targetCharacter_.charInventory.totalWindResist > 0)
                {
                    totalDamage -= targetCharacter_.charInventory.totalWindResist;
                }
                break;

            case CombatManager.DamageType.Rock:
                if (targetCharacter_.charInventory.totalRockResist > 0)
                {
                    totalDamage -= targetCharacter_.charInventory.totalRockResist;
                }
                break;

            case CombatManager.DamageType.Holy:
                if (targetCharacter_.charInventory.totalHolyResist > 0)
                {
                    totalDamage -= targetCharacter_.charInventory.totalFireResist;
                }
                break;

            case CombatManager.DamageType.Dark:
                if (targetCharacter_.charInventory.totalFireResist > 0)
                {
                    totalDamage -= targetCharacter_.charInventory.totalFireResist;
                }
                break;
        }

        //Finding the combat tile that the target character is on
        CombatTile targetCharTile = CombatManager.globalReference.FindCharactersTile(targetCharacter_);

        //Dealing damage to the target character and telling the combat manager to display how much was dealt
        targetCharacter_.charPhysState.DamageCharacter(totalDamage);
        CombatManager.globalReference.DisplayDamageDealt(timeDelay_, totalDamage, type, targetCharTile, isCrit);

        //Increasing the threat to the target based on damage dealt
        //If the attack is a crit, ALL enemies have their threat increased for 25% of the damage
        if(isCrit)
        {
            //Getting 25% of the damage to pass to all enemies
            int threatForAll = totalDamage / 4;
            CombatManager.globalReference.ApplyActionThreat(null, threatForAll, true);

            //Applying the rest of the threat to the target character
            CombatManager.globalReference.ApplyActionThreat(targetCharacter_, totalDamage - threatForAll, false);
        }
        //If the attack wasn't a crit, only the target character takes threat
        else
        {
            CombatManager.globalReference.ApplyActionThreat(targetCharacter_, totalDamage, false);
        }

        //Destroying this effect once everything is finished up
        Destroy(this.gameObject);
    }
}
