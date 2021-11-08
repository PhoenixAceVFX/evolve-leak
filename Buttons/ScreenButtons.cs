using Evolve.Api;
using Evolve.Utils;
using Evolve.Wrappers;
using UnityEngine;
using VRC.UI;
using Process = System.Diagnostics.Process;
using VRC.Core;
using Evolve.AvatarList;
using System.Linq;
using System.Windows.Forms;
using UnityEngine.UI;
using System.Collections;
using MelonLoader;
using Evolve.ConsoleUtils;
using Evolve.Exploits;
using System;

namespace Evolve.Buttons
{
    internal class ScreenButtons
    {
#pragma warning disable CS0649 // Le champ 'ScreenButtons.Kick' n'est jamais assigné et aura toujours sa valeur par défaut null
        public static UnityEngine.UI.Button Kick;
#pragma warning restore CS0649 // Le champ 'ScreenButtons.Kick' n'est jamais assigné et aura toujours sa valeur par défaut null
#pragma warning disable CS0649 // Le champ 'ScreenButtons.Warn' n'est jamais assigné et aura toujours sa valeur par défaut null
        public static UnityEngine.UI.Button Warn;
#pragma warning restore CS0649 // Le champ 'ScreenButtons.Warn' n'est jamais assigné et aura toujours sa valeur par défaut null
#pragma warning disable CS0649 // Le champ 'ScreenButtons.MicOff' n'est jamais assigné et aura toujours sa valeur par défaut null
        public static UnityEngine.UI.Button MicOff;
#pragma warning restore CS0649 // Le champ 'ScreenButtons.MicOff' n'est jamais assigné et aura toujours sa valeur par défaut null
        public static UnityEngine.UI.Button Crash;
        public static float Timetowait;
        public static float ReuploadCD;

