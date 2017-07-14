using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionList : MonoBehaviour
{
    //The list of combat actions that this character has by default
    public List<Action> defaultActions;

    //The full list of standard actions this character has
    [HideInInspector]
    public List<Action> standardActions;

    //The full list of secondary actions this character has
    [HideInInspector]
    public List<Action> secondaryActions;

    //The full list of quick actions this character has
    [HideInInspector]
    public List<Action> quickActions;

    //The full list of full-round actions this character has
    [HideInInspector]
    public List<Action> fullRoundActions;



    //Function called on the first frame this character exists
    private void Start()
    {
        //Initializing our lists
        this.standardActions = new List<Action>();
        this.secondaryActions = new List<Action>();
        this.quickActions = new List<Action>();
        this.fullRoundActions = new List<Action>();

        //Finding all available actions
        this.RefreshActionLists();
    }


    //Clears the current lists so we can find all of the actions that are currently available
    public void RefreshActionLists()
    {
        //Clearing our current lists
        this.standardActions.Clear();
        this.secondaryActions.Clear();
        this.quickActions.Clear();
        this.fullRoundActions.Clear();

        //Adding all actions from our default list
        this.SortDefaultActions();
        //Adding all actions from our equipped weapons
        this.SortWeaponActions();
        //Adding all actions from our equipped armor
        this.SortArmorActions();
    }


    //Function called internally to sort the given action to the correct list
    private void AddActionToList(Action actToAdd_)
    {
        //Adding the action to the correct list based on its type
        switch (actToAdd_.type)
        {
            case Action.ActionType.Standard:
                this.standardActions.Add(actToAdd_);
                break;
            case Action.ActionType.Secondary:
                this.secondaryActions.Add(actToAdd_);
                break;
            case Action.ActionType.Quick:
                this.quickActions.Add(actToAdd_);
                break;
            case Action.ActionType.FullRound:
                this.fullRoundActions.Add(actToAdd_);
                break;
        }
    }


    //Function called from RefreshActionLists to sort the default actions
    private void SortDefaultActions()
    {
        //Looping through each default action so we can sort it to the correct list
        foreach (Action currentAction in this.defaultActions)
        {
            this.AddActionToList(currentAction);
        }
    }


    //Function called from RefreshActionLists to find and sort all actions on currently equipped weapons
    private void SortWeaponActions()
    {
        //Making sure this object has an inventory component first
        if(!this.GetComponent<Inventory>())
        {
            return;
        }

        Inventory ourInventory = this.GetComponent<Inventory>();

        //Checking the right hand to see if there's a weapon equipped
        if(ourInventory.rightHand != null)
        {
            //Adding each action to our lists
            foreach (AttackAction action in ourInventory.rightHand.attackList)
            {
                this.AddActionToList(action);
            }
        }
        //Checking the left hand to see if there's a weapon equipped
        if(ourInventory.leftHand != null)
        {
            //Adding each action to our lists
            foreach(AttackAction action in ourInventory.leftHand.attackList)
            {
                this.AddActionToList(action);
            }
        }
    }


    //Function called from RefreshActionLists to find and sort all actions on currently equipped armor pieces
    private void SortArmorActions()
    {
        //Making sure this object has an inventory component first
        if (!this.GetComponent<Inventory>())
        {
            return;
        }

        Inventory ourInventory = this.GetComponent<Inventory>();

        //Searching each slot of equipped armor to see if they have an action 
        if(ourInventory.helm != null && ourInventory.helm.GetComponent<Action>())
        {
            this.AddActionToList(ourInventory.helm.GetComponent<Action>());
        }
        if (ourInventory.chestPiece != null && ourInventory.chestPiece.GetComponent<Action>())
        {
            this.AddActionToList(ourInventory.chestPiece.GetComponent<Action>());
        }
        if (ourInventory.leggings != null && ourInventory.leggings.GetComponent<Action>())
        {
            this.AddActionToList(ourInventory.leggings.GetComponent<Action>());
        }
        if (ourInventory.shoes != null && ourInventory.shoes.GetComponent<Action>())
        {
            this.AddActionToList(ourInventory.shoes.GetComponent<Action>());
        }
        if (ourInventory.gloves != null && ourInventory.gloves.GetComponent<Action>())
        {
            this.AddActionToList(ourInventory.gloves.GetComponent<Action>());
        }
        if (ourInventory.necklace != null && ourInventory.necklace.GetComponent<Action>())
        {
            this.AddActionToList(ourInventory.necklace.GetComponent<Action>());
        }
        if (ourInventory.cloak != null && ourInventory.cloak.GetComponent<Action>())
        {
            this.AddActionToList(ourInventory.cloak.GetComponent<Action>());
        }
        if (ourInventory.ring != null && ourInventory.ring.GetComponent<Action>())
        {
            this.AddActionToList(ourInventory.ring.GetComponent<Action>());
        }
    }
}


