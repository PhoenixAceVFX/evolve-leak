using System.Reflection;
using MelonLoader;
using UnhollowerRuntimeLib.XrefScans;
using Harmony;
using UnityEngine;
using System.Linq;

namespace Evolve.Modules
{
    public class MenuFix : MonoBehaviour
    {
        public static void Initialize()
        {
            var harmony = Evolve.Patch.Patches.Instance;
            var method = Method;
            harmony.Patch(typeof(VRCUiManager).GetMethod(method.Name), GetPatch(nameof(MenuFix.Patch)));
        }

        private static HarmonyMethod GetPatch(string name)
        {
            return new HarmonyMethod(typeof(MenuFix).GetMethod(name, BindingFlags.NonPublic | BindingFlags.Static));
        }

        private static bool Patch(VRCUiManager __instance, bool __0)
        {
            if (!ComfyUtils.IsInVR()) return true;
            float num = ComfyUtils.GetVRCTrackingManager() != null ? ComfyUtils.GetVRCTrackingManager().transform.localScale.x : 1f;
            if (num <= 0f)
            {
                num = 1f;
            }
            var playerTrackingDisplay = __instance.transform;
            var unscaledUIRoot = __instance.transform.Find("UnscaledUI");
            playerTrackingDisplay.position = ComfyUtils.GetWorldCameraPosition();
            Vector3 rotation = GameObject.Find("Camera (eye)").transform.rotation.eulerAngles;
            Vector3 euler = new Vector3(rotation.x - 30f, rotation.y, 0f);
            //if (rotation.x > 0f && rotation.x < 300f) rotation.x = 0f;
            if (ComfyUtils.GetVRCPlayer() == null)
            {
                euler.x = euler.z = 0f;
            }
            if (!__0)
            {
                playerTrackingDisplay.rotation = Quaternion.Euler(euler);
            }
            else
            {
                Quaternion quaternion = Quaternion.Euler(euler);
                if (!(Quaternion.Angle(playerTrackingDisplay.rotation, quaternion) < 15f))
                {
                    if (!(Quaternion.Angle(playerTrackingDisplay.rotation, quaternion) < 25f))
                    {
                        playerTrackingDisplay.rotation = Quaternion.RotateTowards(playerTrackingDisplay.rotation, quaternion, 5f);
                    }
                    else
                    {
                        playerTrackingDisplay.rotation = Quaternion.RotateTowards(playerTrackingDisplay.rotation, quaternion, 1f);
                    }
                }
            }
            if (num >= 0f)
            {
                playerTrackingDisplay.localScale = num * Vector3.one;
            }
            else
            {
                playerTrackingDisplay.localScale = Vector3.one;
            }
            if (num > float.Epsilon)
            {
                unscaledUIRoot.localScale = 1f / num * Vector3.one;
            }
            else
            {
                unscaledUIRoot.localScale = Vector3.one;
            }
            return false;
        }

        public static MethodInfo Method
        {
            get
            {
                if (_UI == null)
                {
                    try
                    {
                        var xrefs = XrefScanner.XrefScan(typeof(VRCUiManager).GetMethod(nameof(VRCUiManager.LateUpdate)));
                        foreach (var x in xrefs)
                        {
                            if (x.Type == XrefType.Method && x.TryResolve() != null &&
                                x.TryResolve().GetParameters().Length == 2 &&
                                x.TryResolve().GetParameters().All(a => a.ParameterType == typeof(bool)))
                            {
                                _UI = (MethodInfo) x.TryResolve();
                                break;
                            }
                        };
                    }
                    catch
                    {
                    }
                }
                return _UI;
            }
        }



#pragma warning disable CS0169 // Le champ 'ComfyVRMenu._comfyVRMenu' n'est jamais utilisé
#pragma warning restore CS0169 // Le champ 'ComfyVRMenu._comfyVRMenu' n'est jamais utilisé

        private static MethodInfo _UI;
    }
}