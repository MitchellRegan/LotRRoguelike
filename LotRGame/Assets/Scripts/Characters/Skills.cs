using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skills : MonoBehaviour
{
    //The ability to make food edible
    [Range(1, 100)]
    public int cooking = 30;
    public int cookingMod = 0;

    //The ability to heal damage on allies
    [Range(1, 100)]
    public int healing = 30;
    public int healingMod = 0;

    //The ability to build or fix basic items
    [Range(1, 100)]
    public int crafting = 30;
    public int craftingMod = 0;



    //The ability to search locations for food or items
    [Range(1, 100)]
    public int foraging = 30;
    public int foragingMod = 0;

    //The ability to follow characters or animals
    [Range(1, 100)]
    public int tracking = 30;
    public int trackingMod = 0;

    //The ability to fish for food in bodies of water
    [Range(1, 100)]
    public int fishing = 30;
    public int fishingMod = 0;



    //The ability to climb ropes, ladders, rocky walls, etc
    [Range(1, 100)]
    public int climbing = 30;
    public int climbingMod = 0;

    //The ability to avoid being seen by enemies
    [Range(1, 100)]
    public int hiding = 30;
    public int hidingMod = 0;

    //The ability to not-drown
    [Range(1, 100)]
    public int swimming = 30;
    public int swimmingMod = 0;
}
