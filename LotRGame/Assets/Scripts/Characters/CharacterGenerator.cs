using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterGenerator : MonoBehaviour
{
    /* ~~~~~~ RACE ~~~~~~*/
    public Races race = Races.Human;

    /* ~~~~~~ NAMES ~~~~~~*/
    public List<string> firstNames;
    public List<string> lastNames;

    /* ~~~~~~ SEX ~~~~~~*/
    public Genders sex = Genders.Male;

    /* ~~~~~~ PHYSICAL ~~~~~~*/
    public Vector2 health = new Vector2(85, 115);

    public bool requiresFood = true;
    public bool requiresWater = true;
    public bool requiresSleep = true;

    public int daysBeforeStarving = 5;
    public int daysBeforeDehydrated = 3;
    public int daysBeforeFatalInsomnia = 5;


    /* ~~~~~~ SKILLS ~~~~~~*/
    public Vector2 survivalist = new Vector2(10, 50);
    public Vector2 social = new Vector2(10, 50);


    /* ~~~~~~ COMBAT ~~~~~~*/
    public Vector2 unarmed = new Vector2(10, 50);
    public Vector2 daggers = new Vector2(10, 50);
    public Vector2 swords = new Vector2(10, 50);
    public Vector2 mauls = new Vector2(10, 50);
    public Vector2 poles = new Vector2(10, 50);
    public Vector2 bows = new Vector2(10, 50);
    public Vector2 shields = new Vector2(10, 50);

    public Vector2 arcaneMagic = new Vector2(10, 50);
    public Vector2 holyMagic = new Vector2(10, 50);
    public Vector2 darkMagic = new Vector2(10, 50);
    public Vector2 fireMagic = new Vector2(10, 50);
    public Vector2 waterMagic = new Vector2(10, 50);
    public Vector2 windMagic = new Vector2(10, 50);
    public Vector2 electricMagic = new Vector2(10, 50);
    public Vector2 stoneMagic = new Vector2(10, 50);

    public Vector2 initiative = new Vector2(0.005f, 0.015f);

    public Vector2 combatSpeed = new Vector2(2, 5);


    /* ~~~~~~ ARMOR ~~~~~~*/
    public List<Armor> helms;
    public List<Armor> chestPiece;
    public List<Armor> leggings;
    public List<Armor> gloves;
    public List<Armor> shoes;
    public List<Armor> cloak;
    public List<Armor> necklace;
    public List<Armor> ring1;
    public List<Armor> ring2;


    /* ~~~~~~ ITEMS ~~~~~~*/




    //Called on initialization
    private void Awake()
    {
        this.GenerateCharacter();
        this.GeneratePhysicalState();
        this.GenerateSkills();
        this.GenerateCombatStats();
        this.GenerateArmor();

        //Removes this component from the character since we no longer need it
        Destroy(this);
    }


    //Generates this character's Race, Name, Sex, Clan, Height, and Weight
    private void GenerateCharacter()
    {
        //Getting the reference to this object's Character component
        Character characterRef = this.GetComponent<Character>();
        characterRef.charRaceTypes.race = this.race;
        characterRef.sex = this.sex;

        //Gets a random first name from the list of names
        characterRef.firstName = this.firstNames[Random.Range(0, this.firstNames.Count - 1)];

        //Gets a random first name from the list of names
        characterRef.lastName = this.lastNames[Random.Range(0, this.lastNames.Count - 1)];
    }


    //Generates this character's physical stats
    private void GeneratePhysicalState()
    {
        //Getting the reference to this object's PhysicalState component
        PhysicalState characterStateRef = this.GetComponent<PhysicalState>();


        //Gets a random health value using the min and max
        characterStateRef.maxHealth = Mathf.RoundToInt(Random.Range(this.health.x, this.health.y));
        characterStateRef.currentHealth = characterStateRef.maxHealth;

        //Sets the food, water, and sleep requirements
        characterStateRef.requiresFood = this.requiresFood;
        characterStateRef.requiresWater = this.requiresWater;
        characterStateRef.requiresSleep = this.requiresSleep;

        characterStateRef.maxFood = this.daysBeforeStarving;
        characterStateRef.maxWater = this.daysBeforeDehydrated;
        characterStateRef.maxSleep = this.daysBeforeFatalInsomnia;
    }


    //Generates this character's Skill list
    private void GenerateSkills()
    {
        //Getting the reference to this object's Skills component
        Skills characterSkillsRef = this.GetComponent<Skills>();

        characterSkillsRef.GenerateInitialSkillValue(SkillList.Unarmed, this.unarmed.x, this.unarmed.y);
        characterSkillsRef.GenerateInitialSkillValue(SkillList.Daggers, this.unarmed.x, this.unarmed.y);
        characterSkillsRef.GenerateInitialSkillValue(SkillList.Swords, this.unarmed.x, this.unarmed.y);
        characterSkillsRef.GenerateInitialSkillValue(SkillList.Mauls, this.unarmed.x, this.unarmed.y);
        characterSkillsRef.GenerateInitialSkillValue(SkillList.Poles, this.unarmed.x, this.unarmed.y);
        characterSkillsRef.GenerateInitialSkillValue(SkillList.Bows, this.unarmed.x, this.unarmed.y);
        characterSkillsRef.GenerateInitialSkillValue(SkillList.Shields, this.unarmed.x, this.unarmed.y);

        characterSkillsRef.GenerateInitialSkillValue(SkillList.ArcaneMagic, this.unarmed.x, this.unarmed.y);
        characterSkillsRef.GenerateInitialSkillValue(SkillList.HolyMagic, this.unarmed.x, this.unarmed.y);
        characterSkillsRef.GenerateInitialSkillValue(SkillList.DarkMagic, this.unarmed.x, this.unarmed.y);
        characterSkillsRef.GenerateInitialSkillValue(SkillList.FireMagic, this.unarmed.x, this.unarmed.y);
        characterSkillsRef.GenerateInitialSkillValue(SkillList.WaterMagic, this.unarmed.x, this.unarmed.y);
        characterSkillsRef.GenerateInitialSkillValue(SkillList.WindMagic, this.unarmed.x, this.unarmed.y);
        characterSkillsRef.GenerateInitialSkillValue(SkillList.ElectricMagic, this.unarmed.x, this.unarmed.y);
        characterSkillsRef.GenerateInitialSkillValue(SkillList.StoneMagic, this.unarmed.x, this.unarmed.y);

        characterSkillsRef.GenerateInitialSkillValue(SkillList.Survivalist, this.unarmed.x, this.unarmed.y);
        characterSkillsRef.GenerateInitialSkillValue(SkillList.Social, this.unarmed.x, this.unarmed.y);
    }


    //Generates this character's Combat Stats
    private void GenerateCombatStats()
    {
        //Getting the reference ot this object's Combat Stats component
        CombatStats characterCombatStatsRef = this.GetComponent<CombatStats>();
        //Sets the base initiative
        characterCombatStatsRef.currentInitiativeSpeed = Random.Range(this.initiative.x, this.initiative.y);
    }


    //Generates this character's starting armor
    private void GenerateArmor()
    {
        //Getting the reference to this object's Inventory component
        Inventory characterInventoryRef = this.GetComponent<Inventory>();

        if(this.helms.Count > 0)
        {
            characterInventoryRef.helm = this.helms[Random.Range(0, this.helms.Count - 1)];
        }
    }
}