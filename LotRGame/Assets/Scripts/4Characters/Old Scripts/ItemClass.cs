using UnityEngine;
using System.Collections;

public class ItemClass : MonoBehaviour
{
    public string ItemName = "";
    public int ItemWorth = 0;

	// Use this for initialization
	void Start()
    {
        //#################################################################### CONNECT TO TIME PASS EVENT
	}


    //Function called when the "TimePass" event is sent out. Leave blank so that child classes can use it
    virtual public void TimePass()
    {

    }


    //Function called when this item is sold. Returns the worth as money given.
    public int Sell()
    {
        return ItemWorth;
    }
}


public class MoneyItem : ItemClass
{
    public int MoneyAmount;

    //constructor function that sets the amount of money
    public MoneyItem(int moneyAmount_)
    {
        ItemName = "Money";
        MoneyAmount = moneyAmount_;
    }
}


public class FoodItem : ItemClass
{
    private int CurrentWorth = 0;

    //Amount of food this food gives (me english no am good)
    public int FoodAmount;

    //State of the food
    public enum State {Rotten, Stale, Good, Fresh, DoesntSpoil}
    public State FoodState = State.Good;

    //Amount of time until this item is rotten
    public int HoursUntilSpoiled = 0;

    private int CurrentHourCount = 0;


    //constructor function that sets this food's name, worth, food amount, and hours until spoiled
    public FoodItem(string itemName_, int itemWorth_, int foodAmount_, State foodState_, int hoursUntilSpoiled_)
    {
        ItemName = itemName_;
        ItemWorth = itemWorth_;
        FoodState = foodState_;
        FoodAmount = foodAmount_;
        HoursUntilSpoiled = hoursUntilSpoiled_;

        switch(FoodState)
        {
            case State.DoesntSpoil:
                CurrentWorth = ItemWorth;
                break;
            
            //Fresh food is worth more and is more filling
            case State.Fresh:
                CurrentWorth = ItemWorth + (ItemWorth / 10);
                FoodAmount = FoodAmount + (FoodAmount / 10);
                CurrentHourCount = 0;
                break;

            //Good food is just the regular food
            case State.Good:
                CurrentWorth = ItemWorth;
                CurrentHourCount = HoursUntilSpoiled / 10;
                break;

            //Stale food is worth half as much and isn't as filling
            case State.Stale:
                CurrentWorth = ItemWorth / 5;
                FoodAmount = FoodAmount / 5;
                CurrentHourCount = HoursUntilSpoiled / 6;
                break;

            //Rotten food isn't worth much at all and makes the character sick
            case State.Rotten:
                CurrentWorth = 1;
                FoodAmount = -FoodAmount / 4;
                CurrentHourCount = HoursUntilSpoiled;
                break;
        }
    }

    public override void TimePass()
    {
        if(FoodState != State.DoesntSpoil)
        {
            CurrentHourCount += 6;
        }
    }
}


public class DrinkItem : ItemClass
{
    //Amount of thirst this drink gives
    public int DrinkAmount;

    //constructor function that sets the drink's name, worth, and drink amount
    public DrinkItem(string itemName_, int itemWorth_, int drinkAmount_)
    {
        ItemName = itemName_;
        ItemWorth = itemWorth_;
        DrinkAmount = drinkAmount_;
    }
}


public class WeaponItem : ItemClass
{
    public int CurrentWorth = 0;

    //Weapon stat breakdowns
    public enum Weapon { NotAWeapon, Dagger, Sword, Axe, Mace, Spear, Bow, Fist, Magic };
    public Weapon WeaponType = Weapon.NotAWeapon;

    public enum DmgType { NoDamage, Slashing, Bludgeoning, Piercing, Magic };
    public DmgType DamageType = DmgType.NoDamage;

    //Numeric extremes for damage that this weapon can hit for
    private int MinDamage = 0;
    private int MaxDamage = 0;
    public int CurrentMinDamage = 0;
    public int CurrentMaxDamage = 0;

    //Amount of damage the weapon has taken. Affects the damage value if low enough
    public enum State {Broken, Chipped, Worn, Good, Pristine, Indestructible};
    public State WeaponState = State.Good;

    //Chance that this weapon decreases in state when used
    private float FailChance = 0;


    //constructor function that sets this weapon's name, worth, type, damage type, amount of damage dealt, and state
    public WeaponItem(string itemName_, int itemWorth_, Weapon weaponType_, DmgType damageType_, int minDamage_, int maxDamage_, State weaponState_)
    {
        ItemName = itemName_;
        ItemWorth = itemWorth_;
        WeaponType = weaponType_;
        DamageType = damageType_;
        MinDamage = minDamage_;
        MaxDamage = maxDamage_;
        WeaponState = weaponState_;

        //Sets the fail chance based on the weapon's state
        switch(WeaponState)
        {
            case State.Broken:
                FailChance = 0;
                CurrentWorth = 1;
                CurrentMinDamage = 0;
                CurrentMaxDamage = 0;
                break;

            case State.Chipped:
                FailChance = 75;
                CurrentWorth = ItemWorth / 2;
                break;

            case State.Worn:
                FailChance = 90;
                break;

            case State.Good:
                FailChance = 95;
                break;

            case State.Pristine:
                FailChance = 98;
                break;

            case State.Indestructible:
                FailChance = 101;
                break;
        }
    }


    //Function called when a character hits with an attack using this weapon
    public int Attack()
    {
        int damageDealt = Random.Range(MinDamage, MaxDamage);
        CheckState();
        return damageDealt;
    }


    //Function called when this weapon hits something and determines if it breaks
    private void CheckState()
    {
        //random chance from 1-100%. 
        float rand = Random.Range(1, 100);

        //If rand is higher than the fail chance, it fails
        if (rand >= FailChance)
        {
            switch(WeaponState)
            {
                //Chipped becomes Broken. Broken weapons are worthless in combat and can be sold for scrap
                case State.Chipped:
                    WeaponState = State.Broken;
                    FailChance = 0;
                    MinDamage = 0;
                    MaxDamage = 0;
                    ItemWorth = 1;
                    break;
                
                //Worn becomes Chipped. Fail chance, damage, and worth all drop a lot
                case State.Worn:
                    WeaponState = State.Chipped;
                    FailChance = 75;
                    break;
                
                //Good becomes Worn. Fail chance, damage, and worth all drop a bit more
                case State.Good:
                    WeaponState = State.Worn;
                    FailChance = 90;
                    break;
                
                //Pristine becomes Good. Fail chance, damage, and worth all drop slightly
                case State.Pristine:
                    WeaponState = State.Good;
                    FailChance = 95;
                    break;
            }
        }
    }
}


public class ArmorItem : ItemClass
{
    //Armor stat breakdowns
    public enum Armor { NotArmor, Cloth, Leather, ChainMail, PlateMail, Shield };
    public Armor ArmorType = Armor.NotArmor;

    public int ArmorValue = 0;
}

public class ToolItem : ItemClass
{

}

public class JunkItem : ItemClass
{

}

public class QuestItem : ItemClass
{

}
