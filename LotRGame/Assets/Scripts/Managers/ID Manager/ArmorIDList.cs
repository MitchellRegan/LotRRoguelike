using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorIDList : MonoBehaviour
{
    //The list of armor objects with the IDTag component so we can keep track of their IDs
    public List<IDTag> headList;        //ID 3000-3099
    public List<IDTag> torsoList;       //3100-3199
    public List<IDTag> legsList;        //3200-3299
    public List<IDTag> handsList;       //3300-3399
    public List<IDTag> feetList;        //3400-3499
    public List<IDTag> cloakList;       //3500-3599
    public List<IDTag> necklaceList;    //3600-3699
    public List<IDTag> ringList;        //3700-3799



    //Function called externally to return a armor object reference when given the ID number
    public GameObject GetArmorByIDNum(int numberID_)
    {
        //Looping through each head armor in our list to check for the matching ID number
        for (int hd = 0; hd < this.headList.Count; ++hd)
        {
            //If the current armor has the same ID number, we get the armor component reference and return it
            if (this.headList[hd].numberID == numberID_)
            {
                return this.headList[hd].gameObject;
            }
        }

        //Looping through each torso armor in our list to check for the matching ID number
        for (int t = 0; t < this.torsoList.Count; ++t)
        {
            //If the current armor has the same ID number, we get the armor component reference and return it
            if (this.torsoList[t].numberID == numberID_)
            {
                return this.torsoList[t].gameObject;
            }
        }

        //Looping through each leg armor in our list to check for the matching ID number
        for (int l = 0; l < this.legsList.Count; ++l)
        {
            //If the current armor has the same ID number, we get the armor component reference and return it
            if (this.legsList[l].numberID == numberID_)
            {
                return this.legsList[l].gameObject;
            }
        }

        //Looping through each hand armor in our list to check for the matching ID number
        for (int h = 0; h < this.handsList.Count; ++h)
        {
            //If the current armor has the same ID number, we get the armor component reference and return it
            if (this.handsList[h].numberID == numberID_)
            {
                return this.handsList[h].gameObject;
            }
        }

        //Looping through each feet armor in our list to check for the matching ID number
        for (int f = 0; f < this.feetList.Count; ++f)
        {
            //If the current armor has the same ID number, we get the armor component reference and return it
            if (this.feetList[f].numberID == numberID_)
            {
                return this.feetList[f].gameObject;
            }
        }

        //Looping through each cloak armor in our list to check for the matching ID number
        for (int c = 0; c < this.cloakList.Count; ++c)
        {
            //If the current armor has the same ID number, we get the armor component reference and return it
            if (this.cloakList[c].numberID == numberID_)
            {
                return this.cloakList[c].gameObject;
            }
        }

        //Looping through each necklace armor in our list to check for the matching ID number
        for (int n = 0; n < this.necklaceList.Count; ++n)
        {
            //If the current armor has the same ID number, we get the armor component reference and return it
            if (this.necklaceList[n].numberID == numberID_)
            {
                return this.necklaceList[n].gameObject;
            }
        }

        //Looping through each ring armor in our list to check for the matching ID number
        for (int r = 0; r < this.ringList.Count; ++r)
        {
            //If the current armor has the same ID number, we get the armor component reference and return it
            if (this.ringList[r].numberID == numberID_)
            {
                return this.ringList[r].gameObject;
            }
        }

        //If we make it through the loop then we don't have the armor and we return null
        Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: ArmorIDList.CheckList: NULL ID Number: " + numberID_);
        return null;
    }


    //Function called for debugging purposes to make sure there are no problems with any of the armor IDs in the list
    public void CheckForInvalidIDs()
    {
        this.CheckList(this.headList, "Head", ArmorSlot.Head);
        this.CheckList(this.torsoList, "Torso", ArmorSlot.Torso);
        this.CheckList(this.legsList, "Legs", ArmorSlot.Legs);
        this.CheckList(this.handsList, "Hands", ArmorSlot.Hands);
        this.CheckList(this.feetList, "Feet", ArmorSlot.Feet);
        this.CheckList(this.cloakList, "Cloak", ArmorSlot.Cloak);
        this.CheckList(this.necklaceList, "Necklace", ArmorSlot.Necklace);
        this.CheckList(this.ringList, "Ring", ArmorSlot.Ring);
    }


    //Function called from CheckForInvalidIDs to loop through the given armor list
    private void CheckList(List<IDTag> listToCheck_, string nameOfList_, ArmorSlot armorSlot_)
    {
        //Looping through all of the armor in our list
        for (int a1 = 0; a1 < listToCheck_.Count; ++a1)
        {
            //If this slot in the list is empty, we throw a debug
            if (listToCheck_[a1] == null)
            {
                Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: ArmorIDList.CheckList " + nameOfList_ + ": Empty slot in armor list at index " + a1);
            }
            //If this ID has the wrong enum tag, we throw a debug
            else if (listToCheck_[a1].objType != ObjectType.ItemArmor)
            {
                Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: ArmorIDList.CheckList " + nameOfList_ + ": Invalid ID type at index " + a1);
            }
            //If this object doesn't have the armor component, we throw a debug
            else if (listToCheck_[a1].GetComponent<Armor>() == null)
            {
                Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: ArmorIDList.CheckList " + nameOfList_ + ": Object at index " + a1 + " doesn't have the Armor component.");
            }
            //If this armor doesn't fit in the designated armor slot, we throw a debug
            else if(listToCheck_[a1].GetComponent<Armor>().slot != armorSlot_)
            {
                Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: ArmorIDList.CheckList " + nameOfList_ + ": Object at index " + a1 + " doesn't fit in the " + armorSlot_ + " slot.");
            }
        }

        //Looping through the list again with nested for loops to check each ID against all other ID numbers
        for (int x = 0; x < listToCheck_.Count - 1; ++x)
        {
            for (int y = x + 1; y < listToCheck_.Count; ++y)
            {
                //If the ID numbers are the same we need to throw a debug
                if (listToCheck_[x].numberID == listToCheck_[y].numberID)
                {
                    Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: ArmorIDList.CheckList " + nameOfList_ + ": Duplicate ID numbers on index " + x + " and " + y);
                }
            }
        }
    }
}
