using System;
using Evolve.Api;
using Evolve.Buttons;
using Evolve.Utils;
using Evolve.Wrappers;
using MelonLoader;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC;
using System.Threading;
using Evolve.Module;
using Evolve.Login;
using Evolve.AvatarList;

namespace Evolve.Modules
{
    internal class PlayersInformations
    {
        public static List<GameObject> InfoPanel = new List<GameObject>();
        public static float posx = 450;
        public static float posx2 = 785;
        public static float posy = 200;
        public static float posy2 = 200;
        public static float posy3 = 370;
        public static List<MenuText> Logs = new List<MenuText>();
        public static List<MenuText> Logs1 = new List<MenuText>();
        public static List<MenuText> EvoInfos = new List<MenuText>();

        public static IEnumerator SelfInfoInit()
        {
            while (Wrappers.Utils.LocalPlayer != true) yield return null;
            while (Buttons.SelfMenu.SelfInfos == null) yield return null;
            while (Buttons.SelfMenu.SelfInfos.getGameObject().active == false) yield return null;

            for (int i = 0; i <= 19; i++)
            {
                MenuText item = new MenuText(Buttons.SelfMenu.SelfInfos.getGameObject().transform, posx, posy, "");
                Logs.Add(item);
                posy += 70f;
            }
            while (Logs.Count != 20) yield return null;
            Logs[0].SetText("<color=#ff006a>Name:</color> " + Wrappers.Utils.LocalPlayer.field_Private_VRCPlayerApi_0.displayName);
            Logs[1].SetText("<color=#ff006a>Is in VR:</color> " + Wrappers.Utils.LocalPlayer.GetIsInVR().ToString());
            Logs[2].SetText("<color=#ff006a>Is Master:</color> " + Wrappers.Utils.LocalPlayer.GetIsMaster().ToString());
            Logs[3].SetText("<color=#ff006a>Is Bot:</color> " + Wrappers.Utils.LocalPlayer.GetIsBot().ToString());
            Logs[4].SetText("<color=#ff006a>Actor ID:</color> " + Wrappers.Utils.LocalPlayer.field_Private_VRCPlayerApi_0.playerId);
            Logs[5].SetText("<color=#ff006a>User Rank:</color> " + Wrappers.Utils.LocalPlayer.GetAPIUser().GetRank());
            Logs[6].SetText("<color=#ff006a>Steam ID:</color> " + Wrappers.Utils.LocalPlayer.GetSteamID().ToString());
            Logs[7].SetText("<color=#ff006a>Ping:</color> " + Wrappers.Utils.LocalPlayer.GetPingColored());
            Logs[8].SetText("<color=#ff006a>Frames:</color> " + Wrappers.Utils.LocalPlayer.GetFramesColored());
            Logs[9].SetText("<color=#ff006a>Favorite Friends:</color> " + Wrappers.Utils.LocalPlayer.GetAPIUser().GetTotalFavoriteFriendsInAllGroups());
            Logs[10].SetText("<color=#ff006a>Can set status Offline:</color> " + Wrappers.Utils.LocalPlayer.GetAPIUser().canSetStatusOffline.ToString());
            Logs[11].SetText("<color=#ff006a>Avatar Copying:</color> " + Wrappers.Utils.LocalPlayer.GetAPIUser().allowAvatarCopying.ToString());
            Logs[12].SetText("<color=#ff006a>Avatar Name:</color> " + Wrappers.Utils.LocalPlayer.GetApiAvatar().name);
            Logs[13].SetText("<color=#ff006a>Avatar Author:</color> " + Wrappers.Utils.LocalPlayer.GetApiAvatar().authorName);
            Logs[14].SetText("<color=#ff006a>Avatar Status:</color> " + Wrappers.Utils.LocalPlayer.GetApiAvatar().releaseStatus);
            Logs[15].SetText("<color=#ff006a>Avatar Platform:</color> " + Wrappers.Utils.LocalPlayer.GetApiAvatar().platform);
            Logs[16].SetText("<color=#ff006a>Avatar Version:</color> " + Wrappers.Utils.LocalPlayer.GetApiAvatar().version);
            Logs[17].SetText("<color=#ff006a>Has Moderation Power:</color> " + Wrappers.Utils.LocalPlayer.GetAPIUser().hasModerationPowers.ToString());
            Logs[18].SetText("<color=#ff006a>Has Negative Trust:</color> " + Wrappers.Utils.LocalPlayer.GetAPIUser().hasNegativeTrustLevel.ToString());
            Logs[19].SetText("<color=#ff006a>Has you Blocked:</color> " + "False");
            while (true)
            {
                yield return new WaitForSeconds(1.3f);
                if (Wrappers.Utils.LocalPlayer != true) yield return null;
                if (Buttons.SelfMenu.SelfInfos.getGameObject().active != true) yield return null;
                try
                {
                    Logs[0].SetText("<color=#ff006a>Name:</color> " + Wrappers.Utils.LocalPlayer.field_Private_VRCPlayerApi_0.displayName);
                    Logs[2].SetText("<color=#ff0055>Is Master:</color> " + Wrappers.Utils.LocalPlayer.GetIsMaster().ToString());
                    Logs[4].SetText("<color=#ff006a>Actor ID:</color> " + Wrappers.Utils.LocalPlayer.field_Private_VRCPlayerApi_0.playerId);
                    Logs[7].SetText("<color=#ff0055>Ping:</color> " + Wrappers.Utils.LocalPlayer.GetPingColored());
                    Logs[8].SetText("<color=#ff0055>Frames:</color> " + Wrappers.Utils.LocalPlayer.GetFramesColored());
                    Logs[11].SetText("<color=#ff0055>Avatar Copying:</color> " + Wrappers.Utils.LocalPlayer.GetAPIUser().allowAvatarCopying.ToString());
                    Logs[12].SetText("<color=#ff0055>Avatar Name:</color> " + Wrappers.Utils.LocalPlayer.GetApiAvatar().name);
                    Logs[13].SetText("<color=#ff0055>Avatar Author:</color> " + Wrappers.Utils.LocalPlayer.GetApiAvatar().authorName);
                    Logs[14].SetText("<color=#ff0055>Avatar Status:</color> " + Wrappers.Utils.LocalPlayer.GetApiAvatar().releaseStatus);
                    Logs[15].SetText("<color=#ff0055>Avatar Platform:</color> " + Wrappers.Utils.LocalPlayer.GetApiAvatar().platform);
                    Logs[16].SetText("<color=#ff0055>Avatar Version:</color> " + Wrappers.Utils.LocalPlayer.GetApiAvatar().version);
                }
                catch {}
            }
        }

