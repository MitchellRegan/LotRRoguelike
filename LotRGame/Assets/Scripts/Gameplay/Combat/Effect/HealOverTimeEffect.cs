using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HealOverTimeEffect : Effect
{
    //What type of magic this damage counts as
    public CombatManager.DamageType healType = CombatManager.DamageType.Arcane;

    //The chance that this effect will happen whenever it's triggered
    [Range(0, 1)]
    public float chanceToTrigger = 1;

    //The range of health that is restored to the target per tick
    public Vector2 healPerTickRange = new Vector2(1, 8);

    //The crit chance of this effect whenever it heals
    [Range(0, 1)]
    public float critChance = 0;

    //The damage multiplier whenever this effect crits
    public int critMultiplier = 2;

    //The range for the number of times this effects activates before it goes away
    public Vector2 numberOfTicksRange = new Vector2(5, 10);
    //The number of remaining ticks before this effect goes away
    [HideInInspector]
    public int ticksLeft = 1;

    //If true, this effect won't go away on its own, so it doesn't tick down
    public bool unlimitedTicks = false;

    //The amount of times that this effect can stack on the same target
    [Range(1, 10)]
    public int maxStackSize = 1;

    [Space(9)]

    //If true, this effect ticks as soon as the effected character's turn begins
    public bool tickOnStartOfTurn = true;

    //If true, this effect ticks as soon as the effected character's turn ends
    public bool tickOnEndOfTurn = false;

    //If true, this effect ticks whenever the effected character attacks
    public bool tickOnAttack = false;

    //If true, this effect ticks whenever the effect character moves onto a space
    public bool tickOnMove = false;



    //Function inherited from Effect.cs to trigger this effect. Sets the target as the healed character
    public override void TriggerEffect(Character usingCharacter_, Character targetCharacter_, float timeDelay_ = 0)
    {
        //Reference to the effect that's got the least number of ticks left
        HealOverTimeEffect lowestTickEffect = null;

        //Checking the targeted character to see if this effect is already applied to them and how many stacks
        int stackSize = 0;
        foreach (Effect e in targetCharacter_.charCombatStats.combatEffects)
        {
            //If we find a version of this effect already on the target
            if (e.effectName == this.effectName)
            {
                //We increase the current count for the number of stacks found
                stackSize += 1;

                //If the current effect's number of ticks is less than the current lowest tick effect or if the current lowest doesn't exist, this becomes the lowest
                if (lowestTickEffect == null || e.GetComponent<HealOverTimeEffect>().ticksLeft < lowestTickEffect.ticksLeft)
                {
                    lowestTickEffect = e.GetComponent<HealOverTimeEffect>();
                }

                //If the stack size found is higher than this effect's max stack size, we just refresh the ticks on the effect with the lowest
                if (stackSize >= this.maxStackSize)
                {
                    //We refresh the duration of the effect on the target to the max number of ticks
                    lowestTickEffect.ticksLeft = Mathf.RoundToInt(this.numberOfTicksRange.y);
                    //And then we destroy this effect's game object
                    Destroy(this.gameObject);
                }
            }
        }

        this.characterToEffect = targetCharacter_;
        this.characterWhoTriggered = usingCharacter_;

        //Adding this effect to the targeted character's combat effects list
        this.characterToEffect.charCombatStats.combatEffects.Add(this);

        //Determining how many ticks this effect will have (if it's not unlimited that is)
        if (!this.unlimitedTicks)
        {
            this.ticksLeft = Mathf.RoundToInt(Random.Range(this.numberOfTicksRange.x, this.numberOfTicksRange.y));
        }
    }


    //Function called whenever this effect heals the target character
    private void HealCharacter()
    {
        //Making sure the character isn't dead before healing
        if(this.characterToEffect.charPhysState.currentHealth <= 0)
        {
            return;
        }

        //Seeing if this effect will trigger
        float triggerRoll = Random.Range(0, 1);
        if (triggerRoll > this.chanceToTrigger)
        {
            //If we roll over the trigger chance, nothing happens and we don't tick
            return;
        }

        //Finding out how much health we heal this tick
        int damagehealed = Mathf.RoundToInt(Random.Range(this.healPerTickRange.x, this.healPerTickRange.y));

        //Finding out if we crit
        float critRoll = Random.Range(0, 1);
        bool didThisCrit = false;
        if (critRoll < this.critChance)
        {
            //If we crit this tick, the amount healed is multiplied
            damagehealed = damagehealed * this.critMultiplier;
            didThisCrit = true;
        }

        //Looping through the perks of the character that used this ability to see if they have any damage type boost perks
        foreach (Perk charPerk in this.characterWhoTriggered.charPerks.allPerks)
        {
            if (charPerk.GetType() == typeof(DamageTypeBoostPerk) && this.healType == charPerk.GetComponent<DamageTypeBoostPerk>().damageTypeToBoost)
            {
                damagehealed += charPerk.GetComponent<DamageTypeBoostPerk>().GetDamageBoostAmount(this.characterWhoTriggered, didThisCrit, true);
            }
        }

        //Looping through the defending character's perks to see if they have any spell resist or absorb perks
        SpellResistTypes magicResistType = SpellResistTypes.Normal;
        foreach (Perk defPerk in this.characterToEffect.charPerks.allPerks)
        {
            if (defPerk.GetType() == typeof(SpellResistAbsorbPerk))
            {
                SpellResistAbsorbPerk resistPerk = defPerk.GetComponent<SpellResistAbsorbPerk>();

                //Checking to see if the current heal type is the same as this spell resist perk
                if (resistPerk.typeToResist == this.healType)
                {
                    //Checking to see if the heal is negated entirely
                    if (resistPerk.negateAllDamage)
                    {
                        magicResistType = SpellResistTypes.Negate;
                    }
                    //Otherwise we just get the amount that it normally resists
                    else
                    {
                        damagehealed -= resistPerk.GetSpellResistAmount(this.characterToEffect, didThisCrit, false);
                    }
                }
            }
        }

        //Subtracting any magic resistance from the amount that we're trying to heal
        switch (this.healType)
        {
            case CombatManager.DamageType.Arcane:
                damagehealed -= this.characterToEffect.charInventory.totalArcaneResist;
                break;
            case CombatManager.DamageType.Fire:
                damagehealed -= this.characterToEffect.charInventory.totalFireResist;
                break;
            case CombatManager.DamageType.Water:
                damagehealed -= this.characterToEffect.charInventory.totalWaterResist;
                break;
            case CombatManager.DamageType.Electric:
                damagehealed -= this.characterToEffect.charInventory.totalElectricResist;
                break;
            case CombatManager.DamageType.Wind:
                damagehealed -= this.characterToEffect.charInventory.totalWindResist;
                break;
            case CombatManager.DamageType.Nature:
                damagehealed -= this.characterToEffect.charInventory.totalNatureResist;
                break;
            case CombatManager.DamageType.Holy:
                damagehealed -= this.characterToEffect.charInventory.totalHolyResist;
                break;
            case CombatManager.DamageType.Dark:
                damagehealed -= this.characterToEffect.charInventory.totalDarkResist;
                break;
                //Pure damage type has no resist
        }

        //Looping through the attacking character's perks to see if there's any bonus threat to add to this effect
        int bonusThreat = 0;
        foreach (Perk charPerk in this.characterWhoTriggered.charPerks.allPerks)
        {
            //If the perk is a threat boosting perk
            if (charPerk.GetType() == typeof(ThreatBoostPerk))
            {
                ThreatBoostPerk threatPerk = charPerk.GetComponent<ThreatBoostPerk>();

                //If the perk has the same damage type as this HoT or it affects all damage types
                if (threatPerk.damageTypeToThreaten == this.healType || threatPerk.threatenAllDamageTypes)
                {
                    bonusThreat += threatPerk.GetAddedActionThreat(damagehealed, didThisCrit, true);
                }
            }
        }

        //If the heal was negated completely
        if(magicResistType == SpellResistTypes.Negate)
        {
            //Telling the combat manager to display that no damage was healed
            CombatTile healedCharTile = CombatManager.globalReference.combatTileGrid[this.characterToEffect.charCombatStats.gridPositionCol][this.characterToEffect.charCombatStats.gridPositionRow];
            CombatManager.globalReference.DisplayDamageDealt(0, 0, this.healType, healedCharTile, didThisCrit, true);
        }
        //Otherwise, the heal happens normally
        else
        {
            //Healing the damage to the effected character
            this.characterToEffect.charPhysState.HealCharacter(damagehealed);

            //Telling the combat manager to display the damage healed
            CombatTile healedCharTile = CombatManager.globalReference.combatTileGrid[this.characterToEffect.charCombatStats.gridPositionCol][this.characterToEffect.charCombatStats.gridPositionRow];
            CombatManager.globalReference.DisplayDamageDealt(0, damagehealed, this.healType, healedCharTile, didThisCrit, true);

            //If the target character and the character who cast this effect are player characters, we need to increase threat
            if (!this.characterToEffect.GetComponent<EnemyCombatAI_Basic>() && !this.characterWhoTriggered.GetComponent<EnemyCombatAI_Basic>())
            {
                //Applying threat to all enemies for the amount that's healed
                CombatManager.globalReference.ApplyActionThreat(this.characterWhoTriggered, null, damagehealed + bonusThreat, true);
            }
        }
        
        //Creating the visual effect for this effect
        CharacterSpriteBase targetCharSprite = CombatManager.globalReference.GetCharacterSprite(this.characterToEffect);
        this.SpawnVisualAtLocation(targetCharSprite.transform.localPosition, targetCharSprite.transform);

        //If this effect isn't unlimited, we need to reduce the ticks remaining
        if (!this.unlimitedTicks)
        {
            this.ticksLeft -= 1;

            //If there are no more ticks left, this effect is over and the object is destroyed
            if (this.ticksLeft <= 0)
            {
                this.RemoveEffect();
            }
        }
    }


    //Function inherited from Effect.cs at the beginning of the effected character's turn
    public override void EffectOnStartOfTurn()
    {
        //If we damage on the start of the turn
        if (this.tickOnStartOfTurn)
        {
            this.HealCharacter();
        }
    }


    //Function inherited from Effect.cs at the end of the effected character's turn
    public override void EffectOnEndOfTurn()
    {
        //If we damage on the end of the turn
        if (this.tickOnEndOfTurn)
        {
            this.HealCharacter();
        }
    }


    //Function inherited from Effect.cs when the effected character performs an attack
    public override void EffectOnAttack()
    {
        //If we damage on attack
        if (this.tickOnAttack)
        {
            this.HealCharacter();
        }
    }


    //Function inherited from Effect.cs when the effected character moves onto a tile
    public override void EffectOnMove()
    {
        //If we damage on move
        if (this.tickOnMove)
        {
            this.HealCharacter();
        }
    }


    //Function inherited from Effect.cs when this effect is removed from the targeted character
    public override void RemoveEffect()
    {
        base.RemoveEffect();
    }
}
