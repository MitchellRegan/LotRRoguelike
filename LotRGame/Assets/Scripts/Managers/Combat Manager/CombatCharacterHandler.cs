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



    //Function called from CombatManager.InitiateCombat to load all player and enemy characters into combat
    public void InitializeCharactersForCombat(PartyGroup playerParty_, EnemyEncounter enemyParty_)
    {
        this.playerCharacters = new List<Character>();
        this.enemyCharacters = new List<Character>();

        foreach(Character pc in playerParty_.charactersInParty)
        {
            this.playerCharacters.Add(pc);
        }

        foreach(Character ec in enemyParty_.enemies)
        {
            this.enemyCharacters.Add(ec);
        }

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

    }


    //Function called from InitializeCharactersForCombat to find starting buffs
    private void FindStartingBuffs()
    {

    }
}
