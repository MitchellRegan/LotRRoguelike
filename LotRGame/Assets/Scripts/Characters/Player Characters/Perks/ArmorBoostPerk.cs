using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ArmorBoostPerk : Perk
{
    //The skill that this perk boosts armor against
    public SkillList skillToBlock = SkillList.Unarmed;

    //Bool for if this perk blocks all skill types
    public bool blocksAllSkills = false;

    //The base amount of armor added to this perk's owner to block incoming attacks
    public int armorBoost = 0;
}
