using Evolve.ConsoleUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolve.FoldersManager
{
    class CheckIfBlacklistedMod
    {
        public static List<string> BlacklistedMod = new List<string>()
        {
            "dayclient",
            "explorer",
            "fclient",
            "UnityExplorer.ML.IL2CPP",
            "unity",
            "spy",
            "svh",
            "plague",
            "anti",
            "discord",
            "log",
            "DontTouchMyClient",
            "astro",
            "donttouch",
            "dont"
        };

        public static void Check()
        {
            foreach (var Mod in Directory.GetFiles("Mods/"))
            {
                foreach (var Bmod in BlacklistedMod)
                {
                    if (Mod.ToLower().Contains(Bmod.ToLower()))
                    {
                        File.Delete($"{Mod}");
                        EvoConsole.Log($"Detected and deleted: {Mod}");
                        Login.Authorization.DetectedMods = true;
                    }
                }
            }
        }
    }
}
