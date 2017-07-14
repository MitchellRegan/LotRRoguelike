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
                this.ourSlider.value = 0;
                break;
            case CombatManager.GroupCombatDistance.Medium:
                this.ourSlider.value = 1;
                break;
            case CombatManager.GroupCombatDistance.Far:
                this.ourSlider.value = 2;
                break;
        }
    }


    //Function called externally using a slider to set the selected party group's preferred combat distance
	public void SetGroupCombatDistance(int distance_)
    {
        //Setting the distance based on the int given
        switch(distance_)
        {
            case 2:
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
