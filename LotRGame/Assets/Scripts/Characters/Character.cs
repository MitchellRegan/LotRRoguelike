using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(RaceTypes))]
[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(Skills))]
[RequireComponent(typeof(PhysicalState))]
[RequireComponent(typeof(CombatStats))]
[RequireComponent(typeof(ActionList))]
[RequireComponent(typeof(CharacterSprites))]
[RequireComponent(typeof(PerkList))]
[System.Serializable]
public class Character : MonoBehaviour
{
    //This character's first name
    public string firstName = "";
    //This character's last name
    public string lastName = "";

    //This character's gender
    public Genders sex = Genders.Male;


    //References to each of the required components
    [HideInInspector]
    [System.NonSerialized]
    public RaceTypes charRaceTypes;

    [HideInInspector]
    [System.NonSerialized]
    public Inventory charInventory;

    [HideInInspector]
    [System.NonSerialized]
    public Skills charSkills;

    [HideInInspector]
    [System.NonSerialized]
    public PhysicalState charPhysState;

    [HideInInspector]
    [System.NonSerialized]
    public CombatStats charCombatStats;

    [HideInInspector]
    [System.NonSerialized]
    public ActionList charActionList;

    [HideInInspector]
    [System.NonSerialized]
    public CharacterSprites charSprites;

    [HideInInspector]
    [System.NonSerialized]
    public CharacterModelObjs charModels;

    [System.NonSerialized]
    public PerkList charPerks;


    //Function called when this character is created
    private void Awake()
    {
        //Setting the references to each of the required components
        this.charRaceTypes = this.GetComponent<RaceTypes>();
        this.charInventory = this.GetComponent<Inventory>();
        this.charSkills = this.GetComponent<Skills>();
        this.charPhysState = this.GetComponent<PhysicalState>();
        this.charCombatStats = this.GetComponent<CombatStats>();
        this.charActionList = this.GetComponent<ActionList>();
        this.charSprites = this.GetComponent<CharacterSprites>();
        this.charModels = this.GetComponent<CharacterModelObjs>();
        this.charPerks = this.GetComponent<PerkList>();
    }


