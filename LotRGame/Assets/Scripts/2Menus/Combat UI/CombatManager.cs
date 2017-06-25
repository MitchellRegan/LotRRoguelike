using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CombatManager : MonoBehaviour
{
    //Static reference to this combat manager
    public static CombatManager globalReference;

    //Enum for the state of this combat manager to decide what to do on update
    private enum combatState {Wait, IncreaseInitiative, SelectAction, PlayerInput};
    private combatState currentState = combatState.Wait;

    //The amount of time that has passed while waiting
    private float waitTime = 0;
    //The combat state to switch to after the wait time is up
    private combatState stateAfterWait = combatState.IncreaseInitiative;

    //Reference to the characters whose turn it is to act. It's a list because multiple characters could have the same initiative
    public List<Character> actingCharacters = null;

    //2D List of all combat tiles in the combat screen map. Col[row}
    public List<List<CombatTile>> combatTileGrid;

    //The mathematical interpolator that we'll use to find damage using min/max ranges
    private Interpolator ourInterpolator;

    //The list of all characters involved in this combat
    [HideInInspector]
    public List<Character> playerCharactersInCombat;
    [HideInInspector]
    public List<Character> enemyCharactersInCombat;

    //The canvas that is activated at the start of combat
    public Canvas combatCanvas;
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



	// Function called when this object is created
	private void Awake ()
    {
        //Setting the static reference
        if(globalReference == null)
        {
            globalReference = this;
        }
        //If a static reference already exists, this component is destroyed
        else
        {
            Destroy(this);
        }

        //Initializing our interpolator
        this.ourInterpolator = new Interpolator();

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

        this.playerCharactersInCombat = new List<Character>();
        this.enemyCharactersInCombat = new List<Character>();
    }


    //Function called every frame
    private void Update()
    {
        //Determine what we do based on the current state
        switch(this.currentState)
        {
            //Nothing, waiting for player feedback
            case combatState.PlayerInput:
                return;
            //Counting down the wait time
            case combatState.Wait:
                this.waitTime -= Time.deltaTime;
                //If the timer is up, the state changes to the one that was previously designated
                if(this.waitTime <= 0)
                {
                    this.currentState = this.stateAfterWait;
                }
                break;
            //Adding each character's attack speed to their current initative 
            case combatState.IncreaseInitiative:
                this.IncreaseInitiative();
                break;

            //Hilighting the selected character whose turn it is
            case combatState.SelectAction:
                //If the selected character is a player
                if (this.playerCharactersInCombat.Contains(this.actingCharacters[0]))
                {
                    //Hilighting the slider of the player character whose turn it is
                    int selectedCharIndex = this.playerCharactersInCombat.IndexOf(this.actingCharacters[0]);
                    this.playerInitiativeSliders[selectedCharIndex].background.color = this.actingCharacterColor;
                    //Displaying the action panel so players can decide what to do
                    this.showPlayerActions.Invoke();
                    //Default to showing the acting character's standard actions
                    CombatActionPanelUI.globalReference.DisplayActionTypes(0);
                    //Now we wait for player input
                    this.currentState = combatState.PlayerInput;
                }
                //If the selected character is an enemy
                else
                {
                    int selectedEnemyIndex = this.enemyCharactersInCombat.IndexOf(this.actingCharacters[0]);
                    this.enemyInitiativeSliders[selectedEnemyIndex].background.color = this.actingEnemyColor;
                    Debug.Log("Combat Manager.Update. Enemies need AI here");
                    //Resetting this enemy's initiative for now. Can't do much until I get AI in
                    this.enemyInitiativeSliders[selectedEnemyIndex].background.color = this.inactivePanelColor;
                    this.enemyInitiativeSliders[selectedEnemyIndex].initiativeSlider.value = 0;
                    if (this.actingCharacters.Count == 1)
                    {
                        this.actingCharacters.Clear();
                    }
                    else
                    {
                        this.actingCharacters.RemoveAt(0);
                    }
                    this.currentState = combatState.IncreaseInitiative;
                }
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
            }
        }
    }


    //Function called externally from LandTile.cs to initiate combat
    public void InitiateCombat(LandType combatLandType_, PartyGroup charactersInCombat_, EnemyEncounter encounter_)
    {
        //Activating the combat canvas so we can show everything
        this.combatCanvas.enabled = true;

        //Looping through and resetting the combat tiles
        for(int c = 0; c < this.combatTileGrid.Count; ++c)
        {
            for(int r = 0; r < this.combatTileGrid[c].Count; ++r)
            {
                this.combatTileGrid[c][r].ResetTile();
            }
        }

        //Setting the background image
        this.SetBackgroundImage(combatLandType_);

        //Clearing the list of player characters and adding all of the ones given
        this.playerCharactersInCombat.Clear();
        foreach(Character c in charactersInCombat_.charactersInParty.Keys)
        {
            this.playerCharactersInCombat.Add(c);

            //Setting each character's grid position
            c.charCombatStats.gridPositionCol = Mathf.RoundToInt(charactersInCombat_.charactersInParty[c].x);
            c.charCombatStats.gridPositionRow = Mathf.RoundToInt(charactersInCombat_.charactersInParty[c].y);
        }

        //Clearing the list of enemy characters and adding all of the ones from the given encounter
        this.enemyCharactersInCombat.Clear();
        foreach(EncounterEnemy enemy in encounter_.enemies)
        {
            GameObject createdEnemy = Object.Instantiate(enemy.enemyCreature.gameObject, encounter_.transform.position, new Quaternion());
            this.enemyCharactersInCombat.Add(createdEnemy.GetComponent<Character>());
            //Getting a reference to the enemy's combat stats component
            CombatStats enemyCombatStats = createdEnemy.GetComponent<CombatStats>();

            //Setting the enemy's column position
            switch(enemy.colArea)
            {
                case EncounterEnemy.colPositionAreas.Back:
                    enemyCombatStats.gridPositionCol = 12;
                    break;
                case EncounterEnemy.colPositionAreas.Front:
                    enemyCombatStats.gridPositionCol = 10;
                    break;
                case EncounterEnemy.colPositionAreas.Middle:
                    enemyCombatStats.gridPositionCol = 11;
                    break;
                case EncounterEnemy.colPositionAreas.Random:
                    enemyCombatStats.gridPositionCol = Mathf.RoundToInt(Random.Range(10,12));
                    break;
                case EncounterEnemy.colPositionAreas.SpecificPos:
                    enemyCombatStats.gridPositionCol = enemy.specificCol + 10;
                    break;
            }

            //Setting the enemy's row position
            switch(enemy.rowArea)
            {
                case EncounterEnemy.rowPositionAreas.Top:
                    enemyCombatStats.gridPositionRow = Mathf.RoundToInt(Random.Range(0,1));
                    break;
                case EncounterEnemy.rowPositionAreas.Middle:
                    enemyCombatStats.gridPositionRow = Mathf.RoundToInt(Random.Range(2, 5));
                    break;
                case EncounterEnemy.rowPositionAreas.Bottom:
                    enemyCombatStats.gridPositionRow = Mathf.RoundToInt(Random.Range(6, 7));
                    break;
                case EncounterEnemy.rowPositionAreas.Random:
                    enemyCombatStats.gridPositionRow = Mathf.RoundToInt(Random.Range(0, 7));
                    break;
                case EncounterEnemy.rowPositionAreas.SpecificPos:
                    enemyCombatStats.gridPositionRow = enemy.specificRow;
                    break;
            }
        }

        //Looping through and setting all of the player initiative bars to display the correct character
        for (int p = 0; p < this.playerInitiativeSliders.Count; ++p)
        {
            //if the current index isn't outside the count of characters
            if(p < this.playerCharactersInCombat.Count)
            {
                //The initiative slider is shown
                this.playerInitiativeSliders[p].initiativeSlider.gameObject.SetActive(true);
                //Makes sure the initiative is set to 0
                this.playerInitiativeSliders[p].initiativeSlider.value = 0;
                //Setting the character's name
                this.playerInitiativeSliders[p].characterName.text = this.playerCharactersInCombat[p].firstName;
                //Making the background panel set to the inactive color
                this.playerInitiativeSliders[p].background.color = this.inactivePanelColor;

            }
            //If the index is outside the count of characters
            else
            {
                //The initiative slider is hidden
                this.playerInitiativeSliders[p].background.gameObject.SetActive(false);
            }
        }
        
        //Looping through and setting all of the enemy initiative bars to display the correct enemy
        for (int e = 0; e < this.enemyInitiativeSliders.Count; ++e)
        {
            //if the current index isn't outside the count of enemies
            if (e < this.enemyCharactersInCombat.Count)
            {
                //The initiative slider is shown
                this.enemyInitiativeSliders[e].initiativeSlider.gameObject.SetActive(true);
                //Makes sure the initiative is set to 0
                this.enemyInitiativeSliders[e].initiativeSlider.value = 0;
                //Setting the enemy's name
                this.enemyInitiativeSliders[e].characterName.text = this.enemyCharactersInCombat[e].firstName;
                //Making the background panel set to the inactive color
                this.enemyInitiativeSliders[e].background.color = this.inactivePanelColor;
            }
            //If the index is outside the count of enemies
            else
            {
                //The initiative slider is hidden
                this.enemyInitiativeSliders[e].background.gameObject.SetActive(false);
            }
        }

        //Setting each character on the tile positions
        this.UpdateCombatTilePositions();

        //Setting the health bars to display the correct initiatives
        this.UpdateHealthBars();

        //Setting the state to start increasing initiatives
        this.currentState = combatState.IncreaseInitiative;
    }


    //Function called to set the amount of time to wait
    private void SetWaitTime(float timeToWait_, combatState stateAfterWait_ = combatState.IncreaseInitiative)
    {
        this.waitTime = timeToWait_;
        this.currentState = combatState.Wait;
    }


    //Function called internally to hilight the occupied tiles to show where characters are
    private void UpdateCombatTilePositions()
    {
        //Looping through and setting the character objects on each tile
        foreach(Character currentChar in this.playerCharactersInCombat)
        {
            this.combatTileGrid[currentChar.charCombatStats.gridPositionCol][currentChar.charCombatStats.gridPositionRow].SetObjectOnTile(currentChar.gameObject, CombatTile.ObjectType.Player);
        }

        //Looping through and setting the enemy objects on each tile
        foreach(Character currentEnemy in this.enemyCharactersInCombat)
        {
            this.combatTileGrid[currentEnemy.charCombatStats.gridPositionCol][currentEnemy.charCombatStats.gridPositionRow].SetObjectOnTile(currentEnemy.gameObject, CombatTile.ObjectType.Enemy);
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
    public enum DamageType { Physical, Magic, Fire, Water, Electric, Wind, Rock, Light, Dark };
    public void DisplayDamageDealt(int damage_, DamageType type_, CombatTile damagedCharTile_, bool isCrit_)
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
        newDamageText.SetDamageToDisplay(damage_, type_, damagedCharTile_.transform.position, isCrit_);

        //Checking to see if the attacked character is dead
        if(damagedCharTile_.objectOnThisTile.GetComponent<Character>().charPhysState.currentHealth == 0)
        {
            Debug.Log(damagedCharTile_.objectOnThisTile.name + " is dead!");
        }

        //Updating the health bars so we can see how much health characters have
        this.UpdateHealthBars();
    }


    //Function called from DisplayDamageDealt to update all character's health sliders
    private void UpdateHealthBars()
    {
        //Looping through each player character's initiative slider
        for (int p = 0; p < this.playerCharactersInCombat.Count; ++p)
        {
            //Setting the health slider to show the current health based on the max health
            this.playerInitiativeSliders[p].healthSlider.maxValue = this.playerCharactersInCombat[p].charPhysState.maxHealth;
            this.playerInitiativeSliders[p].healthSlider.value = this.playerCharactersInCombat[p].charPhysState.currentHealth;

            //If this character is dead, their initiative slider is set to 0 so they can't act
            if(this.playerCharactersInCombat[p].charPhysState.currentHealth == 0)
            {
                this.playerInitiativeSliders[p].initiativeSlider.value = 0;

                this.playerInitiativeSliders[p].background.color = Color.grey;

                //If this character is in line to act, they are removed from the list
                for(int a = 1; a < this.actingCharacters.Count; ++a)
                {
                    if(this.actingCharacters[a] == this.playerCharactersInCombat[p])
                    {
                        this.actingCharacters.RemoveAt(a);
                        a -= 1;
                    }
                }
            }
        }

        //Looping through each enemy's initiative slider
        for (int e = 0; e < this.enemyCharactersInCombat.Count; ++e)
        {
            //Setting the health slider to show the current health based on the max health
            this.enemyInitiativeSliders[e].healthSlider.maxValue = this.enemyCharactersInCombat[e].charPhysState.maxHealth;
            this.enemyInitiativeSliders[e].healthSlider.value = this.enemyCharactersInCombat[e].charPhysState.currentHealth;

            //If this enemy is dead, their initiative slider is set to 0 so they can't act
            if (this.enemyCharactersInCombat[e].charPhysState.currentHealth == 0)
            {
                this.enemyInitiativeSliders[e].initiativeSlider.value = 0;

                this.enemyInitiativeSliders[e].background.color = Color.grey;

                //If this character is in line to act, they are removed from the list
                for (int a = 1; a < this.actingCharacters.Count; ++a)
                {
                    if (this.actingCharacters[a] == this.enemyCharactersInCombat[e])
                    {
                        this.actingCharacters.RemoveAt(a);
                        a -= 1;
                    }
                }
            }
        }
    }


    //Function called from AttackAction.PerformAction to show that an attack missed
    public void DisplayMissedAttack(CombatTile attackedCharTile_)
    {
        //Creating an instance of the damage text object prefab
        GameObject newDamageDisplay = GameObject.Instantiate(this.damageTextPrefab.gameObject);
        //Parenting the damage text object to this object's transform
        newDamageDisplay.transform.SetParent(this.transform);
        //Getting the DamageText component reference
        DamageText newDamageText = newDamageDisplay.GetComponent<DamageText>();
        //Setting the info for the text
        newDamageText.DisplayMiss(attackedCharTile_.transform.position);
    }


    //Function called from Update. Loops through all characters and increases their initiative
    private void IncreaseInitiative()
    {
        //Looping through each player character
        for(int p = 0; p < this.playerCharactersInCombat.Count; ++p)
        {
            //Making sure the current character isn't dead first
            if (this.playerCharactersInCombat[p].charPhysState.currentHealth > 0)
            {
                //Adding this character's initiative to the coorelating slider. The initiative is multiplied by the energy %
                CombatStats combatStats = this.playerCharactersInCombat[p].charCombatStats;
                this.playerInitiativeSliders[p].initiativeSlider.value += combatStats.currentInitiativeSpeed * (combatStats.currentState.currentEnergy / combatStats.currentState.maxEnergy);

                //If the slider is filled, this character is added to the acting character list
                if (this.playerInitiativeSliders[p].initiativeSlider.value >= this.playerInitiativeSliders[p].initiativeSlider.maxValue)
                {
                    this.actingCharacters.Add(this.playerCharactersInCombat[p]);
                }
            }
        }


        //Looping through each enemy character
        for(int e = 0; e < this.enemyCharactersInCombat.Count; ++e)
        {
            //Making sure the current enemy isn't dead first
            if (this.enemyCharactersInCombat[e].charPhysState.currentHealth > 0)
            {
                //Adding this enemy's initiative to the coorelating slider. The initiative is multiplied by the energy %
                CombatStats combatStats = this.enemyCharactersInCombat[e].charCombatStats;
                this.enemyInitiativeSliders[e].initiativeSlider.value += combatStats.currentInitiativeSpeed * (combatStats.currentState.currentEnergy / combatStats.currentState.maxEnergy);

                //If the slider is filled, this character is added to the acting character list
                if (this.enemyInitiativeSliders[e].initiativeSlider.value >= this.enemyInitiativeSliders[e].initiativeSlider.maxValue)
                {
                    this.actingCharacters.Add(this.enemyCharactersInCombat[e]);
                }
            }
        }
        
        //If there are any characters in the acting Characters list, the state changes so we stop updating initiative meters
        if(this.actingCharacters.Count != 0)
        {
            this.currentState = combatState.SelectAction;
        }
    }


    //Function called externally to find out which combat tile the given character is on
    public CombatTile FindCharactersTile(Character characterToFind_)
    {
        //Making sure the given character is in the current combat encounter
        if(!this.playerCharactersInCombat.Contains(characterToFind_))
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
        //Tells the action to be performed at the tile clicked
        CombatActionPanelUI.globalReference.selectedAction.PerformAction(tileClicked_);
        //Have this combat manager wait a bit before going back to increasing initiative because there could be animations
        this.SetWaitTime(3);
        //Perform the unity event after the action so we can hide some UI elements
        this.eventAfterActionPerformed.Invoke();
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

        //Clearing the highlighted area showing the previously used action's range
        this.ClearCombatTileHighlights();
    }
}

[System.Serializable]
public class BackgroundImageTypes
{
    //The type of land tile that combat is happening on
    public LandType tileType = LandType.Empty;
    //The background image associated with this tile type
    public Sprite backgroundImage;
}


//Class used in CombatManager.cs that represents an individual character's name/health/initiative panel
[System.Serializable]
public class InitiativePanel
{
    public Slider initiativeSlider;
    public Text characterName;
    public Image background;
    public Slider healthSlider;
}