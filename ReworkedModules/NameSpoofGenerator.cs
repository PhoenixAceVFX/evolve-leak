using System;
using System.Collections.Generic;
using System.Linq;

namespace Evolve.Modules
{
    internal class NameSpoofGenerator
    {
        public string InfoSpoofingName = "";
        public static string spoofedName
        {
            get
            {
                return generatedSpoofName;
            }
        }

        internal static void GenerateNewName()
        {
            Random random = new Random();
            int index = random.Next(NameSpoofGenerator.adjectiveList.Count<string>());
            string text = NameSpoofGenerator.adjectiveList[index];
            while (text == NameSpoofGenerator.adjectiveList[index])
            {
                int index2 = random.Next(NameSpoofGenerator.nounList.Count<string>());
                string text2 = text + NameSpoofGenerator.nounList[index2];
                if (text2.Length < 15)
                {
                    text = text2;
                }
            }
            NameSpoofGenerator.generatedSpoofName = text;
        }

        public static string generatedSpoofName = "";

        public static List<string> adjectiveList = new List<string>
        {
           "Evolved ",
           "Evolving "
        };

        public static List<string> nounList = new List<string>
        {
            "Angel",
            "Aurora",
            "Abelia",
            "Acer",
            "Allium",
            "Alpine",
            "Almond",
            "Abode",
            "Abyss",
            "Ace",
            "Aerie",
            "Alum",
            "Bamboo",
            "Bay",
            "Bella",
            "Bunny",
            "Blossom",
            "Engine",
            "Blueberry",
            "Crystal",
            "Camellia",
            "Canna",
            "Carnation",
            "Diamond",
            "Daffodil",
            "Daylight",
            "Demon",
            "Demoness",
            "Emilia",
            "Emerald",
            "Elf",
            "Elderberry",
            "Eucalyptus",
            "Flower",
            "Fairy",
            "Feather",
            "Fox",
            "Garnet",
            "Gamer",
            "Grace",
            "Galaxy",
            "Gift",
            "Gianna",
            "Grape",
            "Hazel",
            "Heart",
            "Humility",
            "Hypatia",
            "Lavender",
            "Lush",
            "Lore",
            "Lapis",
            "Mana",
            "Miracle",
            "Moon",
            "Nora",
            "Nebula",
            "Night",
            "Platinum",
            "Port",
            "Princess",
            "Rhodium",
            "Ruby",
            "Succubus",
            "Seductress",
            "Savior",
            "Topaz",
            "Tera",
            "Tulip",
            "Tale",
            "Tail",
            "Ass",
            "Tits",
            "Pedophile"
        };
    }
}
