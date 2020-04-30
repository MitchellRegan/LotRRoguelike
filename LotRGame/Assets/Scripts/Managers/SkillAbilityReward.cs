using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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