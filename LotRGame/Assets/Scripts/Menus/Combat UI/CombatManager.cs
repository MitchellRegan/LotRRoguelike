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

    //Reference to the characters whose turn it is to act. It's a list because multiple characters could have the same initiative
    [HideInInspector]
    public List<Character> actingCharacters = null;

    //2D List of all combat tiles in the combat screen map. Col[row}
    public List<List<CombatTile>> combatTileGrid;

    //The list of all characters involved in this combat
    [HideInInspector]
    public List<Character> playerCharactersInCombat;
    [HideInInspector]
    public List<Character> enemyCharactersInCombat;

    //The event that is activated at the start of combat
    public UnityEvent combatInitializeEvent;
    //The event that is activated at the end of combat
    public UnityEvent combatEndEvent;
    //The unity event that's invoked when a player character can perform actions
    public UnityEvent showPlayerActions;

    //The color of player initiative panels when they're the acting character
    public Color actingCharacterColor = Color.green;
    //The color of enemy initiative panels when they're the acting character
    public Color actingEnemyColor = Color.red;
    //The color of initiative panels when not acting
    public Color inactivePanelColor = Color.white;

    //The list of all sliders that show each player character's initiative
    public List<InitiativePanel> playerInitiativeSliders;
    //The list of all sliders that show each enemy character's initiative
    public List<InitiativePanel> enemyInitiativeSliders;

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

        //Initializing our combat tile grid
        this.combatTileGrid = new List<List<CombatTile>>();
        //Setting up each column of rows
        for (int col = 0; col < 14; ++col)
        {
            this.combatTileGrid.Add(new List<CombatTile>());
            //Setting up each row inside the current column
            for(int row = 0; row < 8; ++row)
            {
                this.combatTileGrid[col].Add(null);
            }
        }

        //Initializing the active characters list
        this.actingCharacters = new List<Character>();
        this.deadCharacterSprites = new List<CharacterSpriteBase>();
        this.playerCharactersInCombat = new List<Character>();
        this.enemyCharactersInCombat = new List<Character>();
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
        if (this.actingCharacters.Count > 0)
        {
            this.tileHandler.tileHighlight.transform.position = this.FindCharactersTile(this.actingCharacters[0]).transform.position;
        }

        //Determine what we do based on the current state
        switch (this.currentState)
        {
            //Nothing, waiting for player feedback
            case CombatState.PlayerInput:
                return;

            //Counting down the wait time
            case CombatState.Wait:
                this.waitTime -= Time.deltaTime;
                //If the timer is up, the state changes to the one that was previously designated
                if(this.waitTime <= 0)
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
                    if(this.stateAfterWait == CombatState.IncreaseInitiative)
                    {
                        //Making the highlight ring invisible again
                        this.tileHandler.tileHighlight.enabled = false;
                    }

                    this.currentState = this.stateAfterWait;
                    this.stateAfterWait = CombatState.IncreaseInitiative;

                    //Disabling the action blocker so the player can pick actions again
                    this.uiHandler.actionBlocker.enabled = false;
                }
                break;

            //Adding each character's attack speed to their current initative 
            case CombatState.IncreaseInitiative:
                //Making sure the highlight ring is invisible
                this.initiativeHandler.IncreaseInitiatives();
                break;

            //Hilighting the selected character whose turn it is
            case CombatState.SelectAction:
                //Triggering each combat effect on the acting character for the beginning of their turn
                int numberOfEffects = this.actingCharacters[0].charCombatStats.combatEffects.Count;
                Character actingChar = this.actingCharacters[0];
                for(int sotEffects = 0; sotEffects < actingChar.charCombatStats.combatEffects.Count; ++sotEffects)
                {
                    actingChar.charCombatStats.combatEffects[sotEffects].EffectOnStartOfTurn();

                    //Checking to see if the effect has expired to set the counter back by 1
                    if(actingChar.charCombatStats.combatEffects.Count < numberOfEffects)
                    {
                        numberOfEffects -= 1;
                        sotEffects -= 1;
                    }
                }

                //Refreshing the action list for the acting character
                actingChar.charActionList.RefreshActionLists();

                //If the selected character is a player
                if (this.playerCharactersInCombat.Contains(actingChar))
                {
                    //Hilighting the slider of the player character whose turn it is
                    int selectedCharIndex = this.playerCharactersInCombat.IndexOf(actingChar);
                    this.playerInitiativeSliders[selectedCharIndex].background.color = this.actingCharacterColor;

                    //Setting the highlight ring's color to the player color and making it visible
                    this.tileHandler.tileHighlight.SetHighlightColor(this.actingCharacterColor);
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
                    int selectedEnemyIndex = this.enemyCharactersInCombat.IndexOf(actingChar);
                    this.enemyInitiativeSliders[selectedEnemyIndex].background.color = this.actingEnemyColor;

                    //Setting the highlight ring's color to the enemy color and making it visible
                    this.tileHandler.tileHighlight.SetHighlightColor(this.actingEnemyColor);
                    this.tileHandler.tileHighlight.enabled = true;

                    //If this enemy isn't dead from previous effects
                    if (actingChar.charPhysState.currentHealth > 0)
                    {
                        //Now we wait for enemy input
                        this.currentState = CombatState.PlayerInput;
                        //Starting the acting enemy's turn so it can perform its actions
                        this.enemyCharactersInCombat[selectedEnemyIndex].GetComponent<EnemyCombatAI_Basic>().StartEnemyTurn();
                    }
                }
                break;

            //Calls the unity event for when this combat encounter is over
            case CombatState.EndCombat:
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
                break;

            //The game is over
            case CombatState.GameOver:
                Debug.Log("<<<------------- GAME OVER! -------------->>>");
                break;
        }
    }


    //Function called externally from CombatTile.cs on Start. Adds a combat tile to our combat tile grid at the row and column given
    public void AddCombatTileToGrid(CombatTile tileToAdd_, int row_, int col_)
    {
        if(this.combatTileGrid[col_][row_] != null)
        {
            Debug.LogError(col_ + ", " + row_);
        }

        //Setting the given tile to the correct row and column
        this.combatTileGrid[col_][row_] = tileToAdd_;
    }


    //Function called from CombatActionPanelUI to turn off combat tile highlights
    public void ClearCombatTileHighlights()
    {
        //Looping through every tile in each row and column in the grid, making sure they're all not highlighted
        foreach(List<CombatTile> col in this.combatTileGrid)
        {
            foreach(CombatTile tile in col)
            {
                tile.inActionRange = false;
                tile.HighlightTile(false);

                //If there's a character sprite on this tile, we make sure it's visible
                if(tile.objectOnThisTile != null && tile.objectOnThisTile.GetComponent<Character>())
                {
                    //Getting the sprite base for the character
                    CharacterSpriteBase cSprite = CombatManager.globalReference.GetCharacterSprite(tile.objectOnThisTile.GetComponent<Character>());

                    if (cSprite != null)
                    {
                        cSprite.MakeSpritesVisible();
                    }
                }
            }
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
        
        //Looping through and resetting the combat tiles
        for (int c = 0; c < this.combatTileGrid.Count; ++c)
        {
            for(int r = 0; r < this.combatTileGrid[c].Count; ++r)
            {
                this.combatTileGrid[c][r].ResetTile();
            }
        }

        //Setting the background image
        this.SetBackgroundImage(combatLandType_);
        
        //Setting the combat positions for the player characters and enemies based on their distances
        this.SetCombatPositions(charactersInCombat_, encounter_);
        
        //Creating the Combat Character Sprites
        this.CreateCharacterSprites();

        this.initiativeHandler.ResetForCombatStart();
        this.uiHandler.ResetForCombatStart();

        //Setting each character on the tile positions
        this.UpdateCombatTilePositions();
        
        //Hiding the highlight ring
        this.tileHandler.tileHighlight.enabled = false;
        
        //Setting the state to start increasing initiatives after a brief wait
        this.SetWaitTime(3, CombatState.IncreaseInitiative);
        
        //Setting the health bars to display the correct initiatives
        this.UpdateHealthBars();
        
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

        //Looping through each player character to see if anyone has a Threat Boost perk or perk to start combat with an effect
        for (int t = 0; t < this.playerCharactersInCombat.Count; ++t)
        {
            //Looping through all of the current character's perks
            foreach(Perk charPerk in this.playerCharactersInCombat[t].charPerks.allPerks)
            {
                //If the current perk is a ThreatBoostPerk that works at the start of combat, we boost threat against this character for all enemies
                if(charPerk.GetType() == typeof(ThreatBoostPerk) && charPerk.GetComponent<ThreatBoostPerk>().increaseThreatAtStartOfCombat)
                {
                    this.ApplyActionThreat(this.playerCharactersInCombat[t], null, charPerk.GetComponent<ThreatBoostPerk>().baseThreatToAdd, true);
                }
                //If the current perk is a StartCombatWithEffectPerk, we check to see if the effect will be actiavted
                else if(charPerk.GetType() == typeof(StartCombatWithEffectPerk))
                {
                    StartCombatWithEffectPerk effectPerk = charPerk.GetComponent<StartCombatWithEffectPerk>();

                    //Getting a random % roll to see if it's at the application chance
                    float randomRoll = Random.Range(0, 1);
                    if(randomRoll <= effectPerk.chanceToApplyEffect)
                    {
                        //Creating a new instance of the effect object prefab and triggering its effect
                        GameObject effectObj = Instantiate(effectPerk.effectToStartWith.gameObject, new Vector3(), new Quaternion());
                        effectObj.GetComponent<Effect>().TriggerEffect(this.playerCharactersInCombat[t], this.playerCharactersInCombat[t]);
                    }
                }
            }
        }
    }


    //Function called from InitiateCombat to set the positions of characters and enemies based on the party and enemy combat positions
    private void SetCombatPositions(PartyGroup playerParty_, EnemyEncounter enemyParty_)
    {
        //The number of columns the player party is shifted
        int playerColShift = 0;

        //The number of columns the front half of the enemy encounter is shifted
        int enemyColShift0 = 0;
        int enemyColShift1 = 1;
        int enemyColShift2 = 2;
        int enemyColShift3 = 3;

        //Determine if we use the default enemy position or the ambush position
        EnemyCombatPosition encounterPos = enemyParty_.defaultPosition;

        //Rolling to see if this encounter will ambush the player
        float ambushRoll = Random.Range(0f, 1f);

        //If the enemies are able to ambush players, their encounter position is set to the ambush position
        if(ambushRoll < enemyParty_.ambushChance)
        {
            encounterPos = enemyParty_.ambushPosition;
        }

        //Determining which kind of enemy encounter the player's will be facing
        switch(encounterPos)
        {
            //If the enemy is in melee range
            case EnemyCombatPosition.MeleeFront:
                {
                    //Setting the player positions between col 0 - 6
                    switch(playerParty_.combatDistance)
                    {
                        case GroupCombatDistance.Far://0-2
                            playerColShift = 0;
                            break;
                        case GroupCombatDistance.Medium://2-4
                            playerColShift = 2;
                            break;
                        case GroupCombatDistance.Close://4-6
                            playerColShift = 4;
                            break;
                    }

                    //Setting the enemy positions between col 7-10
                    enemyColShift0 += 7;
                    enemyColShift1 += 7;
                    enemyColShift2 += 7;
                    enemyColShift3 += 7;
                }
                break;

            case EnemyCombatPosition.MeleeFlanking:
                {
                    //Setting the player positions between col 6-8 regardless of their preferred distance
                    playerColShift = 6;

                    //Setting the enemy positions so that they're split between cols 4-5 and 9-10
                    enemyColShift0 += 4;//Col 5
                    enemyColShift1 += 8;//Col 9
                    enemyColShift2 += 1;//Col 4
                    enemyColShift3 += 7;//Col 10
                }
                break;

            case EnemyCombatPosition.MeleeBehind:
                {
                    //Setting the player positions between col 7-13
                    switch (playerParty_.combatDistance)
                    {
                        case GroupCombatDistance.Far://7-9
                            playerColShift = 7;
                            break;
                        case GroupCombatDistance.Medium://9-11
                            playerColShift = 9;
                            break;
                        case GroupCombatDistance.Close://11-13
                            playerColShift = 11;
                            break;
                    }

                    //Setting the enemy positions between col 3-6 but in reverse order
                    enemyColShift0 += 6;
                    enemyColShift1 += 4;
                    enemyColShift2 += 2;
                    enemyColShift3 += 0;
                }
                break;

            //If the enemy is in middle range
            case EnemyCombatPosition.MiddleFront:
                {
                    //Setting the player positions between col 0 - 6
                    switch (playerParty_.combatDistance)
                    {
                        case GroupCombatDistance.Far://0-2
                            playerColShift = 0;
                            break;
                        case GroupCombatDistance.Medium://2-4
                            playerColShift = 2;
                            break;
                        case GroupCombatDistance.Close://4-6
                            playerColShift = 4;
                            break;
                    }

                    //Setting the enemy positions between col 9-12
                    enemyColShift0 += 9;
                    enemyColShift1 += 9;
                    enemyColShift2 += 9;
                    enemyColShift3 += 9;
                }
                break;

            case EnemyCombatPosition.MiddleFlanking:
                {
                    //Setting the player positions between col 5-8
                    switch (playerParty_.combatDistance)
                    {
                        case GroupCombatDistance.Far://5-7
                            playerColShift = 5;
                            break;
                        case GroupCombatDistance.Medium://6-8
                            playerColShift = 6;
                            break;
                        case GroupCombatDistance.Close://6-8
                            playerColShift = 6;
                            break;
                    }

                    //Setting the enemy positions split between cols 2-3 and cols 10-11
                    enemyColShift0 += 3;//Col 3
                    enemyColShift1 += 9;//Col 10
                    enemyColShift2 += 0;//Col 2
                    enemyColShift3 += 8;//Col 11
                }
                break;

            case EnemyCombatPosition.MiddleBehind:
                {
                    //Setting the player positions between col 7-13
                    switch (playerParty_.combatDistance)
                    {
                        case GroupCombatDistance.Far://7-9
                            playerColShift = 7;
                            break;
                        case GroupCombatDistance.Medium://9-11
                            playerColShift = 9;
                            break;
                        case GroupCombatDistance.Close://11-13
                            playerColShift = 11;
                            break;
                    }

                    //Setting the enemy positions between col 1-4 but in reverse order
                    enemyColShift0 += 4;
                    enemyColShift1 += 2;
                    enemyColShift2 += 0;
                    enemyColShift3 += -2;
                }
                break;
            
            //If the enemy is in a far range
            case EnemyCombatPosition.RangedFront:
                {
                    //Setting the player positions between col 0 - 6
                    switch (playerParty_.combatDistance)
                    {
                        case GroupCombatDistance.Far://0-2
                            playerColShift = 0;
                            break;
                        case GroupCombatDistance.Medium://2-4
                            playerColShift = 2;
                            break;
                        case GroupCombatDistance.Close://4-6
                            playerColShift = 4;
                            break;
                    }

                    //Setting the enemy positions between col 10-13
                    enemyColShift0 += 10;
                    enemyColShift1 += 10;
                    enemyColShift2 += 10;
                    enemyColShift3 += 10;
                }
                break;

            case EnemyCombatPosition.RangedFlanking:
                {
                    //Setting the player positions between col 5-8
                    switch (playerParty_.combatDistance)
                    {
                        case GroupCombatDistance.Far://5-7
                            playerColShift = 5;
                            break;
                        case GroupCombatDistance.Medium://6-8
                            playerColShift = 6;
                            break;
                        case GroupCombatDistance.Close://6-8
                            playerColShift = 6;
                            break;
                    }

                    //Setting the enemy positions split between cols 0-1 and cols 12-13
                    enemyColShift0 += 1;//Col 1
                    enemyColShift1 += 3;//Col 12
                    enemyColShift2 += -2;//Col 0
                    enemyColShift3 += 10;//Col 13
                }
                break;

            case EnemyCombatPosition.RangedBehind:
                {
                    //Setting the player positions between col 7-11
                    switch (playerParty_.combatDistance)
                    {
                        case GroupCombatDistance.Far://7-9
                            playerColShift = 7;
                            break;
                        case GroupCombatDistance.Medium://9-11
                            playerColShift = 9;
                            break;
                        case GroupCombatDistance.Close://11-13
                            playerColShift = 11;
                            break;
                    }

                    //Setting the enemy positions between col 0-3, so no change
                }
                break;
        }
        
        //After we've found the column shifts, we loop through and set the player positions
        this.playerCharactersInCombat.Clear();
        foreach(Character playerChar in playerParty_.charactersInParty)
        {
            //Offsetting the player position column from the starting position
            playerChar.charCombatStats.gridPositionCol = playerChar.charCombatStats.startingPositionCol + playerColShift;
            playerChar.charCombatStats.gridPositionRow = playerChar.charCombatStats.startingPositionRow;

            //Adding the current character to the list of player characters
            this.playerCharactersInCombat.Add(playerChar);
        }

        //Now we set the enemy positions based on the column shifts
        this.enemyCharactersInCombat.Clear();
        foreach(EncounterEnemy enemyChar in enemyParty_.enemies)
        {
            //Creating an instance of the enemy prefab
            GameObject createdEnemy = Object.Instantiate(enemyChar.enemyCreature.gameObject, enemyParty_.transform.position, new Quaternion());
            this.enemyCharactersInCombat.Add(createdEnemy.GetComponent<Character>());
            //Getting a reference to the enemy's combat stats component
            CombatStats enemyCombatStats = createdEnemy.GetComponent<CombatStats>();

            //If this enemy's column position is random
            if(enemyChar.randomCol)
            {
                enemyCombatStats.gridPositionCol = Random.Range(0, 3);
            }
            //If this enemy's column position isn't random
            else
            {
                enemyCombatStats.gridPositionCol = enemyChar.specificCol;
            }

            //If this enemy's row position is random
            if(enemyChar.randomRow)
            {
                enemyCombatStats.gridPositionRow = Random.Range(0, 4);
            }
            //If this enemy's row position isn't random
            else
            {
                enemyCombatStats.gridPositionRow = enemyChar.specificRow;
            }

            //offsetting the enemy column position based on what their default position is
            switch(enemyCombatStats.gridPositionCol)
            {
                case 0:
                    enemyCombatStats.gridPositionCol = enemyColShift0;
                    break;
                case 1:
                    enemyCombatStats.gridPositionCol = enemyColShift1;
                    break;
                case 2:
                    enemyCombatStats.gridPositionCol = enemyColShift2;
                    break;
                case 3:
                    enemyCombatStats.gridPositionCol = enemyColShift3;
                    break;
                default:
                    //This case SHOULDN'T happen, but if it does, we treat it like the character's col is 0
                    enemyCombatStats.gridPositionCol = enemyColShift0;
                    break;
            }
            
            //Looping through all of the enemies created so far
            for(int i = 0; i < this.enemyCharactersInCombat.Count - 1; ++i)
            {
                //If the enemy we've just added shares the same combat row and column as another enemy
                if(enemyCombatStats.gameObject != this.enemyCharactersInCombat[i].gameObject &&
                    enemyCombatStats.gridPositionCol == this.enemyCharactersInCombat[i].charCombatStats.gridPositionCol &&
                    enemyCombatStats.gridPositionRow == this.enemyCharactersInCombat[i].charCombatStats.gridPositionRow)
                {
                    //The column shift amount for each column loop
                    int cShift = 0;

                    //Looping through all of the combat positions characters can be in
                    for(int c = 0; c < 4; ++c)
                    {
                        //Setting the column shift for this loop
                        if(c == 0)
                        {
                            cShift = enemyColShift0;
                        }
                        else if(c == 1)
                        {
                            cShift = enemyColShift1;
                        }
                        else if(c == 2)
                        {
                            cShift = enemyColShift2;
                        }
                        else if(c == 3)
                        {
                            cShift = enemyColShift3;
                        }

                        for(int r = 0; r < 8; ++r)
                        {
                            //Bool to track if the current position is empty
                            bool emptyPos = true;

                            //Looping through each enemy in the combat
                            foreach(Character ec in this.enemyCharactersInCombat)
                            {
                                //Making sure the enemy isn't somehow null or the enemy we've just created
                                if(ec != null && ec.gameObject != enemyCombatStats.gameObject)
                                {
                                    //If the tile isn't empty, we break out of this foreach loop
                                    if(ec.charCombatStats.gridPositionCol == cShift &&
                                        ec.charCombatStats.gridPositionRow == r)
                                    {
                                        emptyPos = false;
                                        break;
                                    }
                                }
                            }

                            //If we make it through all of the enemies without finding an overlap
                            if(emptyPos)
                            {
                                //Setting this enemy's position col and row to the empty position
                                enemyCombatStats.gridPositionCol = cShift;
                                enemyCombatStats.gridPositionRow = r;

                                //Breaking the row loop, col loop, and loop for checking other enemy positions
                                c = 10;
                                i = this.enemyCharactersInCombat.Count + 1;
                                break;
                            }
                        }
                    }
                }
            }
        }
    }


    //Function called from InitializeCombat. Creates all of the CombatCharacterSprite objects for the player characters and enemies
    private void CreateCharacterSprites()
    {
        //Making sure there are no more character sprites from previous combats on the screen
        foreach(CharacterSpriteBase cSprite in this.characterSpriteList)
        {
            Destroy(cSprite.gameObject);
        }
        this.characterSpriteList.Clear();
        
        //Looping through each player character in this combat
        foreach (Character playerChar in this.playerCharactersInCombat)
        {
            //Creating a new instance of the character sprite prefab
            GameObject newCharSprite = GameObject.Instantiate(playerChar.charSprites.allSprites.spriteBase.gameObject);
            
            //Getting the CharacterSpriteBase component reference
            CharacterSpriteBase newCharSpriteBase = newCharSprite.GetComponent<CharacterSpriteBase>();

            //Telling the sprite base to use the given character's sprites
            newCharSpriteBase.SetSpriteImages(playerChar.charSprites.allSprites, playerChar.charInventory);
            newCharSpriteBase.SetDirectionFacing(DirectionFacing.Right);

            //Finding the combat tile that the current player character is on
            CombatTile playerTile = this.FindCharactersTile(playerChar);

            //Parenting the game object to this object so it shows up on our canvas
            newCharSprite.transform.SetParent(this.transform);
            
            //Setting the position for the character sprite
            newCharSprite.transform.position = playerTile.transform.position;

            //Setting the character that the sprite base represents
            newCharSpriteBase.ourCharacter = playerChar;

            //Adding the character sprite base to our list
            this.characterSpriteList.Add(newCharSpriteBase);
        }

        //Looping through each enemy character in this combat
        foreach (Character enemyChar in this.enemyCharactersInCombat)
        {
            //Creating a new instance of the character sprite prefab
            GameObject newCharSprite = GameObject.Instantiate(enemyChar.charSprites.allSprites.spriteBase.gameObject);

            //Getting the CombatCharacterSprite component reference
            CharacterSpriteBase newCharSpriteBase = newCharSprite.GetComponent<CharacterSpriteBase>();

            //Telling the sprite base to use the given character's sprites
            newCharSpriteBase.SetSpriteImages(enemyChar.charSprites.allSprites, enemyChar.charInventory);

            //Finding the combat tile that the current enemy character is on
            CombatTile enemyTile = this.FindCharactersTile(enemyChar);

            //Getting the direction that this enemy initially faces
            DirectionFacing direction = DirectionFacing.Left;
            if (enemyTile.col < (this.combatTileGrid.Count / 2))
            {
                direction = DirectionFacing.Right;
            }
            newCharSpriteBase.SetDirectionFacing(direction);

            //Parenting the game object to this object so it shows up on our canvas
            newCharSprite.transform.SetParent(this.transform);

            //Setting the position for the character sprite
            newCharSprite.transform.position = enemyTile.transform.position;

            //Setting the character that the sprite base represents
            newCharSpriteBase.ourCharacter = enemyChar;

            //Adding the character sprite base to our list
            this.characterSpriteList.Add(newCharSpriteBase);
        }

        //Sorting the sprites so that they appear in front of each other correctly
        this.UpdateCharacterSpriteOrder();
    }


    //Function called externally to get the CharacterSpriteBase component for the given character
    public CharacterSpriteBase GetCharacterSprite(Character charToLookFor_)
    {
        //Looping through all of the Character Sprites
        foreach(CharacterSpriteBase cSprite in this.characterSpriteList)
        {
            //If we find the character, their sprite is returned
            if(cSprite.ourCharacter == charToLookFor_)
            {
                return cSprite;
            }
        }

        //If we make it out of the loop, nobody was found, so we return null
        return null;
    }


    //Function called from MoveAction.cs to update the sprite positions for each character combat sprite to make them overlap correctly
    public void UpdateCharacterSpriteOrder()
    {
        //Looping through for each row we have
        for(int r = 0; r < this.combatTileGrid[0].Count; ++r)
        {
            //Looping through each character sprite in this combat encounter
            foreach(CharacterSpriteBase cSprite in this.characterSpriteList)
            {
                //If the character for the current combat sprite is positioned on the row we're checking, we move it to the front
                if(cSprite.ourCharacter.charCombatStats.gridPositionRow == r)
                {
                    cSprite.transform.SetAsLastSibling();
                }
            }
        }

        //Setting the info display above all of the other sprites
        this.ourInfoDisplay.transform.SetAsLastSibling();
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


    //Function called internally to hilight the occupied tiles to show where characters are
    public void UpdateCombatTilePositions()
    {
        //Looping through and setting the character objects on each tile
        foreach(Character currentChar in this.playerCharactersInCombat)
        {
            this.combatTileGrid[currentChar.charCombatStats.gridPositionCol][currentChar.charCombatStats.gridPositionRow].SetObjectOnTile(currentChar.gameObject, TileObjectType.Player);
        }

        //Looping through and setting the enemy objects on each tile
        foreach(Character currentEnemy in this.enemyCharactersInCombat)
        {
            this.combatTileGrid[currentEnemy.charCombatStats.gridPositionCol][currentEnemy.charCombatStats.gridPositionRow].SetObjectOnTile(currentEnemy.gameObject, TileObjectType.Enemy);
        }
    }


    //Function called from InitiateCombat to set the background Image object's sprite
    private void SetBackgroundImage(LandType tileType_)
    {
        //Looping through each background image types until we find the correct land type
        foreach(BackgroundImageTypes bgType in this.tileTypeBackgrounds)
        {
            //If we find the matching type, the associated image is set to the background image's sprite
            if(bgType.tileType == tileType_)
            {
                this.backgroundImageObject.sprite = bgType.backgroundImage;
                return;
            }
        }
    }


    //Function called from AttackAction.PerformAction to show damage dealt to a character at the given tile
    public void DisplayDamageDealt(float timeDelay_, int damage_, DamageType type_, CombatTile damagedCharTile_, bool isCrit_, bool isHeal_ = false)
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
        this.UpdateHealthBars();
    }


    //Function called from DisplayDamageDealt to update all character's health sliders
    private void UpdateHealthBars()
    {
        //Looping through each player character's initiative slider
        for (int p = 0; p < this.playerCharactersInCombat.Count; ++p)
        {
            //If the character isn't already dead
            if (this.playerInitiativeSliders[p].healthSlider.value > 0)
            {
                //Setting the health slider to show the current health based on the max health
                this.playerInitiativeSliders[p].healthSlider.maxValue = this.playerCharactersInCombat[p].charPhysState.maxHealth;
                this.playerInitiativeSliders[p].healthSlider.value = this.playerCharactersInCombat[p].charPhysState.currentHealth;

                //If this character is dead, their initiative slider is set to 0 so they can't act
                if (this.playerCharactersInCombat[p].charPhysState.currentHealth == 0)
                {
                    this.playerInitiativeSliders[p].initiativeSlider.value = 0;

                    this.playerInitiativeSliders[p].background.color = Color.grey;

                    //Looping through and clearing all of the effects on the dead character
                    for (int e = 0; e < this.playerCharactersInCombat[p].charCombatStats.combatEffects.Count; ++e)
                    {
                        this.playerCharactersInCombat[p].charCombatStats.combatEffects[e].RemoveEffect();
                        e -= 1;
                    }

                    //If this character is the acting character
                    if (this.playerCharactersInCombat[p] == this.actingCharacters[0])
                    {
                        //Their turn is ended
                        this.EndActingCharactersTurn();
                    }
                    //Otherwise we check to see if they'll be acting soon
                    else
                    {
                        //If this character is in line to act, they are removed from the list
                        for (int a = 1; a < this.actingCharacters.Count; ++a)
                        {
                            if (this.actingCharacters[a] == this.playerCharactersInCombat[p])
                            {
                                this.actingCharacters.RemoveAt(a);
                                a -= 1;
                            }
                        }
                    }
                }
            }
            //If the character is dead but their initiative slider isn't grey, we make it grey
            else if (this.playerInitiativeSliders[p].background.color != Color.grey)
            {
                this.playerInitiativeSliders[p].background.color = Color.grey;
            }
        }

        //Looping through each enemy's initiative slider
        for (int e = 0; e < this.enemyCharactersInCombat.Count; ++e)
        {
            //If the enemy isn't already dead
            if (this.enemyInitiativeSliders[e].healthSlider.value > 0)
            {
                //Setting the health slider to show the current health based on the max health
                this.enemyInitiativeSliders[e].healthSlider.maxValue = this.enemyCharactersInCombat[e].charPhysState.maxHealth;
                this.enemyInitiativeSliders[e].healthSlider.value = this.enemyCharactersInCombat[e].charPhysState.currentHealth;

                //If this enemy is dead, their initiative slider is set to 0 so they can't act
                if (this.enemyCharactersInCombat[e].charPhysState.currentHealth == 0)
                {
                    this.enemyInitiativeSliders[e].initiativeSlider.value = 0;

                    this.enemyInitiativeSliders[e].background.color = Color.grey;

                    //Looping through and clearing all of the effects on the dead character
                    for (int ef = 0; ef < this.enemyCharactersInCombat[e].charCombatStats.combatEffects.Count; ++ef)
                    {
                        this.enemyCharactersInCombat[e].charCombatStats.combatEffects[ef].RemoveEffect();
                        ef -= 1;
                    }

                    //If this character is in line to act, they are removed from the list
                    for (int a = 1; a < this.actingCharacters.Count; ++a)
                    {
                        if (this.actingCharacters[a] == this.enemyCharactersInCombat[e])
                        {
                            this.actingCharacters.RemoveAt(a);
                            a -= 1;
                        }
                    }

                    //Looping through all enemy characters to check their health
                    foreach (Character enemy in this.enemyCharactersInCombat)
                    {
                        //If at least 1 enemy is still alive, we break out of the loop
                        if (enemy.charPhysState.currentHealth > 0)
                        {
                            return;
                        }
                    }
                    //If we get through the loop, that means that all enemies are dead and combat is over

                    //Perform the unity event after the action so we can hide some UI elements
                    this.eventAfterActionPerformed.Invoke();

                    //Looping through and triggering all effects on the acting character for when their turn ends
                    if (this.actingCharacters.Count > 0)
                    {
                        foreach (Effect ef in this.actingCharacters[0].charCombatStats.combatEffects)
                        {
                            ef.EffectOnEndOfTurn();
                        }
                    }

                    //Clearing the highlighted area showing the previously used action's range
                    this.ClearCombatTileHighlights();
                    this.SetWaitTime(2.5f, CombatState.EndCombat);
                }
            }
            //If the enemy is dead but their initiative slider isn't grey, we make it grey
            else if (this.enemyInitiativeSliders[e].background.color != Color.grey)
            {
                this.enemyInitiativeSliders[e].background.color = Color.grey;
            }
        }
    }


    


    //Function called externally to find out which combat tile the given character is on
    public CombatTile FindCharactersTile(Character characterToFind_)
    {
        //Making sure the given character is in the current combat encounter
        if(!this.playerCharactersInCombat.Contains(characterToFind_) && !this.enemyCharactersInCombat.Contains(characterToFind_))
        {
            return null;
        }

        //Getting less confusing references to the character's row/column position
        int row = characterToFind_.charCombatStats.gridPositionRow;
        int col = characterToFind_.charCombatStats.gridPositionCol;

        return this.combatTileGrid[col][row];
    }


    //Function called from CombatTile.cs to perform the selected action in the CombatActionPanelUI
    public void PerformActionAtClickedTile(CombatTile tileClicked_)
    {
        //If the action being performed is a movement action and the tile clicked isn't empty, nothing happens
        if (CombatActionPanelUI.globalReference.selectedAction.GetComponent<MoveAction>() && tileClicked_.objectOnThisTile != null)
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
        this.ClearCombatTileHighlights();
    }


    //Function called from EnemyCombatAI_Basic.cs to perform an enemy's action at the given tile
    public void PerformEnemyActionOnTile(CombatTile tileClicked_, Action enemyAction_)
    {
        //If the action being performed is a movement action and the tile chosen isn't empty, nothing happens
        if(enemyAction_.GetType() == typeof(MoveAction))
        {
            if (tileClicked_.objectOnThisTile != null)
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
        if(CombatActionPanelUI.globalReference.selectedAction != null)
        {
            //If the selected action is a move action, we need to clear all highlighted tiles in the movement path first
            if(CombatActionPanelUI.globalReference.selectedAction.GetComponent<MoveAction>())
            {
                CombatActionPanelUI.globalReference.selectedAction.GetComponent<MoveAction>().ClearMovePathHighlights();
            }

            Destroy(CombatActionPanelUI.globalReference.selectedAction.gameObject);
        }

        //Looping through and triggering all effects on the acting character for when their turn ends
        foreach(Effect e in this.actingCharacters[0].charCombatStats.combatEffects)
        {
            e.EffectOnEndOfTurn();
        }

        //Resets the acting character's initiative and removes them from the list of acting characters
        if (this.playerCharactersInCombat.Contains(this.actingCharacters[0]))
        {
            int selectedCharIndex = this.playerCharactersInCombat.IndexOf(this.actingCharacters[0]);
            //Resetting their initiative slider's color
            this.playerInitiativeSliders[selectedCharIndex].background.color = this.inactivePanelColor;
            //Resetting their initiative slider
            this.playerInitiativeSliders[selectedCharIndex].initiativeSlider.value = 0;
            //Removing the currently acting character
            this.actingCharacters.Remove(this.actingCharacters[0]);
        }
        else if(this.enemyCharactersInCombat.Contains(this.actingCharacters[0]))
        {
            int selectedEnemyIndex = this.enemyCharactersInCombat.IndexOf(this.actingCharacters[0]);
            //Resetting their initiative slider's color
            this.enemyInitiativeSliders[selectedEnemyIndex].background.color = this.inactivePanelColor;
            //Resetting their initiative slider
            this.enemyInitiativeSliders[selectedEnemyIndex].initiativeSlider.value = 0;
            //Removing the currently acting character
            this.actingCharacters.Remove(this.actingCharacters[0]);
        }

        //Clearing the highlighted area showing the previously used action's range
        this.ClearCombatTileHighlights();

        //Have the combat manager wait a moment before going back to increasing initiatives
        this.SetWaitTime(1, CombatState.IncreaseInitiative);
    }


    //Function called from Action.cs and Effect.cs when a player character performs an action
    public void ApplyActionThreat(Character actingCharacter_, Character targetCharacter_, int threatToAdd_, bool increaseForAllEnemies_)
    {
        //If the currently acting character is an enemy, nothing happens. Enemies can't increase their own threat
        if(this.enemyCharactersInCombat.Contains(actingCharacter_))
        {
            return;
        }

        //If the target character is empty or a player character
        if(targetCharacter_ == null || this.playerCharactersInCombat.Contains(targetCharacter_))
        {
            //If threat is increased for all enemies, we can add threat. If it isn't increased for all enemies, we do nothing
            if(increaseForAllEnemies_)
            {
                //Looping through each enemy character in combat
                foreach(Character enemy in this.enemyCharactersInCombat)
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
        else if(this.enemyCharactersInCombat.Contains(targetCharacter_))
        {
            //If we increase threat for all enemies
            if(increaseForAllEnemies_)
            {
                //Looping through each enemy character in combat
                foreach (Character enemy in this.enemyCharactersInCombat)
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
        
        //Getting the character sprite for the dead character
        CharacterSpriteBase deadSprite = this.GetCharacterSprite(data_.characterDeath.deadCharacter);
        //Removing the sprite from our list and destroying it
        this.deadCharacterSprites.Add(deadSprite);
        //Freeing up the tile that the dead character is on
        CombatTile deadCharTile = this.combatTileGrid[data_.characterDeath.deadCharacter.charCombatStats.gridPositionCol][data_.characterDeath.deadCharacter.charCombatStats.gridPositionRow];
        deadCharTile.SetObjectOnTile(null, TileObjectType.Nothing);

        //If the dead character is the acting character, we end it's turn
        if (this.actingCharacters.Count > 0 && this.actingCharacters[0] == data_.characterDeath.deadCharacter)
        {
            this.tileHandler.tileHighlight.enabled = false;
            this.EndActingCharactersTurn();

            //If the dead character is a player character, we set their initiative slider to grey
            if(this.playerCharactersInCombat.Contains(data_.characterDeath.deadCharacter))
            {
                int playerIndex = this.playerCharactersInCombat.IndexOf(data_.characterDeath.deadCharacter);
                this.playerInitiativeSliders[playerIndex].background.color = Color.grey;
            }
            //If the dead character is an enemy, we set their initiative slider to grey
            else if(this.enemyCharactersInCombat.Contains(data_.characterDeath.deadCharacter))
            {
                int enemyIndex = this.enemyCharactersInCombat.IndexOf(data_.characterDeath.deadCharacter);
                this.enemyInitiativeSliders[enemyIndex].background.color = Color.grey;
            }
        }
        //If the dead character was waiting to act, we remove it from the list of acting characters
        else if(this.actingCharacters.Contains(data_.characterDeath.deadCharacter))
        {
            this.actingCharacters.Remove(data_.characterDeath.deadCharacter);
        }

        //Updating the health bars
        this.UpdateHealthBars();

        //If this character was a player character
        if(this.playerCharactersInCombat.Contains(data_.characterDeath.deadCharacter))
        {
            //Updating the quest tracker to see if the dead ally is an escort character
            QuestTracker.globalReference.CheckForDeadEscortCharacter(data_.characterDeath.deadCharacter);

            //Looping through all player characters to see if they're all dead
            bool allPlayersAreDead = true;
            for(int p = 0; p < this.playerCharactersInCombat.Count; ++p)
            {
                //If this player character has any health, we break the loop and nothing happens
                if(this.playerCharactersInCombat[p].charPhysState.currentHealth > 0)
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
        else if(this.enemyCharactersInCombat.Contains(data_.characterDeath.deadCharacter))
        {
            //Updating the quest tracker to see if the dead enemy is a quest target
            QuestTracker.globalReference.UpdateKillQuests(data_.characterDeath.deadCharacter);

            //Looping through all enemy characters to see if they're all dead
            bool allEnemiesAreDead = true;
            for(int e = 0; e < this.enemyCharactersInCombat.Count; ++e)
            {
                //If this enemy has any health, we break the loop and nothing happens
                if(this.enemyCharactersInCombat[e].charPhysState.currentHealth > 0)
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

