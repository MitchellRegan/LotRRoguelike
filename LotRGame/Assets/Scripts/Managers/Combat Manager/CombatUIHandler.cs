using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUIHandler : MonoBehaviour
{
    //Reference to the combat UI canvas
    public Canvas combatUICanvas;

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



    //Function called from CombatManager.InitiateCombat to reset our initiative panels at the start of combat
    public void ResetForCombatStart()
    {
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


    //Function called from AttackAction.PerformAction to show that an attack missed
    public void DisplayMissedAttack(float timeDelay_, CombatTile attackedCharTile_)
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
