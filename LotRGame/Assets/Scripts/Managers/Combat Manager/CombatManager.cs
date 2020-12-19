using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(CombatTileHandler))]
[RequireComponent(typeof(CombatInitiativeHandler))]
[RequireComponent(typeof(CombatCharacterHandler))]
[RequireComponent(typeof(CombatUIHandler))]
public class CombatManager : MonoBehaviour
{
    //Static reference to this combat manager
    public static CombatManager globalReference;

    //References to the scripts that handle different aspects of the combat gameplay
    public CombatTileHandler tileHandler;
    public CombatInitiativeHandler initiativeHandler;
    public CombatCharacterHandler characterHandler;
    public CombatUIHandler uiHandler;

    //The value that attacking characters must reach after all combat modifiers to hit their opponent. Used in AttackAction and WeaponAction
    public const int baseHitDC = 20;

    //Enum for the state of this combat manager to decide what to do on update
    [HideInInspector]
    public CombatState currentState = CombatState.Wait;

    //The amount of time that has passed while waiting
    private float waitTime = 0;
    //The combat state to switch to after the wait time is up
    private CombatState stateAfterWait = CombatState.IncreaseInitiative;

    //The event that is activated at the start of combat
    public UnityEvent combatInitializeEvent;
    //The event that is activated at the end of combat
    public UnityEvent combatEndEvent;
    //The unity event that's invoked when a player character can perform actions
    public UnityEvent showPlayerActions;

    //Reference to the background image that's set at the start of combat based on the type of land tile
    public Image backgroundImageObject;

    //Dictionary that determines which background sprite to set based on the land tile type
    public List<BackgroundImageTypes> tileTypeBackgrounds;

    //Unity Event called right after a player performs an action
    public UnityEvent eventAfterActionPerformed;

    //Object that's created to display damage on an attacked character's tile
    public DamageText damageTextPrefab;
    
    //The list of all CombatCharacterSprites for each character and enemy
    public List<CharacterSpriteBase> characterSpriteList;

    //The reference to the Info Display object so we can show what actions are being used
    public InfoDisplay ourInfoDisplay;

    //The loot table for the current encounter
    private List<EncounterLoot> lootTable;

    //The list of characters who are dead after an attack
    private List<CharacterSpriteBase> deadCharacterSprites;

    //Delegate function that's tied to the CharacterDeathEVT to make sure they're removed from combat correctly
    private DelegateEvent<EVTData> charDeathEVT;



	// Function called when this object is created
	private void Awake ()
    {
        //Setting the static reference
        if (globalReference == null)
        {
            globalReference = this;
        }
        //If a static reference already exists, this component is destroyed
        else
        {
            Destroy(this);
        }

        //Initializing the active characters list
        this.deadCharacterSprites = new List<CharacterSpriteBase>();
        this.characterSpriteList = new List<CharacterSpriteBase>();

        //Setting our charDeathEVT delegate
        this.charDeathEVT = new DelegateEvent<EVTData>(this.CharacterDied);
    }


    //Function called when this component is enabled
    private void OnEnable()
    {
        EventManager.StartListening(CharacterDeathEVT.eventNum, this.charDeathEVT);
    }


    //Function called when this component is disabled
    private void OnDisable()
    {
        EventManager.StopListening(CharacterDeathEVT.eventNum, this.charDeathEVT);
    }


