using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    //The name of this effect
    public string effectName;

    [HideInInspector]
    public Character characterWhoTriggered;

    //The character that will be targeted by this effect
    [HideInInspector]
    public Character characterToEffect;



    //Function called externally to trigger this effect
    public virtual void TriggerEffect(Character usingCharacter_, Character targetCharacter_)
    {
        //Nothing happens
    }


    //Function called externally when a character with this effect's turn begins
    public virtual void EffectOnStartOfTurn()
    {
        //Nothing happens
    }


    //Function called externally when a character with this effect's turn ends
    public virtual void EffectOnEndOfTurn()
    {

    }


    //Function called externally when a character with this effect moves
    public virtual void EffectOnMove()
    {
        //Nothing happens
    }


    //Function called externally when a character with this effect attacks
    public virtual void EffectOnAttack()
    {
        //Nothing happens
    }


    //Function called either externally or internally to remove this effect from the character it's effecting
    public virtual void RemoveEffect()
    {
        //Removing this effect from the target character's list of combat effects
        this.characterToEffect.charCombatStats.combatEffects.Remove(this);
        //Destroying this effect's game object
        Destroy(this.gameObject);
    }
}
