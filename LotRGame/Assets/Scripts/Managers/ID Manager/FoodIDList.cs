using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodIDList : MonoBehaviour
{
    //The list of food objects with the IDTag component so we can keep track of their IDs
    public List<IDTag> foodList;



    //Function called externally to return a food object reference when given the ID number
    public Food GetFiidByIDNum(int numberID_)
    {
        //Looping through each food in our list to check for the matching ID number
        for (int f = 0; f < this.foodList.Count; ++f)
        {
            //If the current food has the same ID number, we get the food component reference and return it
            if (this.foodList[f].numberID == numberID_)
            {
                return this.foodList[f].GetComponent<Food>();
            }
        }

        //If we make it through the loop then we don't have the food and we return null
        return null;
    }


    //Function called for debugging purposes to make sure there are no problems with any of the food IDs in the list
    public void CheckForInvalidIDs()
    {
        //Looping through all of the food in our list
        for (int f1 = 0; f1 < this.foodList.Count; ++f1)
        {
            //If this slot in the list is empty, we throw a debug
            if (this.foodList[f1] == null)
            {
                Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: FoodIDList.CheckForInvalidIDs: Empty slot in food list at index " + f1);
            }
            //If this ID has the wrong enum tag, we throw a debug
            else if (this.foodList[f1].objType != IDTag.ObjectType.ItemConsumable)
            {
                Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: FoodIDList.CheckForInvalidIDs: Invalid ID type at index " + f1);
            }
            //If this object doesn't have the food component, we throw a debug
            else if (this.foodList[f1].GetComponent<Food>() == null)
            {
                Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: FoodIDList.CheckForInvalidIDs: Object at index " + f1 + " doesn't have the Food component.");
            }
        }

        //Looping through the list again with nested for loops to check each ID against all other ID numbers
        for (int x = 0; x < this.foodList.Count - 1; ++x)
        {
            for (int y = x + 1; y < this.foodList.Count; ++y)
            {
                //If the ID numbers are the same we need to throw a debug
                if (this.foodList[x].numberID == this.foodList[y].numberID)
                {
                    Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: FoodIDList.CheckForInvalidIDs: Duplicate ID numbers on index " + x + " and " + y);
                }
            }
        }
    }
}
