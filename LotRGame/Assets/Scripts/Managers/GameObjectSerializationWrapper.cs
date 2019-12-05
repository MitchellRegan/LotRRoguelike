using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class used in Character.cs and SaveLoadManager.cs as a wrapper to store game object references for serialization
[System.Serializable]
public class GameObjectSerializationWrapper
{
    //Reference to the game object we're going to serialize
    public GameObject objToSave;

    //Constructor for this class
    public GameObjectSerializationWrapper(GameObject obj_)
    {
        this.objToSave = obj_;
    }
}