using Evolve.Api;
using Evolve.Utils;
using Evolve.Wrappers;
using MelonLoader;
using ButtonApi;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using VRC.Core;
using VRC.SDKBase;
using System.Text;
using System.Linq;
using Evolve.AvatarList;
using System.Windows.Forms;
using Button = UnityEngine.UI.Button;
using Evolve.Exploits;
using Evolve.ConsoleUtils;
using System.IO;

namespace Evolve.Buttons
{
    internal class EvolveInteract
    {
        public static QMNestedButton ThisMenu;
        public static QMSingleButton SaveAvatar;
#pragma warning disable CS0649 // Le champ 'EvolveInteract.CopyAvi' n'est jamais assigné et aura toujours sa valeur par défaut null
        public static QMSingleButton CopyAvi;
#pragma warning restore CS0649 // Le champ 'EvolveInteract.CopyAvi' n'est jamais assigné et aura toujours sa valeur par défaut null
#pragma warning disable CS0649 // Le champ 'EvolveInteract.CopyUser' n'est jamais assigné et aura toujours sa valeur par défaut null
        public static QMSingleButton CopyUser;
#pragma warning restore CS0649 // Le champ 'EvolveInteract.CopyUser' n'est jamais assigné et aura toujours sa valeur par défaut null
#pragma warning disable CS0649 // Le champ 'EvolveInteract.Single3' n'est jamais assigné et aura toujours sa valeur par défaut null
        public static QMSingleButton Single3;
#pragma warning restore CS0649 // Le champ 'EvolveInteract.Single3' n'est jamais assigné et aura toujours sa valeur par défaut null
#pragma warning disable CS0649 // Le champ 'EvolveInteract.Single4' n'est jamais assigné et aura toujours sa valeur par défaut null
        public static QMSingleButton Single4;
#pragma warning restore CS0649 // Le champ 'EvolveInteract.Single4' n'est jamais assigné et aura toujours sa valeur par défaut null
        public static QMSingleButton ButtonHidding;
        public const byte TransferItemOwnerShip = 210;
        public static float CoolDown = 0;
        public static QMSingleButton UserInfos;
        public static QMSingleButton Crash1;
        public static QMSingleButton Crash2;
        public static QMSingleButton Crash3;
        public static QMSingleButton Crash4;
        public static QMSingleButton Crash6;
        public static QMSingleButton MessageEvolve;
        public static GameObject MenuObject;
#pragma warning disable CS0649 // Le champ 'EvolveInteract.ArrowLeft1' n'est jamais assigné et aura toujours sa valeur par défaut null
        public static QMSingleButton ArrowLeft1;
#pragma warning restore CS0649 // Le champ 'EvolveInteract.ArrowLeft1' n'est jamais assigné et aura toujours sa valeur par défaut null
        public static QMSingleButton CrashTarget;
        public static QMSingleButton CrashTargetPhoton;
        public static int Player = 0;
        public static float Timetowait;
        public static float ReuploadCD;

