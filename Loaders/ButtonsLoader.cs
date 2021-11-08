using Evolve.Buttons;
using Evolve.FoldersManager;
using Evolve.Module;
using System.Collections;
using System.Collections.Generic;

namespace Evolve.Loaders
{
    internal class ButtonsLoader
    {
        public static IEnumerator Initialize()
        {
            while (!FileCheck.FinishedCheck) yield return null;
            while (VRCUiManager.prop_VRCUiManager_0 == null) yield return null;
            if (!FileCheck.ShouldRestart)
            {
                EvolveMenu.Initialize();
                MusicMenu.Initialize();
                SelfMenu.Initialize();
                LobbyMenu.Initialzie();
                ProtectionsMenu.Initialzie();
                TouchMenu.Initialize();
                EvolveInteract.Initialize();
                MovementsMenu.Initialize();
                ExploitsMenu.Initialize();
                LogsMenu.Initialize();
                TouchInteractMenu.Initialize();
                UIMenu.Initialize();
                InstanceHistoryMenu.Initialize();
                DiscordMenu.Initialize();
                WorldMenu.Initialize();
                ScreenButtons.Initialize();
                MiscMenu.Initialize();
                KeybindsMenu.Initialize();
                WorldMenuInteract.Initialize();
                LovenseRemote.Initialiaze();
                ExploitsInteractMenu.Initialize();
                OverseeMenu.Initialize();
                MessageMenu.Initialize();
                RPCMenu.Initialize();
                AvatarsMenu.Initialize();
                EvolveAvatarsMenu.Initialize();
                UdonRPCMenu.Initialize();
            }
        }
    }
}