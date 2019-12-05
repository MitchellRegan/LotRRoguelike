using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class used by Quest to hold Action rewards for completing quests
[System.Serializable]
public class QuestActionReward
{
    //The action that is awarded
    public Action rewardAction;

    public DistributionType rewardDistribution = DistributionType.Everyone;
}