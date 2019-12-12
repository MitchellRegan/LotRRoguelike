using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatCharacterHandler : MonoBehaviour
{
    //The list of all characters involved in this combat
    [HideInInspector]
    public List<Character> playerCharactersInCombat;
    [HideInInspector]
    public List<Character> enemyCharactersInCombat;
}
