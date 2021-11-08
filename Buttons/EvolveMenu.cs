using System.Collections;
using Evolve.Api;
using MelonLoader;
using ButtonApi;
using UnityEngine;
using UnityEngine.UI;
using Math = System.Math;
using Evolve.Utils;
using Evolve.ConsoleUtils;
using System.IO;
using System.Text;
using System;
using Il2CppSystem.Configuration;
using Newtonsoft.Json;

namespace Evolve.Buttons
{
    internal class EvolveMenu
    {
        public static QMNestedButton ThisMenu;
        public static QMSingleButton History;
        public static QMSingleButton Oversee;
        public static QMSingleButton Panel;
        public static QMSingleButton EvoPanel;
        public static QMSingleButton NotifTab;
        public static QMSingleButton ConsoleButton;
        public static QMSingleButton LowerPanel;
        public static QMSingleButton AdminButton;
        public static QMSingleButton TesterButton;
        public static QMSingleButton StaffPanel;
        public static QMSingleButton PlayersList;
        public static MenuText NotifText;
        public static Sprite ButtonSprite;
        public static IEnumerator StaffPanelAndMenu()
        {
            //Panel
            WWW request1 = new WWW("https://i.imgur.com/c8xIT7r.png", null, new Il2CppSystem.Collections.Generic.Dictionary<string, string>());
            yield return request1;
            StaffPanel.getGameObject().name = "EvolveConsole";
            UnityEngine.Component.Destroy(StaffPanel.getGameObject().GetComponent<Button>());
            StaffPanel.getGameObject().GetComponent<RectTransform>().sizeDelta *= new Vector2(3, 3);
            StaffPanel.getGameObject().GetComponent<Image>().sprite = Sprite.CreateSprite(request1.texture,
                new Rect(0, 0, request1.texture.width, request1.texture.height), new Vector2(0, 0), 100 * 1000,
                1000, SpriteMeshType.FullRect, Vector4.zero, false);
            StaffPanel.getGameObject().GetComponent<Image>().color = Color.white;
            StaffPanel.setActive(true);
            //Menues
            AdminButton = new QMSingleButton(ThisMenu, 5.25f, 0.25f, "Admin", null, "");
            AdminButton.setIntractable(false);
            TesterButton = new QMSingleButton(ThisMenu, 5.25f, 1.25f, "Tester", null, "");
            TesterButton.setIntractable(false);

            if (Settings.IsTester)
            {
                TesterMenu.Initialize();
                TesterButton.DestroyMe();
            }

            if (Settings.IsAdmin)
            {
                AdminMenu.Initialize();
                TesterMenu.Initialize();
                TesterButton.DestroyMe();
                AdminButton.DestroyMe();
            }
        }

        public static void Initialize()
        {
            ThisMenu = new QMNestedButton("ShortcutMenu", 0, -1, "Evolve", "Evolve Menu", Color.cyan, Color.magenta, Color.black, Color.yellow);
            ThisMenu.getMainButton().setActive(false);

            MelonCoroutines.Start(ReworkedButtonAPI.CreateButton(MenuType.ShortCut, ButtonType.Single, "", 0, -1, delegate ()
            {
                QMStuff.ShowQuickmenuPage(ThisMenu.getMenuName());
            }, "Evolve Menu", Color.black, Color.clear, null, "https://i.imgur.com/c1TLIVh.png"));
            Panel = new QMSingleButton(ThisMenu, 2.5f, 0.85f, "", null, "");
            EvoPanel = new QMSingleButton(ThisMenu, -0.25f, 0.65f, "", null, "");
            StaffPanel = new QMSingleButton(ThisMenu, 5.25f, 0.65f, "", null, "");
            StaffPanel.setActive(false);

            NotifTab = new QMSingleButton("ShortcutMenu", 3.75f, 2.90f, "--->", () =>
            {
                QMStuff.ShowQuickmenuPage("QuickModeMenus/QuickModeNotificationsMenu");
            },"Notifications tab");
            NotifTab.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);
            NotifText = new MenuText(NotifTab.getGameObject().transform, 475, -20, "Notifications");


            MelonCoroutines.Start(LoadConsole());
            MelonCoroutines.Start(LoadPanel());
            MelonCoroutines.Start(LoadLowerPanel());

            MelonCoroutines.Start(LoadPlayerList());
            IEnumerator LoadPlayerList()
            {
                WWW request1 = new WWW("https://i.imgur.com/fwnd82Z.png", null, new Il2CppSystem.Collections.Generic.Dictionary<string, string>());
                yield return request1;
                PlayersList = new QMSingleButton("ShortcutMenu", -2f, 0.5f, "", null, "PlayerList");
                PlayersList.getGameObject().name = "PlayerListShortcutMenu";
                UnityEngine.Component.Destroy(PlayersList.getGameObject().GetComponent<Button>());
                PlayersList.getGameObject().GetComponent<RectTransform>().sizeDelta *= new Vector2(2.4f, 4.3f);
                PlayersList.getGameObject().GetComponent<Image>().sprite = Sprite.CreateSprite(request1.texture,
                    new Rect(0, 0, request1.texture.width, request1.texture.height), new Vector2(0, 0), 100 * 1000,
                    1000, SpriteMeshType.FullRect, Vector4.zero, false);
                PlayersList.getGameObject().GetComponent<Image>().color = Color.white;
                PlayersList.setActive(Settings.PinPL);
            }

