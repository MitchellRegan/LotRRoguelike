using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAbilityManager : MonoBehaviour
{
    //Static reference for this component so players can check for rewards
    public static SkillAbilityManager globalReference;

    //The curve that determines the required amount of EXP for each skill level
    public AnimationCurve skillEXPCurve;
    //The amount of EXP required for the first skill level
    public int level0EXPRequired = 100;
    //The amount of EXP required for the last skill level
    public int level99EXPRequired = 200;

    //List of reward abilities for combat skills
    public List<SkillAbilityReward> unarmedAbilities;
    public List<SkillAbilityReward> daggerAbilities;
    public List<SkillAbilityReward> swordAbilities;
    public List<SkillAbilityReward> maulAbilities;
    public List<SkillAbilityReward> poleAbilities;
    public List<SkillAbilityReward> bowAbilities;
    public List<SkillAbilityReward> shieldAbilities;

    //List of reward abilities for magic skills
    public List<SkillAbilityReward> arcaneAbilities;
    public List<SkillAbilityReward> holyAbilities;
    public List<SkillAbilityReward> darkAbilities;
    public List<SkillAbilityReward> fireAbilities;
    public List<SkillAbilityReward> waterAbilities;
    public List<SkillAbilityReward> windAbilities;
    public List<SkillAbilityReward> electricAbilities;
    public List<SkillAbilityReward> stoneAbilities;

    //List of reward abilities for non-combat skills
    public List<SkillAbilityReward> survivalistAbilities;
    public List<SkillAbilityReward> socialAbilities;



    //Function called when this object is created
    private void Awake()
    {
        //If the global reference already exists, this component needs to be disabled
        if(globalReference != null)
        {
            this.enabled = false;
        }
        //If there isn't a global reference, this component becomes the global
        else
        {
            globalReference = this;
        }
    }


    //Function called from Skills.cs to get the required amount of EXP for a given skill to level up
    public int GetEXPRequiredForLevel(int currentSkillLevel_)
    {
        //Percent value of the current skill level
        float percent = (1f * currentSkillLevel_) / 100f;
        //The value along the EXP curve that the percent value is at
        float curveProgress = this.skillEXPCurve.Evaluate(percent);

        //The difference in EXP between the level 0 and level 99 requirements
        int minMaxEXPDiff = this.level99EXPRequired - this.level0EXPRequired;

        //Getting the EXP above the level 0 requirement based on the min/max and the curve progress
        int addedEXP = Mathf.RoundToInt(minMaxEXPDiff * curveProgress);

        return addedEXP + this.level0EXPRequired;
    }


    //Function called externally to check a given character's skills to see if they get any new abilities
    public void CheckCharacterSkillForNewAbility(Character charThatLeveled_, SkillList skillToCheck_)
    {
        //Getting the level that the character's given skill is at
        int currentSkillLevel = this.getCharacterSkillLevel(charThatLeveled_, skillToCheck_);

        //Getting the reference the correct skill ability reward lists
        List<SkillAbilityReward> skillRewards = this.getSkillRewardList(skillToCheck_);
        
        //Looping through all of the skill rewards to see if the character has them
        for(int r = 0; r < skillRewards.Count; ++r)
        {
            //If the current skill reward's level is less than or equal to the character's current skill level, we see if the character already has it
            if(skillRewards[r].levelReached <= currentSkillLevel)
            {
                //If the ability reward isn't null
                if(skillRewards[r].learnedAction != null)
                {
                    //Giving the character the learned action
                    charThatLeveled_.charActionList.defaultActions.Add(skillRewards[r].learnedAction);
                    //Refreshing the character's action list to make the added action appear
                    charThatLeveled_.charActionList.RefreshActionLists();
                }

                //If the perk reward isn't null
                if(skillRewards[r].gainedPerk != null)
                {
                    //If the character's perk list doesn't have the learned perk, we give them the new learned perk
                    if(!charThatLeveled_.charPerks.allPerks.Contains(skillRewards[r].gainedPerk))
                    {
                        charThatLeveled_.charPerks.allPerks.Add(skillRewards[r].gainedPerk);
                    }
                }
            }
        }
    }


    //Function called from CheckCharacterSkillForNewAbility to return the list of SkillLevelRewards for a given skill
    private int getCharacterSkillLevel(Character charThatLeveled_, SkillList skillToCheck_)
    {
        //Switch statement to return the correct list
        switch (skillToCheck_)
        {
            //Combat skills
            case SkillList.Unarmed:
                return charThatLeveled_.charSkills.GetSkillLevelValue(SkillList.Unarmed);

            case SkillList.Daggers:
                return charThatLeveled_.charSkills.GetSkillLevelValue(SkillList.Daggers);

            case SkillList.Swords:
                return charThatLeveled_.charSkills.GetSkillLevelValue(SkillList.Swords);

            case SkillList.Mauls:
                return charThatLeveled_.charSkills.GetSkillLevelValue(SkillList.Mauls);

            case SkillList.Poles:
                return charThatLeveled_.charSkills.GetSkillLevelValue(SkillList.Poles);

            case SkillList.Bows:
                return charThatLeveled_.charSkills.GetSkillLevelValue(SkillList.Bows);

            case SkillList.Shields:
                return charThatLeveled_.charSkills.GetSkillLevelValue(SkillList.Shields);


            //Magic skills
            case SkillList.ArcaneMagic:
                return charThatLeveled_.charSkills.GetSkillLevelValue(SkillList.ArcaneMagic);

            case SkillList.HolyMagic:
                return charThatLeveled_.charSkills.GetSkillLevelValue(SkillList.HolyMagic);

            case SkillList.DarkMagic:
                return charThatLeveled_.charSkills.GetSkillLevelValue(SkillList.DarkMagic);

            case SkillList.FireMagic:
                return charThatLeveled_.charSkills.GetSkillLevelValue(SkillList.FireMagic);

            case SkillList.WaterMagic:
                return charThatLeveled_.charSkills.GetSkillLevelValue(SkillList.WaterMagic);

            case SkillList.WindMagic:
                return charThatLeveled_.charSkills.GetSkillLevelValue(SkillList.WindMagic);

            case SkillList.ElectricMagic:
                return charThatLeveled_.charSkills.GetSkillLevelValue(SkillList.ElectricMagic);

            case SkillList.StoneMagic:
                return charThatLeveled_.charSkills.GetSkillLevelValue(SkillList.StoneMagic);


            //Non-combat skills
            case SkillList.Survivalist:
                return charThatLeveled_.charSkills.GetSkillLevelValue(SkillList.Survivalist);

            case SkillList.Social:
                return charThatLeveled_.charSkills.GetSkillLevelValue(SkillList.Social);

            default:
                return charThatLeveled_.charSkills.GetSkillLevelValue(SkillList.Social);
        }
    }


    //Function called from CheckCharacterSkillForNewAbility to return the list of SkillLevelRewards for a given skill
    private List<SkillAbilityReward> getSkillRewardList(SkillList skillToCheck_)
    {
        //Switch statement to return the correct list
        switch(skillToCheck_)
        {
            //Combat skills
            case SkillList.Unarmed:
                return this.unarmedAbilities;

            case SkillList.Daggers:
                return this.daggerAbilities;

            case SkillList.Swords:
                return this.swordAbilities;

            case SkillList.Mauls:
                return this.maulAbilities;

            case SkillList.Poles:
                return this.poleAbilities;

            case SkillList.Bows:
                return this.bowAbilities;

            case SkillList.Shields:
                return this.shieldAbilities;


            //Magic skills
            case SkillList.ArcaneMagic:
                return this.arcaneAbilities;

            case SkillList.HolyMagic:
                return this.holyAbilities;

            case SkillList.DarkMagic:
                return this.darkAbilities;

            case SkillList.FireMagic:
                return this.fireAbilities;

            case SkillList.WaterMagic:
                return this.waterAbilities;

            case SkillList.WindMagic:
                return this.windAbilities;

            case SkillList.ElectricMagic:
                return this.electricAbilities;

            case SkillList.StoneMagic:
                return this.stoneAbilities;


            //Non-combat skills
            case SkillList.Survivalist:
                return this.survivalistAbilities;

            case SkillList.Social:
                return this.socialAbilities;

            default:
                return this.socialAbilities;
        }
    }
}


//Class used in SkillAbilityManager.cs to store the reward for reaching a particular skill level
[System.Serializable]
public class SkillAbilityReward
{
    //The level that the given player has to reach before they learn this skill ability
    [Range(1, 100)]
    public int levelReached = 50;

    //The action that's learned for reaching the new level
    public Action learnedAction;

    //The perk that's gained for reaching the new level
    public Perk gainedPerk;
}