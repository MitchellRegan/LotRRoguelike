﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ModifyStatsEffect : Effect
{
    //The number of times this effects activates before it goes away
    public int numberOfTicks = 3;
    //The number of remaining ticks before this effect goes away
    [HideInInspector]
    public int ticksLeft = 1;

    //If true, this effect won't go away on its own, so it doesn't tick down
    public bool unlimitedTicks = false;

    [Space(9)]

    //If true, this effect ticks down during the time that player initiative is filling up
    public bool tickOnRealTime = true;
    //If this ticks on real time, this is the amount of time it takes between ticks
    public float timeBetweenTicks = 1;
    //The current amount of time that's passed between ticks
    private float currentTickTime = 0;

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
                e.GetComponent<ModifyStatsEffect>().ticksLeft = this.numberOfTicks;
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
            this.ticksLeft = this.numberOfTicks;
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
                case StatModifier.StatName.Unarmed:
                    //If we're adding the modifier
                    if (addingChanges_)
                    {
                        this.characterToEffect.charSkills.ChangeSkillModifier(SkillList.Unarmed, Mathf.RoundToInt(mod.amountToChange));
                    }
                    //If we're removing the modifier
                    else
                    {
                        this.characterToEffect.charSkills.ChangeSkillModifier(SkillList.Unarmed, -Mathf.RoundToInt(mod.amountToChange));
                    }
                    break;

                //Daggers
                case StatModifier.StatName.Daggers:
                    //If we're adding the modifier
                    if (addingChanges_)
                    {
                        this.characterToEffect.charSkills.ChangeSkillModifier(SkillList.Daggers, Mathf.RoundToInt(mod.amountToChange));
                    }
                    //If we're removing the modifier
                    else
                    {
                        this.characterToEffect.charSkills.ChangeSkillModifier(SkillList.Daggers, -Mathf.RoundToInt(mod.amountToChange));
                    }
                    break;

                //Swords
                case StatModifier.StatName.Swords:
                    //If we're adding the modifier
                    if (addingChanges_)
                    {
                        this.characterToEffect.charSkills.ChangeSkillModifier(SkillList.Swords, Mathf.RoundToInt(mod.amountToChange));
                    }
                    //If we're removing the modifier
                    else
                    {
                        this.characterToEffect.charSkills.ChangeSkillModifier(SkillList.Swords, -Mathf.RoundToInt(mod.amountToChange));
                    }
                    break;

                //Axes
                case StatModifier.StatName.Mauls:
                    //If we're adding the modifier
                    if (addingChanges_)
                    {
                        this.characterToEffect.charSkills.ChangeSkillModifier(SkillList.Mauls, Mathf.RoundToInt(mod.amountToChange));
                    }
                    //If we're removing the modifier
                    else
                    {
                        this.characterToEffect.charSkills.ChangeSkillModifier(SkillList.Mauls, -Mathf.RoundToInt(mod.amountToChange));
                    }
                    break;

                //Spears
                case StatModifier.StatName.Poles:
                    //If we're adding the modifier
                    if (addingChanges_)
                    {
                        this.characterToEffect.charSkills.ChangeSkillModifier(SkillList.Poles, Mathf.RoundToInt(mod.amountToChange));
                    }
                    //If we're removing the modifier
                    else
                    {
                        this.characterToEffect.charSkills.ChangeSkillModifier(SkillList.Poles, -Mathf.RoundToInt(mod.amountToChange));
                    }
                    break;

                //Bows
                case StatModifier.StatName.Bows:
                    //If we're adding the modifier
                    if (addingChanges_)
                    {
                        this.characterToEffect.charSkills.ChangeSkillModifier(SkillList.Bows, Mathf.RoundToInt(mod.amountToChange));
                    }
                    //If we're removing the modifier
                    else
                    {
                        this.characterToEffect.charSkills.ChangeSkillModifier(SkillList.Bows, -Mathf.RoundToInt(mod.amountToChange));
                    }
                    break;

                //Shields
                case StatModifier.StatName.Shields:
                    //If we're adding the modifier
                    if (addingChanges_)
                    {
                        this.characterToEffect.charSkills.ChangeSkillModifier(SkillList.Shields, Mathf.RoundToInt(mod.amountToChange));
                    }
                    //If we're removing the modifier
                    else
                    {
                        this.characterToEffect.charSkills.ChangeSkillModifier(SkillList.Shields, -Mathf.RoundToInt(mod.amountToChange));
                    }
                    break;

                //Arcane magic
                case StatModifier.StatName.ArcaneMagic:
                    //If we're adding the modifier
                    if (addingChanges_)
                    {
                        this.characterToEffect.charSkills.ChangeSkillModifier(SkillList.ArcaneMagic, Mathf.RoundToInt(mod.amountToChange));
                    }
                    //If we're removing the modifier
                    else
                    {
                        this.characterToEffect.charSkills.ChangeSkillModifier(SkillList.ArcaneMagic, -Mathf.RoundToInt(mod.amountToChange));
                    }
                    break;


                //Holy magic
                case StatModifier.StatName.HolyMagic:
                    //If we're adding the modifier
                    if (addingChanges_)
                    {
                        this.characterToEffect.charSkills.ChangeSkillModifier(SkillList.HolyMagic, Mathf.RoundToInt(mod.amountToChange));
                    }
                    //If we're removing the modifier
                    else
                    {
                        this.characterToEffect.charSkills.ChangeSkillModifier(SkillList.HolyMagic, -Mathf.RoundToInt(mod.amountToChange));
                    }
                    break;

                //Dark magic
                case StatModifier.StatName.DarkMagic:
                    //If we're adding the modifier
                    if (addingChanges_)
                    {
                        this.characterToEffect.charSkills.ChangeSkillModifier(SkillList.DarkMagic, Mathf.RoundToInt(mod.amountToChange));
                    }
                    //If we're removing the modifier
                    else
                    {
                        this.characterToEffect.charSkills.ChangeSkillModifier(SkillList.DarkMagic, -Mathf.RoundToInt(mod.amountToChange));
                    }
                    break;

                //Fire magic
                case StatModifier.StatName.FireMagic:
                    //If we're adding the modifier
                    if (addingChanges_)
                    {
                        this.characterToEffect.charSkills.ChangeSkillModifier(SkillList.FireMagic, Mathf.RoundToInt(mod.amountToChange));
                    }
                    //If we're removing the modifier
                    else
                    {
                        this.characterToEffect.charSkills.ChangeSkillModifier(SkillList.FireMagic, -Mathf.RoundToInt(mod.amountToChange));
                    }
                    break;

                //Water magic
                case StatModifier.StatName.WaterMagic:
                    //If we're adding the modifier
                    if (addingChanges_)
                    {
                        this.characterToEffect.charSkills.ChangeSkillModifier(SkillList.WaterMagic, Mathf.RoundToInt(mod.amountToChange));
                    }
                    //If we're removing the modifier
                    else
                    {
                        this.characterToEffect.charSkills.ChangeSkillModifier(SkillList.WaterMagic, -Mathf.RoundToInt(mod.amountToChange));
                    }
                    break;

                //Wind magic
                case StatModifier.StatName.WindMagic:
                    //If we're adding the modifier
                    if (addingChanges_)
                    {
                        this.characterToEffect.charSkills.ChangeSkillModifier(SkillList.WindMagic, Mathf.RoundToInt(mod.amountToChange));
                    }
                    //If we're removing the modifier
                    else
                    {
                        this.characterToEffect.charSkills.ChangeSkillModifier(SkillList.WindMagic, -Mathf.RoundToInt(mod.amountToChange));
                    }
                    break;

                //Electric magic
                case StatModifier.StatName.ElectricMagic:
                    //If we're adding the modifier
                    if (addingChanges_)
                    {
                        this.characterToEffect.charSkills.ChangeSkillModifier(SkillList.ElectricMagic, Mathf.RoundToInt(mod.amountToChange));
                    }
                    //If we're removing the modifier
                    else
                    {
                        this.characterToEffect.charSkills.ChangeSkillModifier(SkillList.ElectricMagic, -Mathf.RoundToInt(mod.amountToChange));
                    }
                    break;

                //Stone magic
                case StatModifier.StatName.StoneMagic:
                    //If we're adding the modifier
                    if (addingChanges_)
                    {
                        this.characterToEffect.charSkills.ChangeSkillModifier(SkillList.StoneMagic, Mathf.RoundToInt(mod.amountToChange));
                    }
                    //If we're removing the modifier
                    else
                    {
                        this.characterToEffect.charSkills.ChangeSkillModifier(SkillList.StoneMagic, -Mathf.RoundToInt(mod.amountToChange));
                    }
                    break;

                //Armor
                case StatModifier.StatName.SlashingArmor:
                    //If we're adding the modifier
                    if (addingChanges_)
                    {
                        this.characterToEffect.charInventory.totalSlashingArmor += Mathf.RoundToInt(mod.amountToChange);
                    }
                    //If we're removing the modifier
                    else
                    {
                        this.characterToEffect.charInventory.totalSlashingArmor -= Mathf.RoundToInt(mod.amountToChange);
                    }
                    break;

                //Armor
                case StatModifier.StatName.StabbingArmor:
                    //If we're adding the modifier
                    if (addingChanges_)
                    {
                        this.characterToEffect.charInventory.totalStabbingArmor += Mathf.RoundToInt(mod.amountToChange);
                    }
                    //If we're removing the modifier
                    else
                    {
                        this.characterToEffect.charInventory.totalStabbingArmor -= Mathf.RoundToInt(mod.amountToChange);
                    }
                    break;

                //Armor
                case StatModifier.StatName.CrushingArmor:
                    //If we're adding the modifier
                    if (addingChanges_)
                    {
                        this.characterToEffect.charInventory.totalCrushingArmor += Mathf.RoundToInt(mod.amountToChange);
                    }
                    //If we're removing the modifier
                    else
                    {
                        this.characterToEffect.charInventory.totalCrushingArmor -= Mathf.RoundToInt(mod.amountToChange);
                    }
                    break;

                //Arcane resist
                case StatModifier.StatName.ArcaneResist:
                    //If we're adding the modifier
                    if (addingChanges_)
                    {
                        this.characterToEffect.charInventory.totalArcaneResist += Mathf.RoundToInt(mod.amountToChange);
                    }
                    //If we're removing the modifier
                    else
                    {
                        this.characterToEffect.charInventory.totalArcaneResist -= Mathf.RoundToInt(mod.amountToChange);
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

                //Nature resist
                case StatModifier.StatName.NatureResist:
                    //If we're adding the modifier
                    if (addingChanges_)
                    {
                        this.characterToEffect.charInventory.totalNatureResist += Mathf.RoundToInt(mod.amountToChange);
                    }
                    //If we're removing the modifier
                    else
                    {
                        this.characterToEffect.charInventory.totalNatureResist -= Mathf.RoundToInt(mod.amountToChange);
                    }
                    break;

                //Bleed resist
                case StatModifier.StatName.BleedResist:
                    //If we're adding the modifier
                    if(addingChanges_)
                    {
                        this.characterToEffect.charInventory.totalBleedResist += Mathf.RoundToInt(mod.amountToChange);
                    }
                    //If we're removing the modifier
                    else
                    {
                        this.characterToEffect.charInventory.totalBleedResist -= Mathf.RoundToInt(mod.amountToChange);
                    }
                    break;

                //Armor
                case StatModifier.StatName.Armor:
                    //If we're adding the modifier
                    if(addingChanges_)
                    {
                        this.characterToEffect.charInventory.totalPhysicalArmor += Mathf.RoundToInt(mod.amountToChange);
                    }
                    //If we're removing the modifier
                    else
                    {
                        this.characterToEffect.charInventory.totalPhysicalArmor -= Mathf.RoundToInt(mod.amountToChange);
                    }
                    break;
            }
        }

        //If we're adding the effect, we need to create this effect's visual
        if(addingChanges_)
        {
            //Creating the visual effect for this effect
            CharacterSpriteBase targetCharSprite = CombatManager.globalReference.GetCharacterSprite(this.characterToEffect);
            this.SpawnVisualAtLocation(targetCharSprite.transform.localPosition, targetCharSprite.transform);
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

                //If the timer is above the tick time, we reset the timer
                if (this.currentTickTime >= this.timeBetweenTicks)
                {
                    //Resetting the timer
                    this.currentTickTime -= this.timeBetweenTicks;

                    //If this effect doesn't last forever
                    if (!this.unlimitedTicks)
                    {
                        this.ticksLeft -= 1;

                        //If we're out of ticks, we remove this effect
                        if (this.ticksLeft <= 0)
                        {
                            this.RemoveEffect();
                        }
                    }
                }
            }
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
        Accuracy,
        Evasion,

        Unarmed,
        Daggers,
        Swords,
        Mauls,
        Poles,
        Bows,
        Shields,

        ArcaneMagic,
        HolyMagic,
        DarkMagic,
        FireMagic,
        WaterMagic,
        WindMagic,
        ElectricMagic,
        StoneMagic,

        SlashingArmor,
        StabbingArmor,
        CrushingArmor,
        ArcaneResist,
        HolyResist,
        DarkResist,
        FireResist,
        WaterResist,
        WindResist,
        ElectricResist,
        NatureResist,
        BleedResist,

        Armor //Different from Slashing, Stabbing, and Crushing armor. This effects hit chance
    }

    //Enum for which stat this modifier changes
    public StatName modifiedStat = StatName.Health;

    //Amount that the modified stat is changed
    public float amountToChange = 1;
}