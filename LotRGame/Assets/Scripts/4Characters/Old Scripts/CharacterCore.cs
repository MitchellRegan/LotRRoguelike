using UnityEngine;
using System.Collections;

public class CharacterCore : MonoBehaviour
{
    public enum CharacterRace {Human, Elf, Dwarf, Orc, HalfMan, GillFolk, ScaleSkin}
    public CharacterRace Race = CharacterRace.Human;

    public enum CharacterClan { HillTribe, ReefSailer, Ranger, Mercenary, ImperialKnight,   //Human Clans
                                Nomad, Woodland, Tainted, RoyalGuard, Eternal,              //Elven Clans
                                Exiled, TreeFeller, QuarryDigger, DarkDelver, GemBlessed,   //Dwarven Clans
                                SteelKnuckle, Humble, GrimFang, Grayback, Ashen,            //Orcish Clans
                                Burrow, Tinker, Secluded, DuneDweller, Feindish,            //Half-Men Clans
                                StreamSwimmer, MarshDweller, Urchin, HardShell, Angler,     //Gill Folk Clans
                                Horned, Lightning, Lurker, SawTooth, Boulder};              //Scaleskin Clans
    public CharacterClan Clan = CharacterClan.HillTribe;


    public enum CharacterSex {Genderless, Male, Female};
    public CharacterSex Sex = CharacterSex.Male;

    private string Name;
    private int Height;
    private int Weight;


	//Sets the core definition of the character. Uses the designated Race given and the ID of the tile it was found on
	void SetCharacter(CharacterRace Race_, int tileID_)
    {
        Race = Race_;
        GenerateClan(tileID_);
        GenerateSex();
        Height = GenerateHeight();
        Weight = GenerateWeight();
	}


    //Determines what clan this race of character is from ######### INCOMPLETE
    private void GenerateClan(int tileID_)
    {
        switch (Race)
        {
            //Hill Tribe, Reef Sailer, Ranger, Mercenary, Imperial Knight
            case CharacterRace.Human:

                break;

            //Nomad, Woodland, Tainted, Royal Guard, Eternal
            case CharacterRace.Elf:

                break;

            //Exiled, Tree Feller, Quarry Digger, Dark Delver, Gem Blessed
            case CharacterRace.Dwarf:

                break;

            //Steel Knuckle, Humble, Grim Fang, Grayback, Ashen
            case CharacterRace.Orc:

                break;

            //Burrow, Tinker, Secluded, Dune Dweller, Feindish
            case CharacterRace.HalfMan:

                break;

            //Stream Swimmer, Marsh Dweller, Urchin, Hardshell, Angler
            case CharacterRace.GillFolk:

                break;

            //Horned, Lightning, Lurker, Sawtooth, Boulder
            case CharacterRace.ScaleSkin:

                break;
        }
    }

    //Sets the sex of the character. Can be Male, Female, or Genderless based on the Race
    private void GenerateSex()
    {
        //Checks to make sure this isn't a genderless race before deciding the sex
        switch(Race)
        {
            case CharacterRace.GillFolk:
                Sex = CharacterSex.Genderless;
                break;
            
            //If this isn't a genderless race, 50/50 chance male/female
            default:
                float sex = Random.Range(0, 1);
                if (sex <= 0.5)
                {
                    Sex = CharacterSex.Male;
                }
                else
                {
                    Sex = CharacterSex.Female;
                }
                break;
        }
    }


    //Sets the height based on the character's race, sex, and random values
    private int GenerateHeight()
    {
        int height = 0;

        switch (Race)
        {
            //Humans are between 4'8" (56 inches) and 6'4" (76 inches)
            case CharacterRace.Human:
                //Males are up to 3 inches taller
                if(Sex == CharacterSex.Male)
                {
                    height = Random.Range(0, 20) + 56;
                }
                else
                {
                    height = Random.Range(0, 20) + 53;
                }
                break;

            //Elves are between 5'6" (66 inches) and 6'8" (80)
            case CharacterRace.Elf:
                //Males are up to 2 inches taller
                if (Sex == CharacterSex.Male)
                {
                    height = Random.Range(0, 14) + 66;
                }
                else
                {
                    height = Random.Range(0, 14) + 64;
                }
                break;

            //Dwarves are between 3'0" (36 inches) and 4'2" (50 inches)
            case CharacterRace.Dwarf:
                //Males and Females are about the same height
                height = Random.Range(0, 14) + 36;
                break;

            //Orcs are between 6'0" (72 inches) and 7'6" (90 inches)
            case CharacterRace.Orc:
                //Males are up to 2 inches taller
                if (Sex == CharacterSex.Male)
                {
                    height = Random.Range(0, 18) + 72;
                }
                else
                {
                    height = Random.Range(0, 18) + 70;
                }
                break;

            //Half-Men are between 3'0" (36 inches) and 3'8" (44 inches)
            case CharacterRace.HalfMan:
                //Males and Females are about the same height
                height = Random.Range(0, 8) + 36;
                break;

            //Gill Folk are between 4'6" (54 inches) and 6'0" (72 inches)
            case CharacterRace.GillFolk:
                //Females are up to 5 inches taller
                if (Sex == CharacterSex.Male)
                {
                    height = Random.Range(0, 18) + 49;
                }
                else
                {
                    height = Random.Range(0, 18) + 54;
                }
                break;

            //Scaleskin are between 3'8" (44 inches) and 5'10" (70 inches)
            case CharacterRace.ScaleSkin:
                //Males and Females are about the same height
                height = Random.Range(0, 26) + 44;
                break;
        }

        return height;
    }


    //Sets the weight based on the character's race, sex, and random values ######## INCOMPLETE
    private int GenerateWeight()
    {
        int weight = 0;

        switch (Race)
        {
            //Humans are between 
            case CharacterRace.Human:

                break;
            //Elves are between 
            case CharacterRace.Elf:

                break;
            //Dwarves are between
            case CharacterRace.Dwarf:

                break;
            //Orcs are between 
            case CharacterRace.Orc:

                break;
            //Half-Men are between 
            case CharacterRace.HalfMan:

                break;
            //Gill Folk are between 
            case CharacterRace.GillFolk:

                break;
            //Scaleskin are between 
            case CharacterRace.ScaleSkin:

                break;
        }

        return weight;
    }
}