    //Function called every frame
    private void Update()
    {
        //If the acting character list isn't empty, we highlight the acting character's position
        if (this.initiativeHandler.actingCharacters.Count > 0)
        {
            this.tileHandler.tileHighlight.transform.position = this.tileHandler.FindCharactersTile(this.initiativeHandler.actingCharacters[0]).transform.position;
        }

        //Determine what we do based on the current state
        switch (this.currentState)
        {
            //Nothing, waiting for player feedback
            case CombatState.PlayerInput:
                return;

            //Counting down the wait time
            case CombatState.Wait:
                {
                    this.waitTime -= Time.deltaTime;
                    //If the timer is up, the state changes to the one that was previously designated
                    if (this.waitTime <= 0)
                    {
                        //If we have dead character sprites to remove
                        if (this.deadCharacterSprites.Count > 0)
                        {
                            //Looping through all of our dead character sprites to remove and destroy them
                            foreach (CharacterSpriteBase deadCSprite in this.deadCharacterSprites)
                            {
                                this.characterSpriteList.Remove(deadCSprite);
                                Destroy(deadCSprite.gameObject);
                            }

                            //Clearing the list of dead character sprites
                            this.deadCharacterSprites = new List<CharacterSpriteBase>();
                        }

                        //If the current state is player input or action selecting and the state we're switching to is increasing initiative, we hide the character highlight
                        if (this.stateAfterWait == CombatState.IncreaseInitiative)
                        {
                            //Making the highlight ring invisible again
                            this.tileHandler.tileHighlight.enabled = false;
                        }

                        this.currentState = this.stateAfterWait;
                        this.stateAfterWait = CombatState.IncreaseInitiative;

                        //Disabling the action blocker so the player can pick actions again
                        this.uiHandler.actionBlocker.enabled = false;
                    }
                }
                break;

            //Adding each character's attack speed to their current initative 
            case CombatState.IncreaseInitiative:
                {
                    //Making sure the highlight ring is invisible
                    this.initiativeHandler.IncreaseInitiatives();
                }
                break;

            //Hilighting the selected character whose turn it is
            case CombatState.SelectAction:
                {
                    //Triggering each combat effect on the acting character for the beginning of their turn
                    int numberOfEffects = this.initiativeHandler.actingCharacters[0].charCombatStats.combatEffects.Count;
                    Character actingChar = this.initiativeHandler.actingCharacters[0];
                    for (int sotEffects = 0; sotEffects < actingChar.charCombatStats.combatEffects.Count; ++sotEffects)
                    {
                        actingChar.charCombatStats.combatEffects[sotEffects].EffectOnStartOfTurn();

                        //Checking to see if the effect has expired to set the counter back by 1
                        if (actingChar.charCombatStats.combatEffects.Count < numberOfEffects)
                        {
                            numberOfEffects -= 1;
                            sotEffects -= 1;
                        }
                    }

                    //Refreshing the action list for the acting character
                    actingChar.charActionList.RefreshActionLists();

                    //If the selected character is a player
                    if (this.characterHandler.playerCharacters.Contains(actingChar))
                    {
                        //Hilighting the slider of the player character whose turn it is
                        int selectedCharIndex = this.characterHandler.playerCharacters.IndexOf(actingChar);
                        this.uiHandler.playerPanels[selectedCharIndex].SetBackgroundColor(this.uiHandler.actingPlayerColor);

                        //Setting the highlight ring's color to the player color and making it visible
                        this.tileHandler.tileHighlight.SetHighlightColor(this.uiHandler.actingPlayerColor);
                        this.tileHandler.tileHighlight.enabled = true;

                        //If this player character isn't dead from previous effects
                        if (actingChar.charPhysState.currentHealth > 0)
                        {
                            //Displaying the action panel so players can decide what to do
                            this.showPlayerActions.Invoke();
                            //Default to showing the acting character's standard actions
                            CombatActionPanelUI.globalReference.DisplayActionTypes(0);
                            //Now we wait for player input
                            this.currentState = CombatState.PlayerInput;
                        }
                    }
                    //If the selected character is an enemy
                    else
                    {
                        //Getting the index for the acting enemy
                        int selectedEnemyIndex = this.characterHandler.enemyCharacters.IndexOf(actingChar);
                        this.uiHandler.enemyPanels[selectedEnemyIndex].SetBackgroundColor(this.uiHandler.actingEnemyColor);

                        //Setting the highlight ring's color to the enemy color and making it visible
                        this.tileHandler.tileHighlight.SetHighlightColor(this.uiHandler.actingEnemyColor);
                        this.tileHandler.tileHighlight.enabled = true;

                        //If this enemy isn't dead from previous effects
                        if (actingChar.charPhysState.currentHealth > 0)
                        {
                            //Now we wait for enemy input
                            this.currentState = CombatState.PlayerInput;
                            //Starting the acting enemy's turn so it can perform its actions
                            this.characterHandler.enemyCharacters[selectedEnemyIndex].GetComponent<EnemyCombatAI_Basic>().StartEnemyTurn();
                        }
                    }
                }
                break;

            //Calls the unity event for when this combat encounter is over
            case CombatState.EndCombat:
                {
                    //Rolling for the encounter loot to give to the player
                    this.GetEncounterLoot();
                    //Creating the event data that we'll pass to the TransitionFade through the EventManager
                    EVTData transitionEvent = new EVTData();
                    //Setting the transition to end combat, take 0.5 sec to fade out, stay on black for 1 sec, fade in for 0.5 sec, and call our initialize event to hide the combat canvas
                    transitionEvent.combatTransition = new CombatTransitionEVT(false, 0.5f, 1, 0.5f, this.combatEndEvent);
                    //Invoking the transition event through the EventManager
                    EventManager.TriggerEvent(CombatTransitionEVT.eventNum, transitionEvent);
                    //this.combatEndEvent.Invoke();
                    this.currentState = CombatState.PlayerInput;
                }
                break;

            //The game is over
            case CombatState.GameOver:
                {
                    Debug.Log("<<<------------- GAME OVER! -------------->>>");
                }
                break;
        }
    }
    
    
    //Function called externally from LandTile.cs to initiate combat
    public void InitiateCombat(LandType combatLandType_, PartyGroup charactersInCombat_, EnemyEncounter encounter_)
    {
        //Creating the event data that we'll pass to the TransitionFade through the EventManager
        EVTData transitionEvent = new EVTData();
        //Setting the transition to take 0.5 sec to fade out, stay on black for 1 sec, fade in for 0.5 sec, and call our initialize event to display the combat canvas
        transitionEvent.combatTransition = new CombatTransitionEVT(true, 0.5f, 1, 0.5f, this.combatInitializeEvent);
        //Invoking the transition event through the EventManager
        EventManager.TriggerEvent(CombatTransitionEVT.eventNum, transitionEvent);

        //Resetting all of the combat tiles to their default values
        this.tileHandler.ResetCombatTiles();
        
        //Setting the combat positions for the player characters and enemies based on their distances
        this.characterHandler.InitializeCharactersForCombat(charactersInCombat_, encounter_);

        //Resetting the combat UI
        this.initiativeHandler.ResetForCombatStart();
        //this.uiHandler.ResetForCombatStart();
        
        //Hiding the highlight ring
        this.tileHandler.tileHighlight.enabled = false;
        
        //Setting the state to start increasing initiatives after a brief wait
        this.SetWaitTime(3, CombatState.IncreaseInitiative);
        
        //Looping through and copying the loot table from the encounter
        this.lootTable = new List<EncounterLoot>();
        foreach(EncounterLoot drop in encounter_.lootTable)
        {
            EncounterLoot loot = new EncounterLoot();
            loot.lootItem = drop.lootItem;
            loot.dropChance = drop.dropChance;
            loot.stackSizeMinMax = drop.stackSizeMinMax;
            this.lootTable.Add(loot);
        }
    }
    
    
    //Function called to set the amount of time to wait
    public void SetWaitTime(float timeToWait_, CombatState stateAfterWait_ = CombatState.IncreaseInitiative)
    {
        //If the state after we're done waiting is GameOver or EndCombat, we don't change states
        if(this.stateAfterWait == CombatState.EndCombat || this.stateAfterWait == CombatState.GameOver)
        {
            return;
        }

        this.waitTime = timeToWait_;
        this.currentState = CombatState.Wait;
        this.stateAfterWait = stateAfterWait_;

        //Turns on the action blocker so the player can't perform another action until the wait time is over
        this.uiHandler.actionBlocker.enabled = true;
    }
    

