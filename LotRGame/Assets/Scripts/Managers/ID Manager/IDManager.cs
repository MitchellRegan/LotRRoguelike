using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDManager : MonoBehaviour
{
    //Global reference to this ID Manager
    public static IDManager globalReference;

    //Reference to this object's PerkIDList component
    public PerkIDList perkList;

    //Reference to this object's WeaponIDList component
    public WeaponIDList weaponList;

    //Reference to this object's ArmorIDList compoent
    public ArmorIDList armorList;

    //Reference to this object's QuestItemIDList component
    public QuestItemIDList questItemList;



    //Function called when this object is created
    private void Awake()
    {
        if(globalReference == null)
        {
            globalReference = this;
        }
        else
        {
            Destroy(this);
        }
    }


    //Function called externally from SaveLoadManager.cs to get object references based on their IDTag component
    
}
