using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Item))]
public class Weapon : MonoBehaviour
{
    //Enum for different types of weapons
    public enum WeaponType {Punching, Sword, Dagger, Axe, Spear, Bow, Improvised};
    //The type of weapon this item is
    public WeaponType type = WeaponType.Improvised;

    //Enum for the number of hands it takes to wield a given weapon
    public enum WeaponSize { OneHand, TwoHands };
    //How many hands it takes to wield this weapon
    public WeaponSize size = WeaponSize.OneHand;


    //The distance in units that this weapon can reach
    public float attackRange = 5;

    //The range of physical damage this weapon can deal
    public Vector2 physicalDamageMinMax = new Vector2(1, 5);

    //The range of magic damage this weapon can deal
    public Vector2 magicDamageMinMax = new Vector2(0, 0);

    //The percent chance that this weapon will crit
    [Range(0, 0.5f)]
    public float critChance = 0.05f;

    //The damage multiplier applied when this weapon crits
    [Range(1, 10)]
    public float critMultiplier = 2;
}
