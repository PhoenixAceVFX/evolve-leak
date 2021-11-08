using Evolve.AvatarList;
using Evolve.Buttons;
using Evolve.ConsoleUtils;
using Evolve.Exploits;
using Evolve.FoldersManager;
using Evolve.Modules;
using Evolve.Modules.PostProcessing;
using Evolve.Protections;
using Evolve.Utils;
using Evolve.Yoink;
using MelonLoader;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Evolve.Loaders
{
    class OnSceneLoaded
    {
        public static IEnumerator Initialize(int level)
        {
            if (!FileCheck.FinishedCheck) yield return null;
            if (!FileCheck.ShouldRestart && level == -1)
            {
                try
                {
                    MelonCoroutines.Start(WorldMenu.CheckWorld());
                    MelonCoroutines.Start(WorldMenuInteract.CheckWorld());
                    MelonCoroutines.Start(GetIsEvolved.CreateHandler());
                    MelonCoroutines.Start(InstanceHistoryMenu.EnteredWorld());
                    MelonCoroutines.Start(EvolveMessageReader.CreateHandler());
                    MelonCoroutines.Start(EvolveCommands.CreateHandler());
                    MelonCoroutines.Start(ExploitsMenu.CheckNewWorld());
                    ExploitsOnLoaded.Initialize();
                    PostProcessing.OnLevelWasLoaded(level);
                    LoadingPictures.OnLevelWasInitialized();
                    RPCMenu.RPCList.Clear();
                    UdonRPCMenu.UdonRPCList.Clear();
                    MovementsMenu.Fly.setToggleState(false, true);
                    MovementsMenu.NoCliping.setToggleState(false, true);
                    SelfMenu.MirrorToggle.setToggleState(false, true);
                    Physics.gravity = new Vector3(0, -9.81f, 0);
                    WorldMenu.AppliedChanges = false;
                    Esp.PickupsEsp(Settings.ItemEsp);
                    Esp.TriggersEsp(Settings.TriggersEsp);
                    Toggles.Pickups(!Settings.PickupsProtection);
                    Toggles.Chairs(!Settings.ChairsProtection);
                    if (Settings.Blink) MovementsMenu.BlinkBut.setToggleState(false, true);
                }
                catch { }
            }
        }
    }
}
