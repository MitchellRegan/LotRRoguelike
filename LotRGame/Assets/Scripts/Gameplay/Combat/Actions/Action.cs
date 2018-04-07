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
    [System.Serializable]
    public enum ActionType { Major, Minor, Fast, Massive };
    public ActionType type = ActionType.Major;

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
        //Nothing here, because regular actions don't do anything.
    }


    //Function that is overrided by inheriting classes and called from PerformAction to determine the amount of EXP given
    public virtual void GrantSkillEXP(Character abilityUser_, SkillList skillUsed_, bool abilityMissed_)
    {
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
