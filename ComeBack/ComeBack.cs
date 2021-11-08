using Evolve.ConsoleUtils;
using Evolve.Utils;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using VRC.Core;

namespace Evolve.ComeBack
{
    internal class ComeBack
    {
        public static void OnQuit()
        {
            ApiWorldInstance Instance = RoomManager.field_Internal_Static_ApiWorldInstance_0;
            FoldersManager.Config.Ini.SetString("World", "ID", Instance.id);
            FoldersManager.Config.Ini.SetFloat("World", "VectorX", VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position.x);
            FoldersManager.Config.Ini.SetFloat("World", "VectorY", VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position.y);
            FoldersManager.Config.Ini.SetFloat("World", "VectorZ", VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position.z);
            if (VRCPlayer.field_Internal_Static_VRCPlayer_0.field_Private_VRCPlayerApi_0.IsUserInVR()) Utilities.StartProcess(Directory.GetCurrentDirectory() + "/VRChat.exe", $"vrchat://launch?id={Instance.id}");
            else Utilities.StartProcess(Directory.GetCurrentDirectory() + "/VRChat.exe", $"vrchat://launch?id={Instance.id} --no-vr");
        }

        public static IEnumerator OnEnable()
        {
            if (Settings.AlreadySavedData) yield return null;
            ApiWorldInstance Instance = RoomManager.field_Internal_Static_ApiWorldInstance_0;
            while (Instance == null) yield return null;


            while (VRCPlayer.field_Internal_Static_VRCPlayer_0 != true) yield return null;
            {
                FoldersManager.Config.Ini.SetString("World", "ID", Instance.id);
                FoldersManager.Config.Ini.SetFloat("World", "VectorX", VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position.x);
                FoldersManager.Config.Ini.SetFloat("World", "VectorY", VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position.y);
                FoldersManager.Config.Ini.SetFloat("World", "VectorZ", VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position.z);
            }
        }

        public static IEnumerator OnStart()
        {
            while (VRCPlayer.field_Internal_Static_VRCPlayer_0 != true) yield return null;
            if (RoomManager.field_Internal_Static_ApiWorldInstance_0.id != FoldersManager.Config.Ini.GetString("World", "ID"))
            {
                EvoVrConsole.Log(EvoVrConsole.LogsType.Info, "VRChat Crashed, sending you back to saved world...");
                yield return new WaitForSeconds(2);
                while (VRCPlayer.field_Internal_Static_VRCPlayer_0 != false)
                {
                    Functions.ForceJoin(FoldersManager.Config.Ini.GetString("World", "ID"));
                    yield return new WaitForSeconds(1);
                }
                yield return new WaitForSeconds(2);
                while (VRCPlayer.field_Internal_Static_VRCPlayer_0 != true) yield return null;
                yield return new WaitForSeconds(2);
                VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position = new UnityEngine.Vector3(FoldersManager.Config.Ini.GetFloat("World", "VectorX"), FoldersManager.Config.Ini.GetFloat("World", "VectorY"), FoldersManager.Config.Ini.GetFloat("World", "VectorZ"));
            }
            else
            {
                EvoVrConsole.Log(EvoVrConsole.LogsType.Info, "VRChat Crashed, sending you back to your last position...");
                yield return new WaitForSeconds(2);
                VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position = new UnityEngine.Vector3(FoldersManager.Config.Ini.GetFloat("World", "VectorX"), FoldersManager.Config.Ini.GetFloat("World", "VectorY"), FoldersManager.Config.Ini.GetFloat("World", "VectorZ"));
            }
        }
    }
}