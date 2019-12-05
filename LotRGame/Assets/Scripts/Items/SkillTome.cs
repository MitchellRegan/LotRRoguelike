using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTome : Item
{
    //The type of skill to increase
    public SkillList skillToLearn = SkillList.Unarmed;

    //The list of skill progressions to use when determining the skill points to add
    public List<SkillTomeProgression> progressionCurves;

    

    //Function called externally from InventoryButton.cs to make a character learn this skill
    public void CharacterUseTome(Character charToLearn_)
    {
        //Int to hold the character's skill level that we'll improve
        int currentSkillLevel = this.GetCurrentSkillLevel(charToLearn_);

        //Int to hold what the new skill level will be
        int improvedSkillLevel = currentSkillLevel;

        //Looping through each progression curve
        for(int c = 0; c < this.progressionCurves.Count; ++c)
        {
            SkillTomeProgression curve = this.progressionCurves[c];

            //Checking to see if the player's current skill level is between the ranges for the curve
            if(currentSkillLevel >= curve.skillRangeMin && currentSkillLevel <= curve.skillRangeMax)
            {
                //Switch statement to allocate points based on the curve type
                switch(curve.progressionCurve)
                {
                    case ProgressionCurves.Linear:
                        //Getting the percent that the player is between the curve range min/max
                        float percentL = (1f * (currentSkillLevel - curve.skillRangeMin)) /
                                        (1f * (curve.skillRangeMax - curve.skillRangeMin));
                        //Finding the new value between the new skill min/max
                        float newSkillF = (curve.newSkillMax - curve.newSkillMin) * percentL;
                        improvedSkillLevel = curve.newSkillMin + Mathf.RoundToInt(newSkillF);
                        break;

                    case ProgressionCurves.SineIn:
                        //Getting the percent that the player is between the curve range min/max
                        float percentSI = (1f * (currentSkillLevel - curve.skillRangeMin)) /
                                        (1f * (curve.skillRangeMax - curve.skillRangeMin));
                        //Creating a new interpolator to get the sine in interp using the percent SI
                        Interpolator siInterp = new Interpolator();
                        siInterp.ease = EaseType.SineIn;
                        siInterp.SetDuration(1);
                        siInterp.AddTime(percentSI);
                        //Finding the new value between the new skill min/max using the interpolated percent
                        float newSkillSI = (curve.newSkillMax - curve.newSkillMin) * siInterp.GetProgress();
                        improvedSkillLevel = curve.newSkillMin + Mathf.RoundToInt(newSkillSI);
                        break;

                    case ProgressionCurves.SineOut:
                        //Getting the percent that the player is between the curve range min/max
                        float percentSO = (1f * (currentSkillLevel - curve.skillRangeMin)) /
                                        (1f * (curve.skillRangeMax - curve.skillRangeMin));
                        //Creating a new interpolator to get the sine in interp using the percent SO
                        Interpolator soInterp = new Interpolator();
                        soInterp.ease = EaseType.SineOut;
                        soInterp.SetDuration(1);
                        soInterp.AddTime(percentSO);
                        //Finding the new value between the new skill min/max using the interpolated percent
                        float newSkillSO = (curve.newSkillMax - curve.newSkillMin) * soInterp.GetProgress();
                        improvedSkillLevel = curve.newSkillMin + Mathf.RoundToInt(newSkillSO);
                        break;

                    case ProgressionCurves.UpToMax:
                        //Setting the skill level to the max
                        improvedSkillLevel = curve.newSkillMax;
                        break;
                }

                //Breaking the loop so we don't have to bother with the other curves
                break;
            }
        }

        //Setting the new character skill value
        this.SetImprovedSkillLevel(charToLearn_, improvedSkillLevel - currentSkillLevel);
    }


    //Function called from CharacterUseTome to get the current skill value that this tome improves
    private int GetCurrentSkillLevel(Character characterRef_)
    {
        //Switch statement to go through each skill and get the one that we'll improve
        switch(this.skillToLearn)
        {
            case SkillList.Unarmed:
                return characterRef_.charSkills.GetSkillLevelValue(SkillList.Unarmed);

            case SkillList.Daggers:
                return characterRef_.charSkills.GetSkillLevelValue(SkillList.Daggers);

            case SkillList.Swords:
                return characterRef_.charSkills.GetSkillLevelValue(SkillList.Swords);

            case SkillList.Mauls:
                return characterRef_.charSkills.GetSkillLevelValue(SkillList.Mauls);

            case SkillList.Poles:
                return characterRef_.charSkills.GetSkillLevelValue(SkillList.Poles);

            case SkillList.Bows:
                return characterRef_.charSkills.GetSkillLevelValue(SkillList.Bows);

            case SkillList.Shields:
                return characterRef_.charSkills.GetSkillLevelValue(SkillList.Shields);



            case SkillList.ArcaneMagic:
                return characterRef_.charSkills.GetSkillLevelValue(SkillList.ArcaneMagic);

            case SkillList.HolyMagic:
                return characterRef_.charSkills.GetSkillLevelValue(SkillList.HolyMagic);

            case SkillList.DarkMagic:
                return characterRef_.charSkills.GetSkillLevelValue(SkillList.DarkMagic);

            case SkillList.FireMagic:
                return characterRef_.charSkills.GetSkillLevelValue(SkillList.FireMagic);

            case SkillList.WaterMagic:
                return characterRef_.charSkills.GetSkillLevelValue(SkillList.WaterMagic);

            case SkillList.WindMagic:
                return characterRef_.charSkills.GetSkillLevelValue(SkillList.WindMagic);

            case SkillList.ElectricMagic:
                return characterRef_.charSkills.GetSkillLevelValue(SkillList.ElectricMagic);

            case SkillList.StoneMagic:
                return characterRef_.charSkills.GetSkillLevelValue(SkillList.StoneMagic);



            case SkillList.Survivalist:
                return characterRef_.charSkills.GetSkillLevelValue(SkillList.Survivalist);

            case SkillList.Social:
                return characterRef_.charSkills.GetSkillLevelValue(SkillList.Social);

            default:
                return 0;
        }
    }


    //Function called from CharacterUseTome to set the player's new skill value
    private void SetImprovedSkillLevel(Character characterRef_, int newSkillLevel_)
    {
        //Looping through a number of times that we level up. This is to make sure the character gets all of the skill rewards
        for(int lvl = 0; lvl < newSkillLevel_; ++lvl)
        {
            characterRef_.charSkills.LevelUpSkill(this.skillToLearn, 1);
        }

        //Switch statement to go through each skill and get the one that we'll improve
        /*switch (this.skillToLearn)
        {
            case SkillList.Unarmed:
                characterRef_.charSkills.LevelUpSkill(SkillList.Unarmed, newSkillLevel_);
                break;

            case SkillList.Daggers:
                characterRef_.charSkills.LevelUpSkill(SkillList.Daggers, newSkillLevel_);
                break;

            case SkillList.Swords:
                characterRef_.charSkills.LevelUpSkill(SkillList.Swords, newSkillLevel_);
                break;

            case SkillList.Mauls:
                characterRef_.charSkills.LevelUpSkill(SkillList.Mauls, newSkillLevel_);
                break;

            case SkillList.Poles:
                characterRef_.charSkills.LevelUpSkill(SkillList.Poles, newSkillLevel_);
                break;

            case SkillList.Bows:
                characterRef_.charSkills.LevelUpSkill(SkillList.Bows, newSkillLevel_);
                break;

            case SkillList.Shields:
                characterRef_.charSkills.LevelUpSkill(SkillList.Shields, newSkillLevel_);
                break;



            case SkillList.ArcaneMagic:
                characterRef_.charSkills.LevelUpSkill(SkillList.ArcaneMagic, newSkillLevel_);
                break;

            case SkillList.HolyMagic:
                characterRef_.charSkills.LevelUpSkill(SkillList.HolyMagic, newSkillLevel_);
                break;

            case SkillList.DarkMagic:
                characterRef_.charSkills.LevelUpSkill(SkillList.DarkMagic, newSkillLevel_);
                break;

            case SkillList.FireMagic:
                characterRef_.charSkills.LevelUpSkill(SkillList.FireMagic, newSkillLevel_);
                break;

            case SkillList.WaterMagic:
                characterRef_.charSkills.LevelUpSkill(SkillList.WaterMagic, newSkillLevel_);
                break;

            case SkillList.WindMagic:
                characterRef_.charSkills.LevelUpSkill(SkillList.WindMagic, newSkillLevel_);
                break;

            case SkillList.ElectricMagic:
                characterRef_.charSkills.LevelUpSkill(SkillList.ElectricMagic, newSkillLevel_);
                break;

            case SkillList.StoneMagic:
                characterRef_.charSkills.LevelUpSkill(SkillList.StoneMagic, newSkillLevel_);
                break;



            case SkillList.Survivalist:
                characterRef_.charSkills.LevelUpSkill(SkillList.Survivalist, newSkillLevel_);
                break;

            case SkillList.Social:
                characterRef_.charSkills.LevelUpSkill(SkillList.Social, newSkillLevel_);
                break;
        }*/
    }
}

