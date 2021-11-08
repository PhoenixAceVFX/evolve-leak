using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MelonLoader;
using UnityEngine;
using UnityEngine.XR;


namespace Evolve.Modules
{
    internal class ModMain
    {
        private const string SettingsIdentifier = "PlayerRotater";
        private static MelonPreferences_Category ourCategory;
        private static MelonPreferences_Entry<bool> noClippingEntry, invertPitchEntry;
        private static MelonPreferences_Entry<float> flyingSpeedEntry, rotationSpeedEntry;
        private static MelonPreferences_Entry<string> controlSchemeEntry, rotationOriginEntry;
        private static bool easterEgg;
        private static List<(string SettingsValue, string DisplayName)> controlSchemes, rotationOrigins;
        private static bool failedToLoad;

        public static void Initialize()
        {
            Utilities.IsInVR = Environment.GetCommandLineArgs().All(args => !args.Equals("--no-vr", StringComparison.OrdinalIgnoreCase));
            easterEgg = Environment.GetCommandLineArgs().Any(arg => arg.IndexOf("barrelroll", StringComparison.OrdinalIgnoreCase) != -1);

            if (!RotationSystem.Initialize())
            {
                MelonLogger.Msg("Failed to initialize the rotation system. Instance already exists");
                failedToLoad = true;
                return;
            }

            if (!ModPatches.PatchMethods())
            {
                failedToLoad = true;
                MelonLogger.Warning("Failed to patch everything, disabling player rotater");
                return;
            }
            SetupSettings();
        }

        public static void OnPreferencesSaved()
        {
            if (failedToLoad) return;
            LoadSettings();
        }

        private static void SetupSettings()
        {
            if (failedToLoad) return;

            controlSchemes = new List<(string SettingsValue, string DisplayName)> { ("default", "Default"), ("jannyaa", "JanNyaa's") };
            rotationOrigins = new List<(string SettingsValue, string DisplayName)>
                                  {
                                      ("hips", "Hips"), ("viewpoint", "View Point/Camera"), ("righthand", "Right Hand"), ("lefthand", "Left Hand")
                                  };
            LoadSettings();
        }

        private static void LoadSettings()
        {
            try
            {
                RotationSystem.NoClipFlying = noClippingEntry.Value;
                RotationSystem.RotationSpeed = rotationSpeedEntry.Value;
                RotationSystem.FlyingSpeed = flyingSpeedEntry.Value;
                RotationSystem.InvertPitch = invertPitchEntry.Value;

                switch (controlSchemeEntry.Value)
                {
                    default:
                        controlSchemeEntry.ResetToDefault();
                        controlSchemeEntry.Save();

                        RotationSystem.CurrentControlScheme = new DefaultControlScheme();
                        break;

                    case "default":
                        RotationSystem.CurrentControlScheme = new DefaultControlScheme();
                        break;

                    case "jannyaa":
                        RotationSystem.CurrentControlScheme = new JanNyaaControlScheme();
                        break;
                }

                switch (rotationOriginEntry.Value)
                {
                    default:
                        rotationOriginEntry.ResetToDefault();
                        rotationOriginEntry.Save();

                        RotationSystem.RotationOrigin = RotationSystem.RotationOriginEnum.Hips;
                        break;

                    case "hips":
                        RotationSystem.RotationOrigin = RotationSystem.RotationOriginEnum.Hips;
                        break;

                    case "viewpoint":
                        RotationSystem.RotationOrigin = RotationSystem.RotationOriginEnum.ViewPoint;
                        break;

                    // ReSharper disable once StringLiteralTypo
                    case "righthand":
                        RotationSystem.RotationOrigin = RotationSystem.RotationOriginEnum.RightHand;
                        break;

                    // ReSharper disable once StringLiteralTypo
                    case "lefthand":
                        RotationSystem.RotationOrigin = RotationSystem.RotationOriginEnum.LeftHand;
                        break;
                }

                RotationSystem.UpdateSettings();
            }
            catch (Exception e)
            {
                MelonLogger.Msg("Failed to Load Settings: " + e);
            }
        }

        public static void OnUpdate()
        {
            if (!RotationSystem.rotating) return;
            RotationSystem.holdingShift = Input.GetKey(KeyCode.LeftShift);
        }

    }

}