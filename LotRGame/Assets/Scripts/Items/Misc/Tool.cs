using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Item))]
public class Tool : MonoBehaviour
{
    //If true, this tool can be used as many times as the player wants
    public bool infiniteUses = false;

    //The number of times this item can be used before it breaks (if infiniteUses is false)
    public int numberOfUses = 1;


    //Bonuses to stats while this item is held
    public int cookingMod = 0;
    public int healingMod = 0;
    public int craftingMod = 0;

    public int foragingMod = 0;
    public int trackingMod = 0;
    public int fishingMod = 0;

    public int climbingMod = 0;
    public int hidingMod = 0;
    public int swimmingMod = 0;

    public int punchingMod = 0;
    public int daggersMod = 0;
    public int swordsMod = 0;
    public int axesMod = 0;
    public int spearsMod = 0;
    public int bowsMod = 0;
    public int improvisedMod = 0;
}
