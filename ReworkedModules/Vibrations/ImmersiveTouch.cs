using MelonLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using VRC.SDKBase;

namespace Evolve.Modules
{
    internal class ImmersiveTouch
    {
        public static bool Enable = false;
        private static bool m_IsCapable;
        public static float HapticsPower = 100;
        private static float m_HapticDistance;
        private static Vector3 m_PreviousLeftWristPosition;
        private static Vector3 m_PreviousRightWristPosition;
        private static IntPtr m_LeftWristIntPtr;
        private static IntPtr m_RightWristIntPtr;
        [ThreadStatic] static IntPtr m_CurrentDBI;
        private static readonly List<IntPtr> m_LocalDynamicBonePointers = new List<IntPtr>();
        private static ColliderPrioritization m_ColliderPrioritization = ColliderPrioritization.Wrist;
        private static GameObject m_CurrentAvatarObject;

        public static void OnUiManagerInit()
        {
            Hooks.ApplyPatches();
        }

        public static unsafe void OnAvatarChanged(IntPtr instance, IntPtr __0, IntPtr __1, IntPtr __2)
        {
            Hooks.avatarChangedDelegate(instance, __0, __1, __2);

            try
            {
                VRCAvatarManager avatarManager = new VRCAvatarManager(instance);
                if (avatarManager != null && avatarManager.GetInstanceID().Equals(Manager.GetLocalAvatarManager().GetInstanceID()))
                {
                    Animator animator = avatarManager.field_Private_Animator_0;
                    if (animator == null || !animator.isHuman) return;

                    float scale = Vector3.Distance(animator.GetBoneTransform(HumanBodyBones.LeftHand).position, animator.GetBoneTransform(HumanBodyBones.RightHand).position);
                    m_HapticDistance = scale / 785.0f;

                    m_CurrentAvatarObject = avatarManager.prop_GameObject_0;

                    TryCapability();
                }
            }
            catch { }
        }

        // Credits to knah for this idea
        public static unsafe void OnUpdateParticles(IntPtr instance, bool __0)
        {
            m_CurrentDBI = instance;

            Hooks.updateParticlesDelegate(instance, __0);
        }

        public static unsafe void OnCollide(IntPtr instance, IntPtr particlePosition, float particleRadius)
        {
            void InvokeCollide() => Hooks.collideDelegate(instance, particlePosition, particleRadius);

            try
            {
                if (!m_IsCapable || (!instance.Equals(m_LeftWristIntPtr) && !instance.Equals(m_RightWristIntPtr)))
                {
                    InvokeCollide();
                    return;
                }

                Vector3 prevParticlePos = Marshal.PtrToStructure<Vector3>(particlePosition);
                InvokeCollide();

                if (!prevParticlePos.Equals(Marshal.PtrToStructure<Vector3>(particlePosition))) SendHaptic(instance);

            }
            catch
            {
                InvokeCollide();
            }
        }

        private static void SendHaptic(IntPtr instance)
        {
            if (m_LocalDynamicBonePointers.Contains(m_CurrentDBI)) return;

            DynamicBoneCollider dynamicBoneCollider = new DynamicBoneCollider(instance);

            Vector3 position = dynamicBoneCollider.transform.position;

            if (instance.Equals(m_LeftWristIntPtr) && Vector3.Distance(m_PreviousLeftWristPosition, position) > m_HapticDistance)
            {
                Manager.GetLocalVRCPlayerApi().PlayHapticEventInHand(VRC_Pickup.PickupHand.Left, 0.001f, HapticsPower, 0.001f);

                m_PreviousLeftWristPosition = position;
            }

            if (instance.Equals(m_RightWristIntPtr) && Vector3.Distance(m_PreviousRightWristPosition, position) > m_HapticDistance)
            {
                Manager.GetLocalVRCPlayerApi().PlayHapticEventInHand(VRC_Pickup.PickupHand.Right, 0.001f, HapticsPower, 0.001f);
                
                m_PreviousRightWristPosition = position;
            }
        }

        private static void TryCapability()
        {
            if (!Enable || Manager.GetLocalVRCPlayer() == null)
            {
                m_IsCapable = false;
                return;
            }

            try
            {
                Animator animator = Manager.GetLocalAvatarAnimator();
                if (animator == null || !animator.isHuman) NotCapable();

                HumanBodyBones leftBoneTarget = HumanBodyBones.LeftHand;
                HumanBodyBones rightBoneTarget = HumanBodyBones.RightHand;

                switch (m_ColliderPrioritization)
                {
                    case ColliderPrioritization.Wrist:
                        leftBoneTarget = HumanBodyBones.LeftHand;
                        rightBoneTarget = HumanBodyBones.RightHand;
                        break;
                    case ColliderPrioritization.Thumb:
                        leftBoneTarget = HumanBodyBones.LeftThumbProximal;
                        rightBoneTarget = HumanBodyBones.RightThumbProximal;
                        break;
                    case ColliderPrioritization.Index:
                        leftBoneTarget = HumanBodyBones.LeftIndexProximal;
                        rightBoneTarget = HumanBodyBones.RightIndexProximal;
                        break;
                    case ColliderPrioritization.Middle:
                        leftBoneTarget = HumanBodyBones.LeftMiddleProximal;
                        rightBoneTarget = HumanBodyBones.RightMiddleProximal;
                        break;
                    case ColliderPrioritization.Ring:
                        leftBoneTarget = HumanBodyBones.LeftRingProximal;
                        rightBoneTarget = HumanBodyBones.RightRingProximal;
                        break;
                    case ColliderPrioritization.Pinky:
                        leftBoneTarget = HumanBodyBones.LeftLittleProximal;
                        rightBoneTarget = HumanBodyBones.RightLittleProximal;
                        break;
                }

                var leftHandColliders = animator.GetDynamicBoneColliders(leftBoneTarget);
                var rightHandColliders = animator.GetDynamicBoneColliders(rightBoneTarget);

                if (m_ColliderPrioritization != ColliderPrioritization.Wrist && (leftHandColliders.Count == 0 || rightHandColliders.Count == 0))
                {
                    leftHandColliders = animator.GetDynamicBoneColliders(HumanBodyBones.LeftHand);
                    rightHandColliders = animator.GetDynamicBoneColliders(HumanBodyBones.RightHand);
                }

                m_LeftWristIntPtr = leftHandColliders.Count != 0 ? leftHandColliders[0].Pointer : IntPtr.Zero;
                m_RightWristIntPtr = rightHandColliders.Count != 0 ? rightHandColliders[0].Pointer : IntPtr.Zero;

                m_IsCapable = m_LeftWristIntPtr != IntPtr.Zero && m_RightWristIntPtr != IntPtr.Zero;

                if (m_IsCapable)
                {
                    m_LocalDynamicBonePointers.Clear();
                    foreach (var db in m_CurrentAvatarObject.GetDynamicBones())
                        m_LocalDynamicBonePointers.Add(db.Pointer);
                }
                else
                    NotCapable();
            }
            catch
            {
                m_IsCapable = false;
            }

            static void NotCapable()
            {
                m_IsCapable = false;
                return;
            }
        }

        private enum ColliderPrioritization
        {
            Wrist,
            Thumb,
            Index,
            Middle,
            Ring,
            Pinky
        }
    }
}