using Evolve.Modules;
using Evolve.Utils;
using MelonLoader;
using System.IO;
using UnityEngine;

namespace Evolve.FoldersManager
{
    internal class Config
    {
        public static IniFile Ini = new IniFile(string.Concat(Directory.GetCurrentDirectory(), "/Evolve/Config.ini"));

        public static void CheckConfig()
        {
            if (Ini != null)
            {
                if (Ini.GetBool("UwUMenu", "Haptics") == true)
                {
                    ImmersiveTouch.Enable = true;
                }
                else
                {
                    ImmersiveTouch.Enable = false;
                }

                if (Ini.GetBool("Self", "ToggleFrameSpoof") == true)
                {
                    Settings.FramesSpoof = true;
                }
                else
                {
                    Settings.FramesSpoof = false;
                }

                if (Ini.GetBool("Self", "ForceClone") == true)
                {
                    Settings.ForceClone = true;
                }
                else
                {
                    Settings.ForceClone = false;
                }

                Settings.Frames = Ini.GetFloat("Self", "FramesSpoof");
                Settings.Ping = Ini.GetInt("Self", "Ping");

                if (Ini.GetBool("Self", "OfflineSpoof") == true)
                {
                    Settings.OfflineSpoof = true;
                }
                else Settings.OfflineSpoof = false;

                if (Ini.GetBool("Self", "PingSpoof") == true)
                {
                    Settings.PingSpoof = true;
                }
                else Settings.PingSpoof = false;

                if (Ini.GetBool("Self", "LoudMic") == true)
                {
                    Settings.LoudMic = true;
                }
                else
                {
                    Settings.LoudMic = false;
                }

                if (Ini.GetBool("Lobby", "LoudVoices") == true)
                {
                    Settings.LoudVoices = true;
                }
                else
                {
                    Settings.LoudVoices = false;
                }

                if (Ini.GetBool("Lobby", "UnlimitedPortals") == true)
                {
                    Settings.UnLimitedPortals = true;
                }
                else
                {
                    Settings.UnLimitedPortals = false;
                }

                if (Ini.GetBool("Lobby", "CapsuleEsp") == true)
                {
                    Settings.CapsuleEsp = true;
                }
                else
                {
                    Settings.CapsuleEsp = false;
                }

                if (Ini.GetBool("Lobby", "MeshEsp") == true)
                {
                    Settings.MeshEsp = true;
                }
                else
                {
                    Settings.MeshEsp = false;
                }

                if (Ini.GetBool("Lobby", "ItemEsp") == true)
                {
                    Settings.ItemEsp = true;
                }
                else
                {
                    Settings.ItemEsp = false;
                }

                if (Ini.GetBool("Lobby", "PinPlayersList") == true)
                {
                    Settings.PinPL = true;
                }
                else
                {
                    Settings.PinPL = false;
                }

                if (Ini.GetBool("Lobby", "TriggersEsp") == true)
                {
                    Settings.TriggersEsp = true;
                }
                else
                {
                    Settings.TriggersEsp = false;
                }

                if (Ini.GetBool("Protections", "KickProtection") == true)
                {
                    Settings.KickProtection = true;
                }
                else
                {
                    Settings.KickProtection = false;
                }

                if (Ini.GetBool("Protections", "BlockProtection") == true)
                {
                    Settings.BlockProtection = true;
                }
                else
                {
                    Settings.BlockProtection = false;
                }

                if (Ini.GetBool("Protections", "SteamSpoof") == true)
                {
                    Settings.SteamSpoof = true;
                }
                else
                {
                    Settings.SteamSpoof = false;
                }

                if (Ini.GetBool("Protections", "RPCBlock") == true)
                {
                    Settings.RPCBlock = true;
                }
                else
                {
                    Settings.RPCBlock = false;
                }

                if (Ini.GetBool("Protections", "AvatarCleaning") == true)
                {
                    Settings.AvatarCleaning = true;
                }
                else
                {
                    Settings.AvatarCleaning = false;
                }

                if (Ini.GetBool("Protections", "PickupProtection") == true)
                {
                    Settings.PickupsProtection = true;
                }
                else
                {
                    Settings.PickupsProtection = false;
                }

                if (Ini.GetBool("Protections", "IPProtection") == true)
                {
                    Settings.IpProtection = true;
                }
                else
                {
                    Settings.IpProtection = false;
                }

                if (Ini.GetBool("Protections", "EventProtection") == true)
                {
                    Settings.EventBlock = true;
                }
                else
                {
                    Settings.EventBlock = false;
                }

                if (Ini.GetBool("Protections", "EventProtection") == true)
                {
                    Settings.EventBlock = true;
                }
                else
                {
                    Settings.EventBlock = false;
                }

                if (Ini.GetBool("Protections", "Reflect") == true)
                {
                    Settings.Reflect = true;
                }
                else
                {
                    Settings.Reflect = false;
                }

                if (Ini.GetBool("Protections", "Moderation") == true)
                {
                    Settings.Moderation = true;
                }
                else
                {
                    Settings.Moderation = false;
                }

                if (Ini.GetBool("Protections", "TriggersProtection") == true)
                {
                    Settings.TriggersProtection = true;
                }
                else
                {
                    Settings.TriggersProtection = false;
                }

                if (Ini.GetBool("Protections", "BanProtection") == true)
                {
                    Settings.PublicBanProtection = true;
                }
                else
                {
                    Settings.PublicBanProtection = false;
                }

                if (Ini.GetBool("Protections", "UdonExploits") == true)
                {
                    Settings.UdonExploitProtections = true;
                }
                else
                {
                    Settings.UdonExploitProtections = false;
                }

                if (Ini.GetBool("Protections", "BotProtection") == true)
                {
                    Settings.BotProtection = true;
                }
                else
                {
                    Settings.BotProtection = false;
                }

                if (Ini.GetBool("Console", "EventLogs") == true)
                {
                    Settings.EventLog = true;
                }
                else
                {
                    Settings.EventLog = false;
                }

                if (Ini.GetBool("Console", "MyEventLogs") == true)
                {
                    Settings.MyEventsLog = true;
                }
                else
                {
                    Settings.MyEventsLog = false;
                }

                if (Ini.GetBool("Console", "RPCEventLogs") == true)
                {
                    Settings.RPCEventsLogs = true;
                }
                else
                {
                    Settings.RPCEventsLogs = false;
                }

                if (Ini.GetBool("Console", "UdonLogs") == true)
                {
                    Settings.UdonRPCLogs = true;
                }
                else
                {
                    Settings.UdonRPCLogs = false;
                }

                if (Ini.GetBool("Console", "ApiLogs") == true)
                {
                    Settings.ApiLogs = true;
                }
                else
                {
                    Settings.ApiLogs = false;
                }

                if (Ini.GetBool("Console", "Crashers") == true)
                {
                    Settings.CrashLogs = true;
                }
                else
                {
                    Settings.CrashLogs = false;
                }

                if (Ini.GetBool("Console", "Moderation") == true)
                {
                    Settings.ModerationLogs = true;
                }
                else
                {
                    Settings.ModerationLogs = false;
                }

                if (Ini.GetBool("Console", "Logger") == true)
                {
                    Settings.LoggerEnable = true;
                }
                else
                {
                    Settings.LoggerEnable = false;
                }

                if (Ini.GetBool("Console", "AvatarLogs") == true)
                {
                    Settings.AvatarLogs = true;
                }
                else
                {
                    Settings.AvatarLogs = false;
                }

                if (Ini.GetBool("World", "ComeBack") == true)
                {
                    Settings.ComeBack = true;
                    Settings.AlreadySavedData = true;
                }
                else
                {
                    Settings.ComeBack = false;
                }

                if (Ini.GetBool("World", "PostProcessing") == true)
                {
                    Settings.PostProcessing = true;
                }
                else
                {
                    Settings.PostProcessing = false;
                }

                if (Ini.GetBool("Discord", "HideWorld") == true)
                {
                    Settings.HideDiscordWorld = true;
                }
                else
                {
                    Settings.HideDiscordWorld = false;
                }

                if (Ini.GetBool("Discord", "HideName") == true)
                {
                    Settings.HideDiscordName = true;
                }
                else
                {
                    Settings.HideDiscordName = false;
                }

                if (Ini.GetBool("UwUMenu", "Touch") == true)
                {
                    Settings.GlobalTouch = true;
                }
                else
                {
                    Settings.GlobalTouch = false;
                }

                if (Ini.GetBool("UwUMenu", "HandColliders") == true)
                {
                    Settings.HandColliders = true;
                }
                else
                {
                    Settings.HandColliders = false;
                }

                if (Ini.GetBool("UwUMenu", "FeetColliders") == true)
                {
                    Settings.FeetColliders = true;
                }
                else
                {
                    Settings.FeetColliders = false;
                }

                if (Ini.GetBool("UwUMenu", "Head") == true)
                {
                    Settings.HeadBones = true;
                }
                else
                {
                    Settings.HeadBones = false;
                }

                if (Ini.GetBool("UwUMenu", "Chest") == true)
                {
                    Settings.ChestBones = true;
                }
                else
                {
                    Settings.ChestBones = false;
                }

                if (Ini.GetBool("UwUMenu", "Hips") == true)
                {
                    Settings.HipBones = true;
                }
                else
                {
                    Settings.HipBones = false;
                }

                if (Ini.GetBool("Misc", "LogAvatars") == true)
                {
                    Settings.LogAvatars = true;
                }
                else
                {
                    Settings.LogAvatars = false;
                }

                if (Ini.GetBool("Misc", "UnlimitedFrames") == true)
                {
                    Settings.UnlimitedFrames = true;
                }
                else
                {
                    Settings.UnlimitedFrames = false;
                }

                if (Ini.GetBool("Misc", "NotifyFriendJoin") == true)
                {
                    Settings.NotifyFriend = true;
                }
                else
                {
                    Settings.NotifyFriend = false;
                }

                if (Ini.GetBool("Misc", "NoStats") == true)
                {
                    Settings.NoStats = true;
                }
                else
                {
                    Settings.NoStats = false;
                }

                if (Ini.GetBool("Oversee", "Enabled") == true)
                {
                    Settings.Oversee = true;
                }
                else
                {
                    Settings.Oversee = false;
                }

                if (Ini.GetBool("RPCList", "Enabled") == true)
                {
                    Settings.RPCList = true;
                }
                else
                {
                    Settings.RPCList = false;
                }

                if (Ini.GetBool("UdonRPCList", "Enabled") == true)
                {
                    Settings.UdonRPCList = true;
                }
                else
                {
                    Settings.UdonRPCList = false;
                }

                if (Ini.GetBool("Self", "QuestSpoof") == true)
                {
                    Settings.QuestSpoof = true;
                }
                else
                {
                    Settings.QuestSpoof = false;
                }

                if (Ini.GetBool("Misc", "FullScreenOnStart").ToString() != null && Ini.GetBool("Misc", "FullScreenOnStart") == true)
                {
                    Settings.FullScreenOnStart = true;
                }
                else
                {
                    Settings.FullScreenOnStart = false;
                }

                //Color
                if (Ini.GetBool("UIColor", "SaveUIColor") == true)
                {
                    Settings.SaveUIColor = true;
                }
                else
                {
                    Settings.SaveUIColor = false;
                }

                if (Settings.SaveUIColor)
                {
                    var MainColor = new Color(Ini.GetFloat("UIColor", "R"), Ini.GetFloat("UIColor", "G"), Ini.GetFloat("UIColor", "B"));
                    Settings.UIColorHex = Settings.ColorToHex(MainColor, true);
                    var SecColor = new Color(Ini.GetFloat("UISecColor", "R"), Ini.GetFloat("UISecColor", "G"), Ini.GetFloat("UISecColor", "B"));
                    Settings.UITextColorHex = Settings.ColorToHex(SecColor, true);
                }
            }
        }
    }
}