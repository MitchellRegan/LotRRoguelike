using System.Collections;
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

    //The list of objects that can be used to decorate this tile
    public List<GameObject> tileDecorations;


    //The list of resources that can be generated from foraging
    public List<ResourceBlock> foragingResources;

    //The list of resources that can be generated from fishing
    public List<ResourceBlock> fishingResources;

    //The list of enemy groups that can be encountered from tracking
    public List<EncounterBlock> trackingEncounters;

    [Space(8)]

    //The list of enemy encounters that can happen on this tile
    public List<EncounterBlock> randomEncounterList;

    //The chance that an encounter will happen when time passes
    [Range(0, 1)]
    public float randomEncounterChance = 0.1f;

    [Space(8)]

    //The main city for this region
    public CityLocation regionCity;
    //The dungeon for this region
    public DungeonLocation regionDungeon;
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
    public EnemyEncounter encounterEnemies;
}