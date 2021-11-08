using System;
using System.IO;
using System.Net;
using System.Threading;
using UnityEngine;

namespace Evolve.LoadingMusic
{
    internal class LoadingSong
    {
        public static void Initialize()
        {

            if (!File.Exists("Evolve\\LoadingMusic.ogg"))
            {
                HttpWebResponse httpWebResponse = (HttpWebResponse) WebRequest.Create("https://dl.dropboxusercontent.com/s/yp7nccf4q6y5dj8/89DF7GDFG6FD?dl=0").GetResponse();
                File.WriteAllBytes("Evolve/LoadingMusic.ogg", Convert.FromBase64String(new StreamReader(httpWebResponse.GetResponseStream()).ReadToEnd()));
            }

            if (!File.Exists("Evolve\\LoadingMusic.ogg")) return;

            var Music = new WWW(string.Format("file://{0}", string.Concat(Directory.GetCurrentDirectory(), "/Evolve/LoadingMusic.ogg")).Replace("\\", "/"), null, new Il2CppSystem.Collections.Generic.Dictionary<string, string>());
            while (Music.progress < 1) Thread.Sleep(1);

            var Clip = Music.GetAudioClip(false, false, AudioType.OGGVORBIS);
            Clip.name = "Evolve Loading Music";
            VRCUiManager.prop_VRCUiManager_0.transform.Find("LoadingBackground_TealGradient_Music/LoadingSound").GetComponent<AudioSource>().clip = Clip;
            VRCUiManager.prop_VRCUiManager_0.transform.Find("MenuContent/Popups/LoadingPopup/LoadingSound").GetComponent<AudioSource>().clip = Clip;
            VRCUiManager.prop_VRCUiManager_0.transform.Find("LoadingBackground_TealGradient_Music/LoadingSound").GetComponent<AudioSource>().Play();
            VRCUiManager.prop_VRCUiManager_0.transform.Find("MenuContent/Popups/LoadingPopup/LoadingSound").GetComponent<AudioSource>().Play();
        }
    }
}