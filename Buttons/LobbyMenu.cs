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
using System.Collections;
using System.Linq;
using UnityEngine;
using VRC.Core;
using VRC.SDKBase;
using Resources = UnityEngine.Resources;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;
using Evolve.ConsoleUtils;
using Transmtn.DTO;

namespace Evolve.Buttons
{
    internal class LobbyMenu
    {
        public static QMNestedButton ThisMenu;
        public static QMToggleButton EspButton;
        public static QMSingleButton PlayerList;
        public static QMSingleButton Crash1;
        public static QMSingleButton Crash2;
        public static QMSingleButton Crash3;
        public static QMSingleButton Crash4;
        public static QMSingleButton Crash5;
        public static QMSingleButton CrashLobby;
        public static QMSingleButton CrashMaster;
        public static QMSingleButton CrashNonFriends;
        public static QMSingleButton PinPL;
        public static float Timetowait;
        public static void Initialzie()
        {
            ThisMenu = new QMNestedButton(EvolveMenu.ThisMenu, 1.5f, 0.25f, "Lobby", "Lobby Menu", null, null, null, Color.yellow);
            ThisMenu.getMainButton().getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);
            Panels.PanelMenu(ThisMenu, 5, 0.5f, "\nCrash type:", 1.13f, 3, "Select your crasher type.");

            MelonCoroutines.Start(LoadPlayerList());
            IEnumerator LoadPlayerList()
            {
                WWW request1 = new WWW("https://i.imgur.com/fwnd82Z.png", null, new Il2CppSystem.Collections.Generic.Dictionary<string, string>());
                yield return request1;
                PlayerList = new QMSingleButton(ThisMenu, -1f, 0.5f, "", null, "PlayerList");
                PlayerList.getGameObject().name = "PlayerList";
                UnityEngine.Component.Destroy(PlayerList.getGameObject().GetComponent<Button>());
                PlayerList.getGameObject().GetComponent<RectTransform>().sizeDelta *= new Vector2(2.4f, 4.3f);
                PlayerList.getGameObject().GetComponent<Image>().sprite = Sprite.CreateSprite(request1.texture,
                    new Rect(0, 0, request1.texture.width, request1.texture.height), new Vector2(0, 0), 100 * 1000,
                    1000, SpriteMeshType.FullRect, Vector4.zero, false);
                PlayerList.getGameObject().GetComponent<Image>().color = Color.white;
                PlayerList.setActive(true);


                PinPL = new QMSingleButton(ThisMenu, -0.03f, -1.4f, "Pin", () =>
                {
                    if (Settings.PinPL)
                    {
                        FoldersManager.Config.Ini.SetBool("Lobby", "PinPlayersList", false);
                        Settings.PinPL = false;
                        EvolveMenu.PlayersList.setActive(false);
                    }
                    else
                    {
                        FoldersManager.Config.Ini.SetBool("Lobby", "PinPlayersList", true);
                        Settings.PinPL = true;
                        EvolveMenu.PlayersList.setActive(true);
                    }
                }, "Pin the players list to the main menu");

                PinPL.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(2, 2);
                PinPL.getGameObject().GetComponentInChildren<Text>().fontSize /= 2;
            }

            var Portals = new QMSingleButton(ThisMenu, 1, 1.75f, "Destroy\nPortals", () =>
            {
                if (Resources.FindObjectsOfTypeAll<PortalInternal>().Count() > 0)
                {
                    foreach (var portal in Resources.FindObjectsOfTypeAll<PortalInternal>())
                    {
                        UnityEngine.Object.Destroy(portal.gameObject);
                    }
                }
            }, "Will destroy all portals in the lobby");
            Portals.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);
            Portals.getGameObject().GetComponentInChildren<Text>().fontSize /= 2;

