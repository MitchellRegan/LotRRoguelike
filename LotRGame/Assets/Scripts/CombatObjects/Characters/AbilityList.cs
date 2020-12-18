using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*The list of actions for PlayerCharacter.cs and NPC.cs that can be used in combat*/

public class AbilityList : MonoBehaviour
{
    //The default unarmed ability
    public Action unarmedAbility = null;

    //The regular abilities that can be used
    public Action ability1 = null;
    public Action ability2 = null;
    public Action ability3 = null;
    public Action ability4 = null;

    //The Power Ability that has a longer cooldown
    public Action powerAbility = null;
}
