﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InstantHealEffect : Effect
{
    //The type of heal that's inflicted
    public CombatManager.DamageType type = CombatManager.DamageType.Holy;

    //The base amount of heal dealt
    public int baseHeal = 0;

    //The number of dice that are rolled
    public int diceRolled = 1;

    //The highest value of the type of die rolled
    public int diceSides = 6;

    //The crit chance for this damage
    [Range(0, 1)]
    public float critChance = 0.1f;

    //The heal multiplier when this crits
    public float critMultiplier = 2;



    //Overrided function from Effect.cs to trigger this heal effect
    public override void TriggerEffect(Character usingCharacter_, Character targetCharacter_, float timeDelay_ = 0)
    {
        //Int to hold the heal total for the effect
        int totalHeal = 0;

        //Adding the base heal
        totalHeal += this.baseHeal;

        //Looping through each individual die rolled
        for (int d = 0; d < this.diceRolled; ++d)
        {
            //Finding the value rolled on the current die
            totalHeal += Random.Range(1, this.diceSides);
        }

        //Rolling to see if this effect crits
        float critRoll = Random.Range(0, 1);
        bool isCrit = false;
        if (critRoll < this.critChance)
        {
            totalHeal = Mathf.RoundToInt(totalHeal * this.critMultiplier);
        }

        //Looping through the perks of the character that used this ability to see if they have any damage type boost perks
        foreach (Perk charPerk in usingCharacter_.charPerks.allPerks)
        {
            //If the perk boosts a damage type that's the same as this damage (heal) type, we boost it
            if (charPerk.GetType() == typeof(DamageTypeBoostPerk) && this.type == charPerk.GetComponent<DamageTypeBoostPerk>().damageTypeToBoost)
            {
                totalHeal += charPerk.GetComponent<DamageTypeBoostPerk>().GetDamageBoostAmount(usingCharacter_, isCrit, false);
            }
        }

        //Subtracting the target character's magic resistances
        switch (this.type)
        {
            case CombatManager.DamageType.Slashing:
                if (targetCharacter_.charInventory.totalSlashingArmor > 0)
                {
                    totalHeal -= targetCharacter_.charInventory.totalSlashingArmor;
                }
                break;

            case CombatManager.DamageType.Stabbing:
                if (targetCharacter_.charInventory.totalStabbingArmor > 0)
                {
                    totalHeal -= targetCharacter_.charInventory.totalStabbingArmor;
                }
                break;

            case CombatManager.DamageType.Crushing:
                if (targetCharacter_.charInventory.totalCrushingArmor > 0)
                {
                    totalHeal -= targetCharacter_.charInventory.totalCrushingArmor;
                }
                break;

            case CombatManager.DamageType.Fire:
                if (targetCharacter_.charInventory.totalFireResist > 0)
                {
                    totalHeal -= targetCharacter_.charInventory.totalFireResist;
                }
                break;

            case CombatManager.DamageType.Water:
                if (targetCharacter_.charInventory.totalWaterResist > 0)
                {
                    totalHeal -= targetCharacter_.charInventory.totalWaterResist;
                }
                break;

            case CombatManager.DamageType.Electric:
                if (targetCharacter_.charInventory.totalElectricResist > 0)
                {
                    totalHeal -= targetCharacter_.charInventory.totalElectricResist;
                }
                break;

            case CombatManager.DamageType.Wind:
                if (targetCharacter_.charInventory.totalWindResist > 0)
                {
                    totalHeal -= targetCharacter_.charInventory.totalWindResist;
                }
                break;

            case CombatManager.DamageType.Nature:
                if (targetCharacter_.charInventory.totalNatureResist > 0)
                {
                    totalHeal -= targetCharacter_.charInventory.totalNatureResist;
                }
                break;

            case CombatManager.DamageType.Arcane:
                if (targetCharacter_.charInventory.totalArcaneResist > 0)
                {
                    totalHeal -= targetCharacter_.charInventory.totalArcaneResist;
                }
                break;

            case CombatManager.DamageType.Holy:
                if (targetCharacter_.charInventory.totalHolyResist > 0)
                {
                    totalHeal -= targetCharacter_.charInventory.totalHolyResist;
                }
                break;

            case CombatManager.DamageType.Dark:
                if (targetCharacter_.charInventory.totalDarkResist > 0)
                {
                    totalHeal -= targetCharacter_.charInventory.totalDarkResist;
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
                    bonusThreat += threatPerk.GetAddedActionThreat(totalHeal, isCrit, false);
                }
            }
        }

        //Finding the combat tile that the target character is on
        CombatTile targetCharTile = CombatManager.globalReference.FindCharactersTile(targetCharacter_);

        //Dealing damage to the target character and telling the combat manager to display how much was dealt
        targetCharacter_.charPhysState.DamageCharacter(totalHeal);
        CombatManager.globalReference.DisplayDamageDealt(timeDelay_, totalHeal, type, targetCharTile, isCrit);

        //Creating the visual effect for this effect
        CharacterSpriteBase targetCharSprite = CombatManager.globalReference.GetCharacterSprite(targetCharacter_);
        this.SpawnVisualAtLocation(targetCharSprite.transform.localPosition, targetCharSprite.transform);

        
        //Applying threat to all enemies based on the amount healed
        CombatManager.globalReference.ApplyActionThreat(this.characterWhoTriggered, null, totalHeal + bonusThreat, true);

        //Destroying this effect once everything is finished up
        Destroy(this.gameObject);
    }
}