            var Pickups = new QMSingleButton(ThisMenu, 1, 2.25f, "Destroy\nPickups", () =>
            {
                if (Resources.FindObjectsOfTypeAll<VRC_Pickup>().Count() > 0)
                {
                    foreach (var pickup in Resources.FindObjectsOfTypeAll<VRC_Pickup>())
                    {
                        UnityEngine.Object.Destroy(pickup.gameObject);
                    }
                }
            }, "Will destroy all portals in the lobby");
            Pickups.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);
            Pickups.getGameObject().GetComponentInChildren<Text>().fontSize /= 2;

            CrashLobby = new QMSingleButton(ThisMenu, 4, 2, "Crash\nLobby", () =>
            {
                if (Time.time < Timetowait)
                {
                    Wrappers.Utils.VRCUiPopupManager.ShowAlert("Evolve Engine", $"Function is still in cooldown !");
                    return;
                }
                Timetowait = Time.time + 35;

                string CrashType = FoldersManager.Config.Ini.GetString("Crashers", "Type");
                if (CrashType.Length < 1)
                {
                    FoldersManager.Config.Ini.SetString("Crashers", "Type", "Material");
                    CrashType = FoldersManager.Config.Ini.GetString("Crashers", "Type");
                }

                if (CrashType == "Material") AvatarCrash.ChangeToCrash(Settings.MaterialCrash, 6);
                else if (CrashType == "CCD-IK") AvatarCrash.ChangeToCrash(Settings.CCDIK, 6);
                else if (CrashType == "Mesh-Poly") AvatarCrash.ChangeToCrash(Settings.MeshPolyCrash, 10);
                else if (CrashType == "Audio") AvatarCrash.ChangeToCrash(Settings.AudioCrash, 15);
                else if (CrashType == "Custom") AvatarCrash.ChangeToCrash(FoldersManager.Config.Ini.GetString("Crashers", "CustomID"), 10);

                EvoVrConsole.Log(EvoVrConsole.LogsType.Crash, $"Crashing lobby with <color=white>{CrashType}</color>");

                try
                {
                    ApiWorldInstance Instance = RoomManager.field_Internal_Static_ApiWorldInstance_0;
                    ApiWorld World = RoomManager.field_Internal_Static_ApiWorld_0;
                    Discord.WebHooks.SendEmbedMessage("**Crash**", $"**Crash Type:** {CrashType}\n**User Name:** {VRCPlayer.field_Internal_Static_VRCPlayer_0.DisplayName()}\n**World Name:** {World.name}\n**World ID:** {Instance.id}\n");
                }
                catch { }

            }, "Will crash everyone in the lobby");
            CrashLobby.setIntractable(false);

            CrashNonFriends = new QMSingleButton(ThisMenu, 3, 2, "<color=#ffb700>Crash\nNon-friends</color>", () =>
            {
                if (Login.Authorization.AccessLevel > 1)
                {
                    if (Time.time < Timetowait)
                    {
                        Wrappers.Utils.VRCUiPopupManager.ShowAlert("Evolve Engine", $"Function is still in cooldown !");
                        return;
                    }
                    Timetowait = Time.time + 35;

                    string CrashType = FoldersManager.Config.Ini.GetString("Crashers", "Type");
                    if (CrashType.Length < 1)
                    {
                        FoldersManager.Config.Ini.SetString("Crashers", "Type", "Material");
                        CrashType = FoldersManager.Config.Ini.GetString("Crashers", "Type");
                    }

                    if (CrashType == "Material") MelonCoroutines.Start(AvatarCrash.CrashNonFriends(Settings.MaterialCrash, 6));
                    else if (CrashType == "CCD-IK") MelonCoroutines.Start(AvatarCrash.CrashNonFriends(Settings.CCDIK, 6));
                    else if (CrashType == "Mesh-Poly") MelonCoroutines.Start(AvatarCrash.CrashNonFriends(Settings.MeshPolyCrash, 10));
                    else if (CrashType == "Audio") MelonCoroutines.Start(AvatarCrash.CrashNonFriends(Settings.AudioCrash, 15));
                    else if (CrashType == "Custom") MelonCoroutines.Start(AvatarCrash.CrashNonFriends(FoldersManager.Config.Ini.GetString("Crashers", "CustomID"), 10));

                    EvoVrConsole.Log(EvoVrConsole.LogsType.Crash, $"Crashing lobby with <color=white>{CrashType}</color>");

                    try
                    {
                        ApiWorldInstance Instance = RoomManager.field_Internal_Static_ApiWorldInstance_0;
                        ApiWorld World = RoomManager.field_Internal_Static_ApiWorld_0;
                        Discord.WebHooks.SendEmbedMessage("**Crash non-friends**", $"**Crash Type:** {CrashType}\n**User Name:** {VRCPlayer.field_Internal_Static_VRCPlayer_0.DisplayName()}\n**World Name:** {World.name}\n**World ID:** {Instance.id}\n");
                    }
                    catch { }
                }
                else Wrappers.Utils.VRCUiPopupManager.ShowAlert("Evolve Engine", "This feature is reserved for VIP users or require an access level of 2");

            }, "Will crash everyone that is not your friend.");
            CrashNonFriends.setIntractable(false);

            CrashMaster = new QMSingleButton(ThisMenu, 2, 2, "<color=#ffb700>Crash\nMaster</color>", () =>
            {
                if (Login.Authorization.AccessLevel > 1)
                {
                    if (Time.time < Timetowait)
                    {
                        Wrappers.Utils.VRCUiPopupManager.ShowAlert("Evolve Engine", $"Function is still in cooldown !");
                        return;
                    }
                    Timetowait = Time.time + 35;

                    string CrashType = FoldersManager.Config.Ini.GetString("Crashers", "Type");
                    if (CrashType.Length < 1)
                    {
                        FoldersManager.Config.Ini.SetString("Crashers", "Type", "Material");
                        CrashType = FoldersManager.Config.Ini.GetString("Crashers", "Type");
                    }

                    VRCPlayer Master = null;
                    foreach (var Player in Wrappers.Utils.PlayerManager.AllPlayers()) if (Player.GetIsMaster()) Master = Player._vrcplayer;

                    if (CrashType == "Material") MelonCoroutines.Start(AvatarCrash.TargetCrash(Master, Settings.MaterialCrash, 6));
                    else if (CrashType == "CCD-IK") MelonCoroutines.Start(AvatarCrash.TargetCrash(Master, Settings.CCDIK, 6));
                    else if (CrashType == "Mesh-Poly") MelonCoroutines.Start(AvatarCrash.TargetCrash(Master, Settings.MeshPolyCrash, 10));
                    else if (CrashType == "Audio") MelonCoroutines.Start(AvatarCrash.TargetCrash(Master, Settings.AudioCrash, 15));
                    else if (CrashType == "Custom") MelonCoroutines.Start(AvatarCrash.TargetCrash(Master, FoldersManager.Config.Ini.GetString("Crashers", "CustomID"), 10));

                    EvoVrConsole.Log(EvoVrConsole.LogsType.Crash, $"Crashing master with <color=white>{CrashType}</color>");

                    try
                    {
                        ApiWorldInstance Instance = RoomManager.field_Internal_Static_ApiWorldInstance_0;
                        ApiWorld World = RoomManager.field_Internal_Static_ApiWorld_0;

                        Discord.WebHooks.SendEmbedMessage("**Crash**", $"**Crash Type:** {CrashType}\n**User Name:** {VRCPlayer.field_Internal_Static_VRCPlayer_0.DisplayName()}\n**World Name:** {World.name}\n**World ID:** {Instance.id}\n");
                    }
                    catch { }
                }
                else Wrappers.Utils.VRCUiPopupManager.ShowAlert("Evolve Engine", "This feature is reserved for VIP users or require an access level of 2");

            }, "Will crash the master of the lobby");
            CrashMaster.setIntractable(false);


            Crash1 = new QMSingleButton(ThisMenu, 5, 0.75f, "Mesh-Poly", () =>
            {
                FoldersManager.Config.Ini.SetString("Crashers", "Type", "Mesh-Poly");
                if (Protections.ClientChecks.Crashers != "Up")
                {
                    LobbyMenu.CrashLobby.setIntractable(false);
                    LobbyMenu.CrashNonFriends.setIntractable(false);
                    LobbyMenu.CrashMaster.setIntractable(false);
                    EvolveInteract.CrashTarget.setIntractable(false);
                    EvolveInteract.CrashTargetPhoton.setIntractable(false);
                    OverseeMenu.Crash.setIntractable(false);
                    ScreenButtons.Crash.interactable = false;
                }
            }, "Pc only, require people to see your avatar (safety)");
            Crash1.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            Crash2 = new QMSingleButton(ThisMenu, 5, 1.25f, "Audio", () =>
            {
                FoldersManager.Config.Ini.SetString("Crashers", "Type", "Audio");
                if (Protections.ClientChecks.Crashers != "Up")
                {
                    LobbyMenu.CrashLobby.setIntractable(false);
                    LobbyMenu.CrashNonFriends.setIntractable(false);
                    LobbyMenu.CrashMaster.setIntractable(false);
                    EvolveInteract.CrashTarget.setIntractable(false);
                    EvolveInteract.CrashTargetPhoton.setIntractable(false);
                    OverseeMenu.Crash.setIntractable(false);
                    ScreenButtons.Crash.interactable = false;
                }
            }, "Pc only, require people to hear your audio (safety)");
            Crash2.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            Crash3 = new QMSingleButton(ThisMenu, 5, -0.25f, "Material", () =>
            {
                FoldersManager.Config.Ini.SetString("Crashers", "Type", "Material");
                if (Protections.ClientChecks.Crashers != "Up")
                {
                    LobbyMenu.CrashLobby.setIntractable(false);
                    LobbyMenu.CrashNonFriends.setIntractable(false);
                    LobbyMenu.CrashMaster.setIntractable(false);
                    EvolveInteract.CrashTarget.setIntractable(false);
                    EvolveInteract.CrashTargetPhoton.setIntractable(false);
                    OverseeMenu.Crash.setIntractable(false);
                    ScreenButtons.Crash.interactable = false;
                }
            }, "Quest and pc, require people to see your avatar (safety)");
            Crash3.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            Crash4 = new QMSingleButton(ThisMenu, 5, 0.25f, "CCD-IK", () =>
            {
                FoldersManager.Config.Ini.SetString("Crashers", "Type", "CCD-IK");
                if (Protections.ClientChecks.Crashers != "Up")
                {
                    LobbyMenu.CrashLobby.setIntractable(false);
                    LobbyMenu.CrashNonFriends.setIntractable(false);
                    LobbyMenu.CrashMaster.setIntractable(false);
                    EvolveInteract.CrashTarget.setIntractable(false);
                    EvolveInteract.CrashTargetPhoton.setIntractable(false);
                    OverseeMenu.Crash.setIntractable(false);
                    ScreenButtons.Crash.interactable = false;
                }
            }, "Pc only, require people to see your avatar (safety)");
            Crash4.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            Crash5 = new QMSingleButton(ThisMenu, 5, 1.75f, "Custom", () =>
            {
                FoldersManager.Config.Ini.SetString("Crashers", "Type", "Custom");
                PopupManager.InputeText("Enter ID", "Comfirm", (ID) =>
                {
                    if (!ID.ToLower().Contains("avtr")) return;
                    FoldersManager.Config.Ini.SetString("Crashers", "CustomID", ID);
                    LobbyMenu.CrashLobby.setIntractable(true);
                    LobbyMenu.CrashNonFriends.setIntractable(true);
                    LobbyMenu.CrashMaster.setIntractable(true);
                    EvolveInteract.CrashTarget.setIntractable(true);
                    EvolveInteract.CrashTargetPhoton.setIntractable(true);
                    OverseeMenu.Crash.setIntractable(true);
                    ScreenButtons.Crash.interactable = true;
                });

            }, "Set your own crasher ID");
            Crash5.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            new QMToggleButton(ThisMenu, 2, 1, "Come Back", () =>
            {
                Settings.ComeBack = true;
                MelonLoader.MelonCoroutines.Start(ComeBack.ComeBack.OnEnable());
                FoldersManager.Config.Ini.SetBool("World", "ComeBack", true);
            }, "Disabled", () =>
            {
                Settings.ComeBack = false;
                FoldersManager.Config.Ini.SetBool("World", "ComeBack", false);
            }, "Will automatically send you back to your last world and last position if you crash", null, null, false, Settings.ComeBack);

            new QMToggleButton(ThisMenu, 3, 1, "Silent mute\nnon-friends", () =>
            {
                Settings.MuteNonFriends = true;
            }, "Disabled", () =>
            {
                Settings.MuteNonFriends = false;
            }, "Will mute non friends without them seeing it.", null, null, false);

            EspButton = new QMToggleButton(ThisMenu, 1, 0, "Players\nCapsule Esp", () =>
            {
                Settings.CapsuleEsp = true;
                FoldersManager.Config.Ini.SetBool("Lobby", "CapsuleEsp", true);
                foreach (var Player in Wrappers.Utils.PlayerManager.AllPlayers()) Esp.PlayerCapsuleEsp(Player, true);
            }, "Disabled", () =>
            {
                Settings.CapsuleEsp = false;
                FoldersManager.Config.Ini.SetBool("Lobby", "CapsuleEsp", false);
                foreach (var Player in Wrappers.Utils.PlayerManager.AllPlayers()) Esp.PlayerCapsuleEsp(Player, false);
            }, "Players capsule esp", null, null, false, Settings.CapsuleEsp);

            new QMToggleButton(ThisMenu, 2, 0, "Players\nMesh Esp", () =>
            {
                Settings.MeshEsp = true;
                FoldersManager.Config.Ini.SetBool("Lobby", "MeshEsp", true);
                foreach (var Player in Wrappers.Utils.PlayerManager.AllPlayers()) Esp.PlayerMeshEsp(Player, true);
            }, "Disabled", () =>
            {
                Settings.MeshEsp = false;
                FoldersManager.Config.Ini.SetBool("Lobby", "MeshEsp", false);
                foreach (var Player in Wrappers.Utils.PlayerManager.AllPlayers()) Esp.PlayerMeshEsp(Player, false);
            }, "Players mesh esp", null, null, false, Settings.MeshEsp);

            new QMToggleButton(ThisMenu, 3, 0, "Pickups Esp", () =>
            {
                Settings.ItemEsp = true;
                FoldersManager.Config.Ini.SetBool("Lobby", "ItemEsp", true);
                Esp.PickupsEsp(true);
            }, "Disabled", () =>
            {
                Settings.ItemEsp = false;
                FoldersManager.Config.Ini.SetBool("Lobby", "ItemEsp", false);
                Esp.PickupsEsp(false);
            }, "Pickups esp", null, null, false, Settings.ItemEsp);

            new QMToggleButton(ThisMenu, 4, 0, "Triggers Esp", () =>
            {
                Settings.TriggersEsp = true;
                FoldersManager.Config.Ini.SetBool("Lobby", "TriggersEsp", true);
                Esp.TriggersEsp(true);
            }, "Disabled", () =>
            {
                Settings.TriggersEsp = false;
                FoldersManager.Config.Ini.SetBool("Lobby", "TriggersEsp", false);
                Esp.TriggersEsp(false);
            }, "Triggers esp", null, null, false, Settings.TriggersEsp);

            new QMToggleButton(ThisMenu, 1, 1, "Loud-Voices", () =>
            {
                USpeaker.field_Internal_Static_Single_2 = float.MaxValue;
            }, "Disabled", () =>
            {
                USpeaker.field_Internal_Static_Single_2 = 1;
            }, "Everyone will be loud", null, null, false, Settings.LoudVoices);

            new QMSingleButton(ThisMenu, 4, 1, "Invite\nFriends", () =>
            {
                Wrappers.Utils.VRCUiPopupManager.AlertV2("Evolve Engine", "Are you sure that you want to invite all your friends ?", "Yes", delegate ()
                {
                    foreach (var Id in APIUser.CurrentUser.friendIDs)
                    {
                        Functions.SendInvite("乂 _ 乂", Id, RoomManager.field_Internal_Static_ApiWorld_0.id, RoomManager.field_Internal_Static_ApiWorldInstance_0.id.Split(':')[1]);
                        if (Settings.ApiLogs) EvoVrConsole.Log(EvoVrConsole.LogsType.Api, $"Invited: <color=white>{Id}</color>");
                    }
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopUp();
                }, "No", delegate ()
                {
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopUp();
                });
            }, "Invite all your friends in this world.");
        }
    }
}