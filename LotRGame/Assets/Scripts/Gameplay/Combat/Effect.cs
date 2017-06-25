using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    //The name of this effect
    public string effectName;

    //Bool that determines if this effect happens to the character that created it
    public bool effectUser = false;
    //Bool that determines if this effect happens to the character that it's used on
    public bool effectTarget = true;



	//Function called externally to trigger this effect
    public virtual void TriggerEffect(Character usingCharacter_, Character targetCharacter_)
    {

    }
}
