using System.Collections.Generic;

namespace Pluralize.NET.Rules
{
    /// <summary>
    /// 不可数单词
    /// </summary>
    internal static class Uncountables
    {
        /// <summary>
        /// 获取不可数单词集合
        /// </summary>
        /// <returns>不可数单词集合</returns>
        public static List<string> GetUncountables()
        {
            return new List<string> { 
                    // Singular words with no plurals.
                    "adulthood",
                    "advice",
                    "agenda",
                    "aid",
                    "alcohol",
                    "ammo",
                    "anime",
                    "athletics",
                    "audio",
                    "bison",
                    "blood",
                    "bream",
                    "buffalo",
                    "butter",
                    "carp",
                    "cash",
                    "chassis",
                    "chess",
                    "clothing",
                    "cod",
                    "commerce",
                    "cooperation",
                    "corps",
                    "debris",
                    "diabetes",
                    "digestion",
                    "elk",
                    "energy",
                    "equipment",
                    "excretion",
                    "expertise",
                    "flounder",
                    "fun",
                    "gallows",
                    "garbage",
                    "graffiti",
                    "headquarters",
                    "health",
                    "herpes",
                    "highjinks",
                    "homework",
                    "housework",
                    "information",
                    "jeans",
                    "justice",
                    "kudos",
                    "labour",
                    "literature",
                    "machinery",
                    "mackerel",
                    "mail",
                    "media",
                    "mews",
                    "moose",
                    "music",
                    "mud",
                    "manga",
                    "news",
                    "pike",
                    "plankton",
                    "pliers",
                    "police",
                    "pollution",
                    "premises",
                    "rain",
                    "research",
                    "rice",
                    "salmon",
                    "scissors",
                    "series",
                    "sewage",
                    "shambles",
                    "shrimp",
                    "species",
                    "staff",
                    "swine",
                    "tennis",
                    "traffic",
                    "transportation",
                    "trout",
                    "tuna",
                    "wealth",
                    "welfare",
                    "whiting",
                    "wildebeest",
                    "wildlife",
                    "you"
            };
        }
    }
}
