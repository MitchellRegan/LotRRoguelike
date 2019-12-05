using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class used in LevelUpManager.cs to track and change the time it takes for characters to level up
[System.Serializable]
public class LevelUpCurve
{
    //The name of this curve
    public string name = "";
    //The number of days between each level
    public float daysToLevel = 1;
    //The max level this curve applies to
    [Range(1, 100)]
    public int maxLevelForCurve = 1;
}