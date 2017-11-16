using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Item))]
[System.Serializable]
public class Armor : MonoBehaviour
{
    //Enum for different slots that armor can be worn
    public enum ArmorSlot {Head, Torso, Legs, Hands, Feet, Cloak, Necklace, Ring, None};
    //The slot that this piece of armor is equipped to
    public ArmorSlot slot = ArmorSlot.Torso;

    //Amount reduced from the hit roll of whoever attacks the character wearing this
    public int physicalDefense = 0;

    //Damage blocked from magic damage
    public int magicResist = 0;

    //Damage blocked from light attacks
    public int lightResist = 0;

    //Damage blocked from dark attacks
    public int darkResist = 0;

    //Damage blocked from fire attacks
    public int fireResist = 0;

    //Damage blocked from water attacks
    public int waterResist = 0;

    //Damage blocked from electric attacks
    public int electricResist = 0;

    //Damage blocked from wind attacks
    public int windResist = 0;

    //Damage blocked from rock attacks
    public int rockResist = 0;
}
