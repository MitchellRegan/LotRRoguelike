using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatCharacterHandler : MonoBehaviour
{
    //The list of all characters involved in this combat
    [HideInInspector]
    public List<Character> playerCharacters;
    [HideInInspector]
    public List<Character> enemyCharacters;

    //The list of character models
    [HideInInspector]
    public List<GameObject> playerModels;
    [HideInInspector]
    public List<GameObject> enemyModels;



    //Function called from CombatManager.InitiateCombat to load all player and enemy characters into combat
    public void InitializeCharactersForCombat(PartyGroup playerParty_, EnemyEncounter enemyParty_)
    {
        //Resetting the character lists
        this.playerCharacters = new List<Character>();
        this.enemyCharacters = new List<Character>();

        //Filling both lists with the player and enemy characters
        foreach(Character pc in playerParty_.charactersInParty)
        {
            this.playerCharacters.Add(pc);
        }

        foreach(EncounterEnemy ec in enemyParty_.enemies)
        {
            this.enemyCharacters.Add(ec.enemyCreature);
        }

        //Spawning the character models, setting their locations, and assigning any starting combat buffs
        this.SetCharacterTilePositions();
        this.CreateCharacterModels();
        this.FindStartingBuffs();
    }


    //Function called from InitializeCharactersForCombat to set each character's tile position
    private void SetCharacterTilePositions()
    {

    }


    //Function called from InitializeCharactersForCombat to instantiate character models
    private void CreateCharacterModels()
    {
        //Clearing the model lists
        foreach(GameObject pm in this.playerModels)
        {
            Destroy(pm);
        }
        foreach(GameObject em in this.enemyModels)
        {
            Destroy(em);
        }
        this.playerModels = new List<GameObject>(this.playerCharacters.Count);
        this.enemyModels = new List<GameObject>(this.enemyCharacters.Count);
    }


    //Function called from InitializeCharactersForCombat to find starting buffs
    private void FindStartingBuffs()
    {

    }
}
