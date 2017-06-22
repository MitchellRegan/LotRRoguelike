using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Item))]
public class Weapon : MonoBehaviour
{
    //Enum for different types of weapons
    public enum WeaponType {Punching, Sword, Dagger, Axe, Spear, Bow, Improvised, HolyMagic, DarkMagic, NatureMagic};

    //Enum for the number of hands it takes to wield a given weapon
    public enum WeaponSize { OneHand, TwoHands };
    //How many hands it takes to wield this weapon
    public WeaponSize size = WeaponSize.OneHand;

    //The list of attack actions that this weapon can perform
    public List<AttackAction> attackList;
}
