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

    //The list of all spell actions this character can cast
    public List<SpellAction> allSpellActions;

    //Dictionary for all spells that are recharging and how many hours are remaining for until they're ready
    [HideInInspector]
    public Dictionary<SpellAction, int> rechargingSpells;



    //Function called on the first frame this character exists
    private void Start()
    {
        //Initializing our lists
        this.standardActions = new List<Action>();
        this.secondaryActions = new List<Action>();
        this.quickActions = new List<Action>();
        this.fullRoundActions = new List<Action>();
        this.allSpellActions = new List<SpellAction>();

        this.rechargingSpells = new Dictionary<SpellAction, int>();

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
        this.allSpellActions.Clear();

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
            //If the action is a spell, we need to check if it's recharging
            if (currentAction.gameObject.GetComponent<SpellAction>())
            {
                //If there are more spell charges than recharging, this spell is added to the actions list
                if(this.CanAddSpellToList(currentAction.gameObject.GetComponent<SpellAction>()))
                {
                    this.AddActionToList(currentAction);
                }

                //Adding this spell to the list of all spell actions
                this.allSpellActions.Add(currentAction.gameObject.GetComponent<SpellAction>());
            }
            //If it's not a spell, it's added to the action list normally
            else
            {
                this.AddActionToList(currentAction);
            }
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
                //If the action is a spell, we need to check if it's recharging
                if (action.gameObject.GetComponent<SpellAction>())
                {
                    //If there are more spell charges than recharging, this spell is added to the actions list
                    if (this.CanAddSpellToList(action.gameObject.GetComponent<SpellAction>()))
                    {
                        this.AddActionToList(action);
                    }

                    //Adding this spell to the list of all spell actions
                    this.allSpellActions.Add(action.gameObject.GetComponent<SpellAction>());
                }
                //If it's not a spell, it's added to the action list normally
                else
                {
                    this.AddActionToList(action);
                }
            }
        }
        //Checking the left hand to see if there's a weapon equipped
        if(ourInventory.leftHand != null)
        {
            //Adding each action to our lists
            foreach(AttackAction action in ourInventory.leftHand.attackList)
            {
                //If the action is a spell, we need to check if it's recharging
                if (action.gameObject.GetComponent<SpellAction>())
                {
                    //If there are more spell charges than recharging, this spell is added to the actions list
                    if (this.CanAddSpellToList(action.gameObject.GetComponent<SpellAction>()))
                    {
                        this.AddActionToList(action);
                    }

                    //Adding this spell to the list of all spell actions
                    this.allSpellActions.Add(action.gameObject.GetComponent<SpellAction>());
                }
                //If it's not a spell, it's added to the action list normally
                else
                {
                    this.AddActionToList(action);
                }
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
            //Checking to see if the action is a spell action
            if (ourInventory.helm.GetComponent<SpellAction>())
            {
                //Checking to see if it can be added to the list
                if(this.CanAddSpellToList(ourInventory.helm.GetComponent<SpellAction>()))
                {
                    this.AddActionToList(ourInventory.helm.GetComponent<Action>());
                }

                //Adding the spell to the list of all spell actions
                this.allSpellActions.Add(ourInventory.helm.GetComponent<SpellAction>());
            }
            //If not a spell, it's added normally
            else
            {
                this.AddActionToList(ourInventory.helm.GetComponent<Action>());
            }
        }
        if (ourInventory.chestPiece != null && ourInventory.chestPiece.GetComponent<Action>())
        {
            //Checking to see if the action is a spell action
            if (ourInventory.chestPiece.GetComponent<SpellAction>())
            {
                //Checking to see if it can be added to the list
                if (this.CanAddSpellToList(ourInventory.chestPiece.GetComponent<SpellAction>()))
                {
                    this.AddActionToList(ourInventory.chestPiece.GetComponent<Action>());
                }

                //Adding the spell to the list of all spell actions
                this.allSpellActions.Add(ourInventory.chestPiece.GetComponent<SpellAction>());
            }
            //If not a spell, it's added normally
            else
            {
                this.AddActionToList(ourInventory.chestPiece.GetComponent<Action>());
            }
        }
        if (ourInventory.leggings != null && ourInventory.leggings.GetComponent<Action>())
        {
            //Checking to see if the action is a spell action
            if (ourInventory.leggings.GetComponent<SpellAction>())
            {
                //Checking to see if it can be added to the list
                if (this.CanAddSpellToList(ourInventory.leggings.GetComponent<SpellAction>()))
                {
                    this.AddActionToList(ourInventory.leggings.GetComponent<Action>());
                }

                //Adding the spell to the list of all spell actions
                this.allSpellActions.Add(ourInventory.leggings.GetComponent<SpellAction>());
            }
            //If not a spell, it's added normally
            else
            {
                this.AddActionToList(ourInventory.leggings.GetComponent<Action>());
            }
        }
        if (ourInventory.shoes != null && ourInventory.shoes.GetComponent<Action>())
        {
            //Checking to see if the action is a spell action
            if (ourInventory.shoes.GetComponent<SpellAction>())
            {
                //Checking to see if it can be added to the list
                if (this.CanAddSpellToList(ourInventory.shoes.GetComponent<SpellAction>()))
                {
                    this.AddActionToList(ourInventory.shoes.GetComponent<Action>());
                }

                //Adding the spell to the list of all spell actions
                this.allSpellActions.Add(ourInventory.shoes.GetComponent<SpellAction>());
            }
            //If not a spell, it's added normally
            else
            {
                this.AddActionToList(ourInventory.shoes.GetComponent<Action>());
            }
        }
        if (ourInventory.gloves != null && ourInventory.gloves.GetComponent<Action>())
        {
            //Checking to see if the action is a spell action
            if (ourInventory.gloves.GetComponent<SpellAction>())
            {
                //Checking to see if it can be added to the list
                if (this.CanAddSpellToList(ourInventory.gloves.GetComponent<SpellAction>()))
                {
                    this.AddActionToList(ourInventory.gloves.GetComponent<Action>());
                }

                //Adding the spell to the list of all spell actions
                this.allSpellActions.Add(ourInventory.gloves.GetComponent<SpellAction>());
            }
            //If not a spell, it's added normally
            else
            {
                this.AddActionToList(ourInventory.gloves.GetComponent<Action>());
            }
        }
        if (ourInventory.necklace != null && ourInventory.necklace.GetComponent<Action>())
        {
            //Checking to see if the action is a spell action
            if (ourInventory.necklace.GetComponent<SpellAction>())
            {
                //Checking to see if it can be added to the list
                if (this.CanAddSpellToList(ourInventory.necklace.GetComponent<SpellAction>()))
                {
                    this.AddActionToList(ourInventory.necklace.GetComponent<Action>());
                }

                //Adding the spell to the list of all spell actions
                this.allSpellActions.Add(ourInventory.necklace.GetComponent<SpellAction>());
            }
            //If not a spell, it's added normally
            else
            {
                this.AddActionToList(ourInventory.necklace.GetComponent<Action>());
            }
        }
        if (ourInventory.cloak != null && ourInventory.cloak.GetComponent<Action>())
        {
            //Checking to see if the action is a spell action
            if (ourInventory.cloak.GetComponent<SpellAction>())
            {
                //Checking to see if it can be added to the list
                if (this.CanAddSpellToList(ourInventory.cloak.GetComponent<SpellAction>()))
                {
                    this.AddActionToList(ourInventory.cloak.GetComponent<Action>());
                }

                //Adding the spell to the list of all spell actions
                this.allSpellActions.Add(ourInventory.cloak.GetComponent<SpellAction>());
            }
            //If not a spell, it's added normally
            else
            {
                this.AddActionToList(ourInventory.cloak.GetComponent<Action>());
            }
        }
        if (ourInventory.ring != null && ourInventory.ring.GetComponent<Action>())
        {
            //Checking to see if the action is a spell action
            if (ourInventory.ring.GetComponent<SpellAction>())
            {
                //Checking to see if it can be added to the list
                if (this.CanAddSpellToList(ourInventory.ring.GetComponent<SpellAction>()))
                {
                    this.AddActionToList(ourInventory.ring.GetComponent<Action>());
                }

                //Adding the spell to the list of all spell actions
                this.allSpellActions.Add(ourInventory.ring.GetComponent<SpellAction>());
            }
            //If not a spell, it's added normally
            else
            {
                this.AddActionToList(ourInventory.ring.GetComponent<Action>());
            }
        }
    }


    //Function called from the Sort X Actions functions to check and see if there are more spells recharging than we're able to add
    private bool CanAddSpellToList(SpellAction spellToCheck_)
    {
        //Int to track how many charges of this spell are in the spell actions list. Starts at the number of charges for the current spell
        int spellCharges = spellToCheck_.spellCharges;
        //Looping through all of the spells currently in the spell actions list
        foreach (SpellAction spell in this.allSpellActions)
        {
            //If this current spell is the same as the one we're sorting, we add the number of charges to the current total
            if (spell.actionName == spellToCheck_.actionName)
            {
                spellCharges += spell.spellCharges;
            }
        }

        //Int to track how many charges of this spell are currently recharging
        int spellsRecharging = 0;
        //Looping through all of the recharging spells to see if any match the one we're sorting
        foreach (SpellAction spell in this.rechargingSpells.Keys)
        {
            //If this current spell is the same as the one we're sorting, we add it to the total
            if (spell.actionName == spellToCheck_.actionName)
            {
                spellsRecharging += 1;
            }
        }

        //If there are more spell charges than recharging, this spell can be added to the actions list
        if (spellsRecharging < spellCharges)
        {
            return true;
        }
        //If there are more spells recharging than available, we shouldn't add it to the action list
        else
        {
            return false;
        }
    }


    //Function called from SpellAction.cs when a spell is used
    public void StartSpellRecharge(SpellAction spellUsed_)
    {
        this.rechargingSpells.Add(spellUsed_, spellUsed_.hoursUntilRecharged);
    }
}


