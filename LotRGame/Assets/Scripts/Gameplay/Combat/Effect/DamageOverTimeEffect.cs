using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTimeEffect : Effect
{
    //What type of damage this damage counts as
    public CombatManager.DamageType damageType = CombatManager.DamageType.Magic;

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



    //Function inherited from Effect.cs to trigger this effect. Sets the target as the damaged character
    public override void TriggerEffect(Character usingCharacter_, Character targetCharacter_, float timeDelay_ = 0)
    {
        //Checking the targeted character to make sure this effect isn't already applied to them
        foreach(Effect e in targetCharacter_.charCombatStats.combatEffects)
        {
            //If we find a version of this effect already on the target
            if(e.effectName == this.effectName)
            {
                //We refresh the duration of the effect on the target to the max number of ticks
                e.GetComponent<DamageOverTimeEffect>().ticksLeft = Mathf.RoundToInt(this.numberOfTicksRange.y);
                //And then we destroy this effect's game object
                Destroy(this.gameObject);
            }
        }

        this.characterToEffect = targetCharacter_;
        this.characterWhoTriggered = usingCharacter_;

        //Adding this effect to the targeted character's combat effects list
        this.characterToEffect.charCombatStats.combatEffects.Add(this);

        //Determining how many ticks this effect will have (if it's not unlimited that is)
        if(!this.unlimitedTicks)
        {
            this.ticksLeft = Mathf.RoundToInt(Random.Range(this.numberOfTicksRange.x, this.numberOfTicksRange.y));
        }
    }


    //Function called whenever this effect deals damage
    private void DamageCharacter()
    {
        //Making sure the character isn't dead before dealing damage
        if (this.characterToEffect.charPhysState.currentHealth <= 0)
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

        //Subtracting any magic resistance from the damage that we're trying to deal
        switch(this.damageType)
        {
            case CombatManager.DamageType.Magic:
                damageDealt -= this.characterToEffect.charInventory.totalMagicResist;
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
            case CombatManager.DamageType.Rock:
                damageDealt -= this.characterToEffect.charInventory.totalRockResist;
                break;
            case CombatManager.DamageType.Holy:
                damageDealt -= this.characterToEffect.charInventory.totalHolyResist;
                break;
            case CombatManager.DamageType.Dark:
                damageDealt -= this.characterToEffect.charInventory.totalDarkResist;
                break;
        }

        //Dealing the damage to the effected character
        this.characterToEffect.charPhysState.DamageCharacter(damageDealt);

        //If this character has the EnemyCombatAI component, we increase the threat for the character who put this effect on
        if(this.characterToEffect.GetComponent<EnemyCombatAI_Basic>())
        {
            this.characterToEffect.GetComponent<EnemyCombatAI_Basic>().IncreaseThreat(this.characterWhoTriggered, damageDealt);
        }

        //Telling the combat manager to display the damage dealt
        CombatTile damagedCharTile = CombatManager.globalReference.combatTileGrid[this.characterToEffect.charCombatStats.gridPositionCol][this.characterToEffect.charCombatStats.gridPositionRow];
        CombatManager.globalReference.DisplayDamageDealt(0, damageDealt, this.damageType, damagedCharTile, didThisCrit);

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
