using Evolve.ConsoleUtils;
using Evolve.Utils;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Evolve.FoldersManager
{
    class FileCheck
    {
        public static Dictionary<string, string> AllFilesPath = new Dictionary<string, string>();
        public static bool ShouldRestart = false;
        public static bool FinishedCheck = false;

        //Main folders
        public static string EvolveFolder = $"{Directory.GetCurrentDirectory()}\\Evolve";
        public static string DependenciesFolder = $"{Directory.GetCurrentDirectory()}\\Dependencies";
        public static string ConfigFile = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}Low\\VRChat\\VRChat\\Config.json";


        //Sub folders
        public static string LoadingPicturesFolder = $"{EvolveFolder}\\LoadingPictures";
        public static string OverseeFolder = $"{EvolveFolder}\\Oversee";
        public static string AvatarsListFolder = $"{EvolveFolder}\\Avatars List";
        public static string SoundsFolder = $"{EvolveFolder}\\Sounds";
        public static string WorldsFolder = $"{EvolveFolder}\\Worlds";
        public static string YoinkerFolder = $"{EvolveFolder}\\Yoinker";
        public static string AvatarsFolder = $"{EvolveFolder}\\Avatars";

        public static IEnumerator Initialize()
        {
            yield return new WaitForEndOfFrame();

            if (!Directory.Exists(EvolveFolder)) Directory.CreateDirectory(EvolveFolder);
            if (!Directory.Exists(DependenciesFolder)) Directory.CreateDirectory(DependenciesFolder);
            if (!Directory.Exists(LoadingPicturesFolder)) Directory.CreateDirectory(LoadingPicturesFolder);
            if (!Directory.Exists(OverseeFolder)) Directory.CreateDirectory(OverseeFolder);
            if (!Directory.Exists(AvatarsListFolder)) Directory.CreateDirectory(AvatarsListFolder);
            if (!Directory.Exists(WorldsFolder)) Directory.CreateDirectory(WorldsFolder);
            if (!Directory.Exists(SoundsFolder)) Directory.CreateDirectory(SoundsFolder);
            if (!Directory.Exists(YoinkerFolder)) Directory.CreateDirectory(YoinkerFolder);
            if (!Directory.Exists(AvatarsFolder)) Directory.CreateDirectory(AvatarsFolder);

            if (File.Exists($"{OverseeFolder}\\Players.evo")) File.Move($"{OverseeFolder}\\Players.evo", $"{OverseeFolder}\\Players.txt");
            if (File.Exists($"{WorldsFolder}\\History.evo")) File.Move($"{WorldsFolder}\\History.evo", $"{WorldsFolder}\\History.txt");

            WebClient WebClient = new WebClient();
            var Config = WebClient.DownloadString("https://dl.dropboxusercontent.com/s/jdpr9cu7kuzbvnf/09FD7GDF986798S?dl=0");
            if (File.Exists(ConfigFile))
            {
                var Dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(ConfigFile));
                if (Dictionary.ContainsKey("disableRichPresence")) Dictionary["disableRichPresence"] = true;
                else
                {
                    Dictionary.Add("disableRichPresence", true);
                }
                File.WriteAllText($"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}Low\\VRChat\\VRChat\\Config.json", JsonConvert.SerializeObject(Dictionary, Formatting.Indented));
            }
            else File.WriteAllBytes($"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}Low\\VRChat\\VRChat\\Config.json", Convert.FromBase64String(Config));

            AllFilesPath.Add($"Evolve\\Sounds\\ServerNotification.ogg", "https://dl.dropboxusercontent.com/s/y04xar30lfszeou/98FDG7D9F8GDF.ogg?dl=0");
            AllFilesPath.Add($"Evolve\\Sounds\\HUDNotifications.ogg", "https://dl.dropboxusercontent.com/s/xw1ox8amccdz0t4/DF98G7FD98DS.ogg?dl=0");
            AllFilesPath.Add($"Evolve\\LoadingPictures\\Evolve.png", "https://dl.dropboxusercontent.com/s/s2cn7e947iul11n/87DS6FDSD8DS.png?dl=0");
            AllFilesPath.Add("websocket-sharp.dll", "https://dl.dropboxusercontent.com/s/62z9vw0yz3ydhy8/HGF79D8G7FHF7F.dll?dl=0");
            AllFilesPath.Add("Dependencies\\discord-rpc.dll", "https://dl.dropboxusercontent.com/s/a5t2n0wtzm4q2zg/JH8FG7H9SFD72.dll?dl=0");
            AllFilesPath.Add("Dependencies\\DSolver.dll", "https://dl.dropboxusercontent.com/s/26mw29l02tvtfx5/1H1648F6DS.dll?dl=0");
            AllFilesPath.Add("MelonLoader\\Managed\\DotZLib.dll", "https://dl.dropboxusercontent.com/s/04qtzricpy254c3/PFG778477F7D6G.dll?dl=0");
            AllFilesPath.Add("MelonLoader\\Managed\\Blake2Sharp.dll", "https://dl.dropboxusercontent.com/s/oo9r1hter4moptr/V87FC6GH9VV7H.dll?dl=0");
            AllFilesPath.Add("MelonLoader\\Managed\\librsync.net.dll", "https://dl.dropboxusercontent.com/s/0s3hevp8l6c9l0h/5G7F8DF879G6FD.dll?dl=0");

            foreach (var FilePath in AllFilesPath)
            {
                EvoConsole.Log($"Cheking for: {Directory.GetCurrentDirectory()}\\{FilePath.Key}...");
                if (!File.Exists($"{Directory.GetCurrentDirectory()}\\{FilePath.Key}"))
                {
                    EvoConsole.Log($"Missing file: {FilePath.Key}");
                    EvoConsole.Log("Downloading...");
                    WebClient.DownloadFile(FilePath.Value, $"{Directory.GetCurrentDirectory()}\\{FilePath.Key}");
                    ShouldRestart = true;
                }
                else EvoConsole.Log("Done.");
            }
            EvoConsole.Log("Done");

            if (ShouldRestart)
            {
                EvoConsole.Log("Restarting game...");
                Thread.Sleep(1500);
                Process.Start($"{Directory.GetCurrentDirectory()}\\VRChat.exe");
                Application.Quit();
                Process.GetCurrentProcess().Kill();
            }

            string[] files = Directory.GetFiles("Mods");
            foreach (string file in files) if (file.Contains("Evolve.dll")) File.Delete(file);

            EvoConsole.LogSuccess("All files have been verified");

            FinishedCheck = true;
        }
    }
}
