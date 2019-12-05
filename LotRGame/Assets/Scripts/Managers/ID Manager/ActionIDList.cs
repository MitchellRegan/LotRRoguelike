using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionIDList : MonoBehaviour
{
    //The list of different action types with the IDTag component so we can keep track of their IDs
    public List<IDTag> movementActionList;      //ID 8000-8099

    [Space(8)]

    public List<IDTag> maulActList;             //8100-8199
    public List<IDTag> bowActList;              //8200-8299
    public List<IDTag> daggerActList;           //8300-8399
    public List<IDTag> unarmedActList;          //8400-8499
    public List<IDTag> shieldActList;           //8500-8599
    public List<IDTag> poleActList;             //8600-8699
    public List<IDTag> swordActList;            //8700-8799

    [Space(8)]

    public List<IDTag> arcaneActList;           //8800-8899
    public List<IDTag> holyActList;             //8900-8999
    public List<IDTag> darkActList;             //9000-9099

    [Space(8)]

    public List<IDTag> fireActList;             //9100-9199
    public List<IDTag> waterActList;            //9200-9299
    public List<IDTag> windActList;             //9300-9399
    public List<IDTag> electricActList;         //9400-9499
    public List<IDTag> stoneActList;            //9500-9599

    [Space(8)]

    public List<IDTag> miscActList;             //9600-9699



    //Function called externally to return an action object reference when given the ID number
    public GameObject GetActionByIDNum(int numberID_)
    {
        //Looping through each movement
        for (int mo = 0; mo < this.movementActionList.Count; ++mo)
        {
            //If the current weapon has the same ID number, we get the object reference and return it
            if (this.movementActionList[mo].numberID == numberID_)
            {
                return this.movementActionList[mo].gameObject;
            }
        }



        //Looping through each maul
        for (int ma = 0; ma < this.maulActList.Count; ++ma)
        {
            //If the current weapon has the same ID number, we get the object reference and return it
            if (this.maulActList[ma].numberID == numberID_)
            {
                return this.maulActList[ma].gameObject;
            }
        }

        //Looping through each bow
        for (int bw = 0; bw < this.bowActList.Count; ++bw)
        {
            //If the current weapon has the same ID number, we get the object reference and return it
            if (this.bowActList[bw].numberID == numberID_)
            {
                return this.bowActList[bw].gameObject;
            }
        }

        //Looping through each dagger
        for (int dg = 0; dg < this.daggerActList.Count; ++dg)
        {
            //If the current weapon has the same ID number, we get the object reference and return it
            if (this.daggerActList[dg].numberID == numberID_)
            {
                return this.daggerActList[dg].gameObject;
            }
        }

        //Looping through each unarmed
        for (int ua = 0; ua < this.unarmedActList.Count; ++ua)
        {
            //If the current weapon has the same ID number, we get the object reference and return it
            if (this.unarmedActList[ua].numberID == numberID_)
            {
                return this.unarmedActList[ua].gameObject;
            }
        }

        //Looping through each shield
        for (int sd = 0; sd < this.shieldActList.Count; ++sd)
        {
            //If the current weapon has the same ID number, we get the object reference and return it
            if (this.shieldActList[sd].numberID == numberID_)
            {
                return this.shieldActList[sd].gameObject;
            }
        }

        //Looping through each pole
        for (int pl = 0; pl < this.poleActList.Count; ++pl)
        {
            //If the current weapon has the same ID number, we get the object reference and return it
            if (this.poleActList[pl].numberID == numberID_)
            {
                return this.poleActList[pl].gameObject;
            }
        }

        //Looping through each sword
        for (int sw = 0; sw < this.swordActList.Count; ++sw)
        {
            //If the current weapon has the same ID number, we get the object reference and return it
            if (this.swordActList[sw].numberID == numberID_)
            {
                return this.swordActList[sw].gameObject;
            }
        }



        //Looping through each arcane
        for (int ar = 0; ar < this.arcaneActList.Count; ++ar)
        {
            //If the current weapon has the same ID number, we get the object reference and return it
            if (this.arcaneActList[ar].numberID == numberID_)
            {
                return this.arcaneActList[ar].gameObject;
            }
        }

        //Looping through each holy
        for (int hl = 0; hl < this.holyActList.Count; ++hl)
        {
            //If the current weapon has the same ID number, we get the object reference and return it
            if (this.holyActList[hl].numberID == numberID_)
            {
                return this.holyActList[hl].gameObject;
            }
        }

        //Looping through each dark
        for (int dk = 0; dk < this.darkActList.Count; ++dk)
        {
            //If the current weapon has the same ID number, we get the object reference and return it
            if (this.darkActList[dk].numberID == numberID_)
            {
                return this.darkActList[dk].gameObject;
            }
        }



        //Looping through each fire
        for (int fr = 0; fr < this.fireActList.Count; ++fr)
        {
            //If the current weapon has the same ID number, we get the object reference and return it
            if (this.fireActList[fr].numberID == numberID_)
            {
                return this.fireActList[fr].gameObject;
            }
        }

        //Looping through each water
        for (int wa = 0; wa < this.waterActList.Count; ++wa)
        {
            //If the current weapon has the same ID number, we get the object reference and return it
            if (this.waterActList[wa].numberID == numberID_)
            {
                return this.waterActList[wa].gameObject;
            }
        }

        //Looping through each wind
        for (int wi = 0; wi < this.windActList.Count; ++wi)
        {
            //If the current weapon has the same ID number, we get the object reference and return it
            if (this.windActList[wi].numberID == numberID_)
            {
                return this.windActList[wi].gameObject;
            }
        }

        //Looping through each electric
        for (int el = 0; el < this.electricActList.Count; ++el)
        {
            //If the current weapon has the same ID number, we get the object reference and return it
            if (this.electricActList[el].numberID == numberID_)
            {
                return this.electricActList[el].gameObject;
            }
        }

        //Looping through each stone
        for (int st = 0; st < this.stoneActList.Count; ++st)
        {
            //If the current weapon has the same ID number, we get the object reference and return it
            if (this.stoneActList[st].numberID == numberID_)
            {
                return this.stoneActList[st].gameObject;
            }
        }



        //Looping through each misc
        for (int ms = 0; ms < this.miscActList.Count; ++ms)
        {
            //If the current weapon has the same ID number, we get the object reference and return it
            if (this.miscActList[ms].numberID == numberID_)
            {
                return this.miscActList[ms].gameObject;
            }
        }


        //If we make it through the loop then we don't have the weapon and we return null
        Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: ActionIDList.CheckList: NULL ID Number: " + numberID_);
        return null;
    }


    //Function called for debugging purposes to make sure there are no problems with any of the weapon IDs in the list
    public void CheckForInvalidIDs()
    {
        this.CheckList(this.maulActList, "Mauls", SkillList.Mauls);
        this.CheckList(this.bowActList, "Bows", SkillList.Bows);
        this.CheckList(this.daggerActList, "Daggers", SkillList.Daggers);
        this.CheckList(this.unarmedActList, "Unarmed", SkillList.Unarmed);
        this.CheckList(this.shieldActList, "Shields", SkillList.Shields);
        this.CheckList(this.poleActList, "Poles", SkillList.Poles);
        this.CheckList(this.swordActList, "Swords", SkillList.Swords);

        this.CheckList(this.arcaneActList, "Arcane", SkillList.ArcaneMagic);
        this.CheckList(this.holyActList, "Holy", SkillList.HolyMagic);
        this.CheckList(this.darkActList, "Dark", SkillList.DarkMagic);

        this.CheckList(this.fireActList, "Fire", SkillList.FireMagic);
        this.CheckList(this.waterActList, "Water", SkillList.WaterMagic);
        this.CheckList(this.windActList, "Wind", SkillList.WindMagic);
        this.CheckList(this.electricActList, "Electric", SkillList.ElectricMagic);
        this.CheckList(this.stoneActList, "Stone", SkillList.StoneMagic);

        this.CheckList(this.miscActList, "Misc", SkillList.StoneMagic);
    }


    //Function called from CheckForInvalidIDs to loop through the given action list
    private void CheckList(List<IDTag> listToCheck_, string nameOfList_, SkillList skillUsed_)
    {
        //Looping through all of the actions in our list
        for (int w1 = 0; w1 < listToCheck_.Count; ++w1)
        {
            //If this slot in the list is empty, we throw a debug
            if (listToCheck_[w1] == null)
            {
                Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: ActionIDList.CheckList " + nameOfList_ + ": Empty slot in weapon list at index " + w1);
            }
            //If this ID has the wrong enum tag, we throw a debug
            else if (listToCheck_[w1].objType != ObjectType.Action)
            {
                Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: ActionIDList.CheckList " + nameOfList_ + ": Invalid ID type at index " + w1);
            }
            //If this object doesn't have the Action component, we throw a debug
            else if (listToCheck_[w1].GetComponent<Action>() == null)
            {
                Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: ActionIDList.CheckList " + nameOfList_ + ": Object at index " + w1 + " doesn't have the Action component.");
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
                    Debug.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ERROR: ActionIDList.CheckList " + nameOfList_ + ": Duplicate ID numbers on index " + x + " and " + y);
                }
            }
        }
    }
}
