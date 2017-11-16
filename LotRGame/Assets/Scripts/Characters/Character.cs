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
        .//Need to go through all of the CharacterSaveData and set component variables
        //Since the CharacterSaveData probably didn't serialize completely, we'll probably need a function in Character.cs to handle all of it
    }
}

//Class used in Character.cs and SaveLoadManager.cs to store all serialized character data
[System.Serializable]
public class CharacterSaveData
{
    //Variables in Character.cs
    public Character ourCharacter;

    //Variables in RaceTypes.cs
    public RaceTypes ourRaceTypes;

    //Variables in Inventory.cs
    public GameObject helmObj = null;
    public GameObject chestObj = null;
    public GameObject legObj = null;
    public GameObject gloveObj = null;
    public GameObject shoeObj = null;
    public GameObject cloakObj = null;
    public GameObject necklaceObj = null;
    public GameObject ringObj = null;

    public GameObject leftHandObj = null;
    public GameObject rightHandObj = null;

    public List<GameObject> inventorySlots;


    //Variables in Skills.cs
    public Skills ourSkills;

    //Variables in PhysicalState.cs
    public PhysicalState ourState;

    //Variables in CombatStats.cs
    public CombatStats ourCombatStats;

    //Variables in ActionList.cs
    public ActionList ourActions;

    //Variables in CharacterSprites.cs
    public CharSpritePackage ourSprites;


    //Constructor for this class
    public CharacterSaveData(Character characterToSave_)
    {
        //Setting all of the serializable character component references
        this.ourCharacter = characterToSave_;
        this.ourRaceTypes = characterToSave_.charRaceTypes;
        this.ourSkills = characterToSave_.charSkills;
        this.ourState = characterToSave_.charPhysState;
        this.ourCombatStats = characterToSave_.charCombatStats;
        this.ourActions = characterToSave_.charActionList;
        this.ourSprites = characterToSave_.charSprites.allSprites;

        //Setting all of the equipped object references
        if (characterToSave_.charInventory.helm != null)
        {
            this.helmObj = characterToSave_.charInventory.helm.gameObject;
        }
        if (characterToSave_.charInventory.chestPiece != null)
        {
            this.chestObj = characterToSave_.charInventory.chestPiece.gameObject;
        }
        if (characterToSave_.charInventory.leggings != null)
        {
            this.legObj = characterToSave_.charInventory.leggings.gameObject;
        }
        if (characterToSave_.charInventory.gloves != null)
        {
            this.gloveObj = characterToSave_.charInventory.gloves.gameObject;
        }
        if (characterToSave_.charInventory.shoes != null)
        {
            this.shoeObj = characterToSave_.charInventory.shoes.gameObject;
        }
        if (characterToSave_.charInventory.cloak != null)
        {
            this.cloakObj = characterToSave_.charInventory.cloak.gameObject;
        }
        if (characterToSave_.charInventory.necklace != null)
        {
            this.necklaceObj = characterToSave_.charInventory.necklace.gameObject;
        }
        if (characterToSave_.charInventory.ring != null)
        {
            this.ringObj = characterToSave_.charInventory.ring.gameObject;
        }

        if (characterToSave_.charInventory.leftHand != null)
        {
            this.leftHandObj = characterToSave_.charInventory.leftHand.gameObject;
        }
        if (characterToSave_.charInventory.rightHand != null)
        {
            this.rightHandObj = characterToSave_.charInventory.rightHand.gameObject;
        }

        //Looping through all of the character inventory items to save their object references
        this.inventorySlots = new List<GameObject>();
        for(int i = 0; i < characterToSave_.charInventory.itemSlots.Count; ++i)
        {
            //Making sure the current inventory object isn't null
            if (characterToSave_.charInventory.itemSlots[i] != null)
            {
                this.inventorySlots.Add(characterToSave_.charInventory.itemSlots[i].gameObject);
            }
            //If the current item is null, we set a null slot to keep the empty space
            else
            {
                this.inventorySlots.Add(null);
            }
        }
    }
}