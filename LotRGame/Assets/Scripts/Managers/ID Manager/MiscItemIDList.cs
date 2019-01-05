using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiscItemIDList : MonoBehaviour
{
    //The list of item objects with the IDTag component so we can keep track of their IDs
    public List<IDTag> itemList;    //ID 6000-6999



    //Function called externally to return a item object reference when given the ID number
    public GameObject GetItemByIDNum(int numberID_)
    {
        //Looping through each item in our list to check for the matching ID number
        for (int i = 0; i < this.itemList.Count; ++i)
        {
            //If the current item has the same ID number, we get the item component reference and return it
            if (this.itemList[i].numberID == numberID_)
            {
                return this.itemList[i].gameObject;
            }
        }

        //If we make it through the loop then we don't have the item and we return null
        Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: MiscItemIDList.CheckList: NULL ID Number: " + numberID_);
        return null;
    }


    //Function called for debugging purposes to make sure there are no problems with any of the item IDs in the list
    public void CheckForInvalidIDs()
    {
        //Looping through all of the item in our list
        for (int i1 = 0; i1 < this.itemList.Count; ++i1)
        {
            //If this slot in the list is empty, we throw a debug
            if (this.itemList[i1] == null)
            {
                Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: MiscItemIDList.CheckForInvalidIDs: Empty slot in item list at index " + i1);
            }
            //If this ID has the wrong enum tag, we throw a debug
            else if (this.itemList[i1].objType != IDTag.ObjectType.ItemMisc)
            {
                Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: MiscItemIDList.CheckForInvalidIDs: Invalid ID type at index " + i1);
            }
            //If this object doesn't have the item component, we throw a debug
            else if (this.itemList[i1].GetComponent<Item>() == null)
            {
                Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: MiscItemIDList.CheckForInvalidIDs: Object at index " + i1 + " doesn't have the Food component.");
            }
        }

        //Looping through the list again with nested for loops to check each ID against all other ID numbers
        for (int x = 0; x < this.itemList.Count - 1; ++x)
        {
            for (int y = x + 1; y < this.itemList.Count; ++y)
            {
                //If the ID numbers are the same we need to throw a debug
                if (this.itemList[x].numberID == this.itemList[y].numberID)
                {
                    Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: MiscItemIDList.CheckForInvalidIDs: Duplicate ID numbers on index " + x + " and " + y);
                }
            }
        }
    }
}