            IEnumerator LoadConsole()
            {
                WWW request1 = new WWW("https://i.imgur.com/ibf7o3e.png", null, new Il2CppSystem.Collections.Generic.Dictionary<string, string>());
                yield return request1;
                ConsoleButton = new QMSingleButton("ShortcutMenu", 2.5f, 0.635f, "", null, "Evolve Console");
                ConsoleButton.getGameObject().name = "EvolveConsole";
                UnityEngine.Component.Destroy(ConsoleButton.getGameObject().GetComponent<Button>());
                ConsoleButton.getGameObject().GetComponent<RectTransform>().sizeDelta *= new Vector2(4, 2.2f);
                ConsoleButton.getGameObject().GetComponent<Image>().sprite = Sprite.CreateSprite(request1.texture,
                    new Rect(0, 0, request1.texture.width, request1.texture.height), new Vector2(0, 0), 100 * 1000,
                    1000, SpriteMeshType.FullRect, Vector4.zero, false);
                ConsoleButton.getGameObject().GetComponent<Image>().color = Color.white;
            }

            IEnumerator LoadLowerPanel()
            {
                WWW request1 = new WWW("https://i.imgur.com/Ya9ipH6.png", null, new Il2CppSystem.Collections.Generic.Dictionary<string, string>());
                yield return request1;
                LowerPanel = new QMSingleButton("ShortcutMenu", 1.68f, 2.76f, "", null, "");
                LowerPanel.getGameObject().name = "EvolveConsole";
                UnityEngine.Component.Destroy(LowerPanel.getGameObject().GetComponent<Button>());
                LowerPanel.getGameObject().GetComponent<RectTransform>().sizeDelta *= new Vector2(2.3f, 1.2f);
                LowerPanel.getGameObject().GetComponent<Image>().sprite = Sprite.CreateSprite(request1.texture,
                    new Rect(0, 0, request1.texture.width, request1.texture.height), new Vector2(0, 0), 100 * 1000,
                    1000, SpriteMeshType.FullRect, Vector4.zero, false);
                LowerPanel.getGameObject().GetComponent<Image>().color = Color.white;
            }

            IEnumerator LoadPanel()
            {

                WWW request0 = new WWW("https://i.imgur.com/qNcC5yO.png", null, new Il2CppSystem.Collections.Generic.Dictionary<string, string>());
                yield return request0;
                EvoPanel.getGameObject().name = "EvolveConsole";
                UnityEngine.Component.Destroy(EvoPanel.getGameObject().GetComponent<Button>());
                EvoPanel.getGameObject().GetComponent<RectTransform>().sizeDelta *= new Vector2(3, 3);
                EvoPanel.getGameObject().GetComponent<Image>().sprite = Sprite.CreateSprite(request0.texture,
                    new Rect(0, 0, request0.texture.width, request0.texture.height), new Vector2(0, 0), 100 * 1000,
                    1000, SpriteMeshType.FullRect, Vector4.zero, false);
                EvoPanel.getGameObject().GetComponent<Image>().color = Color.white;


                WWW request1 = new WWW("https://i.imgur.com/TkymXp6.png", null, new Il2CppSystem.Collections.Generic.Dictionary<string, string>());
                yield return request1;
                Panel.getGameObject().name = "EvolveConsole";
                UnityEngine.Component.Destroy(Panel.getGameObject().GetComponent<Button>());
                Panel.getGameObject().GetComponent<RectTransform>().sizeDelta *= new Vector2(4, 4);
                Panel.getGameObject().GetComponent<Image>().sprite = Sprite.CreateSprite(request1.texture,
                    new Rect(0, 0, request1.texture.width, request1.texture.height), new Vector2(0, 0), 100 * 1000,
                    1000, SpriteMeshType.FullRect, Vector4.zero, false);
                Panel.getGameObject().GetComponent<Image>().color = Color.white;

                History = new QMSingleButton(ThisMenu, 3.5f, -0.25f, "History", () =>
                {
                    InstanceHistoryMenu.baseMenu.OpenMenu();
                    InstanceHistoryMenu.Refresh();
                }, "Instance history menu");
                History.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

                Oversee = new QMSingleButton(ThisMenu, 2.5f, 1.25f, "Oversee", () =>
                {
                    if (OverseeMenu.AllPlayers.Count > 0)
                    {
                        OverseeMenu.LoadMenu();
                        OverseeMenu.ThisMenu.OpenMenu();
                    }
                    else
                    {
                        var SerializePlayer = new SerializePlayer
                        {
                            PlayerName = "Name",
                            PlayerID = "ID",
                        };
                        OverseeMenu.AllPlayers.Add(SerializePlayer);
                        File.WriteAllText($"{Directory.GetCurrentDirectory()}\\Evolve\\Oversee\\Players.txt", JsonConvert.SerializeObject(OverseeMenu.AllPlayers));
                        OverseeMenu.LoadMenu();
                        OverseeMenu.ThisMenu.OpenMenu();
                    }
                }, "Oversee");
                Oversee.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);
            }
        }
    }
}