using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class used in EnemyCombatAI_Basic to hold an enemy's action and the tile that they use it on
public class EnemyActionAndTile
{
    //The action that the enemy will use
    public Action enemyActionToUse;
    //The tile that the action will be used on
    public CombatTile3D targetTile;

    //Constructor function for this class
    public EnemyActionAndTile(Action enemyAct_, CombatTile3D targetTile_)
    {
        this.enemyActionToUse = enemyAct_;
        this.targetTile = targetTile_;
    }
}