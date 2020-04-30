using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This class is for all objects that can be targeted or attacked in combat*/

[RequireComponent(typeof(Health))]
public class CombatObject : MonoBehaviour
{
    //The name of this object/character in combat
    public string combatName = "";
}
