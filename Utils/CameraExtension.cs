using System.Collections.Generic;
using UnityEngine;
using VRC;
using VRC.UserCamera;

namespace Evolve.Utils
{
    internal class CameraExtensionUtils
    {
        internal static class CameraExtension
        {
            public static void ResetCamera()
            {
                CameraExtension.SetCameraMode(CameraExtension.CameraMode.Off);
                CameraExtension.SetCameraMode(CameraExtension.CameraMode.Photo);
                UserCameraController userCameraController = Wrappers.Utils.UserCameraController;
                CameraExtension.worldCameraVector = userCameraController.field_Public_Transform_0.transform.position;
                CameraExtension.worldCameraQuaternion = userCameraController.field_Public_Transform_0.transform.rotation;
                CameraExtension.worldCameraQuaternion *= Quaternion.Euler(90f, 0f, 180f);
                userCameraController.field_Public_Transform_0.transform.position = userCameraController.field_Internal_UserCameraIndicator_0.transform.position;
                userCameraController.field_Public_Transform_0.transform.rotation = userCameraController.field_Internal_UserCameraIndicator_0.transform.rotation;
            }

            public static void RotateAround(Vector3 center, Vector3 axis, float angle)
            {
                Vector3 worldCameraVector = CameraExtension.worldCameraVector;
                Quaternion quaternion = Quaternion.AngleAxis(angle, axis);
                Vector3 vector = worldCameraVector - center;
                vector = quaternion * vector;
                CameraExtension.worldCameraVector = center + vector;
                Quaternion worldCameraQuaternion = CameraExtension.worldCameraQuaternion;
                CameraExtension.worldCameraQuaternion *= Quaternion.Inverse(worldCameraQuaternion) * quaternion * worldCameraQuaternion;
            }

            public static void TakePicture(int timer)
            {
                UserCameraController userCameraController = Wrappers.Utils.UserCameraController;
                userCameraController.prop_Int32_0 = 0;
                userCameraController.StartCoroutine(userCameraController.Method_Private_IEnumerator_Int32_PDM_0(timer));
            }

            public static void Disable()
            {
                UserCameraController userCameraController = Wrappers.Utils.UserCameraController;
                userCameraController.enabled = false;
            }

            public static void Enable()
            {
                UserCameraController userCameraController = Wrappers.Utils.UserCameraController;
                userCameraController.enabled = true;
                userCameraController.StopAllCoroutines();
            }

            public static void PictureRPC(Player Target)
            {
                UserCameraIndicator field_Internal_UserCameraIndicator_ = Wrappers.Utils.UserCameraController.field_Internal_UserCameraIndicator_0;
                field_Internal_UserCameraIndicator_.PhotoCapture(Target);
            }

            public static Dictionary<string, int> Filters()
            {
                return new Dictionary<string, int>
            {
                {
                    "None",
                    0
                },
                {
                    "Blueprint",
                    10
                },
                {
                    "Code",
                    4
                },
                {
                    "Sparkles",
                    5
                },
                {
                    "Green\nScreen",
                    7
                },
                {
                    "Hypno",
                    6
                },
                {
                    "Alpha\nTransparent",
                    8
                },
                {
                    "Drawing",
                    9
                },
                {
                    "Glitch",
                    3
                },
                {
                    "Pixelate",
                    2
                },
                {
                    "Old Timey",
                    1
                },
                {
                    "Trippy",
                    11
                }
            };
            }

            public static void SetCameraMode(CameraExtension.CameraMode mode)
            {
                Wrappers.Utils.UserCameraController.prop_UserCameraMode_0 = (UserCameraMode) mode;
            }

            public static void CycleCameraBehaviour()
            {
                Wrappers.Utils.UserCameraController.field_Internal_UserCameraIndicator_0.transform.Find("PhotoControls/Left_CameraMode").GetComponent<CameraInteractable>().Interact();
            }

            public static void CycleCameraSpace()
            {
                Wrappers.Utils.UserCameraController.field_Internal_UserCameraIndicator_0.transform.Find("PhotoControls/Left_Space").GetComponent<CameraInteractable>().Interact();
            }

            public static void TogglePinMenu()
            {
                Wrappers.Utils.UserCameraController.field_Internal_UserCameraIndicator_0.transform.Find("PhotoControls/Left_Pins").GetComponent<CameraInteractable>().Interact();
            }

            public static void ToggleLock()
            {
                Wrappers.Utils.UserCameraController.field_Internal_UserCameraIndicator_0.transform.Find("PhotoControls/Right_Lock").GetComponent<CameraInteractable>().Interact();
            }

            public static Vector3 worldCameraVector
            {
                get
                {
                    return Wrappers.Utils.UserCameraController.field_Private_Vector3_0;
                }
                set
                {
                    Wrappers.Utils.UserCameraController.field_Private_Vector3_0 = value;
                }
            }

            public static Quaternion worldCameraQuaternion
            {
                get
                {
                    return Wrappers.Utils.UserCameraController.field_Private_Quaternion_0;
                }
                set
                {
                    Wrappers.Utils.UserCameraController.field_Private_Quaternion_0 = value;
                }
            }

            public static CameraExtension.CameraBehaviour GetCameraBehaviour()
            {
                return (CameraExtension.CameraBehaviour) Wrappers.Utils.UserCameraController.prop_UserCameraMovementBehaviour_0;
            }

            public static CameraExtension.CameraSpace GetCameraSpace()
            {
                return (CameraExtension.CameraSpace) Wrappers.Utils.UserCameraController.prop_UserCameraMovementBehaviour_0;
            }

            public static CameraExtension.Pin GetCurrentPin()
            {
                return (CameraExtension.Pin) Wrappers.Utils.UserCameraController.prop_Int32_0;
            }

            public enum CameraMode
            {
                Off,
                Photo,
                Video
            }

            public enum CameraScale
            {
                Normal,
                Medium,
                Big
            }

            public enum CameraBehaviour
            {
                None,
                Smooth,
                LookAt
            }

            public enum CameraSpace
            {
                Attached,
                Local,
                World,
                COUNT
            }

            public enum Pin
            {
                Pin1,
                Pin2,
                Pin3
            }
        }
    }
}