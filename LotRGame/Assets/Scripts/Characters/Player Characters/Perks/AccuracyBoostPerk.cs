using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AccuracyBoostPerk : Perk
{
    //The type of skill that this perk boosts accuracy for
    public SkillList skillAccuracyToBoost = SkillList.Unarmed;

    //Bool for if this perk boosts all skill accuracy
    public bool boostAllSkillAccuracy = false;

    //The base amount of accuracy that's added to this perk's owner when attacking
    public int baseAccuracyBoost = 0;
}
