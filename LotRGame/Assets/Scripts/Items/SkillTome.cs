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
                    case SkillTomeProgression.progressionCurves.Linear:
                        //Getting the percent that the player is between the curve range min/max
                        float percentL = (1f * (currentSkillLevel - curve.skillRangeMin)) /
                                        (1f * (curve.skillRangeMax - curve.skillRangeMin));
                        //Finding the new value between the new skill min/max
                        float newSkillF = (curve.newSkillMax - curve.newSkillMin) * percentL;
                        improvedSkillLevel = curve.newSkillMin + Mathf.RoundToInt(newSkillF);
                        break;

                    case SkillTomeProgression.progressionCurves.SineIn:
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

                    case SkillTomeProgression.progressionCurves.SineOut:
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

                    case SkillTomeProgression.progressionCurves.UpToMax:
                        //Setting the skill level to the max
                        improvedSkillLevel = curve.newSkillMax;
                        break;
                }

                //Breaking the loop so we don't have to bother with the other curves
                break;
            }
        }

        //Setting the new character skill value
        this.SetImprovedSkillLevel(charToLearn_, improvedSkillLevel);
    }


    //Function called from CharacterUseTome to get the current skill value that this tome improves
    private int GetCurrentSkillLevel(Character characterRef_)
    {
        //Switch statement to go through each skill and get the one that we'll improve
        switch(this.skillToLearn)
        {
            case SkillList.Unarmed:
                return characterRef_.charSkills.unarmed;

            case SkillList.Daggers:
                return characterRef_.charSkills.daggers;

            case SkillList.Swords:
                return characterRef_.charSkills.swords;

            case SkillList.Mauls:
                return characterRef_.charSkills.mauls;

            case SkillList.Poles:
                return characterRef_.charSkills.poles;

            case SkillList.Bows:
                return characterRef_.charSkills.bows;

            case SkillList.Shields:
                return characterRef_.charSkills.shields;



            case SkillList.ArcaneMagic:
                return characterRef_.charSkills.arcaneMagic;

            case SkillList.HolyMagic:
                return characterRef_.charSkills.holyMagic;

            case SkillList.DarkMagic:
                return characterRef_.charSkills.darkMagic;

            case SkillList.FireMagic:
                return characterRef_.charSkills.fireMagic;

            case SkillList.WaterMagic:
                return characterRef_.charSkills.waterMagic;

            case SkillList.WindMagic:
                return characterRef_.charSkills.windMagic;

            case SkillList.ElectricMagic:
                return characterRef_.charSkills.electricMagic;

            case SkillList.StoneMagic:
                return characterRef_.charSkills.stoneMagic;



            case SkillList.Survivalist:
                return characterRef_.charSkills.survivalist;

            case SkillList.Social:
                return characterRef_.charSkills.social;

            default:
                return 0;
        }
    }


    //Function called from CharacterUseTome to set the player's new skill value
    private void SetImprovedSkillLevel(Character characterRef_, int newSkillLevel_)
    {
        //Switch statement to go through each skill and get the one that we'll improve
        switch (this.skillToLearn)
        {
            case SkillList.Unarmed:
                characterRef_.charSkills.unarmed = newSkillLevel_;
                break;

            case SkillList.Daggers:
                characterRef_.charSkills.daggers = newSkillLevel_;
                break;

            case SkillList.Swords:
                characterRef_.charSkills.swords = newSkillLevel_;
                break;

            case SkillList.Mauls:
                characterRef_.charSkills.mauls = newSkillLevel_;
                break;

            case SkillList.Poles:
                characterRef_.charSkills.poles = newSkillLevel_;
                break;

            case SkillList.Bows:
                characterRef_.charSkills.bows = newSkillLevel_;
                break;



            case SkillList.ArcaneMagic:
                characterRef_.charSkills.arcaneMagic = newSkillLevel_;
                break;

            case SkillList.HolyMagic:
                characterRef_.charSkills.holyMagic = newSkillLevel_;
                break;

            case SkillList.DarkMagic:
                characterRef_.charSkills.darkMagic = newSkillLevel_;
                break;

            case SkillList.FireMagic:
                characterRef_.charSkills.fireMagic = newSkillLevel_;
                break;

            case SkillList.WaterMagic:
                characterRef_.charSkills.waterMagic = newSkillLevel_;
                break;

            case SkillList.WindMagic:
                characterRef_.charSkills.windMagic = newSkillLevel_;
                break;

            case SkillList.ElectricMagic:
                characterRef_.charSkills.electricMagic = newSkillLevel_;
                break;

            case SkillList.StoneMagic:
                characterRef_.charSkills.stoneMagic = newSkillLevel_;
                break;



            case SkillList.Survivalist:
                characterRef_.charSkills.survivalist = newSkillLevel_;
                break;

            case SkillList.Social:
                characterRef_.charSkills.social = newSkillLevel_;
                break;
        }
    }
}

//Class used by SkillTome to determine the amount of skill points recieved when used
[System.Serializable]
public class SkillTomeProgression
{
    //The low end of this skill range
    [Range(0, 99)]
    public int skillRangeMin = 0;
    //The high end of this skill range
    [Range(1, 100)]
    public int skillRangeMax = 1;

    [Space(8)]

    //The minimum skill points to allocate in this level range
    public int newSkillMin = 0;
    //The maximum skill points to allocate in this level range
    public int newSkillMax = 1;
    
    //The mathematical curve that is used to determine how many points to give
    public enum progressionCurves
    {
        Linear,
        UpToMax,
        SineIn,
        SineOut
    };
    public progressionCurves progressionCurve = progressionCurves.Linear;
}