using ButtonApi;
using Evolve.Api;
using Evolve.Utils;
using Evolve.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRC;
using VRC.SDKBase;
using VRC.Udon;
using static VRC.SDKBase.VRC_EventHandler;

namespace Evolve.Buttons
{
    class UdonRPCMenu
    {
        public static PaginatedMenu ThisMenu;
        public static MenuText Text2;
#pragma warning disable CS0649 // Le champ 'UdonRPCMenu.Text' n'est jamais assigné et aura toujours sa valeur par défaut null
        public static MenuText Text;
#pragma warning restore CS0649 // Le champ 'UdonRPCMenu.Text' n'est jamais assigné et aura toujours sa valeur par défaut null
#pragma warning disable CS0649 // Le champ 'UdonRPCMenu.Button1' n'est jamais assigné et aura toujours sa valeur par défaut null
        public static QMSingleButton Button1;
#pragma warning restore CS0649 // Le champ 'UdonRPCMenu.Button1' n'est jamais assigné et aura toujours sa valeur par défaut null
#pragma warning disable CS0649 // Le champ 'UdonRPCMenu.Button2' n'est jamais assigné et aura toujours sa valeur par défaut null
        public static QMSingleButton Button2;
#pragma warning restore CS0649 // Le champ 'UdonRPCMenu.Button2' n'est jamais assigné et aura toujours sa valeur par défaut null
#pragma warning disable CS0649 // Le champ 'UdonRPCMenu.Button3' n'est jamais assigné et aura toujours sa valeur par défaut null
        public static QMSingleButton Button3;
#pragma warning restore CS0649 // Le champ 'UdonRPCMenu.Button3' n'est jamais assigné et aura toujours sa valeur par défaut null
#pragma warning disable CS0649 // Le champ 'UdonRPCMenu.Button4' n'est jamais assigné et aura toujours sa valeur par défaut null
        public static QMSingleButton Button4;
#pragma warning restore CS0649 // Le champ 'UdonRPCMenu.Button4' n'est jamais assigné et aura toujours sa valeur par défaut null
        public static string BroadcastType = "Default";
        public static List<SerializeUdonRPC> UdonRPCList = new List<SerializeUdonRPC>();

        public class SerializeUdonRPC
        {
            public string EventName;
            public Player Player;
        }
        public static void Initialize()
        {
            ThisMenu = new PaginatedMenu(EvolveMenu.ThisMenu, 201945, 104894, "Udon RPC", "", null);
            ThisMenu.menuEntryButton.DestroyMe();
            Text2 = new MenuText(ThisMenu.menuBase, 900, 300, $"Total Udon RPC: {UdonRPCList.Count}");

            new QMToggleButton(ThisMenu.menuBase, 5, 1, "Enabled", () =>
            {
                Settings.UdonRPCList = true;
                FoldersManager.Config.Ini.SetBool("UdonRPCList", "Enabled", true);
            }, "Disabled", () =>
            {
                Settings.UdonRPCList = false;
                FoldersManager.Config.Ini.SetBool("UdonRPCList", "Enabled", false);
            }, "Enable or disable the udon rpc list", null, null, false, Settings.UdonRPCList);
        }

        public static void Refresh()
        {
            ThisMenu.pageItems.Clear();
            using (List<SerializeUdonRPC>.Enumerator enumerator = UdonRPCList.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    SerializeUdonRPC RPC = enumerator.Current;
                    ThisMenu.pageItems.Insert(0, new PageItem($"{RPC.EventName}\n{RPC.Player.DisplayName()}\n<color=magenta>(Send)</color>", () =>
                    {
                        foreach (var Behaviour in Exploits.Exploits.Behaviours)
                        {
                            foreach (Il2CppSystem.Collections.Generic.KeyValuePair<string, Il2CppSystem.Collections.Generic.List<uint>> keyValuePair in Behaviour._eventTable)
                            {
                                if (keyValuePair.key == RPC.EventName)
                                {
                                    Behaviour.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, RPC.EventName);
                                }
                            }
                        }
                    }, $"Name: {RPC.EventName}, Player: {RPC.Player.DisplayName()}"));
                    Text2.SetText($"Total Udon RPC: {UdonRPCList.Count}");
                }
            }
        }
    }
}
