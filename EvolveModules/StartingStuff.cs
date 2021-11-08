using Evolve.Api;
using Evolve.Buttons;
using Evolve.ConsoleUtils;
using Evolve.Login;
using Evolve.Patch;
using Evolve.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using VRC.Core;
using Evolve.Wrappers;

namespace Evolve.Modules
{
    class StartingStuff
    {
        public static IEnumerator StartSong()
        {
            if (!Directory.Exists("Evolve/Sounds"))
            {
                Directory.CreateDirectory("Evolve/Sounds");
            }
            WebClient webClient = new WebClient();
            webClient.DownloadFileAsync(new Uri("https://dl.dropboxusercontent.com/s/efabp8jber7fq2k/0G76D5R988HS.ogg?dl=0"), "Evolve/Sounds/LaunchSong.ogg");
            while (VRCPlayer.field_Internal_Static_VRCPlayer_0 != true) yield return null;
            VRConsoleTitle();
            Notifications.Notify($"Welcome back <b><color=#00fff7>{APIUser.CurrentUser.displayName}</color></b>\nSubscription: <b><color=#00fff7>{Login.Authorization.Subscribtion}</color></b>");
            yield return new WaitForSeconds(1);
            GameObject AudioObject = new GameObject();
            AudioObject.transform.position = VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position;
            AudioObject.AddComponent<AudioSource>();
            var audioLoader = new WWW(string.Format("file://{0}", string.Concat(Directory.GetCurrentDirectory(), "/Evolve/Sounds/LaunchSong.ogg")).Replace("\\", "/"), null, new Il2CppSystem.Collections.Generic.Dictionary<string, string>());
            yield return audioLoader;
            audioLoader.GetAudioClip().name = "Evolve Launch Sound";
            AudioObject.GetComponent<AudioSource>().clip = audioLoader.GetAudioClip(false, false, AudioType.OGGVORBIS);
            AudioObject.GetComponent<AudioSource>().volume = 0.2f;
            AudioObject.GetComponent<AudioSource>().Play();
        }

        public static IEnumerator Time()
        {
            while (VRCPlayer.field_Internal_Static_VRCPlayer_0 != true) yield return null;
            while (EvolveMenu.LowerPanel.getGameObject() == null) yield return null;
            while (true)
            {
                yield return new WaitForSeconds(1);
                try
                {
                    FoldersManager.Config.Ini.SetInt("Misc", "TotalSpentTime", FoldersManager.Config.Ini.GetInt("Misc", "TotalSpentTime") + 1);
                }
                catch { }
            }
        }

        public static IEnumerator LowerPanel()
        {
            while (VRCPlayer.field_Internal_Static_VRCPlayer_0 != true) yield return null;
            while (EvolveMenu.LowerPanel.getGameObject() == null) yield return null;
            MenuText TimeText = new MenuText(EvolveMenu.LowerPanel.getGameObject().transform, 1030, 205, "Time: 0:0:0");
            TimeText.SetColor(Settings.MenuColor());
            EvolveMenu.NotifText.SetColor(Settings.MenuColor());
            while (true)
            {
                yield return new WaitForSeconds(1);
                try
                {
                    DateTime localDate = DateTime.Now;
                    TimeText.SetText($"Time: {localDate.Hour}:{localDate.Minute}:{localDate.Second}");
                }
                catch { }
            }
        }
        public static void VRConsoleTitle()
        {
            string SpentTime = "0 days, 0 hours and 0 minutes";
            int Seconds = FoldersManager.Config.Ini.GetInt("Misc", "TotalSpentTime");
            TimeSpan Time = TimeSpan.FromSeconds(Seconds);
            SpentTime = $"{Time.Days} days, {Time.Hours} hours and {Time.Minutes} minutes";
            EvoVrConsole.Log(EvoVrConsole.LogsType.Empty, @"<color=#ff007a>=========================================================================================</color>");
            EvoVrConsole.Log(EvoVrConsole.LogsType.Empty, "");
            EvoVrConsole.Log(EvoVrConsole.LogsType.Empty, @$"<color=#fc0086>Welcome back {APIUser.CurrentUser.displayName} !</color>");
            EvoVrConsole.Log(EvoVrConsole.LogsType.Empty, @$"<color=#fc0086>Subscription: {Login.Authorization.Subscribtion}</color>");
            EvoVrConsole.Log(EvoVrConsole.LogsType.Empty, @$"<color=#f60094>You used Evolve for {SpentTime} in total !</color>");
            EvoVrConsole.Log(EvoVrConsole.LogsType.Empty, @$"<color=#f60094>Successfully spoofed your HWID: {SystemInfo.deviceUniqueIdentifier}</color>");
            EvoVrConsole.Log(EvoVrConsole.LogsType.Empty, @$"<color=#ed00a3>Successfully spoofed your Device name: {SystemInfo.deviceName}</color>");
            EvoVrConsole.Log(EvoVrConsole.LogsType.Empty, @$"<color=#b800d9>Successfully spoofed your Graphic name: {SystemInfo.graphicsDeviceName}</color>");
            EvoVrConsole.Log(EvoVrConsole.LogsType.Empty, @$"<color=#b800d9>Successfully spoofed your Graphic Id: {SystemInfo.graphicsDeviceID}</color>");
            EvoVrConsole.Log(EvoVrConsole.LogsType.Empty, @$"<color=#9700ec>Successfully spoofed your Graphic vendor id: {SystemInfo.graphicsDeviceVendorID}</color>");
            if (Patches.PatchSucess) EvoVrConsole.Log(EvoVrConsole.LogsType.Empty, @$"<color=#9700ec>Successfully applied {Patches.MainPatchesCount + Patches.Instance.GetPatchedMethods().ToList().Count + 3} patches</color>");
            else EvoVrConsole.Log (EvoVrConsole.LogsType.Empty, $"<color=#red>Error when patching methods please report this to 乂 _ 乂</color>");
            EvoVrConsole.Log(EvoVrConsole.LogsType.Empty, @"");
            EvoVrConsole.Log(EvoVrConsole.LogsType.Empty, @"<color=#6400ff>Ready to go.</color>");
            EvoVrConsole.Log(EvoVrConsole.LogsType.Empty, @"<color=#6400ff>Have fun !</color>");
            EvoVrConsole.Log(EvoVrConsole.LogsType.Empty, "");
            EvoVrConsole.Log(EvoVrConsole.LogsType.Empty, @"<color=#6400ff>=========================================================================================</color>");
            EvoVrConsole.Log(EvoVrConsole.LogsType.Empty, "");
            EvoVrConsole.Log(EvoVrConsole.LogsType.Empty, "");

            try
            {
                foreach (var Log in EvoVrConsole.AllLogsText)
                {
                    Log.fontStyle = FontStyle.Normal;
                }
            }
            catch { }
        }
    }
}
