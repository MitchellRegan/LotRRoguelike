using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTLevelSkillReward : MonoBehaviour
{
    //Number of levels to give
    [Range(1, 10)]
    public int levelsToGive = 1;

    //Skills to level up for each num pad button
    public SkillList numZero = SkillList.Unarmed;
    public SkillList numOne = SkillList.Daggers;
    public SkillList numTwo = SkillList.Swords;
    public SkillList numThree = SkillList.Mauls;
    public SkillList numFour = SkillList.Poles;
    public SkillList numFive = SkillList.Bows;
    public SkillList numSix = SkillList.Shields;
    public SkillList numSeven = SkillList.ArcaneMagic;
    public SkillList numEight = SkillList.HolyMagic;
    public SkillList numNine = SkillList.DarkMagic;
    public SkillList numSlash = SkillList.FireMagic;
    public SkillList numStar = SkillList.WaterMagic;
    public SkillList numMinus = SkillList.WindMagic;
    public SkillList numPlus = SkillList.ElectricMagic;
    public SkillList numEnter = SkillList.StoneMagic;
    public SkillList numPeriod = SkillList.Survivalist;
    public SkillList numLock = SkillList.Social;


    
	// Update is called once per frame
	private void Update ()
    {
        //Numpad 0
		if(Input.GetKeyDown(KeyCode.Keypad0))
        {
            if(PartyGroup.group1.charactersInParty[0] != null)
            {
                PartyGroup.group1.charactersInParty[0].charSkills.LevelUpSkill(this.numZero, this.levelsToGive);
            }
        }

        //Numpad 1
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            if (PartyGroup.group1.charactersInParty[0] != null)
            {
                PartyGroup.group1.charactersInParty[0].charSkills.LevelUpSkill(this.numOne, this.levelsToGive);
            }
        }

        //Numpad 2
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            if (PartyGroup.group1.charactersInParty[0] != null)
            {
                PartyGroup.group1.charactersInParty[0].charSkills.LevelUpSkill(this.numTwo, this.levelsToGive);
            }
        }

        //Numpad 3
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            if (PartyGroup.group1.charactersInParty[0] != null)
            {
                PartyGroup.group1.charactersInParty[0].charSkills.LevelUpSkill(this.numThree, this.levelsToGive);
            }
        }

        //Numpad 4
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            if (PartyGroup.group1.charactersInParty[0] != null)
            {
                PartyGroup.group1.charactersInParty[0].charSkills.LevelUpSkill(this.numFour, this.levelsToGive);
            }
        }

        //Numpad 5
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            if (PartyGroup.group1.charactersInParty[0] != null)
            {
                PartyGroup.group1.charactersInParty[0].charSkills.LevelUpSkill(this.numFive, this.levelsToGive);
            }
        }

        //Numpad 6
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            if (PartyGroup.group1.charactersInParty[0] != null)
            {
                PartyGroup.group1.charactersInParty[0].charSkills.LevelUpSkill(this.numSix, this.levelsToGive);
            }
        }

        //Numpad 7
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            if (PartyGroup.group1.charactersInParty[0] != null)
            {
                PartyGroup.group1.charactersInParty[0].charSkills.LevelUpSkill(this.numSeven, this.levelsToGive);
            }
        }

        //Numpad 8
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            if (PartyGroup.group1.charactersInParty[0] != null)
            {
                PartyGroup.group1.charactersInParty[0].charSkills.LevelUpSkill(this.numEight, this.levelsToGive);
            }
        }

        //Numpad 9
        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            if (PartyGroup.group1.charactersInParty[0] != null)
            {
                PartyGroup.group1.charactersInParty[0].charSkills.LevelUpSkill(this.numNine, this.levelsToGive);
            }
        }

        //Numlock
        if (Input.GetKeyDown(KeyCode.Numlock))
        {
            if (PartyGroup.group1.charactersInParty[0] != null)
            {
                PartyGroup.group1.charactersInParty[0].charSkills.LevelUpSkill(this.numLock, this.levelsToGive);
            }
        }

        //Numpad /
        if (Input.GetKeyDown(KeyCode.KeypadDivide))
        {
            if (PartyGroup.group1.charactersInParty[0] != null)
            {
                PartyGroup.group1.charactersInParty[0].charSkills.LevelUpSkill(this.numSlash, this.levelsToGive);
            }
        }

        //Numpad *
        if (Input.GetKeyDown(KeyCode.KeypadMultiply))
        {
            if (PartyGroup.group1.charactersInParty[0] != null)
            {
                PartyGroup.group1.charactersInParty[0].charSkills.LevelUpSkill(this.numStar, this.levelsToGive);
            }
        }

        //Numpad -
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            if (PartyGroup.group1.charactersInParty[0] != null)
            {
                PartyGroup.group1.charactersInParty[0].charSkills.LevelUpSkill(this.numMinus, this.levelsToGive);
            }
        }

        //Numpad +
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            if (PartyGroup.group1.charactersInParty[0] != null)
            {
                PartyGroup.group1.charactersInParty[0].charSkills.LevelUpSkill(this.numPlus, this.levelsToGive);
            }
        }

        //Numpad enter
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (PartyGroup.group1.charactersInParty[0] != null)
            {
                PartyGroup.group1.charactersInParty[0].charSkills.LevelUpSkill(this.numEnter, this.levelsToGive);
            }
        }

        //Numpad .
        if (Input.GetKeyDown(KeyCode.KeypadPeriod))
        {
            if (PartyGroup.group1.charactersInParty[0] != null)
            {
                PartyGroup.group1.charactersInParty[0].charSkills.LevelUpSkill(this.numPeriod, this.levelsToGive);
            }
        }
    }
}
