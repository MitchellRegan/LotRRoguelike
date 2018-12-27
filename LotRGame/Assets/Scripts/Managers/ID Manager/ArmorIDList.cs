using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorIDList : MonoBehaviour
{
    //The list of armor objects with the IDTag component so we can keep track of their IDs
    public List<IDTag> armorList;



    //Function called externally to return a armor object reference when given the ID number
    public Armor GetArmorByIDNum(int numberID_)
    {
        //Looping through each armor in our list to check for the matching ID number
        for (int a = 0; a < this.armorList.Count; ++a)
        {
            //If the current armor has the same ID number, we get the armor component reference and return it
            if (this.armorList[a].numberID == numberID_)
            {
                return this.armorList[a].GetComponent<Armor>();
            }
        }

        //If we make it through the loop then we don't have the armor and we return null
        return null;
    }


    //Function called for debugging purposes to make sure there are no problems with any of the armor IDs in the list
    public void CheckForInvalidIDs()
    {
        //Looping through all of the armor in our list
        for (int a1 = 0; a1 < this.armorList.Count; ++a1)
        {
            //If this slot in the list is empty, we throw a debug
            if (this.armorList[a1] == null)
            {
                Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: ArmorIDList.CheckForInvalidIDs: Empty slot in armor list at index " + a1);
            }
            //If this ID has the wrong enum tag, we throw a debug
            else if (this.armorList[a1].objType != IDTag.ObjectType.ItemArmor)
            {
                Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: ArmorIDList.CheckForInvalidIDs: Invalid ID type at index " + a1);
            }
            //If this object doesn't have the armor component, we throw a debug
            else if (this.armorList[a1].GetComponent<Armor>() == null)
            {
                Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: ArmorIDList.CheckForInvalidIDs: Object at index " + a1 + " doesn't have the Armor component.");
            }
        }

        //Looping through the list again with nested for loops to check each ID against all other ID numbers
        for (int x = 0; x < this.armorList.Count - 1; ++x)
        {
            for (int y = x + 1; y < this.armorList.Count; ++y)
            {
                //If the ID numbers are the same we need to throw a debug
                if (this.armorList[x].numberID == this.armorList[y].numberID)
                {
                    Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: ArmorIDList.CheckForInvalidIDs: Duplicate ID numbers on index " + x + " and " + y);
                }
            }
        }
    }
}
