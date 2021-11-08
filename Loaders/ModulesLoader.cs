using DiscordRichPresence;
using Evolve.Buttons;
using Evolve.ConsoleUtils;
using Evolve.FoldersManager;
using Evolve.LoadingMusic;
using Evolve.Module;
using Evolve.Modules;
using Evolve.Modules.AvatarHider;
using Evolve.Modules.PlayersList;
using Evolve.Utils;
using MelonLoader;
using System.Collections;
using UnityEngine;

namespace Evolve.Loaders
{
    internal class ModulesLoader
    {
        public static IEnumerator Initialize()
        {
            while (!FileCheck.FinishedCheck) yield return null;
            while (VRCUiManager.prop_VRCUiManager_0 == null) yield return null;
            if (!FileCheck.ShouldRestart)
            {
                MelonCoroutines.Start(Movements.Movements.Initialize());
                MelonCoroutines.Start(StartingStuff.Time());
                MelonCoroutines.Start(StartingStuff.LowerPanel());
                MelonCoroutines.Start(EvoVrConsole.Initialize());
                MelonCoroutines.Start(Protections.ClientChecks.ChecksLoop());
                MelonCoroutines.Start(Protections.ClientChecks.CheckUserLoop());
                MelonCoroutines.Start(Esp.Loop());
                MelonCoroutines.Start(LoadingColors.Particles());
                MelonCoroutines.Start(Optimizations.Loop());
                MelonCoroutines.Start(StartingStuff.StartSong());
                MelonCoroutines.Start(UIChanges.Initialize());
                MelonCoroutines.Start(UIChanges.Initialize2());
                MelonCoroutines.Start(FiveSecTimer());
                MelonCoroutines.Start(AviDistanceHide.AvatarScanner());
                MelonCoroutines.Start(PlayersList.UpdatePlayerList());
                MelonCoroutines.Start(PlayersList.Initialize());
                MelonCoroutines.Start(Exploits.Exploits.AutoDropItems());
                MelonCoroutines.Start(Exploits.Exploits.FramesDropping());
                MelonCoroutines.Start(Exploits.Exploits.MenuRemover());
                MelonCoroutines.Start(PlayersInformations.SelfInfoInit());
                MelonCoroutines.Start(PlayersInformations.UserInfoInit());
                MelonCoroutines.Start(PlayersInformations.EvoInfo());
                AutoCalibrations.Initialize();
                UpdateLoader.Update();
                CheckIfBlacklistedMod.Check();
                CustomNameplates.Initialize();
                MenuFix.Initialize();
                ColorChanger.ApplyNewColor();
                LoadingSong.Initialize();
                DiscordManager.Init();
                AvatarList.Initialize.Module();
                MelonCoroutines.Start(Oversee.PlayerListUpdate());
                ThirdPerson.Initiliaze();
                QMFreeze.OnUI();
                ImmersiveTouch.OnUiManagerInit();
                HighlightsFX.prop_HighlightsFX_0.field_Protected_Material_0.SetColor("_HighlightColor", new Color(0.3f, 0, 5));
                if (Settings.ComeBack) MelonCoroutines.Start(ComeBack.ComeBack.OnStart());
                if (Settings.UnlimitedFrames) Application.targetFrameRate = 999;
                if (Settings.FullScreenOnStart) Screen.fullScreen = true;
                if (Config.Ini.GetFloat("Misc", "FOV") > 60) Camera.main.fieldOfView = Config.Ini.GetFloat("Misc", "FOV");
            }
        }

        public static IEnumerator FiveSecTimer()
        {
            while (true)
            {
                yield return new WaitForSeconds(5f);
                {
                    DiscordManager.Update();
                }
            }
        }
    }
}