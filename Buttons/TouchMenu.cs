using Evolve.Api;
using Evolve.Modules;
using Evolve.Utils;
using Evolve.Wrappers;
using MelonLoader;
using Mono.CSharp;
using ButtonApi;
using UnityEngine;
using VRC.Core;

namespace Evolve.Buttons
{
    internal class TouchMenu
    {
        public static QMNestedButton ThisMenu;
#pragma warning disable CS0649 // Le champ 'UwUMenu.Clear' n'est jamais assigné et aura toujours sa valeur par défaut null
        public static QMSingleButton Clear;
#pragma warning restore CS0649 // Le champ 'UwUMenu.Clear' n'est jamais assigné et aura toujours sa valeur par défaut null
#pragma warning disable CS0649 // Le champ 'UwUMenu.AddAll' n'est jamais assigné et aura toujours sa valeur par défaut null
        public static QMSingleButton AddAll;
#pragma warning restore CS0649 // Le champ 'UwUMenu.AddAll' n'est jamais assigné et aura toujours sa valeur par défaut null
        public static QMToggleButton Global;

        public static void Initialize()
        {
            ThisMenu = new QMNestedButton("ShortcutMenu", 0, 1, "UwU", "UwUMenu", Color.cyan, Color.magenta, Color.black, Color.yellow);
            ThisMenu.getMainButton().getGameObject().SetActive(false);
            Panels.PanelMenu(ThisMenu, 0, 0.89f, "\nMain Controller", 1.2f, 3.5f, "Main controller");
            Panels.PanelMenu(ThisMenu, 2.5f, -0.13f, "\nYour colliders", 2.2f, 1.5f, "Select your colliders");
            Panels.PanelMenu(ThisMenu, 2.5f, 1.88f, "\nPart to touch", 3.2f, 1.5f, "Select the bones you wanna touch");

            MelonCoroutines.Start(ReworkedButtonAPI.CreateButton(MenuType.ShortCut, ButtonType.Single, "", 0, 1, delegate ()
            {
                QMStuff.ShowQuickmenuPage(ThisMenu.getMenuName());
            }, "UwU Menu", Color.black, Color.clear, null, "https://i.imgur.com/6pPJHRe.png"));
            new QMToggleButton(ThisMenu, 0, 0, "Touch", () =>
            {
                Settings.GlobalTouch = true;
                GlobalDynamicBones.currentWorldDynamicBoneColliders.Clear();
                GlobalDynamicBones.currentWorldDynamicBones.Clear();
                GlobalDynamicBones.ProcessDynamicBones(VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCAvatarManager_0);
                FoldersManager.Config.Ini.SetBool("UwUMenu", "Touch", true);
            }, "Disabled", () =>
            {
                Settings.GlobalTouch = false;
                GlobalDynamicBones.Users.Clear();
                GlobalDynamicBones.currentWorldDynamicBoneColliders.Clear();
                GlobalDynamicBones.currentWorldDynamicBones.Clear();
                Global.setToggleState(false, true);
                FoldersManager.Config.Ini.SetBool("UwUMenu", "Touch", false);
                foreach (var Player in Wrappers.Utils.PlayerManager.AllPlayers())
                {
                    Player.ReloadAvatar();
                }
            }, "Enable or disable touch in general", null, null, false, Settings.GlobalTouch);

            new QMToggleButton(ThisMenu, 0, 2, "Haptics\nTouch", () =>
            {
                ImmersiveTouch.Enable = true;
                DynamicPerformances.enableCollisionChecks = false;
                DynamicPerformances.enableUpdate = false;
                DynamicPerformances.updateMultiThread = false;
                GlobalDynamicBones.Users.Clear();
                GlobalDynamicBones.currentWorldDynamicBoneColliders.Clear();
                GlobalDynamicBones.currentWorldDynamicBones.Clear();
                foreach (var Player in Wrappers.Utils.PlayerManager.AllPlayers())
                {
                    Player.ReloadAvatar();
                }
                FoldersManager.Config.Ini.SetBool("UwUMenu", "Haptics", true);
            }, "Disabled", () =>
            {
                ImmersiveTouch.Enable = false;
                DynamicPerformances.enableCollisionChecks = true;
                DynamicPerformances.enableUpdate = true;
                DynamicPerformances.updateMultiThread = true;
                GlobalDynamicBones.Users.Clear();
                GlobalDynamicBones.currentWorldDynamicBoneColliders.Clear();
                GlobalDynamicBones.currentWorldDynamicBones.Clear();
                foreach (var Player in Wrappers.Utils.PlayerManager.AllPlayers())
                {
                    Player.ReloadAvatar();
                }
                FoldersManager.Config.Ini.SetBool("UwUMenu", "Haptics", false);
            }, "Will send vibrations to your controllers when interacting with any dynamic bones", null, null, false, ImmersiveTouch.Enable);

            new QMToggleButton(ThisMenu, 2, 0, "Hand", () =>
            {
                Settings.HandColliders = true;
                FoldersManager.Config.Ini.SetBool("UwUMenu", "HandColliders", true);
            }, "Disabled", () =>
            {
                Settings.HandColliders = false;
                FoldersManager.Config.Ini.SetBool("UwUMenu", "HandColliders", false);
            }, "Select your hand colliders if you have them on your avatar", null, null, false, Settings.HandColliders);

            new QMToggleButton(ThisMenu, 3, 0, "Feet", () =>
            {
                Settings.FeetColliders = true;
                FoldersManager.Config.Ini.SetBool("UwUMenu", "FeetColliders", true);
            }, "Disabled", () =>
            {
                Settings.FeetColliders = false;
                FoldersManager.Config.Ini.SetBool("UwUMenu", "FeetColliders", false);
            }, "Select your feet colliders if you have them on your avatar", null, null, false, Settings.FeetColliders);

            new QMToggleButton(ThisMenu, 1.5f, 2, "Head", () =>
            {
                Settings.HeadBones = true;
                FoldersManager.Config.Ini.SetBool("UwUMenu", "Head", true);
            }, "Disabled", () =>
            {
                Settings.HeadBones = false;
                FoldersManager.Config.Ini.SetBool("UwUMenu", "Head", false);
            }, "Will make you able to touch all the bones from their head", null, null, false, Settings.HeadBones);

            new QMToggleButton(ThisMenu, 3.5f, 2, "Hips", () =>
            {
                Settings.HipBones = true;
                FoldersManager.Config.Ini.SetBool("UwUMenu", "Hips", true);
            }, "Disabled", () =>
            {
                Settings.HipBones = false;
                FoldersManager.Config.Ini.SetBool("UwUMenu", "Hips", false);
            }, "Will make you able to touch all the bones from their hips", null, null, false, Settings.HipBones);

            new QMToggleButton(ThisMenu, 2.5f, 2, "Chest", () =>
            {
                Settings.ChestBones = true;
                FoldersManager.Config.Ini.SetBool("UwUMenu", "Chest", true);
            }, "Disabled", () =>
            {
                Settings.ChestBones = false;
                FoldersManager.Config.Ini.SetBool("UwUMenu", "Chest", false);
            }, "Will make you able to touch all the bones from their chest", null, null, false, Settings.ChestBones);

            Global = new QMToggleButton(ThisMenu, 0, 1, "Global", () =>
            {
                if (Settings.GlobalTouch)
                {
                    GlobalDynamicBones.currentWorldDynamicBoneColliders.Clear();
                    GlobalDynamicBones.currentWorldDynamicBones.Clear();
                    GlobalDynamicBones.ProcessDynamicBones(VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCAvatarManager_0);
                    foreach (var Player in Wrappers.Utils.PlayerManager.GetAllPlayers())
                    {
                        GlobalDynamicBones.Users.Add(Player.prop_APIUser_0.id);
                        GlobalDynamicBones.ProcessDynamicBones(Player._vrcplayer.prop_VRCAvatarManager_0);
                    }
                }
            }, "Disabled", () =>
            {
                GlobalDynamicBones.Users.Clear();
                foreach (var Player in Wrappers.Utils.PlayerManager.AllPlayers())
                {
                    Player.ReloadAvatar();
                }
            }, "You will be able to touch anyone in the room that have dynamic bones. WARNING (This might cause lags and stuttering depending on your pc's performances)", null, null, false, false);
        }
    }
}