    //Function called from AttackAction.PerformAction to show damage dealt to a character at the given tile
    public void DisplayDamageDealt(float timeDelay_, int damage_, DamageType type_, CombatTile3D damagedCharTile_, bool isCrit_, bool isHeal_ = false)
    {
        //If the damage dealt was 0, nothing happens
        if(damage_ <= 0)
        {
            return;
        }

        //Creating an instance of the damage text object prefab
        GameObject newDamageDisplay = GameObject.Instantiate(this.damageTextPrefab.gameObject);
        //Parenting the damage text object to this object's transform
        newDamageDisplay.transform.SetParent(this.transform);
        //Getting the DamageText component reference
        DamageText newDamageText = newDamageDisplay.GetComponent<DamageText>();

        //Setting the info for the text
        newDamageText.SetDamageToDisplay(timeDelay_, damage_, type_, damagedCharTile_.transform.position, isCrit_, isHeal_);
        
        //Updating the health bars so we can see how much health characters have
        this.uiHandler.UpdateHealthBars();
    }


    //Function called from CombatTile.cs to perform the selected action in the CombatActionPanelUI
    public void PerformActionAtClickedTile(CombatTile3D tileClicked_)
    {
        //If the action being performed is a movement action and the tile clicked isn't empty, nothing happens
        if (CombatActionPanelUI.globalReference.selectedAction.GetComponent<MoveAction>() && tileClicked_.typeOnTile != TileObjectType.Nothing)
        {
            return;
        }

        //Tells our info display object to show the name of the action used if it isn't a move action
        if (!CombatActionPanelUI.globalReference.selectedAction.GetComponent<MoveAction>())
        {
            this.ourInfoDisplay.StartInfoDisplay(CombatActionPanelUI.globalReference.selectedAction.actionName, CombatActionPanelUI.globalReference.selectedAction.timeToCompleteAction);
        }

        //Tells the action to be performed at the tile clicked and stops highlighting it
        CombatActionPanelUI.globalReference.selectedAction.PerformAction(tileClicked_);

        //Have this combat manager wait a bit before going back because there could be animations
        if (this.stateAfterWait != CombatState.EndCombat)
        {
            this.SetWaitTime(CombatActionPanelUI.globalReference.selectedAction.timeToCompleteAction, CombatState.PlayerInput);
        }

        //Disables the types of actions that were used 
        CombatActionPanelUI.globalReference.DisableUsedActions();

        //Clearing the highlighted area showing the previously used action's range
        this.tileHandler.ClearTileHilights();
    }


