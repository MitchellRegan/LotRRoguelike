using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

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

    //The base sprite for the item icons
    public Sprite baseIcons;

    //String for the text on the button that's pressed to assign all sprites
    private string assignSprites = "Assign Sprites";
    //Bool for if the sprite and armor references are cleared after assigning
    private bool clearReferences = true;



    //Function called to get this editor window to appear as an option in the "Windows" options
    [MenuItem("Window/Assign Armor Sprite Tool")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(AssignArmorSpriteTool));
    }


    //Function called every frame that this editor window is up
	private void OnGUI()
    {
        //Toggle to determine if our armor references are cleared once we assign the sprites
        this.clearReferences = EditorGUI.Toggle(new Rect(3, 23, position.width + 20, 20), "Clear After Assigning", this.clearReferences);

        //Creating object fields so that we can drag-and-drop object references for each piece of armor
        this.helm = (Armor)EditorGUI.ObjectField(new Rect(3, 43, position.width - 6, 20), "Helm", this.helm, typeof(Armor));
        this.chestpiece = (Armor)EditorGUI.ObjectField(new Rect(3, 63, position.width - 6, 20), "Chestpiece", this.chestpiece, typeof(Armor));
        this.leggings = (Armor)EditorGUI.ObjectField(new Rect(3, 83, position.width - 6, 20), "Leggings", this.leggings, typeof(Armor));
        this.shoes = (Armor)EditorGUI.ObjectField(new Rect(3, 103, position.width - 6, 20), "Shoes", this.shoes, typeof(Armor));
        this.gloves = (Armor)EditorGUI.ObjectField(new Rect(3, 123, position.width - 6, 20), "Gloves", this.gloves, typeof(Armor));
        this.cloak = (Armor)EditorGUI.ObjectField(new Rect(3, 143, position.width - 6, 20), "Cloak", this.cloak, typeof(Armor));

        //Creating an object field so that we can drag-and-drop an object reference for the base sprite
        this.baseSprite = (Sprite)EditorGUI.ObjectField(new Rect(3, 173, position.width - 100, 60), "Base Sprite", this.baseSprite, typeof(Sprite));

        //Creating an object field so that we can drag-and-drop an object reference for the icon sprite
        this.baseIcons = (Sprite)EditorGUI.ObjectField(new Rect(3, 243, position.width - 100, 60), "Icon Sprite", this.baseIcons, typeof(Sprite));

        //Creating a UI button so that we can assign all of the sprites to the armor references with just a click
        if (GUILayout.Button(this.assignSprites))
        {
            //If we have a base sprite to use
            if (this.baseSprite != null)
            {
                //Getting the string for the prefix that our sprites will use
                string prefix = this.baseSprite.name;
                while (prefix[prefix.Length - 1] != '_')
                {
                    prefix = prefix.Remove(prefix.Length - 1, 1);
                }

                //Calling the function to set the armor icons
                this.SetArmorIcons();

                //Calling all of our functions to set armor sprites
                this.AssignHelmSprites(prefix);
                this.AssignChestpieceSprites(prefix);
                this.AssignLeggingSprites(prefix);
                this.AssignShoeSprites(prefix);
                this.AssignGloveSprites(prefix);
                this.AssignCloakSprites(prefix);

                //Clearing all of the references once we're done with them (if we should)
                if (this.clearReferences)
                {
                    this.helm = null;
                    this.chestpiece = null;
                    this.leggings = null;
                    this.shoes = null;
                    this.gloves = null;
                    this.cloak = null;
                    this.baseSprite = null;
                    this.baseIcons = null;
                }
            }
            //If we don't have a base sprite, we output a warning
            else
            {
                Debug.LogWarning("You need to have a base sprite to assign armor sprites!");
            }
        }
    }


    //Function called from OnGUI to assign helm sprites
    private void AssignHelmSprites(string spritePrefix_)
    {
        //If we don't have a helm armor item, nothing happens
        if(helm == null || helm.slot != Armor.ArmorSlot.Head)
        {
            return;
        }
        
        //Creating a new list of all of the sprite views
        this.helm.armorSpriteViews = new List<SpriteViews>();

        //Assigning the helm sprites
        SpriteViews helm0 = this.GetAllSpriteViews(spritePrefix_, 109, 65, 87, BodyTypes.IgnoreBodyType);
        this.helm.armorSpriteViews.Add(helm0);

        //Setting this helm object to dirty so that it saves the changes
        Undo.RecordObject(this.helm, "Set Helm Sprites");
        EditorUtility.SetDirty(this.helm);
    }


    //Function called from OnGUI to assign chestpiece sprites
    private void AssignChestpieceSprites(string spritePrefix_)
    {
        //If we don't have a chest armor item, nothing happens
        if (chestpiece == null || chestpiece.slot != Armor.ArmorSlot.Torso)
        {
            return;
        }

        //Creating a new list of all of the sprite views
        this.chestpiece.armorSpriteViews = new List<SpriteViews>();

        //Assigning the chest sprites
        SpriteViews chest0 = this.GetAllSpriteViews(spritePrefix_, 22, 0, 44, BodyTypes.TorsoSkinnyShortMale);
        this.chestpiece.armorSpriteViews.Add(chest0);
        
        SpriteViews chest1 = this.GetAllSpriteViews(spritePrefix_, 23, 1, 45, BodyTypes.TorsoBuffShortMale);
        this.chestpiece.armorSpriteViews.Add(chest1);
        
        SpriteViews chest2 = this.GetAllSpriteViews(spritePrefix_, 24, 2, 46, BodyTypes.TorsoFatShortMale);
        this.chestpiece.armorSpriteViews.Add(chest2);
        
        SpriteViews chest3 = this.GetAllSpriteViews(spritePrefix_, 25, 3, 47, BodyTypes.TorsoObeseShortMale);
        this.chestpiece.armorSpriteViews.Add(chest3);
        
        SpriteViews chest4 = this.GetAllSpriteViews(spritePrefix_, 26, 4, 48, BodyTypes.TorsoSkinnyShortFemale);
        this.chestpiece.armorSpriteViews.Add(chest4);

        SpriteViews chest5 = this.GetAllSpriteViews(spritePrefix_, 27, 5, 49, BodyTypes.TorsoBuffShortFemale);
        this.chestpiece.armorSpriteViews.Add(chest5);

        SpriteViews chest6 = this.GetAllSpriteViews(spritePrefix_, 28, 6, 50, BodyTypes.TorsoFatShortFemale);
        this.chestpiece.armorSpriteViews.Add(chest6);

        SpriteViews chest7 = this.GetAllSpriteViews(spritePrefix_, 29, 7, 51, BodyTypes.TorsoObeseShortFemale);
        this.chestpiece.armorSpriteViews.Add(chest7);

        SpriteViews chest8 = this.GetAllSpriteViews(spritePrefix_, 88, 66, 110, BodyTypes.TorsoSkinnyTallMale);
        this.chestpiece.armorSpriteViews.Add(chest8);

        SpriteViews chest9 = this.GetAllSpriteViews(spritePrefix_, 89, 67, 111, BodyTypes.TorsoBuffTallMale);
        this.chestpiece.armorSpriteViews.Add(chest9);

        SpriteViews chest10 = this.GetAllSpriteViews(spritePrefix_, 90, 68, 112, BodyTypes.TorsoFatTallMale);
        this.chestpiece.armorSpriteViews.Add(chest10);

        SpriteViews chest11 = this.GetAllSpriteViews(spritePrefix_, 91, 69, 113, BodyTypes.TorsoObeseTallMale);
        this.chestpiece.armorSpriteViews.Add(chest11);

        SpriteViews chest12 = this.GetAllSpriteViews(spritePrefix_, 92, 70, 114, BodyTypes.TorsoSkinnyTallFemale);
        this.chestpiece.armorSpriteViews.Add(chest12);

        SpriteViews chest13 = this.GetAllSpriteViews(spritePrefix_, 93, 71, 115, BodyTypes.TorsoBuffTallFemale);
        this.chestpiece.armorSpriteViews.Add(chest13);

        SpriteViews chest14 = this.GetAllSpriteViews(spritePrefix_, 94, 72, 116, BodyTypes.TorsoFatTallFemale);
        this.chestpiece.armorSpriteViews.Add(chest14);

        SpriteViews chest15 = this.GetAllSpriteViews(spritePrefix_, 95, 73, 117, BodyTypes.TorsoObeseTallFemale);
        this.chestpiece.armorSpriteViews.Add(chest15);

        //Setting this chestpiece object to dirty so that it saves the changes
        Undo.RecordObject(this.chestpiece, "Set Chestpiece Sprites");
        EditorUtility.SetDirty(this.chestpiece);
    }


    //Function called from OnGUI to assign legging sprites
    private void AssignLeggingSprites(string spritePrefix_)
    {
        //If we don't have a leg armor item, nothing happens
        if (leggings == null || leggings.slot != Armor.ArmorSlot.Legs)
        {
            return;
        }

        //Creating a new list of all of the sprite views
        this.leggings.armorSpriteViews = new List<SpriteViews>();

        //Going through each of the leg sprites and assigning the leggings for them
        SpriteViews legs0 = this.GetAllSpriteViews(spritePrefix_, 8, 74, 8, BodyTypes.LegsSkinny1);
        this.leggings.armorSpriteViews.Add(legs0);

        SpriteViews legs1 = this.GetAllSpriteViews(spritePrefix_, 9, 75, 9, BodyTypes.LegsSkinny2);
        this.leggings.armorSpriteViews.Add(legs1);

        SpriteViews legs2 = this.GetAllSpriteViews(spritePrefix_, 10, 76, 10, BodyTypes.LegsSkinny3);
        this.leggings.armorSpriteViews.Add(legs2);

        SpriteViews legs3 = this.GetAllSpriteViews(spritePrefix_, 11, 77, 11, BodyTypes.LegsSkinny4);
        this.leggings.armorSpriteViews.Add(legs3);

        SpriteViews legs4 = this.GetAllSpriteViews(spritePrefix_, 12, 78, 12, BodyTypes.LegsSkinny5);
        this.leggings.armorSpriteViews.Add(legs4);

        SpriteViews legs5 = this.GetAllSpriteViews(spritePrefix_, 13, 79, 13, BodyTypes.LegsSkinny6);
        this.leggings.armorSpriteViews.Add(legs5);

        SpriteViews legs6 = this.GetAllSpriteViews(spritePrefix_, 30, 96, 30, BodyTypes.LegsNormal1);
        this.leggings.armorSpriteViews.Add(legs6);

        SpriteViews legs7 = this.GetAllSpriteViews(spritePrefix_, 31, 97, 31, BodyTypes.LegsNormal2);
        this.leggings.armorSpriteViews.Add(legs7);

        SpriteViews legs8 = this.GetAllSpriteViews(spritePrefix_, 32, 98, 32, BodyTypes.LegsNormal3);
        this.leggings.armorSpriteViews.Add(legs8);

        SpriteViews legs9 = this.GetAllSpriteViews(spritePrefix_, 33, 99, 33, BodyTypes.LegsNormal4);
        this.leggings.armorSpriteViews.Add(legs9);

        SpriteViews legs10 = this.GetAllSpriteViews(spritePrefix_, 34, 100, 34, BodyTypes.LegsNormal5);
        this.leggings.armorSpriteViews.Add(legs10);

        SpriteViews legs11 = this.GetAllSpriteViews(spritePrefix_, 34, 101, 35, BodyTypes.LegsNormal6);
        this.leggings.armorSpriteViews.Add(legs11);

        SpriteViews legs12 = this.GetAllSpriteViews(spritePrefix_, 52, 118, 52, BodyTypes.LegsFat1);
        this.leggings.armorSpriteViews.Add(legs12);

        SpriteViews legs13 = this.GetAllSpriteViews(spritePrefix_, 53, 119, 53, BodyTypes.LegsFat2);
        this.leggings.armorSpriteViews.Add(legs13);

        SpriteViews legs14 = this.GetAllSpriteViews(spritePrefix_, 54, 120, 54, BodyTypes.LegsFat3);
        this.leggings.armorSpriteViews.Add(legs14);

        SpriteViews legs15 = this.GetAllSpriteViews(spritePrefix_, 55, 121, 55, BodyTypes.LegsFat4);
        this.leggings.armorSpriteViews.Add(legs15);

        SpriteViews legs16 = this.GetAllSpriteViews(spritePrefix_, 56, 122, 56, BodyTypes.LegsFat5);
        this.leggings.armorSpriteViews.Add(legs16);

        SpriteViews legs17 = this.GetAllSpriteViews(spritePrefix_, 57, 123, 57, BodyTypes.LegsFat6);
        this.leggings.armorSpriteViews.Add(legs17);

        //Setting this legging object to dirty so that it saves the changes
        Undo.RecordObject(this.leggings, "Set Legging Sprites");
        EditorUtility.SetDirty(this.leggings);
    }


    //Function called from OnGUI to assign shoe sprites
    private void AssignShoeSprites(string spritePrefix_)
    {
        //If we don't have a shoe armor item, nothing happens
        if (this.shoes == null || this.shoes.slot != Armor.ArmorSlot.Feet)
        {
            return;
        }

        //Creating a new list of all of the sprite views
        this.shoes.armorSpriteViews = new List<SpriteViews>();

        //Going through each type of leg sprite and assigning the shoe for it
        SpriteViews shoe0 = this.GetAllSpriteViews(spritePrefix_, 14, 80, 14, BodyTypes.LegsSkinny1);
        this.shoes.armorSpriteViews.Add(shoe0);

        SpriteViews shoe1 = this.GetAllSpriteViews(spritePrefix_, 14, 80, 14, BodyTypes.LegsSkinny2);
        this.shoes.armorSpriteViews.Add(shoe1);

        SpriteViews shoe2 = this.GetAllSpriteViews(spritePrefix_, 14, 80, 14, BodyTypes.LegsSkinny3);
        this.shoes.armorSpriteViews.Add(shoe2);

        SpriteViews shoe3 = this.GetAllSpriteViews(spritePrefix_, 17, 80, 17, BodyTypes.LegsSkinny4);
        this.shoes.armorSpriteViews.Add(shoe3);

        SpriteViews shoe4 = this.GetAllSpriteViews(spritePrefix_, 17, 80, 17, BodyTypes.LegsSkinny5);
        this.shoes.armorSpriteViews.Add(shoe4);

        SpriteViews shoe5 = this.GetAllSpriteViews(spritePrefix_, 17, 80, 17, BodyTypes.LegsSkinny6);
        this.shoes.armorSpriteViews.Add(shoe5);

        SpriteViews shoe6 = this.GetAllSpriteViews(spritePrefix_, 14, 102, 14, BodyTypes.LegsNormal1);
        this.shoes.armorSpriteViews.Add(shoe6);

        SpriteViews shoe7 = this.GetAllSpriteViews(spritePrefix_, 14, 102, 14, BodyTypes.LegsNormal2);
        this.shoes.armorSpriteViews.Add(shoe7);

        SpriteViews shoe8 = this.GetAllSpriteViews(spritePrefix_, 14, 102, 14, BodyTypes.LegsNormal3);
        this.shoes.armorSpriteViews.Add(shoe8);

        SpriteViews shoe9 = this.GetAllSpriteViews(spritePrefix_, 17, 102, 17, BodyTypes.LegsNormal4);
        this.shoes.armorSpriteViews.Add(shoe9);

        SpriteViews shoe10 = this.GetAllSpriteViews(spritePrefix_, 17, 102, 17, BodyTypes.LegsNormal5);
        this.shoes.armorSpriteViews.Add(shoe10);

        SpriteViews shoe11 = this.GetAllSpriteViews(spritePrefix_, 17, 102, 17, BodyTypes.LegsNormal6);
        this.shoes.armorSpriteViews.Add(shoe11);

        SpriteViews shoe12 = this.GetAllSpriteViews(spritePrefix_, 17, 124, 17, BodyTypes.LegsFat1);
        this.shoes.armorSpriteViews.Add(shoe12);

        SpriteViews shoe13 = this.GetAllSpriteViews(spritePrefix_, 17, 124, 17, BodyTypes.LegsFat2);
        this.shoes.armorSpriteViews.Add(shoe13);

        SpriteViews shoe14 = this.GetAllSpriteViews(spritePrefix_, 17, 124, 17, BodyTypes.LegsFat3);
        this.shoes.armorSpriteViews.Add(shoe14);

        SpriteViews shoe15 = this.GetAllSpriteViews(spritePrefix_, 17, 127, 17, BodyTypes.LegsFat4);
        this.shoes.armorSpriteViews.Add(shoe15);

        SpriteViews shoe16 = this.GetAllSpriteViews(spritePrefix_, 17, 127, 17, BodyTypes.LegsFat5);
        this.shoes.armorSpriteViews.Add(shoe16);

        SpriteViews shoe17 = this.GetAllSpriteViews(spritePrefix_, 17, 127, 17, BodyTypes.LegsFat6);
        this.shoes.armorSpriteViews.Add(shoe17);

        //Setting this shoe object to dirty so that it saves the changes
        Undo.RecordObject(this.shoes, "Set Shoe Sprites");
        EditorUtility.SetDirty(this.shoes);
    }


    //Function called from OnGUI to assign glove sprites
    private void AssignGloveSprites(string spritePrefix_)
    {
        //If we don't have a glove armor item, nothing happens
        if (this.gloves == null || this.gloves.slot != Armor.ArmorSlot.Hands)
        {
            return;
        }

        //Creating a new list of all of the sprite views
        this.gloves.armorSpriteViews = new List<SpriteViews>();

        //Assigning the glove sprites
        SpriteViews glove0 = this.GetAllSpriteViews(spritePrefix_, 43, 21, 43, BodyTypes.IgnoreBodyType);
        this.gloves.armorSpriteViews.Add(glove0);

        //Setting this glove object to dirty so that it saves the changes
        Undo.RecordObject(this.gloves, "Set Glove Sprites");
        EditorUtility.SetDirty(this.gloves);
    }


    //Function called from OnGUI to assign cloak sprites
    private void AssignCloakSprites(string spritePrefix_)
    {
        //If we don't have a cloak armor item, nothing happens
        if (this.cloak == null || this.cloak.slot != Armor.ArmorSlot.Cloak)
        {
            return;
        }

        //Creating a new list of all of the sprite views
        this.cloak.armorSpriteViews = new List<SpriteViews>();

        //Assigning the cloak sprites
        SpriteViews cloak0 = this.GetAllSpriteViews(spritePrefix_, 86, 130, 108, BodyTypes.IgnoreBodyType);
        this.cloak.armorSpriteViews.Add(cloak0);

        //Setting this cloak object to dirty so that it saves the changes
        Undo.RecordObject(this.cloak, "Set Cloak Sprites");
        EditorUtility.SetDirty(this.cloak);
    }


    //Function called from OnGUI to assign all armor icons
    private void SetArmorIcons()
    {
        //If we don't have an icon sprite, nothing happens
        if(this.baseIcons == null)
        {
            return;
        }

        //Getting the asset path string to the sprite for the front view
        string[] frontViewSpriteGUID = AssetDatabase.FindAssets(this.baseIcons.name);
        string spritePath = AssetDatabase.GUIDToAssetPath(frontViewSpriteGUID[0]);

        //Getting the array of all sprites in the base sprite's multi-sprite sheet
        Sprite[] allMultiSprites = AssetDatabase.LoadAllAssetsAtPath(spritePath).OfType<Sprite>().ToArray();

        //If we have a helm
        if (this.helm != null)
        {
            this.helm.GetComponent<Item>().icon = allMultiSprites[3];
        }
        //If we have a chestpiece
        if(this.chestpiece != null)
        {
            this.chestpiece.GetComponent<Item>().icon = allMultiSprites[1];
        }
        //If we have leggings
        if(this.leggings != null)
        {
            this.leggings.GetComponent<Item>().icon = allMultiSprites[4];
        }
        //If we have shoes
        if(this.shoes != null)
        {
            this.shoes.GetComponent<Item>().icon = allMultiSprites[2];
        }
        //If we have gloves
        if(this.gloves != null)
        {
            this.gloves.GetComponent<Item>().icon = allMultiSprites[5];
        }
        //If we have a cloak
        if(this.cloak != null)
        {
            this.cloak.GetComponent<Item>().icon = allMultiSprites[0];
        }
    }


    //Function called from all of the Assign Sprites functions. Returns a SpriteView class with all of the designated sprites
    private SpriteViews GetAllSpriteViews(string prefix_, int frontViewIndex_, int sideViewIndex_, int backViewIndex_, BodyTypes bodyType_)
    {
        //Getting the asset path string to the sprite for the front view
        string[] frontViewSpriteGUID = AssetDatabase.FindAssets(prefix_ + "" + frontViewIndex_);
        string spritePath = AssetDatabase.GUIDToAssetPath(frontViewSpriteGUID[0]);

        //Getting the array of all sprites in the base sprite's multi-sprite sheet
        Sprite[] allMultiSprites = AssetDatabase.LoadAllAssetsAtPath(spritePath).OfType<Sprite>().ToArray();

        //Loading the sprites using their paths
        Sprite frontView = allMultiSprites[frontViewIndex_];
        Sprite sideView = allMultiSprites[sideViewIndex_];
        Sprite backView = allMultiSprites[backViewIndex_];

        //Creating the sprite view class to hold our sprites
        SpriteViews newSV = new SpriteViews();
        newSV.front = frontView;
        newSV.side = sideView;
        newSV.back = backView;
        newSV.bodyType = bodyType_;

        return newSV;
    }
}
