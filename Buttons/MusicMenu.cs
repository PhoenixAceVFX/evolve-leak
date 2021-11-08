using Evolve.Api;
using MelonLoader;
using ButtonApi;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Evolve.Buttons
{
    internal class MusicMenu
    {
        public static QMNestedButton ThisMenu;
        public static QMSingleButton Spotify1;
        public static QMSingleButton Spotify2;
        public static QMSingleButton Spotify3;
        public static QMSingleButton Spotify4;
        public static QMSingleButton Youtube1;
        public static QMSingleButton Youtube2;
        public static QMSingleButton Youtube3;
        public static QMSingleButton Youtube4;
#pragma warning disable CS0649 // Le champ 'MusicMenu.Config' n'est jamais assigné et aura toujours sa valeur par défaut null
        public static QMSingleButton Config;
#pragma warning restore CS0649 // Le champ 'MusicMenu.Config' n'est jamais assigné et aura toujours sa valeur par défaut null
#pragma warning disable CS0649 // Le champ 'MusicMenu.VolumeUpButton' n'est jamais assigné et aura toujours sa valeur par défaut null
        public static QMSingleButton VolumeUpButton;
#pragma warning restore CS0649 // Le champ 'MusicMenu.VolumeUpButton' n'est jamais assigné et aura toujours sa valeur par défaut null
#pragma warning disable CS0649 // Le champ 'MusicMenu.VolumeDownButton' n'est jamais assigné et aura toujours sa valeur par défaut null
        public static QMSingleButton VolumeDownButton;
#pragma warning restore CS0649 // Le champ 'MusicMenu.VolumeDownButton' n'est jamais assigné et aura toujours sa valeur par défaut null
#pragma warning disable CS0649 // Le champ 'MusicMenu.Slider1' n'est jamais assigné et aura toujours sa valeur par défaut null
        public static Text Slider1;
#pragma warning restore CS0649 // Le champ 'MusicMenu.Slider1' n'est jamais assigné et aura toujours sa valeur par défaut null

        public static QMSingleButton Spotify1Settings;
        public static QMSingleButton Spotify2Settings;
        public static QMSingleButton Spotify3Settings;
        public static QMSingleButton Spotify4Settings;
        public static QMSingleButton Youtube1Settings;
        public static QMSingleButton Youtube2Settings;
        public static QMSingleButton Youtube3Settings;
        public static QMSingleButton Youtube4Settings;

        [DllImport("user32.dll")]
        public static extern void keybd_event(byte virtualKey, byte scanCode, uint flags, IntPtr extraInfo);

        public const int KEYEVENTF_EXTENTEDKEY = 1;
        public const int KEYEVENTF_KEYUP = 0;
        public const int VK_MEDIA_NEXT_TRACK = 0xB0;
        public const int VK_MEDIA_PLAY_PAUSE = 0xB3;
        public const int VK_MEDIA_PREV_TRACK = 0xB1;
        public const int VK_VOLUME_UP = 175;
        public const int VK_VOLUME_DOWN = 174;

        public static void Initialize()
        {
            ThisMenu = new QMNestedButton("ShortcutMenu", 0, 0, "Music", "Music Menu", Color.cyan, Color.magenta, Color.black, Color.yellow);
            ThisMenu.getMainButton().getGameObject().SetActive(false);
            Panels.PanelMenu(ThisMenu, 0, 0.3f, "\nSpotify:", 1.1f, 2.4F, "Spotify playlists");
            Panels.PanelMenu(ThisMenu, 5, 0.3f, "\nYoutube:", 1.1f, 2.4F, "Youtube playlists");
            if (FoldersManager.Config.Ini.GetString("Music Playlist", "Spotify1Name") == "") FoldersManager.Config.Ini.SetString("Music Playlist", "Spotify1Name", "Example");
            if (FoldersManager.Config.Ini.GetString("Music Playlist", "Spotify2Name") == "") FoldersManager.Config.Ini.SetString("Music Playlist", "Spotify2Name", "2");
            if (FoldersManager.Config.Ini.GetString("Music Playlist", "Spotify3Name") == "") FoldersManager.Config.Ini.SetString("Music Playlist", "Spotify3Name", "3");
            if (FoldersManager.Config.Ini.GetString("Music Playlist", "Spotify4Name") == "") FoldersManager.Config.Ini.SetString("Music Playlist", "Spotify4Name", "4");

            if (FoldersManager.Config.Ini.GetString("Music Playlist", "Youtube1Name") == "") FoldersManager.Config.Ini.SetString("Music Playlist", "Youtube1Name", "Example");
            if (FoldersManager.Config.Ini.GetString("Music Playlist", "Youtube2Name") == "") FoldersManager.Config.Ini.SetString("Music Playlist", "Youtube2Name", "2");
            if (FoldersManager.Config.Ini.GetString("Music Playlist", "Youtube3Name") == "") FoldersManager.Config.Ini.SetString("Music Playlist", "Youtube3Name", "3");
            if (FoldersManager.Config.Ini.GetString("Music Playlist", "Youtube4Name") == "") FoldersManager.Config.Ini.SetString("Music Playlist", "Youtube4Name", "4");

            if (FoldersManager.Config.Ini.GetString("Music Playlist", "Spotify1Url") == "") FoldersManager.Config.Ini.SetString("Music Playlist", "Spotify1Url", "spotify:user:ut50zuhk5ilobz4mkvm0fxfik:playlist:48D6aWRILuFhi6ZTSbmQyk");
            if (FoldersManager.Config.Ini.GetString("Music Playlist", "Spotify2Url") == "") FoldersManager.Config.Ini.SetString("Music Playlist", "Spotify2Url", "2");
            if (FoldersManager.Config.Ini.GetString("Music Playlist", "Spotify3Url") == "") FoldersManager.Config.Ini.SetString("Music Playlist", "Spotify3Url", "3");
            if (FoldersManager.Config.Ini.GetString("Music Playlist", "Spotify4Url") == "") FoldersManager.Config.Ini.SetString("Music Playlist", "Spotify4Url", "4");

            if (FoldersManager.Config.Ini.GetString("Music Playlist", "Youtube1Url") == "") FoldersManager.Config.Ini.SetString("Music Playlist", "Youtube1Url", "https://www.youtube.com/watch?v=p3_QKwJxpC4&list=PL78PCwwep8XNDQM9Laux9x8L6SqUIl0VW");
            if (FoldersManager.Config.Ini.GetString("Music Playlist", "Youtube2Url") == "") FoldersManager.Config.Ini.SetString("Music Playlist", "Youtube2Url", "2");
            if (FoldersManager.Config.Ini.GetString("Music Playlist", "Youtube3Url") == "") FoldersManager.Config.Ini.SetString("Music Playlist", "Youtube3Url", "3");
            if (FoldersManager.Config.Ini.GetString("Music Playlist", "Youtube4Url") == "") FoldersManager.Config.Ini.SetString("Music Playlist", "Youtube4Url", "4");
            MelonCoroutines.Start(ReworkedButtonAPI.CreateButton(MenuType.ShortCut, ButtonType.Single, "", 0, 0, delegate ()
            {
                QMStuff.ShowQuickmenuPage(ThisMenu.getMenuName());
            }, "Music Menu", Color.black, Color.clear, null, "https://i.imgur.com/6o4ZCZr.png"));

            MelonCoroutines.Start(ReworkedButtonAPI.CreateButton(MenuType.Music, ButtonType.Single, "", 1.5f, 0.5f, delegate ()
            {
                keybd_event(VK_MEDIA_PREV_TRACK, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
            }, "Previous", Color.black, Color.clear, null, "https://i.imgur.com/hj6J8eR.png"));

            MelonCoroutines.Start(ReworkedButtonAPI.CreateButton(MenuType.Music, ButtonType.Single, "", 2.5f, 0.5f, delegate ()
            {
                keybd_event(VK_MEDIA_PLAY_PAUSE, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
            }, "Play/Pause", Color.black, Color.clear, null, "https://i.imgur.com/mOTfExO.png"));

            MelonCoroutines.Start(ReworkedButtonAPI.CreateButton(MenuType.Music, ButtonType.Single, "", 3.5f, 0.5f, delegate ()
            {
                keybd_event(VK_MEDIA_NEXT_TRACK, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
            }, "Next", Color.black, Color.clear, null, "https://i.imgur.com/yn03nW1.png"));

            Spotify1 = new QMSingleButton(ThisMenu, 0, -0.25f, FoldersManager.Config.Ini.GetString("Music Playlist", "Spotify1Name"), () =>
            {
                Process.Start(FoldersManager.Config.Ini.GetString("Music Playlist", "Spotify1Url"));
            }, "Launch this playlist");
            Spotify1.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            Spotify2 = new QMSingleButton(ThisMenu, 0, 0.25f, FoldersManager.Config.Ini.GetString("Music Playlist", "Spotify2Name"), () =>
            {
                Process.Start(FoldersManager.Config.Ini.GetString("Music Playlist", "Spotify2Url"));
            }, "Launch this playlist");
            Spotify2.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            Spotify3 = new QMSingleButton(ThisMenu, 0, 0.75f, FoldersManager.Config.Ini.GetString("Music Playlist", "Spotify3Name"), () =>
            {
                Process.Start(FoldersManager.Config.Ini.GetString("Music Playlist", "Spotify3Url"));
            }, "Launch this playlist");
            Spotify3.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            Spotify4 = new QMSingleButton(ThisMenu, 0, 1.25f, FoldersManager.Config.Ini.GetString("Music Playlist", "Spotify4Name"), () =>
            {
                Process.Start(FoldersManager.Config.Ini.GetString("Music Playlist", "Spotify4Url"));
            }, "Launch this playlist");
            Spotify4.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            //Youtube

            Youtube1 = new QMSingleButton(ThisMenu, 5, -0.25f, FoldersManager.Config.Ini.GetString("Music Playlist", "Youtube1Name"), () =>
            {
                Process.Start(FoldersManager.Config.Ini.GetString("Music Playlist", "Youtube1Url"));
            }, "Launch this playlist");
            Youtube1.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            Youtube2 = new QMSingleButton(ThisMenu, 5, 0.25f, FoldersManager.Config.Ini.GetString("Music Playlist", "Youtube2Name"), () =>
            {
                Process.Start(FoldersManager.Config.Ini.GetString("Music Playlist", "Youtube2Url"));
            }, "Launch this playlist");
            Youtube2.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            Youtube3 = new QMSingleButton(ThisMenu, 5, 0.75f, FoldersManager.Config.Ini.GetString("Music Playlist", "Youtube3Name"), () =>
            {
                Process.Start(FoldersManager.Config.Ini.GetString("Music Playlist", "Youtube3Url"));
            }, "Launch this playlist");
            Youtube3.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            Youtube4 = new QMSingleButton(ThisMenu, 5, 1.25f, FoldersManager.Config.Ini.GetString("Music Playlist", "Youtube4Name"), () =>
            {
                Process.Start(FoldersManager.Config.Ini.GetString("Music Playlist", "Youtube4Url"));
            }, "Launch this playlist");
            Youtube4.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            //Settings
            Spotify1Settings = new QMSingleButton(ThisMenu, 0.75f, -0.25f, "*", () =>
            {
                Wrappers.PopupManager.InputeText("Enter Name", "Confirm", (Name) =>
                {
                    MelonCoroutines.Start(Wait());
                    IEnumerator Wait()
                    {
                        yield return new WaitForSeconds(1);
                        Wrappers.PopupManager.InputeText("Enter Url\nExample: spotify:user:username:playlist:id", "Confirm", (Url) =>
                        {
                            FoldersManager.Config.Ini.SetString("Music Playlist", "Spotify1Name", Name);
                            FoldersManager.Config.Ini.SetString("Music Playlist", "Spotify1Url", Url);
                            Spotify1.setButtonText(Name);
                        });
                    }
                });
            }, "Set Playlist");
            Spotify1Settings.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(2, 2);

            Spotify2Settings = new QMSingleButton(ThisMenu, 0.75f, 0.25f, "*", () =>
            {
                Wrappers.PopupManager.InputeText("Enter Name", "Confirm", (Name) =>
                {
                    MelonCoroutines.Start(Wait());
                    IEnumerator Wait()
                    {
                        yield return new WaitForSeconds(1);
                        Wrappers.PopupManager.InputeText("Enter Url\nExample: spotify:user:username:playlist:id", "Confirm", (Url) =>
                        {
                            FoldersManager.Config.Ini.SetString("Music Playlist", "Spotify2Name", Name);
                            FoldersManager.Config.Ini.SetString("Music Playlist", "Spotify2Url", Url);
                            Spotify2.setButtonText(Name);
                        });
                    }
                });
            }, "Set Playlist");
            Spotify2Settings.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(2, 2);

            Spotify3Settings = new QMSingleButton(ThisMenu, 0.75f, 0.75f, "*", () =>
            {
                Wrappers.PopupManager.InputeText("Enter Name", "Confirm", (Name) =>
                {
                    MelonCoroutines.Start(Wait());
                    IEnumerator Wait()
                    {
                        yield return new WaitForSeconds(1);
                        Wrappers.PopupManager.InputeText("Enter Url\nExample: spotify:user:username:playlist:id", "Confirm", (Url) =>
                        {
                            FoldersManager.Config.Ini.SetString("Music Playlist", "Spotify3Name", Name);
                            FoldersManager.Config.Ini.SetString("Music Playlist", "Spotify3Url", Url);
                            Spotify3.setButtonText(Name);
                        });
                    }
                });
            }, "Set Playlist");
            Spotify3Settings.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(2, 2);

            Spotify4Settings = new QMSingleButton(ThisMenu, 0.75f, 1.25f, "*", () =>
            {
                Wrappers.PopupManager.InputeText("Enter Name", "Confirm", (Name) =>
                {
                    MelonCoroutines.Start(Wait());
                    IEnumerator Wait()
                    {
                        yield return new WaitForSeconds(1);
                        Wrappers.PopupManager.InputeText("Enter Url\nExample: spotify:user:username:playlist:id", "Confirm", (Url) =>
                        {
                            FoldersManager.Config.Ini.SetString("Music Playlist", "Spotify4Name", Name);
                            FoldersManager.Config.Ini.SetString("Music Playlist", "Spotify4Url", Url);
                            Spotify4.setButtonText(Name);
                        });
                    }
                });
            }, "Set Playlist");
            Spotify4Settings.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(2, 2);

            Youtube1Settings = new QMSingleButton(ThisMenu, 4.25f, -0.25f, "*", () =>
            {
                Wrappers.PopupManager.InputeText("Enter Name", "Confirm", (Name) =>
                {
                    MelonCoroutines.Start(Wait());
                    IEnumerator Wait()
                    {
                        yield return new WaitForSeconds(1);
                        Wrappers.PopupManager.InputeText("Enter Url\nExample: https://www.youtube.com/watch?ID", "Confirm", (Url) =>
                        {
                            FoldersManager.Config.Ini.SetString("Music Playlist", "Youtube1Name", Name);
                            FoldersManager.Config.Ini.SetString("Music Playlist", "Youtube1Url", Url);
                            Youtube1.setButtonText(Name);
                        });
                    }
                });
            }, "Set Playlist");
            Youtube1Settings.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(2, 2);

            Youtube2Settings = new QMSingleButton(ThisMenu, 4.25f, 0.25f, "*", () =>
            {
                Wrappers.PopupManager.InputeText("Enter Name", "Confirm", (Name) =>
                {
                    MelonCoroutines.Start(Wait());
                    IEnumerator Wait()
                    {
                        yield return new WaitForSeconds(1);
                        Wrappers.PopupManager.InputeText("Enter Url\nExample: https://www.youtube.com/watch?ID", "Confirm", (Url) =>
                        {
                            FoldersManager.Config.Ini.SetString("Music Playlist", "Youtube2Name", Name);
                            FoldersManager.Config.Ini.SetString("Music Playlist", "Youtube2Url", Url);
                            Youtube2.setButtonText(Name);
                        });
                    }
                });
            }, "Set Playlist");
            Youtube2Settings.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(2, 2);

            Youtube3Settings = new QMSingleButton(ThisMenu, 4.25f, 0.75f, "*", () =>
            {
                Wrappers.PopupManager.InputeText("Enter Name", "Confirm", (Name) =>
                {
                    MelonCoroutines.Start(Wait());
                    IEnumerator Wait()
                    {
                        yield return new WaitForSeconds(1);
                        Wrappers.PopupManager.InputeText("Enter Url\nExample: https://www.youtube.com/watch?ID", "Confirm", (Url) =>
                        {
                            FoldersManager.Config.Ini.SetString("Music Playlist", "Youtube3Name", Name);
                            FoldersManager.Config.Ini.SetString("Music Playlist", "Youtube3Url", Url);
                            Youtube3.setButtonText(Name);
                        });
                    }
                });
            }, "Set Playlist");
            Youtube3Settings.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(2, 2);

            Youtube4Settings = new QMSingleButton(ThisMenu, 4.25f, 1.25f, "*", () =>
            {
                Wrappers.PopupManager.InputeText("Enter Name", "Confirm", (Name) =>
                {
                    MelonCoroutines.Start(Wait());
                    IEnumerator Wait()
                    {
                        yield return new WaitForSeconds(1);
                        Wrappers.PopupManager.InputeText("Enter Url\nExample: https://www.youtube.com/watch?ID", "Confirm", (Url) =>
                        {
                            FoldersManager.Config.Ini.SetString("Music Playlist", "Youtube4Name", Name);
                            FoldersManager.Config.Ini.SetString("Music Playlist", "Youtube4Url", Url);
                            Youtube4.setButtonText(Name);
                        });
                    }
                });
            }, "Set Playlist");
            Youtube4Settings.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(2, 2);
        }
    }
}