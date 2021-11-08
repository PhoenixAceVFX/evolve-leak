using System.Diagnostics;
using Evolve.Utils;
using ButtonApi;
using UnityEngine;
using System;
using Evolve.Wrappers;

namespace Evolve.Buttons
{
    internal class DiscordMenu
    {
        public static QMNestedButton ThisMenu;
        public static float CoolDown;
        public static void Initialize()
        {
            ThisMenu = new QMNestedButton(EvolveMenu.ThisMenu, 3.5f, 0.75f, "Discord", "Discord Menu");
            ThisMenu.getMainButton().getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector3(1, 2);

            new QMToggleButton(ThisMenu, 2, 0, "Hide Name", () =>
            {
                Settings.HideDiscordName = true;
                FoldersManager.Config.Ini.SetBool("Discord", "HideName", true);
            }, "Disabled", () =>
            {
                Settings.HideDiscordName = false;
                FoldersManager.Config.Ini.SetBool("Discord", "HideName", false);
            }, "Will hide your name in the rich presence", null, null, false, Settings.HideDiscordName);

            new QMToggleButton(ThisMenu, 3, 0, "Hide World", () =>
            {
                FoldersManager.Config.Ini.SetBool("Discord", "HideWorld", true);
                Settings.HideDiscordWorld = true;
            }, "Disabled", () =>
            {
                FoldersManager.Config.Ini.SetBool("Discord", "HideWorld", false);
                Settings.HideDiscordWorld = false;
            }, "Will hide the world you are in from the rich presence", null, null, false, Settings.HideDiscordWorld);

            new QMSingleButton(ThisMenu, 1, 0, "Join server", () =>
            {
                Process.Start("https://discord.gg/SQDjQXHN3E");
            }, "Join Evolve discord server");

            new QMSingleButton(ThisMenu, 4, 0, "Send\nMessage", () =>
            {
                if (Time.time > CoolDown)
                {
                    CoolDown = Time.time + 20;
                    Wrappers.PopupManager.InputeText("Evolve Engine", "Search", new Action<string>((a) =>
                    {
                        Discord.WebHooks.SendPublicMessage(DateTime.Now.ToString("[HH:mm]: ") + a);
                    }));
                }
                else
                {
                    var CoolDownLeft = CoolDown - Time.time;
                    Wrappers.Utils.VRCUiPopupManager.ShowAlert("Evolve Engine", $"Function is still on cooldown...\nSeconds left: {Math.Floor(CoolDownLeft)}");
                }
            }, "Send message on the discord server");
        }
    }
}