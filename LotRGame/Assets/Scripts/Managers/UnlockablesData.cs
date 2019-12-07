using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockablesData
{
    /* - Starting Races - */
    private bool human = true;
    private bool dwarf = false;
    private bool elf = false;
    private bool halfman = false;
    private bool orc = false;
    private bool gillfolk = false;
    private bool scaleskin = false;
    private bool amazon = false;
    private bool minotaur = false;
    private bool elemental = false;
    private bool dragon = false;

    /* - Starting Physical Gear - */
    private bool unarmed = false;
    private bool daggers = false;
    private bool sword = false;
    private bool maul = false;
    private bool pole = false;
    private bool bow = false;
    private bool shield = false;
    private bool social = false;

    /* - Starting Magic Gear - */
    private bool holy = false;
    private bool dark = false;
    private bool arcane = false;
    private bool fire = false;
    private bool water = false;
    private bool wind = false;
    private bool electric = false;
    private bool stone = false;

    /* - Starting Combo Gear - */
    private bool paladin = false;
    private bool phoenixCleric = false;
    private bool theif = false;
    private bool ascendedMonk = false;
    private bool frostHammer = false;
    private bool golem = false;
    private bool bellowingChanter = false;
    private bool boltStormer = false;
    private bool dragoon = false;
    private bool magus = false;
    private bool knight = false;
    private bool inquisitor = false;
    private bool unholyBehemoth = false;
    private bool druid = false;
    private bool ranger = false;
    private bool necromancer = false;
    private bool hellspear = false;
    private bool flowingFist = false;
    private bool thunderingRager = false;
    private bool greyMage = false;
    private bool monolith = false;
    private bool tempestBow = false;
    private bool hoplite = false;
    private bool stormcaller = false;



    //Default constructor
    public UnlockablesData()
    {
        //Races
        this.human = true; //Default option
        this.dwarf = false;
        this.elf = false;
        this.halfman = false;
        this.orc = false;
        this.gillfolk = false;
        this.scaleskin = false;
        this.amazon = false;
        this.minotaur = false;
        this.elemental = false;
        this.dragon = false;

        //Physical Gear
        this.unarmed = false;
        this.daggers = false;
        this.sword = true; //Default option
        this.maul = false;
        this.pole = false;
        this.bow = true; //Default option
        this.shield = false;
        this.social = false;

        //Magic Gear
        this.holy = false;
        this.dark = false;
        this.arcane = false;
        this.fire = false;
        this.water = false;
        this.wind = false;
        this.electric = false;
        this.stone = false;

        //Combo Gear
        this.paladin = false;
        this.phoenixCleric = false;
        this.theif = false;
        this.ascendedMonk = false;
        this.frostHammer = false;
        this.golem = false;
        this.bellowingChanter = false;
        this.boltStormer = false;
        this.dragoon = false;
        this.magus = false;
        this.knight = false;
        this.inquisitor = false;
        this.unholyBehemoth = false;
        this.druid = false;
        this.ranger = false;
        this.necromancer = false;
        this.hellspear = false;
        this.flowingFist = false;
        this.thunderingRager = false;
        this.greyMage = false;
        this.monolith = false;
        this.tempestBow = false;
        this.hoplite = false;
        this.stormcaller = false;
    }


    //Function called externally from MainMenu scene to check if a race is unlocked
    public bool IsRaceUnlocked(Races raceToCheck_)
    {
        switch(raceToCheck_)
        {
            case Races.Human:
                return true; //Humans are the default

            case Races.Dwarf:
                return this.dwarf;

            case Races.Elf:
                return this.elf;

            case Races.HalfMan:
                return this.halfman;

            case Races.Orc:
                return this.orc;

            case Races.GillFolk:
                return this.gillfolk;

            case Races.ScaleSkin:
                return this.scaleskin;

            case Races.Amazon:
                return this.amazon;

            case Races.Minotaur:
                return this.minotaur;

            case Races.Elemental:
                return this.elemental;

            case Races.Dragon:
                return this.dragon;
        }

        //If somehow we reach this point, we're returning false by default just in case
        return false;
    }


    //Function called externally from MainMenu scene to check if starting gear is unlocked
    public bool IsGearUnlocked(UnlockableGearType gearToCheck_)
    {
        switch(gearToCheck_)
        {
            case UnlockableGearType.Unarmed:
                return this.unarmed;

            case UnlockableGearType.Daggers:
                return this.daggers;

            case UnlockableGearType.Swords:
                return this.sword;

            case UnlockableGearType.Mauls:
                return this.maul;

            case UnlockableGearType.Poles:
                return this.pole;

            case UnlockableGearType.Bows:
                return this.bow;

            case UnlockableGearType.Shields:
                return this.shield;

            case UnlockableGearType.Social:
                return this.social;

            case UnlockableGearType.HolyMagic:
                return this.holy;

            case UnlockableGearType.DarkMagic:
                return this.dark;

            case UnlockableGearType.ArcaneMagic:
                return this.arcane;

            case UnlockableGearType.FireMagic:
                return this.fire;

            case UnlockableGearType.WaterMagic:
                return this.water;

            case UnlockableGearType.WindMagic:
                return this.wind;

            case UnlockableGearType.ElectricMagic:
                return this.electric;

            case UnlockableGearType.StoneMagic:
                return this.stone;

            case UnlockableGearType.Paladin:
                return this.paladin;

            case UnlockableGearType.PhoenixCleric:
                return this.phoenixCleric;

            case UnlockableGearType.Theif:
                return this.theif;

            case UnlockableGearType.AscendedMonk:
                return this.ascendedMonk;

            case UnlockableGearType.FrostHammer:
                return this.frostHammer;

            case UnlockableGearType.Golem:
                return this.golem;

            case UnlockableGearType.BellowingChanter:
                return this.bellowingChanter;

            case UnlockableGearType.BoltStormer:
                return this.boltStormer;

            case UnlockableGearType.Dragoon:
                return this.dragoon;

            case UnlockableGearType.Magus:
                return this.magus;

            case UnlockableGearType.Knight:
                return this.knight;

            case UnlockableGearType.Inquisitor:
                return this.inquisitor;

            case UnlockableGearType.UnholyBehemoth:
                return this.unholyBehemoth;

            case UnlockableGearType.Druid:
                return this.druid;

            case UnlockableGearType.Ranger:
                return this.ranger;

            case UnlockableGearType.Necromancer:
                return this.necromancer;

            case UnlockableGearType.Hellspear:
                return this.hellspear;

            case UnlockableGearType.FlowingFist:
                return this.flowingFist;

            case UnlockableGearType.ThunderingRager:
                return this.thunderingRager;

            case UnlockableGearType.GreyMage:
                return this.greyMage;

            case UnlockableGearType.Monolith:
                return this.monolith;

            case UnlockableGearType.TempestBow:
                return this.tempestBow;

            case UnlockableGearType.Hoplite:
                return this.hoplite;

            case UnlockableGearType.Stormcaller:
                return this.stormcaller;
        }

        //If somehow we reach this point, we're returning false by default just in case
        return false;
    }
}
