using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Component for PlayerCharacter.cs and NPC.cs to reference all equipped items*/

public class Equipment : MonoBehaviour
{
    //References to the equipped weapons
    public Weapon mainWeapon = null;
    public Weapon secondaryWeapon = null;

    //Reference to the different types of equipped armor
    public Armor armor = null;
    public Ring ring = null;
    public Trinket trinket = null;
}