        public static IEnumerator UserInfoInit()
        {
            while (Wrappers.Utils.LocalPlayer != true) yield return null;
            while (Buttons.EvolveInteract.UserInfos == null) yield return null;
            while (Buttons.EvolveInteract.UserInfos.getGameObject().active == false) yield return null;

            for (int i = 0; i <= 19; i++)
            {
                MenuText item = new MenuText(Buttons.EvolveInteract.UserInfos.getGameObject().transform, posx, posy2, "");
                Logs1.Add(item);
                posy2 += 70f;
            }
            while (Logs1.Count != 20) yield return null;

            Logs1[0].SetText("<color=#ff006a>Name:</color> ");
            Logs1[1].SetText("<color=#ff006a>Is in VR:</color> ");
            Logs1[2].SetText("<color=#ff006a>Is Master:</color> ");
            Logs1[3].SetText("<color=#ff006a>Is Bot:</color> ");
            Logs1[4].SetText("<color=#ff006a>Photon ID:</color> ");
            Logs1[5].SetText("<color=#ff006a>User Rank:</color> ");
            Logs1[6].SetText("<color=#ff006a>Steam ID:</color> ");
            Logs1[7].SetText("<color=#ff006a>Ping:</color> ");
            Logs1[8].SetText("<color=#ff006a>Frames:</color> ");
            Logs1[9].SetText("<color=#ff006a>Favorite Friends:</color> ");
            Logs1[10].SetText("<color=#ff006a>Can set status Offline:</color> ");
            Logs1[11].SetText("<color=#ff006a>Avatar Copying:</color> ");
            Logs1[12].SetText("<color=#ff006a>Avatar Name:</color> ");
            Logs1[13].SetText("<color=#ff006a>Avatar Author:</color> ");
            Logs1[14].SetText("<color=#ff006a>Avatar Status:</color> ");
            Logs1[15].SetText("<color=#ff006a>Avatar Platform:</color> ");
            Logs1[16].SetText("<color=#ff006a>Avatar Version:</color> ");
            Logs1[17].SetText("<color=#ff006a>Has Moderation Power:</color> ");
            Logs1[18].SetText("<color=#ff006a>Has Negative Trust:</color> ");
            Logs1[19].SetText("<color=#ff006a>Has you Blocked:</color> ");
            while (true)
            {
                yield return new WaitForSeconds(1.3f);
                if (Wrappers.Utils.LocalPlayer != true) yield return null;
                if (Buttons.EvolveInteract.UserInfos.getGameObject().active != true) yield return null;
                try
                {
                    Logs1[0].SetText("<color=#ff006a>Name:</color> " + Wrappers.Utils.QuickMenu.SelectedVRCPlayer().DisplayName());
                    Logs1[1].SetText("<color=#ff006a>Is in VR:</color> " + Wrappers.Utils.QuickMenu.SelectedVRCPlayer().GetIsInVR().ToString());
                    Logs1[2].SetText("<color=#ff006a>Is Master:</color> " + Wrappers.Utils.QuickMenu.SelectedVRCPlayer().GetIsMaster().ToString());
                    Logs1[3].SetText("<color=#ff006a>Is Bot:</color> " + Wrappers.Utils.QuickMenu.SelectedVRCPlayer().GetIsBot().ToString());
                    Logs1[4].SetText("<color=#ff006a>Actor ID:</color> " + Wrappers.Utils.QuickMenu.SelectedVRCPlayer().field_Private_VRCPlayerApi_0.playerId);
                    Logs1[5].SetText("<color=#ff006a>User Rank:</color> " + Wrappers.Utils.QuickMenu.SelectedVRCPlayer().GetAPIUser().GetRank());
                    Logs1[6].SetText("<color=#ff006a>Steam ID:</color> " + Wrappers.Utils.QuickMenu.SelectedVRCPlayer().GetSteamID().ToString());
                    Logs1[7].SetText("<color=#ff006a>Ping:</color> " + Wrappers.Utils.QuickMenu.SelectedVRCPlayer().GetPingColored());
                    Logs1[8].SetText("<color=#ff006a>Frames:</color> " + Wrappers.Utils.QuickMenu.SelectedVRCPlayer().GetFramesColored());
                    Logs1[9].SetText("<color=#ff006a>Favorite Friends:</color> " + Wrappers.Utils.QuickMenu.SelectedVRCPlayer().GetAPIUser().GetTotalFavoriteFriendsInAllGroups());
                    Logs1[10].SetText("<color=#ff006a>Can set status Offline:</color> " + Wrappers.Utils.QuickMenu.SelectedVRCPlayer().GetAPIUser().canSetStatusOffline.ToString());
                    Logs1[11].SetText("<color=#ff006a>Avatar Copying:</color> " + Wrappers.Utils.QuickMenu.SelectedVRCPlayer().GetAPIUser().allowAvatarCopying.ToString());
                    Logs1[12].SetText("<color=#ff006a>Avatar Name:</color> " + Wrappers.Utils.QuickMenu.SelectedVRCPlayer().GetApiAvatar().name);
                    Logs1[13].SetText("<color=#ff006a>Avatar Author:</color> " + Wrappers.Utils.QuickMenu.SelectedVRCPlayer().GetApiAvatar().authorName);
                    Logs1[14].SetText("<color=#ff006a>Avatar Status:</color> " + Wrappers.Utils.QuickMenu.SelectedVRCPlayer().GetApiAvatar().releaseStatus);
                    Logs1[15].SetText("<color=#ff006a>Avatar Platform:</color> " + Wrappers.Utils.QuickMenu.SelectedVRCPlayer().GetApiAvatar().platform);
                    Logs1[16].SetText("<color=#ff006a>Avatar Version:</color> " + Wrappers.Utils.QuickMenu.SelectedVRCPlayer().GetApiAvatar().version);
                    Logs1[17].SetText("<color=#ff006a>Has Moderation Power:</color> " + Wrappers.Utils.QuickMenu.SelectedVRCPlayer().GetAPIUser().hasModerationPowers.ToString());
                    Logs1[18].SetText("<color=#ff006a>Has Negative Trust:</color> " + Wrappers.Utils.QuickMenu.SelectedVRCPlayer().GetAPIUser().hasNegativeTrustLevel.ToString());
                    Logs1[19].SetText("<color=#ff006a>Has you Blocked:</color> " + "False");
                }
                catch{}
            }
        }

