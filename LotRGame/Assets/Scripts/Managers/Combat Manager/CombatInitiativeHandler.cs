using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatInitiativeHandler : MonoBehaviour
{
    //Reference to the characters whose turn it is to act. It's a list because multiple characters could have the same initiative
    [HideInInspector]
    public List<Character> actingCharacters = null;



    //Function called from CombatManager.InitiateCombat to reset our actingCharacters array at the start of combat
    public void ResetForCombatStart()
    {
        this.actingCharacters = new List<Character>();
    }


    //Function called from CombatManager.Update to increase the initiative of all characters
    public void IncreaseInitiatives()
    {
        //Looping through each player character
        for (int p = 0; p < CombatManager.globalReference.characterHandler.playerCharacters.Count; ++p)
        {
            //Making sure the current character isn't dead first
            if (CombatManager.globalReference.characterHandler.playerCharacters[p].charPhysState.currentHealth > 0)
            {
                //Reducing the character's action cooldown times
                CombatManager.globalReference.characterHandler.playerCharacters[p].charActionList.ReduceCooldowns(Time.deltaTime);

                //Looping through the character's perk list to see if they have any InitiativeBoostPerks
                float perkBoost = 0;
                foreach (Perk charPerk in CombatManager.globalReference.characterHandler.playerCharacters[p].charPerks.allPerks)
                {
                    if (charPerk.GetType() == typeof(InitiativeBoostPerk))
                    {
                        perkBoost += charPerk.GetComponent<InitiativeBoostPerk>().initiativeSpeedBoost;
                    }
                }

                //Adding this character's initiative to the coorelating slider. The initiative is multiplied by the energy %
                CombatStats combatStats = CombatManager.globalReference.characterHandler.playerCharacters[p].charCombatStats;
                float initiativeToAdd = (combatStats.currentInitiativeSpeed + combatStats.initiativeMod + perkBoost);

                //If the character's initiative is lower than 10% of their base initiative, we set it to 10%
                if (initiativeToAdd < combatStats.currentInitiativeSpeed * 0.1f)
                {
                    initiativeToAdd = combatStats.currentInitiativeSpeed * 0.1f;
                }
                
                //If the slider is filled, this character is added to the acting character list
                if (CombatManager.globalReference.uiHandler.UpdateInitiativeSlider(true, p, initiativeToAdd))
                {
                    this.actingCharacters.Add(CombatManager.globalReference.characterHandler.playerCharacters[p]);
                }
            }
        }


        //Looping through each enemy character
        for (int e = 0; e < CombatManager.globalReference.characterHandler.enemyCharacters.Count; ++e)
        {
            //Making sure the current enemy isn't dead first
            if (CombatManager.globalReference.characterHandler.enemyCharacters[e].charPhysState.currentHealth > 0)
            {
                //Reducing the character's action cooldown times
                CombatManager.globalReference.characterHandler.enemyCharacters[e].charActionList.ReduceCooldowns(Time.deltaTime);

                //Looping through the character's perk list to see if they have any InitiativeBoostPerks
                float perkBoost = 0;
                foreach (Perk enemyPerk in CombatManager.globalReference.characterHandler.enemyCharacters[e].charPerks.allPerks)
                {
                    if (enemyPerk.GetType() == typeof(InitiativeBoostPerk))
                    {
                        perkBoost += enemyPerk.GetComponent<InitiativeBoostPerk>().initiativeSpeedBoost;
                    }
                }

                //Adding this enemy's initiative to the coorelating slider. The initiative is multiplied by the energy %
                CombatStats combatStats = CombatManager.globalReference.characterHandler.enemyCharacters[e].charCombatStats;
                float initiativeToAdd = (combatStats.currentInitiativeSpeed + combatStats.initiativeMod + perkBoost);

                //If the enemy's initiative is lower than 10% of their base initiative, we set it to 10%
                if (initiativeToAdd < combatStats.currentInitiativeSpeed * 0.1f)
                {
                    initiativeToAdd = combatStats.currentInitiativeSpeed * 0.1f;
                }

                //If the slider is filled, this character is added to the acting character list
                if (CombatManager.globalReference.uiHandler.UpdateInitiativeSlider(false, e, initiativeToAdd))
                {
                    //Making sure this character isn't already in the list of acting characters
                    if (!this.actingCharacters.Contains(CombatManager.globalReference.characterHandler.enemyCharacters[e]))
                    {
                        this.actingCharacters.Add(CombatManager.globalReference.characterHandler.enemyCharacters[e]);
                    }
                }
            }
        }

        //If there are any characters in the acting Characters list, the state changes so we stop updating initiative meters
        if (this.actingCharacters.Count != 0)
        {
            CombatManager.globalReference.SetWaitTime(1, CombatState.SelectAction);
            CombatManager.globalReference.uiHandler.HilightActingCharacter();
        }
    }
}
