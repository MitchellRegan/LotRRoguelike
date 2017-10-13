using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCustomizer : MonoBehaviour
{
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
    public Text punchingPointText;
    public Text daggersPointText;
    public Text swordsPointText;
    public Text axesPointText;
    public Text spearsPointText;
    public Text bowsPointText;
    public Text improvisedPointText;

    public Text holyMagicPointText;
    public Text darkMagicPointText;
    public Text natureMagicPointText;

    public Text cookingPointText;
    public Text healingPointText;
    public Text craftingPointText;

    public Text foragingPointText;
    public Text trackingPointText;
    public Text fishingPointText;

    public Text climbingPointText;
    public Text hidingPointText;
    public Text swimmingPointText;



    // Use this for initialization
    public void Awake()
    {
        //Resetting the current points for the main character
        this.currentPoints = this.pointsToAllocate;

        //Initializing the allocated skill dictionary
        this.allocatedSkillPoints = new Dictionary<SkillList, int>();

        this.allocatedSkillPoints.Add(SkillList.Punching, 0);
        this.allocatedSkillPoints.Add(SkillList.Daggers, 0);
        this.allocatedSkillPoints.Add(SkillList.Swords, 0);
        this.allocatedSkillPoints.Add(SkillList.Axes, 0);
        this.allocatedSkillPoints.Add(SkillList.Spears, 0);
        this.allocatedSkillPoints.Add(SkillList.Bows, 0);
        this.allocatedSkillPoints.Add(SkillList.Improvised, 0);

        this.allocatedSkillPoints.Add(SkillList.HolyMagic, 0);
        this.allocatedSkillPoints.Add(SkillList.DarkMagic, 0);
        this.allocatedSkillPoints.Add(SkillList.NatureMagic, 0);

        this.allocatedSkillPoints.Add(SkillList.Cooking, 0);
        this.allocatedSkillPoints.Add(SkillList.Healing, 0);
        this.allocatedSkillPoints.Add(SkillList.Crafting, 0);

        this.allocatedSkillPoints.Add(SkillList.Foraging, 0);
        this.allocatedSkillPoints.Add(SkillList.Tracking, 0);
        this.allocatedSkillPoints.Add(SkillList.Fishing, 0);

        this.allocatedSkillPoints.Add(SkillList.Climbing, 0);
        this.allocatedSkillPoints.Add(SkillList.Hiding, 0);
        this.allocatedSkillPoints.Add(SkillList.Swimming, 0);

        //Updating all of our text to show the points
        this.UpdateText();
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
        this.punchingPointText.text = "" + this.allocatedSkillPoints[SkillList.Punching];
        this.daggersPointText.text = "" + this.allocatedSkillPoints[SkillList.Daggers];
        this.swordsPointText.text = "" + this.allocatedSkillPoints[SkillList.Swords];
        this.axesPointText.text = "" + this.allocatedSkillPoints[SkillList.Axes];
        this.spearsPointText.text = "" + this.allocatedSkillPoints[SkillList.Spears];
        this.bowsPointText.text = "" + this.allocatedSkillPoints[SkillList.Bows];
        this.improvisedPointText.text = "" + this.allocatedSkillPoints[SkillList.Improvised];

        this.holyMagicPointText.text = "" + this.allocatedSkillPoints[SkillList.HolyMagic];
        this.darkMagicPointText.text = "" + this.allocatedSkillPoints[SkillList.DarkMagic];
        this.natureMagicPointText.text = "" + this.allocatedSkillPoints[SkillList.NatureMagic];

        this.cookingPointText.text = "" + this.allocatedSkillPoints[SkillList.Cooking];
        this.healingPointText.text = "" + this.allocatedSkillPoints[SkillList.Healing];
        this.craftingPointText.text = "" + this.allocatedSkillPoints[SkillList.Crafting];

        this.foragingPointText.text = "" + this.allocatedSkillPoints[SkillList.Foraging];
        this.trackingPointText.text = "" + this.allocatedSkillPoints[SkillList.Tracking];
        this.fishingPointText.text = "" + this.allocatedSkillPoints[SkillList.Fishing];

        this.climbingPointText.text = "" + this.allocatedSkillPoints[SkillList.Climbing];
        this.hidingPointText.text = "" + this.allocatedSkillPoints[SkillList.Hiding];
        this.swimmingPointText.text = "" + this.allocatedSkillPoints[SkillList.Swimming];
    }
}


public enum SkillList
{
    //Combat skills
    Punching,
    Daggers,
    Swords,
    Axes,
    Spears,
    Bows,
    Improvised,

    //Magic skills
    HolyMagic,
    DarkMagic,
    NatureMagic,

    //Creative
    Cooking,
    Healing,
    Crafting,

    //Survival
    Foraging,
    Tracking,
    Fishing,

    //Tactial
    Climbing,
    Hiding,
    Swimming
}
