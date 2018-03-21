using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class EnemyCombatAI_Basic : MonoBehaviour
{
    //Reference to this enemy's Character component
    private Character ourCharacter;

    //The list of different behaviors that this enemy can be in
    public List<StateBehavior> behaviorList;
    //The index for our current behavior
    private List<int> validBehaviorIndexList;

    //The List to track each character's combat threat
    private List<PlayerThreatMeter> threatList;

    //The reference to the character that this enemy will target
    private Character playerCharToAttack = null;

    //The list of actions this enemy can use to attack
    private List<AttackAction> attackActionList;
    //The list of actions that cause effects
    private List<Action> effectActionList;
    //The list of actions that let this enemy move
    private List<MoveAction> moveActionList;

    //Bools that determine what types of moves have been used so far
    private bool canUseStandard = true;
    private bool canUseSecondary = true;
    private bool canUseQuick = true;
    private bool canUseFull = true;

    //The list of actions that will be used on this enemy's turn IN ORDER and the combat tiles where the action will be performed
    private List<EnemyActionAndTile> actionsToPerformOnTiles;

    //Bool that, when true, means that this enemy is ready to tell the combat manager what actions it will use
    private bool readyToAct = false;
    //Timer for how long this enemy has to wait for their action to be used before they can do anything else
    private float actionCooldown = 0;



    //Function called on the first frame this character exists
    private void Start()
    {
        //Getting the reference to this character component
        this.ourCharacter = this.GetComponent<Character>();

        //Initializing our list that holds all of the valid behavior indexes
        this.validBehaviorIndexList = new List<int>();

        //Initializing our threat list if we take threat into consideration
        this.threatList = new List<PlayerThreatMeter>();

        //Looping through all characters in the current combat encounter and adding them to the threat list
        foreach(Character enemyChar in CombatManager.globalReference.playerCharactersInCombat)
        {
            this.threatList.Add(new PlayerThreatMeter(enemyChar, 0));
        }

        //Getting our lists of actions initialized and organized
        this.InitializeActionLists();


        //If for SOME reason our enemy wasn't created with a behavior list (meaning Mitch screwed up), we create a default
        if (this.behaviorList.Count == 0)
        {
            //Creating a new behavior for our list
            StateBehavior defaultBehavior = new StateBehavior();

            //Creating a basic condition, even though we don't need to use it because it won't change
            defaultBehavior.conditional = StateBehavior.ConditionalType.PersonalHPRange;
            defaultBehavior.healthRange = new Vector2(0, 1);

            //Making it so this enemy just melee attacks whoever is highest on the threat list
            defaultBehavior.preferredTargetType = StateBehavior.PlayerTargetPreference.HighestThreat;
            defaultBehavior.state = StateBehavior.AICombatState.Hostile;
            defaultBehavior.preferredDistFromTarget = 1;
            //Initializing an empty action list so we don't get errors
            defaultBehavior.addedActions = new List<Action>();

            //Adding the default behavior to our behavior list
            this.behaviorList.Add(defaultBehavior);
        }
        //Otherwise we check to make sure all of the health ranges for each behavior is valid
        else
        {
            foreach(StateBehavior sb in this.behaviorList)
            {
                //If the lower health range is higher than the upper health range, that doesn't work
                if(sb.healthRange.x > sb.healthRange.y)
                {
                    sb.healthRange.x = sb.healthRange.y;
                }
            }
        }
    }


    //Function called from Start to get this character's list of actions
    private void InitializeActionLists()
    {
        //Initializing our lists of actions
        this.attackActionList = new List<AttackAction>();
        this.effectActionList = new List<Action>();
        this.moveActionList = new List<MoveAction>();

        //Looping through all of the standard actions in our action list
        foreach (Action stdAct in this.ourCharacter.charActionList.standardActions)
        {
            //If the action is an attack
            if (stdAct.GetComponent<AttackAction>())
            {
                this.attackActionList.Add(stdAct.GetComponent<AttackAction>());

                //If this attack causes any effects
                if (stdAct.GetComponent<AttackAction>().effectsOnHit.Count > 0)
                {
                    this.effectActionList.Add(stdAct);
                }
            }
            //If the action is a move action
            else if (stdAct.GetComponent<MoveAction>())
            {
                this.moveActionList.Add(stdAct.GetComponent<MoveAction>());
            }
        }

        //Looping through all of the secondary actions in our action list
        foreach (Action scndAct in this.ourCharacter.charActionList.secondaryActions)
        {
            //If the action is an attack
            if (scndAct.GetComponent<AttackAction>())
            {
                this.attackActionList.Add(scndAct.GetComponent<AttackAction>());

                //If this attack causes any effects
                if (scndAct.GetComponent<AttackAction>().effectsOnHit.Count > 0)
                {
                    this.effectActionList.Add(scndAct);
                }
            }
            //If the action is a move action
            else if (scndAct.GetComponent<MoveAction>())
            {
                this.moveActionList.Add(scndAct.GetComponent<MoveAction>());
            }
        }

        //Looping through all of the quick actions in our action list
        foreach (Action qkAct in this.ourCharacter.charActionList.quickActions)
        {
            //If the action is an attack
            if (qkAct.GetComponent<AttackAction>())
            {
                this.attackActionList.Add(qkAct.GetComponent<AttackAction>());

                //If this attack causes any effects
                if (qkAct.GetComponent<AttackAction>().effectsOnHit.Count > 0)
                {
                    this.effectActionList.Add(qkAct);
                }
            }
            //If the action is a move action
            else if (qkAct.GetComponent<MoveAction>())
            {
                this.moveActionList.Add(qkAct.GetComponent<MoveAction>());
            }
        }

        //Looping through all of the full round actions in our action list
        foreach (Action fullAct in this.ourCharacter.charActionList.fullRoundActions)
        {
            //If the action is an attack
            if (fullAct.GetComponent<AttackAction>())
            {
                this.attackActionList.Add(fullAct.GetComponent<AttackAction>());

                //If this attack causes any effects
                if (fullAct.GetComponent<AttackAction>().effectsOnHit.Count > 0)
                {
                    this.effectActionList.Add(fullAct);
                }
            }
            //If the action is a move action
            else if (fullAct.GetComponent<MoveAction>())
            {
                this.moveActionList.Add(fullAct.GetComponent<MoveAction>());
            }
        }

        //Looping through all of the default actions in our action list
        /*foreach (Action dftAct in this.ourCharacter.charActionList.defaultActions)
        {
            //If the action is an attack
            if (dftAct.GetComponent<AttackAction>())
            {
                Debug.Log("Default action " + dftAct.actionName + " for character " + this.ourCharacter.firstName);
                this.attackActionList.Add(dftAct.GetComponent<AttackAction>());

                //If this attack causes any effects
                if (dftAct.GetComponent<AttackAction>().effectsOnHit.Count > 0)
                {
                    this.effectActionList.Add(dftAct);
                }
            }
            //If the action is a move action
            else if (dftAct.GetComponent<MoveAction>())
            {
                this.moveActionList.Add(dftAct.GetComponent<MoveAction>());
            }
        }*/


        //Organizing our movement actions in decending order of distance they can move
        for (int i = 0; i < this.moveActionList.Count - 1; ++i)
        {
            for (int j = i + 1; j < this.moveActionList.Count; ++j)
            {
                //If the second move action is greater than the first, they're swapped
                if (this.moveActionList[j].range > this.moveActionList[i].range)
                {
                    MoveAction placeholder = this.moveActionList[j];
                    this.moveActionList[j] = this.moveActionList[i];
                    this.moveActionList[i] = placeholder;
                }
            }
        }
    }

    
	//Function called from the CombatManager.cs when this enemy character's initiative is full
    public void StartEnemyTurn()
    {
        //Setting it so that this enemy isn't ready to act yet
        this.readyToAct = false;

        //Clearing our current list of actions to perform so we can designate new ones
        actionsToPerformOnTiles = new List<EnemyActionAndTile>();

        //Finding the index of the behavior we should have from our behavior list
        this.validBehaviorIndexList = this.DetermineBehavior();

        //If there are no valid behaviors, this enemy skips their turn
        if(this.validBehaviorIndexList.Count == 0)
        {
            Debug.Log("Skipping Enemy Turn! No Valid Behaviors");
            this.EndEnemyTurn();
        }

        //Finding out which function to call for the highest priority behavior
        switch(this.behaviorList[this.validBehaviorIndexList[0]].state)
        {
            case StateBehavior.AICombatState.Hostile:
                this.HostileStateLogic();
                break;

            case StateBehavior.AICombatState.Support:
                this.SupportStateLogic();
                break;

            case StateBehavior.AICombatState.Defensive:
                this.DefensiveStateLogic();
                break;

            case StateBehavior.AICombatState.Terrified:
                this.TerrifiedStateLogic();
                break;
        }
    }
    

    //Function called from StartEnemyTurn to determine what behavior this enemy should have. Returns the index of the behavior from our 
    private List<int> DetermineBehavior()
    {
        //If we only have 1 behavior, that's the one we use
        if(this.behaviorList.Count == 1)
        {
            return new List<int>() { 0 };
        }

        //Creating a new list to hold all of the indexes of valid behaviors
        List<int> returnedIndexList = new List<int>();

        //Looping through all of our available behaviors
        for(int b = 0; b < this.behaviorList.Count; ++b)
        {
            //Bool that, if true, means the behavior we're currently checking has its condition met
            bool conditionMet = false;

            //Switch statement to check what type of condition we're checking for
            switch(this.behaviorList[b].conditional)
            {
                //If this enemy's health is between a specific health range
                case StateBehavior.ConditionalType.PersonalHPRange:
                    //Getting this enemy's health percentage
                    float ourHPPercent = (this.ourCharacter.charPhysState.currentHealth * 1f) / (this.ourCharacter.charPhysState.maxHealth * 1f);
                    //If our HP is within this condition's health range, we meet the condition
                    if(ourHPPercent > this.behaviorList[b].healthRange.x && ourHPPercent <= this.behaviorList[b].healthRange.y)
                    {
                        conditionMet = true;
                    }
                    break;

                //If any of this enemy's ally's health are between a specific range
                case StateBehavior.ConditionalType.OneAllyHPRange:
                    //Looping through all of the enemies in this combat
                    foreach(Character combatEnemy in CombatManager.globalReference.enemyCharactersInCombat)
                    {
                        //If the current enemy isn't this enemy and not null
                        if (combatEnemy != null && combatEnemy != this.ourCharacter)
                        {
                            //Getting the current ally's health percentage
                            float allyHPPercent = (combatEnemy.charPhysState.currentHealth * 1f) / (combatEnemy.charPhysState.maxHealth * 1f);
                            //If the ally's HP is within this condition's health range, we meet the condition
                            if (allyHPPercent >= this.behaviorList[b].healthRange.x && allyHPPercent <= this.behaviorList[b].healthRange.y)
                            {
                                conditionMet = true;
                                break;
                            }
                        }
                    }
                    break;

                //If half of this enemy's ally's health are between a specific range
                case StateBehavior.ConditionalType.HalfAlliesHPRange:
                    //Making an int to track the number of allies whose health is within the range
                    int numAlliesInRange = 0;
                    //Looping through all of the enemies in this combat
                    foreach (Character combatEnemy in CombatManager.globalReference.enemyCharactersInCombat)
                    {
                        //If the current enemy isn't this enemy and not null
                        if (combatEnemy != null && combatEnemy != this.ourCharacter)
                        {
                            //Getting the current ally's health percentage
                            float allyHPPercent = (combatEnemy.charPhysState.currentHealth * 1f) / (combatEnemy.charPhysState.maxHealth * 1f);
                            //If the ally's HP is within this condition's health range, we increase the count
                            if (allyHPPercent >= this.behaviorList[b].healthRange.x && allyHPPercent <= this.behaviorList[b].healthRange.y)
                            {
                                numAlliesInRange += 1;

                                //If the number of allies in range is at least half of the number of enemies in combat, we meet the condition
                                if (numAlliesInRange >= (CombatManager.globalReference.enemyCharactersInCombat.Count / 2))
                                {
                                    conditionMet = true;
                                    break;
                                }
                            }
                        }
                    }
                    break;

                //If all of this enemy's ally's health are between a specific range
                case StateBehavior.ConditionalType.AllAlliesHPRange:
                    //Setting the condition to being true by default, and if it's not true, we change it in the loop
                    conditionMet = true;
                    //Looping through all of the enemies in this combat
                    foreach (Character combatEnemy in CombatManager.globalReference.enemyCharactersInCombat)
                    {
                        //If the current enemy isn't this enemy and not null
                        if (combatEnemy != null && combatEnemy != this.ourCharacter)
                        {
                            //Getting the current ally's health percentage
                            float allyHPPercent = (combatEnemy.charPhysState.currentHealth * 1f) / (combatEnemy.charPhysState.maxHealth * 1f);
                            //If the ally's HP is NOT within this condition's health range, the condition isn't met
                            if (allyHPPercent < this.behaviorList[b].healthRange.x || allyHPPercent > this.behaviorList[b].healthRange.y)
                            {
                                conditionMet = false;
                                break;
                            }
                        }
                    }
                    break;

                //If this enemy has a debuff
                case StateBehavior.ConditionalType.Debuffed:
                    //Looping through all of the effects on this character
                    foreach(Effect combatEffect in this.ourCharacter.charCombatStats.combatEffects)
                    {
                        //If the effect is a damage over time effect, we meet the condition
                        if(combatEffect.GetType() == typeof(DamageOverTimeEffect))
                        {
                            conditionMet = true;
                        }
                        //If the effect is a stat change effect, we need to see if it's good or bad
                        else if(combatEffect.GetType() == typeof(ModifyStatsEffect))
                        {
                            //Getting a float to hold the net gain in stats
                            float netStatChange = 0;
                            //Looping through all of the stat changes
                            ModifyStatsEffect mse = combatEffect.GetComponent<ModifyStatsEffect>();
                            foreach(StatModifier change in mse.StatChanges)
                            {
                                //If the change isn't for initiative, the value is added normally
                                if(change.modifiedStat != StatModifier.StatName.Initiative)
                                {
                                    netStatChange += change.amountToChange;
                                }
                                //if the change is for initiative, we multiply the amount because initiative values are so low
                                else
                                {
                                    netStatChange += change.amountToChange * 100f;
                                }
                            }

                            //If the net stat changes are negative, this effect counts as a debuff so we meet the condition
                            if(netStatChange < 0)
                            {
                                conditionMet = true;
                                break;
                            }
                        }
                    }
                    break;

                //If any of this enemy's allies have a debuff:
                case StateBehavior.ConditionalType.AllyDebuffed:
                    //Looping through all of the enemies in combat to check their effects
                    foreach(Character allyEnemy in CombatManager.globalReference.enemyCharactersInCombat)
                    {
                        //If we've already found an ally that meets our requirements, we break this loop
                        if(conditionMet)
                        {
                            break;
                        }

                        //If the current enemy isn't this enemy or null
                        if(allyEnemy != null && this.ourCharacter != allyEnemy)
                        {
                            //Looping through all of the effects on this ally
                            foreach (Effect combatEffect in allyEnemy.charCombatStats.combatEffects)
                            {
                                //If the effect is a damage over time effect, we meet the condition
                                if (combatEffect.GetType() == typeof(DamageOverTimeEffect))
                                {
                                    conditionMet = true;
                                }
                                //If the effect is a stat change effect, we need to see if it's good or bad
                                else if (combatEffect.GetType() == typeof(ModifyStatsEffect))
                                {
                                    //Getting a float to hold the net gain in stats
                                    float netStatChange = 0;
                                    //Looping through all of the stat changes
                                    ModifyStatsEffect mse = combatEffect.GetComponent<ModifyStatsEffect>();
                                    foreach (StatModifier change in mse.StatChanges)
                                    {
                                        //If the change isn't for initiative, the value is added normally
                                        if (change.modifiedStat != StatModifier.StatName.Initiative)
                                        {
                                            netStatChange += change.amountToChange;
                                        }
                                        //if the change is for initiative, we multiply the amount because initiative values are so low
                                        else
                                        {
                                            netStatChange += change.amountToChange * 100f;
                                        }
                                    }

                                    //If the net stat changes are negative, this effect counts as a debuff so we meet the condition
                                    if (netStatChange < 0)
                                    {
                                        conditionMet = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    break;

                //If at least one player character's health is between a specific range
                case StateBehavior.ConditionalType.OnePlayerHPRange:
                    //Looping through all of the player characters in this combat
                    foreach (Character playerCharacter in CombatManager.globalReference.playerCharactersInCombat)
                    {
                        //If the current player isn't null
                        if (playerCharacter != null)
                        {
                            //Getting the current player's health percentage
                            float playerHPPercent = (playerCharacter.charPhysState.currentHealth * 1f) / (playerCharacter.charPhysState.maxHealth * 1f);
                            //If the player's HP is within this condition's health range, we meet the condition
                            if (playerHPPercent >= this.behaviorList[b].healthRange.x && playerHPPercent <= this.behaviorList[b].healthRange.y)
                            {
                                conditionMet = true;
                                break;
                            }
                        }
                    }
                    break;

                //If at least half of the player characters' health are between a specific range
                case StateBehavior.ConditionalType.HalfPlayersHPRange:
                    //Making an int to track the number of player characters whose health is within the range
                    int numPlayersInRange = 0;
                    //Looping through all of the player characters in this combat
                    foreach (Character playerCharacter in CombatManager.globalReference.playerCharactersInCombat)
                    {
                        //If the current player isn't null
                        if (playerCharacter != null)
                        {
                            //Getting the current player's health percentage
                            float playerHPPercent = (playerCharacter.charPhysState.currentHealth * 1f) / (playerCharacter.charPhysState.maxHealth * 1f);
                            //If the player's HP is within this condition's health range, we increase the count
                            if (playerHPPercent >= this.behaviorList[b].healthRange.x && playerHPPercent <= this.behaviorList[b].healthRange.y)
                            {
                                numPlayersInRange += 1;

                                //If the number of player characters that are within the health range is at least half, we meet the condition
                                if (numPlayersInRange >= CombatManager.globalReference.playerCharactersInCombat.Count)
                                {
                                    conditionMet = true;
                                    break;
                                }
                            }
                        }
                    }
                    break;

                //If all of the player characters' health are between a specific range
                case StateBehavior.ConditionalType.AllPlayersHPRange:
                    //Setting the condition to being true by default, and if it's not true, we change it in the loop
                    conditionMet = true;
                    //Looping through all of the player characters in this combat
                    foreach (Character playerCharacter in CombatManager.globalReference.playerCharactersInCombat)
                    {
                        //If the current player isn't null
                        if (playerCharacter != null)
                        {
                            //Getting the current player's health percentage
                            float playerHPPercent = (playerCharacter.charPhysState.currentHealth * 1f) / (playerCharacter.charPhysState.maxHealth * 1f);
                            //If the player's HP is NOT within this condition's health range, we don't meet the condition
                            if (playerHPPercent < this.behaviorList[b].healthRange.x && playerHPPercent > this.behaviorList[b].healthRange.y)
                            {
                                conditionMet = false;
                                break;
                            }
                        }
                    }
                    break;
            }

            //If the current behavior's condition is met, we add it to our list of valid behaviors
            if(conditionMet)
            {
                returnedIndexList.Add(b);
            }
        }
        
        //Looping through all of the valid indexes and organizing them based on priority, going from highest to lowest
        for(int hp = 0; hp < returnedIndexList.Count - 1; ++hp)
        {
            for(int lp = 1; lp < returnedIndexList.Count; ++lp)
            {
                //If the "high priority" (hp) behavior has a lower priority than the "low priority" (lp) behavior, the indexes are swapped
                if(this.behaviorList[returnedIndexList[hp]].priority < this.behaviorList[returnedIndexList[lp]].priority)
                {
                    int placeholderIndex = returnedIndexList[hp];
                    returnedIndexList[hp] = returnedIndexList[lp];
                    returnedIndexList[lp] = placeholderIndex;
                }
            }
        }

        //Returning the list of behavior indexes
        return returnedIndexList;
    }


    //Function called from StartEnemyTurn if the current behavior is HOSTILE
    private void HostileStateLogic()
    {
        //Find which character to attack
        this.FindPlayerTarget();
        

        //Finding the distance this enemy is from the target
        CombatTile ourEnemyTile = CombatManager.globalReference.FindCharactersTile(this.ourCharacter);
        CombatTile targetPlayerTile = CombatManager.globalReference.FindCharactersTile(this.playerCharToAttack);
        List<CombatTile> pathToTarget = PathfindingAlgorithms.BreadthFirstSearchCombat(ourEnemyTile, targetPlayerTile, true, true);
        int currentDist = pathToTarget.Count;

        //If this enemy is already in the preferred distance
        if(this.behaviorList[0].preferredDistFromTarget == currentDist)
        {
            Debug.Log("Enemy In Attack Range");
            //Creating a dictionary of actions and their estimated damage for each action type
            Dictionary<AttackAction, int> standardAtkDmg = new Dictionary<AttackAction, int>();
            Dictionary<AttackAction, int> secondaryAtkDmg = new Dictionary<AttackAction, int>();
            Dictionary<AttackAction, int> quickAtkDmg = new Dictionary<AttackAction, int>();
            Dictionary<AttackAction, int> fullRoundAtkDmg = new Dictionary<AttackAction, int>();

            //Tracking the actions that have the highest amount of damage
            AttackAction highestStandard = null;
            AttackAction highestSecondary = null;
            AttackAction highestQuick = null;
            AttackAction highestFullRound = null;

            //Looping through all of the actions for our current behavior to find their average damage and type
            foreach(Action addedAct in this.behaviorList[0].addedActions)
            {
                //If this added action is an attack, we can use it
                if (addedAct.GetType() == typeof(AttackAction))
                {
                    //If the current attack action is within range of the target, we can use it
                    if (addedAct.range >= this.behaviorList[0].preferredDistFromTarget)
                    {
                        //Int to hold this attack's average damage
                        int avgDamage = this.FindAttackAverageDamage(addedAct as AttackAction, this.playerCharToAttack);

                        //Getting the type of action this attack is
                        switch (addedAct.type)
                        {
                            case Action.ActionType.Standard:
                                standardAtkDmg.Add(addedAct as AttackAction, avgDamage);
                                //If this standard attack has a higher average damage than the current highest standard attack
                                if(highestStandard == null || standardAtkDmg[highestStandard] < avgDamage)
                                {
                                    //This attack becomes the new highest
                                    highestStandard = addedAct as AttackAction;
                                }
                                break;

                            case Action.ActionType.Secondary:
                                secondaryAtkDmg.Add(addedAct as AttackAction, avgDamage);
                                //If this secondary attack has a higher average damage than the current highest secondary attack
                                if(highestSecondary == null || secondaryAtkDmg[highestSecondary] < avgDamage)
                                {
                                    //This attack becomes the new highest
                                    highestSecondary = addedAct as AttackAction;
                                }
                                break;

                            case Action.ActionType.Quick:
                                quickAtkDmg.Add(addedAct as AttackAction, avgDamage);
                                //If this quick attack has a higher average damage than the current highest quick attack
                                if(highestQuick == null || quickAtkDmg[highestQuick] < avgDamage)
                                {
                                    //This attack becomes the new highest
                                    highestQuick = addedAct as AttackAction;
                                }
                                break;

                            case Action.ActionType.FullRound:
                                fullRoundAtkDmg.Add(addedAct as AttackAction, avgDamage);
                                //If this full round attack has a higher average damage than the current highest full round attack
                                if(highestFullRound == null || fullRoundAtkDmg[highestFullRound] < avgDamage)
                                {
                                    //This attack becomes the new highest
                                    highestFullRound = addedAct as AttackAction;
                                }
                                break;
                        }
                    }
                }
            }

            //If we can use more than just this behavior's added actions
            if(!this.behaviorList[0].onlyUseAddedActions)
            {
                //Looping through all of the actions to find the average damage for each of them and what type of action they are
                foreach (AttackAction atkAct in this.attackActionList)
                {
                    //If the current attack action is within range of the target, we can use it
                    if (atkAct.range >= this.behaviorList[0].preferredDistFromTarget)
                    {
                        //Int to hold this attack's average damage
                        int avgDamage = this.FindAttackAverageDamage(atkAct, this.playerCharToAttack);

                        //Getting the type of action this attack is
                        switch (atkAct.type)
                        {
                            case Action.ActionType.Standard:
                                standardAtkDmg.Add(atkAct, avgDamage);
                                //If this standard attack has a higher average damage than the current highest standard attack
                                if (highestStandard == null || standardAtkDmg[highestStandard] < avgDamage)
                                {
                                    //This attack becomes the new highest
                                    highestStandard = atkAct as AttackAction;
                                }
                                break;

                            case Action.ActionType.Secondary:
                                secondaryAtkDmg.Add(atkAct, avgDamage);
                                //If this secondary attack has a higher average damage than the current highest secondary attack
                                if (highestSecondary == null || secondaryAtkDmg[highestSecondary] < avgDamage)
                                {
                                    //This attack becomes the new highest
                                    highestSecondary = atkAct as AttackAction;
                                }
                                break;

                            case Action.ActionType.Quick:
                                quickAtkDmg.Add(atkAct, avgDamage);
                                //If this quick attack has a higher average damage than the current highest quick attack
                                if (highestQuick == null || quickAtkDmg[highestQuick] < avgDamage)
                                {
                                    //This attack becomes the new highest
                                    highestQuick = atkAct as AttackAction;
                                }
                                break;

                            case Action.ActionType.FullRound:
                                fullRoundAtkDmg.Add(atkAct, avgDamage);
                                //If this full round attack has a higher average damage than the current highest full round attack
                                if (highestFullRound == null || fullRoundAtkDmg[highestFullRound] < avgDamage)
                                {
                                    //This attack becomes the new highest
                                    highestFullRound = atkAct as AttackAction;
                                }
                                break;
                        }
                    }
                }
            }
            
            //Int to hold the average damage if this enemy performs a standard attack, secondary attack, and quick attack
            int avgDamageSSQ = 0;
            //If there's a highest standard attack action, we add its damage to our average
            if(highestStandard != null)
            {
                avgDamageSSQ += standardAtkDmg[highestStandard];
            }
            //If there's a highest secondary attack action, we add its damage to our average
            if(highestSecondary != null)
            {
                avgDamageSSQ += secondaryAtkDmg[highestSecondary];
            }
            //If there's a highest quick attack action, we add its damage to our average
            if(highestQuick != null)
            {
                avgDamageSSQ += quickAtkDmg[highestQuick];
            }
            
            //Int to hold the average damage if this enemy performs a full round attack and quick attack
            int avgDamageFRQ = 0;
            //If there's a highest full round attack action, we add its damage to our average
            if(highestFullRound != null)
            {
                avgDamageFRQ += fullRoundAtkDmg[highestFullRound];
            }
            //If there's a highest quick attack action, we add its damage to our average
            if (highestQuick != null)
            {
                avgDamageFRQ += quickAtkDmg[highestQuick];
            }

            //If the combination of standard/secondary/quick attacks has a HIGHER average than the full round/quick attacks
            if(avgDamageSSQ > avgDamageFRQ)
            {
                //Adding the attacks to our list in order
                if(highestStandard != null)
                {
                    EnemyActionAndTile newActTile = new EnemyActionAndTile(highestStandard, targetPlayerTile);
                    this.actionsToPerformOnTiles.Add(newActTile);
                }
                if(highestSecondary != null)
                {
                    EnemyActionAndTile newActTile = new EnemyActionAndTile(highestSecondary, targetPlayerTile);
                    this.actionsToPerformOnTiles.Add(newActTile);
                }
                if(highestQuick != null)
                {
                    EnemyActionAndTile newActTile = new EnemyActionAndTile(highestQuick, targetPlayerTile);
                    this.actionsToPerformOnTiles.Add(newActTile);
                }
            }
            //If the combination of standard/secondary/quick attacks has a LOWER average than the full round/quick attacks
            else
            {
                //Adding the attacks to our list in order
                if(highestFullRound != null)
                {
                    EnemyActionAndTile newActTile = new EnemyActionAndTile(highestFullRound, targetPlayerTile);
                    this.actionsToPerformOnTiles.Add(newActTile);
                }
                if(highestQuick != null)
                {
                    EnemyActionAndTile newActTile = new EnemyActionAndTile(highestQuick, targetPlayerTile);
                    this.actionsToPerformOnTiles.Add(newActTile);
                }
            }
        }
        //If this enemy isn't within the preferred distance
        else
        {
            //Looping through all of our behavior's added actions to see if there's a full round move action. If so, we're allowed to use full round actions for movement
            bool canUseFullRoundMove = false;
            foreach(Action addedAct in this.behaviorList[0].addedActions)
            {
                //If the current action is a full round action and is a move action, we're allowed to use full round move actions
                if(addedAct.type == Action.ActionType.FullRound && addedAct.GetType() == typeof(MoveAction))
                {
                    canUseFullRoundMove = true;
                    break;
                }
            }

            //Getting the list of movement actions that we will use to reach the target
            List<MoveAction> moveActionsToTake = this.FindMinimumMoveActs(currentDist, canUseFullRoundMove);

            //Adding all of our move actions to our list of actions to take this turn
            int currentMoveDist = 0;
            foreach(MoveAction moveActToPerform in moveActionsToTake)
            {
                //Adding this move action's range to our current move distance so we know which tile this action stops at
                currentMoveDist += moveActToPerform.range;

                //If the move distance is past where the target is, we stop at the last tile
                if(currentMoveDist >= currentDist)
                {
                    currentMoveDist = pathToTarget.Count - 1;
                }

                //Creating a new enemy action and tile class so we know where this enemy will perform this action
                EnemyActionAndTile mvEAT = new EnemyActionAndTile(moveActToPerform, pathToTarget[currentMoveDist]);
                this.actionsToPerformOnTiles.Add(mvEAT);
            }
            
            //Finding out which moves are still available
            bool quickActAvailable = true;
            bool secondaryActAvailable = true;
            bool standardActAvailable = true;
            bool fullRoundActAvailable = true;
            
            //Looping through all of the move actions we know we're going to take so we know which attack actions are available
            foreach (MoveAction mv in moveActionsToTake)
            {
                switch(mv.type)
                {
                    //If we're taking a quick move act, we can't make a quick attack act
                    case Action.ActionType.Quick:
                        quickActAvailable = false;
                        break;
                    //If we're taking a secondary move act, we can't make a secondary or full round attack act
                    case Action.ActionType.Secondary:
                        secondaryActAvailable = false;
                        fullRoundActAvailable = false;
                        break;
                    //If we're taking a standard move act, we can't make a standard or full round attack act
                    case Action.ActionType.Standard:
                        standardActAvailable = false;
                        fullRoundActAvailable = false;
                        break;
                    //If we're taking a full round move act, we can't make a full round, standard, or secondary attack act
                    case Action.ActionType.FullRound:
                        fullRoundActAvailable = false;
                        standardActAvailable = false;
                        secondaryActAvailable = false;
                        break;
                }
            }
            
            //Creating a dictionary of actions and their estimated damage for each action type
            Dictionary<AttackAction, int> standardAtkDmg = new Dictionary<AttackAction, int>();
            Dictionary<AttackAction, int> secondaryAtkDmg = new Dictionary<AttackAction, int>();
            Dictionary<AttackAction, int> quickAtkDmg = new Dictionary<AttackAction, int>();
            Dictionary<AttackAction, int> fullRoundAtkDmg = new Dictionary<AttackAction, int>();

            //Tracking the actions that have the highest amount of damage
            AttackAction highestStandard = null;
            AttackAction highestSecondary = null;
            AttackAction highestQuick = null;
            AttackAction highestFullRound = null;

            //Looping through all of the actions for our current behavior to find their average damage and type
            foreach (Action addedAct in this.behaviorList[0].addedActions)
            {
                //If this added action is an attack, we can use it
                if (addedAct.GetType() == typeof(AttackAction))
                {
                    //If the current attack action is within range of the target, we can use it
                    if (addedAct.range >= this.behaviorList[0].preferredDistFromTarget)
                    {
                        //Int to hold this attack's average damage
                        int avgDamage = this.FindAttackAverageDamage(addedAct as AttackAction, this.playerCharToAttack);

                        //Getting the type of action this attack is
                        switch (addedAct.type)
                        {
                            case Action.ActionType.Standard:
                                standardAtkDmg.Add(addedAct as AttackAction, avgDamage);
                                //If this standard attack has a higher average damage than the current highest standard attack
                                if (highestStandard == null || standardAtkDmg[highestStandard] < avgDamage)
                                {
                                    //This attack becomes the new highest
                                    highestStandard = addedAct as AttackAction;
                                }
                                break;

                            case Action.ActionType.Secondary:
                                secondaryAtkDmg.Add(addedAct as AttackAction, avgDamage);
                                //If this secondary attack has a higher average damage than the current highest secondary attack
                                if (highestSecondary == null || secondaryAtkDmg[highestSecondary] < avgDamage)
                                {
                                    //This attack becomes the new highest
                                    highestSecondary = addedAct as AttackAction;
                                }
                                break;

                            case Action.ActionType.Quick:
                                quickAtkDmg.Add(addedAct as AttackAction, avgDamage);
                                //If this quick attack has a higher average damage than the current highest quick attack
                                if (highestQuick == null || quickAtkDmg[highestQuick] < avgDamage)
                                {
                                    //This attack becomes the new highest
                                    highestQuick = addedAct as AttackAction;
                                }
                                break;

                            case Action.ActionType.FullRound:
                                fullRoundAtkDmg.Add(addedAct as AttackAction, avgDamage);
                                //If this full round attack has a higher average damage than the current highest full round attack
                                if (highestFullRound == null || fullRoundAtkDmg[highestFullRound] < avgDamage)
                                {
                                    //This attack becomes the new highest
                                    highestFullRound = addedAct as AttackAction;
                                }
                                break;
                        }
                    }
                }
            }
            
            //If we can use more than just this behavior's added actions
            if (!this.behaviorList[0].onlyUseAddedActions)
            {
                //Looping through all of the actions to find the average damage for each of them and what type of action they are
                foreach (AttackAction atkAct in this.attackActionList)
                {
                    //If the current attack action is within range of the target, we can use it
                    if (atkAct.range >= (currentDist - currentMoveDist))
                    {
                        //Int to hold this attack's average damage
                        int avgDamage = this.FindAttackAverageDamage(atkAct, this.playerCharToAttack);

                        //Getting the type of action this attack is
                        switch (atkAct.type)
                        {
                            case Action.ActionType.Standard:
                                standardAtkDmg.Add(atkAct, avgDamage);
                                //If this standard attack has a higher average damage than the current highest standard attack
                                if (highestStandard == null || standardAtkDmg[highestStandard] < avgDamage)
                                {
                                    //This attack becomes the new highest
                                    highestStandard = atkAct as AttackAction;
                                }
                                break;

                            case Action.ActionType.Secondary:
                                secondaryAtkDmg.Add(atkAct, avgDamage);
                                //If this secondary attack has a higher average damage than the current highest secondary attack
                                if (highestSecondary == null || secondaryAtkDmg[highestSecondary] < avgDamage)
                                {
                                    //This attack becomes the new highest
                                    highestSecondary = atkAct as AttackAction;
                                }
                                break;

                            case Action.ActionType.Quick:
                                quickAtkDmg.Add(atkAct, avgDamage);
                                //If this quick attack has a higher average damage than the current highest quick attack
                                if (highestQuick == null || quickAtkDmg[highestQuick] < avgDamage)
                                {
                                    //This attack becomes the new highest
                                    highestQuick = atkAct as AttackAction;
                                }
                                break;

                            case Action.ActionType.FullRound:
                                fullRoundAtkDmg.Add(atkAct, avgDamage);
                                //If this full round attack has a higher average damage than the current highest full round attack
                                if (highestFullRound == null || fullRoundAtkDmg[highestFullRound] < avgDamage)
                                {
                                    //This attack becomes the new highest
                                    highestFullRound = atkAct as AttackAction;
                                }
                                break;
                        }
                    }
                }
            }

            //If our quick action is available and we've found a quick attack action, we add it to our list
            if (quickActAvailable && highestQuick != null)
            {
                EnemyActionAndTile qEAT = new EnemyActionAndTile(highestQuick, targetPlayerTile);
                this.actionsToPerformOnTiles.Add(qEAT);
            }
            //If our secondary action is available and we've found a secondary attack action, we add it to our list
            if(secondaryActAvailable && highestSecondary != null)
            {
                EnemyActionAndTile scEAT = new EnemyActionAndTile(highestSecondary, targetPlayerTile);
                this.actionsToPerformOnTiles.Add(scEAT);
            }
            //If our standard action is available and we've found a standard attack action, we add it to our list
            if(standardActAvailable && highestStandard != null)
            {
                EnemyActionAndTile stEAT = new EnemyActionAndTile(highestStandard, targetPlayerTile);
                this.actionsToPerformOnTiles.Add(stEAT);
            }
            //If our full round action is available and we've found a full round attack action, we add it to our list
            if(fullRoundActAvailable && highestFullRound != null)
            {
                EnemyActionAndTile frEAT = new EnemyActionAndTile(highestFullRound, targetPlayerTile);
                this.actionsToPerformOnTiles.Add(frEAT);
            }
        }


        //If we have actions to perform, then we're ready to inform the combat manager (look in Update)
        if(this.actionsToPerformOnTiles.Count > 0)
        {
            this.readyToAct = true;
        }
        //If we don't have any actions, then we end our turn
        else
        {
            this.EndEnemyTurn();
        }
    }


    //Function called from StartEnemyTurn if the current behavior is SUPPORT
    private void SupportStateLogic()
    {
        Debug.Log("Support Logic");
        this.EndEnemyTurn();
    }


    //Function called from StartEnemyTurn if the current behavior is DEFENSIVE
    private void DefensiveStateLogic()
    {
        Debug.Log("Defensive Logic");
        this.EndEnemyTurn();
    }


    //Function called from StartEnemyTurn if the current behavior is TERRIFIED
    private void TerrifiedStateLogic()
    {
        Debug.Log("Terrified Logic");
        this.EndEnemyTurn();
    }


    //Function called from HostileStateLogic. Finds out which player to target based on threat and preference
    private void FindPlayerTarget()
    {
        //If this character takes threat into consideration
        if (!this.behaviorList[this.validBehaviorIndexList[0]].ignoreThreat)
        {
            //Ranking the threat list in order from highest to lowest threat
            this.RankThreatList();
        }

        //Finding the target based on preference
        switch(this.behaviorList[this.validBehaviorIndexList[0]].preferredTargetType)
        {
            //Finding the character with the highest threat
            case StateBehavior.PlayerTargetPreference.HighestThreat:
                this.playerCharToAttack = this.threatList[0].characterRef;
                break;

            //Finding the character with the lowest threat
            case StateBehavior.PlayerTargetPreference.LowestThreat:
                this.playerCharToAttack = this.threatList[this.threatList.Count - 1].characterRef;
                break;
            
            //Finding the closest character to this enemy
            case StateBehavior.PlayerTargetPreference.ClosestPlayer:
                //The index for the character that best matches our attack preference
                int closestDistIndex = 0;
                //The distance that's currently the closest
                float closestDist = 100000000000;
                //Finding this enemy character's position on the combat tile grid
                CombatTile thisEnemyTile = CombatManager.globalReference.combatTileGrid[this.ourCharacter.charCombatStats.gridPositionCol][this.ourCharacter.charCombatStats.gridPositionRow];

                //Looping through the threat list to find the character that's closest to this enemy
                for (int t = 0; t < this.threatList.Count; ++t)
                {
                    //If we don't ignore threat, the loop breaks after the top 3 characters
                    if(!this.behaviorList[this.validBehaviorIndexList[0]].ignoreThreat && t > 2)
                    {
                        break;
                    }

                    //Finding the current character's position on the combat tile grid
                    CombatTile currentCharTile = CombatManager.globalReference.combatTileGrid[this.threatList[t].characterRef.charCombatStats.gridPositionCol][this.threatList[t].characterRef.charCombatStats.gridPositionRow];

                    //Finding the distance between the current character's tile and this enemy's tile
                    float distToCheck = Vector3.Distance(thisEnemyTile.gameObject.transform.position, currentCharTile.gameObject.transform.position);

                    //If the distance to check is closer than the current closest distance, we save this character's index as the best
                    if(distToCheck < closestDist)
                    {
                        closestDistIndex = t;
                    }
                }

                //Now that we've found the character that matches our criteria the best, we set it as the target
                this.playerCharToAttack = this.threatList[closestDistIndex].characterRef;
                break;

            //Finding the furthest character to this enemy
            case StateBehavior.PlayerTargetPreference.FurthestPlayer:
                //The index for the character that best matches our attack preference
                int furthestDistIndex = 0;
                //The distance that's currently the furthest
                float furthestDist = 0;
                //Finding this enemy character's position on the combat tile grid
                CombatTile ourEnemyTile = CombatManager.globalReference.combatTileGrid[this.ourCharacter.charCombatStats.gridPositionCol][this.ourCharacter.charCombatStats.gridPositionRow];

                //Looping through the threat list to find the character that's furthest from this enemy
                for (int t = 0; t < this.threatList.Count; ++t)
                {
                    //If we don't ignore threat, the loop breaks after the top 3 characters
                    if (!this.behaviorList[this.validBehaviorIndexList[0]].ignoreThreat && t > 2)
                    {
                        break;
                    }

                    //Finding the current character's position on the combat tile grid
                    CombatTile currentCharTile = CombatManager.globalReference.combatTileGrid[this.threatList[t].characterRef.charCombatStats.gridPositionCol][this.threatList[t].characterRef.charCombatStats.gridPositionRow];

                    //Finding the distance between the current character's tile and this enemy's tile
                    float distToCheck = Vector3.Distance(ourEnemyTile.gameObject.transform.position, currentCharTile.gameObject.transform.position);

                    //If the distance to check is further than the current furthest distance, we save this character's index as the best
                    if (distToCheck > furthestDist)
                    {
                        furthestDistIndex = t;
                    }
                }

                //Now that we've found the character that matches our criteria the best, we set it as the target
                this.playerCharToAttack = this.threatList[furthestDistIndex].characterRef;
                break;

            //Finding the character with the lowest health
            case StateBehavior.PlayerTargetPreference.LowestHealth:
                //The index for the character that best matches our attack preference
                int lowestHPIndex = 0;

                //Looping through the threat list to find the character with the lowest health
                for(int t = 0; t < this.threatList.Count; ++t)
                {
                    //If we don't ignore threat, the loop breaks after the top 3 characters
                    if(!this.behaviorList[this.validBehaviorIndexList[0]].ignoreThreat && t > 2)
                    {
                        break;
                    }

                    //If the character at the current index has less health than the character at the best index so far, we save the current index as the best
                    if(this.threatList[t].characterRef.charPhysState.currentHealth < this.threatList[lowestHPIndex].characterRef.charPhysState.currentHealth)
                    {
                        lowestHPIndex = t;
                    }
                }

                //Now that we've found the character that matches our criteria the best, we set it as the target
                this.playerCharToAttack = this.threatList[lowestHPIndex].characterRef;
                break;

            //Finding the character with the highest health
            case StateBehavior.PlayerTargetPreference.HighestHealth:
                //The index for the character that best matches our attack preference
                int highestHPIndex = 0;

                //Looping through the threat list to find the character with the highest health
                for (int t = 0; t < this.threatList.Count; ++t)
                {
                    //If we don't ignore threat, the loop breaks after the top 3 characters
                    if (!this.behaviorList[this.validBehaviorIndexList[0]].ignoreThreat && t > 2)
                    {
                        break;
                    }

                    //If the character at the current index has more health than the character at the best index so far, we save the current index as the best
                    if (this.threatList[t].characterRef.charPhysState.currentHealth > this.threatList[highestHPIndex].characterRef.charPhysState.currentHealth)
                    {
                        highestHPIndex = t;
                    }
                }

                //Now that we've found the character that matches our criteria the best, we set it as the target
                this.playerCharToAttack = this.threatList[highestHPIndex].characterRef;
                break;

            //Finding the character with the lowest armor
            case StateBehavior.PlayerTargetPreference.LowestArmor:
                //The index for the character that best matches our attack preference
                int lowestArmorIndex = 0;

                //Looping through the threat list to find the character with the lowest armor value
                for (int t = 0; t < this.threatList.Count; ++t)
                {
                    //If we don't ignore threat, the loop breaks after the top 3 characters
                    if (!this.behaviorList[this.validBehaviorIndexList[0]].ignoreThreat && t > 2)
                    {
                        break;
                    }

                    //If the character at the current index has less armor than the character at the best index so far, we save the current index as the best
                    if (this.threatList[t].characterRef.charInventory.totalPhysicalArmor < this.threatList[lowestArmorIndex].characterRef.charInventory.totalPhysicalArmor)
                    {
                        lowestArmorIndex = t;
                    }
                }

                //Now that we've found the character that matches our criteria the best, we set it as the target
                this.playerCharToAttack = this.threatList[lowestArmorIndex].characterRef;
                break;

            //Finding the character with the highest armor
            case StateBehavior.PlayerTargetPreference.HighestArmor:
                //The index for the character that best matches our attack preference
                int highestArmorIndex = 0;

                //Looping through the threat list to find the character with the highest armor value
                for (int t = 0; t < this.threatList.Count; ++t)
                {
                    //If we don't ignore threat, the loop breaks after the top 3 characters
                    if (!this.behaviorList[this.validBehaviorIndexList[0]].ignoreThreat && t > 2)
                    {
                        break;
                    }

                    //If the character at the current index has more armor than the character at the best index so far, we save the current index as the best
                    if (this.threatList[t].characterRef.charInventory.totalPhysicalArmor > this.threatList[highestArmorIndex].characterRef.charInventory.totalPhysicalArmor)
                    {
                        highestArmorIndex = t;
                    }
                }

                //Now that we've found the character that matches our criteria the best, we set it as the target
                this.playerCharToAttack = this.threatList[highestArmorIndex].characterRef;
                break;

            //Finding the character with the most quest items in their inventory
            case StateBehavior.PlayerTargetPreference.QuestItem:
                //The index for the character that best matches our attack preference
                int mostQuestItemsIndex = 0;
                //The number of quest items that the current best character has in their inventory
                int mostQuestItems = 0;

                //Looping through the threat list to find the character with the most quest items in their inventory
                for(int t = 0; t < this.threatList.Count; ++t)
                {
                    //If we don't ignore threat, the loop breaks after the top 3 characters
                    if (!this.behaviorList[this.validBehaviorIndexList[0]].ignoreThreat && t > 2)
                    {
                        break;
                    }

                    //Int that tracks the number of quest items the current character has
                    int thisCharsQuestItems = 0;

                    //Looping through the current character's inventory to find quest items
                    foreach(Item currentItem in this.threatList[t].characterRef.charInventory.itemSlots)
                    {
                        //If the current item is a quest item, we increase the count for this character's quest itmes
                        if(currentItem.GetComponent<QuestItem>())
                        {
                            thisCharsQuestItems += 1;
                        }
                    }

                    //Checking each equipped armor slot for quest items
                    if(this.threatList[t].characterRef.charInventory.helm != null && this.threatList[t].characterRef.charInventory.helm.GetComponent<QuestItem>())
                    {
                        thisCharsQuestItems += 1;
                    }
                    if (this.threatList[t].characterRef.charInventory.chestPiece != null && this.threatList[t].characterRef.charInventory.chestPiece.GetComponent<QuestItem>())
                    {
                        thisCharsQuestItems += 1;
                    }
                    if (this.threatList[t].characterRef.charInventory.leggings != null && this.threatList[t].characterRef.charInventory.leggings.GetComponent<QuestItem>())
                    {
                        thisCharsQuestItems += 1;
                    }
                    if (this.threatList[t].characterRef.charInventory.shoes != null && this.threatList[t].characterRef.charInventory.shoes.GetComponent<QuestItem>())
                    {
                        thisCharsQuestItems += 1;
                    }
                    if (this.threatList[t].characterRef.charInventory.gloves != null && this.threatList[t].characterRef.charInventory.gloves.GetComponent<QuestItem>())
                    {
                        thisCharsQuestItems += 1;
                    }
                    if (this.threatList[t].characterRef.charInventory.ring != null && this.threatList[t].characterRef.charInventory.ring.GetComponent<QuestItem>())
                    {
                        thisCharsQuestItems += 1;
                    }
                    if (this.threatList[t].characterRef.charInventory.necklace != null && this.threatList[t].characterRef.charInventory.necklace.GetComponent<QuestItem>())
                    {
                        thisCharsQuestItems += 1;
                    }
                    if (this.threatList[t].characterRef.charInventory.cloak != null && this.threatList[t].characterRef.charInventory.cloak.GetComponent<QuestItem>())
                    {
                        thisCharsQuestItems += 1;
                    }
                    if (this.threatList[t].characterRef.charInventory.rightHand != null && this.threatList[t].characterRef.charInventory.rightHand.GetComponent<QuestItem>())
                    {
                        thisCharsQuestItems += 1;
                    }
                    if (this.threatList[t].characterRef.charInventory.leftHand != null && this.threatList[t].characterRef.charInventory.leftHand.GetComponent<QuestItem>())
                    {
                        thisCharsQuestItems += 1;
                    }

                    //If the current character has more quest items than the best so far, we set the current index as the best
                    if(thisCharsQuestItems > mostQuestItems)
                    {
                        mostQuestItemsIndex = t;
                        mostQuestItems = thisCharsQuestItems;
                    }
                }

                //Now that we've found the character that matches our criteria the best, we set it as the target
                this.playerCharToAttack = this.threatList[mostQuestItemsIndex].characterRef;
                break;
        }
    }


    //Function called from HostileStateLogic. Finds the average damage for an attack action
    private int FindAttackAverageDamage(AttackAction attackAct_, Character targetChar_)
    {
        //Int to hold this attack's average damage
        int avgDamage = 0;

        //Looping through all of the attack damage classes for this attack
        foreach (AttackDamage dmg in attackAct_.damageDealt)
        {
            //Starting with the lowest dice roll amount
            int atkAvg = dmg.diceRolled;
            //Adding the highest dice roll amount
            atkAvg += (dmg.diceRolled * dmg.diceRolled);
            //Averaging the damage between the highest and lowest roll values
            atkAvg = atkAvg / 2;
            //Adding the base amount of damage
            atkAvg += dmg.baseDamage;

            //Adding the average non-crit damage
            avgDamage += Mathf.RoundToInt((1 - attackAct_.critChance) * atkAvg);
            //Adding the average crit damage
            avgDamage += Mathf.RoundToInt(attackAct_.critChance * atkAvg) * attackAct_.critMultiplier;
        }

        //Looping through all of the attack effect classes for this attack
        foreach (AttackEffect eft in attackAct_.effectsOnHit)
        {
            //If the effect hits enemies
            if (eft.effectedTargets == AttackEffect.EffectedTargets.Defender || eft.effectedTargets == AttackEffect.EffectedTargets.EnemiesOnly ||
                eft.effectedTargets == AttackEffect.EffectedTargets.Everyone || eft.effectedTargets == AttackEffect.EffectedTargets.EnemiesExceptDefender)
            {
                //If the effect only hits a specific type or race, we need to make sure the target meets the race/type requirements
                if (!eft.hitEffectedType || (eft.effectedRace == RaceTypes.Races.None && eft.effectedType == RaceTypes.Subtypes.None) ||
                    (eft.effectedRace == this.playerCharToAttack.charRaceTypes.race && eft.effectedType == RaceTypes.Subtypes.None) ||
                    (eft.effectedRace == RaceTypes.Races.None && this.playerCharToAttack.charRaceTypes.subtypeList.Contains(eft.effectedType) ||
                    (eft.effectedRace == this.playerCharToAttack.charRaceTypes.race && this.playerCharToAttack.charRaceTypes.subtypeList.Contains(eft.effectedType))))
                {
                    //If the effect is a damage over time
                    if (eft.effectToApply.GetType() == typeof(DamageOverTimeEffect))
                    {
                        //Bool to track if the enemy is already effected by this DoT
                        bool alreadyHasDot = false;
                        //Looping through all of the combat effects on the target character
                        foreach(Effect ce in targetChar_.charCombatStats.combatEffects)
                        {
                            //If this target is already effected by this DoT, we can't apply it again
                            if(ce.effectName == eft.effectToApply.effectName)
                            {
                                alreadyHasDot = true;
                                break;
                            }
                        }

                        //If the target enemy isn't already effected by this effect
                        if (!alreadyHasDot)
                        {
                            DamageOverTimeEffect dotEft = eft.effectToApply as DamageOverTimeEffect;
                            //Starting with the average of the damage per tick
                            int dotDmgAvg = Mathf.RoundToInt(dotEft.damagePerTickRange.x + dotEft.damagePerTickRange.y) / 2;
                            //Multiplying the damage by the average number of ticks
                            dotDmgAvg *= (Mathf.RoundToInt(dotEft.numberOfTicksRange.x + dotEft.numberOfTicksRange.y) / 2);
                            //Multiplying by the chance to trigger
                            dotDmgAvg = Mathf.RoundToInt(dotDmgAvg * dotEft.chanceToTrigger);

                            //Adding the average non-crit damage
                            avgDamage += Mathf.RoundToInt((1 - dotEft.critChance) * dotDmgAvg);
                            //Adding the average crit damage
                            avgDamage += Mathf.RoundToInt(dotEft.critChance * dotDmgAvg) * dotEft.critMultiplier;
                        }
                    }
                    //If the effect is instant damage
                    else if (eft.effectToApply.GetType() == typeof(InstantDamageEffect))
                    {
                        InstantDamageEffect instantDamageEft = eft.effectToApply as InstantDamageEffect;
                        //Starting with the lowest dice roll amount
                        int instDmgAvg = instantDamageEft.diceRolled;
                        //Adding the highest roll amount
                        instDmgAvg += (instantDamageEft.diceRolled * instantDamageEft.diceSides);
                        //Averaging the damage between the highest and lowest roll values
                        instDmgAvg = instDmgAvg / 2;
                        //Adding the base amount of damage
                        instDmgAvg += instantDamageEft.baseDamage;

                        //Adding the average non-crit damage
                        avgDamage += Mathf.RoundToInt((1 - instantDamageEft.critChance) * instDmgAvg);
                        //Adding the average crit damage
                        avgDamage += Mathf.RoundToInt(instantDamageEft.critChance * instDmgAvg) * instantDamageEft.critMultiplier;
                    }
                }
            }
        }

        //Returning the average
        return avgDamage;
    }


    //Function called from all of the state logic functions to find the least amount of movement actions to get to a target range
    private List<MoveAction> FindMinimumMoveActs(int rangeToMeet_, bool allowFullRoundMoves_ = false)
    {
        //Int to hold the current range of all of our movement actions
        int currentMoveSum = 0;
        //Move actions for each of the different types
        MoveAction quickMoveAct = null;
        MoveAction secondaryMoveAct = null;
        MoveAction standardMoveAct = null;
        MoveAction fullRoundMoveAct = null;

        //Looping through all of our added actions to see if they are Quick Movement actions
        foreach (Action aaQ in this.behaviorList[0].addedActions)
        {
            //If the action is a movement action and is a quick action
            if (aaQ.type == Action.ActionType.Quick && aaQ.GetType() == typeof(MoveAction))
            {
                //If the current quick move action is null or has a lower range, this action becomes the one we use
                if (quickMoveAct == null || quickMoveAct.range < aaQ.range)
                {
                    quickMoveAct = aaQ as MoveAction;
                    currentMoveSum = aaQ.range;

                    //If this quick action is enough to reach the target, we don't need to keep looking
                    if (currentMoveSum >= rangeToMeet_)
                    {
                        break;
                    }
                }
            }
        }
        
        //If we can use all of our actions and not just the added actions, we loop through all of our other move actions
        if (currentMoveSum < rangeToMeet_ && !this.behaviorList[0].onlyUseAddedActions)
        {
            //Looping through all of our move actions
            foreach (MoveAction moveAct in this.moveActionList)
            {
                //If the current move action is a quick action and has a longer range than our current quick action
                if (moveAct.type == Action.ActionType.Quick && moveAct.range > currentMoveSum)
                {
                    //This move act becomes the new quick move
                    quickMoveAct = moveAct;
                    currentMoveSum = quickMoveAct.range;

                    //If we have enough range to reach the target, we can stop looking for more actions
                    if(currentMoveSum >= rangeToMeet_)
                    {
                        break;
                    }
                }
            }
        }
        
        //If we still don't have enough movement range with the quick actions, we loop through secondary actions
        if (currentMoveSum < rangeToMeet_)
        {
            //Looping through all of our behavior's added actions
            foreach(Action aaS in this.behaviorList[0].addedActions)
            {
                //If the action is a movement action and is a secondary action
                if(aaS.type == Action.ActionType.Secondary && aaS.GetType() == typeof(MoveAction))
                {
                    //If the current secondary move action is null or has a lower range, this action becomes the one we use
                    if(secondaryMoveAct == null || secondaryMoveAct.range < aaS.range)
                    {
                        //If the secondary move act isn't null, we remove its range from our move sum
                        if(secondaryMoveAct != null)
                        {
                            currentMoveSum -= secondaryMoveAct.range;
                        }

                        secondaryMoveAct = aaS as MoveAction;
                        currentMoveSum += aaS.range;

                        //If this secondary action has enough range to reach the target without any other action, we clear the quick action and break the loop
                        if(secondaryMoveAct.range >= rangeToMeet_)
                        {
                            if (quickMoveAct != null)
                            {
                                currentMoveSum -= quickMoveAct.range;
                                quickMoveAct = null;
                            }
                            break;
                        }
                        //Otherwise, if this secondary action and the quick action together can reach the target, we don't need to keep looking
                        else if(currentMoveSum >= rangeToMeet_)
                        {
                            break;
                        }
                    }
                }
            }

            //If we can use all of our move actions and not just the added actions, we loop through the rest of our move actions
            if(currentMoveSum < rangeToMeet_ && !this.behaviorList[0].onlyUseAddedActions)
            {
                //Looping through all of our move actions
                foreach(MoveAction moveAct in this.moveActionList)
                {
                    //If the current move action is a secondary action and has a longer range than our current secondary action
                    if(moveAct.type == Action.ActionType.Secondary && secondaryMoveAct == null || moveAct.range > secondaryMoveAct.range)
                    {
                        //If the current secondary move act isn't null, we subtract its range from our move sum
                        if(secondaryMoveAct != null)
                        {
                            currentMoveSum -= secondaryMoveAct.range;
                        }

                        //This move act becomes the new secondary move
                        secondaryMoveAct = moveAct;
                        currentMoveSum += secondaryMoveAct.range;

                        //If this secondary move act has enough range to reach the target without the quick action, we clear the quick action and break the loop
                        if(secondaryMoveAct.range >= rangeToMeet_)
                        {
                            if (quickMoveAct != null)
                            {
                                currentMoveSum -= quickMoveAct.range;
                                quickMoveAct = null;
                            }
                            break;
                        }
                        //If we have enough range to reach the target, we can stop looking for more actions
                        else if (currentMoveSum >= rangeToMeet_)
                        {
                            break;
                        }
                    }
                }
            }
        }
        
        //If we've looked through the quick and secondary actions and STILL don't have enough range, we loop through our standard actions
        if (currentMoveSum < rangeToMeet_)
        {
            //Looping through all of our behavior's added actions
            foreach(Action aaSt in this.behaviorList[0].addedActions)
            {
                //If the action is a movement action and is a standard action
                if(aaSt.type == Action.ActionType.Standard && aaSt.GetType() == typeof(MoveAction))
                {
                    //If the current standard move action is null or has a lower range, this action becomes the one we use
                    if(standardMoveAct == null || standardMoveAct.range < aaSt.range)
                    {
                        //If the standard move act isn't null, we remove its range from our move sum
                        if(standardMoveAct != null)
                        {
                            currentMoveSum -= standardMoveAct.range;
                        }

                        standardMoveAct = aaSt as MoveAction;
                        currentMoveSum += aaSt.range;

                        //If this standard action has enough range to reach the target without any other action, we clear the quick and secondary actions and break the loop
                        if(standardMoveAct.range >= rangeToMeet_)
                        {
                            if (quickMoveAct != null)
                            {
                                currentMoveSum -= quickMoveAct.range;
                                quickMoveAct = null;
                            }
                            if (secondaryMoveAct != null)
                            {
                                currentMoveSum -= secondaryMoveAct.range;
                                secondaryMoveAct = null;
                            }
                            break;
                        }
                        //If this standard action and the quick action together can reach the target without the secondary action, we clear the secondary action
                        else if(standardMoveAct.range + quickMoveAct.range >= rangeToMeet_)
                        {
                            if (secondaryMoveAct != null)
                            {
                                currentMoveSum -= secondaryMoveAct.range;
                                secondaryMoveAct = null;
                            }
                        }
                        //If this standard action and the secondary action together can reach the target without the quick action, we clear the quick action
                        else if(standardMoveAct.range + secondaryMoveAct.range >= rangeToMeet_)
                        {
                            if (quickMoveAct != null)
                            {
                                currentMoveSum -= quickMoveAct.range;
                                quickMoveAct = null;
                            }
                        }
                    }
                }
            }
            
            //If we can still use all of our move actions and not just the added actions, we loop through the rest of our move actions
            if(currentMoveSum < rangeToMeet_ && !this.behaviorList[0].onlyUseAddedActions)
            {
                //Looping through all of our move actions
                foreach(MoveAction moveAct in this.moveActionList)
                {
                    //If the current move action is a standard action and has a longer range than our current standard action
                    if(moveAct.type == Action.ActionType.Standard && (standardMoveAct == null || moveAct.range > standardMoveAct.range))
                    {
                        //If the current standard move act isn't null, we subtract its range from our move sum
                        if(standardMoveAct != null)
                        {
                            currentMoveSum -= standardMoveAct.range;
                        }

                        //This move act becomes the new standard move
                        standardMoveAct = moveAct;
                        currentMoveSum += standardMoveAct.range;

                        //If this standard move act has enough range to reach the target without the quick or secondary actions, we clear the other actions and break the loop
                        if(standardMoveAct.range >= rangeToMeet_)
                        {
                            if (quickMoveAct != null)
                            {
                                currentMoveSum -= quickMoveAct.range;
                                quickMoveAct = null;
                            }
                            if (secondaryMoveAct != null)
                            {
                                currentMoveSum -= secondaryMoveAct.range;
                                secondaryMoveAct = null;
                            }
                            break;
                        }
                        //If this standard move act and the quick move act can reach the target without the secondary action, we clear the secondary action
                        else if(standardMoveAct.range + quickMoveAct.range >= rangeToMeet_)
                        {
                            if (secondaryMoveAct != null)
                            {
                                currentMoveSum -= secondaryMoveAct.range;
                                secondaryMoveAct = null;
                            }
                        }
                        //If this standard move act and the secondary move act can reach the target without the quick action, we clear the quick action
                        else if(standardMoveAct.range + secondaryMoveAct.range >= rangeToMeet_)
                        {
                            if (quickMoveAct != null)
                            {
                                currentMoveSum -= quickMoveAct.range;
                                quickMoveAct = null;
                            }
                        }
                    }
                }
            }
        }
        
        //If we're still not in range and we're allowed to use full round actions, we loop through our full round actions
        if (currentMoveSum < rangeToMeet_ && allowFullRoundMoves_)
        {
            //Looping through all of our behavior's added actions
            foreach(Action aaFr in this.behaviorList[0].addedActions)
            {
                //If the action is a movement action and is a full round action
                if(aaFr.type == Action.ActionType.FullRound && aaFr.GetType() == typeof(MoveAction))
                {
                    //If the current full round action is null or has a lower range, this action becomes the one we use
                    if(fullRoundMoveAct == null || fullRoundMoveAct.range < aaFr.range)
                    {
                        //If the full round move act isn't null, we remove its range from our move sum
                        if (fullRoundMoveAct != null)
                        {
                            currentMoveSum -= fullRoundMoveAct.range;
                        }

                        fullRoundMoveAct = aaFr as MoveAction;
                        currentMoveSum += aaFr.range;

                        //If this full round action has enough range to reach the target without the quick action, we clear the quick, secondary and standard actions
                        if (fullRoundMoveAct.range >= rangeToMeet_)
                        {
                            if (quickMoveAct != null)
                            {
                                currentMoveSum -= quickMoveAct.range;
                                quickMoveAct = null;
                            }
                            if (secondaryMoveAct != null)
                            {
                                currentMoveSum -= secondaryMoveAct.range;
                                secondaryMoveAct = null;
                            }
                            if (standardMoveAct != null)
                            {
                                currentMoveSum -= standardMoveAct.range;
                                standardMoveAct = null;
                            }
                            break;
                        }
                        //If this full round action and the quick action together can reach the target without the secondary action, we clear the secondary action
                        else if (fullRoundMoveAct.range + quickMoveAct.range >= rangeToMeet_)
                        {
                            if (secondaryMoveAct != null)
                            {
                                currentMoveSum -= secondaryMoveAct.range;
                                secondaryMoveAct = null;
                            }
                            if (standardMoveAct != null)
                            {
                                currentMoveSum -= standardMoveAct.range;
                                standardMoveAct = null;
                            }
                            break;
                        }
                    }
                }
            }

            //If we can still use all of our move actions and not just the added actions, we loop through the rest of our move actions
            if(currentMoveSum < rangeToMeet_ && !this.behaviorList[0].onlyUseAddedActions)
            {
                //Looping through all of our move actions
                foreach(MoveAction moveAct in this.moveActionList)
                {
                    //If the current move action is a full round action and has a longer range than our current full round action
                    if(moveAct.type == Action.ActionType.FullRound && fullRoundMoveAct == null || moveAct.range > fullRoundMoveAct.range)
                    {
                        //If the current full round move act isn't null, we subtract its range from our move sum
                        if(fullRoundMoveAct != null)
                        {
                            currentMoveSum -= fullRoundMoveAct.range;
                        }

                        //This move act becomes the new full round move
                        fullRoundMoveAct = moveAct;
                        currentMoveSum += fullRoundMoveAct.range;

                        //If this full round action has enough range to reach the target without the quick action, we clear the quick, secondary and standard actions
                        if (fullRoundMoveAct.range >= rangeToMeet_)
                        {
                            if (quickMoveAct != null)
                            {
                                currentMoveSum -= quickMoveAct.range;
                                quickMoveAct = null;
                            }
                            if (secondaryMoveAct != null)
                            {
                                currentMoveSum -= secondaryMoveAct.range;
                                secondaryMoveAct = null;
                            }
                            if (standardMoveAct != null)
                            {
                                currentMoveSum -= standardMoveAct.range;
                                standardMoveAct = null;
                            }
                            break;
                        }
                        //If this full round action and the quick action together can reach the target without the secondary action, we clear the secondary action
                        else if (fullRoundMoveAct.range + quickMoveAct.range >= rangeToMeet_)
                        {
                            if (secondaryMoveAct != null)
                            {
                                currentMoveSum -= secondaryMoveAct.range;
                                secondaryMoveAct = null;
                            }
                            if (standardMoveAct != null)
                            {
                                currentMoveSum -= standardMoveAct.range;
                                standardMoveAct = null;
                            }
                            break;
                        }
                    }
                }
            }
        }
        
        //Returning all of the actions that aren't null
        List<MoveAction> returnList = new List<MoveAction>();
        if(quickMoveAct != null)
        {
            returnList.Add(quickMoveAct);
        }
        if(secondaryMoveAct != null)
        {
            returnList.Add(secondaryMoveAct);
        }
        if(standardMoveAct != null)
        {
            returnList.Add(standardMoveAct);
        }
        if(fullRoundMoveAct != null)
        {
            returnList.Add(fullRoundMoveAct);
        }
        return returnList;
    }


    //Function called from FindPlayerTarget. Organizes the threat list so that they're in decending order of threat
    private void RankThreatList()
    {
        //Looping through all of the characters on the threat list to see if any of them are dead
        foreach(PlayerThreatMeter ptm in this.threatList)
        {
            //If the current player is dead, their threat is cleared
            if(ptm.characterRef.charPhysState.currentHealth == 0)
            {
                ptm.threatLevel = 0;
            }
        }

        //Looping through the list starting from the beginning and ending at the 2nd to last index
        for(int i = 0; i < this.threatList.Count - 1; ++i)
        {
            //Looping through every index after the "i" index and ending at the end of the list
            for(int j = i + 1; j < this.threatList.Count; ++j)
            {
                //If the player at index "j" has a higher threat than the one at index "i" their positions are swapped
                if(this.threatList[j].threatLevel > this.threatList[i].threatLevel)
                {
                    PlayerThreatMeter placeholder = new PlayerThreatMeter(this.threatList[i].characterRef, this.threatList[i].threatLevel);

                    this.threatList[j].characterRef = this.threatList[i].characterRef;
                    this.threatList[j].threatLevel = this.threatList[i].threatLevel;

                    this.threatList[i].characterRef = placeholder.characterRef;
                    this.threatList[i].threatLevel = placeholder.threatLevel;
                }
            }
        }
    }

    
    //Function called from CombatManager.cs to add to a character's threat when they use an action
    public void IncreaseThreat(Character actingCharacter_, int threatToAdd_)
    {
        //Looping through our threat list until we find the character that's raising threat
        foreach(PlayerThreatMeter playerThreat in this.threatList)
        {
            //If we find the matching character, their threat is added
            if(playerThreat.characterRef == actingCharacter_)
            {
                playerThreat.threatLevel += threatToAdd_;

                //Making sure that the threat stays at or above 0 since there are effects to lower threat
                if(playerThreat.threatLevel < 0)
                {
                    playerThreat.threatLevel = 0;
                }
                break;
            }
        }
    }


    //Function called every frame
    private void Update()
    {
        //If we're still waiting for the action cooldown to expire, we subtract the amount of time that's passed
        if(this.actionCooldown > 0)
        {
            this.actionCooldown -= Time.deltaTime;
        }

        //If we aren't ready to act for some reason, OR we're waiting for the combat manager to let us give input, nothing happens
        if(!this.readyToAct || this.actionCooldown > 0 || CombatManager.globalReference.currentState != CombatManager.combatState.PlayerInput)
        {
            return;
        }

        //If we still have actions to complete, we tell the combat manager to perform them
        if (this.actionsToPerformOnTiles.Count > 0)
        {
            //Getting the action and the tile where it happens
            CombatManager.globalReference.PerformEnemyActionOnTile(this.actionsToPerformOnTiles[0].targetTile, this.actionsToPerformOnTiles[0].enemyActionToUse);

            //Setting our action cooldown for the amount of time this takes
            this.actionCooldown = this.actionsToPerformOnTiles[0].enemyActionToUse.timeToCompleteAction;

            //If this action is a move action, we need to multiply the cooldown based on how many tiles we're moving
            if(this.actionsToPerformOnTiles[0].enemyActionToUse.GetType() == typeof(MoveAction))
            {
                //Finding the distance this enemy is from the target
                CombatTile ourEnemyTile = CombatManager.globalReference.FindCharactersTile(this.ourCharacter);
                List<CombatTile> pathToTarget = PathfindingAlgorithms.BreadthFirstSearchCombat(ourEnemyTile, this.actionsToPerformOnTiles[0].targetTile, true, true);

                //Multiplying the number of tiles by the action time for our cooldown
                this.actionCooldown *= pathToTarget.Count;
            }

            //Clearing the current action so we don't use it again
            this.actionsToPerformOnTiles.RemoveAt(0);
        }
        //If there are no more actions to complete, we end our turn
        else
        {
            this.readyToAct = false;
            this.EndEnemyTurn();
        }
    }


    //Function called once our turn is over. We could just tell the CombatManager.cs direcly, but I want some uniformity with all of my state logic functions
    private void EndEnemyTurn()
    {
        CombatManager.globalReference.EndActingCharactersTurn();
    }
}



//Class used in EnemyCombatAI_Basic to track each character's threat level
public class PlayerThreatMeter
{
    //The character for this threat meter
    public Character characterRef;
    //The threat value for this character
    public int threatLevel = 0;


    //Constructor function for this class
    public PlayerThreatMeter(Character characterRef_, int threatLevel_)
    {
        this.characterRef = characterRef_;
        this.threatLevel = threatLevel_;
    }
}


//Class used in EnemyCombatAI_Basic.cs to determine which state this enemy will be in at a given time
[System.Serializable]
public class StateBehavior
{
    //The priority for this condition in case there are multiple behavior conditions that are true. 0 is lowest, 10 is highest
    [Range(0, 10)]
    public int priority = 1;

    //Enum for what kind of condition this is
    public enum ConditionalType { PersonalHPRange, OneAllyHPRange, HalfAlliesHPRange, AllAlliesHPRange, Debuffed, AllyDebuffed, AnyTargetBuffed, OnePlayerHPRange, HalfPlayersHPRange, AllPlayersHPRange };
    public ConditionalType conditional = ConditionalType.PersonalHPRange;

    //The range for health percentage for the TargetHPRange, PersonalHPRange, and AllyHPRange
    public Vector2 healthRange = new Vector2(0, 1);

    //The range of risky behavior from very low risk to very high risk
    [Range(0.1f, 0.9f)]
    public float chanceOfRisk = 0.5f;

    //Enum for what type of target this enemy prefers to attack
    public enum PlayerTargetPreference { HighestThreat, LowestThreat, ClosestPlayer, FurthestPlayer, LowestHealth, HighestHealth, LowestArmor, HighestArmor, QuestItem };
    public PlayerTargetPreference preferredTargetType = PlayerTargetPreference.HighestThreat;

    //Bool that determines if this enemy takes threat into consideration at all
    public bool ignoreThreat = false;

    //Enum for the state that this enemy shifts to once this state is entered
    public enum AICombatState { Hostile, Support, Defensive, Terrified };
    public AICombatState state = AICombatState.Hostile;

    //The preferred distance from the target
    [Range(1,13)]
    public int preferredDistFromTarget = 1;

    //The list of actions that are added to this enemy's action list when this state is entered
    public List<Action> addedActions;

    //If true, the actions in the addedActions list are the ONLY actions that can be used in this behavior
    public bool onlyUseAddedActions = false;
}


//Class used in EnemyCombatAI_Basic to hold an enemy's action and the tile that they use it on
public class EnemyActionAndTile
{
    //The action that the enemy will use
    public Action enemyActionToUse;
    //The tile that the action will be used on
    public CombatTile targetTile;

    //Constructor function for this class
    public EnemyActionAndTile(Action enemyAct_, CombatTile targetTile_)
    {
        this.enemyActionToUse = enemyAct_;
        this.targetTile = targetTile_;
    }
}