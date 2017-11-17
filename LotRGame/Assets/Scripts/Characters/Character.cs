using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Enum that determines a character's gender and some stat modifiers
public enum Genders { Male, Female, Genderless };

[RequireComponent(typeof(RaceTypes))]
[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(Skills))]
[RequireComponent(typeof(PhysicalState))]
[RequireComponent(typeof(CombatStats))]
[RequireComponent(typeof(ActionList))]
[RequireComponent(typeof(CharacterSprites))]
[System.Serializable]
public class Character : MonoBehaviour
{
    //This character's first name
    public string firstName = "Generic";
    //This character's last name
    public string lastName = "McPersonface";

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
    }


    //Function called externally from SaveLoadManager.cs to load this character's component data
    public void LoadCharacterFromSave(CharacterSaveData saveData_)
    {
        Debug.Log("LoadCharacterFromSave 1");
        //Setting the Character.cs variables
        this.firstName = saveData_.firstName;
        this.lastName = saveData_.lastName;
        this.sex = saveData_.sex;

        Debug.Log("LoadCharacterFromSave 2");
        //Setting the RaceTypes.cs variables
        this.charRaceTypes.race = saveData_.race;
        this.charRaceTypes.subtypeList = saveData_.subtypeList;

        Debug.Log("LoadCharacterFromSave 3");
        //Setting all of the equipped items in Inventory.cs
        if (saveData_.helmObj != "")
        {
            GameObject itemObj = GameObject.Instantiate(JsonUtility.FromJson(saveData_.helmObj, typeof(GameObject)) as GameObject);
            this.charInventory.helm = itemObj.GetComponent<Armor>();
        }
        if (saveData_.chestObj != "")
        {
            GameObject itemObj = GameObject.Instantiate(JsonUtility.FromJson(saveData_.chestObj, typeof(GameObject)) as GameObject);
            this.charInventory.chestPiece = itemObj.GetComponent<Armor>();
        }
        if (saveData_.legObj != "")
        {
            GameObject itemObj = GameObject.Instantiate(JsonUtility.FromJson(saveData_.legObj, typeof(GameObject)) as GameObject);
            this.charInventory.leggings = itemObj.GetComponent<Armor>();
        }
        if (saveData_.shoeObj != "")
        {
            GameObject itemObj = GameObject.Instantiate(JsonUtility.FromJson(saveData_.shoeObj, typeof(GameObject)) as GameObject);
            this.charInventory.shoes = itemObj.GetComponent<Armor>();
        }
        if (saveData_.gloveObj != "")
        {
            GameObject itemObj = GameObject.Instantiate(JsonUtility.FromJson(saveData_.gloveObj, typeof(GameObject)) as GameObject);
            this.charInventory.gloves = itemObj.GetComponent<Armor>();
        }
        if (saveData_.cloakObj != "")
        {
            GameObject itemObj = GameObject.Instantiate(JsonUtility.FromJson(saveData_.cloakObj, typeof(GameObject)) as GameObject);
            this.charInventory.cloak = itemObj.GetComponent<Armor>();
        }
        if (saveData_.necklaceObj != "")
        {
            GameObject itemObj = GameObject.Instantiate(JsonUtility.FromJson(saveData_.necklaceObj, typeof(GameObject)) as GameObject);
            this.charInventory.necklace = itemObj.GetComponent<Armor>();
        }
        if (saveData_.ringObj != "")
        {
            GameObject itemObj = GameObject.Instantiate(JsonUtility.FromJson(saveData_.ringObj, typeof(GameObject)) as GameObject);
            this.charInventory.ring = itemObj.GetComponent<Armor>();
        }
        if(saveData_.leftHandObj != "")
        {
            GameObject itemObj = GameObject.Instantiate(JsonUtility.FromJson(saveData_.leftHandObj, typeof(GameObject)) as GameObject);
            this.charInventory.leftHand = itemObj.GetComponent<Weapon>();
        }
        if(saveData_.rightHandObj != "")
        {
            GameObject itemObj = GameObject.Instantiate(JsonUtility.FromJson(saveData_.rightHandObj, typeof(GameObject)) as GameObject);
            this.charInventory.rightHand = itemObj.GetComponent<Weapon>();
        }

        Debug.Log("LoadCharacterFromSave 4");
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
                GameObject itemObj = JsonUtility.FromJson(saveData_.inventorySlots[i], typeof(GameObject)) as GameObject;
                this.charInventory.itemSlots.Add(itemObj.GetComponent<Item>());
            }
        }

        Debug.Log("LoadCharacterFromSave 5");
        //Setting the variables in Skill.cs
        this.charSkills.cooking = saveData_.cooking;
        this.charSkills.healing = saveData_.healing;
        this.charSkills.crafting = saveData_.crafting;
        this.charSkills.foraging = saveData_.foraging;
        this.charSkills.tracking = saveData_.tracking;
        this.charSkills.fishing = saveData_.fishing;
        this.charSkills.climbing = saveData_.climbing;
        this.charSkills.hiding = saveData_.hiding;
        this.charSkills.swimming = saveData_.swimming;

        Debug.Log("LoadCharacterFromSave 6");
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
        this.charPhysState.maxEnergy = saveData_.maxEnergy;
        this.charPhysState.currentEnergy = saveData_.currentEnergy;

        Debug.Log("LoadCharacterFromSave 7");
        //Setting the variables in CombatStats.cs
        this.charCombatStats.currentInitiativeSpeed = saveData_.currentInitiativeSpeed;
        this.charCombatStats.startingPositionCol = saveData_.startingCol;
        this.charCombatStats.startingPositionRow = saveData_.startingRow;
        this.charCombatStats.evasion = saveData_.evasion;
        this.charCombatStats.punching = saveData_.punching;
        this.charCombatStats.daggers = saveData_.daggers;
        this.charCombatStats.swords = saveData_.swords;
        this.charCombatStats.axes = saveData_.axes;
        this.charCombatStats.spears = saveData_.spears;
        this.charCombatStats.bows = saveData_.bows;
        this.charCombatStats.improvised = saveData_.improvised;
        this.charCombatStats.holyMagic = saveData_.holyMagic;
        this.charCombatStats.darkMagic = saveData_.darkMagic;
        this.charCombatStats.natureMagic = saveData_.natureMagic;

        this.charCombatStats.combatEffects = new List<Effect>();
        for (int ce = 0; ce < saveData_.combatEffects.Count; ++ce)
        {
            this.charCombatStats.combatEffects.Add(JsonUtility.FromJson(saveData_.combatEffects[ce], typeof(Effect)) as Effect);
        }

        Debug.Log("LoadCharacterFromSave 8");
        //Setting the variables in ActionList.cs
        this.charActionList.defaultActions = new List<Action>();
        for(int da = 0; da < saveData_.defaultActions.Count; ++da)
        {
            GameObjectSerializationWrapper objWrapper = JsonUtility.FromJson(saveData_.defaultActions[da], typeof(GameObjectSerializationWrapper)) as GameObjectSerializationWrapper;
            GameObject actionPrefab = objWrapper.objToSave;
            this.charActionList.defaultActions.Add(actionPrefab.GetComponent<Action>());
        }
        this.charActionList.rechargingSpells = new List<SpellRecharge>();
        for (int rs = 0; rs < saveData_.rechargingSpells.Count; ++rs)
        {
            this.charActionList.rechargingSpells.Add(JsonUtility.FromJson(saveData_.rechargingSpells[rs], typeof(SpellRecharge)) as SpellRecharge);
        }

        Debug.Log("LoadCharacterFromSave 9");
        //Setting the variables in CharacterSprites.cs
        this.charSprites.allSprites = saveData_.ourSprites;
        Debug.Log("LoadCharacterFromSave 10");
    }
}

