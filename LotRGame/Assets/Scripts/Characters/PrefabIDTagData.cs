using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class used in Character.cs and SaveLoadManager.cs to store IDTag data
[System.Serializable]
public class PrefabIDTagData
{
    //The enum for what type of object this is
    public ObjectType objType;
    //The int for the object's ID number
    public int iDNumber;

    //Constructor for this class
    public PrefabIDTagData(IDTag objIDTag_)
    {
        this.objType = objIDTag_.objType;
        this.iDNumber = objIDTag_.numberID;
    }
}