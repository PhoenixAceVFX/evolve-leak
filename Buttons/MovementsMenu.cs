using System.Collections;
using System.Net.Mime;
using Evolve.Api;
using Evolve.ConsoleUtils;
using Evolve.Modules;
using Evolve.Movements;
using Evolve.Utils;
using Evolve.Wrappers;
using Il2CppSystem.Runtime.CompilerServices;
using ButtonApi;
using UnityEngine;
using UnityEngine.UI;
using VRC.Animation;

namespace Evolve.Buttons
{
    internal class MovementsMenu
    {
        public static QMNestedButton ThisMenu;
        public static QMToggleButton Fly;
        public static QMToggleButton NoCliping;
        public static GameObject Slider1;
        public static QMToggleButton SpeedHack;
        public static QMToggleButton Jump;
        public static QMToggleButton BlinkBut;
        public static QMToggleButton Rotator;
        public static GameObject Slider2;
        public static GameObject Slider3;
        public static QMToggleButton RocketJump;
        public static float Strafe;
        public static float Walk;
        public static float Run;
        public static float JumpPower = 3;
#pragma warning disable CS0649 // Le champ 'MovementsMenu.alignTrackingToPlayer' n'est jamais assigné et aura toujours sa valeur par défaut null
        public static Modules.Utilities.AlignTrackingToPlayerDelegate alignTrackingToPlayer;
#pragma warning restore CS0649 // Le champ 'MovementsMenu.alignTrackingToPlayer' n'est jamais assigné et aura toujours sa valeur par défaut null
        public static void Initialize()
        {
            ThisMenu = new QMNestedButton(EvolveMenu.ThisMenu, 1.5f, 1.25f, "Movements", "Movements menu", Color.cyan, Color.magenta, Color.black, Color.yellow);
            ThisMenu.getMainButton().getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            var InfJump = new QMToggleButton(ThisMenu, 3, 0, "Infinite\nJump", () =>
            {
                if (Movements.Movements.RocketJump) RocketJump.setToggleState(false, true);
                Movements.Movements.InfiniteJump = true;
            }, "Disabled", () =>
            {
                Movements.Movements.InfiniteJump = false;
            }, "Infinite jump");

            RocketJump = new QMToggleButton(ThisMenu, 3, 1, "Rocket\nJump", () =>
            {
                if (Movements.Movements.InfiniteJump) InfJump.setToggleState(false, true);
                Movements.Movements.RocketJump = true;
            }, "Disabled", () =>
            {
                Movements.Movements.RocketJump = false;
            }, "Infinite jump");

            SpeedHack = new QMToggleButton(ThisMenu, 2, 1, "Speed", () =>
             {
                 var Loco = VRCPlayer.field_Internal_Static_VRCPlayer_0.GetComponent<LocomotionInputController>();
                 Strafe = Loco.field_Public_Single_1;
                 Run = Loco.field_Public_Single_0;
                 Walk = Loco.field_Public_Single_2;
                 Settings.Speed = true;
             }, "Disabled", () =>
             {
                 var Loco = VRCPlayer.field_Internal_Static_VRCPlayer_0.GetComponent<LocomotionInputController>();
                 Loco.field_Public_Single_1 = Strafe;
                 Loco.field_Public_Single_0 = Run;
                 Loco.field_Public_Single_2 = Walk;
                 Settings.Speed = false;
             }, "Enable Speed Boost", Color.black, Color.yellow);

            Fly = new QMToggleButton(ThisMenu, 1, 0, "Fly", () =>
            {
                Settings.Fly = true;
                Physics.gravity = Vector3.zero;
            }, "Disabled", () =>
            {
                Settings.Fly = false;
                Physics.gravity = new Vector3(0, -9.81f, 0);
                NoCliping.setToggleState(false, true);
            }, "Enable Fly", Color.black, Color.yellow);

            NoCliping = new QMToggleButton(ThisMenu, 1, 1, "No-Clip", () =>
            {
                Fly.setToggleState(true, true);
                NoClip.Enable();
                Settings.NoClip = true;
            }, "Disabled", () =>
            {
                NoClip.Disable();
                Settings.NoClip = false;
            }, "Enable NoClip", Color.black, Color.yellow, false, Settings.NoClip);

            Rotator = new QMToggleButton(ThisMenu, 4, 0, "Rotations", () =>
            {
                RotationSystem.Toggle();
                Settings.Rotator = true;
            }, "Disabled", () =>
            {
                RotationSystem.Toggle();
                Settings.Rotator = false;
            }, "Enable rotations");

            BlinkBut = new QMToggleButton(ThisMenu, 4, 1, "Blink", () =>
            {
                Settings.Blink = true;
            }, "Disabled", () =>
            {
                Settings.Serialize = false;
                Settings.Blink = false;
                Camera Camera = Camera.main;
                Camera.OffsetOfInstanceIDInCPlusPlusObject = 8;
            }, "Will freeze your position everytime you move around");

            //Sliders
            Slider1 = SliderApi.CreateCustom(ThisMenu.getMenuName(), 1.2f, 1.75f, "Fly speed: 5", (float Val) =>
            {
                Settings.FlySpeed = Val;
                Slider1.GetComponentInChildren<Text>().text = $"Fly speed: {Settings.FlySpeed}";
            }, Settings.FlySpeed, 1f, 50f);

            Slider2 = SliderApi.CreateCustom(ThisMenu.getMenuName(), 1.2f, 2.25f, "Run speed: 5", (float Val) =>
            {
                Settings.SpeedValue = Val;
                Slider2.GetComponentInChildren<Text>().text = $"Speed: {Settings.SpeedValue}";
            }, Settings.FlySpeed, 0f, 50f);

            Slider3 = SliderApi.CreateCustom(ThisMenu.getMenuName(), 3.2f, 1.75f, "Jump power: 3", (float Val) =>
            {
                JumpPower = Val;
                Slider3.GetComponentInChildren<Text>().text = $"Power: {JumpPower}";
                VRCPlayer.field_Internal_Static_VRCPlayer_0.gameObject.GetComponent<LocomotionInputController>()
                    .field_Public_Single_3 = JumpPower;
                VRCPlayer.field_Internal_Static_VRCPlayer_0.gameObject.GetComponent<LocomotionInputController>()
                    .field_Public_Single_3 = JumpPower;
            }, Settings.FlySpeed, 1f, 50f);

            Jump = new QMToggleButton(ThisMenu, 2, 0, "Jump", () =>
            {
                if (VRCPlayer.field_Internal_Static_VRCPlayer_0.gameObject.GetComponent<PlayerModComponentJump>() ==
                    null)
                {
                    VRCPlayer.field_Internal_Static_VRCPlayer_0.gameObject.AddComponent<PlayerModComponentJump>();
                    VRCPlayer.field_Internal_Static_VRCPlayer_0.gameObject.GetComponent<PlayerModComponentJump>()
                        .field_Private_Single_0 = JumpPower;
                    VRCPlayer.field_Internal_Static_VRCPlayer_0.gameObject.GetComponent<PlayerModComponentJump>()
                        .field_Private_Single_1 = JumpPower;
                }
                else
                {
                    VRCPlayer.field_Internal_Static_VRCPlayer_0.gameObject.GetComponent<PlayerModComponentJump>()
                        .field_Private_Single_0 = JumpPower;
                    VRCPlayer.field_Internal_Static_VRCPlayer_0.gameObject.GetComponent<PlayerModComponentJump>()
                        .field_Private_Single_1 = JumpPower;
                }
            }, "Disabled", () =>
            {
                if (VRCPlayer.field_Internal_Static_VRCPlayer_0.gameObject.GetComponent<PlayerModComponentJump>() !=
                    null)
                {
                    VRCPlayer.field_Internal_Static_VRCPlayer_0.gameObject.GetComponent<PlayerModComponentJump>()
                        .field_Private_Single_0 = 0;
                    VRCPlayer.field_Internal_Static_VRCPlayer_0.gameObject.GetComponent<PlayerModComponentJump>()
                        .field_Private_Single_1 = 0;
                    UnityEngine.Component.Destroy(VRCPlayer.field_Internal_Static_VRCPlayer_0.gameObject
                        .GetComponent<PlayerModComponentJump>());
                }
            }, "Enable or disable jumping in this world");
        }
    }
}