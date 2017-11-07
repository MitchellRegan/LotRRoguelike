using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterGenerator : MonoBehaviour
{
    /* ~~~~~~ RACE ~~~~~~*/
    public RaceTypes.Races race = RaceTypes.Races.Human;

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
    public Vector2 cooking = new Vector2(10, 50);
    public Vector2 healing = new Vector2(10, 50);
    public Vector2 crafting = new Vector2(10, 50);

    public Vector2 foraging = new Vector2(10, 50);
    public Vector2 tracking = new Vector2(10, 50);
    public Vector2 fishing = new Vector2(10, 50);

    public Vector2 climbing = new Vector2(10, 50);
    public Vector2 hiding = new Vector2(10, 50);
    public Vector2 swimming = new Vector2(10, 50);


    /* ~~~~~~ COMBAT ~~~~~~*/
    public Vector2 punching = new Vector2(10, 50);
    public Vector2 daggers = new Vector2(10, 50);
    public Vector2 swords = new Vector2(10, 50);
    public Vector2 axes = new Vector2(10, 50);
    public Vector2 spears = new Vector2(10, 50);
    public Vector2 bows = new Vector2(10, 50);
    public Vector2 improvised = new Vector2(10, 50);
    public Vector2 holyMagic = new Vector2(10, 50);
    public Vector2 darkMagic = new Vector2(10, 50);
    public Vector2 natureMagic = new Vector2(10, 50);

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

        characterSkillsRef.cooking = Mathf.RoundToInt(Random.Range(this.cooking.x, this.cooking.y));
        characterSkillsRef.healing = Mathf.RoundToInt(Random.Range(this.healing.x, this.healing.y));
        characterSkillsRef.crafting = Mathf.RoundToInt(Random.Range(this.crafting.x, this.crafting.y));

        characterSkillsRef.foraging = Mathf.RoundToInt(Random.Range(this.foraging.x, this.foraging.y));
        characterSkillsRef.tracking = Mathf.RoundToInt(Random.Range(this.tracking.x, this.tracking.y));
        characterSkillsRef.fishing = Mathf.RoundToInt(Random.Range(this.fishing.x, this.fishing.y));

        characterSkillsRef.climbing = Mathf.RoundToInt(Random.Range(this.climbing.x, this.climbing.y));
        characterSkillsRef.hiding = Mathf.RoundToInt(Random.Range(this.hiding.x, this.hiding.y));
        characterSkillsRef.swimming = Mathf.RoundToInt(Random.Range(this.swimming.x, this.swimming.y));
    }


    //Generates this character's Combat Stats
    private void GenerateCombatStats()
    {
        //Getting the reference ot this object's Combat Stats component
        CombatStats characterCombatStatsRef = this.GetComponent<CombatStats>();

        characterCombatStatsRef.punching = Mathf.RoundToInt(Random.Range(this.punching.x, this.punching.y));
        characterCombatStatsRef.daggers = Mathf.RoundToInt(Random.Range(this.daggers.x, this.daggers.y));
        characterCombatStatsRef.swords = Mathf.RoundToInt(Random.Range(this.swords.x, this.swords.y));
        characterCombatStatsRef.axes = Mathf.RoundToInt(Random.Range(this.axes.x, this.axes.y));
        characterCombatStatsRef.spears = Mathf.RoundToInt(Random.Range(this.spears.x, this.spears.y));
        characterCombatStatsRef.bows = Mathf.RoundToInt(Random.Range(this.bows.x, this.bows.y));
        characterCombatStatsRef.improvised = Mathf.RoundToInt(Random.Range(this.improvised.x, this.improvised.y));
        characterCombatStatsRef.holyMagic = Mathf.RoundToInt(Random.Range(this.holyMagic.x, this.holyMagic.y));
        characterCombatStatsRef.darkMagic = Mathf.RoundToInt(Random.Range(this.darkMagic.x, this.darkMagic.y));
        characterCombatStatsRef.natureMagic = Mathf.RoundToInt(Random.Range(this.natureMagic.x, this.natureMagic.y));

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