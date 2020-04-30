using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class used in AttackAction.cs to determine what effect can be applied when an attack happens and its chance of happening
[System.Serializable]
public class AttackEffect
{
    //The effect applied when this is triggered
    public Effect effectToApply;

    //If true, this effect only happens if the attack lands. If false, it will happen even if the initial attack misses
    public bool requireHit = true;

    //Bool where if true, this attack effect requires a separate roll to see if it hits
    public bool requireSecondHitRoll = false;

    //The percent chance that the effect on hit will proc
    [Range(0, 1)]
    public float effectChance = 0.2f;

    //The radius of effect from the hit target
    [Range(0, 10)]
    public int effectRadius = 0;

    //Determines if this damages specific enemy types
    public Races effectedRace = Races.None;
    public Subtypes effectedType = Subtypes.None;
    //Determines if the effected type is the only type hit or if it's the only type ignored
    public bool hitEffectedType = true;

    //The type of targets that are effected
    public EffectedTargets effectedTargets = EffectedTargets.Defender;
}