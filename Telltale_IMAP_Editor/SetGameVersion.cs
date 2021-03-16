using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telltale_IMAP_Editor
{
    public class SetGameVersion
    {
        /// <summary>
        /// All the game enums that lib telltale supports
        /// </summary>
        public enum Versions
        {
            Game_Of_Thrones = 0,
            The_Wolf_Among_Us = 1,
            Marvels_Gaurdians_Of_The_Galaxy = 2,
            Batman_The_Telltale_Series = 3,
            Batman_The_Enemy_Within = 4,
            Tales_from_The_Borderlands = 5,
            Telltale_Texas_Holdem = 6,
            Bone_Out_from_Boneville = 7,
            Bone_The_Great_Cow_Race = 8,
            CSI_3_Dimensions_of_Murder = 9,
            CSI_Deadly_Intent = 10,
            CSI_Fatal_Conspiracy = 11,
            CSI_Hard_Evidence = 12,
            Jurassic_Park_The_Game = 13,
            Law_and_Order_Legacies = 14,
            Minecraft_Story_Mode_Season_One = 15,
            Minecraft_Story_Mode_Season_Two = 16,
            Poker_Night_at_The_Inventory = 17,
            Poker_Night_2 = 18,
            The_Walkind_Dead_Season_One_101 = 19,
            The_Walking_Dead_Season_Two = 20,
            The_Walking_Dead_Michonne = 21,
            The_Walking_Dead_Season_Three = 22,
            The_Walking_Dead_Season_Four = 23,
            The_Walking_Dead_Definitive_Series = 24,
            Sam_and_Max_Season_One_101 = 25,
            Sam_and_Max_Season_One_102 = 26,
            Sam_and_Max_Season_One_103 = 27,
            Sam_and_Max_Season_One_104 = 28,
            Sam_and_Max_Season_One_105 = 29,
            Sam_and_Max_Season_One_106 = 30,
            Sam_and_Max_Season_Two_201 = 31,
            Sam_and_Max_Season_Two_202 = 32,
            Sam_and_Max_Season_Two_203 = 33,
            Sam_and_Max_Season_Two_204 = 34,
            Sam_and_Max_Season_Two_205 = 35,
            Sam_and_Max_Season_Three_301 = 36,
            Sam_and_Max_Season_Three_302 = 37,
            Sam_and_Max_Season_Three_303 = 38,
            Sam_and_Max_Season_Three_304 = 39,
            Sam_and_Max_Season_Three_305 = 40,
            Sam_and_Max_Save_The_World_Remastered = 41,
            Hector_Badge_Of_Carnage_101 = 42,
            Hector_Badge_Of_Carnage_102 = 43,
            Hector_Badge_Of_Carnage_103 = 44,
            Puzzle_Agent_101 = 45,
            Puzzle_Agent_102 = 46,
            Back_To_The_Future_101 = 47,
            Back_To_The_Future_102 = 48,
            Back_To_The_Future_103 = 49,
            Back_To_The_Future_104 = 50,
            Back_To_The_Future_105 = 51,
            Strong_Bad_Cool_Game_for_Attractive_People_101 = 52,
            Strong_Bad_Cool_Game_for_Attractive_People_102 = 53,
            Strong_Bad_Cool_Game_for_Attractive_People_103 = 54,
            Strong_Bad_Cool_Game_for_Attractive_People_104 = 55,
            Strong_Bad_Cool_Game_for_Attractive_People_105 = 56,
            Tales_of_Monkey_Island_101 = 57,
            Tales_of_Monkey_Island_102 = 58,
            Tales_of_Monkey_Island_103 = 59,
            Tales_of_Monkey_Island_104 = 60,
            Tales_of_Monkey_Island_105 = 61,
            Wallace_And_Gromits_Grand_Adventures_101 = 62,
            Wallace_And_Gromits_Grand_Adventures_102 = 63,
            Wallace_And_Gromits_Grand_Adventures_103 = 64,
            Wallace_And_Gromits_Grand_Adventures_104 = 65,
        }

        /// <summary>
        /// Turns the game version enum into a string list
        /// </summary>
        /// <param name="replaceUnderscoreWithSpaces"></param>
        /// <returns></returns>
        public static List<string> Get_Versions_ToStringList(bool replaceUnderscoreWithSpaces = false)
        {
            //if we want the names to look nicer with any underscores
            if (replaceUnderscoreWithSpaces)
            {
                //our enums into a list
                List<string> enumNames = new List<string>(Enum.GetNames(typeof(Versions)));

                //the list of our cleaned up enum names
                List<string> changed_enumNames = new List<string>();

                //go through our main enum list
                foreach(string enumName in enumNames)
                {
                    //replace the underscore with a space
                    changed_enumNames.Add(enumName.Replace("_", " "));
                }

                //return the final list
                return changed_enumNames;
            }
            else
                return new List<string>(Enum.GetNames(typeof(Versions)));
        }

        /// <summary>
        /// Turn a game version string value into a game version enum
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Versions Get_Versions_ParseStringValue(string value)
        {
            return (Versions)Enum.Parse(typeof(Versions), value);
        }

        /// <summary>
        /// Turn an int value into a game version enum
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Versions Get_Versions_ParseIntValue(int value)
        {
            return (Versions)value;
        }

        /// <summary>
        /// Turns a game version enum into the actual string that lib telltle recognizes and uses for game id's
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public static string Get_GameID_FromVersion(Versions version)
        {
            switch(version)
            {
                case Versions.Back_To_The_Future_101:
                    return "bttf101";
                case Versions.Back_To_The_Future_102:
                    return "bttf102";
                case Versions.Back_To_The_Future_103:
                    return "bttf103";
                case Versions.Back_To_The_Future_104:
                    return "bttf104";
                case Versions.Back_To_The_Future_105:
                    return "bttf105";
                case Versions.Batman_The_Telltale_Series:
                    return "batman";
                case Versions.Batman_The_Enemy_Within:
                    return "batman2";
                case Versions.Bone_Out_from_Boneville:
                    return "boneville";
                case Versions.Bone_The_Great_Cow_Race:
                    return "cowrace";
                case Versions.CSI_3_Dimensions_of_Murder:
                    return "csi3dimensions";
                case Versions.CSI_Deadly_Intent:
                    return "csideadly";
                case Versions.CSI_Fatal_Conspiracy:
                    return "csifatal";
                case Versions.CSI_Hard_Evidence:
                    return "csihard";
                case Versions.Game_Of_Thrones:
                    return "thrones";
                case Versions.Hector_Badge_Of_Carnage_101:
                    return "hector101";
                case Versions.Hector_Badge_Of_Carnage_102:
                    return "hector102";
                case Versions.Hector_Badge_Of_Carnage_103:
                    return "hector103";
                case Versions.Jurassic_Park_The_Game:
                    return "jurrassicpark";
                case Versions.Law_and_Order_Legacies:
                    return "lawandorder";
                case Versions.Marvels_Gaurdians_Of_The_Galaxy:
                    return "marvel";
                case Versions.Minecraft_Story_Mode_Season_One:
                    return "mcsm";
                case Versions.Minecraft_Story_Mode_Season_Two:
                    return "mc2";
                case Versions.Poker_Night_at_The_Inventory:
                    return "celebritypoker";
                case Versions.Poker_Night_2:
                    return "celebritypoker2";
                case Versions.Puzzle_Agent_101:
                    return "grickle101";
                case Versions.Puzzle_Agent_102:
                    return "grickle102";
                case Versions.Sam_and_Max_Save_The_World_Remastered:
                    return "sammaxremaster";
                case Versions.Sam_and_Max_Season_One_101:
                    return "sammax101";
                case Versions.Sam_and_Max_Season_One_102:
                    return "sammax102";
                case Versions.Sam_and_Max_Season_One_103:
                    return "sammax103";
                case Versions.Sam_and_Max_Season_One_104:
                    return "sammax104";
                case Versions.Sam_and_Max_Season_One_105:
                    return "sammax105";
                case Versions.Sam_and_Max_Season_One_106:
                    return "sammax106";
                case Versions.Sam_and_Max_Season_Three_301:
                    return "sammax301";
                case Versions.Sam_and_Max_Season_Three_302:
                    return "sammax302";
                case Versions.Sam_and_Max_Season_Three_303:
                    return "sammax303";
                case Versions.Sam_and_Max_Season_Three_304:
                    return "sammax304";
                case Versions.Sam_and_Max_Season_Three_305:
                    return "sammax305";
                case Versions.Sam_and_Max_Season_Two_201:
                    return "sammax201";
                case Versions.Sam_and_Max_Season_Two_202:
                    return "sammax202";
                case Versions.Sam_and_Max_Season_Two_203:
                    return "sammax203";
                case Versions.Sam_and_Max_Season_Two_204:
                    return "sammax204";
                case Versions.Sam_and_Max_Season_Two_205:
                    return "sammax205";
                case Versions.Strong_Bad_Cool_Game_for_Attractive_People_101:
                    return "sbcg4ap101";
                case Versions.Strong_Bad_Cool_Game_for_Attractive_People_102:
                    return "sbcg4ap102";
                case Versions.Strong_Bad_Cool_Game_for_Attractive_People_103:
                    return "sbcg4ap103";
                case Versions.Strong_Bad_Cool_Game_for_Attractive_People_104:
                    return "sbcg4ap104";
                case Versions.Strong_Bad_Cool_Game_for_Attractive_People_105:
                    return "sbcg4ap105";
                case Versions.Tales_from_The_Borderlands:
                    return "borderlands";
                case Versions.Tales_of_Monkey_Island_101:
                    return "monkeyisland101";
                case Versions.Tales_of_Monkey_Island_102:
                    return "monkeyisland102";
                case Versions.Tales_of_Monkey_Island_103:
                    return "monkeyisland103";
                case Versions.Tales_of_Monkey_Island_104:
                    return "monkeyisland104";
                case Versions.Tales_of_Monkey_Island_105:
                    return "monkeyisland105";
                case Versions.Telltale_Texas_Holdem:
                    return "texasholdem";
                case Versions.The_Walkind_Dead_Season_One_101:
                    return "twd1";
                case Versions.The_Walking_Dead_Definitive_Series:
                    return "wdc";
                case Versions.The_Walking_Dead_Michonne:
                    return "michonne";
                case Versions.The_Walking_Dead_Season_Four:
                    return "wd4";
                case Versions.The_Walking_Dead_Season_Three:
                    return "wd3";
                case Versions.The_Walking_Dead_Season_Two:
                    return "wd2";
                case Versions.The_Wolf_Among_Us:
                    return "fables";
                case Versions.Wallace_And_Gromits_Grand_Adventures_101:
                    return "wag101";
                case Versions.Wallace_And_Gromits_Grand_Adventures_102:
                    return "wag102";
                case Versions.Wallace_And_Gromits_Grand_Adventures_103:
                    return "wag103";
                case Versions.Wallace_And_Gromits_Grand_Adventures_104:
                    return "wag104";
                default:
                    return "";
            }
        }
    }
}