        public static IEnumerator EvoInfo()
        {
            while (Wrappers.Utils.LocalPlayer != true) yield return null;
            while (EvolveMenu.EvoPanel == null) yield return null;
            while (EvolveMenu.EvoPanel.getGameObject().active == false) yield return null;
            while (Authorization.Subscribtion == null) yield return null;

            for (int i = 0; i <= 7; i++)
            {
                MenuText item = new MenuText(EvolveMenu.EvoPanel.getGameObject().transform, posx2, posy3, "");
                EvoInfos.Add(item);
                posy3 += 70f;
            }
            while (EvoInfos.Count != 8) yield return null;
            EvoInfos[0].SetText("<color=#ff006a>Account:</color> " + Wrappers.Utils.LocalPlayer.field_Private_VRCPlayerApi_0.displayName);
            EvoInfos[1].SetText("<color=#ff006a>Subscription:</color> " + Authorization.Subscribtion);
            EvoInfos[2].SetText("<color=#ff006a>Access Level:</color> " + Authorization.AccessLevel.ToString());
            EvoInfos[3].SetText("<color=#ff006a>Servers:</color> Online");
            EvoInfos[4].SetText("<color=#ff006a>Total users:</color> " + Authorization.UsersCount);
            EvoInfos[5].SetText("<color=#ff006a>Crashers:</color> " + Protections.ClientChecks.Crashers);
            EvoInfos[6].SetText("<color=#ff006a>Avatars:</color> " + AvatarListHelper.List.Count);
        }
    }
}