using Evolve.Api;
using Evolve.Exploits;
using Evolve.Modules;
using Evolve.Modules.AvatarHider;
using Evolve.Modules.NearClippingPlaneAdjuster;
using Evolve.Modules.PortableMirror;
using Evolve.Modules.PostProcessing;
using Evolve.Utils;
using Evolve.Wrappers;
using MelonLoader;
using ButtonApi;
using System;
using System.Linq;
using Evolve.ConsoleUtils;
using UnityEngine;
using VRC;
using VRC.Core;
using VRC.Networking;
using VRCSDK2;
using static VRC.SDKBase.VRC_EventHandler;
using Resources = UnityEngine.Resources;
using VRC_EventHandler = VRC.SDKBase.VRC_EventHandler;
using System.Collections.Generic;
using System.Diagnostics;
using UnhollowerBaseLib;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon;
using VRTK;
using Object = Il2CppSystem.Object;
using VRC_Pickup = VRC.SDKBase.VRC_Pickup;
using System.Collections;
using MelonCoroutines = MelonLoader.MelonCoroutines;
using UnityEngine.UI;
using System.Net;

namespace Evolve.Buttons
{
    internal class WorldMenu
    {
        public static QMNestedButton Club24VIP;
        public static QMNestedButton JustBClub;
        public static QMNestedButton FreezeTag;
        public static QMNestedButton ThisMenu;
        public static QMNestedButton Murder23;
        public static QMNestedButton Murder4;
        public static QMNestedButton Murder4Page2;
        public static QMNestedButton AmongUs;
        public static QMToggleButton TogglePostProcessing;
        public static QMSingleButton Single1;
        public static QMSingleButton Single2;
        public static QMSingleButton Single3;
        public static QMNestedButton AmongUsPage2;
        public static QMSingleButton Single4;
        public static float ReuploadCD;
        public static bool AppliedChanges = false;
        private static List<QMNestedButton> AllMenues = new List<QMNestedButton>();
        public static List<QMToggleButton> AllToggles = new List<QMToggleButton>();
        public static IEnumerator CheckWorld()
        {
            while (RoomManager.field_Internal_Static_ApiWorld_0 == null) yield return null;
            {
                var ApiWorld = RoomManager.field_Internal_Static_ApiWorld_0;
                try
                {
                    foreach (var Menu in AllMenues)
                    {
                        Menu.getMainButton().setIntractable(false);
                        var MenuObject = GameObject.Find(Menu.getMenuName());
                    }

                    if (ApiWorld.id == "wrld_858dfdfc-1b48-4e1e-8a43-f0edc611e5fe") Murder4.getMainButton().setIntractable(true);
                    if (ApiWorld.id == "wrld_726c8a44-8222-4858-8b33-49e70d495b62" || ApiWorld.id == "wrld_ccbf8103-c23e-472f-8efb-38a9a9164357") Murder23.getMainButton().setIntractable(true);
                    if (ApiWorld.id == "wrld_dd036610-a246-4f52-bf01-9d7cea3405d7") AmongUs.getMainButton().setIntractable(true);
                    if (ApiWorld.id == "wrld_7487d91a-3ef4-44c6-ad6d-9bdc7dee5efd") FreezeTag.getMainButton().setIntractable(true);
                    //if (ApiWorld.id == "wrld_1b3b3259-0a1f-4311-984e-826abab6f481") JustBClub.getMainButton().setIntractable(true);
                    if (ApiWorld.id == "wrld_e6350d59-8c1e-48ff-ba97-f3ff283bc1b1")
                    {
                        Club24VIP.getMainButton().setIntractable(true);
                        /*if (!AppliedChanges)
                        {
                            MelonCoroutines.Start(Delayed());

                            IEnumerator Delayed()
                            {
                                UnityEngine.Object.Destroy(GameObject.Find("neon girl"));
                                var EvolveLogo = new GameObject("Evolved");
                                EvolveLogo.transform.position = new Vector3(1.942403f, 5.229344f, 0.4550986f);
                                EvolveLogo.transform.rotation = new Quaternion(0, 90, 0, 0);
                                EvolveLogo.transform.localScale = new Vector3(2, 2, 2);
                                EvolveLogo.AddComponent<SpriteRenderer>();
                                WWW www = new WWW("https://i.imgur.com/6lUyX9W.png", null, new Il2CppSystem.Collections.Generic.Dictionary<string, string>());
                                yield return www;
                                EvolveLogo.GetComponent<SpriteRenderer>().sprite = Sprite.CreateSprite(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0), 100 * 1000, 1000, SpriteMeshType.FullRect, Vector4.zero, false);
                                AppliedChanges = true;
                            }
                        }*/
                    }

                    foreach (var ToggleButton in AllToggles) ToggleButton.setToggleState(false, true);
                    Settings.UdonExploitProtections = ProtectionsMenu.UdonProt.btnOn.active;
                }
                catch { }
            }
        }

        public static void Initialize()
        {
            ThisMenu = new QMNestedButton(EvolveMenu.ThisMenu, 3.5f, 0.25f, "Worlds", "Worlds Menu", null, null, null, Color.yellow);
            ThisMenu.getMainButton().getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);
            Panels.PanelMenu(ThisMenu, 0, 0.89f, "\nWorld Options", 1.1f, 3.5f, "World Options");

