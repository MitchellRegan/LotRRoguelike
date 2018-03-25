﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Item))]
[System.Serializable]
public class Weapon : MonoBehaviour
{
    //Enum for different types of weapons
    public enum WeaponType {Unarmed, Sword, Dagger, Maul, Pole, Bow, ArcaneMagic, HolyMagic, DarkMagic, FireMagic, WaterMagic, WindMagic, ElectricMagic, StoneMagic};

    //Enum for the number of hands it takes to wield a given weapon
    public enum WeaponSize { OneHand, TwoHands };
    //How many hands it takes to wield this weapon
    public WeaponSize size = WeaponSize.OneHand;

    //The list of attack actions that this weapon can perform
    public List<AttackAction> attackList;

    [Space(8)]

    //Bool that determines if this weapon is displayed on the outside of the character's hands
    public bool overlapCharacterHand = false;

    //The sprite views for this weapon on the character sprite base
    public SpriteViews weaponSpriteViews;

    //The sprite used mainly for shields to display the reverse side of a weapon
    public Sprite reverseView = null;
}
