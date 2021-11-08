using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ButtonApi;
using Evolve.Utils;
using Evolve.Wrappers;
using MelonLoader;
using UnityEngine;

namespace Evolve.Bot
{
    class NullInstancesBots
    {
        public static QMNestedButton ThisMenu;
        public static void Initialize()
        {
            ThisMenu = new QMNestedButton(BotMenu.ThisMenu, 5, 1.75f, "Null Instances", "Null Instances bot, make a genocide");
            ThisMenu.getMainButton().getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            new QMSingleButton(ThisMenu, 1, 0, "Start", () =>
            {
                PopupManager.InputeText("Amount of bots", "Start", new Action<string>((Number) =>
                {
                    MelonCoroutines.Start(Delayed());
                    IEnumerator Delayed()
                    {
                        string[] Mods = Directory.GetFiles("Mods\\");
                        List<string> AllMods = new List<string>();

                        foreach (string Mod in Mods)
                        {
                            var DllName = Mod.Split('\\')[1];
                            if (Mod.Contains(".dll"))
                            {
                                File.Move($"{Mod}", $"Mods\\Bots\\{DllName}");
                                AllMods.Add(DllName);
                            }
                        }
                        File.Move("Mods/Bots/NullInstances.dll", "Mods/NullInstances.dll");

                        var Amount = int.Parse(Number);
                        int StartID = 0;
                        for (int I = 0; I < Amount; I++)
                        {
                            StartID++;
                            Utilities.StartProcess("VRChat.exe", $"--profile=10{StartID} -batchmode -nographics -noUpm -disable-gpu-skinning -no-stereo-rendering -nolog --no-vr %2");
                        }
                        yield return new WaitForSeconds(5);

                        foreach (var Mod in AllMods)
                        {
                            File.Move($"Mods/Bots/{Mod}", $"Mods/{Mod}");
                        }
                        File.Move("Mods/NullInstances.dll", "Mods/Bots/NullInstances.dll");
                    }
                }));
            }, "Choose how many bots you wanna start");

            new QMSingleButton(ThisMenu, 2, 0, "Shutdown", () =>
            {
                Server.SendMessage($"Shutdown");
            }, "Will shutdown all running bots");
        }
    }
}
