using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InstantDamageEffect : Effect
{
    //The type of damage that's inflicted
    public CombatManager.DamageType type = CombatManager.DamageType.Slashing;

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

        //Looping through the perks of the character that used this ability to see if they have any damage type boost perks
        foreach(Perk charPerk in usingCharacter_.charPerks.allPerks)
        {
            //If the perk boosts a damage type that's the same as this damage type, we boost it
            if(charPerk.GetType() == typeof(DamageTypeBoostPerk) && this.type == charPerk.GetComponent<DamageTypeBoostPerk>().damageTypeToBoost)
            {
                totalDamage += charPerk.GetComponent<DamageTypeBoostPerk>().GetDamageBoostAmount(usingCharacter_, isCrit, false);
            }
        }

        //Subtracting the target character's armor resist and magic resistances
        switch(this.type)
        {
            case CombatManager.DamageType.Slashing:
                if (targetCharacter_.charInventory.totalSlashingArmor > 0)
                {
                    totalDamage -= targetCharacter_.charInventory.totalSlashingArmor;
                }
                break;

            case CombatManager.DamageType.Stabbing:
                if (targetCharacter_.charInventory.totalStabbingArmor > 0)
                {
                    totalDamage -= targetCharacter_.charInventory.totalStabbingArmor;
                }
                break;

            case CombatManager.DamageType.Crushing:
                if (targetCharacter_.charInventory.totalCrushingArmor > 0)
                {
                    totalDamage -= targetCharacter_.charInventory.totalCrushingArmor;
                }
                break;

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

            case CombatManager.DamageType.Nature:
                if (targetCharacter_.charInventory.totalNatureResist > 0)
                {
                    totalDamage -= targetCharacter_.charInventory.totalNatureResist;
                }
                break;

            case CombatManager.DamageType.Arcane:
                if (targetCharacter_.charInventory.totalArcaneResist > 0)
                {
                    totalDamage -= targetCharacter_.charInventory.totalArcaneResist;
                }
                break;

            case CombatManager.DamageType.Holy:
                if (targetCharacter_.charInventory.totalHolyResist > 0)
                {
                    totalDamage -= targetCharacter_.charInventory.totalHolyResist;
                }
                break;

            case CombatManager.DamageType.Dark:
                if (targetCharacter_.charInventory.totalDarkResist > 0)
                {
                    totalDamage -= targetCharacter_.charInventory.totalDarkResist;
                }
                break;
        }

        //Looping through the attacking character's perks to see if there's any bonus threat to add to this effect
        int bonusThreat = 0;
        foreach (Perk charPerk in this.characterWhoTriggered.charPerks.allPerks)
        {
            //If the perk is a threat boosting perk
            if (charPerk.GetType() == typeof(ThreatBoostPerk))
            {
                ThreatBoostPerk threatPerk = charPerk.GetComponent<ThreatBoostPerk>();

                //If the perk has the same damage type as this effect or it affects all damage types
                if (threatPerk.damageTypeToThreaten == this.type || threatPerk.threatenAllDamageTypes)
                {
                    bonusThreat += threatPerk.GetAddedActionThreat(totalDamage, isCrit, false);
                }
            }
        }

        //Finding the combat tile that the target character is on
        CombatTile targetCharTile = CombatManager.globalReference.FindCharactersTile(targetCharacter_);

        //Dealing damage to the target character and telling the combat manager to display how much was dealt
        targetCharacter_.charPhysState.DamageCharacter(totalDamage);
        CombatManager.globalReference.DisplayDamageDealt(timeDelay_, totalDamage, type, targetCharTile, isCrit);

        //Creating the visual effect for this effect
        CharacterSpriteBase targetCharSprite = CombatManager.globalReference.GetCharacterSprite(targetCharacter_);
        this.SpawnVisualAtLocation(targetCharSprite.transform.localPosition, targetCharSprite.transform);


        //Increasing the threat to the target based on damage dealt
        //If the attack is a crit, ALL enemies have their threat increased for 25% of the damage
        if (isCrit)
        {
            //Getting 25% of the damage to pass to all enemies
            int threatForAll = (totalDamage + bonusThreat) / 4;
            CombatManager.globalReference.ApplyActionThreat(usingCharacter_, null, threatForAll, true);

            //Applying the rest of the threat to the target character
            CombatManager.globalReference.ApplyActionThreat(usingCharacter_, targetCharacter_, (totalDamage + bonusThreat) - threatForAll, false);
        }
        //If the attack wasn't a crit, only the target character takes threat
        else
        {
            CombatManager.globalReference.ApplyActionThreat(usingCharacter_, targetCharacter_, totalDamage + bonusThreat, false);
        }

        //Destroying this effect once everything is finished up
        Destroy(this.gameObject);
    }
}