    //Function called externally from SaveLoadManager.cs to load this character's component data
    public void LoadCharacterFromSave(CharacterSaveData saveData_)
    {
        //Setting the Character.cs variables
        this.firstName = saveData_.firstName;
        this.lastName = saveData_.lastName;
        this.sex = saveData_.sex;
        
        //Setting the RaceTypes.cs variables
        this.charRaceTypes.race = saveData_.race;
        this.charRaceTypes.subtypeList = saveData_.subtypeList;
        
        //Setting all of the equipped items in Inventory.cs
        if (saveData_.helmObj != "")
        {
            PrefabIDTagData prefabID = JsonUtility.FromJson(saveData_.helmObj, typeof(PrefabIDTagData)) as PrefabIDTagData;
            GameObject itemObj = GameObject.Instantiate(IDManager.globalReference.GetPrefabFromID(prefabID.objType, prefabID.iDNumber));
            
            itemObj.transform.SetParent(this.transform);
            this.charInventory.helm = itemObj.GetComponent<Armor>();
        }
        else
        {
            this.charInventory.helm = null;
        }
        if (saveData_.chestObj != "")
        {
            PrefabIDTagData prefabID = JsonUtility.FromJson(saveData_.chestObj, typeof(PrefabIDTagData)) as PrefabIDTagData;
            GameObject itemObj = GameObject.Instantiate(IDManager.globalReference.GetPrefabFromID(prefabID.objType, prefabID.iDNumber));

            itemObj.transform.SetParent(this.transform);
            this.charInventory.chestPiece = itemObj.GetComponent<Armor>();
        }
        else
        {
            this.charInventory.chestPiece = null;
        }
        if (saveData_.legObj != "")
        {
            PrefabIDTagData prefabID = JsonUtility.FromJson(saveData_.legObj, typeof(PrefabIDTagData)) as PrefabIDTagData;
            GameObject itemObj = GameObject.Instantiate(IDManager.globalReference.GetPrefabFromID(prefabID.objType, prefabID.iDNumber));

            itemObj.transform.SetParent(this.transform);
            this.charInventory.leggings = itemObj.GetComponent<Armor>();
        }
        else
        {
            this.charInventory.leggings = null;
        }
        if (saveData_.shoeObj != "")
        {
            PrefabIDTagData prefabID = JsonUtility.FromJson(saveData_.shoeObj, typeof(PrefabIDTagData)) as PrefabIDTagData;
            GameObject itemObj = GameObject.Instantiate(IDManager.globalReference.GetPrefabFromID(prefabID.objType, prefabID.iDNumber));

            itemObj.transform.SetParent(this.transform);
            this.charInventory.shoes = itemObj.GetComponent<Armor>();
        }
        else
        {
            this.charInventory.shoes = null;
        }
        if (saveData_.gloveObj != "")
        {
            PrefabIDTagData prefabID = JsonUtility.FromJson(saveData_.gloveObj, typeof(PrefabIDTagData)) as PrefabIDTagData;
            GameObject itemObj = GameObject.Instantiate(IDManager.globalReference.GetPrefabFromID(prefabID.objType, prefabID.iDNumber));

            itemObj.transform.SetParent(this.transform);
            this.charInventory.gloves = itemObj.GetComponent<Armor>();
        }
        else
        {
            this.charInventory.gloves = null;
        }
        if (saveData_.cloakObj != "")
        {
            PrefabIDTagData prefabID = JsonUtility.FromJson(saveData_.cloakObj, typeof(PrefabIDTagData)) as PrefabIDTagData;
            GameObject itemObj = GameObject.Instantiate(IDManager.globalReference.GetPrefabFromID(prefabID.objType, prefabID.iDNumber));

            itemObj.transform.SetParent(this.transform);
            this.charInventory.cloak = itemObj.GetComponent<Armor>();
        }
        else
        {
            this.charInventory.cloak = null;
        }
        if (saveData_.necklaceObj != "")
        {
            PrefabIDTagData prefabID = JsonUtility.FromJson(saveData_.necklaceObj, typeof(PrefabIDTagData)) as PrefabIDTagData;
            GameObject itemObj = GameObject.Instantiate(IDManager.globalReference.GetPrefabFromID(prefabID.objType, prefabID.iDNumber));

            itemObj.transform.SetParent(this.transform);
            this.charInventory.necklace = itemObj.GetComponent<Armor>();
        }
        else
        {
            this.charInventory.necklace = null;
        }
        if (saveData_.ringObj != "")
        {
            PrefabIDTagData prefabID = JsonUtility.FromJson(saveData_.ringObj, typeof(PrefabIDTagData)) as PrefabIDTagData;
            GameObject itemObj = GameObject.Instantiate(IDManager.globalReference.GetPrefabFromID(prefabID.objType, prefabID.iDNumber));

            itemObj.transform.SetParent(this.transform);
            this.charInventory.ring = itemObj.GetComponent<Armor>();
        }
        else
        {
            this.charInventory.ring = null;
        }
        if (saveData_.leftHandObj != "")
        {
            PrefabIDTagData prefabID = JsonUtility.FromJson(saveData_.leftHandObj, typeof(PrefabIDTagData)) as PrefabIDTagData;
            GameObject itemObj = GameObject.Instantiate(IDManager.globalReference.GetPrefabFromID(prefabID.objType, prefabID.iDNumber));

            itemObj.transform.SetParent(this.transform);
            this.charInventory.leftHand = itemObj.GetComponent<Weapon>();
        }
        else
        {
            this.charInventory.leftHand = null;
        }
        if (saveData_.rightHandObj != "")
        {
            PrefabIDTagData prefabID = JsonUtility.FromJson(saveData_.rightHandObj, typeof(PrefabIDTagData)) as PrefabIDTagData;
            GameObject itemObj = GameObject.Instantiate(IDManager.globalReference.GetPrefabFromID(prefabID.objType, prefabID.iDNumber));

            itemObj.transform.SetParent(this.transform);
            this.charInventory.rightHand = itemObj.GetComponent<Weapon>();
        }
        else
        {
            this.charInventory.rightHand = null;
        }
        
        //Looping through all of the inventory slot objects in the save data
        this.charInventory.itemSlots = new List<Item>();
        for(int i = 0; i < saveData_.inventorySlots.Count; ++i)
        {
            //If the current item is emtpy, we add an empty slot
            if(saveData_.inventorySlots[i] == "")
            {
                this.charInventory.itemSlots.Add(null);
            }
            //If the current item isn't empty, we add it's item component to our inventory
            else
            {
                PrefabIDTagData prefabID = JsonUtility.FromJson(saveData_.inventorySlots[i], typeof(PrefabIDTagData)) as PrefabIDTagData;
                GameObject itemObj = GameObject.Instantiate(IDManager.globalReference.GetPrefabFromID(prefabID.objType, prefabID.iDNumber));
                
                itemObj.transform.SetParent(this.transform);
                this.charInventory.itemSlots.Add(itemObj.GetComponent<Item>());
            }
        }
        for(int s = 0; s < saveData_.stackedItems.Count; ++s)
        {
            //Making sure the item in this item stack matches the same item in the 
            //Getting the stack data
            InventoryItemStackData stackData = JsonUtility.FromJson(saveData_.stackedItems[s], typeof(InventoryItemStackData)) as InventoryItemStackData;
            GameObject stackedObj = GameObject.Instantiate(IDManager.globalReference.GetPrefabFromID(stackData.objType, stackData.iDNumber));

            //Making sure the stacked object is actually an item
            if (stackedObj.GetComponent<Item>())
            {
                //Making sure the item in this stack matches the item in the designated inventory index
                if (stackData.itemStackIndex < this.charInventory.itemSlots.Count && 
                    this.charInventory.itemSlots[stackData.itemStackIndex].GetComponent<IDTag>().numberID == stackData.iDNumber)
                {
                    //Looping through every item that's in this stack
                    for (int si = 0; si < stackData.numberOfItemsInStack; ++si)
                    {
                        //Creating a new instance of the stacked item
                        GameObject stackedItem = GameObject.Instantiate(IDManager.globalReference.GetPrefabFromID(stackData.objType, stackData.iDNumber));
                        //Parenting the stacked item to the one that's in the inventory slot
                        stackedItem.transform.SetParent(this.charInventory.itemSlots[stackData.itemStackIndex].transform);
                        //Increasing the stack size count in the inventory slot
                        this.charInventory.itemSlots[stackData.itemStackIndex].currentStackSize += 1;

                        //If the inventory slot has reached the max stack size, we stop
                        if(this.charInventory.itemSlots[stackData.itemStackIndex].currentStackSize >= this.charInventory.itemSlots[stackData.itemStackIndex].maxStackSize)
                        {
                            break;
                        }
                    }
                }
            }
        }
        this.charInventory.FindArmorStats();

        //Setting the variables in Skill.cs
        this.charSkills.LoadSkillValue(saveData_);
        
        //Setting the variables in PhysicalState.cs
        this.charPhysState.maxHealth = saveData_.maxHP;
        this.charPhysState.currentHealth = saveData_.currentHP;
        this.charPhysState.requiresFood = saveData_.requireFood;
        this.charPhysState.maxFood = saveData_.maxFood;
        this.charPhysState.currentFood = saveData_.currentFood;
        this.charPhysState.requiresWater = saveData_.requireWater;
        this.charPhysState.maxWater = saveData_.maxWater;
        this.charPhysState.currentWater = saveData_.currentWater;
        this.charPhysState.requiresSleep = saveData_.requireSleep;
        this.charPhysState.maxSleep = saveData_.maxSleep;
        this.charPhysState.currentSleep = saveData_.currentSleep;
        this.charPhysState.startingHealthCurve = saveData_.startingHealthCurve;
        this.charPhysState.healthCurveLevels = saveData_.healthCurveLevels;

        this.charPhysState.highestHealthPercent = saveData_.highestHealthPercent;
        this.charPhysState.highestFoodPercent = saveData_.highestFoodPercent;
        this.charPhysState.highestWaterPercent = saveData_.highestWaterPercent;
        this.charPhysState.highestSleepPercent = saveData_.highestSleepPercent;

        this.charPhysState.trackingHealthPercents = saveData_.trackingHealthPercents;
        this.charPhysState.trackingFoodPercents = saveData_.trackingFoodPercents;
        this.charPhysState.trackingWaterPercents = saveData_.trackingWaterPercents;
        this.charPhysState.trackingSleepPercents = saveData_.trackingSleepPercents;

        //Setting the variables in CombatStats.cs
        this.charCombatStats.currentInitiativeSpeed = saveData_.currentInitiativeSpeed;
        this.charCombatStats.startingPositionCol = saveData_.startingCol;
        this.charCombatStats.startingPositionRow = saveData_.startingRow;
        this.charCombatStats.accuracy = saveData_.accuracy;
        this.charCombatStats.evasion = saveData_.evasion;

        this.charCombatStats.combatEffects = new List<Effect>();
        for (int ce = 0; ce < saveData_.combatEffects.Count; ++ce)
        {
            this.charCombatStats.combatEffects.Add(JsonUtility.FromJson(saveData_.combatEffects[ce], typeof(Effect)) as Effect);
        }
        
        //Setting the variables in ActionList.cs
        this.charActionList.defaultActions = new List<Action>();
        for(int da = 0; da < saveData_.defaultActions.Count; ++da)
        {
            PrefabIDTagData actionData = JsonUtility.FromJson(saveData_.defaultActions[da], typeof(PrefabIDTagData)) as PrefabIDTagData;
            GameObject actionObj = IDManager.globalReference.GetPrefabFromID(actionData.objType, actionData.iDNumber);
            this.charActionList.defaultActions.Add(actionObj.GetComponent<Action>());
        }
        this.charActionList.rechargingSpells = new List<SpellRecharge>();
        for (int rs = 0; rs < saveData_.rechargingSpells.Count; ++rs)
        {
            this.charActionList.rechargingSpells.Add(JsonUtility.FromJson(saveData_.rechargingSpells[rs], typeof(SpellRecharge)) as SpellRecharge);
        }
        
        //Setting the variables in CharacterSprites.cs
        this.charSprites.allSprites = saveData_.ourSprites;

        //Setting the variables in PerkList.cs
        this.charPerks.allPerks = new List<Perk>();
        for(int p = 0; p < saveData_.perkNames.Count; ++p)
        {
            PrefabIDTagData perkTagData = JsonUtility.FromJson(saveData_.perkNames[p], typeof(PrefabIDTagData)) as PrefabIDTagData;
            GameObject loadedPerk = GameObject.Instantiate(IDManager.globalReference.GetPrefabFromID(perkTagData.objType, perkTagData.iDNumber));
            this.charPerks.allPerks.Add(loadedPerk.GetComponent<Perk>());
        }
    }
}


