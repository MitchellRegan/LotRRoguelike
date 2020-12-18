using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUIHandler : MonoBehaviour
{
    //Reference to the combat UI canvas and camera
    public Canvas combatUICanvas;
    public GameObject combatCamera;

    //List of all initiative panels for player characters and enemies
    public List<CombatInitiativePanel> playerPanels;
    public List<CombatInitiativePanel> enemyPanels;

    [Space(8)]

    //The colors for the background panels for who is acting
    public Color actingPlayerColor = Color.green;
    public Color actingEnemyColor = Color.red;
    public Color inactiveColor = Color.white;
    public Color highlightColor = Color.yellow;
    public Color deadColor = Color.gray;

    [Space(8)]

    //Reference to the object spawned to display how much damage was dealt
    public DamageText damageTextPrefab;
    //Image that blocks the player from performing actions before the current action is finished
    public Image actionBlocker;



    private void Start()
    {
        //Disabling the combat canvas so we know it's always off when the CamePlay scene starts
        this.combatUICanvas.enabled = false;
        this.combatCamera.SetActive(false);
    }


    //Function called from CombatManager.InitiateCombat to reset our initiative panels at the start of combat
    public void ResetForCombatStart()
    {
        //Displaying the combat UI canvas
        this.combatUICanvas.enabled = true;
        this.combatCamera.SetActive(true);

        Character currentChar;

        //Resetting all of the player panels
        for(int p = 0; p < this.playerPanels.Count; p++)
        {
            //Setting the current panel for a player
            if(p < CombatManager.globalReference.characterHandler.playerCharacters.Count)
            {
                currentChar = CombatManager.globalReference.characterHandler.playerCharacters[p];
                this.playerPanels[p].gameObject.SetActive(true);
                this.playerPanels[p].SetBackgroundColor(this.inactiveColor);
                this.playerPanels[p].nameText.text = currentChar.firstName;
                this.playerPanels[p].initiativeSlider.value = 0;
                this.playerPanels[p].healthSlider.maxValue = currentChar.charPhysState.maxHealth;
                this.playerPanels[p].healthSlider.value = currentChar.charPhysState.currentHealth;
            }
            //Hiding the current panel
            else
            {
                this.playerPanels[p].gameObject.SetActive(false);
            }
        }

        //Resetting all of the enemy panels
        for(int e = 0; e < this.enemyPanels.Count; e++)
        {
            //Setting the current panel for a player
            if (e < CombatManager.globalReference.characterHandler.enemyCharacters.Count)
            {
                currentChar = CombatManager.globalReference.characterHandler.enemyCharacters[e];

                this.playerPanels[e].gameObject.SetActive(true);
                this.playerPanels[e].SetBackgroundColor(this.inactiveColor);
                this.playerPanels[e].nameText.text = currentChar.firstName;
                this.playerPanels[e].initiativeSlider.value = 0;
                this.playerPanels[e].healthSlider.maxValue = currentChar.charPhysState.maxHealth;
                this.playerPanels[e].healthSlider.value = currentChar.charPhysState.currentHealth;
            }
            //Hiding the current panel
            else
            {
                this.enemyPanels[e].gameObject.SetActive(false);
            }
        }
    }


    //Function called from CombatManager.InitiateCombat to turn off the UI for the end of combat
    public void TurnOfForCombatEnd()
    {
        this.combatUICanvas.enabled = false;
        this.combatCamera.SetActive(false);
    }


    //Function called from CombatInitiativeHandler.cs to update the initiative slider for a character
    //Returns true if the initiative is full
    public bool UpdateInitiativeSlider(bool isPlayer_, int charIndex_, float initToAdd_)
    {
        //Bool for if this character's initiative is full
        bool isInitFull = false;

        //Updating a player slider
        if(isPlayer_)
        {
            this.playerPanels[charIndex_].initiativeSlider.value += initToAdd_;

            if(this.playerPanels[charIndex_].initiativeSlider.value >= this.playerPanels[charIndex_].initiativeSlider.maxValue)
            {
                isInitFull = true;

                this.playerPanels[charIndex_].SetBackgroundColor(this.actingPlayerColor);
            }
        }
        //Updating an enemy slider
        else
        {
            this.enemyPanels[charIndex_].initiativeSlider.value += initToAdd_;

            if (this.enemyPanels[charIndex_].initiativeSlider.value >= this.enemyPanels[charIndex_].initiativeSlider.maxValue)
            {
                isInitFull = true;

                this.enemyPanels[charIndex_].SetBackgroundColor(this.actingEnemyColor);
            }
        }

        return isInitFull;
    }


    //Function called to update a character's health panel
    public void UpdateHealthSlider(bool isPlayer_, int charIndex_, int currentHP_)
    {
        //Updating a player slider
        if(isPlayer_)
        {
            this.playerPanels[charIndex_].healthSlider.value = currentHP_;

            if(currentHP_ == 0)
            {
                this.playerPanels[charIndex_].SetBackgroundColor(this.deadColor);
            }
        }
        //Updating an enemy slider
        else
        {
            this.enemyPanels[charIndex_].healthSlider.value = currentHP_;

            if (currentHP_ == 0)
            {
                this.enemyPanels[charIndex_].SetBackgroundColor(this.deadColor);
            }
        }
    }


    //Function called from CombatManager.DisplayDamageDealt to update all character's health sliders
    public void UpdateHealthBars()
    {
        //Counters for how many player characters and enemy characters are still alive
        int playersAlive = 0;
        int enemiesAlive = 0;

        //Looping through each player character's initiative slider
        for (int p = 0; p < CombatManager.globalReference.characterHandler.playerCharacters.Count; ++p)
        {
            Character pc = CombatManager.globalReference.characterHandler.playerCharacters[p];

            //If the character isn't already dead
            if (this.playerPanels[p].healthSlider.value > 0)
            {
                //Setting the health slider to show the current health based on the max health
                this.playerPanels[p].healthSlider.maxValue = pc.charPhysState.maxHealth;
                this.playerPanels[p].healthSlider.value = pc.charPhysState.currentHealth;

                //If this character is dead, their initiative slider is set to 0 so they can't act
                if (pc.charPhysState.currentHealth <= 0)
                {
                    this.playerPanels[p].initiativeSlider.value = 0;

                    this.playerPanels[p].backgroundImage.color = Color.grey;

                    //Looping through and clearing all of the effects on the dead character
                    for (int e = 0; e < pc.charCombatStats.combatEffects.Count; ++e)
                    {
                        pc.charCombatStats.combatEffects[e].RemoveEffect();
                        e -= 1;
                    }

                    //If this character is the acting character
                    if (pc == CombatManager.globalReference.initiativeHandler.actingCharacters[0])
                    {
                        //Their turn is ended
                        CombatManager.globalReference.EndActingCharactersTurn();
                    }
                    //Otherwise we check to see if they'll be acting soon
                    else
                    {
                        //If this character is in line to act, they are removed from the list
                        for (int a = 1; a < CombatManager.globalReference.initiativeHandler.actingCharacters.Count; ++a)
                        {
                            if (CombatManager.globalReference.initiativeHandler.actingCharacters[a] == pc)
                            {
                                CombatManager.globalReference.initiativeHandler.actingCharacters.RemoveAt(a);
                                a -= 1;
                            }
                        }
                    }
                }
                //If this character is alive, we add it to the counter so we know the player hasn't lost
                else if(pc.charPhysState.currentHealth > 0)
                {
                    playersAlive++;
                }
            }
            //If the character is dead but their initiative slider isn't grey, we make it grey
            else if (this.playerPanels[p].backgroundImage.color != Color.grey)
            {
                this.playerPanels[p].backgroundImage.color = Color.grey;
            }
        }

        //Looping through each enemy's initiative slider
        for (int e = 0; e < CombatManager.globalReference.characterHandler.enemyCharacters.Count; ++e)
        {
            Character ec = CombatManager.globalReference.characterHandler.enemyCharacters[e];

            //If the enemy isn't already dead
            if (this.enemyPanels[e].healthSlider.value > 0)
            {
                //Setting the health slider to show the current health based on the max health
                this.enemyPanels[e].healthSlider.maxValue = ec.charPhysState.maxHealth;
                this.enemyPanels[e].healthSlider.value = ec.charPhysState.currentHealth;

                //If this enemy is dead, their initiative slider is set to 0 so they can't act
                if (ec.charPhysState.currentHealth <= 0)
                {
                    this.enemyPanels[e].initiativeSlider.value = 0;

                    this.enemyPanels[e].backgroundImage.color = Color.grey;

                    //Looping through and clearing all of the effects on the dead character
                    for (int ef = 0; ef < ec.charCombatStats.combatEffects.Count; ++ef)
                    {
                        ec.charCombatStats.combatEffects[ef].RemoveEffect();
                        ef -= 1;
                    }

                    //If this character is in line to act, they are removed from the list
                    for (int a = 1; a < CombatManager.globalReference.initiativeHandler.actingCharacters.Count; ++a)
                    {
                        if (CombatManager.globalReference.initiativeHandler.actingCharacters[a] == CombatManager.globalReference.characterHandler.enemyCharacters[e])
                        {
                            CombatManager.globalReference.initiativeHandler.actingCharacters.RemoveAt(a);
                            a -= 1;
                        }
                    }
                }
                //If the enemy is still alive we add it to the counter to make sure combat doesn't end yet
                else if(ec.charPhysState.currentHealth > 0)
                {
                    enemiesAlive++;
                }
            }
            //If the enemy is dead but their initiative slider isn't grey, we make it grey
            else if (this.enemyPanels[e].backgroundImage.color != Color.grey)
            {
                this.enemyPanels[e].backgroundImage.color = Color.grey;
            }
        }

        //If player characters are alive but enemies aren't, the player wins the battle
        if(playersAlive > 0 && enemiesAlive == 0)
        {
            CombatManager.globalReference.SetWaitTime(2, CombatState.EndCombat);
        }
        //If all player characters are dead, they lose the game no matter how many enemies remain
        else if(playersAlive == 0)
        {
            CombatManager.globalReference.SetWaitTime(3, CombatState.GameOver);
        }
        //Otherwise combat keeps going
    }


    //Function called from AttackAction.PerformAction to show that an attack missed
    public void DisplayMissedAttack(float timeDelay_, CombatTile3D attackedCharTile_)
    {
        //Creating an instance of the damage text object prefab
        GameObject newDamageDisplay = GameObject.Instantiate(this.damageTextPrefab.gameObject);
        //Parenting the damage text object to this object's transform
        newDamageDisplay.transform.SetParent(this.transform);
        //Getting the DamageText component reference
        DamageText newDamageText = newDamageDisplay.GetComponent<DamageText>();
        //Setting the info for the text
        newDamageText.DisplayMiss(timeDelay_, attackedCharTile_.transform.position);
    }
}
