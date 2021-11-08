using Evolve.ConsoleUtils;
using Evolve.Utils;
using MelonLoader;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Evolve.Modules.PostProcessing
{
    internal class PostProcessing
    {
        public static List<OriginalVolume> OriginalVolumes;

        public struct OriginalVolume
        {
            public PostProcessVolume postProcessVolume;
            public bool defaultState;
        }

        public static readonly NightMode NightMode = new NightMode();
        public static readonly Bloom Bloom = new Bloom();

        public static void OnLevelWasLoaded(int level)
        {
            try
            {
                GrabWorldVolumes();
                TogglePostProcessing(Settings.PostProcessing);
            }
            catch { }
        }

        #region Toggle

        public static void GrabWorldVolumes()
        {
            try
            {
                OriginalVolumes = new List<OriginalVolume>();
                foreach (var volume in Resources.FindObjectsOfTypeAll<PostProcessVolume>())
                {
                    OriginalVolumes.Add(new OriginalVolume() { postProcessVolume = volume, defaultState = volume.enabled });
                }
            }
            catch { }
        }

        public static void TogglePostProcessing(bool Toggle)
        {
            try
            {
                if (Toggle)
                {
                    foreach (OriginalVolume originalVolume in OriginalVolumes)
                    {
                        if (originalVolume.postProcessVolume) originalVolume.postProcessVolume.enabled = true;
                    }
                }
                else
                {
                    if (OriginalVolumes != null)
                    {
                        foreach (OriginalVolume originalVolume in OriginalVolumes)
                        {
                            if (originalVolume.postProcessVolume) originalVolume.postProcessVolume.enabled = false;
                        }
                    }
                }
            }
            catch { }
        }
        #endregion Toggle
    }
}