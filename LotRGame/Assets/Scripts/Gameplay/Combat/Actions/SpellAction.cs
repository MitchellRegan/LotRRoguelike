using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpellAction : AttackAction
{
    //Int for the number of times this spell can be used before it needs to be recharged
    public int spellCharges = 1;

    //The number of hours that need to pass before this spell is available after it's used
    public int hoursUntilRecharged = 6;



    //Function inherited from AttackAction.cs and called from CombatManager.cs so we can attack a target
    public override void PerformAction(CombatTile targetTile_)
    {
        base.PerformAction(targetTile_);

        //Once the attack is performed, this spell needs to recharge
        Character actingChar = CombatManager.globalReference.actingCharacters[0];
        actingChar.charActionList.StartSpellRecharge(this);
    }
}
