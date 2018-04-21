using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAction : AttackAction
{
    //Enum for the hand that this weapon attack requires
    public enum WeaponHand
    {
        MainHand,//Required weapon needs to be in the main hand
        OffHand,//Required weapon needs to be in the off hand
        OneHand,//Required weapon can be in the main OR off hand as long as it's 1-handed
        TwoHand//Required weapon needs to be 2-handed
    };
    public WeaponHand requiredWeaponHand = WeaponHand.OneHand;



	//Function called from CombatActionPanelUI.cs to check if this action can be used
    public bool CanCharacterUseAction(Character charToCheck_)
    {
        //Bool to return
        bool canUse = false;

        //switch statement so we can check the character based on our weapon hand type
        switch(this.requiredWeaponHand)
        {
            case WeaponHand.MainHand:
                //Checking the character's main hand weapon to see if it matches our weapon skill type
                if(charToCheck_.charInventory.rightHand != null)
                {
                    if(charToCheck_.charInventory.rightHand.weaponType == this.weaponSkillUsed)
                    {
                        canUse = true;
                    }
                }
                //If the main hand is empty and this action is for unarmed skills, they can use it
                else if(this.weaponSkillUsed == SkillList.Unarmed)
                {
                    canUse = true;
                }
                break;

            case WeaponHand.OffHand:
                //Checking the character's off hand weapon to see if it matches our weapon skill type
                if (charToCheck_.charInventory.leftHand != null)
                {
                    if (charToCheck_.charInventory.leftHand.weaponType == this.weaponSkillUsed)
                    {
                        canUse = true;
                    }
                }
                //If the off hand is empty and this action is for unarmed skills
                else if(this.weaponSkillUsed == SkillList.Unarmed)
                {
                    //Making sure the main hand weapon isn't holding a 2-handed weapon
                    if(charToCheck_.charInventory.rightHand == null || 
                        charToCheck_.charInventory.rightHand.size == Weapon.WeaponSize.OneHand)
                    {
                        canUse = true;
                    }
                }
                break;

            case WeaponHand.OneHand:
                //Checking the character's main hand weapon to see if it matches our weapon skill type and size
                if (charToCheck_.charInventory.rightHand != null && charToCheck_.charInventory.rightHand.size == Weapon.WeaponSize.OneHand)
                {
                    if (charToCheck_.charInventory.rightHand.weaponType == this.weaponSkillUsed)
                    {
                        canUse = true;
                        //As long as the main hand weapon works, we don't need to check the off hand
                        break;
                    }
                }
                //If the main hand is empty and this action is for unarmed skills, they can use it
                else if (this.weaponSkillUsed == SkillList.Unarmed)
                {
                    canUse = true;
                    break;
                }
                //Checking the character's off hand weapon to see if it matches our weapon skill type
                if (charToCheck_.charInventory.leftHand != null)
                {
                    if (charToCheck_.charInventory.leftHand.weaponType == this.weaponSkillUsed)
                    {
                        canUse = true;
                    }
                }
                //If the off hand is empty and this action is for unarmed skills
                else if (this.weaponSkillUsed == SkillList.Unarmed)
                {
                    //Making sure the main hand weapon isn't holding a 2-handed weapon
                    if (charToCheck_.charInventory.rightHand == null ||
                        charToCheck_.charInventory.rightHand.size == Weapon.WeaponSize.OneHand)
                    {
                        canUse = true;
                    }
                }
                break;

            case WeaponHand.TwoHand:
                //Checking the character's main hand weapon to see if it matches our weapon skill type and size
                if (charToCheck_.charInventory.rightHand != null && charToCheck_.charInventory.rightHand.size == Weapon.WeaponSize.TwoHands)
                {
                    if (charToCheck_.charInventory.rightHand.weaponType == this.weaponSkillUsed)
                    {
                        canUse = true;
                    }
                }
                else if(this.weaponSkillUsed == SkillList.Unarmed)
                {
                    //Making sure both hands aren't holding anything
                    if(charToCheck_.charInventory.rightHand == null && charToCheck_.charInventory.leftHand == null)
                    {
                        canUse = true;
                    }
                }
                break;
        }

        return canUse;
    }


    //Function inherited from AttackAction.cs and called from CombatManager.cs so we can attack a target
    public override void PerformAction(CombatTile targetTile_)
    {
        base.PerformAction(targetTile_);
    }
}
