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
    //The current amount of EXP the unarmed skill has
    private int unarmedEXP = 0;

    //How accurately this character can use daggers in combat
    [Range(1, 100)]
    private int daggers = 0;
    private int daggersMod = 0;
    //The current amount of EXP the daggers skill has
    private int daggersEXP = 0;

    //How accurately this character can use swords in combat
    [Range(1, 100)]
    private int swords = 0;
    private int swordsMod = 0;
    //The current amount of EXP the swords skill has
    private int swordsEXP = 0;

    //How accurately this character can use axes and maces in combat
    [Range(1, 100)]
    private int mauls = 0;
    private int maulsMod = 0;
    //The current amount of EXP the mauls skill has
    private int maulsEXP = 0;

    //How accurately this character can use pole weapons in combat
    [Range(1, 100)]
    private int poles = 0;
    private int polesMod = 0;
    //The current amount of EXP the poles skill has
    private int polesEXP = 0;

    //How accurately this character can use bows in combat
    [Range(1, 100)]
    private int bows = 0;
    private int bowsMod = 0;
    //The current amount of EXP the bows skill has
    private int bowsEXP = 0;

    //How accurately this character can use shields in combat
    [Range(1, 100)]
    private int shields = 0;
    private int shieldsMod = 0;
    //The current amount of EXP the shield skill has
    private int shieldsEXP = 0;


    //How accurately this character can use arcane magic in combat
    [Range(1, 100)]
    private int arcaneMagic = 0;
    private int arcaneMagicMod = 0;
    //The current amount of EXP the arcane skill has
    private int arcaneMagicEXP = 0;

    //How accurately this character can use holy magic spells in combat
    [Range(1, 100)]
    private int holyMagic = 0;
    private int holyMagicMod = 0;
    //The current amount of EXP the holy skill has
    private int holyMagicEXP = 0;

    //How accurately this character can use dark magic spells in combat
    [Range(1, 100)]
    private int darkMagic = 0;
    private int darkMagicMod = 0;
    //The current amount of EXP the dark skill has
    private int darkMagicEXP = 0;

    //How accurately this character can use fire magic in combat
    [Range(1, 100)]
    private int fireMagic = 0;
    private int fireMagicMod = 0;
    //The current amount of EXP the fire skill has
    private int fireMagicEXP = 0;

    //How accurately this character can use water magic in combat
    [Range(1, 100)]
    private int waterMagic = 0;
    private int waterMagicMod = 0;
    //The current amount of EXP the water skill has
    private int waterMagicEXP = 0;

    //How accurately this character can use wind magic in combat
    [Range(1, 100)]
    private int windMagic = 0;
    private int windMagicMod = 0;
    //The current amount of EXP the wind skill has
    private int windMagicEXP = 0;

    //How accurately this character can use electric magic in combat
    [Range(1, 100)]
    private int electricMagic = 0;
    private int electricMagicMod = 0;
    //The current amount of EXP the electric skill has
    private int electricMagicEXP = 0;

    //How accurately this character can use stone magic in combat
    [Range(1, 100)]
    private int stoneMagic = 0;
    private int stoneMagicMod = 0;
    //The current amount of EXP the stone skill has
    private int stoneMagicEXP = 0;


    //The ability to forage, hunt/track, and fish in the wilderness
    [Range(1, 100)]
    private int survivalist = 0;
    private int survivalistMod = 0;
    //The current amount of EXP the survivalist skill has
    private int survivalistEXP = 0;

    //The ability to barter at shops and perform in cities
    [Range(1, 100)]
    private int social = 0;
    private int socialMod = 0;
    //The current amount of EXP the social skill has
    private int socialEXP = 0;



    //Function called from CharacterGenerator.cs to set this character's initial skill values within random bounds
    public void GenerateInitialSkillValue(SkillList skillToSet_, float min_, float max_)
    {
        //Int to hold the level that the skill is generated at
        int skillLevel = 0;

        switch (skillToSet_)
        {
            case SkillList.Unarmed:
                this.unarmed = Mathf.RoundToInt(Random.Range(min_, max_));
                skillLevel = this.unarmed;
                break;

            case SkillList.Daggers:
                this.daggers = Mathf.RoundToInt(Random.Range(min_, max_));
                skillLevel = this.daggers;
                break;

            case SkillList.Swords:
                this.swords = Mathf.RoundToInt(Random.Range(min_, max_));
                skillLevel = this.swords;
                break;

            case SkillList.Mauls:
                this.mauls = Mathf.RoundToInt(Random.Range(min_, max_));
                skillLevel = this.mauls;
                break;

            case SkillList.Poles:
                this.poles = Mathf.RoundToInt(Random.Range(min_, max_));
                skillLevel = this.poles;
                break;

            case SkillList.Bows:
                this.bows = Mathf.RoundToInt(Random.Range(min_, max_));
                skillLevel = this.bows;
                break;

            case SkillList.Shields:
                this.shields = Mathf.RoundToInt(Random.Range(min_, max_));
                skillLevel = this.shields;
                break;


            case SkillList.ArcaneMagic:
                this.arcaneMagic = Mathf.RoundToInt(Random.Range(min_, max_));
                skillLevel = this.arcaneMagic;
                break;

            case SkillList.HolyMagic:
                this.holyMagic = Mathf.RoundToInt(Random.Range(min_, max_));
                skillLevel = this.holyMagic;
                break;

            case SkillList.DarkMagic:
                this.darkMagic = Mathf.RoundToInt(Random.Range(min_, max_));
                skillLevel = this.darkMagic;
                break;

            case SkillList.FireMagic:
                this.fireMagic = Mathf.RoundToInt(Random.Range(min_, max_));
                skillLevel = this.fireMagic;
                break;

            case SkillList.WaterMagic:
                this.waterMagic = Mathf.RoundToInt(Random.Range(min_, max_));
                skillLevel = this.waterMagic;
                break;

            case SkillList.WindMagic:
                this.windMagic = Mathf.RoundToInt(Random.Range(min_, max_));
                skillLevel = this.windMagic;
                break;

            case SkillList.ElectricMagic:
                this.electricMagic = Mathf.RoundToInt(Random.Range(min_, max_));
                skillLevel = this.electricMagic;
                break;

            case SkillList.StoneMagic:
                this.stoneMagic = Mathf.RoundToInt(Random.Range(min_, max_));
                skillLevel = this.stoneMagic;
                break;


            case SkillList.Survivalist:
                this.survivalist = Mathf.RoundToInt(Random.Range(min_, max_));
                skillLevel = this.survivalist;
                break;

            case SkillList.Social:
                this.social = Mathf.RoundToInt(Random.Range(min_, max_));
                skillLevel = this.social;
                break;


            default:
                this.social = Mathf.RoundToInt(Random.Range(min_, max_));
                skillLevel = this.social;
                break;
        }

        //Making sure we check for any skill rewards for the leveled up skill
        SkillAbilityManager.globalReference.CheckCharacterSkillForNewAbility(this.GetComponent<Character>(), skillToSet_, 0, skillLevel);
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


    //Function called externally to add EXP to a given skill
    public void AddSkillEXP(SkillList skillToLevel_, int expToAdd_)
    {
        //Ints to track the skill's current level and the level it is after adding exp
        int currentSkillLevel = 0;
        int newSkillLevel = 0;

        switch (skillToLevel_)
        {
            case SkillList.Unarmed:
                currentSkillLevel = this.unarmed;
                if (this.unarmed < 100)
                {
                    this.unarmedEXP += expToAdd_;
                    //Checking to see if this skill levels up
                    while(this.unarmedEXP >= SkillAbilityManager.globalReference.GetEXPRequiredForLevel(this.unarmed))
                    {
                        this.unarmedEXP -= SkillAbilityManager.globalReference.GetEXPRequiredForLevel(this.unarmed);
                        this.LevelUpSkill(SkillList.Unarmed, 1);
                    }
                }
                newSkillLevel = this.unarmed;
                break;

            case SkillList.Daggers:
                currentSkillLevel = this.daggers;
                if (this.daggers < 100)
                {
                    this.daggersEXP += expToAdd_;
                    //Checking to see if this skill levels up
                    while (this.daggersEXP >= SkillAbilityManager.globalReference.GetEXPRequiredForLevel(this.daggers))
                    {
                        this.daggersEXP -= SkillAbilityManager.globalReference.GetEXPRequiredForLevel(this.daggers);
                        this.LevelUpSkill(SkillList.Daggers, 1);
                    }
                }
                newSkillLevel = this.daggers;
                break;

            case SkillList.Swords:
                currentSkillLevel = this.swords;
                if (this.swords < 100)
                {
                    this.swordsEXP += expToAdd_;
                    //Checking to see if this skill levels up
                    while (this.swordsEXP >= SkillAbilityManager.globalReference.GetEXPRequiredForLevel(this.swords))
                    {
                        this.swordsEXP -= SkillAbilityManager.globalReference.GetEXPRequiredForLevel(this.swords);
                        this.LevelUpSkill(SkillList.Swords, 1);
                    }
                }
                newSkillLevel = this.swords;
                break;

            case SkillList.Mauls:
                currentSkillLevel = this.mauls;
                if (this.mauls < 100)
                {
                    this.maulsEXP += expToAdd_;
                    //Checking to see if this skill levels up
                    while (this.maulsEXP >= SkillAbilityManager.globalReference.GetEXPRequiredForLevel(this.mauls))
                    {
                        this.maulsEXP -= SkillAbilityManager.globalReference.GetEXPRequiredForLevel(this.mauls);
                        this.LevelUpSkill(SkillList.Mauls, 1);
                    }
                }
                newSkillLevel = this.mauls;
                break;

            case SkillList.Poles:
                currentSkillLevel = this.poles;
                if (this.poles < 100)
                {
                    this.polesEXP += expToAdd_;
                    //Checking to see if this skill levels up
                    while (this.polesEXP >= SkillAbilityManager.globalReference.GetEXPRequiredForLevel(this.poles))
                    {
                        this.polesEXP -= SkillAbilityManager.globalReference.GetEXPRequiredForLevel(this.poles);
                        this.LevelUpSkill(SkillList.Poles, 1);
                    }
                }
                newSkillLevel = this.poles;
                break;

            case SkillList.Bows:
                currentSkillLevel = this.bows;
                if (this.bows < 100)
                {
                    this.bowsEXP += expToAdd_;
                    //Checking to see if this skill levels up
                    while (this.bowsEXP >= SkillAbilityManager.globalReference.GetEXPRequiredForLevel(this.bows))
                    {
                        this.bowsEXP -= SkillAbilityManager.globalReference.GetEXPRequiredForLevel(this.bows);
                        this.LevelUpSkill(SkillList.Bows, 1);
                    }
                }
                newSkillLevel = this.bows;
                break;

            case SkillList.Shields:
                currentSkillLevel = this.shields;
                if (this.shields < 100)
                {
                    this.shieldsEXP += expToAdd_;
                    //Checking to see if this skill levels up
                    while (this.shieldsEXP >= SkillAbilityManager.globalReference.GetEXPRequiredForLevel(this.shields))
                    {
                        this.shieldsEXP -= SkillAbilityManager.globalReference.GetEXPRequiredForLevel(this.shields);
                        this.LevelUpSkill(SkillList.Shields, 1);
                    }
                }
                newSkillLevel = this.shields;
                break;


            case SkillList.ArcaneMagic:
                currentSkillLevel = this.arcaneMagic;
                if (this.arcaneMagic < 100)
                {
                    this.arcaneMagicEXP += expToAdd_;
                    //Checking to see if this skill levels up
                    while (this.arcaneMagicEXP >= SkillAbilityManager.globalReference.GetEXPRequiredForLevel(this.arcaneMagic))
                    {
                        this.arcaneMagicEXP -= SkillAbilityManager.globalReference.GetEXPRequiredForLevel(this.arcaneMagic);
                        this.LevelUpSkill(SkillList.ArcaneMagic, 1);
                    }
                }
                newSkillLevel = this.arcaneMagic;
                break;

            case SkillList.HolyMagic:
                currentSkillLevel = this.holyMagic;
                if (this.holyMagic < 100)
                {
                    this.holyMagicEXP += expToAdd_;
                    //Checking to see if this skill levels up
                    while (this.holyMagicEXP >= SkillAbilityManager.globalReference.GetEXPRequiredForLevel(this.holyMagic))
                    {
                        this.holyMagicEXP -= SkillAbilityManager.globalReference.GetEXPRequiredForLevel(this.holyMagic);
                        this.LevelUpSkill(SkillList.HolyMagic, 1);
                    }
                }
                newSkillLevel = this.holyMagic;
                break;

            case SkillList.DarkMagic:
                currentSkillLevel = this.darkMagic;
                if (this.darkMagic < 100)
                {
                    this.darkMagicEXP += expToAdd_;
                    //Checking to see if this skill levels up
                    while (this.darkMagicEXP >= SkillAbilityManager.globalReference.GetEXPRequiredForLevel(this.darkMagic))
                    {
                        this.darkMagicEXP -= SkillAbilityManager.globalReference.GetEXPRequiredForLevel(this.darkMagic);
                        this.LevelUpSkill(SkillList.DarkMagic, 1);
                    }
                }
                newSkillLevel = this.darkMagic;
                break;

            case SkillList.FireMagic:
                currentSkillLevel = this.fireMagic;
                if (this.fireMagic < 100)
                {
                    this.fireMagicEXP += expToAdd_;
                    //Checking to see if this skill levels up
                    while (this.fireMagicEXP >= SkillAbilityManager.globalReference.GetEXPRequiredForLevel(this.fireMagic))
                    {
                        this.fireMagicEXP -= SkillAbilityManager.globalReference.GetEXPRequiredForLevel(this.fireMagic);
                        this.LevelUpSkill(SkillList.FireMagic, 1);
                    }
                }
                newSkillLevel = this.fireMagic;
                break;

            case SkillList.WaterMagic:
                currentSkillLevel = this.waterMagic;
                if (this.waterMagic < 100)
                {
                    this.waterMagicEXP += expToAdd_;
                    //Checking to see if this skill levels up
                    while (this.waterMagicEXP >= SkillAbilityManager.globalReference.GetEXPRequiredForLevel(this.waterMagic))
                    {
                        this.waterMagicEXP -= SkillAbilityManager.globalReference.GetEXPRequiredForLevel(this.waterMagic);
                        this.LevelUpSkill(SkillList.WaterMagic, 1);
                    }
                }
                newSkillLevel = this.waterMagic;
                break;

            case SkillList.WindMagic:
                currentSkillLevel = this.windMagic;
                if (this.windMagic < 100)
                {
                    this.windMagicEXP += expToAdd_;
                    //Checking to see if this skill levels up
                    while (this.windMagicEXP >= SkillAbilityManager.globalReference.GetEXPRequiredForLevel(this.windMagic))
                    {
                        this.windMagicEXP -= SkillAbilityManager.globalReference.GetEXPRequiredForLevel(this.windMagic);
                        this.LevelUpSkill(SkillList.WindMagic, 1);
                    }
                }
                newSkillLevel = this.windMagic;
                break;

            case SkillList.ElectricMagic:
                currentSkillLevel = this.electricMagic;
                if (this.electricMagic < 100)
                {
                    this.electricMagicEXP += expToAdd_;
                    //Checking to see if this skill levels up
                    while (this.electricMagicEXP >= SkillAbilityManager.globalReference.GetEXPRequiredForLevel(this.electricMagic))
                    {
                        this.electricMagicEXP -= SkillAbilityManager.globalReference.GetEXPRequiredForLevel(this.electricMagic);
                        this.LevelUpSkill(SkillList.ElectricMagic, 1);
                    }
                }
                newSkillLevel = this.electricMagic;
                break;

            case SkillList.StoneMagic:
                currentSkillLevel = this.stoneMagic;
                if (this.stoneMagic < 100)
                {
                    this.stoneMagicEXP += expToAdd_;
                    //Checking to see if this skill levels up
                    while (this.stoneMagicEXP >= SkillAbilityManager.globalReference.GetEXPRequiredForLevel(this.stoneMagic))
                    {
                        this.stoneMagicEXP -= SkillAbilityManager.globalReference.GetEXPRequiredForLevel(this.stoneMagic);
                        this.LevelUpSkill(SkillList.StoneMagic, 1);
                    }
                }
                newSkillLevel = this.stoneMagic;
                break;


            case SkillList.Survivalist:
                currentSkillLevel = this.survivalist;
                if (this.survivalist < 100)
                {
                    this.survivalistEXP += expToAdd_;
                    //Checking to see if this skill levels up
                    while (this.survivalistEXP >= SkillAbilityManager.globalReference.GetEXPRequiredForLevel(this.survivalist))
                    {
                        this.survivalistEXP -= SkillAbilityManager.globalReference.GetEXPRequiredForLevel(this.survivalist);
                        this.LevelUpSkill(SkillList.Survivalist, 1);
                    }
                }
                newSkillLevel = this.survivalist;
                break;

            case SkillList.Social:
                currentSkillLevel = this.social;
                if (this.social < 100)
                {
                    this.socialEXP += expToAdd_;
                    //Checking to see if this skill levels up
                    while (this.socialEXP >= SkillAbilityManager.globalReference.GetEXPRequiredForLevel(this.social))
                    {
                        this.socialEXP -= SkillAbilityManager.globalReference.GetEXPRequiredForLevel(this.social);
                        this.LevelUpSkill(SkillList.Social, 1);
                    }
                }
                newSkillLevel = this.social;
                break;


            default:
                currentSkillLevel = this.social;
                if (this.social < 100)
                {
                    this.socialEXP += expToAdd_;
                    //Checking to see if this skill levels up
                    while (this.socialEXP >= SkillAbilityManager.globalReference.GetEXPRequiredForLevel(this.social))
                    {
                        this.socialEXP -= SkillAbilityManager.globalReference.GetEXPRequiredForLevel(this.social);
                        this.LevelUpSkill(SkillList.Social, 1);
                    }
                }
                newSkillLevel = this.social;
                break;
        }

        //If the skill has gone up, we make sure we check for any skill rewards for the leveled up skill
        if (newSkillLevel > currentSkillLevel)
        {
            SkillAbilityManager.globalReference.CheckCharacterSkillForNewAbility(this.GetComponent<Character>(), skillToLevel_, currentSkillLevel, newSkillLevel);
        }
    }


    //Function called externally from PartyCreator.cs and SkillTome.cs to add levels to this character
    public void LevelUpSkill(SkillList skillToLevel_, int levelsToAdd_)
    {
        //Ints to track the skill's current level and the level it is after adding exp
        int currentSkillLevel = 0;
        int newSkillLevel = 0;

        switch (skillToLevel_)
        {
            case SkillList.Unarmed:
                currentSkillLevel = this.unarmed;
                this.unarmed += levelsToAdd_;
                if (this.unarmed > 100)
                {
                    this.unarmed = 100;
                }
                newSkillLevel = this.unarmed;
                break;

            case SkillList.Daggers:
                currentSkillLevel = this.daggers;
                this.daggers += levelsToAdd_;
                if (this.daggers > 100)
                {
                    this.daggers = 100;
                }
                newSkillLevel = this.daggers;
                break;

            case SkillList.Swords:
                currentSkillLevel = this.swords;
                this.swords += levelsToAdd_;
                if (this.swords > 100)
                {
                    this.swords = 100;
                }
                newSkillLevel = this.swords;
                break;

            case SkillList.Mauls:
                currentSkillLevel = this.mauls;
                this.mauls += levelsToAdd_;
                if (this.mauls > 100)
                {
                    this.mauls = 100;
                }
                newSkillLevel = this.mauls;
                break;

            case SkillList.Poles:
                currentSkillLevel = this.poles;
                this.poles += levelsToAdd_;
                if (this.poles > 100)
                {
                    this.poles = 100;
                }
                newSkillLevel = this.poles;
                break;

            case SkillList.Bows:
                currentSkillLevel = this.bows;
                this.bows += levelsToAdd_;
                if (this.bows > 100)
                {
                    this.bows = 100;
                }
                newSkillLevel = this.bows;
                break;

            case SkillList.Shields:
                currentSkillLevel = this.shields;
                this.shields += levelsToAdd_;
                if (this.shields > 100)
                {
                    this.shields = 100;
                }
                newSkillLevel = this.shields;
                break;


            case SkillList.ArcaneMagic:
                currentSkillLevel = this.arcaneMagic;
                this.arcaneMagic += levelsToAdd_;
                if (this.arcaneMagic > 100)
                {
                    this.arcaneMagic = 100;
                }
                newSkillLevel = this.arcaneMagic;
                break;

            case SkillList.HolyMagic:
                currentSkillLevel = this.holyMagic;
                this.holyMagic += levelsToAdd_;
                if (this.holyMagic > 100)
                {
                    this.holyMagic = 100;
                }
                newSkillLevel = this.holyMagic;
                break;

            case SkillList.DarkMagic:
                currentSkillLevel = this.darkMagic;
                this.darkMagic += levelsToAdd_;
                if (this.darkMagic > 100)
                {
                    this.darkMagic = 100;
                }
                newSkillLevel = this.darkMagic;
                break;

            case SkillList.FireMagic:
                currentSkillLevel = this.fireMagic;
                this.fireMagic += levelsToAdd_;
                if (this.fireMagic > 100)
                {
                    this.fireMagic = 100;
                }
                newSkillLevel = this.fireMagic;
                break;

            case SkillList.WaterMagic:
                currentSkillLevel = this.waterMagic;
                this.waterMagic += levelsToAdd_;
                if (this.waterMagic > 100)
                {
                    this.waterMagic = 100;
                }
                newSkillLevel = this.waterMagic;
                break;

            case SkillList.WindMagic:
                currentSkillLevel = this.windMagic;
                this.windMagic += levelsToAdd_;
                if (this.windMagic > 100)
                {
                    this.windMagic = 100;
                }
                newSkillLevel = this.windMagic;
                break;

            case SkillList.ElectricMagic:
                currentSkillLevel = this.electricMagic;
                this.electricMagic += levelsToAdd_;
                if (this.electricMagic > 100)
                {
                    this.electricMagic = 100;
                }
                newSkillLevel = this.electricMagic;
                break;

            case SkillList.StoneMagic:
                currentSkillLevel = this.stoneMagic;
                this.stoneMagic += levelsToAdd_;
                if (this.stoneMagic > 100)
                {
                    this.stoneMagic = 100;
                }
                newSkillLevel = this.stoneMagic;
                break;


            case SkillList.Survivalist:
                currentSkillLevel = this.survivalist;
                this.survivalist += levelsToAdd_;
                if (this.survivalist > 100)
                {
                    this.survivalist = 100;
                }
                newSkillLevel = this.survivalist;
                break;

            case SkillList.Social:
                currentSkillLevel = this.social;
                this.social += levelsToAdd_;
                if (this.social > 100)
                {
                    this.social = 100;
                }
                newSkillLevel = this.social;
                break;


            default:
                currentSkillLevel = this.social;
                this.social += levelsToAdd_;
                if(this.social > 100)
                {
                    this.social = 100;
                }
                newSkillLevel = this.social;
                break;
        }

        //If the skill has gone up, we make sure we check for any skill rewards for the leveled up skill
        if (newSkillLevel > currentSkillLevel)
        {
            SkillAbilityManager.globalReference.CheckCharacterSkillForNewAbility(this.GetComponent<Character>(), skillToLevel_, currentSkillLevel, newSkillLevel);
        }
        //Also checking to see if there are any class combination rewards tied to this skill
        ClassCombinationManager.globalReference.CheckForClassCombinationRewards(this.GetComponent<Character>(), skillToLevel_);
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
