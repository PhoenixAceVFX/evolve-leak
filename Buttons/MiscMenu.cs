using System.Collections;
using System.Diagnostics;
using Evolve.Api;
using Evolve.ConsoleUtils;
using Evolve.Modules;
using Evolve.Utils;
using MelonLoader;
using Mono.CSharp;
using ButtonApi;
using UnityEngine;
using UnityEngine.UI;
using Evolve.Wrappers;
using Evolve.Module;
using Evolve.FoldersManager;

namespace Evolve.Buttons
{
    internal class MiscMenu
    {
        public static QMNestedButton ThisMenu;
        public static GameObject Slider1;
        public static GameObject Slider2;
        public static GameObject Slider3;
        public static GameObject Slider4;
        public static GameObject Slider5;
        public static float ComfortDef = 200f;
        public static float MouseDef = 200;
        public static float DefaultMicSensivity = 100;
        public static float MicSensivity = 100;
        public static bool Mods = true;

        public static void Initialize()
        {
            ThisMenu = new QMNestedButton(EvolveMenu.ThisMenu, 3.5f, 1.75f, "Misc", "Misc menu");
            ThisMenu.getMainButton().getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            new QMToggleButton(ThisMenu, 0, 0, "Notify on\nFriend join/left", () =>
            {
                Settings.NotifyFriend = true;
                FoldersManager.Config.Ini.SetBool("Misc", "NotifyFriendJoin", true);

            }, "Disabled", () =>
            {
                Settings.NotifyFriend = false;
                FoldersManager.Config.Ini.SetBool("Misc", "NotifyFriendJoin", false);
            }, "Set the current process priority", null, null, false, Settings.NotifyFriend);

            new QMToggleButton(ThisMenu, 1, 0, "Priority High", () =>
            {
                using (Process p = Process.GetCurrentProcess())
                    p.PriorityClass = ProcessPriorityClass.High;

            }, "Priority Normal", () =>
            {
                using (Process p = Process.GetCurrentProcess())
                    p.PriorityClass = ProcessPriorityClass.Normal;
            }, "Set the current process priority", null, null, false, false);

            new QMToggleButton(ThisMenu, 2, 0, "Full screen \n(On start)", () =>
            {
                FoldersManager.Config.Ini.SetBool("Misc", "FullScreenOnStart", true);
            }, "Disabled", () =>
            {
                FoldersManager.Config.Ini.SetBool("Misc", "FullScreenOnStart", false);
            }, "", null, null, false, Settings.FullScreenOnStart);

            new QMToggleButton(ThisMenu, 3, 0, "Unlimited\nFrames", () =>
            {
                FoldersManager.Config.Ini.SetBool("Misc", "UnlimitedFrames", true);
                Settings.UnlimitedFrames = true;
                Application.targetFrameRate = 999;
            }, "Disabled", () =>
            {
                FoldersManager.Config.Ini.SetBool("Misc", "UnlimitedFrames", false);
                Settings.UnlimitedFrames = false;
                Application.targetFrameRate = 90;
            }, "", null, null, false, Settings.UnlimitedFrames);

            new QMToggleButton(ThisMenu, 0, 1, "No stats", () =>
            {
                FoldersManager.Config.Ini.SetBool("Misc", "NoStats", true);
                Settings.NoStats = true;
            }, "Yes stats", () =>
            {
                FoldersManager.Config.Ini.SetBool("Misc", "NoStats", false);
                Settings.NoStats = false;
            }, "When toggled on, will make avatars loading faster but you won't be able to see any stats.", null, null, false, Settings.NoStats);

            new QMToggleButton(ThisMenu, 1, 2, "Mods", () =>
            {
                Mods = true;
            }, "No Mods", () =>
            {
                Mods = false;
            }, "", null, null, false, true);


            new QMSingleButton(ThisMenu, 0, 2, "Launch\nProfile ID", () =>
            {
                Wrappers.PopupManager.InputeText("Enter profile number", "Launch", new System.Action<string>((ID) =>
                {
                    if (Mods) Utils.Utilities.StartProcess("VRChat.exe", $"--profile={ID}  --no-vr %2");
                    else Utils.Utilities.StartProcess("VRChat.exe", $"--profile={ID} --no-mods --no-vr %2");
                }));

            }, "Launch a new instance of vrchat on a different profile, you can choose to load mods or not.");

            Slider1 = SliderApi.CreateCustom(ThisMenu.getMenuName(), 4.1f, -0.25f, "Comfort turning speed: 1", (float Val) =>
            {
                Slider1.GetComponentInChildren<Text>().text = $"Comfort turning speed: {Val}";
                VRCPlayer.field_Internal_Static_VRCPlayer_0.GetComponent<LocomotionInputController>().field_Public_Single_5 = 200 * Val;
            }, 1, 1, 10);

            Slider2 = SliderApi.CreateCustom(ThisMenu.getMenuName(), 4.1f, 0.25f, "Mouse speed: 1 ", (float Val) =>
            {
                Slider2.GetComponentInChildren<Text>().text = $"Mouse speed: {Val}";
                VRCPlayer.field_Internal_Static_VRCPlayer_0.GetComponent<LocomotionInputController>().field_Public_Single_4 = 200 * Val;
                VRCPlayer.field_Internal_Static_VRCPlayer_0.GetComponent<LocomotionInputController>().field_Protected_NeckMouseRotator_0.field_Public_Single_0 = 3 * Val;
            }, 1, 1, 10);

            Slider3 = SliderApi.CreateCustom(ThisMenu.getMenuName(), 4.1f, 0.75f, $"FOV: {Config.Ini.GetFloat("Misc", "FOV")}", (float Val) =>
            {
                Camera Camera = Camera.main;
                Camera.fieldOfView = Val;
                Config.Ini.SetFloat("Misc", "FOV", Val);
                Slider3.GetComponentInChildren<Text>().text = $"FOV: {Val}";
            }, Config.Ini.GetFloat("Misc", "FOV"), 60f, 120) ;

            Slider4 = SliderApi.CreateCustom(ThisMenu.getMenuName(), 4.1f, 1.25f, $"Camera smoothness: 0.02", (float Val) =>
            {
                VRCPlayer.field_Internal_Static_VRCPlayer_0.GetComponent<LocomotionInputController>().field_Protected_NeckMouseRotator_0.field_Public_Single_1 = Val;
                Slider4.GetComponentInChildren<Text>().text = $"Camera smoothness: {Val}";
            }, 0.02f, 0.02f, 1);

            Slider5 = SliderApi.CreateCustom(ThisMenu.getMenuName(), 4.1f, 1.75f, "Camera aspect: 1.7", (float Val) =>
            {
                Camera Camera = Camera.main;
                Slider5.GetComponentInChildren<Text>().text = $"Camera aspect: {Val}";
                if (Val == 1.777778f) Camera.aspect = 1.777778f;
                else Camera.aspect = 1.777778f * Val;
            }, 1.777778f, 1.777778f, 5);
        }
    }
}