    //Function called from EnemyCombatAI_Basic.cs to perform an enemy's action at the given tile
    public void PerformEnemyActionOnTile(CombatTile3D tileClicked_, Action enemyAction_)
    {
        //If the action being performed is a movement action and the tile chosen isn't empty, nothing happens
        if(enemyAction_.GetType() == typeof(MoveAction))
        {
            if (tileClicked_.typeOnTile != TileObjectType.Nothing)
            {
                return;
            }
        }
        //Tells our info display object to show the name of the action used if it isn't a move action
        else
        {
            this.ourInfoDisplay.StartInfoDisplay(enemyAction_.actionName, enemyAction_.timeToCompleteAction);
        }

        //Creating a new instance of the action to use
        GameObject actionInstance = Instantiate(enemyAction_.gameObject);

        //Tells the action to be performed at the tile chosen
        actionInstance.GetComponent<Action>().PerformAction(tileClicked_);

        //Have this combat manager wait a bit before going back because there could be animations
        if(this.stateAfterWait != CombatState.EndCombat)
        {
            this.SetWaitTime(enemyAction_.timeToCompleteAction, CombatState.PlayerInput);
        }
    }


    //Function called externally to end the acting character's turn
    public void EndActingCharactersTurn()
    {
        //Perform the unity event after the action so we can hide some UI elements
        this.eventAfterActionPerformed.Invoke();
        
        //If there's a selected action in the CombatActionPanelUI, we need to destroy it
        if (CombatActionPanelUI.globalReference.selectedAction != null)
        {
            //If the selected action is a move action, we need to clear all highlighted tiles in the movement path first
            if(CombatActionPanelUI.globalReference.selectedAction.GetComponent<MoveAction>())
            {
                CombatActionPanelUI.globalReference.selectedAction.GetComponent<MoveAction>().ClearMovePathHighlights();
            }

            Destroy(CombatActionPanelUI.globalReference.selectedAction.gameObject);
        }

        //Looping through and triggering all effects on the acting character for when their turn ends
        foreach(Effect e in this.initiativeHandler.actingCharacters[0].charCombatStats.combatEffects)
        {
            e.EffectOnEndOfTurn();
        }

        //Resets the acting character's initiative and removes them from the list of acting characters
        if (this.characterHandler.playerCharacters.Contains(this.initiativeHandler.actingCharacters[0]))
        {
            int selectedCharIndex = this.characterHandler.playerCharacters.IndexOf(this.initiativeHandler.actingCharacters[0]);
            //Resetting their initiative slider's color
            this.uiHandler.playerPanels[selectedCharIndex].backgroundImage.color = this.uiHandler.inactiveColor;
            //Resetting their initiative slider
            this.uiHandler.playerPanels[selectedCharIndex].initiativeSlider.value = 0;
            //Removing the currently acting character
            this.initiativeHandler.actingCharacters.Remove(this.initiativeHandler.actingCharacters[0]);
        }
        else if(this.characterHandler.enemyCharacters.Contains(this.initiativeHandler.actingCharacters[0]))
        {
            int selectedEnemyIndex = this.characterHandler.enemyCharacters.IndexOf(this.initiativeHandler.actingCharacters[0]);
            //Resetting their initiative slider's color
            this.uiHandler.enemyPanels[selectedEnemyIndex].backgroundImage.color = this.uiHandler.inactiveColor;
            //Resetting their initiative slider
            this.uiHandler.enemyPanels[selectedEnemyIndex].initiativeSlider.value = 0;
            //Removing the currently acting character
            this.initiativeHandler.actingCharacters.Remove(this.initiativeHandler.actingCharacters[0]);
        }

        //Clearing the highlighted area showing the previously used action's range
        this.tileHandler.ClearTileHilights();

        //If there are still acting characters we transition to their turn
        if (this.initiativeHandler.actingCharacters.Count > 0)
        {
            this.uiHandler.HilightActingCharacter();
            this.SetWaitTime(1, CombatState.SelectAction);
        }
        //Otherwise we have the combat manager wait a moment before going back to increasing initiatives
        else
        {
            this.SetWaitTime(1, CombatState.IncreaseInitiative);
        }
    }


