using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EvasionBoostPerk : Perk
{
    //The skill that this perk boosts armor against
    public SkillList skillToBlock = SkillList.Unarmed;

    //Bool for if this perk blocks all skill types
    public bool blocksAllSkills = false;

    //The amount of evasion that's added to this perk's owner when attacked
    public int evasionBoost = 0;
}
