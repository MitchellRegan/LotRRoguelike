using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
[RequireComponent(typeof(EnemyTag))]
public class EnemyCombatAI_Basic : MonoBehaviour
{
    //Reference to this enemy's Character component
    private Character ourCharacter;

    //Enum for what type of target this enemy prefers to attack
    public enum PlayerTargetPreference { HighestThreat, LowestThreat, ClosestPlayer, FurthestPlayer, LowestHealth, HighestHealth, LowestArmor, HighestArmor, QuestItem };
    public PlayerTargetPreference preferredTargetType = PlayerTargetPreference.HighestThreat;

    //Bool that determines if this enemy takes threat into consideration at all
    public bool ignoreThreat = false;

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



    //Function called on the first frame this character exists
    private void Start()
    {
        //Getting the reference to this character component
        this.ourCharacter = this.GetComponent<Character>();

        //Initializing our threat list if we take threat into consideration
        if (!this.ignoreThreat)
        {
            this.threatList = new List<PlayerThreatMeter>();
        }

        //Looping through all characters in the current combat encounter and adding them to the threat list
        foreach(Character enemyChar in CombatManager.globalReference.playerCharactersInCombat)
        {
            this.threatList.Add(new PlayerThreatMeter(enemyChar, 0));
        }

        //Initializing our lists of actions
        this.attackActionList = new List<AttackAction>();
        this.effectActionList = new List<Action>();
        this.moveActionList = new List<MoveAction>();

        //Looping through all of the standard actions in our action list
        foreach(Action stdAct in this.ourCharacter.charActionList.standardActions)
        {
            //If the action is an attack
            if(stdAct.GetComponent<AttackAction>())
            {
                this.attackActionList.Add(stdAct.GetComponent<AttackAction>());

                //If this attack causes any effects
                if(stdAct.GetComponent<AttackAction>().effectsOnHit.Count > 0)
                {
                    this.effectActionList.Add(stdAct);
                }
            }
            //If the action is a move action
            else if(stdAct.GetComponent<MoveAction>())
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
        foreach (Action dftAct in this.ourCharacter.charActionList.defaultActions)
        {
            //If the action is an attack
            if (dftAct.GetComponent<AttackAction>())
            {
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
        }
    }

    
	//Function called from the CombatManager.cs when this enemy character's initiative is full
    public void StartEnemyTurn()
    {
        //Find which character to attack
        this.FindPlayerTarget();

        
        //When moving:
            //If moving closer to the target to attack
                //find the movement action with the highest range
                //Have the pathfinding algorithm find the fastest path to the target character (don't care about the range right now)
                //Follow the path but only for the number of tiles that the movement range has
            //If moving away from all player characters
                //Find the movement action with the highest range
                //Have the pathfinding algorithm find all tiles in range and return them
                //Make a list of floats that has the same number of indexes as the list of combat tiles
                //Loop through all combat tiles
                    //Loop through all living player characters (ignore dead)
                    //Find the distance between the current tile and the current player character's tile
                    //Add that distance to the value of the list of floats at the same index as the current tile
                //Loop through all of the indexes of floats to find the one with the largest value
                //The index with the largest value (furthest total distance from players) is the same index for the tile to move to
    }


    //Function called from StartEnemyTurn. Finds out which player to target based on threat and preference
    private void FindPlayerTarget()
    {
        //If this character takes threat into consideration
        if (!this.ignoreThreat)
        {
            //Ranking the threat list in order from highest to lowest threat
            this.RankThreatList();
        }

        //Finding the target based on preference
        switch(this.preferredTargetType)
        {
            //Finding the character with the highest threat
            case PlayerTargetPreference.HighestThreat:
                this.playerCharToAttack = this.threatList[0].characterRef;
                break;

            //Finding the character with the lowest threat
            case PlayerTargetPreference.LowestThreat:
                this.playerCharToAttack = this.threatList[this.threatList.Count - 1].characterRef;
                break;
            
            //Finding the closest character to this enemy
            case PlayerTargetPreference.ClosestPlayer:
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
                    if(!this.ignoreThreat && t > 2)
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
            case PlayerTargetPreference.FurthestPlayer:
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
                    if (!this.ignoreThreat && t > 2)
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
            case PlayerTargetPreference.LowestHealth:
                //The index for the character that best matches our attack preference
                int lowestHPIndex = 0;

                //Looping through the threat list to find the character with the lowest health
                for(int t = 0; t < this.threatList.Count; ++t)
                {
                    //If we don't ignore threat, the loop breaks after the top 3 characters
                    if(!this.ignoreThreat && t > 2)
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
            case PlayerTargetPreference.HighestHealth:
                //The index for the character that best matches our attack preference
                int highestHPIndex = 0;

                //Looping through the threat list to find the character with the highest health
                for (int t = 0; t < this.threatList.Count; ++t)
                {
                    //If we don't ignore threat, the loop breaks after the top 3 characters
                    if (!this.ignoreThreat && t > 2)
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
            case PlayerTargetPreference.LowestArmor:
                //The index for the character that best matches our attack preference
                int lowestArmorIndex = 0;

                //Looping through the threat list to find the character with the lowest armor value
                for (int t = 0; t < this.threatList.Count; ++t)
                {
                    //If we don't ignore threat, the loop breaks after the top 3 characters
                    if (!this.ignoreThreat && t > 2)
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
            case PlayerTargetPreference.HighestArmor:
                //The index for the character that best matches our attack preference
                int highestArmorIndex = 0;

                //Looping through the threat list to find the character with the highest armor value
                for (int t = 0; t < this.threatList.Count; ++t)
                {
                    //If we don't ignore threat, the loop breaks after the top 3 characters
                    if (!this.ignoreThreat && t > 2)
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
            case PlayerTargetPreference.QuestItem:
                //The index for the character that best matches our attack preference
                int mostQuestItemsIndex = 0;
                //The number of quest items that the current best character has in their inventory
                int mostQuestItems = 0;

                //Looping through the threat list to find the character with the most quest items in their inventory
                for(int t = 0; t < this.threatList.Count; ++t)
                {
                    //If we don't ignore threat, the loop breaks after the top 3 characters
                    if (!this.ignoreThreat && t > 2)
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


    //Function called from FindPlayerTarget. Organizes the threat list so that they're in decending order of threat
    private void RankThreatList()
    {
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
        .//GET TO WORK HERE!
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
