using System.Collections;
using Evolve.Api;
using Evolve.ConsoleUtils;
using Evolve.Utils;
using MelonLoader;
using Mono.CSharp;
using ButtonApi;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Evolve.Wrappers;
using Il2CppSystem.Collections;
using IEnumerator = System.Collections.IEnumerator;
using Newtonsoft.Json;

namespace Evolve.Buttons
{
    public class SerializePlayer
    {
        public string PlayerName;
        public string PlayerID;
    }

    internal class OverseeMenu
    {
        public static PaginatedMenu ThisMenu;
        public static QMSingleButton Add;
        public static QMSingleButton Remove;
        public static QMSingleButton Add1;
        public static QMSingleButton Remove1;
        public static QMSingleButton Crash;
        public static QMSingleButton Notify;
        public static QMSingleButton Leave;
#pragma warning disable CS0649 // Le champ 'OverseeMenu.Slider3' n'est jamais assigné et aura toujours sa valeur par défaut null
        public static Text Slider3;
#pragma warning restore CS0649 // Le champ 'OverseeMenu.Slider3' n'est jamais assigné et aura toujours sa valeur par défaut null
        public static MenuText Text1;
        public static MenuText Text2;
        public static QMSingleButton ClearAll;
        public static float ComfortDef = 200f;
        public static float MouseDef = 200;
        public static float DefaultMicSensivity = 100;
        public static float MicSensivity = 100;
        public static List<SerializePlayer> AllPlayers;
        public static void Initialize()
        {
            ThisMenu = new PaginatedMenu(EvolveMenu.ThisMenu, 201945, 104894, "Oversee", "", null);
            ThisMenu.menuEntryButton.DestroyMe();
            Panels.PanelMenu(EvolveInteract.ThisMenu, 0, 1.77f, "\nOversee", 1.1f, 1.5f, "Oversee panel");
            Panels.PanelMenu(ThisMenu.menuBase, 5, 0.3f, "\nList", 1.1f, 2.4F, "List panel");
            Panels.PanelMenu(ThisMenu.menuBase, 0, 0.89f, "\nMain Controller", 1.2f, 3.5f, "Main controller");
            Text1 = new MenuText(ThisMenu.menuBase, 900, 380, "Players in the oversee: 1");
            Text2 = new MenuText(ThisMenu.menuBase, 900, 300, $"Action when detected: {FoldersManager.Config.Ini.GetString("Oversee", "Action")}");
            if (!File.Exists($"{Directory.GetCurrentDirectory()}\\Evolve\\Oversee\\Players.txt"))
            {
                AllPlayers = new List<SerializePlayer>();
                File.WriteAllText($"{Directory.GetCurrentDirectory()}\\Evolve\\Oversee\\Players.txt", JsonConvert.SerializeObject(AllPlayers));
                return;
            }

            string Oversee = File.ReadAllText($"{Directory.GetCurrentDirectory()}\\Evolve\\Oversee\\Players.txt");
            AllPlayers = JsonConvert.DeserializeObject<List<SerializePlayer>>(Oversee);

            new QMToggleButton(ThisMenu.menuBase, 0, 0, "Enabled", () =>
           {
               Settings.Oversee = true;
               FoldersManager.Config.Ini.SetBool("Oversee", "Enabled", true);
           }, "Disabled", () =>
           {
               Settings.Oversee = false;
               FoldersManager.Config.Ini.SetBool("Oversee", "Enabled", false);
           }, "Enable or disable the oversee", null, null,false, Settings.Oversee);

            Crash = new QMSingleButton(ThisMenu.menuBase, 0, 0.75f, "Crash", () =>
            {
                FoldersManager.Config.Ini.SetString("Oversee", "Action", "Crash");
                Text2.SetText($"Action when detected: {FoldersManager.Config.Ini.GetString("Oversee", "Action")}");
            }, "Crash the player when detected");

            Notify = new QMSingleButton(ThisMenu.menuBase, 0, 1.25f, "Notify", () =>
            {
                FoldersManager.Config.Ini.SetString("Oversee", "Action", "Notify");
                Text2.SetText($"Action when detected: {FoldersManager.Config.Ini.GetString("Oversee", "Action")}");
            }, "Notify you when the player is detected");

            Leave = new QMSingleButton(ThisMenu.menuBase, 0, 1.75f, "Leave", () =>
            {
                FoldersManager.Config.Ini.SetString("Oversee", "Action", "Leave");
                Text2.SetText($"Action when detected: {FoldersManager.Config.Ini.GetString("Oversee", "Action")}");
            }, "Leave the room when the player is detected");

            Add = new QMSingleButton(EvolveInteract.ThisMenu, 0, 1.75f, "Add", () =>
            {
                var PlayerID = Wrappers.Utils.QuickMenu.SelectedVRCPlayer().GetAPIUser().id;
                SerializePlayer SerializePlayer = new SerializePlayer
                {
                    PlayerName = Wrappers.Utils.QuickMenu.SelectedVRCPlayer().field_Private_VRCPlayerApi_0.displayName,
                    PlayerID = Wrappers.Utils.QuickMenu.SelectedVRCPlayer().GetAPIUser().id,
                };
                foreach (var Player in AllPlayers)
                {
                    if (Player.PlayerID == PlayerID) return;
                }
                AllPlayers.Add(SerializePlayer);
                File.WriteAllText($"{Directory.GetCurrentDirectory()}\\Evolve\\Oversee\\Players.txt", JsonConvert.SerializeObject(AllPlayers));
                LoadMenu();
            }, "Add player by his ID");

            Remove = new QMSingleButton(EvolveInteract.ThisMenu, 0, 2.25f, "Remove", () =>
            {
                foreach (var Player in AllPlayers)
                {
                    if (Player.PlayerID == Wrappers.Utils.QuickMenu.SelectedVRCPlayer().GetAPIUser().id) AllPlayers.Remove(Player);
                }
                File.WriteAllText($"{Directory.GetCurrentDirectory()}\\Evolve\\Oversee\\Players.txt", JsonConvert.SerializeObject(AllPlayers));
                LoadMenu();
            }, "Add player by his ID");

            Add1 = new QMSingleButton(ThisMenu.menuBase, 5, -0.25f, "Add", () =>
            {
                Wrappers.PopupManager.InputeText("Enter name", "Confirm", new Action<string>((a) =>
                {
                    
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopUp();
                    MelonCoroutines.Start(SecondPopup());
                    IEnumerator SecondPopup()
                    {
                        yield return new WaitForSeconds(1);
                        Wrappers.PopupManager.InputeText("Enter user ID", "Confirm", new Action<string>((b) =>
                        {
                            SerializePlayer SerializePlayer = new SerializePlayer
                            {
                                PlayerName = a,
                                PlayerID = b,
                            };
                            foreach (var Player in AllPlayers)
                            {
                                if (Player.PlayerID == b) return;
                            }
                            AllPlayers.Add(SerializePlayer);
                            File.WriteAllText($"{Directory.GetCurrentDirectory()}\\Evolve\\Oversee\\Players.txt", JsonConvert.SerializeObject(AllPlayers));
                            LoadMenu();
                        }));
                    }
                }));
            }, "Add");

            Remove1 = new QMSingleButton(ThisMenu.menuBase, 5, 0.25f, "Remove", () =>
            {
                Wrappers.PopupManager.InputeText("Enter name", "Confirm", new Action<string>((a) =>
                {
                    foreach (var Player in AllPlayers)
                    {
                        if (Player.PlayerName.ToLower() == a.ToLower()) AllPlayers.Remove(Player);
                    }
                    File.WriteAllText($"{Directory.GetCurrentDirectory()}\\Evolve\\Oversee\\Players.txt", JsonConvert.SerializeObject(AllPlayers));
                    LoadMenu();
                }));
            }, "Remove");

            ClearAll = new QMSingleButton(ThisMenu.menuBase, 5, 0.75f, "Clear", () =>
            {
                Wrappers.Utils.VRCUiPopupManager.Alert("Evolve Engine", "Are you sure you want to clear this list ?", "Yes", () =>
                 {
                     AllPlayers.Clear();
                     var SerializePlayer = new SerializePlayer
                     {
                         PlayerName = "Name",
                         PlayerID = "ID",
                     };
                     AllPlayers.Add(SerializePlayer);
                     File.WriteAllText($"{Directory.GetCurrentDirectory()}\\Evolve\\Oversee\\Players.txt", JsonConvert.SerializeObject(AllPlayers));
                     LoadMenu();
                     VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopUp();
                 }, "No", () =>
                 {
                     VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopUp();
                 });
            }, "Clear the whole list");

            Add.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);
            Add1.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);
            Remove.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);
            Remove1.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);
            ClearAll.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);
            Crash.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);
            Notify.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);
            Leave.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);
        }

        public static void LoadMenu()
        {
            ThisMenu.pageItems.Clear();
            using (List<SerializePlayer>.Enumerator enumerator = AllPlayers.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    SerializePlayer Player = enumerator.Current;
                    ThisMenu.pageItems.Insert(0, new PageItem($"{Player.PlayerName}\n<color=magenta>(Remove)</color>", () =>
                    {
                        AllPlayers.Remove(Player);
                        File.WriteAllText($"{Directory.GetCurrentDirectory()}\\Evolve\\Oversee\\Players.txt", JsonConvert.SerializeObject(AllPlayers));
                        if (AllPlayers.Count == 0)
                        {
                            var SerializePlayer = new SerializePlayer
                            {
                                PlayerName = "Name",
                                PlayerID = "ID",
                            };
                            AllPlayers.Add(SerializePlayer);
                            File.WriteAllText($"{Directory.GetCurrentDirectory()}\\Evolve\\Oversee\\Players.txt", JsonConvert.SerializeObject(AllPlayers));
                        }
                        LoadMenu();
                        ThisMenu.OpenMenu();
                    }, string.Concat(new string[]
                    {
                        Player.PlayerName,
                        "\nClick to remove"
                    }), true));
                    Text1.SetText($"Players in the oversee: {AllPlayers.Count}");
                }
            }
        }
    }
}