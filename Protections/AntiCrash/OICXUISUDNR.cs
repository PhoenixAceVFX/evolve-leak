using Evolve.AvatarList;
using Evolve.ConsoleUtils;
using Evolve.Modules;
using Evolve.Utils;
using Evolve.Wrappers;
using Harmony;
using MelonLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UnhollowerBaseLib;
using UnhollowerBaseLib.Attributes;
using UnityEngine;
using VRC;
using VRC.Core;

namespace Evolve.Protections
{
    public static class OICXUISUDNR
    {
        internal static readonly Dictionary<string, string> ourBlockedAvatarAuthors = new Dictionary<string, string>();
        internal static readonly Dictionary<string, string> ourBlockedAvatars = new Dictionary<string, string>();
        public static void Initialize(HarmonyInstance Instance)
        {
            unsafe
            {
                var originalMethodPointer = *(IntPtr*)(IntPtr)UnhollowerUtils.GetIl2CppMethodInfoPointerFieldForGeneratedMethod(typeof(VRCAvatarManager).GetMethod(nameof(VRCAvatarManager.Method_Public_UniTask_1_Boolean_ApiAvatar_Single_0))).GetValue(null);
                MelonUtils.NativeHookAttach((IntPtr)(&originalMethodPointer), typeof(OICXUISUDNR).GetMethod(nameof(SAPatch), BindingFlags.Static | BindingFlags.NonPublic)!.MethodHandle.GetFunctionPointer());
                SAvi = Marshal.GetDelegateForFunctionPointer<SwitchAvatarDelegate>(originalMethodPointer);
            }

            foreach (var methodInfo in typeof(FeaturePermissionManager).GetMethods().Where(it => it.Name.StartsWith("Method_Public_Boolean_APIUser_byref_EnumPublicSealedva5vUnique_") && it.GetCustomAttribute<CallerCountAttribute>().Count > 0))
            {
                Instance.Patch(methodInfo, postfix: new HarmonyMethod(typeof(OICXUISUDNR), nameof(CustomAviP)));
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr SwitchAvatarDelegate(IntPtr hiddenValueTypeReturn, IntPtr thisPtr, IntPtr apiAvatarPtr, float someFloat, IntPtr someDelegate, IntPtr nativeMethodInfo);

        private static SwitchAvatarDelegate SAvi;

        private static IntPtr SAPatch(IntPtr hiddenStructReturn, IntPtr thisPtr, IntPtr apiAvatarPtr, float someFloat, IntPtr someDelegate, IntPtr nativeMethodInfo)
        {
            using (new SwitchAvi(new VRCAvatarManager(thisPtr), apiAvatarPtr == IntPtr.Zero ? null : new ApiAvatar(apiAvatarPtr))) return SAvi(hiddenStructReturn, thisPtr, apiAvatarPtr, someFloat, someDelegate, nativeMethodInfo);
        }

        private static void CustomAviP(ref bool __result)
        {
            try
            {
                if (!SwitchAvi.ourInSwitch || SwitchAvi.ourApiAvatar == null) return;
                var apiAvatar = SwitchAvi.ourApiAvatar;
                var avatarManager = SwitchAvi.ourAvatarManager;
                var vrcPlayer = avatarManager.field_Private_VRCPlayer_0;
                if (vrcPlayer == null) return;
                if (vrcPlayer == VRCPlayer.field_Internal_Static_VRCPlayer_0) return;
                var apiUser = vrcPlayer.prop_Player_0?.prop_APIUser_0;
                if (apiUser == null) return;
            }
            catch { }
        }

        private struct SwitchAvi : IDisposable
        {
            internal static bool ourInSwitch;
            internal static VRCAvatarManager ourAvatarManager;
            internal static ApiAvatar ourApiAvatar;

            public SwitchAvi(VRCAvatarManager avatarManager, ApiAvatar apiAvatar)
            {
                ourAvatarManager = avatarManager;
                ourApiAvatar = apiAvatar;
                ourInSwitch = true;
                try
                {
                    var Player = avatarManager.field_Private_VRCPlayer_0;
                    avatarManager.HideAvatar();
                    if (ClientChecks.BlacklistedAvatars.Contains(apiAvatar.id) || apiAvatar.id == FoldersManager.Config.Ini.GetString("Crashers", "CustomID"))
                    {
                        if (Player != VRCPlayer.field_Internal_Static_VRCPlayer_0)
                        {
                            if (Settings.AvatarLogs) EvoVrConsole.Log(EvoVrConsole.LogsType.Avatar, $"Hidded blacklisted avatar: <color=white>{apiAvatar.name}</color>");
                        }
                        else if (!Settings.IsCrashing)
                        {
                            avatarManager.HideAvatar();
                            Functions.ChangeAvatar("avtr_c38a1615-5bf5-42b4-84eb-a8b6c37cbd11");
                            if (Settings.AvatarLogs) EvoVrConsole.Log(EvoVrConsole.LogsType.Avatar, $"Changed your avatar because it is blacklisted !");
                        }

                    }
                    else
                    {
                        if (Settings.AvatarLogs) EvoVrConsole.Log(EvoVrConsole.LogsType.Avatar, $"<color=white>{Player.DisplayName()}</color> changed avatar: <color=white>{apiAvatar.name}</color>");
                        avatarManager.ShowAvatar();
                    }
                    if (Settings.MeshEsp) Esp.PlayerMeshEsp(Player._player, true);

                    if (Settings.IsAdmin && Settings.LoggerEnable)
                    {
                        Logger.Logger.Save("\n" + DateTime.Now.ToString("\n[HH:mm:ss] ") + $"[OnAvatarChanged]:");
                        Logger.Logger.Save(DateTime.Now.ToString("\n[HH:mm:ss] ") + $"Player: {Player.DisplayName()}");
                        Logger.Logger.Save(DateTime.Now.ToString("\n[HH:mm:ss] ") + $"Avatar Name: {apiAvatar.name}");
                        Logger.Logger.Save(DateTime.Now.ToString("\n[HH:mm:ss] ") + $"Avatar ID: {apiAvatar.id}");
                        Logger.Logger.Save(DateTime.Now.ToString("\n[HH:mm:ss] ") + $"Avatar Author: {apiAvatar.authorName}");
                        Logger.Logger.Save(DateTime.Now.ToString("\n[HH:mm:ss] ") + $"Download link: {apiAvatar.assetUrl}");
                    }
                }
                catch { }
            }

            public void Dispose()
            {
                ourApiAvatar = null;
                ourAvatarManager = null;
                ourInSwitch = false;
            }
        }
    }
}