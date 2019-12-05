using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class used by Quest to hold item rewards for completing quests
[System.Serializable]
public class QuestItemReward
{
    //The prefab for the item that is awarded
    public Item rewardItem;
    //The amount of items that are awarded
    public int amount = 1;
}