    //Function called from Action.cs and Effect.cs when a player character performs an action
    public void ApplyActionThreat(Character actingCharacter_, Character targetCharacter_, int threatToAdd_, bool increaseForAllEnemies_)
    {
        //If the currently acting character is an enemy, nothing happens. Enemies can't increase their own threat
        if(this.characterHandler.enemyCharacters.Contains(actingCharacter_))
        {
            return;
        }

        //If the target character is empty or a player character
        if(targetCharacter_ == null || this.characterHandler.playerCharacters.Contains(targetCharacter_))
        {
            //If threat is increased for all enemies, we can add threat. If it isn't increased for all enemies, we do nothing
            if(increaseForAllEnemies_)
            {
                //Looping through each enemy character in combat
                foreach(Character enemy in this.characterHandler.enemyCharacters)
                {
                    //Making sure the enemy is alive first
                    if(enemy.charPhysState.currentHealth > 0)
                    {
                        //Making sure they have the EnemyCombatAI component
                        if(enemy.GetComponent<EnemyCombatAI_Basic>())
                        {
                            enemy.GetComponent<EnemyCombatAI_Basic>().IncreaseThreat(actingCharacter_, threatToAdd_);
                        }
                    }
                }
            }
        }
        //If the target character is an enemy and they have the EnemyCombatAI component
        else if(this.characterHandler.enemyCharacters.Contains(targetCharacter_))
        {
            //If we increase threat for all enemies
            if(increaseForAllEnemies_)
            {
                //Looping through each enemy character in combat
                foreach (Character enemy in this.characterHandler.enemyCharacters)
                {
                    //Making sure the enemy is alive first
                    if (enemy.charPhysState.currentHealth > 0)
                    {
                        //Making sure they have the EnemyCombatAI component
                        if (enemy.GetComponent<EnemyCombatAI_Basic>())
                        {
                            enemy.GetComponent<EnemyCombatAI_Basic>().IncreaseThreat(actingCharacter_, threatToAdd_);
                        }
                    }
                }
            }
            //Otherwise we only increase threat on the target
            else
            {
                //Making sure the target is alive and has the EnemyCombatAI component first
                if(targetCharacter_.charPhysState.currentHealth > 0 && targetCharacter_.GetComponent<EnemyCombatAI_Basic>())
                {
                    targetCharacter_.GetComponent<EnemyCombatAI_Basic>().IncreaseThreat(actingCharacter_, threatToAdd_);
                }
            }
        }
    }


