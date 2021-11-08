using System;
using System.Collections;
using MelonLoader;
using UnhollowerRuntimeLib;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Evolve.Modules
{
    internal class RotationSystem
    {
        internal static float FlyingSpeed = 5f;
        internal static float RotationSpeed = 180f;
        internal static bool NoClipFlying = true;
        internal static RotationSystem Instance;
        internal static IControlScheme CurrentControlScheme;
        internal static RotationOriginEnum RotationOrigin = RotationOriginEnum.Hips;
        internal static bool InvertPitch, LockRotation;
        private static Utilities.AlignTrackingToPlayerDelegate alignTrackingToPlayer;
        private static Transform cameraTransform;
        private static Vector3 originalGravity;
        private static Transform playerTransform, originTransform;
        public static bool rotating;
        public static bool usePlayerAxis, holdingShift;
        private RotationSystem()
        { }
        public static bool Rotating => rotating;

        internal void Pitch(float inputAmount)
        {
            if (InvertPitch) inputAmount *= -1;
            playerTransform.RotateAround(
                originTransform.position,
                usePlayerAxis ? playerTransform.right : originTransform.right,
                inputAmount * RotationSpeed * Time.deltaTime * (holdingShift ? 2f : 1f));
        }

        internal void Yaw(float inputAmount)
        {
            playerTransform.RotateAround(
                originTransform.position,
                usePlayerAxis ? playerTransform.up : originTransform.up,
                inputAmount * RotationSpeed * Time.deltaTime * (holdingShift ? 2f : 1f));
        }

        internal void Roll(float inputAmount)
        {
            playerTransform.RotateAround(
                originTransform.position,
                usePlayerAxis ? -playerTransform.forward : -originTransform.forward,
                inputAmount * RotationSpeed * Time.deltaTime * (holdingShift ? 2f : 1f));
        }

        internal void Fly(float inputAmount, Vector3 direction)
        {
            playerTransform.position += direction * inputAmount * FlyingSpeed * (holdingShift ? 2f : 1f) * Time.deltaTime;
        }

        internal static bool Initialize()
        {
            if (Instance != null) return false;
            Instance = new RotationSystem();
            MelonCoroutines.Start(GrabMainCamera());
            return true;
        }

        private static IEnumerator GrabMainCamera()
        {
            while (!cameraTransform)
            {
                yield return new WaitForSeconds(1f);
                foreach (Object component in Object.FindObjectsOfType(Il2CppType.Of<Transform>()))
                {
                    yield return null;
                    Transform transform;
                    if ((transform = component.TryCast<Transform>()) == null) continue;
                    if (!transform.name.Equals("Camera (eye)", StringComparison.OrdinalIgnoreCase)) continue;
                    cameraTransform = transform;
                    break;
                }
            }
        }

        // bit weird but i've gotten some errors few times where it bugged out a bit
        internal static void Toggle()
        {
            if (!rotating) originalGravity = Physics.gravity;

            try
            {
                playerTransform ??= Utilities.GetLocalVRCPlayer().transform;
                rotating = !rotating;

                if (rotating)
                {
                    originalGravity = Physics.gravity;
                    Physics.gravity = Vector3.zero;
                    alignTrackingToPlayer ??= Utilities.GetAlignTrackingToPlayerDelegate;
                }
                else
                {
                    Quaternion local = playerTransform.localRotation;
                    playerTransform.localRotation = new Quaternion(0f, local.y, 0f, local.w);
                    Physics.gravity = originalGravity;
                }
            }
            catch (Exception e)
            {
                MelonLogger.Error("Error Toggling: " + e);
                rotating = false;
            }

            UpdateSettings();
            if (rotating) return;
            Physics.gravity = originalGravity;
            alignTrackingToPlayer?.Invoke();
        }

        private static void GrabOriginTransform()
        {
            var isHumanoid = false;

            void GetHumanBoneTransform(HumanBodyBones bone)
            {
                // ReSharper disable twice Unity.NoNullPropagation
                GameObject localAvatar = Utilities.GetLocalVRCPlayer()?.prop_VRCAvatarManager_0?.prop_GameObject_0;
                Animator localAnimator = localAvatar?.GetComponent<Animator>();

                if (localAnimator != null)
                {
                    isHumanoid = localAnimator.isHuman;
                    originTransform = isHumanoid ? localAnimator.GetBoneTransform(bone) : cameraTransform;
                }
                else originTransform = cameraTransform;

            }

            switch (RotationOrigin)
            {
                case RotationOriginEnum.Hips:
                    GetHumanBoneTransform(HumanBodyBones.Hips);
                    break;

                case RotationOriginEnum.ViewPoint:
                    originTransform = cameraTransform;
                    break;

                case RotationOriginEnum.RightHand:
                    GetHumanBoneTransform(HumanBodyBones.RightHand);
                    break;

                case RotationOriginEnum.LeftHand:
                    GetHumanBoneTransform(HumanBodyBones.LeftHand);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(RotationOrigin), RotationOrigin, "What kind of dinkleberry thing did you do to my enum?");
            }

            usePlayerAxis = RotationOrigin == RotationOriginEnum.Hips && isHumanoid;
        }

        internal static void UpdateSettings()
        {
            if (!playerTransform) return;
            CharacterController characterController = playerTransform.GetComponent<CharacterController>();
            if (!characterController) return;

            if (rotating) GrabOriginTransform();

            if (rotating && !Utilities.IsInVR) characterController.enabled = !NoClipFlying;
            else if (!characterController.enabled)
                characterController.enabled = true;

            if (Utilities.IsInVR) Utilities.GetLocalVRCPlayer()?.prop_VRCPlayerApi_0.Immobilize(rotating);
        }

        internal void OnLeftWorld()
        {
            rotating = false;
            playerTransform = null;
            alignTrackingToPlayer = null;
        }

        internal enum RotationOriginEnum
        {

            Hips,

            ViewPoint,

            RightHand,

            LeftHand

        }

    }

}