//Class used in Character.cs and SaveLoadManager.cs to store all serialized character data
[System.Serializable]
public class CharacterSaveData
{
    //Variables in Character.cs
    public string firstName;
    public string lastName;
    public Genders sex;

    //Variables in RaceTypes.cs
    public RaceTypes.Races race;
    public List<RaceTypes.Subtypes> subtypeList;

    //Variables in Inventory.cs
    public string helmObj = "";
    public string chestObj = "";
    public string legObj = "";
    public string gloveObj = "";
    public string shoeObj = "";
    public string cloakObj = "";
    public string necklaceObj = "";
    public string ringObj = "";

    public string leftHandObj = "";
    public string rightHandObj = "";

    public List<string> inventorySlots;

    //Variables in Skills.cs
    public int cooking = 0;
    public int healing = 0;
    public int crafting = 0;
    public int foraging = 0;
    public int tracking = 0;
    public int fishing = 0;
    public int climbing = 0;
    public int hiding = 0;
    public int swimming = 0;

    //Variables in PhysicalState.cs
    public int maxHP = 0;
    public int currentHP = 0;
    public float maxFood = 0;
    public float currentFood = 0;
    public float maxWater = 0;
    public float currentWater = 0;
    public float maxSleep = 0;
    public float currentSleep = 0;
    public bool requireFood = true;
    public bool requireWater = true;
    public bool requireSleep = true;
    public float maxEnergy = 1;
    public float currentEnergy = 1;