           /* new QMSingleButton(ThisMenu, 5, 0, "Reupload\nworld", () =>
            {
                if (Login.Authorization.AccessLevel > 1)
                {
                    if (Time.time < ReuploadCD)
                    {
                        Wrappers.Utils.VRCUiPopupManager.ShowAlert("Evolve Engine", $"Function is still in cooldown !");
                        return;
                    }
                    ReuploadCD = Time.time + 60;

                    Yoink.Yoinker.ReuploadWorld(RoomManager.field_Internal_Static_ApiWorld_0.id);
                }
                else Wrappers.Utils.VRCUiPopupManager.ShowAlert("Evolve Engine", "This feature is reserved for VIP users or require an access level of 2");
            }, "Will reupload the avatar your are in on your account"); */

            TogglePostProcessing = new QMToggleButton(ThisMenu, 0, 2, "Post Processing", new Action(() =>
            {
                PostProcessing.TogglePostProcessing(true);
            }), "Disabled", new Action(() =>
            {
                PostProcessing.TogglePostProcessing(false);
            }), "Toggle Post Processing", null, null, false, true);

            Single1 = new QMSingleButton(ThisMenu, 0, -0.25f, "Rejoin", () =>
            {
                ApiWorldInstance Room = RoomManager.field_Internal_Static_ApiWorldInstance_0;
                Functions.ForceJoin(Room.id);
            }, "Rejoin current world");
            Single1.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            Single2 = new QMSingleButton(ThisMenu, 0, 0.25f, "Join ID", () =>
            {
                PopupManager.InputeText("Evolve Engine", "Confirm", new Action<string>((a) =>
                {
                    Functions.ForceJoin(a);
                }));
            }, "Force join a world with it's ID");
            Single2.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            Single3 = new QMSingleButton(ThisMenu, 0, 0.75f, "Copy ID", () =>
            {
                ApiWorldInstance Room = RoomManager.field_Internal_Static_ApiWorldInstance_0;
                Functions.copytoclip(Room.id);
            }, "Copy the current world ID");
            Single3.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            Single4 = new QMSingleButton(ThisMenu, 0, 1.25f, "Download vrcw", () =>
            {
                ApiWorld Room = RoomManager.field_Internal_Static_ApiWorld_0;
                Process.Start(Room.assetUrl);
            }, "Copy the current world ID");
            Single4.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);


            new QMSingleButton(ThisMenu, 5, 0.55f, "Drop portal\nto ID", () =>
            {

                PopupManager.InputeText("Enter the world id", "Confirm", new Action<string>((a) =>
                {
                    var WorldID = a.Split(':')[0];
                    var InstanceID = a.Split(':')[1];
                    Functions.DropPortal(WorldID, InstanceID, 0, VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position + VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.forward * 1.5f, Quaternion.identity);
                }));

            }, "Will drop a portal to a world id");

            new QMSingleButton(ThisMenu, 5, 1.55f, "Respawn\nPickups", () =>
            {
                foreach (var Pickup in Exploits.Exploits.AllPickups)
                {
                    Functions.TakeOwnershipIfNecessary(Pickup.gameObject);
                    Pickup.transform.position = new Vector3(0, -99, 0);
                }
                foreach (var Pickup in Exploits.Exploits.AllUdonPickups)
                {
                    Functions.TakeOwnershipIfNecessary(Pickup.gameObject);
                    Pickup.transform.position = new Vector3(0, -99, 0);
                }
                foreach (var Pickup in Exploits.Exploits.AllSyncPickups)
                {
                    Functions.TakeOwnershipIfNecessary(Pickup.gameObject);
                    Pickup.transform.position = new Vector3(0, -99, 0);
                }
            }, "Will respawn all the pickups");


            Murder23 = new QMNestedButton(ThisMenu, 1, 0, "Murder 2-3", "Murder 2-3", null, null, null, Color.yellow);
            Worlds.Murder23.Initialize();
            AllMenues.Add(Murder23);

            Murder4 = new QMNestedButton(ThisMenu, 2, 0, "Murder 4", "Murder 4", null, null, null, Color.yellow);
            AllMenues.Add(Murder4);
            Murder4Page2 = new QMNestedButton(Murder4, 5, 1.75f, "Page 2", "Murder 4", null, null, null, Color.yellow);
            Murder4Page2.getMainButton().getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);
            Worlds.Murder4.Initialize();
           

            AmongUs = new QMNestedButton(ThisMenu, 3, 0, "Among Us", "Among Us", null, null, null, Color.yellow);
            AmongUsPage2 = new QMNestedButton(AmongUs, 5, 1.75f, "Page 2", "Among Us", null, null, null, Color.yellow);
            AmongUsPage2.getMainButton().getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);
            Worlds.AmongUs.Initialize();
            AllMenues.Add(AmongUs);

            FreezeTag= new QMNestedButton(ThisMenu, 4, 0, "Freeze Tag", "Freeze Tag", null, null, null, Color.yellow);
            Worlds.FreezeTag.Initialize();
            AllMenues.Add(FreezeTag);

            JustBClub = new QMNestedButton(ThisMenu, 1, 1, "JustBClub\n(WIP)", "JustBClub", null, null, null, Color.yellow);
            Worlds.JustBClub.Initialize();
            AllMenues.Add(JustBClub);

            Club24VIP = new QMNestedButton(ThisMenu, 2, 1, "Club24 + VIP", "Club24 + VIP", null, null, null, Color.yellow);
            Worlds.Club24VIP.Initialize();
            AllMenues.Add(Club24VIP);
        }
    }
}