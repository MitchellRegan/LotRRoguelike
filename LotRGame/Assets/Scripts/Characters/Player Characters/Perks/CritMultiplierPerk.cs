using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritMultiplierPerk : Perk
{
    //The weapon skill that this perk boosts crit damage for
    public SkillList skillCritToBoost = SkillList.Unarmed;
    //Bool for if this perk boosts all skill crit damage
    public bool boostAllSkills = false;

    //The type of weapon size this perk effects
    public WeaponHand weaponSizeRequirement = WeaponHand.OneHand;
    //Bool for if this perk boosts all weapon sizes
    public bool noSizeRequirement = true;

    //The amount that this increases crit multiplier by
    [Range(0.01f, 5)]
    public float critMultiplierBoost = 1f;
}
