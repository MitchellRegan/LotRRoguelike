using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyStatsEffect : Effect
{
    //The range for the number of times this effects activates before it goes away
    public Vector2 numberOfTicksRange = new Vector2(5, 10);
    //The number of remaining ticks before this effect goes away
    [HideInInspector]
    public int ticksLeft = 1;

    //If true, this effect won't go away on its own, so it doesn't tick down
    public bool unlimitedTicks = false;

    [Space(9)]

    //If true, this effect ticks as soon as the effected character's turn begins
    public bool tickOnStartOfTurn = true;

    //If true, this effect ticks as soon as the effected character's turn ends
    public bool tickOnEndOfTurn = false;

    //If true, this effect ticks whenever the effected character attacks
    public bool tickOnAttack = false;

    //If true, this effect ticks whenever the effect character moves onto a space
    public bool tickOnMove = false;

    //The list of stat modifiers that are applied
    public List<StatModifier> StatChanges;



    //Function inherited from Effect.cs to trigger this effect. Sets the target that has their stats modified
    public override void TriggerEffect(Character usingCharacter_, Character targetCharacter_, float timeDelay_ = 0)
    {
        //Checking the targeted character to make sure this character isn't already applied to them
        foreach(Effect e in targetCharacter_.charCombatStats.combatEffects)
        {
            //If we find a version of this effect already on the target
            if(e.effectName == this.effectName)
            {
                //We refresh the duration of the effect on the target to the max number of ticks
                e.GetComponent<ModifyStatsEffect>().ticksLeft = Mathf.RoundToInt(this.numberOfTicksRange.y);
                //And then we destroy this effect's game object
                Destroy(this.gameObject);
            }
        }

        this.characterToEffect = targetCharacter_;
        this.characterWhoTriggered = usingCharacter_;

        //Adding this effect to the targeted character's combat effects list
        this.characterToEffect.charCombatStats.combatEffects.Add(this);

        //Determining how many ticks this effect will have if it's not unlimited
        if(!this.unlimitedTicks)
        {
            this.ticksLeft = Mathf.RoundToInt(Random.Range(this.numberOfTicksRange.x, this.numberOfTicksRange.y));
        }

        //Applying the stat changes
        this.ApplyStatChanges(true);
    }


    //Function called when this effect is applied or removed.
    private void ApplyStatChanges(bool addingChanges_)
    {
        //Looping through each stat modifier that this effect applies
        foreach(StatModifier mod in this.StatChanges)
        {
            switch(mod.modifiedStat)
            {
                //Increasing the target character's max health
                case StatModifier.StatName.Health:
                    //If we're adding the modifier
                    if(addingChanges_)
                    {
                        this.characterToEffect.charPhysState.currentHealth += Mathf.RoundToInt(mod.amountToChange);
                        this.characterToEffect.charPhysState.maxHealth += Mathf.RoundToInt(mod.amountToChange);
                    }
                    //If we're removing the modifier
                    else
                    {
                        this.characterToEffect.charPhysState.currentHealth -= Mathf.RoundToInt(mod.amountToChange);
                        this.characterToEffect.charPhysState.maxHealth -= Mathf.RoundToInt(mod.amountToChange);
                    }
                    break;

                //Increasing the target character's max energy level
                case StatModifier.StatName.Energy:
                    //If we're adding the modifier
                    if (addingChanges_)
                    {
                        this.characterToEffect.charPhysState.currentEnergy += Mathf.RoundToInt(mod.amountToChange);
                        this.characterToEffect.charPhysState.maxEnergy += Mathf.RoundToInt(mod.amountToChange);
                    }
                    //If we're removing the modifier
                    else
                    {
                        this.characterToEffect.charPhysState.currentEnergy -= Mathf.RoundToInt(mod.amountToChange);
                        this.characterToEffect.charPhysState.maxEnergy -= Mathf.RoundToInt(mod.amountToChange);
                    }
                    break;

                //Increasing the target character's combat initiative speed
                case StatModifier.StatName.Initiative:
                    //If we're adding the modifier
                    if (addingChanges_)
                    {
                        this.characterToEffect.charCombatStats.initiativeMod += mod.amountToChange;
                    }
                    //If we're removing the modifier
                    else
                    {
                        this.characterToEffect.charCombatStats.initiativeMod -= mod.amountToChange;
                    }
                    break;

                //Increasing the target character's combat evasion speed
                case StatModifier.StatName.Evasion:
                    //If we're adding the modifier
                    if (addingChanges_)
                    {
                        this.characterToEffect.charCombatStats.evasion += Mathf.RoundToInt(mod.amountToChange);
                    }
                    //If we're removing the modifier
                    else
                    {
                        this.characterToEffect.charCombatStats.evasion -= Mathf.RoundToInt(mod.amountToChange);
                    }
                    break;

                //Increasing the target character's combat skills
                case StatModifier.StatName.Punching:
                    //If we're adding the modifier
                    if (addingChanges_)
                    {
                        this.characterToEffect.charCombatStats.punchingMod += Mathf.RoundToInt(mod.amountToChange);
                    }
                    //If we're removing the modifier
                    else
                    {
                        this.characterToEffect.charCombatStats.punchingMod -= Mathf.RoundToInt(mod.amountToChange);
                    }
                    break;

                //Daggers
                case StatModifier.StatName.Daggers:
                    //If we're adding the modifier
                    if (addingChanges_)
                    {
                        this.characterToEffect.charCombatStats.daggersMod += Mathf.RoundToInt(mod.amountToChange);
                    }
                    //If we're removing the modifier
                    else
                    {
                        this.characterToEffect.charCombatStats.daggersMod -= Mathf.RoundToInt(mod.amountToChange);
                    }
                    break;

                //Swords
                case StatModifier.StatName.Swords:
                    //If we're adding the modifier
                    if (addingChanges_)
                    {
                        this.characterToEffect.charCombatStats.swordsMod += Mathf.RoundToInt(mod.amountToChange);
                    }
                    //If we're removing the modifier
                    else
                    {
                        this.characterToEffect.charCombatStats.swordsMod -= Mathf.RoundToInt(mod.amountToChange);
                    }
                    break;

                //Axes
                case StatModifier.StatName.Axes:
                    //If we're adding the modifier
                    if (addingChanges_)
                    {
                        this.characterToEffect.charCombatStats.axesMod += Mathf.RoundToInt(mod.amountToChange);
                    }
                    //If we're removing the modifier
                    else
                    {
                        this.characterToEffect.charCombatStats.axesMod -= Mathf.RoundToInt(mod.amountToChange);
                    }
                    break;

                //Spears
                case StatModifier.StatName.Spears:
                    //If we're adding the modifier
                    if (addingChanges_)
                    {
                        this.characterToEffect.charCombatStats.spearsMod += Mathf.RoundToInt(mod.amountToChange);
                    }
                    //If we're removing the modifier
                    else
                    {
                        this.characterToEffect.charCombatStats.spearsMod -= Mathf.RoundToInt(mod.amountToChange);
                    }
                    break;

                //Bows
                case StatModifier.StatName.Bows:
                    //If we're adding the modifier
                    if (addingChanges_)
                    {
                        this.characterToEffect.charCombatStats.bowsMod += Mathf.RoundToInt(mod.amountToChange);
                    }
                    //If we're removing the modifier
                    else
                    {
                        this.characterToEffect.charCombatStats.bowsMod -= Mathf.RoundToInt(mod.amountToChange);
                    }
                    break;

                //Improvised
                case StatModifier.StatName.Improvised:
                    //If we're adding the modifier
                    if (addingChanges_)
                    {
                        this.characterToEffect.charCombatStats.improvisedMod += Mathf.RoundToInt(mod.amountToChange);
                    }
                    //If we're removing the modifier
                    else
                    {
                        this.characterToEffect.charCombatStats.improvisedMod -= Mathf.RoundToInt(mod.amountToChange);
                    }
                    break;

                //Holy magic
                case StatModifier.StatName.HolyMagic:
                    //If we're adding the modifier
                    if (addingChanges_)
                    {
                        this.characterToEffect.charCombatStats.holyMagicMod += Mathf.RoundToInt(mod.amountToChange);
                    }
                    //If we're removing the modifier
                    else
                    {
                        this.characterToEffect.charCombatStats.holyMagicMod -= Mathf.RoundToInt(mod.amountToChange);
                    }
                    break;

                //Dark magic
                case StatModifier.StatName.DarkMagic:
                    //If we're adding the modifier
                    if (addingChanges_)
                    {
                        this.characterToEffect.charCombatStats.darkMagicMod += Mathf.RoundToInt(mod.amountToChange);
                    }
                    //If we're removing the modifier
                    else
                    {
                        this.characterToEffect.charCombatStats.darkMagicMod -= Mathf.RoundToInt(mod.amountToChange);
                    }
                    break;

                //Nature magic
                case StatModifier.StatName.NatureMagic:
                    //If we're adding the modifier
                    if (addingChanges_)
                    {
                        this.characterToEffect.charCombatStats.natureMagicMod += Mathf.RoundToInt(mod.amountToChange);
                    }
                    //If we're removing the modifier
                    else
                    {
                        this.characterToEffect.charCombatStats.natureMagicMod -= Mathf.RoundToInt(mod.amountToChange);
                    }
                    break;

                //Armor
                case StatModifier.StatName.Armor:
                    //If we're adding the modifier
                    if (addingChanges_)
                    {
                        this.characterToEffect.charInventory.totalPhysicalArmor += Mathf.RoundToInt(mod.amountToChange);
                    }
                    //If we're removing the modifier
                    else
                    {
                        this.characterToEffect.charInventory.totalPhysicalArmor -= Mathf.RoundToInt(mod.amountToChange);
                    }
                    break;

                //Magic resist
                case StatModifier.StatName.MagicResist:
                    //If we're adding the modifier
                    if (addingChanges_)
                    {
                        this.characterToEffect.charInventory.totalMagicResist += Mathf.RoundToInt(mod.amountToChange);
                    }
                    //If we're removing the modifier
                    else
                    {
                        this.characterToEffect.charInventory.totalMagicResist -= Mathf.RoundToInt(mod.amountToChange);
                    }
                    break;

                //Holy resist
                case StatModifier.StatName.HolyResist:
                    //If we're adding the modifier
                    if (addingChanges_)
                    {
                        this.characterToEffect.charInventory.totalHolyResist += Mathf.RoundToInt(mod.amountToChange);
                    }
                    //If we're removing the modifier
                    else
                    {
                        this.characterToEffect.charInventory.totalHolyResist -= Mathf.RoundToInt(mod.amountToChange);
                    }
                    break;

                //Dark resist
                case StatModifier.StatName.DarkResist:
                    //If we're adding the modifier
                    if (addingChanges_)
                    {
                        this.characterToEffect.charInventory.totalDarkResist += Mathf.RoundToInt(mod.amountToChange);
                    }
                    //If we're removing the modifier
                    else
                    {
                        this.characterToEffect.charInventory.totalDarkResist -= Mathf.RoundToInt(mod.amountToChange);
                    }
                    break;

                //Fire resist
                case StatModifier.StatName.FireResist:
                    //If we're adding the modifier
                    if (addingChanges_)
                    {
                        this.characterToEffect.charInventory.totalFireResist += Mathf.RoundToInt(mod.amountToChange);
                    }
                    //If we're removing the modifier
                    else
                    {
                        this.characterToEffect.charInventory.totalFireResist -= Mathf.RoundToInt(mod.amountToChange);
                    }
                    break;

                //Water resist
                case StatModifier.StatName.WaterResist:
                    //If we're adding the modifier
                    if (addingChanges_)
                    {
                        this.characterToEffect.charInventory.totalWaterResist += Mathf.RoundToInt(mod.amountToChange);
                    }
                    //If we're removing the modifier
                    else
                    {
                        this.characterToEffect.charInventory.totalWaterResist -= Mathf.RoundToInt(mod.amountToChange);
                    }
                    break;

                //Electric resist
                case StatModifier.StatName.ElectricResist:
                    //If we're adding the modifier
                    if (addingChanges_)
                    {
                        this.characterToEffect.charInventory.totalElectricResist += Mathf.RoundToInt(mod.amountToChange);
                    }
                    //If we're removing the modifier
                    else
                    {
                        this.characterToEffect.charInventory.totalElectricResist -= Mathf.RoundToInt(mod.amountToChange);
                    }
                    break;

                //Wind resist
                case StatModifier.StatName.WindResist:
                    //If we're adding the modifier
                    if (addingChanges_)
                    {
                        this.characterToEffect.charInventory.totalWindResist += Mathf.RoundToInt(mod.amountToChange);
                    }
                    //If we're removing the modifier
                    else
                    {
                        this.characterToEffect.charInventory.totalWindResist -= Mathf.RoundToInt(mod.amountToChange);
                    }
                    break;

                //Rock resist
                case StatModifier.StatName.RockResist:
                    //If we're adding the modifier
                    if (addingChanges_)
                    {
                        this.characterToEffect.charInventory.totalRockResist += Mathf.RoundToInt(mod.amountToChange);
                    }
                    //If we're removing the modifier
                    else
                    {
                        this.characterToEffect.charInventory.totalRockResist -= Mathf.RoundToInt(mod.amountToChange);
                    }
                    break;
            }
        }

        //If we're adding the effect, we need to create this effect's visual
        if(addingChanges_)
        {
            //Creating the visual effect for this effect
            CombatCharacterSprite targetCharSprite = CombatManager.globalReference.GetCharacterSprite(this.characterToEffect);
            this.SpawnVisualAtLocation(targetCharSprite.transform.localPosition, targetCharSprite.transform);
        }
    }


    //Function inherited from Effect.cs at the beginning of the effected character's turn
    public override void EffectOnStartOfTurn()
    {
        //If we tick on the start of turn
        if(this.tickOnStartOfTurn && !this.unlimitedTicks)
        {
            this.ticksLeft -= 1;

            //If we're out of ticks, we remove this effect
            if (this.ticksLeft <= 0)
            {
                this.RemoveEffect();
            }
        }
    }


    //Function inherited from Effect.cs at the end of the effected character's turn
    public override void EffectOnEndOfTurn()
    {
        //If we tick on the end of turn
        if (this.tickOnEndOfTurn && !this.unlimitedTicks)
        {
            this.ticksLeft -= 1;

            //If we're out of ticks, we remove this effect
            if (this.ticksLeft <= 0)
            {
                this.RemoveEffect();
            }
        }
    }


    //Function inherited from Effect.cs when the effected character performs an attack
    public override void EffectOnAttack()
    {
        //If we tick on attack
        if (this.tickOnAttack && !this.unlimitedTicks)
        {
            this.ticksLeft -= 1;

            //If we're out of ticks, we remove this effect
            if (this.ticksLeft <= 0)
            {
                this.RemoveEffect();
            }
        }
    }


    //Function inherited from Effect.cs when the effected character moves onto a tile
    public override void EffectOnMove()
    {
        //If we tick on move
        if (this.tickOnMove && !this.unlimitedTicks)
        {
            this.ticksLeft -= 1;

            //If we're out of ticks, we remove this effect
            if (this.ticksLeft <= 0)
            {
                this.RemoveEffect();
            }
        }
    }


    //Function inherited from Effect.cs when this effect is removed from the targeted character
    public override void RemoveEffect()
    {
        //Reversing all of the stat changes first
        this.ApplyStatChanges(false);
        //Destroying our visual effect object reference if it exists
        if(this.visualEffect != null)
        {
            Destroy(this.visualEffect);
        }

        base.RemoveEffect();
    }


    //Function inherited from Effect.cs to display the visual for this effect
    public override void SpawnVisualAtLocation(Vector3 posToSpawn_, Transform parentTransform_)
    {
        //Creating a new instance of our visual effect at the given location relative to the parent transform (if it isn't null)
        if (this.visualEffect != null)
        {
            GameObject visual = GameObject.Instantiate(this.visualEffect, posToSpawn_, new Quaternion(), parentTransform_);
            //Setting our visualEffect object reference as the created instance so we can destroy it later
            this.visualEffect = visual;
        }
    }
}


//Class used in ModifyStatEffect.cs for an individual 
[System.Serializable]
public class StatModifier
{
    //Enum for each stat that can be modified
    public enum StatName
    {
        Health,
        Energy,

        Initiative,
        Evasion,

        Punching,
        Daggers,
        Swords,
        Axes,
        Spears,
        Bows,
        Improvised,

        HolyMagic,
        DarkMagic,
        NatureMagic,

        Armor,
        MagicResist,
        HolyResist,
        DarkResist,
        FireResist,
        WaterResist,
        ElectricResist,
        WindResist,
        RockResist
    }

    //Enum for which stat this modifier changes
    public StatName modifiedStat = StatName.Health;

    //Amount that the modified stat is changed
    public float amountToChange = 1;
}