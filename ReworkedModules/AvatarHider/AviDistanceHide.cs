using Evolve.ConsoleUtils;
using Evolve.Utils;
using Evolve.Wrappers;
using System;
using System.Collections;
using UnityEngine;
using VRC;
using VRC.Core;

namespace Evolve.Modules.AvatarHider
{
    internal class AviDistanceHide
    {
        public static VRCPlayer GetLocalVRCPlayer() => VRCPlayer.field_Internal_Static_VRCPlayer_0;
        public static GameObject GetAvatarObject(VRC.Player p) => p.prop_VRCPlayer_0.prop_VRCAvatarManager_0.prop_GameObject_0;
        public static bool IsMe(VRC.Player p) => p.name == GetLocalVRCPlayer().name;
        public static bool IsFriendsWith(string id) => APIUser.CurrentUser.friendIDs.Contains(id);

        public static IEnumerator AvatarScanner()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);
                if (Settings.m_HideAvatars && GetLocalVRCPlayer() != null)
                {
                    foreach (var Player in Wrappers.Utils.PlayerManager.AllPlayers())
                    {
                        if (Player != null && Player != VRCPlayer.field_Internal_Static_VRCPlayer_0._player)
                        {
                            GameObject Avatar = GetAvatarObject(Player);
                            var Distance = Vector3.Distance(GetLocalVRCPlayer().transform.position, Avatar.transform.position);
                            var IsActive = Avatar.active;

                            if (Settings.m_IgnoreFriends)
                            {
                                if (!IsFriendsWith(Player.UserID()))
                                {
                                    if (Distance > Settings.m_Distance && IsActive && Settings.m_HideAvatars) Avatar.SetActive(false);
                                    else if (Distance <= Settings.m_Distance && !IsActive && Settings.m_HideAvatars) Avatar.SetActive(true);
                                    else if (!Settings.m_HideAvatars && !IsActive) Avatar.SetActive(true);
                                }
                            }
                            else
                            {
                                if (Distance > Settings.m_Distance && IsActive && Settings.m_HideAvatars) Avatar.SetActive(false);
                                else if (Distance <= Settings.m_Distance && !IsActive && Settings.m_HideAvatars) Avatar.SetActive(true);
                                else if (!Settings.m_HideAvatars && !IsActive) Avatar.SetActive(true);
                            }
                        }
                    }
                }
            }
        }

        public static void UnHideAvatars()
        {
            try
            {
                foreach (VRC.Player player in PlayerManager.Method_Public_Static_ArrayOf_Player_0())
                {
                    if (player == null || IsMe(player)) continue;


                    GameObject avtrObject = GetAvatarObject(player);
                    if (avtrObject == null || avtrObject.active)
                    {
                        continue;
                    }

                    avtrObject.SetActive(true);
                }
            }
            catch (Exception e)
            {
                EvoVrConsole.Log(EvoVrConsole.LogsType.Avatar, $"Failed to unhide avatar: {e}");
            }
        }
    }
}