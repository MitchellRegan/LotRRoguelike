using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionDifficultyDefinition : MonoBehaviour
{
    /*List of prefabs for different each region that can appear in this band*/
    public List<RegionInfo> regions;
    /*The number of different regions that are spawned for the band at one time*/
    public Vector2 minMaxSplits = new Vector2(1, 2);
}