        public static void Initialize()
        {

            //Users
            ScreenAPI.UserScreen(1, 0, "Teleport", () =>
            {
                var UserScreen = GameObject.Find("Screens").transform.Find("UserInfo");
                var PlayerInfos = UserScreen.transform.GetComponentInChildren<PageUserInfo>();
                var User = Wrappers.Utils.PlayerManager.GetPlayer(PlayerInfos.field_Public_APIUser_0.id);
                Wrappers.Utils.VRCUiPopupManager.HideCurrentPopUp();
                VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position = User._vrcplayer.transform.position;
                VRCUiManager.prop_VRCUiManager_0.Method_Public_Void_Boolean_0(true);
                VRCUiManager.prop_VRCUiManager_0.Method_Public_Void_Boolean_1(true);
            });

            ScreenAPI.UserScreen(2, 0, "Save Avatar", () =>
            {
                var UserScreen = GameObject.Find("Screens").transform.Find("UserInfo");
                var PlayerInfos = UserScreen.transform.GetComponentInChildren<PageUserInfo>();
                var User = Wrappers.Utils.PlayerManager.GetPlayer(PlayerInfos.field_Public_APIUser_0.id);
                if (User != null)
                {
                    var avatar = User.GetApiAvatar();
                    if (avatar.releaseStatus != "private")
                    {
                        if (!Config.DAvatars.Any(v => v.AvatarID == avatar.id))
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
                }
                else
                {
                    var APIUser = PlayerInfos.field_Public_APIUser_0;
                    if (!Config.DAvatars.Any(v => v.AvatarID == APIUser.avatarId))
                    {
                        AvatarListHelper.AvatarListPassthru(APIUser.avatarId, APIUser.currentAvatarImageUrl, "Unknown");
                        AvatarList.Initialize.CustomList.AList.Refresh(Config.DAvatars.Select(x => x.AvatarID).Reverse());
                        AvatarList.Initialize.FavoriteButton.Title.text = "Remove";
                        AvatarList.Initialize.CustomList.ListTitle.text = $"Evolve Engine [{Config.DAvatars.Count}]";
                    }
                    else
                    {
                        AvatarListHelper.AvatarListPassthru(APIUser.avatarId, APIUser.currentAvatarImageUrl, "Unknown");
                        AvatarList.Initialize.CustomList.AList.Refresh(Config.DAvatars.Select(x => x.AvatarID).Reverse());
                        AvatarList.Initialize.FavoriteButton.Title.text = "Save";
                        AvatarList.Initialize.CustomList.ListTitle.text = $"Evolve Engine [{Config.DAvatars.Count}]";
                    }
                }
            });

            Crash = ScreenAPI.UserScreen(1, 1, "Crash", () =>
            {
                if (Time.time < Timetowait)
                {
                    Wrappers.Utils.VRCUiPopupManager.ShowAlert("Evolve Engine", $"Function is still in cooldown !");
                    return;
                }
                Timetowait = Time.time + 35;
                var UserScreen = GameObject.Find("Screens").transform.Find("UserInfo");
                var PlayerInfos = UserScreen.transform.GetComponentInChildren<PageUserInfo>();
                var Player = Wrappers.Utils.PlayerManager.GetPlayer(PlayerInfos.field_Public_APIUser_0.id);
                string CrashType = FoldersManager.Config.Ini.GetString("Crashers", "Type");
                if (CrashType.Length < 1)
                {
                    FoldersManager.Config.Ini.SetString("Crashers", "Type", "Material");
                    CrashType = FoldersManager.Config.Ini.GetString("Crashers", "Type");
                }

                if (CrashType == "Material") MelonCoroutines.Start(AvatarCrash.TargetCrash(Player._vrcplayer, Settings.MaterialCrash, 6));
                else if (CrashType == "CCD-IK") MelonCoroutines.Start(AvatarCrash.TargetCrash(Player._vrcplayer, Settings.CCDIK, 6));
                else if (CrashType == "Mesh-Poly") MelonCoroutines.Start(AvatarCrash.TargetCrash(Player._vrcplayer, Settings.MeshPolyCrash, 10));
                else if (CrashType == "Audio") MelonCoroutines.Start(AvatarCrash.TargetCrash(Player._vrcplayer, Settings.AudioCrash, 15));
                else if (CrashType == "Custom") MelonCoroutines.Start(AvatarCrash.TargetCrash(Player._vrcplayer, FoldersManager.Config.Ini.GetString("Crashers", "CustomID"), 10));

                EvoVrConsole.Log(EvoVrConsole.LogsType.Crash, $"Crashing target with <color=white>{CrashType}</color>");

                VRCUiManager.prop_VRCUiManager_0.Method_Public_Void_Boolean_0(true);
                VRCUiManager.prop_VRCUiManager_0.Method_Public_Void_Boolean_1(true);

                try
                {
                    ApiWorldInstance Instance = RoomManager.field_Internal_Static_ApiWorldInstance_0;
                    ApiWorld World = RoomManager.field_Internal_Static_ApiWorld_0;
                    Discord.WebHooks.SendEmbedMessage("**Target crash**", $"**Crash Type:** {CrashType}\n**User Name:** {VRCPlayer.field_Internal_Static_VRCPlayer_0.DisplayName()}\n**Target name:** {Player.DisplayName()}\n**Target rank:** {Player.GetAPIUser().GetRank()}\n**World Name:** {World.name}\n**World ID:** {Instance.id}\n");
                }
                catch { }
            });
            Crash.interactable = false;

            ScreenAPI.UserScreen(1, 0, "Drop portal", () =>
            {
                var UserScreen = GameObject.Find("Screens").transform.Find("UserInfo");
                var PlayerInfos = UserScreen.transform.GetComponentInChildren<PageUserInfo>();
                if (PlayerInfos.field_Public_APIUser_0.location.Contains(":"))
                {
                    var WorldID = PlayerInfos.field_Public_APIUser_0.location.Split(':')[0];
                    var InstanceID = PlayerInfos.field_Public_APIUser_0.location.Split(':')[1];
                    Functions.DropPortal(WorldID, InstanceID, 0, VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position + VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.forward * 1.5f, Quaternion.identity);
                    VRCUiManager.prop_VRCUiManager_0.Method_Public_Void_Boolean_0(true);
                    VRCUiManager.prop_VRCUiManager_0.Method_Public_Void_Boolean_1(true);
                    VRCUiManager.prop_VRCUiManager_0.Method_Public_Void_Boolean_2(true);
                }
            });

            ScreenAPI.UserScreen(3, 1, "<color=#ffb700>Reupload</color>", () =>
            {
                var UserScreen = GameObject.Find("Screens").transform.Find("UserInfo");
                var PlayerInfos = UserScreen.transform.GetComponentInChildren<PageUserInfo>();
                var User = Wrappers.Utils.PlayerManager.GetPlayer(PlayerInfos.field_Public_APIUser_0.id);
                if (Login.Authorization.AccessLevel > 1)
                {
                    if (Time.time < ReuploadCD)
                    {
                        Wrappers.Utils.VRCUiPopupManager.ShowAlert("Evolve Engine", $"Function is still in cooldown !");
                        return;
                    }
                    ReuploadCD = Time.time + 60;
                    bool SetName = false;
                    bool SetDesc = false;
                    bool SetThumbnailUrl = false;

                    var Name = User.GetApiAvatar().name;
                    var Desc = User.GetApiAvatar().description;
                    var Thumbnail = User.GetApiAvatar().imageUrl;

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

                        Yoink.Yoinker.ReuploadAvatar(User.GetApiAvatar(), Name, Desc, Thumbnail);
                    }
                }
                else Wrappers.Utils.VRCUiPopupManager.ShowAlert("Evolve Engine", "This feature is reserved for VIP users or require an access level of 2");
            });
        }
        public static void Initialize2()
        {

            //World
            ScreenAPI.WorldScreen(-200, 300, "Copy ID", () =>
            {
                var WorldScreen = GameObject.Find("Screens").transform.Find("WorldInfo");
                var Infos = WorldScreen.transform.GetComponentInChildren<PageWorldInfo>();

                Functions.copytoclip(Infos.field_Private_ApiWorld_0.id + ":" + Infos.field_Public_ApiWorldInstance_0.tags);
            });

            ScreenAPI.WorldScreen(-200, 375, "Send Bots", () =>
            {
                var WorldScreen = GameObject.Find("Screens").transform.Find("WorldInfo");
                var Infos = WorldScreen.transform.GetComponentInChildren<PageWorldInfo>();
            });
        }
    }
}