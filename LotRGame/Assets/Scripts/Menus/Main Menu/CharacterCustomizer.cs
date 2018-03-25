﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCustomizer : MonoBehaviour
{
    //The list of character customizers for each playable race
    public List<CustomizerRaceSpriteBase> raceSpriteBases;

    [Space(8)]

    //The text for the first name of the character
    public InputField firstNameText;
    //The text for the last name of the character
    public InputField lastNameText;

    [Space(8)]

    //The sex of the character
    [HideInInspector]
    public Genders sex = Genders.Male;
    //The text for the sex of the character
    public Text sexText;

    [Space(8)]

    //The race of the character
    [HideInInspector]
    public RaceTypes.Races race = RaceTypes.Races.Human;

    //The total number of skill points that can be allocated to the character
    public int pointsToAllocate = 20;
    //The number of remaining points that can be allocated
    private int currentPoints = 0;

    //Dictionary to hold the number of points that track how many skill points are currently allocated to each skill
    public Dictionary<SkillList, int> allocatedSkillPoints;

    [Space(8)]

    //Reference to the text that shows the remaining points to allocate
    public Text remainingPointsText;

    [Space(8)]

    //Reference to the text that shows the allocated points for each skill
    public Text unarmedPointText;
    public Text daggersPointText;
    public Text swordsPointText;
    public Text maulsPointText;
    public Text polesPointText;
    public Text bowsPointText;

    public Text arcaneMagicPointText;
    public Text holyMagicPointText;
    public Text darkMagicPointText;
    public Text fireMagicPointText;
    public Text waterMagicPointText;
    public Text windMagicPointText;
    public Text electricMagicPointText;
    public Text stoneMagicPointText;

    public Text survivalistPointText;
    public Text socialPointText;



    // Use this for initialization
    public void InitializeCustomizer()
    {
        //Resetting the current points for the main character
        this.currentPoints = this.pointsToAllocate;

        //Initializing the allocated skill dictionary
        this.allocatedSkillPoints = new Dictionary<SkillList, int>();

        this.allocatedSkillPoints.Add(SkillList.Unarmed, 0);
        this.allocatedSkillPoints.Add(SkillList.Daggers, 0);
        this.allocatedSkillPoints.Add(SkillList.Swords, 0);
        this.allocatedSkillPoints.Add(SkillList.Mauls, 0);
        this.allocatedSkillPoints.Add(SkillList.Poles, 0);
        this.allocatedSkillPoints.Add(SkillList.Bows, 0);

        this.allocatedSkillPoints.Add(SkillList.ArcaneMagic, 0);
        this.allocatedSkillPoints.Add(SkillList.HolyMagic, 0);
        this.allocatedSkillPoints.Add(SkillList.DarkMagic, 0);
        this.allocatedSkillPoints.Add(SkillList.FireMagic, 0);
        this.allocatedSkillPoints.Add(SkillList.WaterMagic, 0);
        this.allocatedSkillPoints.Add(SkillList.WindMagic, 0);
        this.allocatedSkillPoints.Add(SkillList.ElectricMagic, 0);
        this.allocatedSkillPoints.Add(SkillList.StoneMagic, 0);

        this.allocatedSkillPoints.Add(SkillList.Survivalist, 0);
        this.allocatedSkillPoints.Add(SkillList.Social, 0);

        //Updating all of our text to show the points
        this.UpdateText();
    }


    //Function called when this game object is enabled
    private void OnEnable()
    {
        //Looping through each customizer race sprite base
        foreach(CustomizerRaceSpriteBase rsb in this.raceSpriteBases)
        {
            //Setting the sprite customizers to our character's sex
            rsb.customizer.sex = this.sex;

            //If the currently selected race is the same as this race sprite base, we turn on its customizer
            if (rsb.race == GameData.globalReference.startingRace)
            {
                rsb.customizer.gameObject.SetActive(true);
            }
            //If the customizer's race isn't the selected one, we disable its customizer
            else
            {
                rsb.customizer.gameObject.SetActive(false);
            }
        }
    }


    //Function called externally to move to the next character sex
    public void SwitchToNextSex()
    {
        switch(this.sex)
        {
            //If this character is male, they're now female
            case Genders.Male:
                this.sex = Genders.Female;
                this.sexText.text = "Female";
                break;
            
            //If this character is female, they're now genderless
            case Genders.Female:
                //If the current player race is Amazon, nothing happens because Amazons are all women
                if (GameData.globalReference.startingRace != RaceTypes.Races.Amazon)
                {
                    //If the current player race is Gill Folk, they can be genderless
                    if (GameData.globalReference.startingRace == RaceTypes.Races.GillFolk || GameData.globalReference.startingRace == RaceTypes.Races.Elf)
                    {
                        this.sex = Genders.Genderless;
                        this.sexText.text = "Genderless";
                    }
                    //Otherwise, they're now male
                    else
                    {
                        this.sex = Genders.Male;
                        this.sexText.text = "Male";
                    }
                }
                break;
            
            //If this character is genderless, they're now male
            case Genders.Genderless:
                this.sex = Genders.Male;
                this.sexText.text = "Male";
                break;
        }

        //Setting all of our race sprite bases to the designated sex
        foreach(CustomizerRaceSpriteBase rsb in this.raceSpriteBases)
        {
            rsb.customizer.sex = this.sex;
        }
    }


    //Function called externally to move to the previous character sex
    public void SwitchToPreviousSex()
    {
        switch (this.sex)
        {
            //If this character is male, they're now genderless
            case Genders.Male:
                //If the current player race is Gill Folk or Elf, they can be genderless
                if (GameData.globalReference.startingRace == RaceTypes.Races.GillFolk || GameData.globalReference.startingRace == RaceTypes.Races.Elf)
                {
                    this.sex = Genders.Genderless;
                    this.sexText.text = "Genderless";
                }
                //Otherwise, they're now female
                else
                {
                    this.sex = Genders.Female;
                    this.sexText.text = "Female";
                }
                    break;

            //If this character is female, they're now male
            case Genders.Female:
                //If the current player race is Amazon, nothing happens because Amazons are all women
                if (GameData.globalReference.startingRace != RaceTypes.Races.Amazon)
                {
                    this.sex = Genders.Male;
                    this.sexText.text = "Male";
                }
                break;

            //If this character is genderless, they're now female
            case Genders.Genderless:
                this.sex = Genders.Female;
                this.sexText.text = "Female";
                break;
        }

        //Setting all of our race sprite bases to the designated sex
        foreach (CustomizerRaceSpriteBase rsb in this.raceSpriteBases)
        {
            rsb.customizer.sex = this.sex;
        }
    }


    //Function called externally to allocate a point to a skill
    public void AllocateSkillPoint(string skillToIncrease_)
    {
        //If we don't have any remaining points to allocate, nothing happens
        if (this.currentPoints < 1)
        {
            return;
        }

        //Converting the given string for the skill name to the enum so we can find it in our dictionary
        SkillList parsedEnum = (SkillList)System.Enum.Parse(typeof(SkillList), skillToIncrease_);

        //Making sure we're able to convert the string to the enum
        int s;
        if (this.allocatedSkillPoints.TryGetValue(parsedEnum, out s))
        {
            //Removing a point from the remaining pool
            this.currentPoints -= 1;

            //Adding a point to the given skill
            this.allocatedSkillPoints[parsedEnum] += 1;

            //Updating all of our text to show the points
            this.UpdateText();
        }
    }


    //Function called externally to de-allocate a point from a skill
    public void DeallocateSkillPoint(string skillToDecrease_)
    {
        //Converting the given string for the skill name to the enum so we can find it in our dictionary
        SkillList parsedEnum = (SkillList)System.Enum.Parse(typeof(SkillList), skillToDecrease_);

        //Making sure we're able to convert the string to the enum
        int s;
        if (this.allocatedSkillPoints.TryGetValue(parsedEnum, out s))
        {
            //Making sure the skill that we're removing a point from has points to remove
            if (this.allocatedSkillPoints[parsedEnum] < 1)
            {
                return;
            }

            //Removing a point from the given skill
            this.allocatedSkillPoints[parsedEnum] -= 1;

            //Adding a point to the remaining pool
            this.currentPoints += 1;

            //Updating all of our text to show the points
            this.UpdateText();
        }
    }


    //Function called internally to update the text for each allocated point
    private void UpdateText()
    {
        //Displaying the current points
        this.remainingPointsText.text = "" + this.currentPoints;

        //Displaying all of the allocated skill points
        this.unarmedPointText.text = "" + this.allocatedSkillPoints[SkillList.Unarmed];
        this.daggersPointText.text = "" + this.allocatedSkillPoints[SkillList.Daggers];
        this.swordsPointText.text = "" + this.allocatedSkillPoints[SkillList.Swords];
        this.maulsPointText.text = "" + this.allocatedSkillPoints[SkillList.Mauls];
        this.polesPointText.text = "" + this.allocatedSkillPoints[SkillList.Poles];
        this.bowsPointText.text = "" + this.allocatedSkillPoints[SkillList.Bows];

        this.arcaneMagicPointText.text = "" + this.allocatedSkillPoints[SkillList.ArcaneMagic];
        this.holyMagicPointText.text = "" + this.allocatedSkillPoints[SkillList.HolyMagic];
        this.darkMagicPointText.text = "" + this.allocatedSkillPoints[SkillList.DarkMagic];
        this.fireMagicPointText.text = "" + this.allocatedSkillPoints[SkillList.FireMagic];
        this.waterMagicPointText.text = "" + this.allocatedSkillPoints[SkillList.WaterMagic];
        this.windMagicPointText.text = "" + this.allocatedSkillPoints[SkillList.WindMagic];
        this.electricMagicPointText.text = "" + this.allocatedSkillPoints[SkillList.ElectricMagic];
        this.stoneMagicPointText.text = "" + this.allocatedSkillPoints[SkillList.StoneMagic];

        this.survivalistPointText.text = "" + this.allocatedSkillPoints[SkillList.Survivalist];
        this.socialPointText.text = "" + this.allocatedSkillPoints[SkillList.Social];
    }
}

//Class used by CharacterCustomizer.cs so we can distinguish which sprite base to use
[System.Serializable]
public class CustomizerRaceSpriteBase
{
    //The race that this customizer uses
    public RaceTypes.Races race;
    //The customizer that is tied to the selected race
    public SpriteCustomizer customizer;
}