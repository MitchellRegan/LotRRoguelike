﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionInfo : MonoBehaviour
{
    //The name of the region that these tiles will be in
    public string regionName = "";

    //The environment type for this region
    public LandType environmentType = LandType.Empty;

    //The material applied to tiles in this region
    public Material tileMaterial;

    //The height range for tiles in this region
    public Vector2 heightMinMax = new Vector2(0.1f, 0.2f);

    //The movement cost ranges for tiles in this region
    public Vector2 movementCostMinMax = new Vector2(1, 2);


    //The list of resources that can be generated from foraging
    public List<ResourceBlock> foragenResources;

    //The list of resources that can be generated from fishing
    public List<ResourceBlock> fishingResources;

    //The list of enemy groups that can be encountered from tracking
    public List<EncounterBlock> trackingEncounters;
}


//Class used in RegionInfo.cs. Represents a collection of items in a group that have a chance to be spawned together
[System.Serializable]
public class ResourceBlock
{
    //The likelihood that this resource block will be chosen
    [Range(0.01f,1)]
    public float generationChance = 0.1f;

    //The list of items that are in this resource block and the key is the quantity
    public List<Item> resources;

    //The quantity of each resource. The length MUST be the same as resources
    public List<int> resourceQuantities;
}


//Class used in RegionInfo.cs. Represents a collection of enemies in a group that have a chance to be spawned together
[System.Serializable]
public class EncounterBlock
{
    //The likelihood that this encounter block will be chosen
    [Range(0.01f, 1f)]
    public float encounterChance = 0.1f;

    //The list of enemies in this encounter
    public List<GameObject> enemies;
}