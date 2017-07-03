using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : Action
{
    //Function inherited from Action.cs
    public override void PerformAction(CombatTile targetTile_)
    {
        //Start working on making characters move
        Debug.Log("Move Action Called");
        base.PerformAction(targetTile_);
    }
}
