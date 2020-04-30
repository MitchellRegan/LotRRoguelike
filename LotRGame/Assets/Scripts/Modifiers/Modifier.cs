using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Modifiers are effects for characters that are applied from equipped armor/weapons and abilities*/

public class Modifier : MonoBehaviour
{
    public string modName = "";
    public string modDescription = "";

    //The amount that this modifier changes the default value of equippable items (multiplier)
    public int priceModifier = 1;

    //Effect applied to the owner of this modifier when they attack
    public Effect AttackEffectSelf = null;
    //Effect applied to the target when the owner of this modifier attacks
    public Effect AttackEffectTarget = null;

    //Effect applied to the owner of this modifier when they are the target of an attack
    public Effect DefendEffectSelf = null;
    //Effect applied to the attacker when the owner of this modifier is the target of an attack
    public Effect DefendEffectAttacker = null;
}
