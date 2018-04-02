using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skills : MonoBehaviour
{
    //How accurately this character can fight without weapons
    [Range(1, 100)]
    private int unarmed = 0;
    private int unarmedMod = 0;

    //How accurately this character can use daggers in combat
    [Range(1, 100)]
    private int daggers = 0;
    private int daggersMod = 0;

    //How accurately this character can use swords in combat
    [Range(1, 100)]
    private int swords = 0;
    private int swordsMod = 0;

    //How accurately this character can use axes and maces in combat
    [Range(1, 100)]
    private int mauls = 0;
    private int maulsMod = 0;

    //How accurately this character can use pole weapons in combat
    [Range(1, 100)]
    private int poles = 0;
    private int polesMod = 0;

    //How accurately this character can use bows in combat
    [Range(1, 100)]
    private int bows = 0;
    private int bowsMod = 0;

    //How accurately this character can use shields in combat
    [Range(1, 100)]
    private int shields = 0;
    private int shieldsMod = 0;


    //How accurately this character can use arcane magic in combat
    [Range(1, 100)]
    private int arcaneMagic = 0;
    private int arcaneMagicMod = 0;

    //How accurately this character can use holy magic spells in combat
    [Range(1, 100)]
    private int holyMagic = 0;
    private int holyMagicMod = 0;

    //How accurately this character can use dark magic spells in combat
    [Range(1, 100)]
    private int darkMagic = 0;
    private int darkMagicMod = 0;

    //How accurately this character can use fire magic in combat
    [Range(1, 100)]
    private int fireMagic = 0;
    private int fireMagicMod = 0;

    //How accurately this character can use water magic in combat
    [Range(1, 100)]
    private int waterMagic = 0;
    private int waterMagicMod = 0;

    //How accurately this character can use wind magic in combat
    [Range(1, 100)]
    private int windMagic = 0;
    private int windMagicMod = 0;

    //How accurately this character can use electric magic in combat
    [Range(1, 100)]
    private int electricMagic = 0;
    private int electricMagicMod = 0;

    //How accurately this character can use stone magic in combat
    [Range(1, 100)]
    private int stoneMagic = 0;
    private int stoneMagicMod = 0;

    
    //The ability to forage, hunt/track, and fish in the wilderness
    [Range(1, 100)]
    private int survivalist = 0;
    private int survivalistMod = 0;

    //The ability to barter at shops and perform in cities
    [Range(1, 100)]
    private int social = 0;
    private int socialMod = 0;



    //Function called from CharacterGenerator.cs to set this character's initial skill values within random bounds
    public void GenerateInitialSkillValue(SkillList skillToSet_, float min_, float max_)
    {
        switch (skillToSet_)
        {
            case SkillList.Unarmed:
                this.unarmed = Mathf.RoundToInt(Random.Range(min_, max_));
                break;

            case SkillList.Daggers:
                this.daggers = Mathf.RoundToInt(Random.Range(min_, max_));
                break;

            case SkillList.Swords:
                this.swords = Mathf.RoundToInt(Random.Range(min_, max_));
                break;

            case SkillList.Mauls:
                this.mauls = Mathf.RoundToInt(Random.Range(min_, max_));
                break;

            case SkillList.Poles:
                this.poles = Mathf.RoundToInt(Random.Range(min_, max_));
                break;

            case SkillList.Bows:
                this.bows = Mathf.RoundToInt(Random.Range(min_, max_));
                break;

            case SkillList.Shields:
                this.shields = Mathf.RoundToInt(Random.Range(min_, max_));
                break;


            case SkillList.ArcaneMagic:
                this.arcaneMagic = Mathf.RoundToInt(Random.Range(min_, max_));
                break;

            case SkillList.HolyMagic:
                this.holyMagic = Mathf.RoundToInt(Random.Range(min_, max_));
                break;

            case SkillList.DarkMagic:
                this.darkMagic = Mathf.RoundToInt(Random.Range(min_, max_));
                break;

            case SkillList.FireMagic:
                this.fireMagic = Mathf.RoundToInt(Random.Range(min_, max_));
                break;

            case SkillList.WaterMagic:
                this.waterMagic = Mathf.RoundToInt(Random.Range(min_, max_));
                break;

            case SkillList.WindMagic:
                this.windMagic = Mathf.RoundToInt(Random.Range(min_, max_));
                break;

            case SkillList.ElectricMagic:
                this.electricMagic = Mathf.RoundToInt(Random.Range(min_, max_));
                break;

            case SkillList.StoneMagic:
                this.stoneMagic = Mathf.RoundToInt(Random.Range(min_, max_));
                break;


            case SkillList.Survivalist:
                this.survivalist = Mathf.RoundToInt(Random.Range(min_, max_));
                break;

            case SkillList.Social:
                this.social = Mathf.RoundToInt(Random.Range(min_, max_));
                break;


            default:
                this.social = Mathf.RoundToInt(Random.Range(min_, max_));
                break;
        }

        //Making sure we check for any skill rewards for the leveled up skill
        SkillAbilityManager.globalReference.CheckCharacterSkillForNewAbility(this.GetComponent<Character>(), skillToSet_);
    }


    //Function called from Character.cs to set skill values when loading from a save file
    public void LoadSkillValue(CharacterSaveData dataToLoad_)
    {
        //Loading martial skill values
        this.unarmed = dataToLoad_.unarmed;
        this.daggers = dataToLoad_.daggers;
        this.swords = dataToLoad_.swords;
        this.mauls = dataToLoad_.mauls;
        this.poles = dataToLoad_.poles;
        this.bows = dataToLoad_.bows;
        this.shields = dataToLoad_.shields;

        //Loading magic skill values
        this.arcaneMagic = dataToLoad_.arcaneMagic;
        this.holyMagic = dataToLoad_.holyMagic;
        this.darkMagic = dataToLoad_.darkMagic;
        this.fireMagic = dataToLoad_.fireMagic;
        this.waterMagic = dataToLoad_.waterMagic;
        this.windMagic = dataToLoad_.windMagic;
        this.electricMagic = dataToLoad_.electricMagic;
        this.stoneMagic = dataToLoad_.stoneMagic;

        //Loading non-combat skill values
        this.survivalist = dataToLoad_.survivalist;
        this.social = dataToLoad_.social;
    }


    //Function called externally to get a skill level value (not including modifier)
    public int GetSkillLevelValue(SkillList skillToGet_)
    {
        switch (skillToGet_)
        {
            case SkillList.Unarmed:
                return this.unarmed;

            case SkillList.Daggers:
                return this.daggers;

            case SkillList.Swords:
                return this.swords;

            case SkillList.Mauls:
                return this.mauls;

            case SkillList.Poles:
                return this.poles;

            case SkillList.Bows:
                return this.bows;

            case SkillList.Shields:
                return this.shields;


            case SkillList.ArcaneMagic:
                return this.arcaneMagic;

            case SkillList.HolyMagic:
                return this.holyMagic;

            case SkillList.DarkMagic:
                return this.darkMagic;

            case SkillList.FireMagic:
                return this.fireMagic;

            case SkillList.WaterMagic:
                return this.waterMagic;

            case SkillList.WindMagic:
                return this.windMagic;

            case SkillList.ElectricMagic:
                return this.electricMagic;

            case SkillList.StoneMagic:
                return this.stoneMagic;


            case SkillList.Survivalist:
                return this.survivalist;

            case SkillList.Social:
                return this.social;


            default:
                return this.social;
        }
    }


    //Function called externally to get a skil level value WITH the modifier
    public int GetSkillLevelValueWithMod(SkillList skillToGet_)
    {
        switch(skillToGet_)
        {
            case SkillList.Unarmed:
                return this.unarmed + this.unarmedMod;

            case SkillList.Daggers:
                return this.daggers + this.daggersMod;

            case SkillList.Swords:
                return this.swords + this.swordsMod;

            case SkillList.Mauls:
                return this.mauls + this.maulsMod;

            case SkillList.Poles:
                return this.poles + this.polesMod;

            case SkillList.Bows:
                return this.bows + this.bowsMod;

            case SkillList.Shields:
                return this.shields + this.shieldsMod;


            case SkillList.ArcaneMagic:
                return this.arcaneMagic + this.arcaneMagicMod;

            case SkillList.HolyMagic:
                return this.holyMagic + this.holyMagicMod;

            case SkillList.DarkMagic:
                return this.darkMagic + this.darkMagicMod;

            case SkillList.FireMagic:
                return this.fireMagic + this.fireMagicMod;

            case SkillList.WaterMagic:
                return this.waterMagic + this.waterMagicMod;

            case SkillList.WindMagic:
                return this.windMagic + this.windMagicMod;

            case SkillList.ElectricMagic:
                return this.electricMagic + this.electricMagicMod;

            case SkillList.StoneMagic:
                return this.stoneMagic + this.stoneMagicMod;


            case SkillList.Survivalist:
                return this.survivalist + this.survivalistMod;

            case SkillList.Social:
                return this.social + this.socialMod;


            default:
                return this.social + socialMod;
        }
    }


    //Function called externally from PartyCreator.cs and SkillTome.cs to add levels to this character
    public void LevelUpSkill(SkillList skillToLevel_, int levelsToAdd_)
    {
        switch (skillToLevel_)
        {
            case SkillList.Unarmed:
                this.unarmed += levelsToAdd_;
                if (this.unarmed > 100)
                {
                    this.unarmed = 100;
                }
                break;

            case SkillList.Daggers:
                this.daggers += levelsToAdd_;
                if (this.daggers > 100)
                {
                    this.daggers = 100;
                }
                break;

            case SkillList.Swords:
                this.swords += levelsToAdd_;
                if (this.swords > 100)
                {
                    this.swords = 100;
                }
                break;

            case SkillList.Mauls:
                this.mauls += levelsToAdd_;
                if (this.mauls > 100)
                {
                    this.mauls = 100;
                }
                break;

            case SkillList.Poles:
                this.poles += levelsToAdd_;
                if (this.poles > 100)
                {
                    this.poles = 100;
                }
                break;

            case SkillList.Bows:
                this.bows += levelsToAdd_;
                if (this.bows > 100)
                {
                    this.bows = 100;
                }
                break;

            case SkillList.Shields:
                this.shields += levelsToAdd_;
                if (this.shields > 100)
                {
                    this.shields = 100;
                }
                break;


            case SkillList.ArcaneMagic:
                this.arcaneMagic += levelsToAdd_;
                if (this.arcaneMagic > 100)
                {
                    this.arcaneMagic = 100;
                }
                break;

            case SkillList.HolyMagic:
                this.holyMagic += levelsToAdd_;
                if (this.holyMagic > 100)
                {
                    this.holyMagic = 100;
                }
                break;

            case SkillList.DarkMagic:
                this.darkMagic += levelsToAdd_;
                if (this.darkMagic > 100)
                {
                    this.darkMagic = 100;
                }
                break;

            case SkillList.FireMagic:
                this.fireMagic += levelsToAdd_;
                if (this.fireMagic > 100)
                {
                    this.fireMagic = 100;
                }
                break;

            case SkillList.WaterMagic:
                this.waterMagic += levelsToAdd_;
                if (this.waterMagic > 100)
                {
                    this.waterMagic = 100;
                }
                break;

            case SkillList.WindMagic:
                this.windMagic += levelsToAdd_;
                if (this.windMagic > 100)
                {
                    this.windMagic = 100;
                }
                break;

            case SkillList.ElectricMagic:
                this.electricMagic += levelsToAdd_;
                if (this.electricMagic > 100)
                {
                    this.electricMagic = 100;
                }
                break;

            case SkillList.StoneMagic:
                this.stoneMagic += levelsToAdd_;
                if (this.stoneMagic > 100)
                {
                    this.stoneMagic = 100;
                }
                break;


            case SkillList.Survivalist:
                this.survivalist += levelsToAdd_;
                if (this.survivalist > 100)
                {
                    this.survivalist = 100;
                }
                break;

            case SkillList.Social:
                this.social += levelsToAdd_;
                if (this.social > 100)
                {
                    this.social = 100;
                }
                break;


            default:
                this.social += levelsToAdd_;
                if(this.social > 100)
                {
                    this.social = 100;
                }
                break;
        }

        //Making sure we check for any skill rewards for the leveled up skill
        SkillAbilityManager.globalReference.CheckCharacterSkillForNewAbility(this.GetComponent<Character>(), skillToLevel_);
    }


    //Function called from ModifyStatsEffect.cs to change a skill modifier
    public void ChangeSkillModifier(SkillList skillModifier_, int amountToChange_)
    {
        switch (skillModifier_)
        {
            case SkillList.Unarmed:
                this.unarmedMod += amountToChange_;
                break;

            case SkillList.Daggers:
                this.daggersMod += amountToChange_;
                break;

            case SkillList.Swords:
                this.swordsMod += amountToChange_;
                break;

            case SkillList.Mauls:
                this.maulsMod += amountToChange_;
                break;

            case SkillList.Poles:
                this.polesMod += amountToChange_;
                break;

            case SkillList.Bows:
                this.bowsMod += amountToChange_;
                break;

            case SkillList.Shields:
                this.shieldsMod += amountToChange_;
                break;


            case SkillList.ArcaneMagic:
                this.arcaneMagicMod += amountToChange_;
                break;

            case SkillList.HolyMagic:
                this.holyMagicMod += amountToChange_;
                break;

            case SkillList.DarkMagic:
                this.darkMagicMod += amountToChange_;
                break;

            case SkillList.FireMagic:
                this.fireMagicMod += amountToChange_;
                break;

            case SkillList.WaterMagic:
                this.waterMagicMod += amountToChange_;
                break;

            case SkillList.WindMagic:
                this.windMagicMod += amountToChange_;
                break;

            case SkillList.ElectricMagic:
                this.electricMagicMod += amountToChange_;
                break;

            case SkillList.StoneMagic:
                this.stoneMagicMod += amountToChange_;
                break;


            case SkillList.Survivalist:
                this.survivalistMod += amountToChange_;
                break;

            case SkillList.Social:
                this.socialMod += amountToChange_;
                break;


            default:
                this.socialMod += amountToChange_;
                break;
        }
    }
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
    Shields,

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
