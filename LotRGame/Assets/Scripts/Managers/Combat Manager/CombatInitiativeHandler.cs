using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatInitiativeHandler : MonoBehaviour
{
    //List of all initiative panels for player characters and enemies
    public List<CombatInitiativePanel> playerPanels;
    public List<CombatInitiativePanel> enemyPanels;

    [Space(8)]

    //The colors for the background panels for who is acting
    public Color actingPlayerColor = Color.green;
    public Color actingEnemyColor = Color.red;
    public Color inactiveColor = Color.white;
    public Color highlightColor = Color.yellow;
    
    //Reference to the characters whose turn it is to act. It's a list because multiple characters could have the same initiative
    [HideInInspector]
    public List<Character> actingCharacters = null;



    //Function called from CombatManager.cs to reset our initiative panels and actingCharacters array at the start of combat
    public void ResetForCombatStart()
    {

    }


    //Function called from CombatManager.cs to increase the initiative of all characters
    public void IncreaseInitiatives(float timePassed_)
    {
        
    }
}
