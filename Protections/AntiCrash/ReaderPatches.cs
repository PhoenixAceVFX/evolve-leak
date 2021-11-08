using MelonLoader;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Evolve.Protections.AntiCrash
{
    class ReaderPatches
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr AudioMixerReadDelegate(IntPtr thisPtr, IntPtr readerPtr);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void FloatReadDelegate(IntPtr readerPtr, float* result, byte* fieldName);
        private static AudioMixerReadDelegate ourAudioMixerReadDelegate;
        private static IntPtr ourAudioMixerReadPointer;
        private static FloatReadDelegate ourFloatReadDelegate;
        private static IntPtr ourFloatReadPointer;
        private const float MaxAllowedValueTop = 3.402823E+7f;
        private const float MaxAllowedValueBottom = -3.402823E+7f;
        private static readonly List<object> ourPinnedDelegates = new List<object>();
        private static string[] ourAllowedFields = { "m_BreakForce", "m_BreakTorque", "collisionSphereDistance", "maxDistance", "inSlope", "outSlope" };

        internal static void ApplyPatches()
        {
            foreach (ProcessModule module in Process.GetCurrentProcess().Modules)
            {
                if (!module.FileName.Contains("UnityPlayer")) continue;

                unsafe
                {
                    ourAudioMixerReadPointer = module.BaseAddress + 0x4997C0;
                    var patchDelegate = new AudioMixerReadDelegate(AudioMixerReadPatch);
                    ourPinnedDelegates.Add(patchDelegate);
                    fixed (IntPtr* mixerReadAddress = &ourAudioMixerReadPointer) MelonUtils.NativeHookAttach((IntPtr)mixerReadAddress, Marshal.GetFunctionPointerForDelegate(patchDelegate));
                    ourAudioMixerReadDelegate = Marshal.GetDelegateForFunctionPointer<AudioMixerReadDelegate>(ourAudioMixerReadPointer);
                }

                unsafe
                {
                    ourFloatReadPointer = module.BaseAddress + 0xD7320;
                    var patchDelegate = new FloatReadDelegate(FloatTransferPatch);
                    ourPinnedDelegates.Add(patchDelegate);
                    fixed (IntPtr* floatReadAddress = &ourFloatReadPointer) MelonUtils.NativeHookAttach((IntPtr)floatReadAddress, Marshal.GetFunctionPointerForDelegate(patchDelegate));
                    ourFloatReadDelegate = Marshal.GetDelegateForFunctionPointer<FloatReadDelegate>(ourFloatReadPointer);
                }

                break;
            }
        }

        private static unsafe void FloatTransferPatch(IntPtr reader, float* result, byte* fieldName)
        {
            ourFloatReadDelegate(reader, result, fieldName);
            if (*result > MaxAllowedValueBottom && *result < MaxAllowedValueTop || AntiCrashSettings.AllowReadingBadFloats) return;
            if (float.IsNaN(*result)) goto clamp;
            if (fieldName != null)
            {
                foreach (var allowedField in ourAllowedFields)
                {
                    for (var j = 0; j < allowedField.Length; j++)
                        if (fieldName[j] == 0 || fieldName[j] != allowedField[j])
                            goto next;
                    return;
                    next:;
                }
            }

            clamp:
            *result = 0;
        }

        private static IntPtr AudioMixerReadPatch(IntPtr thisPtr, IntPtr readerPtr)
        {
            if (!N5HF8S65K.CanReadAudioMixers && !AntiCrashSettings.AllowReadingMixers) return IntPtr.Zero;
            // just in case something ever races
            ourAudioMixerReadDelegate ??= Marshal.GetDelegateForFunctionPointer<AudioMixerReadDelegate>(ourAudioMixerReadPointer);
            return ourAudioMixerReadDelegate(thisPtr, readerPtr);
        }
    }
}
