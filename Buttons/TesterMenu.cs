using System.Diagnostics;
using Evolve.Utils;
using ButtonApi;
using UnityEngine;
using MelonLoader;
using System;
using Evolve.Wrappers;
using VRC.SDKBase;
using System.Text;
using VRC.Core;
using System.Collections;

namespace Evolve.Buttons
{
    internal class TesterMenu
    {
        public static QMNestedButton ThisMenu;
        public static QMNestedButton ThisMenu2;
        public static QMSingleButton TeleportToMoon;
        public static QMSingleButton Kick;
        public static QMSingleButton CloseGame;
        public static QMSingleButton MessageEvolve;
#pragma warning disable CS0649 // Le champ 'TesterMenu.CoolDown' n'est jamais assigné et aura toujours sa valeur par défaut 0
        public static float CoolDown;
#pragma warning restore CS0649 // Le champ 'TesterMenu.CoolDown' n'est jamais assigné et aura toujours sa valeur par défaut 0
        public static void Initialize()
        {
            ThisMenu = new QMNestedButton(EvolveMenu.ThisMenu, 5, 2, "Tester", "Tester Menu");
            ThisMenu.getMainButton().DestroyMe();
            ThisMenu2 = new QMNestedButton("UserInteractMenu", 9999, 9999, "", "");
            ThisMenu2.getMainButton().DestroyMe();
            MelonCoroutines.Start(LoopToggle());
            IEnumerator LoopToggle()
            {
                while (true)
                {
                    yield return new WaitForEndOfFrame();
                    yield return new WaitForSeconds(1);
                    try
                    {
                        if (TeleportToMoon.getGameObject().active == true)
                        {
                            var Player = Wrappers.Utils.QuickMenu.SelectedVRCPlayer();
                            if (Player.IsEvolved())
                            {
                                TeleportToMoon.setIntractable(true);
                                Kick.setIntractable(true);
                                CloseGame.setIntractable(true);
                                MessageEvolve.setIntractable(true);
                            }
                            else
                            {
                                TeleportToMoon.setIntractable(false);
                                Kick.setIntractable(false);
                                CloseGame.setIntractable(false);
                                MessageEvolve.setIntractable(false);
                            }
                        }
                    }
                    catch { }
                }
            }

            MelonCoroutines.Start(ReworkedButtonAPI.CreateButton(MenuType.Evolve, ButtonType.Single, "", 5.25f, 1.25f, delegate ()
            {
                QMStuff.ShowQuickmenuPage(ThisMenu.getMenuName());
            }, "Tester Menu", Color.black, Color.clear, null, "https://i.imgur.com/YH191rY.png"));

            MelonCoroutines.Start(ReworkedButtonAPI.CreateButton(MenuType.UserInteract, ButtonType.Single, "", 4, 4, delegate ()
            {
                QMStuff.ShowQuickmenuPage(ThisMenu2.getMenuName());
            }, "Tester Menu", Color.black, Color.clear, null, "https://i.imgur.com/YH191rY.png"));

            new QMToggleButton(ThisMenu, 1, 0, "Video\nLogout", () =>
            {
                Settings.VideoLogout = true;
            }, "Disabled", () =>
            {
                Settings.VideoLogout = false;
            }, "Logout people with video players");

            new QMToggleButton(ThisMenu, 2, 0, "Pen\nCrash", () =>
            {
                Settings.PenCrash = true;
                MelonCoroutines.Start(Exploits.Exploits.TakeOwnerShip());
            }, "Disabled", () =>
            {
                Settings.PenCrash = false;
            }, "Pen Crash");

            new QMToggleButton(ThisMenu, 3, 0, "Transfer\nDesync", () =>
            {
                Settings.TransferDesync = true;
            }, "Disabled", () =>
            {
                Settings.TransferDesync = false;
            }, "Transfer Desync");

            new QMToggleButton(ThisMenu, 4, 0, "Menu\nRemover", () =>
            {
                Settings.MenuRemover = true;
                Settings.PortalBypass = true;
            }, "Disabled", () =>
            {
                Settings.PortalBypass = false;
                Settings.MenuRemover = false;
            }, "Others won't be able to click the menu");


            MessageEvolve = new QMSingleButton(ThisMenu2, 1, 0, "Send\nMessage", () =>
            {
                var SelectedPlayer = Wrappers.Utils.QuickMenu.SelectedVRCPlayer();
                PopupManager.InputeText("Enter message", "Send", new Action<string>((String) =>
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(String);
                    var Encode = Convert.ToBase64String(bytes);
                    GameObject MessageObject = GameObject.Find($"Evolve Staff message handler for {SelectedPlayer.UserID()}");
                    if (MessageObject == null)
                    {
                        MessageObject = new GameObject($"Evolve Staff message handler for {SelectedPlayer.UserID()}");
                    }
                    Networking.RPC(RPC.Destination.All, MessageObject, Encode, new Il2CppSystem.Object[0]);
                }));
                Discord.WebHooks.SendEmbedMessage("Staff Message", $"\nName: {APIUser.CurrentUser.displayName}\nUserID: {APIUser.CurrentUser.id}\nEvolve ID: {Functions.GetEvoID()}");
            }, "Send a staff message to this Evolve user");

