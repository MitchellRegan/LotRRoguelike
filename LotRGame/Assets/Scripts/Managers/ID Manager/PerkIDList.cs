using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkIDList : MonoBehaviour
{
    //The list of perk objects with the IDTag component so we can keep track of their IDs
    public List<IDTag> perkList;


    
    //Function called externally to return a perk object reference when given the ID number
    public Perk GetPerkByIDNum(int numberID_)
    {
        //Looping through each perk in our list to check for the matching ID number
        for(int p = 0; p < this.perkList.Count; ++p)
        {
            //If the current perk has the same ID number, we get the Perk component reference and return it
            if(this.perkList[p].numberID == numberID_)
            {
                return this.perkList[p].GetComponent<Perk>();
            }
        }

        //If we make it through the loop then we don't have the perk and we return null
        return null;
    }


    //Function called for debugging purposes to make sure there are no problems with any of the perk IDs in the list
    public void CheckForInvalidIDs()
    {
        //Looping through all of the perks in our list
        for(int p1 = 0; p1 < this.perkList.Count; ++p1)
        {
            //If this slot in the list is empty, we throw a debug
            if(this.perkList[p1] == null)
            {
                Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: PerkIDList.CheckForInvalidIDs: Empty slot in perk list at index " + p1);
            }
            //If this ID has the wrong enum tag, we throw a debug
            else if(this.perkList[p1].objType != IDTag.ObjectType.Perk)
            {
                Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: PerkIDList.CheckForInvalidIDs: Invalid ID type at index " + p1);
            }
            //If this object doesn't have the perk component, we throw a debug
            else if(this.perkList[p1].GetComponent<Perk>() == null)
            {
                Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: PerkIDList.CheckForInvalidIDs: Object at index " + p1 + " doesn't have the Perk component.");
            }
        }

        //Looping through the list again with nested for loops to check each ID against all other ID numbers
        for(int x = 0; x < this.perkList.Count - 1; ++x)
        {
            for(int y = x + 1; y < this.perkList.Count; ++y)
            {
                //If the ID numbers are the same we need to throw a debug
                if(this.perkList[x].numberID == this.perkList[y].numberID)
                {
                    Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: PerkIDList.CheckForInvalidIDs: Duplicate ID numbers on index " + x + " and " + y);
                }
            }
        }
    }
}