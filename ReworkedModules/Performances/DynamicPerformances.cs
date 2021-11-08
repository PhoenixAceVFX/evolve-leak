using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using MelonLoader;
using System.Reflection;
using HarmonyLib;
using UnhollowerBaseLib;
using UnhollowerRuntimeLib;
using UnhollowerRuntimeLib.XrefScans;
using Evolve.Patch;
using Evolve.ConsoleUtils;
using Patches = Evolve.Patch.Patches;

namespace Evolve.Modules
{
    internal class DynamicPerformances
    {
        private static IntPtr ourDynBoneCollideEntryPoint;
        private static IntPtr ourDynBoneUpdateEntryPoint;
        private static IntPtr ourLastPatchPointer;
        public static bool enableCollisionChecks = true;
        public static bool enableUpdate = true;
        public static bool updateMultiThread = true;
        public static int threadCount = Math.Max(1, Environment.ProcessorCount / 2 - 1);

        public static void OnApplicationStart()
        {
            var dllName = "DSolver.dll";

            if (!SolverApi.Initialize( Directory.GetCurrentDirectory() + "\\Dependencies\\" + dllName))
            ourDynBoneCollideEntryPoint = Marshal.ReadIntPtr((IntPtr)UnhollowerUtils.GetIl2CppMethodInfoPointerFieldForGeneratedMethod(typeof(DynamicBoneCollider).GetMethod(nameof(DynamicBoneCollider.Method_Public_Void_byref_Vector3_Single_0))).GetValue(null));
            ourDynBoneUpdateEntryPoint = Marshal.ReadIntPtr((IntPtr)UnhollowerUtils.GetIl2CppMethodInfoPointerFieldForGeneratedMethod(typeof(DynamicBone).GetMethod(nameof(DynamicBone.Method_Private_Void_Single_Boolean_0))).GetValue(null));

            var isCollidePatched = false;


            unsafe void PatchCollide()
            {
                if (isCollidePatched) return;

                fixed (IntPtr* a = &ourDynBoneCollideEntryPoint)
                    MelonUtils.NativeHookAttach((IntPtr)a, SolverApi.LibDynBoneCollideEntryPoint);

                isCollidePatched = true;
            }

            unsafe void UnpatchCollide()
            {
                if (!isCollidePatched) return;

                fixed (IntPtr* a = &ourDynBoneCollideEntryPoint)
                    MelonUtils.NativeHookDetach((IntPtr)a, SolverApi.LibDynBoneCollideEntryPoint);

                isCollidePatched = false;
            }

            unsafe void RepatchUpdate(bool useFast, bool useMt)
            {
                if (ourLastPatchPointer != IntPtr.Zero)
                {
                    fixed (IntPtr* a = &ourDynBoneUpdateEntryPoint)
                        MelonUtils.NativeHookDetach((IntPtr)a, ourLastPatchPointer);
                    ourLastPatchPointer = IntPtr.Zero;
                }

                if (useFast)
                {
                    ourLastPatchPointer = useMt ? SolverApi.LibDynBoneUpdateMultiThreaded : SolverApi.LibDynBoneUpdateSingleThreaded;

                    fixed (IntPtr* a = &ourDynBoneUpdateEntryPoint)
                        MelonUtils.NativeHookAttach((IntPtr)a, ourLastPatchPointer);
                }
                else
                {
                    ourLastPatchPointer = SolverApi.DynamicBoneUpdateNotifyPatch;

                    fixed (IntPtr* a = &ourDynBoneUpdateEntryPoint)
                        MelonUtils.NativeHookAttach((IntPtr)a, ourLastPatchPointer);

                    SolverApi.SetOriginalBoneUpdateDelegate(ourDynBoneUpdateEntryPoint);
                }
            }

            if (enableCollisionChecks) PatchCollide();

            RepatchUpdate(enableUpdate, updateMultiThread);

            SolverApi.SetNumThreads(Math.Max(Math.Min(threadCount, 32), 1));

            Patches.Instance.Patch(typeof(DynamicBone).GetMethod(nameof(DynamicBone.OnEnable)), new HarmonyMethod(typeof(DynamicPerformances), nameof(OnEnablePrefix)));
            Patches.Instance.Patch(typeof(DynamicBone).GetMethod(nameof(DynamicBone.OnDisable)), new HarmonyMethod(typeof(DynamicPerformances), nameof(OnDisablePrefix)));
            Patches.Instance.Patch(typeof(AvatarClone).GetMethod(nameof(AvatarClone.LateUpdate)), new HarmonyMethod(typeof(DynamicPerformances), nameof(LateUpdatePrefix)));
            Patches.Instance.Patch(XrefScanner.XrefScan(typeof(DynamicBone).GetMethod(nameof(DynamicBone.OnEnable))).Single(it => it.Type == XrefType.Method && it.TryResolve() != null).TryResolve(),new HarmonyMethod(typeof(DynamicPerformances), nameof(ResetParticlesPatch)));
        }

        public static void ResetParticlesPatch(DynamicBone __instance)
        {
            SolverApi.ResetParticlePositions(__instance.Pointer);
        }

        public static void OnUpdate()
        {
            try
            {
                SolverApi.FlushColliderCache();
            }
            catch { }
        }

        public static void LateUpdatePrefix()
        {
            SolverApi.JoinMultithreadedJobs();
        }

        public static void OnEnablePrefix(DynamicBone __instance)
        {
            SolverApi.DynamicBoneOnEnablePatch(__instance.Pointer);

            if (__instance.gameObject.GetComponent<BoneDeleteHandler>() == null)
                __instance.gameObject.AddComponent<BoneDeleteHandler>();
        }

        public static void OnDisablePrefix(DynamicBone __instance)
        {
            SolverApi.DynamicBoneOnDisablePatch(__instance.Pointer);
        }

        public static void OnStartSuffix(DynamicBone __instance)
        {
            SolverApi.DynamicBoneStartPatch(__instance.Pointer);
        }

        public static void OnDestroyInjected(DynamicBone __instance)
        {
            SolverApi.DynamicBoneOnDestroyPatch(__instance.Pointer);
        }

    }
}