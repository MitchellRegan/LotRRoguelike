using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class CombatPartyGroupDistance : MonoBehaviour
{
    //Reference to the slider component on this object
    private Slider ourSlider;



    //Function called when this object is created
    private void Awake()
    {
        //Getting the reference to the slider on this object
        this.ourSlider = this.GetComponent<Slider>();
    }



    //Function called when this component is enabled
    private void OnEnable()
    {
        this.RefreshSlider();
    }


    //Function called externally and from OnEnable to make the slider display the current party group's distance
    public void RefreshSlider()
    {
        //Setting our slider's value based on which party group is selected
        switch(CharacterManager.globalReference.selectedGroup.combatDistance)
        {
            case CombatManager.GroupCombatDistance.Close:
                this.ourSlider.value = 2;
                break;
            case CombatManager.GroupCombatDistance.Medium:
                this.ourSlider.value = 1;
                break;
            case CombatManager.GroupCombatDistance.Far:
                this.ourSlider.value = 0;
                break;
        }
    }


    //Function called every frame
	private void Update()
    {
        //Getting the current value for our slider
        int distance = Mathf.RoundToInt(this.ourSlider.value);

        //Setting the distance based on the int given
        switch(distance)
        {
            case 0:
                CharacterManager.globalReference.selectedGroup.combatDistance = CombatManager.GroupCombatDistance.Far;
                break;
            case 1:
                CharacterManager.globalReference.selectedGroup.combatDistance = CombatManager.GroupCombatDistance.Medium;
                break;
            default:
                CharacterManager.globalReference.selectedGroup.combatDistance = CombatManager.GroupCombatDistance.Close;
                break;
        }
    }
}
