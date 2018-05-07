using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActionList : MonoBehaviour
{
    //The list of combat actions that this character has by default
    public List<Action> defaultActions;

    //The full list of major actions this character has
    [HideInInspector]
    [System.NonSerialized]
    public List<Action> majorActions;

    //The full list of minor actions this character has
    [HideInInspector]
    [System.NonSerialized]
    public List<Action> minorActions;

    //The full list of fast actions this character has
    [HideInInspector]
    [System.NonSerialized]
    public List<Action> fastActions;

    //The full list of massive actions this character has
    [HideInInspector]
    [System.NonSerialized]
    public List<Action> massiveActions;

    //The list of all spell actions this character can cast
    [HideInInspector]
    [System.NonSerialized]
    public List<SpellAction> allSpellActions;

    //Dictionary for all spells that are recharging and how many hours are remaining for until they're ready
    [HideInInspector]
    public List<SpellRecharge> rechargingSpells;

    //Delegate void that's tied to the time advance event so we can recharge spells
    private DelegateEvent<EVTData> rechargeSpellsEVT;

    //List of actions that are cooling down
    public List<ActionCooldown> actionCooldowns;



    //Function called when this object is created
    private void Awake()
    {
        //Initializing our lists
        this.majorActions = new List<Action>();
        this.minorActions = new List<Action>();
        this.fastActions = new List<Action>();
        this.massiveActions = new List<Action>();
        this.allSpellActions = new List<SpellAction>();
        this.rechargingSpells = new List<SpellRecharge>();

        //Initializes the Delegate Event for the Event manager
        this.rechargeSpellsEVT = new DelegateEvent<EVTData>(this.RechargeSpells);

        //Initializing the empty list of action cooldowns
        this.actionCooldowns = new List<ActionCooldown>();
    }


    //Function called when this component is enabled to tell the event manager to listen for this delegate event's trigger
    private void OnEnable()
    {
        EventManager.StartListening("Advance Time", this.rechargeSpellsEVT);
    }


    //Function called when this component is disabled to tell the event manager to stop listenening for this delegate event's trigger
    private void OnDisable()
    {
        EventManager.StopListening("Advance Time", this.rechargeSpellsEVT);
    }


    //Function called on the first frame this character exists
    private void Start()
    {
        //Finding all available actions
        this.RefreshActionLists();
    }


    //Clears the current lists so we can find all of the actions that are currently available
    public void RefreshActionLists()
    {
        //Clearing our current lists
        if (this.majorActions == null)
        {
            this.majorActions = new List<Action>();
        }
        else
        {
            this.majorActions.Clear();
        }

        if (this.minorActions == null)
        {
            this.minorActions = new List<Action>();
        }
        else
        {
            this.minorActions.Clear();
        }

        if (this.fastActions == null)
        {
            this.fastActions = new List<Action>();
        }
        else
        {
            this.fastActions.Clear();
        }

        if (this.massiveActions == null)
        {
            this.massiveActions = new List<Action>();
        }
        else
        {
            this.massiveActions.Clear();
        }

        if (this.allSpellActions == null)
        {
            this.allSpellActions = new List<SpellAction>();
        }
        else
        {
            this.allSpellActions.Clear();
        }

        if(this.actionCooldowns == null)
        {
            this.actionCooldowns = new List<ActionCooldown>();
        }

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
            case Action.ActionType.Major:
                this.majorActions.Add(actToAdd_);
                break;
            case Action.ActionType.Minor:
                this.minorActions.Add(actToAdd_);
                break;
            case Action.ActionType.Fast:
                this.fastActions.Add(actToAdd_);
                break;
            case Action.ActionType.Massive:
                this.massiveActions.Add(actToAdd_);
                break;
        }
    }


    //Function called from RefreshActionLists to sort the default actions
    private void SortDefaultActions()
    {
        //Looping through each default action so we can sort it to the correct list
        foreach (Action currentAction in this.defaultActions)
        {
            //Making sure the action isn't currently cooling down
            if (!this.IsActionOnCooldown(currentAction))
            {
                //If the action is a spell, we need to check if it's recharging
                if (currentAction.gameObject.GetComponent<SpellAction>())
                {
                    //If there are more spell charges than recharging, this spell is added to the actions list
                    if (this.CanAddSpellToList(currentAction.gameObject.GetComponent<SpellAction>()))
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
    }


    //Function called from RefreshActionLists to find and sort all actions on currently equipped weapons
    private void SortWeaponActions()
    {
        //Making sure this object has an inventory component first
        if(!this.GetComponent<Inventory>() || !this.GetComponent<Character>())
        {
            return;
        }
        
        Inventory ourInventory = this.GetComponent<Inventory>();

        //Checking the right hand to see if there's a weapon equipped
        if(ourInventory.rightHand != null)
        {
            //Adding each action to our lists
            foreach (Action action in ourInventory.rightHand.specialActionList)
            {
                //If this action isn't cooling down
                if (!this.IsActionOnCooldown(action))
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
        //Checking the left hand to see if there's a weapon equipped
        if(ourInventory.leftHand != null)
        {
            //Adding each action to our lists
            foreach(Action action in ourInventory.leftHand.specialActionList)
            {
                //If this action isn't cooling down
                if (!this.IsActionOnCooldown(action))
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
            //Making sure the action isn't currently cooling down
            if (!this.IsActionOnCooldown(ourInventory.helm.GetComponent<Action>()))
            {
                //Checking to see if the action is a spell action
                if (ourInventory.helm.GetComponent<SpellAction>())
                {
                    //Checking to see if it can be added to the list
                    if (this.CanAddSpellToList(ourInventory.helm.GetComponent<SpellAction>()))
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
        }
        if (ourInventory.chestPiece != null && ourInventory.chestPiece.GetComponent<Action>())
        {
            //Making sure the action isn't currently cooling down
            if (!this.IsActionOnCooldown(ourInventory.chestPiece.GetComponent<Action>()))
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
        }
        if (ourInventory.leggings != null && ourInventory.leggings.GetComponent<Action>())
        {
            //Making sure the action isn't currently cooling down
            if (!this.IsActionOnCooldown(ourInventory.leggings.GetComponent<Action>()))
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
        }
        if (ourInventory.shoes != null && ourInventory.shoes.GetComponent<Action>())
        {
            //Making sure the action isn't currently cooling down
            if (!this.IsActionOnCooldown(ourInventory.shoes.GetComponent<Action>()))
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
        }
        if (ourInventory.gloves != null && ourInventory.gloves.GetComponent<Action>())
        {
            //Making sure the action isn't currently cooling down
            if (!this.IsActionOnCooldown(ourInventory.gloves.GetComponent<Action>()))
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
        }
        if (ourInventory.necklace != null && ourInventory.necklace.GetComponent<Action>())
        {
            //Making sure the action isn't currently cooling down
            if (!this.IsActionOnCooldown(ourInventory.necklace.GetComponent<Action>()))
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
        }
        if (ourInventory.cloak != null && ourInventory.cloak.GetComponent<Action>())
        {
            //Making sure the action isn't currently cooling down
            if (!this.IsActionOnCooldown(ourInventory.cloak.GetComponent<Action>()))
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
        }
        if (ourInventory.ring != null && ourInventory.ring.GetComponent<Action>())
        {
            //Making sure the action isn't currently cooling down
            if (!this.IsActionOnCooldown(ourInventory.ring.GetComponent<Action>()))
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
        foreach (SpellRecharge spell in this.rechargingSpells)
        {
            //If this current spell is the same as the one we're sorting, we add it to the total
            if (spell.spellThatsCharging.actionName == spellToCheck_.actionName)
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
        this.rechargingSpells.Add(new SpellRecharge(spellUsed_, spellUsed_.hoursUntilRecharged));
    }


    //Function called from the time advance delegate
    private void RechargeSpells(EVTData data_)
    {
        //Creating a list of spells that need to be removed when their recharge time is over
        List<SpellRecharge> spellsToRemove = new List<SpellRecharge>();

        //Looping through all spells that are recharging
        foreach(SpellRecharge spell in this.rechargingSpells)
        {
            //Subtracting the amount of time that's passed from the spell's remaining recharge time
            spell.hoursRemaining -= TimePanelUI.globalReference.hoursAdvancedPerUpdate;

            //If the spell is finished recharging, it's added to the list to remove
            if(spell.hoursRemaining <= 0)
            {
                spellsToRemove.Add(spell);
            }
        }

        //Now we loop through all of the spells we need to remove and taking them out of the recharging spell list
        foreach(SpellRecharge finishedSpell in spellsToRemove)
        {
            this.rechargingSpells.Remove(finishedSpell);
        }
    }


    //Function called externally to check if the default action list contains a given action
    public bool DoesDefaultListContainAction(Action actionToCheck_)
    {
        //Looping through all of the actions in our default action list
        foreach(Action ourAct in this.defaultActions)
        {
            //If the action name and description are the same
            if(ourAct.actionName == actionToCheck_.actionName && ourAct.actionDescription == actionToCheck_.actionDescription)
            {
                //If the action is the same type and range
                if(ourAct.type == actionToCheck_.type && ourAct.range == actionToCheck_.range)
                {
                    //We have the same action in our list
                    return true;
                }
            }
        }

        //If we make it out of the loop without finding the action, we return false
        return false;
    }

    
    //Function called externally from ActionList.cs to check if an attack is cooling down
    public bool IsActionOnCooldown(Action actionToCheck_)
    {
        //Looping through each action that's cooling down to see if any are the action we're checking
        foreach (ActionCooldown ac in this.actionCooldowns)
        {
            //Making sure the name strings for the compared actions are the same length
            if (actionToCheck_.actionName.Length == ac.actionCoolingDown.actionName.Length)
            {
                //If the action is the same as one that's cooling down, we return true
                if (actionToCheck_.actionName == ac.actionCoolingDown.actionName)
                {
                    return true;
                }
            }
        }
        
        //If we make it through the loop, that means the action isn't cooling down
        return false;
    }


    //Function called from CombatManager.cs to reduce the cooldown time for any actions that are cooling down
    public void ReduceCooldowns(float timeToReduce_)
    {
        //Looping through all of our cooldown actions
        for (int acd = 0; acd < this.actionCooldowns.Count; ++acd)
        {
            //Reducing the cooldown's remaining time
            this.actionCooldowns[acd].remainingCooldownTime -= timeToReduce_;
            
            //If the remaining time is at 0, we remove it from the cooldowns list
            if (this.actionCooldowns[acd].remainingCooldownTime <= 0)
            {
                this.actionCooldowns.RemoveAt(acd);
                acd -= 1;
            }
        }
    }
}

//Class used in ActionList.cs to handle recharging spells
[System.Serializable]
public class SpellRecharge
{
    //The reference to the spell that's recharging
    public SpellAction spellThatsCharging;
    //The amount of time remaining for this spell to finish recharging
    public int hoursRemaining = 0;

    //Constructor for this class
    public SpellRecharge(SpellAction spell_, int hours_)
    {
        this.spellThatsCharging = spell_;
        this.hoursRemaining = hours_;
    }
}

//Class used in ActionList.cs to track each type of ability this character has used and remaining cooldown time
public class ActionCooldown
{
    //The action that's cooling down
    public Action actionCoolingDown;
    //The remaining time left on the cooldown
    public float remainingCooldownTime = 0;


    //Constructor function for this class
    public ActionCooldown(Action actionCoolingDown_, float cooldownTime_)
    {
        this.actionCoolingDown = actionCoolingDown_;
        this.remainingCooldownTime = cooldownTime_;
    }
}
