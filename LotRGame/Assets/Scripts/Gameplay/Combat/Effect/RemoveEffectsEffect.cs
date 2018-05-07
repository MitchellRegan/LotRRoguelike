using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveEffectsEffect : Effect
{
    //The number of effects to remove
    [Range(1, 10)]
    public int numEffectsToRemove = 1;

    //Enum for the type of effect to remove
    public enum EffectTypeToRemove
    {
        Any, //Removes any kind of effect
        Beneficial, //HoT and positive stat effects
        Negative, //DoT and negative stat effects
        DoT, //Damage over Time effects
        DoTType, //Damage over Time effect of a specific damage type
        HoT, //Heal over Time effects
        HoTType, //Heal over Time effect of a specific damage type
        ModifyStat, //Any modify stat effect
        StatBoost, //Modify stat effects that are beneficial
        StatNerf //Modify stat effects that are negative
    };
    public EffectTypeToRemove typeToRemove = EffectTypeToRemove.Any;

    //Bool for if this effect ignores the designated damage type
    public bool ignoreSelectedDamageType = false;

    //The damage type to remove if removing DoTType or HoTType
    public CombatManager.DamageType doTHoTType = CombatManager.DamageType.Arcane;



    //Function inherited from Effect.cs to trigger this effect
    public override void TriggerEffect(Character usingCharacter_, Character targetCharacter_, float timeDelay_ = 0)
    {
        //Setting the character references for the attacking and defending characters
        this.characterToEffect = targetCharacter_;
        this.characterWhoTriggered = usingCharacter_;

        //The list of effects that we remove
        List<Effect> removedEffects = new List<Effect>();

        //Looping through all of the target character's combat effects
        for(int e = 0; e < targetCharacter_.charCombatStats.combatEffects.Count; ++e)
        {
            //If the effect can be removed, we add it to our list to remove
            if(this.CanEffectBeRemoved(targetCharacter_.charCombatStats.combatEffects[e]))
            {
                Debug.Log("Removing " + targetCharacter_.charCombatStats.combatEffects[e].effectName + " from " + targetCharacter_.firstName);
                removedEffects.Add(targetCharacter_.charCombatStats.combatEffects[e]);

                //If the number of removed effects has reached the max amount we can remove, we break the loop
                if(removedEffects.Count >= this.numEffectsToRemove)
                {
                    break;
                }
            }
            else
            {
                Debug.Log("NOT removing " + targetCharacter_.charCombatStats.combatEffects[e].effectName);
            }
        }

        //Looping through each effect and removing it from the target player
        foreach(Effect re in removedEffects)
        {
            re.RemoveEffect();
        }
    }


    //Function called from TriggerEffect to check if the current effect can be removed
    private bool CanEffectBeRemoved(Effect effectToCheck_)
    {
        bool canRemove = false;

        //Switch statement for the type of effect we can remove
        switch(this.typeToRemove)
        {
            case EffectTypeToRemove.Any:
                //If we remove any effect type, obviously we remove it
                canRemove = true;
                break;

            case EffectTypeToRemove.Negative:
                //If the effect is damage over time, we can remove it
                if (effectToCheck_.GetType() == typeof(DamageOverTimeEffect))
                {
                    canRemove = true;
                }
                //If the effect modifies stats, we need to check if the stat change is negative
                else if(effectToCheck_.GetType() == typeof(ModifyStatsEffect))
                {
                    if (this.EffectNetBenefit(effectToCheck_.GetComponent<ModifyStatsEffect>()) < 0)
                    {
                        canRemove = true;
                    }
                }
                break;

            case EffectTypeToRemove.Beneficial:
                //If the effect is heal over time, we can remove it
                if (effectToCheck_.GetType() == typeof(HealOverTimeEffect))
                {
                    canRemove = true;
                }
                //If the effect modifies stats, we need to check if the stat change is positive
                else if (effectToCheck_.GetType() == typeof(ModifyStatsEffect))
                {
                    if (this.EffectNetBenefit(effectToCheck_.GetComponent<ModifyStatsEffect>()) > 0)
                    {
                        canRemove = true;
                    }
                }
                break;

            case EffectTypeToRemove.DoT:
                //If the effect is damage over time, we can remove it
                if(effectToCheck_.GetType() == typeof(DamageOverTimeEffect))
                {
                    canRemove = true;
                }
                break;

            case EffectTypeToRemove.DoTType:
                //If the effect is damage over time, we need to check if the damage type matches
                if (effectToCheck_.GetType() == typeof(DamageOverTimeEffect))
                {
                    //If we ignore a specific damage type and the DoT isn't that type then we remove
                    if(this.ignoreSelectedDamageType && effectToCheck_.GetComponent<DamageOverTimeEffect>().damageType != this.doTHoTType)
                    {
                        canRemove = true;
                    }
                    //If the effect's damage type is the same as the damage type we remove
                    else if (effectToCheck_.GetComponent<DamageOverTimeEffect>().damageType == this.doTHoTType)
                    {
                        canRemove = true;
                    }
                }
                break;

            case EffectTypeToRemove.HoT:
                //If the effect is heal over time, we can remove it
                if (effectToCheck_.GetType() == typeof(HealOverTimeEffect))
                {
                    canRemove = true;
                }
                break;

            case EffectTypeToRemove.HoTType:
                //If the effect is heal over time, we need to check if the damage type matches
                if (effectToCheck_.GetType() == typeof(HealOverTimeEffect))
                {
                    //If we ignore a specific damage type, and the HoT isn't that type then we remove
                    if(this.ignoreSelectedDamageType && effectToCheck_.GetComponent<HealOverTimeEffect>().healType != this.doTHoTType)
                    {
                        canRemove = true;
                    }
                    //If the effect's damage type is the same as the damage type we remove
                    else if (effectToCheck_.GetComponent<HealOverTimeEffect>().healType == this.doTHoTType)
                    {
                        canRemove = true;
                    }
                }
                break;

            case EffectTypeToRemove.ModifyStat:
                //If the effect modifies stats, we can remove it
                if (effectToCheck_.GetType() == typeof(ModifyStatsEffect))
                {
                    canRemove = true;
                }
                break;

            case EffectTypeToRemove.StatBoost:
                //If the effect modifies stats, we need to check if the stat change is positive
                if (effectToCheck_.GetType() == typeof(ModifyStatsEffect))
                {
                    if (this.EffectNetBenefit(effectToCheck_.GetComponent<ModifyStatsEffect>()) > 0)
                    {
                        canRemove = true;
                    }
                }
                break;

            case EffectTypeToRemove.StatNerf:
                //If the effect modifies stats, we need to check if the stat change is negative
                if (effectToCheck_.GetType() == typeof(ModifyStatsEffect))
                {
                    if(this.EffectNetBenefit(effectToCheck_.GetComponent<ModifyStatsEffect>()) < 0)
                    {
                        canRemove = true;
                    }
                }
                break;
        }

        return canRemove;
    }


    //Function called from CanEffectBeRemoved to check a ModifyStatsEffect.cs to see if the effect is good or bad
    public float EffectNetBenefit(ModifyStatsEffect effectToCheck_)
    {
        //The float for the net gain for the modify stat effect
        float netBenefit = 0;

        //Looping through each stat modifier in the effect
        foreach(StatModifier mod in effectToCheck_.StatChanges)
        {
            //Since the initiative mod is small, we have to multiply it before it's bonus is added
            if(mod.modifiedStat == StatModifier.StatName.Initiative)
            {
                netBenefit += 20 * mod.amountToChange;
            }
            //Adding the stat modifier amount to the net
            else
            {
                netBenefit += mod.amountToChange;
            }
        }

        return netBenefit;
    }
}
