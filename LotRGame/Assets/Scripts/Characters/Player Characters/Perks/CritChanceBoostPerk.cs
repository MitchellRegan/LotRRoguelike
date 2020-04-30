using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritChanceBoostPerk : Perk
{
    //The weapon skill that this perk boosts crit chance for
    public SkillList skillCritToBoost = SkillList.Unarmed;
    //Bool for if this perk boosts all skill crit chance
    public bool boostAllSkills = false;

    //The type of weapon size this perk effects
    public WeaponHand weaponSizeRequirement = WeaponHand.OneHand;
    //Bool for if this perk boosts all weapon sizes
    public bool noSizeRequirement = true;

    //The amount that this increases crit chance by
    [Range(0.01f, 0.2f)]
    public float critChanceBoost = 0.1f;
}
