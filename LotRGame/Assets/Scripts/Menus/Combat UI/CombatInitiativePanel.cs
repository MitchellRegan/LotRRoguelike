using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatInitiativePanel : MonoBehaviour
{
    //The text box that holds this character's name
    public Text nameText;
    //The slider that shows this character's current health
    public Slider healthSlider;
    //The slider that shows this character's initiative
    public Slider initiativeSlider;
    //The background image that changes color
    public Image backgroundImage;



    //Function called externally to set our background image color
    public void SetBackgroundColor(Color new_color_)
    {
        this.backgroundImage.color = new_color_;
    }
}
