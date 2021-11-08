using System;
using System.Collections;
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
    internal class TouchInteractMenu
    {
        public static QMNestedButton ThisMenu;
        public static QMSingleButton OpenUwUMenu;
        public static QMToggleButton Touch;

        public static IEnumerator Loop()
        {
            for (; ; )
            {
                yield return new WaitForSeconds(0.3f);
                try
                {
                    if (Touch.getGameObject().active == true)
                    {
                        var Player = Wrappers.Utils.QuickMenu.SelectedVRCPlayer();
                        if (Player != null)
                        {
                            Touch.setOnText($"Touch: {Player.DisplayName()}");
                            if (GlobalDynamicBones.Users.Contains(Player.prop_Player_0.prop_APIUser_0.id))
                            {
                                Touch.setToggleState(true);
                            }
                            else Touch.setToggleState(false);
                        }
                    }
                }
                catch
                {
                }
                yield return new WaitForEndOfFrame();
            }
        }
        public static void Initialize()
        {
            ThisMenu = new QMNestedButton("UserInteractMenu", 3, 3, "UwU", "UwUMenu", Color.cyan, Color.magenta, Color.black, Color.yellow);
            ThisMenu.getMainButton().getGameObject().SetActive(false);
            Panels.PanelMenu(ThisMenu, 0, 0, "\nTouch \nController", 1.1f, 2, "Touch Controller");

            MelonCoroutines.Start(ReworkedButtonAPI.CreateButton(MenuType.UserInteract, ButtonType.Single, "", 3, 3, delegate ()
            {
                QMStuff.ShowQuickmenuPage(ThisMenu.getMenuName());
            }, "UwU Menu", Color.black, Color.clear, null, "https://i.imgur.com/6pPJHRe.png"));



            Touch = new QMToggleButton(ThisMenu, 0, 0, "Touch: ", () =>
            {

                if (Settings.GlobalTouch)
                {
                    var Player = Wrappers.Utils.QuickMenu.SelectedVRCPlayer();
                    GlobalDynamicBones.currentWorldDynamicBoneColliders.Clear();
                    GlobalDynamicBones.currentWorldDynamicBones.Clear();
                    GlobalDynamicBones.ProcessDynamicBones(VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCAvatarManager_0);
                    GlobalDynamicBones.Users.Add(Player._player.prop_APIUser_0.id);
                    GlobalDynamicBones.ProcessDynamicBones(Player.prop_VRCAvatarManager_0);
                }
                else
                {
                    Touch.setToggleState(false, true);
                    Wrappers.Utils.VRCUiPopupManager.ShowAlert("Evolve Engine", "Touch is currently disabled, to enable it go into the touch menu.");
                }
            }, "Disabled", () =>
            {
                var Player = Wrappers.Utils.QuickMenu.SelectedVRCPlayer();
                GlobalDynamicBones.Users.Remove(Player.prop_Player_0.prop_APIUser_0.id);
                if (Settings.GlobalTouch) Player.ReloadAvatar();
            }, "Interact with the selected player dynamic bones if Touch is on");

            OpenUwUMenu = new QMSingleButton(ThisMenu, 0, 0.75f, "Touch Menu", () =>
            {
                QMStuff.ShowQuickmenuPage(TouchMenu.ThisMenu.getMenuName());
            }, "Open the touch menu");
            OpenUwUMenu.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            MelonCoroutines.Start(Loop());

            new QMSingleButton(ThisMenu, 1, 0, "Attach to\nplayer's head", () =>
            {
                var Player = Wrappers.Utils.QuickMenu.SelectedVRCPlayer();
                Settings.AttachToPlayer = true;
                MelonCoroutines.Start(SitOnHead.Attach(Player));
            }, "Attach to the player's head");

            new QMSingleButton(ThisMenu, 2, 0, "Attach to\nplayer's hand", () =>
            {
                var Player = Wrappers.Utils.QuickMenu.SelectedVRCPlayer();
                Settings.AttachToPlayerHand = true;
                MelonCoroutines.Start(SitOnHead.AttachHand(Player));
            }, "Attach to the player's head");

            new QMSingleButton(ThisMenu, 3, 0, "Lewdify", () =>
            {
                Lewd.Lewdify(Wrappers.Utils.QuickMenu.SelectedVRCPlayer().GetAvatar(), true);
            }, "Lewdify works only on 2.0 avatars");
        }
    }
}