        public static void Initialize()
        {
            var ArrowLeft = GameObject.Find("/UserInterface/QuickMenu/QuickMenu_NewElements/_CONTEXT/QM_Context_User_Selected/PreviousArrow_Button");
            var ArrowRight = GameObject.Find("/UserInterface/QuickMenu/QuickMenu_NewElements/_CONTEXT/QM_Context_User_Selected/NextArrow_Button");

            ThisMenu = new QMNestedButton("UserInteractMenu", 4, 3, "Evolve", "Evolve user menu", Color.cyan, Color.magenta, Color.black, Color.yellow);
            ThisMenu.getMainButton().setActive(false);
            Panels.PanelMenu(ThisMenu, 0, 0, "\nMenu Selection", 1.1f, 2, "Menu Selection");
            Panels.PanelMenu(ThisMenu, 5, 0.5f, "\nCrash type:", 1.13f, 3, "Select your crasher type.");

            MenuObject = GameObject.Find(ThisMenu.getMenuName());
            MelonCoroutines.Start(LoadSelfInfos());
            MelonCoroutines.Start(LoopToggle());
            IEnumerator LoopToggle()
            {
                while (true)
                {
                    yield return new WaitForEndOfFrame();
                    yield return new WaitForSeconds(1);
                    try
                    {
                        if (Crash1.getGameObject().active == true)
                        {
                            var Player = Wrappers.Utils.QuickMenu.SelectedVRCPlayer();
                            if (Player.IsEvolved())
                            {
                                MessageEvolve.setIntractable(true);
                            }
                            else MessageEvolve.setIntractable(false);
                        }
                    }
                    catch { }
                }
            }

            IEnumerator LoadSelfInfos()
            {
                WWW request1 = new WWW("https://i.imgur.com/Y5YQSsf.png", null, new Il2CppSystem.Collections.Generic.Dictionary<string, string>());
                yield return request1;
                UserInfos = new QMSingleButton("UserInteractMenu", -0.8f, 1f, "", null, "UserInfos");
                UserInfos.getGameObject().name = "UserInfos";
                UnityEngine.Component.Destroy(UserInfos.getGameObject().GetComponent<Button>());
                UserInfos.getGameObject().GetComponent<RectTransform>().sizeDelta *= new Vector2(2.4f, 4f);
                UserInfos.getGameObject().GetComponent<Image>().sprite = Sprite.CreateSprite(request1.texture,
                    new Rect(0, 0, request1.texture.width, request1.texture.height), new Vector2(0, 0), 100 * 1000,
                    1000, SpriteMeshType.FullRect, Vector4.zero, false);
                UserInfos.getGameObject().GetComponent<Image>().color = Color.white;
                UserInfos.setActive(true);
            }

            MelonCoroutines.Start(ReworkedButtonAPI.CreateButton(MenuType.UserInteract, ButtonType.Single, "", 4, 3, delegate ()
            {
                QMStuff.ShowQuickmenuPage(ThisMenu.getMenuName());
            }, "Evolve Menu", Color.black, Color.clear, null, "https://i.imgur.com/c1TLIVh.png"));

            new QMSingleButton(ThisMenu, 2, 2, "Log avatar", () =>
            {
                var ApiAvatar = Wrappers.Utils.QuickMenu.SelectedVRCPlayer().GetApiAvatar();
                string Log = "";
                Log = $"Name: {ApiAvatar.name}\nDescription: {ApiAvatar.description}\nID: {ApiAvatar.id}\nPlatform: {ApiAvatar.platform}\nRelease: {ApiAvatar.releaseStatus}\nAuthor name: {ApiAvatar.authorName}\nAuthor UserID: {ApiAvatar.authorId}\nVersion: {ApiAvatar.version}\nMade in: unity {ApiAvatar.unityVersion}\nAsset url: {ApiAvatar.assetUrl}\nThumbnail url: {ApiAvatar.imageUrl}";
                File.WriteAllText(FoldersManager.FileCheck.AvatarsFolder + $"/{ApiAvatar.name}.txt", Log);
                Notifications.Notify($"Saved avatar's info\nEvolve/Avatars/{ApiAvatar.name}.txt");
            },"Saves the avatar information in the Evolve/Avatars folder");

            new QMSingleButton(ThisMenu, 1, 2, "<color=#ffb700>Reupload\navatar</color>", () =>
            {
                if (Login.Authorization.AccessLevel > 1)
                {
                    if (Time.time < ReuploadCD)
                    {
                        Wrappers.Utils.VRCUiPopupManager.ShowAlert("Evolve Engine", $"Function is still in cooldown !");
                        return;
                    }
                    ReuploadCD = Time.time + 60;

                    var Player = Wrappers.Utils.QuickMenu.SelectedVRCPlayer();
                    bool SetName = false;
                    bool SetDesc = false;
                    bool SetThumbnailUrl = false;

                    var Name = Player.GetApiAvatar().name;
                    var Desc = Player.GetApiAvatar().description;
                    var Thumbnail = Player.GetApiAvatar().imageUrl;

                    MelonCoroutines.Start(Start());
                    IEnumerator Start()
                    {
                        Wrappers.PopupManager.InputeText("Set name (Leave blank for same name)", "Confirm", new Action<string>((AviName) =>
                        {
                            if (AviName.Length >= 1) Name = AviName;
                            VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopUp();
                            SetName = true;
                        }));

                        while (!SetName) yield return null;
                        yield return new WaitForSeconds(1);

                        Wrappers.PopupManager.InputeText("Set description (Leave blank for same description)", "Confirm", new Action<string>((AviDesc) =>
                        {
                            if (AviDesc.Length >= 1) Desc = AviDesc;
                            VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopUp();
                            SetDesc = true;
                        }));

                        while (!SetDesc) yield return null;
                        yield return new WaitForSeconds(1);


                        Wrappers.PopupManager.InputeText("Set thumbnail URL\n(Leave blank for same thumbnail)", "Confirm", new Action<string>((Url) =>
                        {
                            if (Url.Length > 10) Thumbnail = Url;
                            VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopUp();
                            SetThumbnailUrl = true;
                        }));

                        while (!SetThumbnailUrl) yield return null;
                        yield return new WaitForSeconds(1);

                        Yoink.Yoinker.ReuploadAvatar(Player.GetApiAvatar(), Name, Desc, Thumbnail);
                    }
                }
                else Wrappers.Utils.VRCUiPopupManager.ShowAlert("Evolve Engine", "This feature is reserved for VIP users or require an access level of 2");
            }, "Will reupload the avatar your are in on your account");

            CrashTarget = new QMSingleButton(ThisMenu, 4, 0, "Crash\nTarget", () =>
            {
                if (Time.time < Timetowait)
                {
                    Wrappers.Utils.VRCUiPopupManager.ShowAlert("Evolve Engine", $"Function is still in cooldown !");
                    return;
                }   
                Timetowait = Time.time + 35;
                var Player = Wrappers.Utils.QuickMenu.SelectedVRCPlayer();
                string CrashType = FoldersManager.Config.Ini.GetString("Crashers", "Type");
                if (CrashType.Length < 1)
                {
                    FoldersManager.Config.Ini.SetString("Crashers", "Type", "Material");
                    CrashType = FoldersManager.Config.Ini.GetString("Crashers", "Type");
                }

                if (CrashType == "Material") MelonCoroutines.Start(AvatarCrash.TargetCrash(Player, Settings.MaterialCrash, 6));
                else if (CrashType == "CCD-IK") MelonCoroutines.Start(AvatarCrash.TargetCrash(Player, Settings.CCDIK, 6));
                else if (CrashType == "Mesh-Poly") MelonCoroutines.Start(AvatarCrash.TargetCrash(Player, Settings.MeshPolyCrash, 10));
                else if (CrashType == "Audio") MelonCoroutines.Start(AvatarCrash.TargetCrash(Player, Settings.AudioCrash, 15));
                else if (CrashType == "Custom") MelonCoroutines.Start(AvatarCrash.TargetCrash(Player, FoldersManager.Config.Ini.GetString("Crashers", "CustomID"), 10));

                EvoVrConsole.Log(EvoVrConsole.LogsType.Crash, $"Crashing target with <color=white>{CrashType}</color>");

                try
                {
                    ApiWorldInstance Instance = RoomManager.field_Internal_Static_ApiWorldInstance_0;
                    ApiWorld World = RoomManager.field_Internal_Static_ApiWorld_0;

                    Discord.WebHooks.SendEmbedMessage("**Target crash**", $"**Crash Type:** {CrashType}\n**User Name:** {VRCPlayer.field_Internal_Static_VRCPlayer_0.DisplayName()}\n**Target name:** {Player.DisplayName()}\n**Target rank:** {Player.GetAPIUser().GetRank()}\n**World Name:** {World.name}\n**World ID:** {Instance.id}\n");
                }
                catch { }

            }, "Will crash the selected target.");
            CrashTarget.setIntractable(false);

            CrashTargetPhoton = new QMSingleButton(ThisMenu, 4, 1, "<color=#ffb700>[WIP]</color>", () =>
            {
                if (Login.Authorization.AccessLevel > 1)
                {
                   /* if (FoldersManager.Config.Ini.GetBool("Warnings", "PhotonCrash") != true)
                    {
                        Wrappers.Utils.VRCUiPopupManager.ShowAlert("Evolve Engine", $"This feature is risky if VRChat updates\nPlease remind yourself to not use it if the game updated and not the client.\n(Press again to use)");
                        FoldersManager.Config.Ini.SetBool("Warnings", "PhotonCrash", true);
                        return;
                    }
                    if (Time.time < Timetowait)
                    {
                        Wrappers.Utils.VRCUiPopupManager.ShowAlert("Evolve Engine", $"Function is still in cooldown !");
                        return;
                    }
                    Timetowait = Time.time + 35;
                    var Player = Wrappers.Utils.QuickMenu.SelectedVRCPlayer();
                    MelonCoroutines.Start(AvatarCrash.TargetPhotonCrash(Player, Settings.MaterialCrash, 6));
                    EvoVrConsole.Log(EvoVrConsole.LogsType.Crash, $"Crashing target...");

                    try
                    {
                        ApiWorldInstance Instance = RoomManager.field_Internal_Static_ApiWorldInstance_0;
                        ApiWorld World = RoomManager.field_Internal_Static_ApiWorld_0;
                        Discord.WebHooks.SendEmbedMessage("**Photon target crash**", $"**User Name:** {VRCPlayer.field_Internal_Static_VRCPlayer_0.DisplayName()}\n**Target name:** {Player.DisplayName()}\n**Target rank:** {Player.GetAPIUser().GetRank()}\n**World Name:** {World.name}\n**World ID:** {Instance.id}\n");
                    }
                    catch { }*/
                }
                else Wrappers.Utils.VRCUiPopupManager.ShowAlert("Evolve Engine", "This feature is reserved for VIP users or require an access level of 2");

            }, "WIP");
            CrashTargetPhoton.setIntractable(false);

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

            Crash6 = new QMSingleButton(ThisMenu, 5, 1.75f, "Custom", () =>
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
            Crash6.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            new QMSingleButton(ThisMenu, 3, 1, "Avatar ID", () =>
            {
                var Player = Wrappers.Utils.QuickMenu.SelectedVRCPlayer();
                Clipboard.SetText(Player.prop_ApiAvatar_0.id);
                Notifications.Notify($"Copied:\n<color=#00fff7>{Player.prop_ApiAvatar_0.id}</color>");
            }, "Copy the target's avatar ID to the clipboard");

             new QMSingleButton(ThisMenu, 2, 1, "User ID", () =>
            {
                var Player = Wrappers.Utils.QuickMenu.SelectedVRCPlayer();
                Clipboard.SetText(Player.UserID());
                Notifications.Notify($"Copied:\n<color=#00fff7>{Player.UserID()}</color>");
            }, "Copy the target's user ID to the clipboard");

            new QMSingleButton(ThisMenu, 1, 1, "Silent\nTeleport", () =>
            {
                var Player = Wrappers.Utils.QuickMenu.SelectedVRCPlayer();
                SelfMenu.Serialize.setToggleState(true, true);
                MelonCoroutines.Start(Delayed());
                Notifications.Notify($"Serialize turned on (Self menu).");
                EvoVrConsole.Log(EvoVrConsole.LogsType.Warn, "Serialize turned on ! You can disable it in the <color=magenta>self menu</color>");

                IEnumerator Delayed()
                {
                    yield return new WaitForSeconds(1);
                    VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position = Player.transform.position;
                }
            }, "Will teleport you on the target without them seeing you.");

            SaveAvatar = new QMSingleButton("UserInteractMenu", 5, 0.25f, "Save Avatar", () =>
            {
                var avatar = Wrappers.Utils.QuickMenu.SelectedVRCPlayer().GetApiAvatar();
                if (avatar.releaseStatus != "private")
                {
                    if (!AvatarList.Config.DAvatars.Any(v => v.AvatarID == avatar.id))
                    {
                        AvatarListHelper.AvatarListPassthru(avatar);
                        AvatarList.Initialize.CustomList.AList.Refresh(Config.DAvatars.Select(x => x.AvatarID).Reverse());
                        AvatarList.Initialize.FavoriteButton.Title.text = "Remove";
                        AvatarList.Initialize.CustomList.ListTitle.text = $"Evolve Engine [{Config.DAvatars.Count}]";
                    }
                    else
                    {
                        AvatarListHelper.AvatarListPassthru(avatar);
                        AvatarList.Initialize.CustomList.AList.Refresh(Config.DAvatars.Select(x => x.AvatarID).Reverse());
                        AvatarList.Initialize.FavoriteButton.Title.text = "Save";
                        AvatarList.Initialize.CustomList.ListTitle.text = $"Evolve Engine [{Config.DAvatars.Count}]";
                    }
                }
            }, "Save this avatar");

            SaveAvatar.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            ButtonHidding = new QMSingleButton("UserInteractMenu", 5, 0.25f, "", () =>
            {
            }, "Can't save this avatar cause it's private");
            ButtonHidding.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);
            ButtonHidding.setIntractable(false);
            ButtonHidding.setActive(false);

            new QMSingleButton(ThisMenu, 1, 0, "Teleport", () =>
            {
                var SelectedPlayer = Wrappers.Utils.QuickMenu.SelectedVRCPlayer();
                VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position = SelectedPlayer.transform.position;
            }, "Teleport", Color.cyan, Color.magenta);

            new QMSingleButton(ThisMenu, 3, 0, "Download\nVrca", () =>
            {
                var Player = Wrappers.Utils.QuickMenu.SelectedVRCPlayer();
                Process.Start(Player.prop_ApiAvatar_0.assetUrl);
            }, "Downlaod the vrca of this avatar");

            MessageEvolve = new QMSingleButton(ThisMenu, 2, 0, "Send\nMessage", () =>
            {
                var SelectedPlayer = Wrappers.Utils.QuickMenu.SelectedVRCPlayer();
                PopupManager.InputeText("Enter message", "Send", new Action<string>((String) =>
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(String);
                    var Encode = Convert.ToBase64String(bytes);
                    GameObject MessageObject = GameObject.Find($"Evolve message handler for {SelectedPlayer.UserID()}");
                    if (MessageObject == null)
                    {
                        MessageObject = new GameObject($"Evolve message handler for {SelectedPlayer.UserID()}");
                    }
                    Networking.RPC(RPC.Destination.All, MessageObject, Encode, new Il2CppSystem.Object[0]);
                }));
            }, "Send Message to this Evolve member", Color.cyan, Color.magenta);

            MessageEvolve.setIntractable(false);

        }
    }
}