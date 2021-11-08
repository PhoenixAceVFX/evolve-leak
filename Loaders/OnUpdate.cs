using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evolve.Exploits;
using Evolve.FoldersManager;
using Evolve.Module;
using Evolve.Modules;
using Evolve.Movements;
using Evolve.Utils;
using UnityEngine;
using UnityEngine.XR;
using VRC.Core;

namespace Evolve.Loaders
{
    class OnUpdate
    {
        public static void Initialize()
        {
            if (FileCheck.FinishedCheck && !FileCheck.ShouldRestart)
            {
                Movements.Movements.Update();
                KeyBinds.Initialize();
                LoadingPictures.Initialize();
                ExploitsUpdate.Initialize();
                //ModMain.OnUpdate();
                if (Settings.IsAdmin)
                {
                    if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha0))
                    {
                        Resources.FindObjectsOfTypeAll<DebugLogGui>().First().field_Public_Boolean_0 = !Resources.FindObjectsOfTypeAll<DebugLogGui>().First().field_Public_Boolean_0;
                        Resources.FindObjectsOfTypeAll<DebugLogGui>().First().field_Public_Boolean_1 = !Resources.FindObjectsOfTypeAll<DebugLogGui>().First().field_Public_Boolean_1;
                        Resources.FindObjectsOfTypeAll<DebugLogGui>().First().field_Public_Boolean_2 = !Resources.FindObjectsOfTypeAll<DebugLogGui>().First().field_Public_Boolean_2;
                        Resources.FindObjectsOfTypeAll<DebugLogGui>().First().field_Public_Boolean_3 = !Resources.FindObjectsOfTypeAll<DebugLogGui>().First().field_Public_Boolean_3;
                    }
                    if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha9))
                    {
                        Resources.FindObjectsOfTypeAll<VRCNetworkGUI>().First().field_Public_Boolean_0 = !Resources.FindObjectsOfTypeAll<VRCNetworkGUI>().First().field_Public_Boolean_0;
                        Resources.FindObjectsOfTypeAll<VRCNetworkGUI>().First().field_Public_Boolean_1 = !Resources.FindObjectsOfTypeAll<VRCNetworkGUI>().First().field_Public_Boolean_1;
                        Resources.FindObjectsOfTypeAll<VRCNetworkGUI>().First().field_Public_Boolean_2 = !Resources.FindObjectsOfTypeAll<VRCNetworkGUI>().First().field_Public_Boolean_2;
                        Resources.FindObjectsOfTypeAll<VRCNetworkGUI>().First().field_Public_Boolean_3 = !Resources.FindObjectsOfTypeAll<VRCNetworkGUI>().First().field_Public_Boolean_3;
                    }
                }
                if (Settings.LovenseRemote) LovenseRemote.OnUpdate();
                if (!XRDevice.isPresent && !Application.isFocused) Application.targetFrameRate = 15;
                else Application.targetFrameRate = 999;
            }
        }
    }
}
