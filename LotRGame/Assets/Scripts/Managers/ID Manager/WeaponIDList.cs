using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponIDList : MonoBehaviour
{
    //The list of weapon objects with the IDTag component so we can keep track of their IDs
    public List<IDTag> weaponList;



    //Function called externally to return a weapon object reference when given the ID number
    public Weapon GetWeaponByIDNum(int numberID_)
    {
        //Looping through each weapon in our list to check for the matching ID number
        for (int w = 0; w < this.weaponList.Count; ++w)
        {
            //If the current weapon has the same ID number, we get the Weapon component reference and return it
            if (this.weaponList[w].numberID == numberID_)
            {
                return this.weaponList[w].GetComponent<Weapon>();
            }
        }

        //If we make it through the loop then we don't have the weapon and we return null
        return null;
    }


    //Function called for debugging purposes to make sure there are no problems with any of the weapon IDs in the list
    public void CheckForInvalidIDs()
    {
        //Looping through all of the weapons in our list
        for (int w1 = 0; w1 < this.weaponList.Count; ++w1)
        {
            //If this slot in the list is empty, we throw a debug
            if (this.weaponList[w1] == null)
            {
                Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: WeaponIDList.CheckForInvalidIDs: Empty slot in weapon list at index " + w1);
            }
            //If this ID has the wrong enum tag, we throw a debug
            else if (this.weaponList[w1].objType != IDTag.ObjectType.ItemWeapon)
            {
                Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: WeaponIDList.CheckForInvalidIDs: Invalid ID type at index " + w1);
            }
            //If this object doesn't have the weapon component, we throw a debug
            else if (this.weaponList[w1].GetComponent<Weapon>() == null)
            {
                Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: WeaponIDList.CheckForInvalidIDs: Object at index " + w1 + " doesn't have the Weapon component.");
            }
        }

        //Looping through the list again with nested for loops to check each ID against all other ID numbers
        for (int x = 0; x < this.weaponList.Count - 1; ++x)
        {
            for (int y = x + 1; y < this.weaponList.Count; ++y)
            {
                //If the ID numbers are the same we need to throw a debug
                if (this.weaponList[x].numberID == this.weaponList[y].numberID)
                {
                    Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: WeaponIDList.CheckForInvalidIDs: Duplicate ID numbers on index " + x + " and " + y);
                }
            }
        }
    }
}
