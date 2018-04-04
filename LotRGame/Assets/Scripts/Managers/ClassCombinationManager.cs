using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassCombinationManager : MonoBehaviour
{
    //Static reference for this component so players can check for class combination rewards
    public static ClassCombinationManager globalReference;

    //The list of different class combinations
    public List<ClassCombination> combinations;



	// Use this for initialization
	private void Awake ()
    {
        //If the global reference already exists, this component needs to be disabled
        if (globalReference != null)
        {
            this.enabled = false;
        }
        //If there isn't a global reference, this component becomes the global
        else
        {
            globalReference = this;
        }
    }
	

	//Function called from Skills.cs to check for any skill rewards for meeting a class combination level requirement
    public void CheckForClassCombinationRewards(Character characterToCheck_, SkillList skillThatLeveled_)
    {
        //Looping through all of our class combinations to find any that require the skill that leveled
        foreach(ClassCombination combo in this.combinations)
        {
            //If the current class combination uses the leveled skill, we need to check the skill levels
            if(combo.firstSkill == skillThatLeveled_ || combo.secondSkill == skillThatLeveled_)
            {
                //Getting the current skill levels
                int firstSkillLevel = characterToCheck_.charSkills.GetSkillLevelValue(combo.firstSkill);
                int secondSkillLevel = characterToCheck_.charSkills.GetSkillLevelValue(combo.secondSkill);

                //Looping through all of the rewards to see if the character skill levels match
                for(int r = 0; r < combo.rewards.Count; ++r)
                {
                    //The character only gets the reward if both class skills are at the level requirement AND one of them just reached that level
                    if((firstSkillLevel == combo.rewards[r].levelReached && secondSkillLevel >= combo.rewards[r].levelReached) ||
                        (firstSkillLevel >= combo.rewards[r].levelReached && secondSkillLevel == combo.rewards[r].levelReached))
                    {
                        //If the ability reward isn't null
                        if(combo.rewards[r].learnedAction != null)
                        {
                            //If the character's default action list doesn't already have the learned action, we give it to them
                            if(!characterToCheck_.charActionList.defaultActions.Contains(combo.rewards[r].learnedAction))
                            {
                                characterToCheck_.charActionList.defaultActions.Add(combo.rewards[r].learnedAction);
                                //Refreshing the character's action list to make the added action appear
                                characterToCheck_.charActionList.RefreshActionLists();
                            }
                        }

                        //If the perk reward isn't null
                        if(combo.rewards[r].gainedPerk != null)
                        {
                            //If the character's perk list doesn't have the learned perk, we give them the new learned perk
                            if (!characterToCheck_.charPerks.allPerks.Contains(combo.rewards[r].gainedPerk))
                            {
                                characterToCheck_.charPerks.allPerks.Add(combo.rewards[r].gainedPerk);
                            }
                        }
                    }
                }
            }
        }
    }


    //Function called from CheckForClassCombinationRewards to check if the given skills are in a class combination
    private ClassCombination FindClassCombination(SkillList skill1_, SkillList skill2_)
    {
        //Looping through our list of class combinations
        foreach(ClassCombination combo in this.combinations)
        {
            //If the current combination uses both of the given skills, we return it
            if((combo.firstSkill == skill1_ && combo.secondSkill == skill2_) || 
                (combo.firstSkill == skill2_ && combo.secondSkill == skill1_))
            {
                return combo;
            }
        }

        //If we make it past the loop, that means there are no valid class combinations
        return null;
    }
}

//Class used in ClassCombinationManager.cs to determine which classes form a combination and what rewards are given
[System.Serializable]
public class ClassCombination
{
    //The name of this class combination
    public string className;

    //The first skill required for this class
    public SkillList firstSkill = SkillList.Unarmed;
    //The second skill required for this class
    public SkillList secondSkill = SkillList.ArcaneMagic;

    //The list of rewards for this class
    public List<SkillAbilityReward> rewards;
}