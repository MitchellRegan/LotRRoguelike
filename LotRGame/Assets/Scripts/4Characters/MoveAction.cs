using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : Action
{
    //Function inherited from Action.cs
    public override void PerformAction(CombatTile targetTile_)
    {
        Debug.Log("Move Action Called");
        base.PerformAction(targetTile_);
    }
}
