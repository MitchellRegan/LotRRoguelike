using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*A type of item that can be equipped. Parent class for Armor, Rings, Trinkets, and Weapons*/

public class EquippableItem : Item
{
    //Bool for if this item is soulbound (can't be equipped to other characters)
    public bool soulbound = false;
    //Bool for if this item is blessed (better heals, resists corruption)
    public bool blessed = false;
    //Bool for if this item is corrupted (can't be unequipped, chance to turn evil)
    public bool corrupted = false;

    //The list of modifiers that can be on this item
    public List<Modifier> modifierList;
}
