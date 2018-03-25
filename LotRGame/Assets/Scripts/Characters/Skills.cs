using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skills : MonoBehaviour
{
    //How accurately this character can fight without weapons
    [Range(1, 100)]
    public int unarmed = 30;
    public int unarmedMod = 0;

    //How accurately this character can use daggers in combat
    [Range(1, 100)]
    public int daggers = 30;
    public int daggersMod = 0;

    //How accurately this character can use swords in combat
    [Range(1, 100)]
    public int swords = 30;
    public int swordsMod = 0;

    //How accurately this character can use axes and maces in combat
    [Range(1, 100)]
    public int mauls = 30;
    public int maulsMod = 0;

    //How accurately this character can use pole weapons in combat
    [Range(1, 100)]
    public int poles = 30;
    public int polesMod = 0;

    //How accurately this character can use bows in combat
    [Range(1, 100)]
    public int bows = 30;
    public int bowsMod = 0;


    //How accurately this character can use arcane magic in combat
    [Range(1, 100)]
    public int arcaneMagic = 30;
    public int arcaneMagicMod = 0;

    //How accurately this character can use holy magic spells in combat
    [Range(1, 100)]
    public int holyMagic = 30;
    public int holyMagicMod = 0;

    //How accurately this character can use dark magic spells in combat
    [Range(1, 100)]
    public int darkMagic = 30;
    public int darkMagicMod = 0;

    //How accurately this character can use fire magic in combat
    [Range(1, 100)]
    public int fireMagic = 30;
    public int fireMagicMod = 0;

    //How accurately this character can use water magic in combat
    [Range(1, 100)]
    public int waterMagic = 30;
    public int waterMagicMod = 0;

    //How accurately this character can use wind magic in combat
    [Range(1, 100)]
    public int windMagic = 30;
    public int windMagicMod = 0;

    //How accurately this character can use electric magic in combat
    [Range(1, 100)]
    public int electricMagic = 30;
    public int electricMagicMod = 0;

    //How accurately this character can use stone magic in combat
    [Range(1, 100)]
    public int stoneMagic = 30;
    public int stoneMagicMod = 0;



    //The ability to forage, hunt/track, and fish in the wilderness
    [Range(1, 100)]
    public int survivalist = 30;
    public int survivalistMod = 0;

    //The ability to barter at shops and perform in cities
    [Range(1, 100)]
    public int social = 30;
    public int socialMod = 0;
}

//Enum used to reference each player skill
public enum SkillList
{
    //Combat skills
    Unarmed,
    Daggers,
    Swords,
    Mauls,
    Poles,
    Bows,

    //Magic skills
    ArcaneMagic,
    HolyMagic,
    DarkMagic,
    FireMagic,
    WaterMagic,
    WindMagic,
    ElectricMagic,
    StoneMagic,

    //Non-combat skills
    Survivalist,
    Social
}
