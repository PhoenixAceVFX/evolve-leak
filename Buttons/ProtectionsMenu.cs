using Evolve.Utils;
using ButtonApi;
using UnityEngine;
using Evolve.Protections;

namespace Evolve.Buttons
{
    internal class ProtectionsMenu
    {
        public static QMNestedButton ThisMenu;
        public static QMToggleButton UdonProt;
        public static QMToggleButton RPCBlock;
        public static QMToggleButton EventsBlock;
        public static QMToggleButton AvatarCleaning;
        public static QMToggleButton Chairs;
        public static QMToggleButton Pickups;
        public static QMToggleButton Portals;

        public static void Initialzie()
        {
            ThisMenu = new QMNestedButton(EvolveMenu.ThisMenu, 1.5f, 0.75f, "Protections", "Protections Menu", null, null, null, Color.yellow);
            ThisMenu.getMainButton().getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            new QMToggleButton(ThisMenu, 4, 2, "Reflect", () =>
            {
                FoldersManager.Config.Ini.SetBool("Protections", "Reflect", true);
                Settings.Reflect = true;
            }, "Disabled", () =>
            {
                FoldersManager.Config.Ini.SetBool("Protections", "Reflect", false);
                Settings.Reflect = false;
            }, "Will automatically send back malicious events or crashes.", null, null, false, Settings.Reflect);

            Pickups = new QMToggleButton(ThisMenu, 2, 1, "Pickups", () =>
            {
                FoldersManager.Config.Ini.SetBool("Protections", "PickupProtection", true);
                Settings.PickupsProtection = true;
                Toggles.Pickups(false);
            }, "Disabled", () =>
            {
                FoldersManager.Config.Ini.SetBool("Protections", "PickupProtection", false);
                Settings.PickupsProtection = false;
                Toggles.Pickups(true);
            }, "Will automatically hide pickups in the world", null, null, false, Settings.PickupsProtection);

            new QMToggleButton(ThisMenu, 1, 1, "Public Ban", () =>
            {
                Settings.PublicBanProtection = true;
                FoldersManager.Config.Ini.SetBool("Protections", "BanProtection", true);
            }, "Disabled", () =>
            {
                Settings.PublicBanProtection = false;
                FoldersManager.Config.Ini.SetBool("Protections", "BanProtection", false);
            }, "You won't be able to get public banned", null, null, false, Settings.PublicBanProtection);

            EventsBlock = new QMToggleButton(ThisMenu, 1, 0, "Events Block", () =>
            {
                Settings.EventBlock = true;
                FoldersManager.Config.Ini.SetBool("Protections", "EventProtection", true);
            }, "Disabled", () =>
            {
                Settings.EventBlock = false;
                FoldersManager.Config.Ini.SetBool("Protections", "EventProtection", false);
            }, "Will block malicious events", null, null, false, Settings.EventBlock);

            RPCBlock = new QMToggleButton(ThisMenu, 2, 0, "RPC Block", () =>
            {
                Settings.RPCBlock = true;
                FoldersManager.Config.Ini.SetBool("Protections", "RPCBlock", true);
            }, "Disabled", () =>
            {
                Settings.RPCBlock = false;
                FoldersManager.Config.Ini.SetBool("Protections", "RPCBlock", false);
            }, "Will block malicious RPC", null, null, false, Settings.RPCBlock);

            new QMToggleButton(ThisMenu, 3,  2, "SteamID Spoof", () =>
            {
                Settings.SteamSpoof = true;
                FoldersManager.Config.Ini.SetBool("Protections", "SteamSpoof", true);
            }, "Disabled", () =>
            {
                Settings.SteamSpoof = false;
                FoldersManager.Config.Ini.SetBool("Protections", "SteamSpoof", false);
            }, "Will spoof your steam id", null, null, false, Settings.SteamSpoof);

            new QMToggleButton(ThisMenu, 2, 2, "IP-Protection", () =>
            {
                Settings.IpProtection = true;
                FoldersManager.Config.Ini.SetBool("Protections", "IPProtection", true);
            }, "Disabled", () =>
            {
                Settings.IpProtection = false;
                FoldersManager.Config.Ini.SetBool("Protections", "IPProtection", false);
            }, "Will protect your ip from being logged through anything\n<b><color=red>YOU WONT BE ABLE TO SEE ANY IMAGES</color></b>", null, null, false, Settings.IpProtection);

            UdonProt =new QMToggleButton(ThisMenu, 3, 0, "Udon RPC\nBlock", () =>
            {
                Settings.UdonExploitProtections = true;
                FoldersManager.Config.Ini.SetBool("Protections", "UdonExploits", true);
            }, "Disabled", () =>
            {
                Settings.UdonExploitProtections = false;
                FoldersManager.Config.Ini.SetBool("Protections", "UdonExploits", false);
            }, "Will protect you from being affected by udon exploits", null, null, false, Settings.UdonExploitProtections);

            AvatarCleaning = new QMToggleButton(ThisMenu, 1, 2, "Avatar Cleaning", () =>
            {
                Settings.AvatarCleaning = true;
                FoldersManager.Config.Ini.SetBool("Protections", "AvatarCleaning", true);
            }, "Disabled", () =>
            {
                Settings.AvatarCleaning = false;
                FoldersManager.Config.Ini.SetBool("Protections", "AvatarCleaning", false);
            }, "Will protect you from other player's avatar", null, null, false, Settings.AvatarCleaning);

            Chairs = new QMToggleButton(ThisMenu, 3, 1, "Chairs", () =>
            {
                Settings.ChairsProtection = true;
                FoldersManager.Config.Ini.SetBool("Protections", "Chairs", true);
                Toggles.Chairs(false);

            }, "Disabled", () =>
            {
                Settings.ChairsProtection = false;
                FoldersManager.Config.Ini.SetBool("Protections", "Chairs", false);
                Toggles.Chairs(true);
            }, "Will disable every chairs in the world", null, null, false, Settings.ChairsProtection);

            Portals = new QMToggleButton(ThisMenu, 4, 1, "Portals", () =>
            {
                Settings.PortalProtection = true;
            }, "Disabled", () =>
            {
                Settings.PortalProtection = false;
            }, "Will protect you from portal droppers", null, null, false, Settings.PortalProtection);

            new QMToggleButton(ThisMenu, 4, 0, "Moderation", () =>
            {
                Settings.Moderation = true;
                FoldersManager.Config.Ini.SetBool("Protections", "Moderation", true);
            }, "Disabled", () =>
            {
                Settings.Moderation = false;
                FoldersManager.Config.Ini.SetBool("Protections", "Moderation", false);
            }, "Anti block, mute, turn on Moderation logs to get notifications.", null, null, false, Settings.Moderation);
        }
    }
}