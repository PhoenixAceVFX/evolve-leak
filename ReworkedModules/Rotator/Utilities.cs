namespace Evolve.Modules
{

    using System;
    using System.Collections;
    using System.Linq;
    using System.Reflection;

    using Il2CppSystem.Collections.Generic;

    using MelonLoader;
    using UnhollowerRuntimeLib.XrefScans;

    using UnityEngine;
    using UnityEngine.XR;
    using VRC.Core;

    internal static class Utilities
    {
        private static MethodInfo alignTrackingToPlayerMethod;
        internal static GameObject LockRotationButton, ToggleRotaterButton;

        internal static AlignTrackingToPlayerDelegate GetAlignTrackingToPlayerDelegate
        {
            get
            {
                alignTrackingToPlayerMethod ??= typeof(VRCPlayer).GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).First(m => m.ReturnType == typeof(void) && m.GetParameters().Length == 0 && m.Name.IndexOf("PDM", StringComparison.OrdinalIgnoreCase) == -1 && m.XRefScanForMethod("get_Transform") && m.XRefScanForMethod(reflectedType: nameof(VRCTrackingManager)) && m.XRefScanForMethod(reflectedType: nameof(InputStateController)));
                return (AlignTrackingToPlayerDelegate)Delegate.CreateDelegate(typeof(AlignTrackingToPlayerDelegate), GetLocalVRCPlayer(), alignTrackingToPlayerMethod);
            }
        }

        public static bool GetStreamerMode => VRCInputManager.Method_Public_Static_Boolean_EnumNPublicSealedvaUnCoHeToTaThShPeVoUnique_0(VRCInputManager.EnumNPublicSealedvaUnCoHeToTaThShPeVoUnique.StreamerModeEnabled);
        internal static bool IsInVR { get; set; }
        private static void SetRotationButtons(bool enabled)
        {
            LockRotationButton.SetActive(enabled);
            ToggleRotaterButton.SetActive(enabled);
        }

        internal static bool AnyActionMenuesOpen()
        {
            return ActionMenuDriver.field_Public_Static_ActionMenuDriver_0.field_Public_ActionMenuOpener_0.field_Private_Boolean_0 || ActionMenuDriver.field_Public_Static_ActionMenuDriver_0.field_Public_ActionMenuOpener_1.field_Private_Boolean_0;
        }

        internal static VRCPlayer GetLocalVRCPlayer()
        {
            return VRCPlayer.field_Internal_Static_VRCPlayer_0;
        }

        internal static bool XRefScanForMethod(this MethodBase methodBase, string methodName = null, string reflectedType = null)
        {
            var found = false;
            foreach (XrefInstance xref in XrefScanner.XrefScan(methodBase))
            {
                if (xref.Type != XrefType.Method) continue;
                MethodBase resolved = xref.TryResolve();
                if (resolved == null) continue;
                if (!string.IsNullOrEmpty(methodName)) found = !string.IsNullOrEmpty(resolved.Name) && resolved.Name.IndexOf(methodName, StringComparison.OrdinalIgnoreCase) >= 0;
                if (!string.IsNullOrEmpty(reflectedType)) found = !string.IsNullOrEmpty(resolved.ReflectedType?.Name) && resolved.ReflectedType.Name.IndexOf(reflectedType, StringComparison.OrdinalIgnoreCase) >= 0;
                if (found) return true;
            }
            return false;
        }

        internal delegate void AlignTrackingToPlayerDelegate();

    }

}