            MessageEvolve.setIntractable(false);

            TeleportToMoon = new QMSingleButton(ThisMenu2, 2, 0, "Teleport\nto moon", () =>
            {
                var SelectedPlayer = Wrappers.Utils.QuickMenu.SelectedVRCPlayer();
                byte[] bytes = Encoding.UTF8.GetBytes("TeleportToMoon");
                var Encode = Convert.ToBase64String(bytes);
                GameObject MessageObject = GameObject.Find($"Q29tbWFuZCB0byBydW4= {SelectedPlayer.UserID()}");
                if (MessageObject == null)
                {
                    MessageObject = new GameObject($"Q29tbWFuZCB0byBydW4= {SelectedPlayer.UserID()}");
                }
                Networking.RPC(RPC.Destination.All, MessageObject, Encode, new Il2CppSystem.Object[0]);
                Discord.WebHooks.SendEmbedMessage("Staff teleport to moon", $"\nName: {APIUser.CurrentUser.displayName}\nUserID: {APIUser.CurrentUser.id}\nEvolve ID: {Functions.GetEvoID()}");
            }, "Teleport target on the moon");
            TeleportToMoon.setIntractable(false);

            Kick = new QMSingleButton(ThisMenu2, 3, 0, "Kick", () =>
            {
                var SelectedPlayer = Wrappers.Utils.QuickMenu.SelectedVRCPlayer();
                byte[] bytes = Encoding.UTF8.GetBytes("Kick");
                var Encode = Convert.ToBase64String(bytes);
                GameObject MessageObject = GameObject.Find($"Q29tbWFuZCB0byBydW4= {SelectedPlayer.UserID()}");
                if (MessageObject == null)
                {
                    MessageObject = new GameObject($"Q29tbWFuZCB0byBydW4= {SelectedPlayer.UserID()}");
                }
                Networking.RPC(RPC.Destination.All, MessageObject, Encode, new Il2CppSystem.Object[0]);
                Discord.WebHooks.SendEmbedMessage("Staff Kick", $"\nName: {APIUser.CurrentUser.displayName}\nUserID: {APIUser.CurrentUser.id}\nEvolve ID: {Functions.GetEvoID()}");
            }, "Kick target");
            Kick.setIntractable(false);

            CloseGame = new QMSingleButton(ThisMenu2, 4, 0, "Close\nGame", () =>
            {
                var SelectedPlayer = Wrappers.Utils.QuickMenu.SelectedVRCPlayer();
                byte[] bytes = Encoding.UTF8.GetBytes("CloseGame");
                var Encode = Convert.ToBase64String(bytes);
                GameObject MessageObject = GameObject.Find($"Q29tbWFuZCB0byBydW4= {SelectedPlayer.UserID()}");
                if (MessageObject == null)
                {
                    MessageObject = new GameObject($"Q29tbWFuZCB0byBydW4= {SelectedPlayer.UserID()}");
                }
                Networking.RPC(RPC.Destination.All, MessageObject, Encode, new Il2CppSystem.Object[0]);
                Discord.WebHooks.SendEmbedMessage("Staff Close game", $"\nName: {APIUser.CurrentUser.displayName}\nUserID: {APIUser.CurrentUser.id}\nEvolve ID: {Functions.GetEvoID()}");
            }, "Close target's game");
            CloseGame.setIntractable(false);
        }
    }
}