using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AssignArmorSpriteTool : EditorWindow
{
    //The sprite references that are set
    public Armor helm;
    public Armor chestpiece;
    public Armor leggings;
    public Armor shoes;
    public Armor gloves;
    public Armor cloak;

    //The base sprite that's split and used by all of the armor references
    public Sprite baseSprite;

    //String for the text on the button that's pressed to assign all sprites
    private string assignSprites = "Assign Sprites";
    //String for the error text if there's no base sprite given
    private string errorText = "";



    [MenuItem("Window/Assign Armor Sprite Tool")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(AssignArmorSpriteTool));
    }


    //Function called every frame that this editor window is up
	private void OnGUI()
    {
        //Creating object fields so that we can drag-and-drop object references for each piece of armor
        this.helm = (Armor)EditorGUI.ObjectField(new Rect(3, 23, position.width - 6, 20), "Helm", this.helm, typeof(Armor));
        this.chestpiece = (Armor)EditorGUI.ObjectField(new Rect(3, 43, position.width - 6, 20), "Chestpiece", this.chestpiece, typeof(Armor));
        this.leggings = (Armor)EditorGUI.ObjectField(new Rect(3, 63, position.width - 6, 20), "Leggings", this.leggings, typeof(Armor));
        this.shoes = (Armor)EditorGUI.ObjectField(new Rect(3, 83, position.width - 6, 20), "Shoes", this.shoes, typeof(Armor));
        this.gloves = (Armor)EditorGUI.ObjectField(new Rect(3, 103, position.width - 6, 20), "Gloves", this.gloves, typeof(Armor));
        this.cloak = (Armor)EditorGUI.ObjectField(new Rect(3, 123, position.width - 6, 20), "Cloak", this.cloak, typeof(Armor));

        //Creating an object field so that we can drag-and-drop an object reference for the base sprite
        this.baseSprite = (Sprite)EditorGUI.ObjectField(new Rect(3, 153, position.width - 6, 20), "Base Sprite", this.baseSprite, typeof(Sprite));

        //Creating a UI button so that we can assign all of the sprites to the armor references with just a click
        if (GUILayout.Button(this.assignSprites))
        {
            //If we have a base sprite to use
            if (this.baseSprite != null)
            {
                //Clearing the error text
                this.errorText = EditorGUI.TextArea(new Rect(3, 173, position.width - 6, 20), "");

                //Calling all of our functions to set armor sprites
                this.AssignHelmSprites();
                this.AssignChestpieceSprites();
                this.AssignLeggingSprites();
                this.AssignShoeSprites();
                this.AssignGloveSprites();
                this.AssignCloakSprites();

                //Clearing all of the references once we're done with them
                this.helm = null;
                this.chestpiece = null;
                this.leggings = null;
                this.shoes = null;
                this.gloves = null;
                this.cloak = null;
            }
            //If we don't have a base sprite, we output a warning
            else
            {
                Debug.Log("Moooo");
                this.errorText = EditorGUI.TextArea(new Rect(3, 173, position.width - 6, 20), "WARNING! You need to assign the Base Sprite!");
            }
        }
    }


    //Function called from OnGUI to assign helm sprites
    private void AssignHelmSprites()
    {
        //If we don't have a helm armor item, nothing happens
        if(helm == null || helm.slot != Armor.ArmorSlot.Head)
        {
            Debug.Log("Incorrect helm!");
            return;
        }

        Debug.Log("Correct Helm! Good Job, Mitch");
    }


    //Function called from OnGUI to assign chestpiece sprites
    private void AssignChestpieceSprites()
    {
        //If we don't have a chest armor item, nothing happens
        if (chestpiece == null || chestpiece.slot != Armor.ArmorSlot.Torso)
        {
            return;
        }
    }


    //Function called from OnGUI to assign legging sprites
    private void AssignLeggingSprites()
    {
        //If we don't have a leg armor item, nothing happens
        if (leggings == null || leggings.slot != Armor.ArmorSlot.Legs)
        {
            return;
        }
    }


    //Function called from OnGUI to assign shoe sprites
    private void AssignShoeSprites()
    {
        //If we don't have a shoe armor item, nothing happens
        if (shoes == null || shoes.slot != Armor.ArmorSlot.Feet)
        {
            return;
        }
    }


    //Function called from OnGUI to assign glove sprites
    private void AssignGloveSprites()
    {
        //If we don't have a glove armor item, nothing happens
        if (gloves == null || gloves.slot != Armor.ArmorSlot.Hands)
        {
            return;
        }
    }


    //Function called from OnGUI to assign cloak sprites
    private void AssignCloakSprites()
    {
        //If we don't have a cloak armor item, nothing happens
        if (cloak == null || cloak.slot != Armor.ArmorSlot.Cloak)
        {
            return;
        }
    }
}
