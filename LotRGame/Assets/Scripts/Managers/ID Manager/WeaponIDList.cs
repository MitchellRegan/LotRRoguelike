using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponIDList : MonoBehaviour
{
    //The lists of weapon objects with the IDTag component so we can keep track of their IDs
    public List<IDTag> maulList;        //ID 1000-1099
    public List<IDTag> bowList;         //1100-1199
    public List<IDTag> daggerList;      //1200-1299
    public List<IDTag> unarmedList;     //1300-1399
    public List<IDTag> shieldList;      //1400-1499
    public List<IDTag> poleList;        //1500-1599
    public List<IDTag> swordList;       //1600-1699

    [Space(8)]

    public List<IDTag> arcaneList;      //1700-1799
    public List<IDTag> holyList;        //1800-1899
    public List<IDTag> darkList;        //1900-1999

    [Space(8)]

    public List<IDTag> fireList;        //2000-2099
    public List<IDTag> waterList;       //2100-2199
    public List<IDTag> windList;        //2200-2299
    public List<IDTag> electricList;    //2300-2399
    public List<IDTag> stoneList;       //2400-2499



    //Function called externally to return a weapon object reference when given the ID number
    public GameObject GetWeaponByIDNum(int numberID_)
    {
        //Looping through each maul
        for(int m = 0; m < this.maulList.Count; ++m)
        {
            //If the current weapon has the same ID number, we get the weapon component reference and return it
            if(this.maulList[m].numberID == numberID_)
            {
                return this.maulList[m].gameObject;
            }
        }

        //Looping through each bow
        for (int b = 0; b < this.bowList.Count; ++b)
        {
            //If the current weapon has the same ID number, we get the weapon component reference and return it
            if (this.bowList[b].numberID == numberID_)
            {
                return this.bowList[b].gameObject;
            }
        }

        //Looping through each dagger
        for (int d = 0; d < this.daggerList.Count; ++d)
        {
            //If the current weapon has the same ID number, we get the weapon component reference and return it
            if (this.daggerList[d].numberID == numberID_)
            {
                return this.daggerList[d].gameObject;
            }
        }

        //Looping through each punching weapon
        for (int u = 0; u < this.unarmedList.Count; ++u)
        {
            //If the current weapon has the same ID number, we get the weapon component reference and return it
            if (this.unarmedList[u].numberID == numberID_)
            {
                return this.unarmedList[u].gameObject;
            }
        }

        //Looping through each shield
        for (int sh = 0; sh < this.shieldList.Count; ++sh)
        {
            //If the current weapon has the same ID number, we get the weapon component reference and return it
            if (this.shieldList[sh].numberID == numberID_)
            {
                return this.shieldList[sh].gameObject;
            }
        }

        //Looping through each spear
        for (int p = 0; p < this.poleList.Count; ++p)
        {
            //If the current weapon has the same ID number, we get the weapon component reference and return it
            if (this.poleList[p].numberID == numberID_)
            {
                return this.poleList[p].gameObject;
            }
        }

        //Looping through each sword
        for (int sw = 0; sw < this.swordList.Count; ++sw)
        {
            //If the current weapon has the same ID number, we get the weapon component reference and return it
            if (this.swordList[sw].numberID == numberID_)
            {
                return this.swordList[sw].gameObject;
            }
        }



        //Looping through each arcane magic weapon
        for (int amg = 0; amg < this.arcaneList.Count; ++amg)
        {
            //If the current weapon has the same ID number, we get the weapon component reference and return it
            if (this.arcaneList[amg].numberID == numberID_)
            {
                return this.arcaneList[amg].gameObject;
            }
        }

        //Looping through each holy magic weapon
        for (int hmg = 0; hmg < this.holyList.Count; ++hmg)
        {
            //If the current weapon has the same ID number, we get the weapon component reference and return it
            if (this.holyList[hmg].numberID == numberID_)
            {
                return this.holyList[hmg].gameObject;
            }
        }

        //Looping through each dark magic weapon
        for (int dmg = 0; dmg < this.darkList.Count; ++dmg)
        {
            //If the current weapon has the same ID number, we get the weapon component reference and return it
            if (this.darkList[dmg].numberID == numberID_)
            {
                return this.darkList[dmg].gameObject;
            }
        }



        //Looping through each fire magic weapon
        for (int fmg = 0; fmg < this.fireList.Count; ++fmg)
        {
            //If the current weapon has the same ID number, we get the weapon component reference and return it
            if (this.fireList[fmg].numberID == numberID_)
            {
                return this.fireList[fmg].gameObject;
            }
        }

        //Looping through each water magic weapon
        for (int wtmg = 0; wtmg < this.waterList.Count; ++wtmg)
        {
            //If the current weapon has the same ID number, we get the weapon component reference and return it
            if (this.waterList[wtmg].numberID == numberID_)
            {
                return this.waterList[wtmg].gameObject;
            }
        }

        //Looping through each wind magic weapon
        for (int wdmg = 0; wdmg < this.windList.Count; ++wdmg)
        {
            //If the current weapon has the same ID number, we get the weapon component reference and return it
            if (this.windList[wdmg].numberID == numberID_)
            {
                return this.windList[wdmg].gameObject;
            }
        }

        //Looping through each electric magic weapon
        for (int emg = 0; emg < this.electricList.Count; ++emg)
        {
            //If the current weapon has the same ID number, we get the weapon component reference and return it
            if (this.electricList[emg].numberID == numberID_)
            {
                return this.electricList[emg].gameObject;
            }
        }

        //Looping through each stone magic weapon
        for (int smg = 0; smg < this.stoneList.Count; ++smg)
        {
            //If the current weapon has the same ID number, we get the weapon component reference and return it
            if (this.stoneList[smg].numberID == numberID_)
            {
                return this.stoneList[smg].gameObject;
            }
        }


        //If we make it through the loop then we don't have the weapon and we return null
        return null;
    }


    //Function called for debugging purposes to make sure there are no problems with any of the weapon IDs in the list
    public void CheckForInvalidIDs()
    {
        this.CheckList(this.maulList, "Mauls", SkillList.Mauls);
        this.CheckList(this.bowList, "Bows", SkillList.Bows);
        this.CheckList(this.daggerList, "Daggers", SkillList.Daggers);
        this.CheckList(this.unarmedList, "Unarmed", SkillList.Unarmed);
        this.CheckList(this.shieldList, "Shields", SkillList.Shields);
        this.CheckList(this.poleList, "Poles", SkillList.Poles);
        this.CheckList(this.swordList, "Swords", SkillList.Swords);

        this.CheckList(this.arcaneList, "Arcane", SkillList.ArcaneMagic);
        this.CheckList(this.holyList, "Holy", SkillList.HolyMagic);
        this.CheckList(this.darkList, "Dark", SkillList.DarkMagic);

        this.CheckList(this.fireList, "Fire", SkillList.FireMagic);
        this.CheckList(this.waterList, "Water", SkillList.WaterMagic);
        this.CheckList(this.windList, "Wind", SkillList.WindMagic);
        this.CheckList(this.electricList, "Electric", SkillList.ElectricMagic);
        this.CheckList(this.stoneList, "Stone", SkillList.StoneMagic);
    }


    //Function called from CheckForInvalidIDs to loop through the given weapon list
    private void CheckList(List<IDTag> listToCheck_, string nameOfList_, SkillList skillUsed_)
    {
        //Looping through all of the weapons in our list
        for (int w1 = 0; w1 < listToCheck_.Count; ++w1)
        {
            //If this slot in the list is empty, we throw a debug
            if (listToCheck_[w1] == null)
            {
                Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: WeaponIDList.CheckList " + nameOfList_ + ": Empty slot in weapon list at index " + w1);
            }
            //If this ID has the wrong enum tag, we throw a debug
            else if (listToCheck_[w1].objType != IDTag.ObjectType.ItemWeapon)
            {
                Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: WeaponIDList.CheckList " + nameOfList_ + ": Invalid ID type at index " + w1);
            }
            //If this object doesn't have the weapon component, we throw a debug
            else if (listToCheck_[w1].GetComponent<Weapon>() == null)
            {
                Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: WeaponIDList.CheckList " + nameOfList_ + ": Object at index " + w1 + " doesn't have the Weapon component.");
            }
            //If this weapon doesn't use the designated skill, we throw a debug
            else if(listToCheck_[w1].GetComponent<Weapon>().weaponType != skillUsed_)
            {
                Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: WeaponIDList.CheckList " + nameOfList_ + ": Object at index " + w1 + " doesn't use the " + skillUsed_ + " skill.");
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
                    Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: WeaponIDList.CheckList " + nameOfList_ + ": Duplicate ID numbers on index " + x + " and " + y);
                }
            }
        }
    }
}
