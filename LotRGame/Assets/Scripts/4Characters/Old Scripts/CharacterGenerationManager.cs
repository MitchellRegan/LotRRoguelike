using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGenerationManager : MonoBehaviour
{
    public enum CharacterClan
    {
        HillTribe, ReefSailer, Ranger, Mercenary, Imperial,         //Human Clans
        Nomad, Woodland, Tainted, RoyalGuard, Eternal,              //Elven Clans
        Exiled, TreeFeller, QuarryDigger, DarkDelver, GemBlessed,   //Dwarven Clans
        SteelKnuckle, Humble, GrimFang, Grayback, Ashen,            //Orcish Clans
        Burrow, Tinker, Secluded, DuneDweller, Feindish,            //Half-Men Clans
        StreamSwimmer, MarshDweller, Urchin, HardShell, Angler,     //Gill Folk Clans
        Horned, Lightning, Lurker, SawTooth, Boulder,               //Scaleskin Clans
        SkullCrusher, Hawken, LightBlade, Farseer, Gallant          //Amazonian Clans
    };

    public List<CharacterStatRanges> statRanges;
}


[System.Serializable]
public class CharacterStatRanges
{
    /* ~~~~~~ RACE ~~~~~~*/
    public Races race = Races.Human;

    /* ~~~~~~ NAMES ~~~~~~*/
    public List<string> maleNames;
    public List<string> femaleNames;
    public List<string> genderlessNames;
    public List<string> lastNames;
    public string clanName;

    /* ~~~~~~ SEX ~~~~~~*/
    [Range(0, 1)]
    public float maleDistribution = 0.5f;
    [Range(0, 1)]
    public float femaleDistribution = 0.5f;
    [Range(0, 1)]
    public float genderlessDistribution = 0f;

    /* ~~~~~~ PHYSICAL ~~~~~~*/
    public Vector2 maleHeight = new Vector2(150, 190);
    public Vector2 maleWeight = new Vector2(58, 105);
    public Vector2 maleHealth = new Vector2(85, 115);

    public Vector2 femaleHeight = new Vector2(150, 190);
    public Vector2 femaleWeight = new Vector2(58, 105);
    public Vector2 femaleHealth = new Vector2(85, 115);

    public Vector2 genderlessHeight = new Vector2(150, 190);
    public Vector2 genderlessWeight = new Vector2(58, 105);
    public Vector2 genderlessHealth = new Vector2(85, 115);

    public bool requiresFood = true;
    public bool requiresWater = true;
    public bool requiresSleep = true;

    public int daysBeforeStarving = 5;
    public int daysBeforeDehydrated = 3;
    public int daysBeforeFatalInsomnia = 5;


    /* ~~~~~~ SKILLS ~~~~~~*/
    public Vector2 cooking = new Vector2(10, 50);
    public Vector2 healing = new Vector2(10, 50);
    public Vector2 crafting = new Vector2(10, 50);

    public Vector2 foraging = new Vector2(10, 50);
    public Vector2 tracking = new Vector2(10, 50);
    public Vector2 fishing = new Vector2(10, 50);

    public Vector2 climbing = new Vector2(10, 50);
    public Vector2 hiding = new Vector2(10, 50);
    public Vector2 swimming = new Vector2(10, 50);

    public Vector2 punching = new Vector2(10, 50);
    public Vector2 daggers = new Vector2(10, 50);
    public Vector2 swords = new Vector2(10, 50);
    public Vector2 axes = new Vector2(10, 50);
    public Vector2 spears = new Vector2(10, 50);
    public Vector2 bows = new Vector2(10, 50);
    public Vector2 improvised = new Vector2(10, 50);
}