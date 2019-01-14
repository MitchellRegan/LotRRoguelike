using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkIDList : MonoBehaviour
{
    //The list of perk objects with the IDTag component so we can keep track of their IDs
    public List<IDTag> damageTypeBoost; //ID 0-99
    public List<IDTag> damageSkillBoost;    //ID 100-199
    public List<IDTag> negateDamageBoost;   //200-299
    public List<IDTag> spellAbsorb;         //300-399
    public List<IDTag> armorBoost;          //400-499
    public List<IDTag> accuracyBoost;       //500-599
    public List<IDTag> evasionBoost;        //600-699



    //Function called externally to return a perk object reference when given the ID number
    public GameObject GetPerkByIDNum(int numberID_)
    {
        //Looping through each damage type boost perk in our list to check for the matching ID number
        for(int dt = 0; dt < this.damageTypeBoost.Count; ++dt)
        {
            //If the current perk has the same ID number, we get the Perk component reference and return it
            if(this.damageTypeBoost[dt].numberID == numberID_)
            {
                return this.damageTypeBoost[dt].gameObject;
            }
        }

        //Looping through each damage skill boost perk in our list to check for the matching ID number
        for (int ds = 0; ds < this.damageSkillBoost.Count; ++ds)
        {
            //If the current perk has the same ID number, we get the Perk component reference and return it
            if (this.damageSkillBoost[ds].numberID == numberID_)
            {
                return this.damageSkillBoost[ds].gameObject;
            }
        }

        //Looping through each negate damage perk in our list to check for the matching ID number
        for (int nd = 0; nd < this.negateDamageBoost.Count; ++nd)
        {
            //If the current perk has the same ID number, we get the Perk component reference and return it
            if (this.negateDamageBoost[nd].numberID == numberID_)
            {
                return this.negateDamageBoost[nd].gameObject;
            }
        }

        //Looping through each spell absorb perk in our list to check for the matching ID number
        for (int sa = 0; sa < this.spellAbsorb.Count; ++sa)
        {
            //If the current perk has the same ID number, we get the Perk component reference and return it
            if (this.spellAbsorb[sa].numberID == numberID_)
            {
                return this.spellAbsorb[sa].gameObject;
            }
        }

        //Looping through each armor boost perk in our list to check for the matching ID number
        for (int am = 0; am < this.armorBoost.Count; ++am)
        {
            //If the current perk has the same ID number, we get the Perk component reference and return it
            if (this.armorBoost[am].numberID == numberID_)
            {
                return this.armorBoost[am].gameObject;
            }
        }

        //Looping through each accuracy perk in our list to check for the matching ID number
        for (int ac = 0; ac < this.accuracyBoost.Count; ++ac)
        {
            //If the current perk has the same ID number, we get the Perk component reference and return it
            if (this.accuracyBoost[ac].numberID == numberID_)
            {
                return this.accuracyBoost[ac].gameObject;
            }
        }

        //Looping through each perk in our list to check for the matching ID number
        for (int ev = 0; ev < this.evasionBoost.Count; ++ev)
        {
            //If the current perk has the same ID number, we get the Perk component reference and return it
            if (this.evasionBoost[ev].numberID == numberID_)
            {
                return this.evasionBoost[ev].gameObject;
            }
        }

        //If we make it through the loop then we don't have the perk and we return null
        Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: PerkIDList.CheckList: NULL ID Number: " + numberID_);
        return null;
    }


    //Function called for debugging purposes to make sure there are no problems with any of the perk IDs in the list
    public void CheckForInvalidIDs()
    {
        this.CheckList(this.damageTypeBoost, "Damage Type Boost");
        this.CheckList(this.damageSkillBoost, "Damage Skill Boost");
        this.CheckList(this.negateDamageBoost, "Negate Damage");
        this.CheckList(this.armorBoost, "Armor Boost");
        this.CheckList(this.accuracyBoost, "Accuracy Boost");
        this.CheckList(this.evasionBoost, "Evasion Boost");
        this.CheckList(this.spellAbsorb, "Spell Absorb");
    }


    //Function called from CheckForInvalidIDs to loop through the given perk list
    private void CheckList(List<IDTag> listToCheck_, string nameOfList_)
    {
        //Looping through all of the perks in our list
        for (int a1 = 0; a1 < listToCheck_.Count; ++a1)
        {
            //If this slot in the list is empty, we throw a debug
            if (listToCheck_[a1] == null)
            {
                Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: PerkIDList.CheckList " + nameOfList_ + ": Empty slot in perk list at index " + a1);
            }
            //If this ID has the wrong enum tag, we throw a debug
            else if (listToCheck_[a1].objType != IDTag.ObjectType.Perk)
            {
                Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: PerkIDList.CheckList " + nameOfList_ + ": Invalid ID type at index " + a1);
            }
            //If this object doesn't have the perk component, we throw a debug
            else if (listToCheck_[a1].GetComponent<Perk>() == null)
            {
                Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: PerkIDList.CheckList " + nameOfList_ + ": Object at index " + a1 + " doesn't have the Perk component.");
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
                    Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: PerkIDList.CheckList " + nameOfList_ + ": Duplicate ID numbers on index " + x + " and " + y);
                }
            }
        }
    }
}