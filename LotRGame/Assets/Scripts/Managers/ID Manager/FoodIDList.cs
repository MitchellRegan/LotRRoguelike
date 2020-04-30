using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodIDList : MonoBehaviour
{
    //The list of food objects with the IDTag component so we can keep track of their IDs
    public List<IDTag> foodList;        //ID 5000-5099
    public List<IDTag> drinkList;       //5100-5199
    public List<IDTag> potionList;      //5200-5299



    //Function called externally to return a food object reference when given the ID number
    public GameObject GetFoodByIDNum(int numberID_)
    {
        //Looping through each food in our list to check for the matching ID number
        for (int f = 0; f < this.foodList.Count; ++f)
        {
            //If the current food has the same ID number, we get the food component reference and return it
            if (this.foodList[f].numberID == numberID_)
            {
                return this.foodList[f].gameObject;
            }
        }

        //Looping through each drink in our list to check for the matching ID number
        for (int d = 0; d < this.drinkList.Count; ++d)
        {
            //If the current food has the same ID number, we get the food component reference and return it
            if (this.drinkList[d].numberID == numberID_)
            {
                return this.foodList[d].gameObject;
            }
        }

        //Looping through each potion in our list to check for the matching ID number
        for (int p = 0; p < this.potionList.Count; ++p)
        {
            //If the current food has the same ID number, we get the food component reference and return it
            if (this.potionList[p].numberID == numberID_)
            {
                return this.potionList[p].gameObject;
            }
        }

        //If we make it through the loop then we don't have the food and we return null
        Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: FoodIDList.CheckList: NULL ID Number: " + numberID_);
        return null;
    }


    //Function called for debugging purposes to make sure there are no problems with any of the food IDs in the list
    public void CheckForInvalidIDs()
    {
        this.CheckList(this.foodList, "Food");
        this.CheckList(this.drinkList, "Drink");
        this.CheckList(this.potionList, "Potion");
    }


    //Function called from CheckForInvalidIDs to loop through the given food list
    private void CheckList(List<IDTag> listToCheck_, string nameOfList_)
    {
        //Looping through all of the food in our list
        for (int f1 = 0; f1 < listToCheck_.Count; ++f1)
        {
            //If this slot in the list is empty, we throw a debug
            if (listToCheck_[f1] == null)
            {
                Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: FoodIDList.CheckList " + nameOfList_ + ": Empty slot in food list at index " + f1);
            }
            //If this ID has the wrong enum tag, we throw a debug
            else if (listToCheck_[f1].objType != ObjectType.ItemConsumable)
            {
                Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: FoodIDList.CheckList " + nameOfList_ + ": Invalid ID type at index " + f1);
            }
            //If this object doesn't have the food component, we throw a debug
            else if (listToCheck_[f1].GetComponent<Food>() == null)
            {
                Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: FoodIDList.CheckList " + nameOfList_ + ": Object at index " + f1 + " doesn't have the Food component.");
            }
        }

        //Looping through the list again with nested for loops to check each ID against all other ID numbers
        for (int x = 0; x < listToCheck_.Count - 1; ++x)
        {
            for (int y = x + 1; y < listToCheck_.Count; ++y)
            {
                //If the ID numbers are the same we need to throw a debug
                if (listToCheck_[x].numberID == this.foodList[y].numberID)
                {
                    Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: FoodIDList.CheckList " + nameOfList_ + ": Duplicate ID numbers on index " + x + " and " + y);
                }
            }
        }
    }
}
