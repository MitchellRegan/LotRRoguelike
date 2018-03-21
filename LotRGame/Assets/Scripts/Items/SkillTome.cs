using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTome : Item
{
    //The type of skill to increase
    public SkillList skillToLearn = SkillList.Punching;

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
            case SkillList.Punching:
                return characterRef_.charCombatStats.punching;

            case SkillList.Daggers:
                return characterRef_.charCombatStats.daggers;

            case SkillList.Swords:
                return characterRef_.charCombatStats.swords;

            case SkillList.Axes:
                return characterRef_.charCombatStats.axes;

            case SkillList.Spears:
                return characterRef_.charCombatStats.spears;

            case SkillList.Bows:
                return characterRef_.charCombatStats.bows;

            case SkillList.Improvised:
                return characterRef_.charCombatStats.improvised;

            case SkillList.HolyMagic:
                return characterRef_.charCombatStats.holyMagic;

            case SkillList.DarkMagic:
                return characterRef_.charCombatStats.darkMagic;

            case SkillList.NatureMagic:
                return characterRef_.charCombatStats.natureMagic;

            case SkillList.Cooking:
                return characterRef_.charSkills.cooking;

            case SkillList.Healing:
                return characterRef_.charSkills.healing;

            case SkillList.Crafting:
                return characterRef_.charSkills.crafting;

            case SkillList.Foraging:
                return characterRef_.charSkills.foraging;

            case SkillList.Tracking:
                return characterRef_.charSkills.tracking;

            case SkillList.Fishing:
                return characterRef_.charSkills.fishing;

            case SkillList.Climbing:
                return characterRef_.charSkills.climbing;

            case SkillList.Hiding:
                return characterRef_.charSkills.hiding;

            case SkillList.Swimming:
                return characterRef_.charSkills.swimming;

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
            case SkillList.Punching:
                characterRef_.charCombatStats.punching = newSkillLevel_;
                break;

            case SkillList.Daggers:
                characterRef_.charCombatStats.daggers = newSkillLevel_;
                break;

            case SkillList.Swords:
                characterRef_.charCombatStats.swords = newSkillLevel_;
                break;

            case SkillList.Axes:
                characterRef_.charCombatStats.axes = newSkillLevel_;
                break;

            case SkillList.Spears:
                characterRef_.charCombatStats.spears = newSkillLevel_;
                break;

            case SkillList.Bows:
                characterRef_.charCombatStats.bows = newSkillLevel_;
                break;

            case SkillList.Improvised:
                characterRef_.charCombatStats.improvised = newSkillLevel_;
                break;

            case SkillList.HolyMagic:
                characterRef_.charCombatStats.holyMagic = newSkillLevel_;
                break;

            case SkillList.DarkMagic:
                characterRef_.charCombatStats.darkMagic = newSkillLevel_;
                break;

            case SkillList.NatureMagic:
                characterRef_.charCombatStats.natureMagic = newSkillLevel_;
                break;

            case SkillList.Cooking:
                characterRef_.charSkills.cooking = newSkillLevel_;
                break;

            case SkillList.Healing:
                characterRef_.charSkills.healing = newSkillLevel_;
                break;

            case SkillList.Crafting:
                characterRef_.charSkills.crafting = newSkillLevel_;
                break;

            case SkillList.Foraging:
                characterRef_.charSkills.foraging = newSkillLevel_;
                break;

            case SkillList.Tracking:
                characterRef_.charSkills.tracking = newSkillLevel_;
                break;

            case SkillList.Fishing:
                characterRef_.charSkills.fishing = newSkillLevel_;
                break;

            case SkillList.Climbing:
                characterRef_.charSkills.climbing = newSkillLevel_;
                break;

            case SkillList.Hiding:
                characterRef_.charSkills.hiding = newSkillLevel_;
                break;

            case SkillList.Swimming:
                characterRef_.charSkills.swimming = newSkillLevel_;
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