    //Variables in CombatStats.cs
    public float currentInitiativeSpeed = 0.01f;
    public int startingCol = 0;
    public int startingRow = 0;
    public int evasion = 10;
    public int punching = 0;
    public int daggers = 0;
    public int swords = 0;
    public int axes = 0;
    public int spears = 0;
    public int bows = 0;
    public int improvised = 0;
    public int holyMagic = 0;
    public int darkMagic = 0;
    public int natureMagic = 0;
    public List<string> combatEffects;

    //Variables in ActionList.cs
    public List<string> defaultActions;
    public List<string> rechargingSpells;

    //Variables in CharacterSprites.cs
    public CharSpritePackage ourSprites;


    //Constructor for this class
    public CharacterSaveData(Character characterToSave_)
    {
        //Setting variables from Character.cs
        this.firstName = characterToSave_.firstName;
        this.lastName = characterToSave_.lastName;
        this.sex = characterToSave_.sex;

        //Setting variables from RaceTypes.cs
        this.race = characterToSave_.charRaceTypes.race;
        this.subtypeList = characterToSave_.charRaceTypes.subtypeList;

        //Setting variables from Skills.cs
        this.cooking = characterToSave_.charSkills.cooking;
        this.healing = characterToSave_.charSkills.healing;
        this.crafting = characterToSave_.charSkills.crafting;
        this.foraging = characterToSave_.charSkills.foraging;
        this.tracking = characterToSave_.charSkills.tracking;
        this.fishing = characterToSave_.charSkills.fishing;
        this.climbing = characterToSave_.charSkills.climbing;
        this.hiding = characterToSave_.charSkills.hiding;
        this.swimming = characterToSave_.charSkills.swimming;

        //Setting variables from PhysicalState.cs
        this.maxHP = characterToSave_.charPhysState.maxHealth;
        this.currentHP = characterToSave_.charPhysState.currentHealth;
        this.maxFood = characterToSave_.charPhysState.maxFood;
        this.currentFood = characterToSave_.charPhysState.currentFood;
        this.maxWater = characterToSave_.charPhysState.maxWater;
        this.currentWater = characterToSave_.charPhysState.currentWater;
        this.maxSleep = characterToSave_.charPhysState.maxSleep;
        this.currentSleep = characterToSave_.charPhysState.currentSleep;
        this.requireFood = characterToSave_.charPhysState.requiresFood;
        this.requireWater = characterToSave_.charPhysState.requiresWater;
        this.requireSleep = characterToSave_.charPhysState.requiresSleep;
        this.maxEnergy = characterToSave_.charPhysState.maxEnergy;
        this.currentEnergy = characterToSave_.charPhysState.currentEnergy;

        //Setting variables from CombatStats.cs
        this.currentInitiativeSpeed = characterToSave_.charCombatStats.currentInitiativeSpeed;
        this.startingCol = characterToSave_.charCombatStats.startingPositionCol;
        this.startingRow = characterToSave_.charCombatStats.startingPositionRow;
        this.evasion = characterToSave_.charCombatStats.evasion;
        this.punching = characterToSave_.charCombatStats.punching;
        this.daggers = characterToSave_.charCombatStats.daggers;
        this.swords = characterToSave_.charCombatStats.swords;
        this.axes = characterToSave_.charCombatStats.axes;
        this.spears = characterToSave_.charCombatStats.spears;
        this.bows = characterToSave_.charCombatStats.bows;
        this.improvised = characterToSave_.charCombatStats.improvised;
        this.holyMagic = characterToSave_.charCombatStats.holyMagic;
        this.darkMagic = characterToSave_.charCombatStats.darkMagic;
        this.natureMagic = characterToSave_.charCombatStats.natureMagic;

        this.combatEffects = new List<string>();
        for(int ce = 0; ce < characterToSave_.charCombatStats.combatEffects.Count; ++ce)
        {
            this.combatEffects.Add(JsonUtility.ToJson(characterToSave_.charCombatStats.combatEffects[ce]));
        }

        //Setting variables from ActionList.cs
        this.defaultActions = new List<string>();
        for(int da = 0; da < characterToSave_.charActionList.defaultActions.Count; ++da)
        {
            GameObjectSerializationWrapper objWrapper = new GameObjectSerializationWrapper(characterToSave_.charActionList.defaultActions[da].gameObject);
            this.defaultActions.Add(JsonUtility.ToJson(objWrapper));
        }

        this.rechargingSpells = new List<string>();
        for(int rs = 0; rs < characterToSave_.charActionList.rechargingSpells.Count; ++rs)
        {
            this.rechargingSpells.Add(JsonUtility.ToJson(characterToSave_.charActionList.rechargingSpells[rs]));
        }

        //Setting variables from CharacterSprites.cs
        this.ourSprites = characterToSave_.charSprites.allSprites;

        //Setting all of the equipped object references
        if (characterToSave_.charInventory.helm != null)
        {
            this.helmObj = JsonUtility.ToJson(characterToSave_.charInventory.helm.gameObject);
        }
        if (characterToSave_.charInventory.chestPiece != null)
        {
            this.chestObj = JsonUtility.ToJson(characterToSave_.charInventory.chestPiece.gameObject);
        }
        if (characterToSave_.charInventory.leggings != null)
        {
            this.legObj = JsonUtility.ToJson(characterToSave_.charInventory.leggings.gameObject);
        }
        if (characterToSave_.charInventory.gloves != null)
        {
            this.gloveObj = JsonUtility.ToJson(characterToSave_.charInventory.gloves.gameObject);
        }
        if (characterToSave_.charInventory.shoes != null)
        {
            this.shoeObj = JsonUtility.ToJson(characterToSave_.charInventory.shoes.gameObject);
        }
        if (characterToSave_.charInventory.cloak != null)
        {
            this.cloakObj = JsonUtility.ToJson(characterToSave_.charInventory.cloak.gameObject);
        }
        if (characterToSave_.charInventory.necklace != null)
        {
            this.necklaceObj = JsonUtility.ToJson(characterToSave_.charInventory.necklace.gameObject);
        }
        if (characterToSave_.charInventory.ring != null)
        {
            this.ringObj = JsonUtility.ToJson(characterToSave_.charInventory.ring.gameObject);
        }

        if (characterToSave_.charInventory.leftHand != null)
        {
            this.leftHandObj = JsonUtility.ToJson(characterToSave_.charInventory.leftHand.gameObject);
        }
        if (characterToSave_.charInventory.rightHand != null)
        {
            this.rightHandObj = JsonUtility.ToJson(characterToSave_.charInventory.rightHand.gameObject);
        }

        //Looping through all of the character inventory items to save their object references
        this.inventorySlots = new List<string>();
        for(int i = 0; i < characterToSave_.charInventory.itemSlots.Count; ++i)
        {
            //Making sure the current inventory object isn't null
            if (characterToSave_.charInventory.itemSlots[i] != null)
            {
                this.inventorySlots.Add(JsonUtility.ToJson(characterToSave_.charInventory.itemSlots[i].gameObject));
            }
            //If the current item is null, we set a null slot to keep the empty space
            else
            {
                this.inventorySlots.Add("");
            }
        }
    }
}


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
