using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ArmorBoostPerk : Perk
{
    //Bool for if this perk only works on crits
    public bool onlyWorksOnCrit = false;

    //The base amount of armor added to this perk's owner to block incoming attacks
    public int armorBoost = 0;
}
