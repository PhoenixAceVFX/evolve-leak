using Evolve.Buttons;
using Evolve.ConsoleUtils;
using Evolve.Loaders;
using Evolve.Utils;
using MelonLoader;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine;
using Evolve.Module;
using UnhollowerRuntimeLib;

namespace Evolve.Loaders
{
    internal class MainMelonLoader : MelonMod
    {
        public override unsafe void OnApplicationStart()
        {
            MelonCoroutines.Start(OnStart.Initialize());
        }

        public override void OnLevelWasLoaded(int level)       
        {
            MelonCoroutines.Start(OnSceneLoaded.Initialize(level));
        }

        public override void OnUpdate()
        {
            Loaders.OnUpdate.Initialize();
        }

        public override void OnApplicationQuit()         
        {
            OnQuit.Initialize();
        }
    }
}