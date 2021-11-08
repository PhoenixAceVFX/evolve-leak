using Evolve.Patch;
using Harmony;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Evolve.Modules
{
    public class AutoCalibrations
    {

        //Credits to Requis the racist
        private class Calibrations
        {
            public KeyValuePair<Vector3, Quaternion> Hip;
            public KeyValuePair<Vector3, Quaternion> LeftFoot;
            public KeyValuePair<Vector3, Quaternion> RightFoot;
        }

        private static Dictionary<string, Calibrations> _savedCalibrations;
        private const string CalibrationsDirectory = "Evolve/Calibrations/";
        private const string CalibrationsFile = "AutoSave.json";

        public static void Initialize()
        {
            Directory.CreateDirectory(CalibrationsDirectory);

            if (File.Exists($"{CalibrationsDirectory}{CalibrationsFile}")) _savedCalibrations = JsonConvert.DeserializeObject<Dictionary<string, Calibrations>>(File.ReadAllText($"{CalibrationsDirectory}{CalibrationsFile}"));
            else
            {
                _savedCalibrations = new Dictionary<string, Calibrations>(128);
                File.WriteAllText($"{CalibrationsDirectory}{CalibrationsFile}", JsonConvert.SerializeObject(_savedCalibrations, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }));
            }

            var methods = typeof(VRCTrackingSteam).GetMethods();
            foreach (var methodInfo in methods)
            {
                switch (methodInfo.GetParameters().Length)
                {
                    case 1 when methodInfo.GetParameters().First().ParameterType == typeof(string) && methodInfo.ReturnType == typeof(bool) && methodInfo.GetRuntimeBaseDefinition() == methodInfo:
                        Patches.Instance.Patch(methodInfo, new HarmonyMethod(typeof(AutoCalibrations).GetMethod(nameof(CheckCalibration), BindingFlags.Static | BindingFlags.NonPublic)));
                        break;
                    case 3 when methodInfo.GetParameters().First().ParameterType == typeof(Animator) && methodInfo.ReturnType == typeof(void) && methodInfo.GetRuntimeBaseDefinition() == methodInfo:
                        Patches.Instance.Patch(methodInfo, null, new HarmonyMethod(typeof(AutoCalibrations).GetMethod(nameof(Calibrate), BindingFlags.Static | BindingFlags.NonPublic)));
                        break;
                }
            }
        }

        private static void Calibrate(ref VRCTrackingSteam __instance, Animator __0, bool __1, bool __2)
        {
            var avatarId = VRCPlayer.field_Internal_Static_VRCPlayer_0._player.prop_ApiAvatar_0.id;
            _savedCalibrations[avatarId] = new Calibrations
            {
                LeftFoot = new KeyValuePair<Vector3, Quaternion>(__instance.field_Public_Transform_10.localPosition, __instance.field_Public_Transform_10.localRotation),
                RightFoot = new KeyValuePair<Vector3, Quaternion>(__instance.field_Public_Transform_11.localPosition, __instance.field_Public_Transform_11.localRotation),
                Hip = new KeyValuePair<Vector3, Quaternion>(__instance.field_Public_Transform_12.localPosition, __instance.field_Public_Transform_12.localRotation),
            };

            try
            {
                File.WriteAllText($"{CalibrationsDirectory}{CalibrationsFile}", JsonConvert.SerializeObject(_savedCalibrations, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    ContractResolver = new DCR("normalized")
                }));
            }
            catch (Exception e)
            {
                File.WriteAllText($"{CalibrationsDirectory}error.log", e.Message);
            }
        }

        private static bool CheckCalibration(ref VRCTrackingSteam __instance, ref bool __result, string __0)
        {
            if (__instance.field_Private_String_0 == null)
            {
                __result = false;
                return true;
            }

            var avatarId = __0;
            if (_savedCalibrations.ContainsKey(avatarId))
            {
                var savedCalib = _savedCalibrations[avatarId];
                __instance.field_Public_Transform_10.localPosition = savedCalib.LeftFoot.Key;
                __instance.field_Public_Transform_10.localRotation = savedCalib.LeftFoot.Value;
                __instance.field_Public_Transform_11.localPosition = savedCalib.RightFoot.Key;
                __instance.field_Public_Transform_11.localRotation = savedCalib.RightFoot.Value;
                __instance.field_Public_Transform_12.localPosition = savedCalib.Hip.Key;
                __instance.field_Public_Transform_12.localRotation = savedCalib.Hip.Value;
            }

            __result = true;
            return false;
        }

        private class DCR : DefaultContractResolver
        {
            private readonly string _propertyNameToExclude;

            public DCR(string propertyNameToExclude)
            {
                _propertyNameToExclude = propertyNameToExclude;
            }

            protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
            {
                var properties = base.CreateProperties(type, memberSerialization);
                properties = properties.Where(p => string.Compare(p.PropertyName, _propertyNameToExclude, StringComparison.OrdinalIgnoreCase) != 0).ToList();
                return properties;
            }
        }
    }
}
