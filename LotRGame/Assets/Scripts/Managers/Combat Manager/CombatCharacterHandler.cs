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
            GameObject newEnemy = GameObject.Instantiate(ec.enemyCreature.gameObject);
            this.enemyCharacters.Add(newEnemy.GetComponent<Character>());
        }

        //Spawning the character models, setting their locations, and assigning any starting combat buffs
        this.SetCharacterTilePositions(playerParty_.combatDistance, enemyParty_);
        this.CreateCharacterModels();
        this.FindStartingBuffs();
    }


    //Function called from InitializeCharactersForCombat to set each character's tile position
    private void SetCharacterTilePositions(GroupCombatDistance partyDistance_, EnemyEncounter encounter_)
    {
        //The number of columns the player party is shifted
        int playerColShift = 0;

        //The number of columns the front half of the enemy encounter is shifted
        int enemyColShift0 = 0;
        int enemyColShift1 = 1;
        int enemyColShift2 = 2;
        int enemyColShift3 = 3;

        //Determine if we use the default enemy position or the ambush position
        EnemyCombatPosition encounterPos = encounter_.defaultPosition;

        //Rolling to see if this encounter will ambush the player
        float ambushRoll = Random.Range(0f, 1f);

        //If the enemies are able to ambush players, their encounter position is set to the ambush position
        if (ambushRoll < encounter_.ambushChance)
        {
            encounterPos = encounter_.ambushPosition;
        }

        //Determining which kind of enemy encounter the player's will be facing
        switch (encounterPos)
        {
            //If the enemy is in melee range
            case EnemyCombatPosition.MeleeFront:
                {
                    //Setting the player positions between col 0 - 6
                    switch (partyDistance_)
                    {
                        case GroupCombatDistance.Far://0-2
                            playerColShift = 0;
                            break;
                        case GroupCombatDistance.Medium://2-4
                            playerColShift = 2;
                            break;
                        case GroupCombatDistance.Close://4-6
                            playerColShift = 4;
                            break;
                    }

                    //Setting the enemy positions between col 7-10
                    enemyColShift0 += 7;
                    enemyColShift1 += 7;
                    enemyColShift2 += 7;
                    enemyColShift3 += 7;
                }
                break;

            case EnemyCombatPosition.MeleeFlanking:
                {
                    //Setting the player positions between col 6-8 regardless of their preferred distance
                    playerColShift = 6;

                    //Setting the enemy positions so that they're split between cols 4-5 and 9-10
                    enemyColShift0 += 4;//Col 5
                    enemyColShift1 += 8;//Col 9
                    enemyColShift2 += 1;//Col 4
                    enemyColShift3 += 7;//Col 10
                }
                break;

            case EnemyCombatPosition.MeleeBehind:
                {
                    //Setting the player positions between col 7-13
                    switch (partyDistance_)
                    {
                        case GroupCombatDistance.Far://7-9
                            playerColShift = 7;
                            break;
                        case GroupCombatDistance.Medium://9-11
                            playerColShift = 9;
                            break;
                        case GroupCombatDistance.Close://11-13
                            playerColShift = 11;
                            break;
                    }

                    //Setting the enemy positions between col 3-6 but in reverse order
                    enemyColShift0 += 6;
                    enemyColShift1 += 4;
                    enemyColShift2 += 2;
                    enemyColShift3 += 0;
                }
                break;

            //If the enemy is in middle range
            case EnemyCombatPosition.MiddleFront:
                {
                    //Setting the player positions between col 0 - 6
                    switch (partyDistance_)
                    {
                        case GroupCombatDistance.Far://0-2
                            playerColShift = 0;
                            break;
                        case GroupCombatDistance.Medium://2-4
                            playerColShift = 2;
                            break;
                        case GroupCombatDistance.Close://4-6
                            playerColShift = 4;
                            break;
                    }

                    //Setting the enemy positions between col 9-12
                    enemyColShift0 += 9;
                    enemyColShift1 += 9;
                    enemyColShift2 += 9;
                    enemyColShift3 += 9;
                }
                break;

            case EnemyCombatPosition.MiddleFlanking:
                {
                    //Setting the player positions between col 5-8
                    switch (partyDistance_)
                    {
                        case GroupCombatDistance.Far://5-7
                            playerColShift = 5;
                            break;
                        case GroupCombatDistance.Medium://6-8
                            playerColShift = 6;
                            break;
                        case GroupCombatDistance.Close://6-8
                            playerColShift = 6;
                            break;
                    }

                    //Setting the enemy positions split between cols 2-3 and cols 10-11
                    enemyColShift0 += 3;//Col 3
                    enemyColShift1 += 9;//Col 10
                    enemyColShift2 += 0;//Col 2
                    enemyColShift3 += 8;//Col 11
                }
                break;

            case EnemyCombatPosition.MiddleBehind:
                {
                    //Setting the player positions between col 7-13
                    switch (partyDistance_)
                    {
                        case GroupCombatDistance.Far://7-9
                            playerColShift = 7;
                            break;
                        case GroupCombatDistance.Medium://9-11
                            playerColShift = 9;
                            break;
                        case GroupCombatDistance.Close://11-13
                            playerColShift = 11;
                            break;
                    }

                    //Setting the enemy positions between col 1-4 but in reverse order
                    enemyColShift0 += 4;
                    enemyColShift1 += 2;
                    enemyColShift2 += 0;
                    enemyColShift3 += -2;
                }
                break;

            //If the enemy is in a far range
            case EnemyCombatPosition.RangedFront:
                {
                    //Setting the player positions between col 0 - 6
                    switch (partyDistance_)
                    {
                        case GroupCombatDistance.Far://0-2
                            playerColShift = 0;
                            break;
                        case GroupCombatDistance.Medium://2-4
                            playerColShift = 2;
                            break;
                        case GroupCombatDistance.Close://4-6
                            playerColShift = 4;
                            break;
                    }

                    //Setting the enemy positions between col 10-13
                    enemyColShift0 += 10;
                    enemyColShift1 += 10;
                    enemyColShift2 += 10;
                    enemyColShift3 += 10;
                }
                break;

            case EnemyCombatPosition.RangedFlanking:
                {
                    //Setting the player positions between col 5-8
                    switch (partyDistance_)
                    {
                        case GroupCombatDistance.Far://5-7
                            playerColShift = 5;
                            break;
                        case GroupCombatDistance.Medium://6-8
                            playerColShift = 6;
                            break;
                        case GroupCombatDistance.Close://6-8
                            playerColShift = 6;
                            break;
                    }

                    //Setting the enemy positions split between cols 0-1 and cols 12-13
                    enemyColShift0 += 1;//Col 1
                    enemyColShift1 += 3;//Col 12
                    enemyColShift2 += -2;//Col 0
                    enemyColShift3 += 10;//Col 13
                }
                break;

            case EnemyCombatPosition.RangedBehind:
                {
                    //Setting the player positions between col 7-11
                    switch (partyDistance_)
                    {
                        case GroupCombatDistance.Far://7-9
                            playerColShift = 7;
                            break;
                        case GroupCombatDistance.Medium://9-11
                            playerColShift = 9;
                            break;
                        case GroupCombatDistance.Close://11-13
                            playerColShift = 11;
                            break;
                    }

                    //Setting the enemy positions between col 0-3, so no change
                }
                break;
        }

        //After we've found the column shifts, we loop through and set the player positions
        foreach (Character playerChar in this.playerCharacters)
        {
            //Offsetting the player position column from the starting position
            playerChar.charCombatStats.gridPositionCol = playerChar.charCombatStats.startingPositionCol + playerColShift;
            playerChar.charCombatStats.gridPositionRow = playerChar.charCombatStats.startingPositionRow;
        }

        //Now we set the enemy positions based on the column shifts
        for (int e = 0; e < this.enemyCharacters.Count; e++)
        {
            //If this enemy's column position is random
            if (encounter_.enemies[e].randomCol)
            {
                this.enemyCharacters[e].charCombatStats.gridPositionCol = Random.Range(0, 3);
            }
            //If this enemy's column position isn't random
            else
            {
                this.enemyCharacters[e].charCombatStats.gridPositionCol = encounter_.enemies[e].specificCol;
            }

            //If this enemy's row position is random
            if (encounter_.enemies[e].randomRow)
            {
                this.enemyCharacters[e].charCombatStats.gridPositionRow = Random.Range(0, 4);
            }
            //If this enemy's row position isn't random
            else
            {
                this.enemyCharacters[e].charCombatStats.gridPositionRow = encounter_.enemies[e].specificRow;
            }

            //offsetting the enemy column position based on what their default position is
            switch (this.enemyCharacters[e].charCombatStats.gridPositionCol)
            {
                case 0:
                    this.enemyCharacters[e].charCombatStats.gridPositionCol = enemyColShift0;
                    break;
                case 1:
                    this.enemyCharacters[e].charCombatStats.gridPositionCol = enemyColShift1;
                    break;
                case 2:
                    this.enemyCharacters[e].charCombatStats.gridPositionCol = enemyColShift2;
                    break;
                case 3:
                    this.enemyCharacters[e].charCombatStats.gridPositionCol = enemyColShift3;
                    break;
                default:
                    //This case SHOULDN'T happen, but if it does, we treat it like the character's col is 0
                    this.enemyCharacters[e].charCombatStats.gridPositionCol = enemyColShift0;
                    break;
            }

            //Looping through all of the enemies created so far
            for (int i = 0; i < this.enemyCharacters.Count - 1; ++i)
            {
                //If the enemy we've just added shares the same combat row and column as another enemy
                if (this.enemyCharacters[e].charCombatStats.gameObject != this.enemyCharacters[i].gameObject &&
                    this.enemyCharacters[e].charCombatStats.gridPositionCol == this.enemyCharacters[i].charCombatStats.gridPositionCol &&
                    this.enemyCharacters[e].charCombatStats.gridPositionRow == this.enemyCharacters[i].charCombatStats.gridPositionRow)
                {
                    //The column shift amount for each column loop
                    int cShift = 0;

                    //Looping through all of the combat positions characters can be in
                    for (int c = 0; c < 4; ++c)
                    {
                        //Setting the column shift for this loop
                        if (c == 0)
                        {
                            cShift = enemyColShift0;
                        }
                        else if (c == 1)
                        {
                            cShift = enemyColShift1;
                        }
                        else if (c == 2)
                        {
                            cShift = enemyColShift2;
                        }
                        else if (c == 3)
                        {
                            cShift = enemyColShift3;
                        }

                        for (int r = 0; r < 8; ++r)
                        {
                            //Bool to track if the current position is empty
                            bool emptyPos = true;

                            //Looping through each enemy in the combat
                            foreach (Character ec in this.enemyCharacters)
                            {
                                //Making sure the enemy isn't somehow null or the enemy we've just created
                                if (ec != null && ec.gameObject != this.enemyCharacters[e].charCombatStats.gameObject)
                                {
                                    //If the tile isn't empty, we break out of this foreach loop
                                    if (ec.charCombatStats.gridPositionCol == cShift &&
                                        ec.charCombatStats.gridPositionRow == r)
                                    {
                                        emptyPos = false;
                                        break;
                                    }
                                }
                            }

                            //If we make it through all of the enemies without finding an overlap
                            if (emptyPos)
                            {
                                //Setting this enemy's position col and row to the empty position
                                this.enemyCharacters[e].charCombatStats.gridPositionCol = cShift;
                                this.enemyCharacters[e].charCombatStats.gridPositionRow = r;

                                //Breaking the row loop, col loop, and loop for checking other enemy positions
                                c = 10;
                                i = this.enemyCharacters.Count + 1;
                                break;
                            }
                        }
                    }
                }
            }
        }
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
        this.playerModels = new List<GameObject>();
        this.enemyModels = new List<GameObject>();

        //Creating each player character model at their tile position
        for(int p = 0; p < this.playerCharacters.Count; p++)
        {
            GameObject charModel = GameObject.Instantiate(this.playerCharacters[p].charModels.charModel);

            CombatTile3D tile = CombatManager.globalReference.tileHandler.FindCharactersTile(this.playerCharacters[p]);

            if(tile == null)
            {
                Debug.Log("Tile null");
            }
            else
            {
                charModel.transform.position = tile.gameObject.transform.position;
                this.playerModels.Add(charModel);
            }
        }

        //Creating each enemy character model at their tile position
        for(int e = 0; e < this.enemyCharacters.Count; e++)
        {
            GameObject charModel = GameObject.Instantiate(this.enemyCharacters[e].charModels.charModel);

            CombatTile3D tile = CombatManager.globalReference.tileHandler.FindCharactersTile(this.enemyCharacters[e]);

            if (tile == null)
            {
                Debug.Log("Tile null");
            }
            else
            {
                charModel.transform.position = tile.gameObject.transform.position;
                this.enemyModels.Add(charModel);
            }
        }
    }


    //Function called from InitializeCharactersForCombat to find starting buffs
    private void FindStartingBuffs()
    {
        //Looping through each player character to see if anyone has a Threat Boost perk or perk to start combat with an effect
        for (int t = 0; t < this.playerCharacters.Count; ++t)
        {
            //Looping through all of the current character's perks
            foreach (Perk charPerk in this.playerCharacters[t].charPerks.allPerks)
            {
                //If the current perk is a ThreatBoostPerk that works at the start of combat, we boost threat against this character for all enemies
                if (charPerk.GetType() == typeof(ThreatBoostPerk) && charPerk.GetComponent<ThreatBoostPerk>().increaseThreatAtStartOfCombat)
                {
                    CombatManager.globalReference.ApplyActionThreat(this.playerCharacters[t], null, charPerk.GetComponent<ThreatBoostPerk>().baseThreatToAdd, true);
                }
                //If the current perk is a StartCombatWithEffectPerk, we check to see if the effect will be actiavted
                else if (charPerk.GetType() == typeof(StartCombatWithEffectPerk))
                {
                    StartCombatWithEffectPerk effectPerk = charPerk.GetComponent<StartCombatWithEffectPerk>();

                    //Getting a random % roll to see if it's at the application chance
                    float randomRoll = Random.Range(0, 1);
                    if (randomRoll <= effectPerk.chanceToApplyEffect)
                    {
                        //Creating a new instance of the effect object prefab and triggering its effect
                        GameObject effectObj = Instantiate(effectPerk.effectToStartWith.gameObject, new Vector3(), new Quaternion());
                        effectObj.GetComponent<Effect>().TriggerEffect(this.playerCharacters[t], this.playerCharacters[t]);
                    }
                }
            }
        }
        
        //Looping through each enemy character to see if anyone has a perk to start combat with an effect
        for (int e = 0; e < this.enemyCharacters.Count; ++e)
        {
            //Looping through all of the current character's perks
            foreach (Perk charPerk in this.enemyCharacters[e].charPerks.allPerks)
            {
                //If the current perk is a StartCombatWithEffectPerk, we check to see if the effect will be actiavted
                if (charPerk.GetType() == typeof(StartCombatWithEffectPerk))
                {
                    StartCombatWithEffectPerk effectPerk = charPerk.GetComponent<StartCombatWithEffectPerk>();

                    //Getting a random % roll to see if it's at the application chance
                    float randomRoll = Random.Range(0, 1);
                    if (randomRoll <= effectPerk.chanceToApplyEffect)
                    {
                        //Creating a new instance of the effect object prefab and triggering its effect
                        GameObject effectObj = Instantiate(effectPerk.effectToStartWith.gameObject, new Vector3(), new Quaternion());
                        effectObj.GetComponent<Effect>().TriggerEffect(this.enemyCharacters[e], this.enemyCharacters[e]);
                    }
                }
            }
        }
    }


    //Function called externally to get the reference to a character's model object
    public GameObject GetCharacterModel(Character charToGet_)
    {
        //If the character is a player character
        if (this.playerCharacters.Contains(charToGet_))
        {
            int index = this.playerCharacters.IndexOf(charToGet_);
            return this.playerModels[index];
        }
        //If the character is an enemy character
        else if (this.enemyCharacters.Contains(charToGet_))
        {
            int index = this.enemyCharacters.IndexOf(charToGet_);
            return this.enemyModels[index];
        }
        //Otherwise ....??
        else
        {
            return null;
        }
    }
}
