using Evolve.Api;
using Evolve.Wrappers;
using ButtonApi;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using VRC.Core;
using System.Collections;
using MelonLoader;
using System.IO;
using Evolve.ConsoleUtils;
using System;
using Evolve.Utils;
using VRC.SDK3.Components;
using System.Collections.Generic;

namespace Evolve.Bot
{
    internal class BotMenu
    {
        public static QMNestedButton ThisMenu;
        public static QMNestedButton ThisMenu2;
        public static QMSingleButton Orbit;
        public static QMSingleButton Follow;
        public static QMSingleButton Stop;
        public static QMSingleButton Join;
        public static QMSingleButton SelectTarget;
#pragma warning disable CS0649 // Le champ 'BotMenu.ResetTarget' n'est jamais assigné et aura toujours sa valeur par défaut null
        public static QMSingleButton ResetTarget;
#pragma warning restore CS0649 // Le champ 'BotMenu.ResetTarget' n'est jamais assigné et aura toujours sa valeur par défaut null
        public static QMSingleButton GoHome;
        public static QMSingleButton Respawn;
        public static QMSingleButton SpeedPlus;
        public static QMSingleButton SpeedMinus;
        public static QMSingleButton CurrentSpeed;
        public static QMSingleButton DistancePlus;
        public static QMSingleButton DistanceMinus;
        public static QMSingleButton CurrentDistance;
        public static QMSingleButton StartAll;
        public static QMSingleButton ShutDown;
        public static QMSingleButton SendBot;
        public static GameObject SphereObject;
        public static void Initialize()
        {
            ThisMenu = new QMNestedButton("ShortcutMenu", 999, 4, "Bot", "Evolve Engine Bot");
            ThisMenu.getMainButton().setActive(false);
            Panels.PanelMenu(ThisMenu, 0, 0, "\nMovement\ntypes", 1.1f, 2, "");
            Panels.PanelMenu(ThisMenu, 5, 0, "\nSpeed &\nDistance", 1.1f, 2, "");
            MelonCoroutines.Start(Target.Postion());

            MelonCoroutines.Start(ReworkedButtonAPI.CreateButton(MenuType.ShortCut, ButtonType.Single, "", 5, -1, delegate ()
            {
                QMStuff.ShowQuickmenuPage(BotMenu.ThisMenu.getMenuName());
            }, "Evolve Menu", Color.black, Color.clear, null, "https://i.imgur.com/0Q4aFHE.png"));

            MelonCoroutines.Start(ReworkedButtonAPI.CreateButton(MenuType.UserInteract, ButtonType.Single, "", 5, 4, delegate ()
            {
                QMStuff.ShowQuickmenuPage(BotMenu.ThisMenu2.getMenuName());
            }, "Evolve Menu", Color.black, Color.clear, null, "https://i.imgur.com/0Q4aFHE.png"));


            Join = new QMSingleButton(ThisMenu, 1, -0.75f, "Join me", () =>
            {
                ApiWorldInstance Instance = RoomManager.field_Internal_Static_ApiWorldInstance_0;
                Server.SendMessage($"Join/{Instance.id}");
            }, "Will make the bot join on you");

            SendBot = new QMSingleButton(ThisMenu, 2, -0.75f, "Send to ID", () =>
            {
                Wrappers.PopupManager.InputeText("Evolve Engine", "Confirm", new Action<string>((WorldID) =>
                {
                    Server.SendMessage($"Join/{WorldID}");
                }));
            }, "Will make the bot join an id");
            SendBot.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            GoHome = new QMSingleButton(ThisMenu, 3, -0.75f, "Send home", () =>
            {
                Server.SendMessage($"Join/wrld_0b4e5774-473d-4943-bb22-aac0c1b706f3:999~private(usr_04eb2511-cee1-4d9b-ace4-c029abb4ac75)~nonce(9E3835519D36774FF8248190D2BD855E80C62ACCF7970A75D9539400AA509E6E)");
            }, "Will make the bot go home");

            Respawn = new QMSingleButton(ThisMenu, 4, -0.75f, "Respawn", () =>
            {
                Server.SendMessage("Respawn");
            }, "Will respawn the bot");

            new QMToggleButton(ThisMenu, 2, 0, "Quest\nMaster crash", () =>
            {   
                Server.SendMessage($"Exploits/Event6M/True");
            }, "Disabled", () =>
            {
                Server.SendMessage($"Exploits/Event6M/False");
            }, "Lag the master");

            new QMToggleButton(ThisMenu, 3, 0, "Emote/Emoji\nCrash", () =>
            {
                Server.SendMessage($"Exploits/EmoteEmojiCrash/True");
            }, "Disabled", () =>
            {
                Server.SendMessage($"Exploits/EmoteEmojiCrash/False");
            }, "");

            new QMToggleButton(ThisMenu, 4, 0, "Friend\nEveryone", () =>
            {
                Server.SendMessage($"Utils/FriendRequest/True");
            }, "Disabled", () =>
            {
                Server.SendMessage($"Utils/FriendRequest/False");
            }, "");

            new QMToggleButton(ThisMenu, 1, 1, "Disconnect\nEveryone", () =>
            {
                Server.SendMessage($"Exploits/Disconnect/True");
            }, "Disabled", () =>
            {
                Server.SendMessage($"Exploits/Disconnect/False");
            }, "");


            new QMToggleButton(ThisMenu, 1, 0, "Quest\nCrash", () =>
            {
                Server.SendMessage($"Exploits/Event6/True");
            }, "Poggers", () =>
            {
                Server.SendMessage($"Exploits/Event6/False");
            }, "Lag the lobby");

            new QMToggleButton(ThisMenu, 2, 1, "Desync", () =>
            {
                Server.SendMessage($"Exploits/Desync/True");
            }, "Disabled", () =>
            {
                Server.SendMessage($"Exploits/Desync/False");
            }, "Desync the lobby");

            new QMToggleButton(ThisMenu, 3, 1, "Portal\nCrash", () =>
            {
                Server.SendMessage($"Exploits/PortalCrash/True");
            }, "Disabled", () =>
            {
                Server.SendMessage($"Exploits/PortalCrash/False");
            }, "Desync the lobby");

            Orbit = new QMSingleButton(ThisMenu, 0, -0.25f, "Orbit", () =>
            {
                Server.SendMessage($"Movements/Orbit");
                if (SphereObject != null) UnityEngine.Object.Destroy(SphereObject);
                Target.FollowType = "Orbit";
            }, "Will change movements of the bot for Orbit");

            Follow = new QMSingleButton(ThisMenu, 0, 0.25f, "Hold", () =>
            {
                if (SphereObject != null) UnityEngine.Object.Destroy(SphereObject);
                var Sphere = new GameObject("Sphere");
                Sphere.AddComponent<MeshFilter>();
                Sphere.AddComponent<MeshRenderer>();
                Sphere.AddComponent<VRCPickup>();
                Sphere.AddComponent<Rigidbody>();
                Sphere.AddComponent<SphereCollider>();
                var MeshFilter = Sphere.GetComponent<MeshFilter>();
                var MeshRenderer = Sphere.GetComponent<MeshRenderer>();
                var Pickup = Sphere.GetComponent<VRCPickup>();
                var RigidBody = Sphere.GetComponent<Rigidbody>();
                var AllMeshes = UnityEngine.Resources.FindObjectsOfTypeAll<Mesh>();
                foreach (var Mesh in AllMeshes)
                {
                    if (Mesh.name == "Sphere") MeshFilter.mesh = Mesh;
                }
                var Material = new Material(Shader.Find("Standard"));
                Material.color = Color.black;
                MeshRenderer.material = Material;
                RigidBody.mass = 0.9f;
                RigidBody.drag = 0;
                RigidBody.angularDrag = 0.05f;
                Pickup.pickupable = true;
                Pickup.ThrowVelocityBoostMinSpeed = 3;
                Pickup.AutoHold = VRC.SDKBase.VRC_Pickup.AutoHoldMode.Yes;
                Sphere.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

                SphereObject = Sphere;
                SphereObject.transform.position = new Vector3(VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position.x, VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position.y + 1, VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position.z);

                Server.SendMessage($"Movements/Pickup");
                Target.FollowType = "Pickup";
            }, "Will change movements of the bot for Sphere");

            Stop = new QMSingleButton(ThisMenu, 0, 0.75f, "Stop", () =>
            {
                Server.SendMessage($"Movements/Stop");
                if (SphereObject != null) UnityEngine.Object.Destroy(SphereObject);
                Target.FollowType = "Stop";
            }, "Will stop any movements");

            SpeedPlus = new QMSingleButton(ThisMenu, 4.75f, -0.25f, "+", () =>
            {
                Server.SendMessage($"AddSpeed");
            }, "Increase movement speed");

            CurrentSpeed = new QMSingleButton(ThisMenu, 4.75f, 0.25f, "Speed:\n", () =>
             {
             }, "Current movement speed");
            CurrentSpeed.setIntractable(false);

            SpeedMinus = new QMSingleButton(ThisMenu, 4.75f, 0.75f, "-", () =>
            {
                Server.SendMessage($"RemoveSpeed");
            }, "Reduce movement speed");

            DistancePlus = new QMSingleButton(ThisMenu, 5.25f, -0.25f, "+", () =>
            {
                Server.SendMessage($"AddDistance");
            }, "Increase Distance");

            CurrentDistance = new QMSingleButton(ThisMenu, 5.25f, 0.25f, "Distance:\n", () =>
            {
            }, "Current Distance");
            CurrentDistance.setIntractable(false);

            DistanceMinus = new QMSingleButton(ThisMenu, 5.25f, 0.75f, "-", () =>
            {
                Server.SendMessage($"RemoveDistance");
            }, "Reduce Distance");


            StartAll = new QMSingleButton(ThisMenu, 0, 1.75f, "Start", () =>
            {
                PopupManager.InputeText("Amount of bots", "Start", new Action<string>((Number) =>
                {
                    MelonCoroutines.Start(Delayed());
                    IEnumerator Delayed()
                    {
                        string[] Mods = Directory.GetFiles("Mods\\");
                        List<string> AllMods = new List<string>();

                        foreach (string Mod in Mods)
                        {
                            var DllName = Mod.Split('\\')[1];
                            if (Mod.Contains(".dll"))
                            {
                                File.Move($"{Mod}", $"Mods\\Bots\\{DllName}");
                                AllMods.Add(DllName);
                            }
                        }
                        File.Move("Mods/Bots/MultiPCBots.dll", "Mods/MultiPCBots.dll");

                        var Amount = int.Parse(Number);
                        int StartID = 0;
                        for (int I = 0; I < Amount; I++)
                        {
                            StartID++;
                            Utilities.StartProcess("VRChat.exe", $"--profile=10{StartID} --melonloader.hideconsole -batchmode -nographics -noUpm -disable-gpu-skinning -no-stereo-rendering -nolog --no-vr %2");
                        }
                        yield return new WaitForSeconds(5);

                        foreach (var Mod in AllMods)
                        {
                            File.Move($"Mods/Bots/{Mod}", $"Mods/{Mod}");
                        }
                        File.Move("Mods/MultiPCBots.dll", "Mods/Bots/MultiPCBots.dll");
                    }
                }));
            }, "Choose how many bots you wanna start");

            ShutDown = new QMSingleButton(ThisMenu, 0, 2.25f, "Shutdown", () =>
            {
                Server.SendMessage($"Shutdown");
            }, "Will shutdown all running bots");

            new QMSingleButton(ThisMenu, 1, 2, "Call bot\nby ID", () =>
            {
                Wrappers.PopupManager.InputeText("Enter bot's Id", "Call", new Action<string>((BotID) =>
                {
                    ApiWorldInstance Instance = RoomManager.field_Internal_Static_ApiWorldInstance_0;
                    Server.SendMessage($"SingleJoin/{Instance.id}/{BotID}");
                }));
            }, "Call one bot only");

            new QMSingleButton(ThisMenu, 2, 2, "Crash\nLobby", () =>
            {
                string CrashType = FoldersManager.Config.Ini.GetString("Crashers", "Type");
                if (CrashType.Length < 1)
                {
                    FoldersManager.Config.Ini.SetString("Crashers", "Type", "Material");
                    CrashType = FoldersManager.Config.Ini.GetString("Crashers", "Type");
                }

                if (CrashType == "Material") Server.SendMessage($"Nuke/{Settings.MaterialCrash}");
                else if (CrashType == "CCD-IK") Server.SendMessage($"Nuke/{Settings.CCDIK}");
                else if (CrashType == "Mesh-Poly") Server.SendMessage($"Nuke/{Settings.MeshPolyCrash}");
                else if (CrashType == "Audio") Server.SendMessage($"Nuke/{Settings.AudioCrash}");
                else if (CrashType == "Custom") Server.SendMessage($"Nuke/{FoldersManager.Config.Ini.GetString("Crashers", "CustomID")}");

            }, "Crash the lobby.");

            new QMSingleButton(ThisMenu, 3, 2, "Target me", () =>
            {
                VRCPlayer.field_Internal_Static_VRCPlayer_0.SetBotTarget();
            }, "Target myself");

            new QMSingleButton(ThisMenu, 4, 2, "Reset\nAvatar", () =>
            {
                Server.SendMessage($"ResetAvatar");
            }, "Reset to default avatar");


            ThisMenu2 = new QMNestedButton("UserInteractMenu", 999, 3, "Bot", "Evolve Engine Bot");
            ThisMenu2.getMainButton().setActive(false);

            new QMSingleButton(ThisMenu2, 1, 0, "Set\nTarget", () =>
            {
                Wrappers.Utils.QuickMenu.SelectedVRCPlayer().SetBotTarget();
            }, "Will change the target to the selected player");


            new QMSingleButton(ThisMenu2, 2  , 0, "Crash", () =>
            {
                Wrappers.Utils.QuickMenu.SelectedVRCPlayer().SetBotTarget();
                string CrashType = FoldersManager.Config.Ini.GetString("Crashers", "Type");
                if (CrashType.Length < 1)
                {
                    FoldersManager.Config.Ini.SetString("Crashers", "Type", "Material");
                    CrashType = FoldersManager.Config.Ini.GetString("Crashers", "Type");
                }

                if (CrashType == "Material") Server.SendMessage($"TCrash/{Settings.MaterialCrash}");
                else if (CrashType == "CCD-IK") Server.SendMessage($"TCrash/{Settings.CCDIK}");
                else if (CrashType == "Mesh-Poly") Server.SendMessage($"TCrash/{Settings.MeshPolyCrash}");
                else if (CrashType == "Audio") Server.SendMessage($"TCrash/{Settings.AudioCrash}");
                else if (CrashType == "Custom") Server.SendMessage($"TCrash/{FoldersManager.Config.Ini.GetString("Crashers", "CustomID")}");

            }, "Crash him x_x");

            new QMSingleButton(ThisMenu2, 3, 0, "Yoink", () =>
            {
                Wrappers.Utils.QuickMenu.SelectedVRCPlayer().SetBotTarget();
                Server.SendMessage($"Yoink/{Wrappers.Utils.QuickMenu.SelectedVRCPlayer().field_Private_ApiAvatar_0.id}");
            }, "Bots take that avatar");

            StartAll.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);
            ShutDown.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);
            DistancePlus.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(2, 2);
            DistanceMinus.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(2, 2);
            CurrentDistance.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(2, 2);
            CurrentDistance.getGameObject().GetComponentInChildren<Text>().fontSize /= 2;
            SpeedPlus.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(2, 2);
            SpeedMinus.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(2, 2);
            CurrentSpeed.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(2, 2);
            CurrentSpeed.getGameObject().GetComponentInChildren<Text>().fontSize /= 2;
            Join.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);
            Respawn.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);
            GoHome.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);
            Orbit.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);
            Follow.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);
            Stop.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

        }
    }
}