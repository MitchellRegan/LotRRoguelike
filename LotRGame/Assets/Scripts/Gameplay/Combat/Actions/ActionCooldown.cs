using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class used in ActionList.cs to track each type of ability this character has used and remaining cooldown time
public class ActionCooldown
{
    //The action that's cooling down
    public Action actionCoolingDown;
    //The remaining time left on the cooldown
    public float remainingCooldownTime = 0;


    //Constructor function for this class
    public ActionCooldown(Action actionCoolingDown_, float cooldownTime_)
    {
        this.actionCoolingDown = actionCoolingDown_;
        this.remainingCooldownTime = cooldownTime_;
    }
}