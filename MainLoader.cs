using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evolve.Loaders;
using MelonLoader;

namespace Evolve.Loaders
{
    public class MainLoader
    {
        public static unsafe void OnApplicationStart()
        {
            MelonCoroutines.Start(OnStart.Initialize());
        }

        public static void OnLevelWasLoaded(int level)
        {
            MelonCoroutines.Start(OnSceneLoaded.Initialize(level));
        }

        public static void OnUpdate()
        {
            Loaders.OnUpdate.Initialize();
        }

        public static void OnApplicationQuit()
        {
             OnQuit.Initialize();
        }

        public static void OnUIManagerInit()
        {
        }
    }
}
