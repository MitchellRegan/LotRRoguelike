using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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