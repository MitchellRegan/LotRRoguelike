using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Item))]
public class Armor : MonoBehaviour
{
    //Enum for different slots that armor can be worn
    public enum ArmorSlot {Head, Torso, Legs, Hands, Feet, Cloak, Necklace, Ring};
    //The slot that this piece of armor is equipped to
    public ArmorSlot slot = ArmorSlot.Torso;

    //Damage blocked from physical attacks
    public int physicalDefense = 0;

    //Damage blocked from magic attacks
    public int magicDefense = 0;
}
