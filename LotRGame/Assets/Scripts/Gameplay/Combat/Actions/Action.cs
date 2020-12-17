using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Action : MonoBehaviour
{
    //The name of this action
    public string actionName = "";

    //Description of this action
    public string actionDescription = "";

    //The range of this action in terms of spaces on the combat tile grid
    [Range(0, 12)]
    public int range = 1;

    //Enum for the type of action this is
    public ActionType type = ActionType.Major;

    //The amount of time this action takes to cooldown after use
    public float cooldownTime = 1;

    //The amount of time in seconds that this action takes to be performed
    public float timeToCompleteAction = 3;

    //The amount of skill EXP given when this action is performed sucessfully
    public int skillEXP = 0;

    //The fraction of EXP given on a missed attack
    [Range(0, 1)]
    public static float expPercentOnMiss = 0.25f;


    
    //Function that is overrided by inheriting classes and called from the CombatManager to use this ability
    public virtual void PerformAction(CombatTile targetTile_)
    {
        //Nothing here because the inheriting classes act differently
    }


    //Function that is overrided by inheriting classes to begin this action's cooldown time
    public virtual void BeginActionCooldown()
    {
        //Adding this action to the acting character's recharge list if the cooldown time is above 0
        if (this.cooldownTime > 0)
        {
            //Creating a new action cooldown class for this action
            ActionCooldown acd = new ActionCooldown(this, this.cooldownTime);

            CombatManager.globalReference.initiativeHandler.actingCharacters[0].charActionList.actionCooldowns.Add(acd);
        }
    }


    //Function that is overrided by inheriting classes and called from PerformAction to determine the amount of EXP given
    public virtual void GrantSkillEXP(Character abilityUser_, SkillList skillUsed_, bool abilityMissed_)
    {
        //Making sure the character to give EXP to is a player character. Shouldn't give EXP to enemies
        if(!PartyGroup.group1.charactersInParty.Contains(abilityUser_))
        {
            return;
        }

        //If the ability hits
        if(!abilityMissed_)
        {
            abilityUser_.charSkills.AddSkillEXP(skillUsed_, this.skillEXP);
        }
        //If the ability misses
        else
        {
            abilityUser_.charSkills.AddSkillEXP(skillUsed_, Mathf.RoundToInt(this.skillEXP * expPercentOnMiss));
        }
    }
}
