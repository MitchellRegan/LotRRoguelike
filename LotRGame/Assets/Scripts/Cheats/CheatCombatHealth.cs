using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheatCombatHealth : MonoBehaviour
{
    //Button to kill the next living enemy in combat
    public KeyCode killNextEnemyButton = KeyCode.Keypad4;
    //Button to heal all living enemies to full health
    public KeyCode healAllEnemiesButton = KeyCode.Keypad6;

    [Space(8)]

    //Button to kill the next living player character in combat
    public KeyCode killNextPlayerCharacterButton = KeyCode.Keypad7;
    //Button to heal all living player characters to full health
    public KeyCode healAllPlayerCharactersButton = KeyCode.Keypad9;


    
	// Update is called once per frame
	private void Update ()
    {
		//If the button to kill the next enemy is pressed
        if(Input.GetKeyDown(this.killNextEnemyButton))
        {
            this.KillNextEnemy();
        }
        //If the button to heal all enemies is pressed
        else if(Input.GetKeyDown(this.healAllEnemiesButton))
        {
            this.HealAllEnemies();
        }
        //If the button to kill the next player character is pressed
        else if (Input.GetKeyDown(this.killNextPlayerCharacterButton))
        {
            this.KillNextPlayerCharacter();
        }
        //If the button to use the action on the next enemy is pressed
        else if (Input.GetKeyDown(this.healAllPlayerCharactersButton))
        {
            this.HealAllPlayerCharacters();
        }
        //If the /? button is pressed we debug the keys for this script
        else if (Input.GetKeyDown(KeyCode.Slash))
        {
            Debug.Log("CHEAT: Combat Health info: Kill Next Enemy: " + this.killNextEnemyButton + 
                ", Heal All Enemies: " + this.healAllEnemiesButton + 
                ", Kill Next Player Character: " + this.killNextPlayerCharacterButton + 
                ", Heal All Player Characters: " + this.healAllPlayerCharactersButton);
        }
    }


    //Function called from Update to kill the next enemy
    private void KillNextEnemy()
    {
        //If we're not in the gameplay scene, nothing happens
        if (SceneManager.GetActiveScene().buildIndex != 1)
        {
            Debug.Log("CHEAT: Combat Health, Kill Next Enemy: ERROR not in gameplay scene");
            return;
        }

        //If we're in combat, nothing happens
        if (!CombatManager.globalReference.GetComponent<Canvas>().enabled)
        {
            Debug.Log("CHEAT: Combat Health, Kill Next Enemy: ERROR must be in combat");
            return;
        }

        //Looping through all enemies until we find a live one
        for(int e = 0; e < CombatManager.globalReference.enemyCharactersInCombat.Count; ++e)
        {
            //Making sure the current enemy isn't null or dead
            if(CombatManager.globalReference.enemyCharactersInCombat[e] != null &&
                CombatManager.globalReference.enemyCharactersInCombat[e].charPhysState.currentHealth > 0)
            {
                Debug.Log("CHEAT: Combat Health, Kill Next Enemy: Killing " + CombatManager.globalReference.enemyCharactersInCombat[e].firstName);
                //Dealing a ton of damage to kill this enemy
                CombatManager.globalReference.enemyCharactersInCombat[e].charPhysState.DamageCharacter(999999);
                break;
            }
        }
    }


    //Function called from Update to full heal all enemies
    private void HealAllEnemies()
    {
        //If we're not in the gameplay scene, nothing happens
        if (SceneManager.GetActiveScene().buildIndex != 1)
        {
            Debug.Log("CHEAT: Combat Health, Heal All Enemies: ERROR not in gameplay scene");
            return;
        }

        //If we're in combat, nothing happens
        if (!CombatManager.globalReference.GetComponent<Canvas>().enabled)
        {
            Debug.Log("CHEAT: Combat Health, Heal All Enemies: ERROR must be in combat");
            return;
        }

        //Looping through all enemies until we find a live one
        for (int e = 0; e < CombatManager.globalReference.enemyCharactersInCombat.Count; ++e)
        {
            //Making sure the current enemy isn't null or dead
            if (CombatManager.globalReference.enemyCharactersInCombat[e] != null &&
                CombatManager.globalReference.enemyCharactersInCombat[e].charPhysState.currentHealth > 0)
            {
                //Healing all damage to this enemy
                CombatManager.globalReference.enemyCharactersInCombat[e].charPhysState.HealCharacter(999999);
            }
        }
        Debug.Log("CHEAT: Combat Health, Heal All Enemies: Healing All Enemies");
    }


    //Function called from Update to kill the next player character
    private void KillNextPlayerCharacter()
    {
        //If we're not in the gameplay scene, nothing happens
        if (SceneManager.GetActiveScene().buildIndex != 1)
        {
            Debug.Log("CHEAT: Combat Health, Kill Next Player Character: ERROR not in gameplay scene");
            return;
        }

        //If we're in combat, nothing happens
        if (!CombatManager.globalReference.GetComponent<Canvas>().enabled)
        {
            Debug.Log("CHEAT: Combat Health, Kill Next Player Character: ERROR must be in combat");
            return;
        }

        //Looping through all player characters until we find a live one
        for (int p = 0; p < CombatManager.globalReference.playerCharactersInCombat.Count; ++p)
        {
            //Making sure the current character isn't null or dead
            if (CombatManager.globalReference.playerCharactersInCombat[p] != null &&
                CombatManager.globalReference.playerCharactersInCombat[p].charPhysState.currentHealth > 0)
            {
                Debug.Log("CHEAT: Combat Health, Kill Next Player Character: Killing " + CombatManager.globalReference.playerCharactersInCombat[p].firstName);
                //Dealing a ton of damage to kill this character
                CombatManager.globalReference.playerCharactersInCombat[p].charPhysState.DamageCharacter(999999);
                break;
            }
        }
    }


    //Function called from Update to full heal all player characters
    private void HealAllPlayerCharacters()
    {
        //If we're not in the gameplay scene, nothing happens
        if (SceneManager.GetActiveScene().buildIndex != 1)
        {
            Debug.Log("CHEAT: Combat Health, Heal All Player Characters: ERROR not in gameplay scene");
            return;
        }

        //If we're in combat, nothing happens
        if (!CombatManager.globalReference.GetComponent<Canvas>().enabled)
        {
            Debug.Log("CHEAT: Combat Health, Heal All Player Characters: ERROR must be in combat");
            return;
        }

        //Looping through all player characters until we find a live one
        for (int p = 0; p < CombatManager.globalReference.playerCharactersInCombat.Count; ++p)
        {
            //Making sure the current character isn't null or dead
            if (CombatManager.globalReference.playerCharactersInCombat[p] != null &&
                CombatManager.globalReference.playerCharactersInCombat[p].charPhysState.currentHealth > 0)
            {
                //Healing all damage to this character
                CombatManager.globalReference.playerCharactersInCombat[p].charPhysState.HealCharacter(999999);
            }
        }
        Debug.Log("CHEAT: Combat Health, Heal All Player Characters: Healing All Player Characters");
    }
}
