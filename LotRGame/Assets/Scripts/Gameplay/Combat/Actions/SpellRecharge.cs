using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class used in ActionList.cs to handle recharging spells
[System.Serializable]
public class SpellRecharge
{
    //The reference to the spell that's recharging
    public SpellAction spellThatsCharging;
    //The amount of time remaining for this spell to finish recharging
    public int hoursRemaining = 0;

    //Constructor for this class
    public SpellRecharge(SpellAction spell_, int hours_)
    {
        this.spellThatsCharging = spell_;
        this.hoursRemaining = hours_;
    }
}