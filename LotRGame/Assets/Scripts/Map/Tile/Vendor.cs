using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class used in CityLocation.cs to hold information about item vendors
[System.Serializable]
public class Vendor
{
    //The type of vendor this is
    public VendorTypes type = VendorTypes.GenderalStore;

    //The name of this vendor
    public string vendorName;
    //The name of this building
    public string buildingName;

    [Space(8)]

    //Bools to determine if this vendor will buy certain item types
    public bool willBuyNormalItem = true;
    public bool willBuyFood = true;
    public bool willBuyArmor = true;
    public bool willBuyWeapons = true;

    [Space(8)]

    //The multipliers for how much markdown this vendor buys certain item types
    public float buyNormalItemValueMultiplier = 1f;
    public float buyFoodValueMultiplier = 1f;
    public float buyArmorValueMultiplier = 1f;
    public float buyWeaponValueMultiplier = 1f;

    [Space(8)]

    //The multipliers for how much markup this vendor sells certain item types
    public float normalItemMarkupMultiplier = 1.3f;
    public float foodMarkupMultiplier = 1.3f;
    public float armorMarkupMultiplier = 1.3f;
    public float weaponMarkupMultiplier = 1.3f;

    [Space(8)]

    //The list of items that this vendor can sell
    public List<Item> itemsForSale;

    [Space(8)]

    //The list of quests that are available for the players to accept
    public List<Quest> availableQuests;
}