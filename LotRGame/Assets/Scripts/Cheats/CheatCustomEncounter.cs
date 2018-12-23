using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheatCustomEncounter : MonoBehaviour
{
    //The key that's pressed to spawn the selected encounter
    public KeyCode spawnEncounterButton = KeyCode.KeypadEnter;
    //The key that's pressed to cycle encounters forward
    public KeyCode cycleForwardButton = KeyCode.KeypadPlus;
    //The key that's pressed to cycle encounters backward
    public KeyCode cycleBackwardButton = KeyCode.KeypadMinus;

    [Space(8)]

    //The current index for the selected encounter
    private int selectedIndex = 0;
    
    //The list of encounters we can spawn
    public List<EnemyEncounter> encounterList;



    //Function called every frame
    private void Update()
    {
        //If the button to spawn encounters is pressed
        if(Input.GetKeyDown(this.spawnEncounterButton))
        {
            this.SpawnSelectedEncounter();
        }
        //If the button to cycle forward is pressed
        else if(Input.GetKeyDown(this.cycleForwardButton))
        {
            this.CycleEncounterForward();
        }
        //If the button to cycle backward is pressed
        else if(Input.GetKeyDown(this.cycleBackwardButton))
        {
            this.CycleEncounterBackward();
        }
    }


	//Function called from Update to cycle the index forward
    private void CycleEncounterForward()
    {
        //If the encounter list is empty, nothing happens
        if (this.encounterList.Count < 1)
        {
            Debug.Log("CHEAT: Custom Encounter, Cycle Forward: ERROR encounter list is empty");
            return;
        }

        //Increasing the selected encounter index by 1
        this.selectedIndex += 1;

        //If the index is higher than our encounter list length, we cycle back to 0
        if(this.selectedIndex >= this.encounterList.Count)
        {
            this.selectedIndex = 0;
        }

        Debug.Log("CHEAT: Custom Encounter, Cycle Forward: Selected Encounter is " + this.encounterList[this.selectedIndex].name);
    }


    //Function called from Update to cycle the index backward
    private void CycleEncounterBackward()
    {
        //If the encounter list is empty, nothing happens
        if(this.encounterList.Count < 1)
        {
            Debug.Log("CHEAT: Custom Encounter, Cycle Backward: ERROR encounter list is empty");
            return;
        }

        //Decreasing the selected encounter index by 1
        this.selectedIndex -= 1;

        //If the index is lower than 0, we cycle back to the last index in the encounter list
        if(this.selectedIndex < 0)
        {
            this.selectedIndex = this.encounterList.Count - 1;
        }

        Debug.Log("CHEAT: Custom Encounter, Cycle Backward: Selected Encounter is " + this.encounterList[this.selectedIndex].name);
    }


    //Function called from Update to spawn the selected encounter
    private void SpawnSelectedEncounter()
    {
        //If the encounter list is empty, nothing happens
        if (this.encounterList.Count < 1)
        {
            Debug.Log("CHEAT: Custom Encounter, Spawn Encounter: ERROR encounter list is empty");
            return;
        }

        //If we're not in the gameplay scene, nothing happens
        if(SceneManager.GetActiveScene().buildIndex != 1)
        {
            Debug.Log("CHEAT: Custom Encounter, Spawn Encounter: ERROR not in gameplay scene");
            return;
        }

        //If we're in combat, nothing happens
        if(CombatManager.globalReference.GetComponent<Canvas>().enabled)
        {
            Debug.Log("CHEAT: Custom Encounter, Spawn Encounter: ERROR cannot spawn encounter while in combat");
            return;
        }

        //Otherwise we spawn the selected encounter on the player's overworld location
        TileInfo partyTile = PartyGroup.group1.GetComponent<WASDOverworldMovement>().currentTile;
        GameObject spawnedEncounter = GameObject.Instantiate(this.encounterList[this.selectedIndex].gameObject) as GameObject;

        partyTile.AddObjectToThisTile(spawnedEncounter, false);

        Debug.Log("CHEAT: Custom Encounter, Spawn Encounter: Spawning Encounter " + this.encounterList[this.selectedIndex].name);
    }
}
