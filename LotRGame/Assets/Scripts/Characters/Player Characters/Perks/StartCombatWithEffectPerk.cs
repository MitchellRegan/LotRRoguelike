using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StartCombatWithEffectPerk : Perk
{
    //The effect to apply to this character
    public Effect effectToStartWith;

    //Bool for if this perk is applied when ambushed
    public bool appliedWhenAmbushed = false;

    //The percent chance that this perk is applied (default is 100%)
    [Range(0.01f, 1)]
    public float chanceToApplyEffect = 1f;
}
