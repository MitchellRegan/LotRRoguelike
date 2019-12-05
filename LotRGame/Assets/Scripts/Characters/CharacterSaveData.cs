using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class used in Character.cs and SaveLoadManager.cs to store all serialized character data
[System.Serializable]
public class CharacterSaveData
{
    //Variables in Character.cs
    public string firstName;
    public string lastName;
    public Genders sex;

    //Variables in RaceTypes.cs
    public Races race;
    public List<Subtypes> subtypeList;

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
    public List<string> stackedItems;

    //Variables in Skills.cs
    public int unarmed = 0;
    public int daggers = 0;
    public int swords = 0;
    public int mauls = 0;
    public int poles = 0;
    public int bows = 0;
    public int shields = 0;
    public int arcaneMagic = 0;
    public int holyMagic = 0;
    public int darkMagic = 0;
    public int fireMagic = 0;
    public int waterMagic = 0;
    public int windMagic = 0;
    public int electricMagic = 0;
    public int stoneMagic = 0;
    public int survivalist = 0;
    public int social = 0;

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
    public HealthCurveTypes startingHealthCurve = HealthCurveTypes.Average;
    public int[] healthCurveLevels = new int[4];
    public float highestHealthPercent;
    public float highestFoodPercent;
    public float highestWaterPercent;
    public float highestSleepPercent;
    public List<float> trackingHealthPercents;
    public List<float> trackingFoodPercents;
    public List<float> trackingWaterPercents;
    public List<float> trackingSleepPercents;

    //Variables in CombatStats.cs
    public int startingCol = 0;
    public int startingRow = 0;
    public int accuracy = 0;
    public int evasion = 10;
    public float currentInitiativeSpeed = 0.01f;
    public List<string> combatEffects;

    //Variables in ActionList.cs
    public List<string> defaultActions;
    public List<string> rechargingSpells;

    //Variables in CharacterSprites.cs
    public CharSpritePackage ourSprites;

    //Variables in PerkList.cs
    public List<string> perkNames;


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
        this.unarmed = characterToSave_.charSkills.GetSkillLevelValue(SkillList.Unarmed);
        this.daggers = characterToSave_.charSkills.GetSkillLevelValue(SkillList.Daggers);
        this.swords = characterToSave_.charSkills.GetSkillLevelValue(SkillList.Swords);
        this.mauls = characterToSave_.charSkills.GetSkillLevelValue(SkillList.Mauls);
        this.poles = characterToSave_.charSkills.GetSkillLevelValue(SkillList.Poles);
        this.bows = characterToSave_.charSkills.GetSkillLevelValue(SkillList.Bows);
        this.shields = characterToSave_.charSkills.GetSkillLevelValue(SkillList.Shields);
        this.arcaneMagic = characterToSave_.charSkills.GetSkillLevelValue(SkillList.ArcaneMagic);
        this.holyMagic = characterToSave_.charSkills.GetSkillLevelValue(SkillList.HolyMagic);
        this.darkMagic = characterToSave_.charSkills.GetSkillLevelValue(SkillList.DarkMagic);
        this.fireMagic = characterToSave_.charSkills.GetSkillLevelValue(SkillList.FireMagic);
        this.waterMagic = characterToSave_.charSkills.GetSkillLevelValue(SkillList.WaterMagic);
        this.windMagic = characterToSave_.charSkills.GetSkillLevelValue(SkillList.WindMagic);
        this.electricMagic = characterToSave_.charSkills.GetSkillLevelValue(SkillList.ElectricMagic);
        this.stoneMagic = characterToSave_.charSkills.GetSkillLevelValue(SkillList.StoneMagic);
        this.survivalist = characterToSave_.charSkills.GetSkillLevelValue(SkillList.Survivalist);
        this.social = characterToSave_.charSkills.GetSkillLevelValue(SkillList.Social);

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
        this.startingHealthCurve = characterToSave_.charPhysState.startingHealthCurve;
        this.healthCurveLevels = characterToSave_.charPhysState.healthCurveLevels;

        this.highestHealthPercent = characterToSave_.charPhysState.highestHealthPercent;
        this.highestFoodPercent = characterToSave_.charPhysState.highestFoodPercent;
        this.highestWaterPercent = characterToSave_.charPhysState.highestWaterPercent;
        this.highestSleepPercent = characterToSave_.charPhysState.highestSleepPercent;

        this.trackingHealthPercents = characterToSave_.charPhysState.trackingHealthPercents;
        this.trackingFoodPercents = characterToSave_.charPhysState.trackingFoodPercents;
        this.trackingWaterPercents = characterToSave_.charPhysState.trackingWaterPercents;
        this.trackingSleepPercents = characterToSave_.charPhysState.trackingSleepPercents;

        //Setting variables from CombatStats.cs
        this.currentInitiativeSpeed = characterToSave_.charCombatStats.currentInitiativeSpeed;
        this.startingCol = characterToSave_.charCombatStats.startingPositionCol;
        this.startingRow = characterToSave_.charCombatStats.startingPositionRow;
        this.accuracy = characterToSave_.charCombatStats.accuracy;
        this.evasion = characterToSave_.charCombatStats.evasion;

