using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : Action
{
    //The skill used for determining accuracy
    public Weapon.WeaponType weaponSkillUsed = Weapon.WeaponType.Punching;

    //Enum that determines how enemy evasion and armor affects the chance of this attack hitting
    public enum attackTouchType { Regular, IgnoreEvasion, IgnoreArmor, IgnoreEvasionAndArmor};
    public attackTouchType touchType = attackTouchType.Regular;

    //The range of this attack interms of spaces
    [Range(1, 12)]
    public int attackRange = 1;

    //The percent chance that this attack will crit
    [Range(0, 1)]
    public float critChance = 0.2f;

    //The damage multiplier applied when this weapon crits
    public float critMultiplier = 2;

    //The list of damage dice that are rolled when this attack deals damage
    public List<AttackDamage> damageDealt;

    //The list of effects that can proc when this attack hits
    public List<AttackEffect> effectsOnHit;
}

//Class used in AttackAction.cs to determine damage dealt when an attack hits
[System.Serializable]
public class AttackDamage
{
    //The type of damage that's inflicted
    public enum DamageType { Physical, Magic, Fire, Water, Electric, Wind, Rock, Light, Dark };
    public DamageType type = DamageType.Physical;

    //The amount of damage inflicted before dice rolls
    public int baseDamage = 0;

    //The number of dice that are rolled
    public int diceRolled = 1;
    //The highest value of the type of die rolled
    public int diceSides = 6;
}

//Class used in AttackAction.cs to determine what effect can be applied when an attack happens and its chance of happening
[System.Serializable]
public class AttackEffect
{
    //The effect applied when this attack hits
    public Effect effectOnHit;

    //The percent chance that the effect on hit will proc
    [Range(0, 100)]
    public float effectChance = 0.2f;
}