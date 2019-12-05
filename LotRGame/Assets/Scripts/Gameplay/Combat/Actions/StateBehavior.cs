using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class used in EnemyCombatAI_Basic.cs to determine which state this enemy will be in at a given time
[System.Serializable]
public class StateBehavior
{
    //The priority for this condition in case there are multiple behavior conditions that are true. 0 is lowest, 10 is highest
    [Range(0, 10)]
    public int priority = 1;

    //Enum for what kind of condition this is
    public ConditionalType conditional = ConditionalType.PersonalHPRange;

    //The range for health percentage for the TargetHPRange, PersonalHPRange, and AllyHPRange
    public Vector2 healthRange = new Vector2(0, 1);

    //The range of risky behavior from very low risk to very high risk
    [Range(0.1f, 0.9f)]
    public float chanceOfRisk = 0.5f;

    //Enum for what type of target this enemy prefers to attack
    public PlayerTargetPreference preferredTargetType = PlayerTargetPreference.HighestThreat;

    //Bool that determines if this enemy takes threat into consideration at all
    public bool ignoreThreat = false;

    //Enum for the state that this enemy shifts to once this state is entered
    public AICombatState state = AICombatState.Hostile;

    //The preferred distance from the target
    [Range(1, 13)]
    public int preferredDistFromTarget = 1;

    //The list of actions that are added to this enemy's action list when this state is entered
    public List<Action> addedActions;

    //If true, the actions in the addedActions list are the ONLY actions that can be used in this behavior
    public bool onlyUseAddedActions = false;
}