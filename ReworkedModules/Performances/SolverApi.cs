using System;
using System.Runtime.InteropServices;
using MelonLoader;
using UnhollowerBaseLib;

namespace Evolve.Modules
{
    internal static class SolverApi
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void SetPointersAndOffsetsDelegate(
            ref ICallOffsets icalls,
            ref ColliderComponentOffsets component_offsets,
            ref BoneComponentOffsets bone_offsets,
            ref ParticleClassOffsets particle_offsets,
            ref ListClassOffsets list_offsets,
            ref ObjectOffsets object_offsets
        );

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void VoidDelegate();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void SetThreadNumberDelegate(int threadNumber);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void ComponentDelegate(IntPtr boneComponent);


        internal static VoidDelegate FlushColliderCache { get; private set; }
        internal static VoidDelegate JoinMultithreadedJobs { get; private set; }
        internal static SetPointersAndOffsetsDelegate SetPointersAndOffsets { get; private set; }
        internal static SetThreadNumberDelegate SetNumThreads { get; private set; }

        internal static ComponentDelegate DynamicBoneOnEnablePatch { get; private set; }
        internal static ComponentDelegate DynamicBoneOnDisablePatch { get; private set; }
        internal static ComponentDelegate DynamicBoneStartPatch { get; private set; }
        internal static ComponentDelegate DynamicBoneOnDestroyPatch { get; private set; }
        internal static ComponentDelegate ResetParticlePositions { get; private set; }

        public delegate void RegisterColliderForCollisionFeedbackDelegate(IntPtr colliderPtr, byte group);
        public delegate void UnregisterColliderForCollisionFeedbackDelegate(IntPtr colliderPtr);
        public delegate ulong GetAndClearCollidingGroupsMaskDelegate();

        public delegate void BoneConsumingDelegate(IntPtr bonePtr);

        public static RegisterColliderForCollisionFeedbackDelegate RegisterColliderForCollisionFeedback { get; private set; }

        public static UnregisterColliderForCollisionFeedbackDelegate UnregisterColliderForCollisionFeedback { get; private set; }

        public static VoidDelegate ClearCollisionFeedbackColliders { get; private set; }

        public static GetAndClearCollidingGroupsMaskDelegate GetAndClearCollidingGroupsMask { get; private set; }

        public static BoneConsumingDelegate SetCurrentlySimulatingBone { get; private set; }

        public static BoneConsumingDelegate SetOriginalBoneUpdateDelegate { get; private set; }

        public static BoneConsumingDelegate ExcludeBoneFromCollisionFeedback { get; private set; }

        public static BoneConsumingDelegate UnExcludeBoneFromCollisionFeedback { get; private set; }

        public static VoidDelegate ClearExcludedBonesFromCollisionFeedback { get; private set; }


        internal static IntPtr LibDynBoneCollideEntryPoint;
        internal static IntPtr LibDynBoneUpdateSingleThreaded;
        internal static IntPtr LibDynBoneUpdateMultiThreaded;
        internal static IntPtr DynamicBoneUpdateNotifyPatch;

        internal static bool Initialize(string dllName)
        {
            var lib = LoadLibraryA(dllName);
            if (lib == IntPtr.Zero)
            {
                MelonLogger.Error("Native library load failed, mod won't work");
                return false;
            }

            SetPointersAndOffsets = GetPointer<SetPointersAndOffsetsDelegate>(lib, nameof(SetPointersAndOffsets));
            FlushColliderCache = GetPointer<VoidDelegate>(lib, nameof(FlushColliderCache));
            JoinMultithreadedJobs = GetPointer<VoidDelegate>(lib, nameof(JoinMultithreadedJobs));
            SetNumThreads = GetPointer<SetThreadNumberDelegate>(lib, nameof(SetNumThreads));

            DynamicBoneOnDestroyPatch = GetPointer<ComponentDelegate>(lib, nameof(DynamicBoneOnDestroyPatch));
            DynamicBoneOnEnablePatch = GetPointer<ComponentDelegate>(lib, nameof(DynamicBoneOnEnablePatch));
            DynamicBoneOnDisablePatch = GetPointer<ComponentDelegate>(lib, nameof(DynamicBoneOnDisablePatch));
            DynamicBoneStartPatch = GetPointer<ComponentDelegate>(lib, nameof(DynamicBoneStartPatch));
            ResetParticlePositions = GetPointer<ComponentDelegate>(lib, nameof(ResetParticlePositions));

            RegisterColliderForCollisionFeedback = GetPointer<RegisterColliderForCollisionFeedbackDelegate>(lib, nameof(RegisterColliderForCollisionFeedback));
            UnregisterColliderForCollisionFeedback = GetPointer<UnregisterColliderForCollisionFeedbackDelegate>(lib, nameof(UnregisterColliderForCollisionFeedback));
            ClearCollisionFeedbackColliders = GetPointer<VoidDelegate>(lib, nameof(ClearCollisionFeedbackColliders));
            GetAndClearCollidingGroupsMask = GetPointer<GetAndClearCollidingGroupsMaskDelegate>(lib, nameof(GetAndClearCollidingGroupsMask));

            ExcludeBoneFromCollisionFeedback = GetPointer<BoneConsumingDelegate>(lib, nameof(ExcludeBoneFromCollisionFeedback));
            UnExcludeBoneFromCollisionFeedback = GetPointer<BoneConsumingDelegate>(lib, nameof(UnExcludeBoneFromCollisionFeedback));
            SetCurrentlySimulatingBone = GetPointer<BoneConsumingDelegate>(lib, nameof(SetCurrentlySimulatingBone));
            SetOriginalBoneUpdateDelegate = GetPointer<BoneConsumingDelegate>(lib, nameof(SetOriginalBoneUpdateDelegate));
            ClearExcludedBonesFromCollisionFeedback = GetPointer<VoidDelegate>(lib, nameof(ClearExcludedBonesFromCollisionFeedback));

            LibDynBoneCollideEntryPoint = GetProcAddress(lib, "ColliderCollidePatch");
            LibDynBoneUpdateSingleThreaded = GetProcAddress(lib, "DynamicBoneUpdateSingleThreadPatch");
            LibDynBoneUpdateMultiThreaded = GetProcAddress(lib, "DynamicBoneUpdateMultiThreadPatch");
            DynamicBoneUpdateNotifyPatch = GetProcAddress(lib, "DynamicBoneUpdateNotifyPatch");

            Offsets.SetOffsets();

            return true;
        }

        private static T GetPointer<T>(IntPtr lib, string name) where T : MulticastDelegate
        {
            var result = Marshal.GetDelegateForFunctionPointer<T>(GetProcAddress(lib, name));
            if (result == null) MelonLogger.Error($"Delegate for {name} not found! Bug?");

            return result;
        }


        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        internal static extern IntPtr LoadLibraryA(string libName);
    }
}