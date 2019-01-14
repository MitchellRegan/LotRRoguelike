using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDTag : MonoBehaviour
{
    //Enum for what type of object this tag is for so we know what list to look in
    public enum ObjectType
    {
        Quest,
        Perk,
        Character,
        ItemWeapon,
        ItemArmor,
        ItemConsumable,
        ItemQuest,
        ItemMisc,
        Action,
        Location
    };
    public ObjectType objType = ObjectType.ItemMisc;

    //The ID number for this object
    public int numberID = 0;
}
