using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Item))]
[System.Serializable]
public class Weapon : MonoBehaviour
{
    //The type of skill this weapon mainly uses. Only used in checks for WeaponAction.cs
    public SkillList weaponType = SkillList.Daggers;

    //How many hands it takes to wield this weapon
    public WeaponSize size = WeaponSize.OneHand;

    //The base attack damage for this weapon
    public AttackDamage weaponDamage;

    //The list of effects that this weapon can have when attacking
    public List<AttackEffect> weaponEffects;

    //The list of actions that this weapon can perform
    public List<Action> specialActionList;

    [Space(8)]

    //Bool that determines if this weapon is displayed on the outside of the character's hands
    public bool overlapCharacterHand = false;

    //The sprite views for this weapon on the character sprite base
    public SpriteViews weaponSpriteViews;

    //The sprite used mainly for shields to display the reverse side of a weapon
    public Sprite reverseView = null;
}
