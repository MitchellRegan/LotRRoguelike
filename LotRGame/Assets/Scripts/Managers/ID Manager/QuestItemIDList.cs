using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItemIDList : MonoBehaviour
{
    //The list of quest item objects with the IDTag component so we can keep track of their IDs
    public List<IDTag> questItemList;       //ID 4000-4999



    //Function called externally to return a quest item object reference when given the ID number
    public GameObject GetQuestItemByIDNum(int numberID_)
    {
        //Looping through each quest item in our list to check for the matching ID number
        for (int q = 0; q < this.questItemList.Count; ++q)
        {
            //If the current quest item has the same ID number, we get the quest item component reference and return it
            if (this.questItemList[q].numberID == numberID_)
            {
                return this.questItemList[q].gameObject;
            }
        }

        //If we make it through the loop then we don't have the quest item and we return null
        Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: QuestItemIDList.CheckList: NULL ID Number: " + numberID_);
        return null;
    }


    //Function called for debugging purposes to make sure there are no problems with any of the quest item IDs in the list
    public void CheckForInvalidIDs()
    {
        //Looping through all of the quest item in our list
        for (int q1 = 0; q1 < this.questItemList.Count; ++q1)
        {
            //If this slot in the list is empty, we throw a debug
            if (this.questItemList[q1] == null)
            {
                Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: QuestItemIDList.CheckForInvalidIDs: Empty slot in quest item list at index " + q1);
            }
            //If this ID has the wrong enum tag, we throw a debug
            else if (this.questItemList[q1].objType != ObjectType.ItemQuest)
            {
                Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: QuestItemIDList.CheckForInvalidIDs: Invalid ID type at index " + q1);
            }
            //If this object doesn't have the quest item component, we throw a debug
            else if (this.questItemList[q1].GetComponent<QuestItem>() == null)
            {
                Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: QuestItemIDList.CheckForInvalidIDs: Object at index " + q1 + " doesn't have the QuestItem component.");
            }
        }

        //Looping through the list again with nested for loops to check each ID against all other ID numbers
        for (int x = 0; x < this.questItemList.Count - 1; ++x)
        {
            for (int y = x + 1; y < this.questItemList.Count; ++y)
            {
                //If the ID numbers are the same we need to throw a debug
                if (this.questItemList[x].numberID == this.questItemList[y].numberID)
                {
                    Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: QuestItemIDList.CheckForInvalidIDs: Duplicate ID numbers on index " + x + " and " + y);
                }
            }
        }
    }
}
