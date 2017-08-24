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

    //The object that's spawned to display the visual of this effect
    public GameObject visualEffect;



    //Function called externally to trigger this effect
    public virtual void TriggerEffect(Character usingCharacter_, Character targetCharacter_, float timeDelay_ = 0)
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
        //Nothing happens
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


    //Function called internally to display the visual for this effect
    public virtual void SpawnVisualAtLocation(Vector3 posToSpawn_, Transform parentTransform_)
    {
        //Creating a new instance of our visual effect at the given location relative to the parent transform (if it isn't null)
        if (this.visualEffect != null)
        {
            GameObject visual = GameObject.Instantiate(this.visualEffect, posToSpawn_, new Quaternion(), parentTransform_);
        }
    }
}
