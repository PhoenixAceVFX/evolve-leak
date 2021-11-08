using Evolve.ApiLogs;
using Evolve.ConsoleUtils;
using Evolve.FoldersManager;
using Evolve.Login;
using Evolve.Module;
using Evolve.Modules.PostProcessing;
using Evolve.Yoink;
using Evolve.Patch;
using Evolve.Protections;
using MelonLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnhollowerRuntimeLib;
using Evolve.Modules;
using UnityEngine;

namespace Evolve.Loaders
{
    class OnStart
    {
        public static void ConsoleASCII()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(@"                      _____               _                 _____                _");
            Console.WriteLine(@"                     | ____|__   __ ___  | |__   __ ___    | ____| _ __    __ _ (_) _ __    ___");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(@"                     |  _|  \ \ / // _ \ | |\ \ / // _ \   |  _|  | '_ \  / _` || || '_ \  / _ \");
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine(@"                     | |___  \ V /| (_) || | \ V /|  __/   | |___ | | | || (_| || || | | ||  __/");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(@"                     |_____|  \_/  \___/ |_|  \_/  \___|   |_____||_| |_| \__, ||_||_| |_| \___|");
            Console.WriteLine(@"                                                                          | ___/");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static IEnumerator Initialize()
        {
            ClassInjector.RegisterTypeInIl2Cpp<Yoinker>();
            ClassInjector.RegisterTypeInIl2Cpp<YoinkUtils>();
            ClassInjector.RegisterTypeInIl2Cpp<EnableDisableListener>();
            MelonCoroutines.Start(Protections.ClientChecks.OfflineCheck());
            MelonCoroutines.Start(ClientChecks.EvolveDllProtection());
            Console.Title = "Evolve Engine";
            ConsoleASCII();
            MelonCoroutines.Start(FileCheck.Initialize());
            while (!FileCheck.FinishedCheck) yield return null;
            while (ReferenceEquals(NetworkManager.field_Internal_Static_NetworkManager_0, null)) yield return null;
            while (ReferenceEquals(VRCAudioManager.field_Private_Static_VRCAudioManager_0, null)) yield return null;
            while (ReferenceEquals(VRCUiManager.prop_VRCUiManager_0, null)) yield return null;

            if (!FileCheck.ShouldRestart)
            {
                UnityEngine.Object.DontDestroyOnLoad(new GameObject("Yoinker").AddComponent<Yoinker>());
                Config.CheckConfig();
                ConsoleASCII();
                EvoConsole.Initialize();
                Patches.Initialize();
                AvatarList.Initialize.OnStart();
                N5HF8S65K.Initialize();
                MelonCoroutines.Start(Authorization.Check());
                ApiLog.Start();
                QMFreeze.Apply();
                MelonCoroutines.Start(ButtonsLoader.Initialize());
                MelonCoroutines.Start(ModulesLoader.Initialize());
            }
        }
    }
}