        this.combatEffects = new List<string>();
        for (int ce = 0; ce < characterToSave_.charCombatStats.combatEffects.Count; ++ce)
        {
            this.combatEffects.Add(JsonUtility.ToJson(characterToSave_.charCombatStats.combatEffects[ce]));
        }

        //Setting variables from ActionList.cs
        this.defaultActions = new List<string>();
        for (int da = 0; da < characterToSave_.charActionList.defaultActions.Count; ++da)
        {
            PrefabIDTagData actionIDData = new PrefabIDTagData(characterToSave_.charActionList.defaultActions[da].GetComponent<IDTag>());
            this.defaultActions.Add(JsonUtility.ToJson(actionIDData));
        }

        this.rechargingSpells = new List<string>();
        for (int rs = 0; rs < characterToSave_.charActionList.rechargingSpells.Count; ++rs)
        {
            this.rechargingSpells.Add(JsonUtility.ToJson(characterToSave_.charActionList.rechargingSpells[rs]));
        }

        //Setting variables from CharacterSprites.cs
        this.ourSprites = characterToSave_.charSprites.allSprites;

        //Setting all of the equipped object references
        if (characterToSave_.charInventory.helm != null)
        {
            this.helmObj = JsonUtility.ToJson(new PrefabIDTagData(characterToSave_.charInventory.helm.GetComponent<IDTag>()));
        }
        if (characterToSave_.charInventory.chestPiece != null)
        {
            this.chestObj = JsonUtility.ToJson(new PrefabIDTagData(characterToSave_.charInventory.chestPiece.GetComponent<IDTag>()));
        }
        if (characterToSave_.charInventory.leggings != null)
        {
            this.legObj = JsonUtility.ToJson(new PrefabIDTagData(characterToSave_.charInventory.leggings.GetComponent<IDTag>()));
        }
        if (characterToSave_.charInventory.gloves != null)
        {
            this.gloveObj = JsonUtility.ToJson(new PrefabIDTagData(characterToSave_.charInventory.gloves.GetComponent<IDTag>()));
        }
        if (characterToSave_.charInventory.shoes != null)
        {
            this.shoeObj = JsonUtility.ToJson(new PrefabIDTagData(characterToSave_.charInventory.shoes.GetComponent<IDTag>()));
        }
        if (characterToSave_.charInventory.cloak != null)
        {
            this.cloakObj = JsonUtility.ToJson(new PrefabIDTagData(characterToSave_.charInventory.cloak.GetComponent<IDTag>()));
        }
        if (characterToSave_.charInventory.necklace != null)
        {
            this.necklaceObj = JsonUtility.ToJson(new PrefabIDTagData(characterToSave_.charInventory.necklace.GetComponent<IDTag>()));
        }
        if (characterToSave_.charInventory.ring != null)
        {
            this.ringObj = JsonUtility.ToJson(new PrefabIDTagData(characterToSave_.charInventory.ring.GetComponent<IDTag>()));
        }

        if (characterToSave_.charInventory.leftHand != null)
        {
            this.leftHandObj = JsonUtility.ToJson(new PrefabIDTagData(characterToSave_.charInventory.leftHand.GetComponent<IDTag>()));
        }
        if (characterToSave_.charInventory.rightHand != null)
        {
            this.rightHandObj = JsonUtility.ToJson(new PrefabIDTagData(characterToSave_.charInventory.rightHand.GetComponent<IDTag>()));
        }

        //Looping through all of the character inventory items to save their object references
        this.inventorySlots = new List<string>();
        this.stackedItems = new List<string>();
        for (int i = 0; i < characterToSave_.charInventory.itemSlots.Count; ++i)
        {
            //Making sure the current inventory object isn't null
            if (characterToSave_.charInventory.itemSlots[i] != null)
            {
                //Reference to the item's IDTag component
                IDTag itemTag = characterToSave_.charInventory.itemSlots[i].GetComponent<IDTag>();

                //Saving the IDTag info
                this.inventorySlots.Add(JsonUtility.ToJson(new PrefabIDTagData(itemTag)));

                //If the current item is a stack
                if (characterToSave_.charInventory.itemSlots[i].currentStackSize > 1)
                {
                    //Creating a new InventoryItemStackData class to store the item stack
                    InventoryItemStackData stack = new InventoryItemStackData(i, itemTag, characterToSave_.charInventory.itemSlots[i].currentStackSize);
                    //Adding a serialized version of the stack data to our list of stacked items
                    this.stackedItems.Add(JsonUtility.ToJson(stack));
                }
            }
            //If the current item is null, we set a null slot to keep the empty space
            else
            {
                this.inventorySlots.Add("");
            }
        }

        //Looping through all of the character perks to save their object references
        this.perkNames = new List<string>();
        for (int p = 0; p < characterToSave_.charPerks.allPerks.Count; ++p)
        {
            //Making sure the current perk isn't null
            if (characterToSave_.charPerks.allPerks[p] != null)
            {
                //Saving this perk's ID tag info
                this.perkNames.Add(JsonUtility.ToJson(new PrefabIDTagData(characterToSave_.charPerks.allPerks[p].GetComponent<IDTag>())));
            }
        }
    }
}