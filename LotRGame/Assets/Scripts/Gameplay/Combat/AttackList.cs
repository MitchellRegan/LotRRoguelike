using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackList : MonoBehaviour
{
    //The list of attacks that this character or weapon can perform
    public List<AttackInfo> attacks;
}


//A class used in AttackList.cs and CombatManager.cs that describes different attacks
[System.Serializable]
public class AttackInfo
{
    //The displayed name of this attack
    public string attackName = "";

    //The type of action this attack requires
    public enum AttackActionType { Standard, FullRound, Quick, Free };
    public AttackActionType actionType = AttackActionType.Standard;

    //The range of this attack in terms of spaces
    public int attackRange = 1;

    //The skill used for determining accuracy
    public Weapon.WeaponType weaponSkillUsed = Weapon.WeaponType.Punching;

    //The skill check to beat when using this attack
    [Range(10, 100)]
    public int hitDC = 50;

    //The percent chance that this attack will crit
    [Range(0, 90)]
    public float critChance = 20;

    //The damage multiplier applied when this weapon crits
    public float critMultiplier = 2;
    [Space(10)]


    //The range of physical damage this attack can deal if it hits
    public Vector2 physicalDamageRange = new Vector2(2, 12);
    public EaseType physicalDamageDistribution = EaseType.Linear;
    [Space(10)]

    //The range of fire damage this attack can deal if it hits
    public Vector2 fireDamageRange = new Vector2(0, 0);
    public EaseType fireDamageDistribution = EaseType.Linear;
    [Space(10)]

    //The range of water damage this attack can deal if it hits
    public Vector2 waterDamageRange = new Vector2(0, 0);
    public EaseType waterDamageDistribution = EaseType.Linear;
    [Space(10)]

    //The range of electric damage this attack can deal if it hits
    public Vector2 electricDamageRange = new Vector2(0, 0);
    public EaseType electricDamageDistribution = EaseType.Linear;
    [Space(10)]

    //The range of wind damage this attack can deal if it hits
    public Vector2 windDamageRange = new Vector2(0, 0);
    public EaseType windDamageDistribution = EaseType.Linear;
    [Space(10)]

    //The range of rock damage this attack can deal if it hits
    public Vector2 rockDamageRange = new Vector2(0, 0);
    public EaseType rockDamageDistribution = EaseType.Linear;
    [Space(10)]

    //The range of light damage this attack can deal if it hits
    public Vector2 lightDamageRange = new Vector2(0, 0);
    public EaseType lightDamageDistribution = EaseType.Linear;
    [Space(10)]

    //The range of dark damage this attack can deal if it hits
    public Vector2 darkDamageRange = new Vector2(0, 0);
    public EaseType darkDamageDistribution = EaseType.Linear;
}