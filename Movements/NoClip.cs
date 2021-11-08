using Evolve.Wrappers;
using System.Linq;
using Evolve.Utils;
using UnityEngine;
using VRC;
using VRCSDK2;
using VRC.Animation;
using Evolve.ConsoleUtils;

namespace Evolve.Movements
{
    internal class NoClip
    {
        public static void Enable()
        {
            var Colliders = UnityEngine.Resources.FindObjectsOfTypeAll<Collider>();
            foreach (var Collider in Colliders) if (Collider.name.Contains("VRCPlayer")) Collider.enabled = false;
            Settings.NoClip= true;
        }

        public static void Disable()
        {
            Settings.NoClip = false;
            var Colliders = UnityEngine.Resources.FindObjectsOfTypeAll<Collider>();
            foreach (var Collider in Colliders) if (Collider.name.Contains("VRCPlayer")) Collider.enabled = true;
        }
    }
}