    //Function called from the charDeathEVT delegate when a character dies
    private void CharacterDied(EVTData data_)
    {
        //Making sure the data isn't null
        if(data_ == null || data_.characterDeath == null)
        {
            return;
        }

        //If the dead character is the acting character, we end it's turn
        if (this.initiativeHandler.actingCharacters.Count > 0 && this.initiativeHandler.actingCharacters[0] == data_.characterDeath.deadCharacter)
        {
            this.tileHandler.tileHighlight.enabled = false;
            this.EndActingCharactersTurn();
        }
        //If the dead character was waiting to act, we remove it from the list of acting characters
        else if(this.initiativeHandler.actingCharacters.Contains(data_.characterDeath.deadCharacter))
        {
            this.initiativeHandler.actingCharacters.Remove(data_.characterDeath.deadCharacter);
        }

        //Updating the health bars
        this.uiHandler.UpdateHealthBars();

        //If this character was a player character
        if(this.characterHandler.playerCharacters.Contains(data_.characterDeath.deadCharacter))
        {
            //Updating the quest tracker to see if the dead ally is an escort character
            QuestTracker.globalReference.CheckForDeadEscortCharacter(data_.characterDeath.deadCharacter);

            //Looping through all player characters to see if they're all dead
            bool allPlayersAreDead = true;
            for(int p = 0; p < this.characterHandler.playerCharacters.Count; ++p)
            {
                //If this player character has any health, we break the loop and nothing happens
                if(this.characterHandler.playerCharacters[p].charPhysState.currentHealth > 0)
                {
                    allPlayersAreDead = false;
                    break;
                }
            }

            //If we made it through the loop without finding any living players, GAME OVER
            if(allPlayersAreDead)
            {
                this.SetWaitTime(5f, CombatState.GameOver);
            }
        }
        //If this character was an enemy character
        else if(this.characterHandler.enemyCharacters.Contains(data_.characterDeath.deadCharacter))
        {
            //Updating the quest tracker to see if the dead enemy is a quest target
            QuestTracker.globalReference.UpdateKillQuests(data_.characterDeath.deadCharacter);

            //Looping through all enemy characters to see if they're all dead
            bool allEnemiesAreDead = true;
            for(int e = 0; e < this.characterHandler.enemyCharacters.Count; ++e)
            {
                //If this enemy has any health, we break the loop and nothing happens
                if(this.characterHandler.enemyCharacters[e].charPhysState.currentHealth > 0)
                {
                    allEnemiesAreDead = false;
                    break;
                }
            }

            //If we made it through the loop without finding any living enemies, the combat is over
            if(allEnemiesAreDead)
            {
                this.SetWaitTime(2.5f, CombatState.EndCombat);
            }
        }
    }


    //Function called from UpdateHealthBars when all enemies are dead. Rolls the loot table for the encounter
    private void GetEncounterLoot()
    {
        //Getting the reference to the inventory where we put loot
        Inventory lootInventory = InventoryOpener.globalReference.bagInventory;
        //Clearing all of the items in the loot inventory
        for (int i = 0; i < lootInventory.itemSlots.Count; ++i)
        {
            //If there is an item in this slot, it's destroyed
            if(lootInventory.itemSlots[i] != null)
            {
                Destroy(lootInventory.itemSlots[i].gameObject);
                lootInventory.itemSlots[i] = null;
            }
        }

        //Looping through all of the items in the loot table
        foreach(EncounterLoot potentialLoot in this.lootTable)
        {
            //Rolling a random number to see if the loot drops
            float randRoll = Random.Range(0f, 1f);
            if(randRoll <= potentialLoot.dropChance)
            {
                //Creating an instance of the item
                GameObject objInstance = GameObject.Instantiate(potentialLoot.lootItem.gameObject);
                Item itemInstance = objInstance.GetComponent<Item>();
                //Setting the prefab reference for the item
                itemInstance.itemPrefabRoot = potentialLoot.lootItem.gameObject;
                //Adding the item to the loot inventory
                lootInventory.AddItemToInventory(itemInstance);

                //Getting the number of items in the stack
                int stackSize = Mathf.RoundToInt(Random.Range(potentialLoot.stackSizeMinMax.x, potentialLoot.stackSizeMinMax.y));
                if (stackSize > 1)
                {
                    //Looping through all of the stacked items
                    for(int s = 0; s < stackSize - 1; ++s)
                    {
                        //Creating an instance of the stacked item
                        GameObject stackObj = GameObject.Instantiate(potentialLoot.lootItem.gameObject);
                        Item stackItem = stackObj.GetComponent<Item>();
                        //Setting the prefab reference for the item
                        stackItem.itemPrefabRoot = potentialLoot.lootItem.gameObject;
                        //Adding the stack item to the loot inventory
                        lootInventory.AddItemToInventory(stackItem);
                    }
                }
            }
        }

        //If the loot inventory has at least 1 item in it, we display the loot UI
        if(lootInventory.itemSlots[0] != null)
        {
            InventoryOpener.globalReference.bagInventoryUIObject.SetActive(true);
            //Looping through the list of characters in the selected player party until we find one
            foreach(Character c in CharacterManager.globalReference.selectedGroup.charactersInParty)
            {
                if(c != null)
                {
                    //Setting the selected character to be the party character whose inventory is displayed
                    CharacterInventoryUI.partyInventory.selectedCharacterInventory = c.charInventory;
                    break;
                }
            }
            InventoryOpener.globalReference.partyInventoryUIObject.SetActive(true);
        }
    }
}

