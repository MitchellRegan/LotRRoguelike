using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DamageOverTimeEffect : Effect
{
    //What type of damage this damage counts as
    public CombatManager.DamageType damageType = CombatManager.DamageType.Arcane;

    //The chance that this effect will happen whenever it's triggered
    [Range(0,1)]
    public float chanceToTrigger = 1;

    //The range of damage that is dealt to the target
    public Vector2 damagePerTickRange = new Vector2(1, 8);

    //The crit chance of this effect whenever it deals damage
    [Range(0,1)]
    public float critChance = 0;

    //The damage multiplier whenever this effect crits
    public int critMultiplier = 2;

    //The range for the number of times this effect activates before it goes away
    public int numberOfTicks = 3;
    //The number of remaining ticks before this effect goes away
    [HideInInspector]
    public int ticksLeft = 1;

    //If true, this effect won't go away on its own, so it doesn't tick down
    public bool unlimitedTicks = false;

    //The amount of times that this effect can stack on the same target
    [Range(1, 10)]
    public int maxStackSize = 1;

    [Space(9)]

    //If true, this effect ticks down during the time that player initiative is filling up
    public bool tickOnRealTime = true;
    //If this ticks on real time, this is the amount of time it takes between ticks
    public float timeBetweenTicks = 1;
    //The current amount of time that's passed between ticks
    private float currentTickTime = 0;

    //If true, this effect ticks as soon as the effected character's turn begins
    public bool tickOnStartOfTurn = false;

    //If true, this effect ticks as soon as the effected character's turn ends
    public bool tickOnEndOfTurn = false;

    //If true, this effect ticks whenever the effected character attacks
    public bool tickOnAttack = false;

    //If true, this effect ticks whenever the effect character moves onto a space
    public bool tickOnMove = false;



    //Function inherited from Effect.cs to trigger this effect. Sets the target as the damaged character
    public override void TriggerEffect(Character usingCharacter_, Character targetCharacter_, float timeDelay_ = 0)
    {
        //Reference to the effect that's got the least number of ticks left
        DamageOverTimeEffect lowestTickEffect = null;

        //Checking the targeted character to see if this effect is already applied to them and how many stacks
        int stackSize = 0;
        foreach(Effect e in targetCharacter_.charCombatStats.combatEffects)
        {
            //If we find a version of this effect already on the target
            if(e.effectName == this.effectName)
            {
                //We increase the current count for the number of stacks found
                stackSize += 1;

                //If the current effect's number of ticks is less than the current lowest tick effect or if the current lowest doesn't exist, this becomes the lowest
                if(lowestTickEffect == null || e.GetComponent<DamageOverTimeEffect>().ticksLeft < lowestTickEffect.ticksLeft)
                {
                    lowestTickEffect = e.GetComponent<DamageOverTimeEffect>();
                }

                //If the stack size found is higher than this effect's max stack size, we just refresh the ticks on the effect with the lowest
                if (stackSize >= this.maxStackSize)
                {
                    //We refresh the duration of the effect on the target to the max number of ticks
                    lowestTickEffect.ticksLeft = this.numberOfTicks;
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
        if(!this.unlimitedTicks)
        {
            this.ticksLeft = this.numberOfTicks;
        }
    }


    //Function called whenever this effect deals damage
    private void DamageCharacter()
    {
        //Making sure the character isn't dead before dealing damage
        if (this.characterToEffect == null || this.characterToEffect.charPhysState.currentHealth <= 0)
        {
            return;
        }

        //Seeing if this effect will trigger
        float triggerRoll = Random.Range(0, 1);
        if(triggerRoll > this.chanceToTrigger)
        {
            //If we roll over the trigger chance, nothing happens and we don't tick
            return;
        }

        //Finding out how much damage we deal this tick
        int damageDealt = Mathf.RoundToInt(Random.Range(this.damagePerTickRange.x, this.damagePerTickRange.y));

        //Finding out if we crit
        float critRoll = Random.Range(0,1);
        bool didThisCrit = false;
        if(critRoll < this.critChance)
        {
            //If we crit this tick, the damage is multiplied
            damageDealt = damageDealt * this.critMultiplier;
            didThisCrit = true;
        }

        //Looping through the perks of the character that used this ability to see if they have any damage type boost perks
        foreach (Perk charPerk in this.characterWhoTriggered.charPerks.allPerks)
        {
            if (charPerk.GetType() == typeof(DamageTypeBoostPerk) && this.damageType == charPerk.GetComponent<DamageTypeBoostPerk>().damageTypeToBoost)
            {
                damageDealt += charPerk.GetComponent<DamageTypeBoostPerk>().GetDamageBoostAmount(this.characterWhoTriggered, didThisCrit, true, this.damageType);
            }
        }

        //Looping through the defending character's perks to see if they have any spell resist or absorb perks
        SpellResistTypes magicResistType = SpellResistTypes.Normal;
        foreach (Perk defPerk in this.characterToEffect.charPerks.allPerks)
        {
            if (defPerk.GetType() == typeof(SpellResistAbsorbPerk))
            {
                SpellResistAbsorbPerk resistPerk = defPerk.GetComponent<SpellResistAbsorbPerk>();

                //Checking to see if the current damage type is the same as this spell resist perk
                if (resistPerk.typeToResist == this.damageType)
                {
                    //Checking to see if the damage is negated entirely
                    if (resistPerk.negateAllDamage)
                    {
                        //If the resist type for this spell isn't on absorb, we can negate it. ALWAYS have preference to absorb because it heals
                        if (magicResistType != SpellResistTypes.Absorb)
                        {
                            magicResistType = SpellResistTypes.Negate;
                        }
                    }
                    //Checking to see if the damage is absorbed to heal the target
                    else if (resistPerk.absorbDamage)
                    {
                        magicResistType = SpellResistTypes.Absorb;
                        //Applying the damage reduction so the defender isn't healed as much
                        damageDealt -= resistPerk.GetSpellResistAmount(this.characterToEffect, didThisCrit, false);
                    }
                    //Otherwise we just get the amount that it normally resists
                    else
                    {
                        damageDealt -= resistPerk.GetSpellResistAmount(this.characterToEffect, didThisCrit, false);
                    }
                }
            }
        }

        //Subtracting any magic resistance from the damage that we're trying to deal
        switch (this.damageType)
        {
            case CombatManager.DamageType.Arcane:
                damageDealt -= this.characterToEffect.charInventory.totalArcaneResist;
                break;
            case CombatManager.DamageType.Fire:
                damageDealt -= this.characterToEffect.charInventory.totalFireResist;
                break;
            case CombatManager.DamageType.Water:
                damageDealt -= this.characterToEffect.charInventory.totalWaterResist;
                break;
            case CombatManager.DamageType.Electric:
                damageDealt -= this.characterToEffect.charInventory.totalElectricResist;
                break;
            case CombatManager.DamageType.Wind:
                damageDealt -= this.characterToEffect.charInventory.totalWindResist;
                break;
            case CombatManager.DamageType.Nature:
                damageDealt -= this.characterToEffect.charInventory.totalNatureResist;
                break;
            case CombatManager.DamageType.Holy:
                damageDealt -= this.characterToEffect.charInventory.totalHolyResist;
                break;
            case CombatManager.DamageType.Dark:
                damageDealt -= this.characterToEffect.charInventory.totalDarkResist;
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

                //If the perk has the same damage type as this DoT or it affects all damage types
                if(threatPerk.damageTypeToThreaten == this.damageType || threatPerk.threatenAllDamageTypes)
                {
                    bonusThreat += threatPerk.GetAddedActionThreat(damageDealt, didThisCrit, true);
                }
            }
        }

        //If the damage was dealt normally
        if (magicResistType == SpellResistTypes.Normal)
        {
            //Dealing the damage to the effected character
            this.characterToEffect.charPhysState.DamageCharacter(damageDealt);

            //Telling the combat manager to display the damage dealt
            CombatTile damagedCharTile = CombatManager.globalReference.combatTileGrid[this.characterToEffect.charCombatStats.gridPositionCol][this.characterToEffect.charCombatStats.gridPositionRow];

            CombatManager.globalReference.DisplayDamageDealt(0, damageDealt, this.damageType, damagedCharTile, didThisCrit);
            
            //If this character has the EnemyCombatAI component, we increase the threat for the character who put this effect on
            if (this.characterToEffect.GetComponent<EnemyCombatAI_Basic>())
            {
                //If the character who cast this effect is a player character, we make the enemies hate that character
                if (!this.characterWhoTriggered.GetComponent<EnemyCombatAI_Basic>())
                {
                    //If the attack didn't crit
                    if (!didThisCrit)
                    {
                        //Applying threat to the targeted character
                        this.characterToEffect.GetComponent<EnemyCombatAI_Basic>().IncreaseThreat(this.characterWhoTriggered, damageDealt + bonusThreat);
                    }
                    //If the attack did crit, we boost threat against all enemies by 25%
                    else
                    {
                        //Finding the bonus amount of threat that's applied to all enemies
                        int boostedThreat = damageDealt + bonusThreat;
                        boostedThreat = Mathf.RoundToInt(boostedThreat * 0.25f);
                        CombatManager.globalReference.ApplyActionThreat(this.characterWhoTriggered, null, boostedThreat, true);

                        //Applying the rest of the threat to the target character
                        CombatManager.globalReference.ApplyActionThreat(this.characterWhoTriggered, this.characterToEffect, damageDealt + bonusThreat - boostedThreat, false);
                    }
                }
            }
        }
        //If the damage was negated completely
        else if(magicResistType == SpellResistTypes.Negate)
        {
            //Telling the combat manager to display no damage dealt
            CombatTile damagedCharTile = CombatManager.globalReference.combatTileGrid[this.characterToEffect.charCombatStats.gridPositionCol][this.characterToEffect.charCombatStats.gridPositionRow];
            CombatManager.globalReference.DisplayDamageDealt(0, 0, this.damageType, damagedCharTile, didThisCrit);
        }
        //If the damage was absorbed and healed the character
        else if(magicResistType == SpellResistTypes.Absorb)
        {
            //Healing the damage to the effected character
            this.characterToEffect.charPhysState.HealCharacter(damageDealt);

            //Telling the combat manager to display the damage healed
            CombatTile damagedCharTile = CombatManager.globalReference.combatTileGrid[this.characterToEffect.charCombatStats.gridPositionCol][this.characterToEffect.charCombatStats.gridPositionRow];
            CombatManager.globalReference.DisplayDamageDealt(0, damageDealt, this.damageType, damagedCharTile, didThisCrit, true);

            //If the caster of this effect and the target are player characters, we increase the threat for the character who put this effect on them
            if (!this.characterToEffect.GetComponent<EnemyCombatAI_Basic>() && !this.characterWhoTriggered.GetComponent<EnemyCombatAI_Basic>())
            {
                //Applying threat to all enemies for the amount that's healed
                CombatManager.globalReference.ApplyActionThreat(this.characterWhoTriggered, null, damageDealt + bonusThreat, true);
            }
        }

        //Creating the visual effect for this effect
        CharacterSpriteBase targetCharSprite = CombatManager.globalReference.GetCharacterSprite(this.characterToEffect);
        this.SpawnVisualAtLocation(targetCharSprite.transform.localPosition, targetCharSprite.transform);

        //If this effect isn't unlimited, we need to reduce the ticks remaining
        if(!this.unlimitedTicks)
        {
            this.ticksLeft -= 1;

            //If there are no more ticks left, this effect is over and the object is destroyed
            if(this.ticksLeft <= 0)
            {
                this.RemoveEffect();
            }
        }
    }


    //Function called every frame
    private void Update()
    {
        //If this effect ticks while player initiative is building
        if (this.tickOnRealTime)
        {
            //We can only track our timer when the combat manager is increasing initiative
            if (CombatManager.globalReference.currentState == CombatManager.combatState.IncreaseInitiative)
            {
                //Increasing our tick timer
                this.currentTickTime += Time.deltaTime;

                //If the timer is above the tick time, we damage the target
                if(this.currentTickTime >= this.timeBetweenTicks)
                {
                    //Resetting the timer and damaging the target character
                    this.currentTickTime -= this.timeBetweenTicks;
                    this.DamageCharacter();
                }
            }
        }
    }


    //Function inherited from Effect.cs at the beginning of the effected character's turn
    public override void EffectOnStartOfTurn()
    {
        //If we damage on the start of the turn
        if(this.tickOnStartOfTurn)
        {
            this.DamageCharacter();
        }
    }


    //Function inherited from Effect.cs at the end of the effected character's turn
    public override void EffectOnEndOfTurn()
    {
        //If we damage on the end of the turn
        if(this.tickOnEndOfTurn)
        {
            this.DamageCharacter();
        }
    }


    //Function inherited from Effect.cs when the effected character performs an attack
    public override void EffectOnAttack()
    {
        //If we damage on attack
        if(this.tickOnAttack)
        {
            this.DamageCharacter();
        }
    }


    //Function inherited from Effect.cs when the effected character moves onto a tile
    public override void EffectOnMove()
    {
        //If we damage on move
        if(this.tickOnMove)
        {
            this.DamageCharacter();
        }
    }


    //Function inherited from Effect.cs when this effect is removed from the targeted character
    public override void RemoveEffect()
    {
        base.RemoveEffect();
    }
}
