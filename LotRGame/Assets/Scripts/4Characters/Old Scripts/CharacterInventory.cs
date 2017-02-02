using UnityEngine;
using System.Collections;

public class CharacterInventory : MonoBehaviour
{
    public int Money;
    public float CarryWeight;
    //private vector<FoodItems> FoodInventory (FoodItems are sub-class of ItemClass)
    //private vector<DrinkItems> DrinkInventory (DrinkItems are sub-class of ItemClass)
    //private vector<BasicItems> ItemInventory (BasicItems are sub-class of ItemClass)


	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    /*public void PickUpItem(ItemClass pickedUpItem_)
    {
    if(pickedUpItem_.Type == Money)
    {
    Money += pickedUpItem_.Money;
    }
    else if(pickedUpItem_.Type == FoodItems)
    {
    FoodInventory.push(pickedUpItem_);
    }
    else if(pickedUpItem_.Type == DrinkItems)
    {
    DrinkInventory.push(pickedUpItem_);
    }
    else
    {
    ItemInventory.push(pickedUpItem_);
    }
    }*/

    public void EatFood()
    {

    }

    public void Drink()
    {

    }
}
