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
    //Reference to this object's FoodIDList component
    public FoodIDList foodList;
    //Reference to this object's MiscItemIDList component
    public MiscItemIDList itemList;
    //Reference to this object's LocationIDList component
    public LocationIDList locationList;
    //Reference to this object's ActionIDList component
    public ActionIDList actionList;



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

        Debug.Log("Checking all ID lists. Remove this debug");
        //this.CheckAllLists();
    }


    //Function called externally from SaveLoadManager.cs to get object references based on their IDTag component
    public GameObject GetPrefabFromID(IDTag tagToGet_)
    {
        //Switch statement for the type of objet to get based on the ID enum
        switch(tagToGet_.objType)
        {
            case IDTag.ObjectType.Perk:
                return this.perkList.GetPerkByIDNum(tagToGet_.numberID);

            case IDTag.ObjectType.ItemWeapon:
                return this.weaponList.GetWeaponByIDNum(tagToGet_.numberID);

            case IDTag.ObjectType.ItemArmor:
                return this.armorList.GetArmorByIDNum(tagToGet_.numberID);

            case IDTag.ObjectType.ItemQuest:
                return this.questItemList.GetQuestItemByIDNum(tagToGet_.numberID);

            case IDTag.ObjectType.ItemConsumable:
                return this.foodList.GetFoodByIDNum(tagToGet_.numberID);

            case IDTag.ObjectType.ItemMisc:
                return this.itemList.GetItemByIDNum(tagToGet_.numberID);

            case IDTag.ObjectType.Location:
                return this.locationList.GetLocationByIDNum(tagToGet_.numberID);

            case IDTag.ObjectType.Action:
                return this.actionList.GetActionByIDNum(tagToGet_.numberID);

            default:
                //If for some reason the enum has no match, we return null and let the SaveLoadManager deal with it
                return null;
        }
    }


    //Function called externally from SaveLoadManager.cs to get object references based on the values from the IDTag component if the component isn't available
    public GameObject GetPrefabFromID(IDTag.ObjectType objType_, int numberID_)
    {
        //Switch statement for the type of objet to get based on the ID enum
        switch (objType_)
        {
            case IDTag.ObjectType.Perk:
                return this.perkList.GetPerkByIDNum(numberID_);

            case IDTag.ObjectType.ItemWeapon:
                return this.weaponList.GetWeaponByIDNum(numberID_);

            case IDTag.ObjectType.ItemArmor:
                return this.armorList.GetArmorByIDNum(numberID_);

            case IDTag.ObjectType.ItemQuest:
                return this.questItemList.GetQuestItemByIDNum(numberID_);

            case IDTag.ObjectType.ItemConsumable:
                return this.foodList.GetFoodByIDNum(numberID_);

            case IDTag.ObjectType.ItemMisc:
                return this.itemList.GetItemByIDNum(numberID_);

            case IDTag.ObjectType.Location:
                return this.locationList.GetLocationByIDNum(numberID_);

            case IDTag.ObjectType.Action:
                return this.actionList.GetActionByIDNum(numberID_);

            default:
                //If for some reason the enum has no match, we return null and let the SaveLoadManager deal with it
                return null;
        }
    }


    //Function called on Awake to check all of our ID lists for invalid ID tags
    private void CheckAllLists()
    {
        this.perkList.CheckForInvalidIDs();
        this.weaponList.CheckForInvalidIDs();
        this.armorList.CheckForInvalidIDs();
        this.questItemList.CheckForInvalidIDs();
        this.foodList.CheckForInvalidIDs();
        this.itemList.CheckForInvalidIDs();
        this.locationList.CheckForInvalidIDs();
        this.actionList.CheckForInvalidIDs();
    }
}
