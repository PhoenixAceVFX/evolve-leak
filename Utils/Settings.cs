using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace Evolve.Utils
{
    internal class Settings
    {
        public static string CCDIK = "avtr_999";
        public static string AudioCrash = "avtr_999";
        public static string MeshPolyCrash = "avtr_999";
        public static string MaterialCrash = "avtr_999";
        public static bool MasterAviCrash = false;
        public static bool RPCList = false;
        public static bool SendToMoon = false;
        public static bool USpeakExploit = false;
        public static bool MassPortal = false;
        public static bool WorldTriggers = false;
        public static bool ForceInteractPickups = false;
        public static bool NotifyFriend = false;
        public static bool Send7 = false;
        public static bool MaxVelocity = false;
        public static bool GhostMode = false;
        public static bool NoStats = false;
        public static bool PinPL = false;
        public static bool LongDistancePickups = false;
        public static bool AntiTheft = false;
        public static string UIColorHex = "#6a00ff";
        public static string UITextColorHex = "#ff0055";
#pragma warning disable CS0649              
        public static Color Purple;
#pragma warning restore CS0649              
#pragma warning disable CS0649              
        public static Color PinkRed;
#pragma warning restore CS0649              
        public static bool TriggersEsp = false;
        public static bool InBlacklistedAvatar = false;
        public static bool PickupSteal = false;
        public static bool AvatarLoaded = false;
        public static bool IsCrashing = false;
        public static bool Blink = false;
        public static bool MasterLag = false;
        public static bool LockRoom = false;
        public static bool QuestLagger = false;
        public static bool UdonRPCList = false;
        public static bool Rotator = false;
        public static bool ApiLogs = false;
        public static int Ping = 0;
        public static bool Oversee = false;
        public static bool DisconnectLobby = false;
        public static bool AvatarLogs = false;
        public static bool FramesSpoof = false;
        public static bool PingSpoof = false;
        public static bool DisconnectMaster = false;
        public static bool LobbyTimout = false;
        public static bool DesyncLobby = false;
        public static bool FreeFall = false;
        public static bool InvisibleJoin = false;
        public static bool InvisibleJoin2 = false;
        public static bool PortalBypass = false;
        public static bool MeshEsp = false;
        public static bool FbtDesync = false;
        public static bool RoomRainDesync = false;
        public static bool LogoutLobby = false;
        public static bool IsTester = false;
        public static bool VideoLogout = false;
        public static bool BotLogoutLobby = false;
        public static bool MenuRemover = false;
        public static bool PenCrash = false;
        public static bool TransferDesync = false;
        public static bool OfflineSpoof = false;
        public static bool WorldSpoof = false;
        public static float Frames = 0;
        public static bool EmoteLag = false;
        public static bool AntiLogout = false;
        public static bool MurderGodMode = false;
        public static bool SpamPrefabs = false;
        public static bool MuteNonFriends = false;
        public static bool m_HideAvatars = false;
        public static bool m_IgnoreFriends = true;
        public static bool Moderation = false;
        public static float m_Distance = 7f;
        public static bool VrFly = false;
        public static bool PortalCrash = false;
        public static bool GlobalTouch = false;
        public static bool EventBlock = false;
        public static bool RPCBlock = false;
        public static bool MyEventsLog = false;
        public static bool EventLog = false;
        public static bool CapsuleEsp = false;
        public static bool AvatarSpoof = false;
        public static bool LogoutTarget = false;
        public static bool ItemCrash = false;
        public static bool EspWasEnabled = false;
        public static bool ItemEsp = false;
        public static bool Reflect = false;
        public static bool ItemEspWasEnabled = false;
        public static bool UnLimitedPortals = false;
        public static bool AttachToPlayerHand = false;
        public static bool EvoNameplates = true;
        public static bool UnlimitedFrames = false;
        public static bool AutoDrop = false;
        public static bool LogAvatars = true;
        public static bool RPCEventsLogs = false;
        public static bool PostProcessing = false;
        public static bool Debug = false;
        public static bool EmojiSpam = false;
        public static bool HideDiscordName = false;
        public static bool DesyncWorld = false;
        public static bool MasterDesync = false;
        public static bool ComeBack = false;
        public static bool AlreadySavedData = false;
        public static bool TreeCrash = false;
        public static bool SpamMirror = false;
        public static bool SteamSpoof = true;
        public static bool BlockProtection = true;
        public static bool KickProtection = true;
        public static bool BotProtection = true;
        public static bool FullScreenOnStart = true;
        public static bool PickupsProtection = false;
        public static bool PortalProtection = false;
        public static bool PublicBanProtection = true;
        public static bool TriggersProtection = true;
        public static bool IpProtection = false;
        public static bool VideoProtection = true;
        public static bool Serialize = false;
        public static bool UIMicIconColorChangingEnabled = true;
        public static bool LoudMic = false;
        public static bool UIActionMenuColorChangingEnabled = true;
        public static bool LoudVoices = false;
        public static bool ForceClone = true;
        public static bool IsAdmin = false;
        public static bool LoggerEnable = false;
        public static bool PlayersInformations = true;
        public static bool NoClip = false;
        public static bool ModerationLogs = false;
        public static bool ItemsOrbit = false;
        public static bool AttachToPlayer = false;
        public static bool JarGameGodMode = false;
        public static bool SaveUIColor = false;
        public static bool HideDiscordWorld = false;
        public static bool FeetColliders = false;
        public static bool HandColliders = true;
        public static bool HipBones = false;
        public static bool ChestBones = false;
        public static bool HeadBones = true;
        public static float SpeedValue = 5f;
        public static bool Fly = false;
        public static float FlySpeed = 5;
        public static float BHopSpeed = 4f;
        public static bool LovenseRemote = false;
        public static bool StreamerMode = false;
        public static bool TargetItemsOrbit = false;
        public static bool SpamPickupsTarget = false;
        public static bool RamCrash = false;
        public static bool Speed = false;
        public static bool UdonExploitProtections = false;
        public static bool AvatarCleaning = true;
        public static bool ChairsProtection = false;
        public static bool UdonRPCLogs = false;
#pragma warning disable CS0649               
        public static float ComfortSpeed;
#pragma warning restore CS0649               
#pragma warning disable CS0649               
        public static float MouseSpeed;
#pragma warning restore CS0649               
        public static bool Murder4Shield;
        public static bool AmongAntiVoteOut;
        public static bool DesyncLobby6 = false;
        public static bool QuestSpoof = false;
        public static bool CrashLogs = false;

        public static Vector3 Gravity;

        public static void SavedGravity()
        {
            Gravity = Physics.gravity;
        }
        public static Color HexToColor(string hexColor)
        {
            if (hexColor.IndexOf('#') != -1)
            {
                hexColor = hexColor.Replace("#", "");
            }
            float r = int.Parse(hexColor.Substring(0, 2), NumberStyles.AllowHexSpecifier) / 255f;
            float g = int.Parse(hexColor.Substring(2, 2), NumberStyles.AllowHexSpecifier) / 255f;
            float b = int.Parse(hexColor.Substring(4, 2), NumberStyles.AllowHexSpecifier) / 255f;
            return new Color(r, g, b);
        }

        public static string ColorToHex(Color baseColor, bool hash = false)
        {
            int num = Convert.ToInt32(baseColor.r * 255f);
            int num2 = Convert.ToInt32(baseColor.g * 255f);
            int num3 = Convert.ToInt32(baseColor.b * 255f);
            string text = num.ToString("X2") + num2.ToString("X2") + num3.ToString("X2");
            if (hash)
            {
                text = "#" + text;
            }
            return text;
        }

        public static Color MenuColor()
        {
            return HexToColor(UIColorHex);
        }

        public static Color TextColor()
        {
            return HexToColor(UITextColorHex);
        }

#pragma warning disable CS0649               
        public static Dictionary<string, FbtCalibration> _savedCalibrations;
#pragma warning restore CS0649               

        public class FbtCalibration
        {
#pragma warning disable CS0649              
            public KeyValuePair<Vector3, Quaternion> Hip;
#pragma warning restore CS0649              
#pragma warning disable CS0649              
            public KeyValuePair<Vector3, Quaternion> LeftFoot;
#pragma warning restore CS0649              
#pragma warning disable CS0649              
            public KeyValuePair<Vector3, Quaternion> RightFoot;
#pragma warning restore CS0649              
        }
    }
}