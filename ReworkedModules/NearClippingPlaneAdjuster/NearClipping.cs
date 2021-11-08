using Evolve.ConsoleUtils;
using UnityEngine;
using VRC.UserCamera;

namespace Evolve.Modules.NearClippingPlaneAdjuster
{
    internal class NearClipping
    {
        public static void ChangeNearClipPlane(float value)
        {
            VRCVrCamera vrCamera = VRCVrCamera.field_Private_Static_VRCVrCamera_0;
            if (!vrCamera)
            {
                return;
            }

            Camera screenCamera = vrCamera.field_Public_Camera_0;
            if (!screenCamera)
            {
                return;
            }

            screenCamera.nearClipPlane = value;
            EvoVrConsole.Log(EvoVrConsole.LogsType.Info, "Clipping distance adjusted: " + value);
            ChangePhotoCameraNearField(value);
        }

        public static void ChangePhotoCameraNearField(float value)
        {
            var cameraController = UserCameraController.field_Internal_Static_UserCameraController_0;
            if (cameraController == null)
            {
                return;
            }

            Camera cam = cameraController.field_Internal_UserCameraIndicator_0.GetComponent<Camera>();
            if (cam != null)
            {
                cam.nearClipPlane = value;
            }
        }
    }
}