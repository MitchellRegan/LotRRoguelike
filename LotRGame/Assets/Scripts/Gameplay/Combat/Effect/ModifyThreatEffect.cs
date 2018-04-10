using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ModifyThreatEffect : Effect
{
    //The amount of threat that's passed to the target enemy when triggered
    public int initialThreatAdded = 10;
    //The amount of threat that's passed to the target enemy every tick
    public int threatAddedPerTick = 2;
    //If true, this threat is applied to all enemies
    public bool threatenAllEnemies = false;
    //If true, the inverse of the threat added will be reapplied when this effect ends
    public bool reapplyThreatWhenRemoved = false;

    [Space(8)]

    //The range for the number of turns this effect activates before it goes away
    public Vector2 numberofTicksRange = new Vector2(5, 5);
    //The number of remaining ticks left
    [HideInInspector]
    public int ticksLeft = 1;

    [Space(8)]

    //If true, this effect ticks as soon as the effected character's turn begins
    public bool tickOnStartOfTurn = false;
    //If true, this effect ticks as soon as the effected character's turn ends
    public bool tickOnEndOfTurn = true;



    //Function inherited from Effect.cs to trigger this effect
    public override void TriggerEffect(Character usingCharacter_, Character targetCharacter_, float timeDelay_ = 0)
    {
        //If this effect threatens all enemies
        if(this.threatenAllEnemies)
        {
            CombatManager.globalReference.ApplyActionThreat(usingCharacter_, null, this.initialThreatAdded, true);
        }
        //If we only threaten the hit character
        else
        {
            //Making sure the target character has the EnemyCombatAI component
            if(targetCharacter_.GetComponent<EnemyCombatAI_Basic>())
            {
                targetCharacter_.GetComponent<EnemyCombatAI_Basic>().IncreaseThreat(usingCharacter_, this.initialThreatAdded);
            }
            //If this character doesn't have the EnemyCombatAI component, this effect is useless and is destroyed
            else
            {
                this.RemoveEffect();
            }
        }

        //If the max number of ticks this effect can have is 0, we'll immediately destroy this
        if (this.numberofTicksRange.y <= 0)
        {
            this.RemoveEffect();
            return;
        }
        //If this effect lasts for a while, we find out how long
        else
        {
            this.ticksLeft = Mathf.RoundToInt(Random.Range(this.numberofTicksRange.x, this.numberofTicksRange.y));
            this.characterToEffect = targetCharacter_;
            this.characterWhoTriggered = usingCharacter_;
        }
    }


    //Function called every time this effect ticks
    private void ThreatenCharacter()
    {
        //If this threatens all enemies
        if (this.threatenAllEnemies)
        {
            CombatManager.globalReference.ApplyActionThreat(this.characterWhoTriggered, null, this.threatAddedPerTick, true);

            //If we have a visual for this effect, we need to spawn it on all of the enemies
            if(this.visualEffect != null)
            {
                //Looping through every enemy in this encounter
                foreach(Character enemy in CombatManager.globalReference.enemyCharactersInCombat)
                {
                    //Creating the visual effect for this effect
                    CharacterSpriteBase targetCharSprite = CombatManager.globalReference.GetCharacterSprite(enemy);
                    this.SpawnVisualAtLocation(targetCharSprite.transform.localPosition, targetCharSprite.transform);
                }
            }
        }
        //If this threatens only the target
        else
        {
            this.characterToEffect.GetComponent<EnemyCombatAI_Basic>().IncreaseThreat(this.characterWhoTriggered, this.threatAddedPerTick);

            //If we have a visual for this effect, we need to spawn it on the target enemy
            if (this.visualEffect != null)
            {
                //Creating the visual effect for this effect
                CharacterSpriteBase targetCharSprite = CombatManager.globalReference.GetCharacterSprite(this.characterToEffect);
                this.SpawnVisualAtLocation(targetCharSprite.transform.localPosition, targetCharSprite.transform);
            }
        }

        //Counting down the ticks remaining
        this.ticksLeft -= 1;

        //If the ticks left hits 0, the effect is over
        if(this.ticksLeft <= 0)
        {
            //If we re-apply the threat, we send the inverse of the initial threat
            if(this.reapplyThreatWhenRemoved)
            {
                //If this threatens all enemies
                if (this.threatenAllEnemies)
                {
                    CombatManager.globalReference.ApplyActionThreat(this.characterWhoTriggered, null, this.initialThreatAdded * -1, true);
                }
                //If this threatens only the target
                else
                {
                    this.characterToEffect.GetComponent<EnemyCombatAI_Basic>().IncreaseThreat(this.characterWhoTriggered, this.initialThreatAdded * -1);
                }
            }

            //removing this effect
            this.RemoveEffect();
        }
    }


    //Function inherited from Effect.cs at the end of the effected character's turn
    public override void EffectOnEndOfTurn()
    {
        //If this effect ticks on the end of turn
        if(this.tickOnEndOfTurn)
        {
            this.ThreatenCharacter();
        }
    }


    //Function inherited from Effect.cs at the start of the effected character's turn
    public override void EffectOnStartOfTurn()
    {
        //If this effect ticks on the start of turn
        if(this.tickOnStartOfTurn)
        {
            this.ThreatenCharacter();
        }
    }


    //Function inherited from Effect.cs when this effect is removed from the targeted character
    public override void RemoveEffect()
    {
        base.RemoveEffect();
    }
}
