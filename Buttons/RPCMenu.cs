using ButtonApi;
using Evolve.Api;
using Evolve.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRC.SDKBase;
using static VRC.SDKBase.VRC_EventHandler;

namespace Evolve.Buttons
{
    class RPCMenu
    {
        public static PaginatedMenu ThisMenu;
        public static MenuText Text2;
        public static MenuText Text;
        public static QMSingleButton Button1;
        public static QMSingleButton Button2;
        public static QMSingleButton Button3;
        public static QMSingleButton Button4;
        public static string BroadcastType = "Default";
        public static List<SerializeRPC> RPCList = new List<SerializeRPC>();

        public class SerializeRPC
        {
            public VrcEventType EventType;
            public string Name;
            public GameObject ParameterObject;
            public int ParameterInt;
            public float ParameterFloat;
            public string ParameterString;
            public VrcBooleanOp ParameterBoolOp;
            public VrcBroadcastType Broadcast;
        }
        public static void Initialize()
        {
            ThisMenu = new PaginatedMenu(EvolveMenu.ThisMenu, 201945, 104894, "RPC", "", null);
            ThisMenu.menuEntryButton.DestroyMe();
            Text = new MenuText(ThisMenu.menuBase, 900, 380, $"Broadcast type: {BroadcastType}");
            Text2 = new MenuText(ThisMenu.menuBase, 900, 300, $"Total RPC: {RPCList.Count}");
            Panels.PanelMenu(ThisMenu.menuBase, 0, 0.3f, "\nBroadcast Type:", 1.1f, 2.4F, "Choose the broadcast type");

            new QMToggleButton(ThisMenu.menuBase, 5, 1, "Enabled", () =>
            {
                Settings.RPCList = true;
                FoldersManager.Config.Ini.SetBool("RPCList", "Enabled", true);
            }, "Disabled", () =>
            {
                Settings.RPCList = false;
                FoldersManager.Config.Ini.SetBool("RPCList", "Enabled", false);
            }, "Enable or disable the rpc list", null, null, false, Settings.RPCList);

            Button1 = new QMSingleButton(ThisMenu.menuBase, 0, -0.25f, "Default", () =>
            {
                BroadcastType = "Default";
                Text.SetText($"Broadcast type: {BroadcastType}");
            },"Default: Sending the exact same RPC");

            Button2 = new QMSingleButton(ThisMenu.menuBase, 0, 0.25f, "Local", () =>
            {
                BroadcastType = "Local";
                Text.SetText($"Broadcast type: {BroadcastType}");
            }, "Local: Sending the RPC only for yourself");

            Button3 = new QMSingleButton(ThisMenu.menuBase, 0, 0.75f, "Always", () =>
            {
                BroadcastType = "Always";
                Text.SetText($"Broadcast type: {BroadcastType}");
            }, "Always: Sending the RPC to everyone");

            Button4 = new QMSingleButton(ThisMenu.menuBase, 0, 1.25f, "Owner", () =>
            {
                BroadcastType = "Owner";
                Text.SetText($"Broadcast type: {BroadcastType}");
            }, "Owner: Sending the RPC to the Master of the room");

            Button1.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);
            Button2.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);
            Button3.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);
            Button4.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);
        }

        public static void Refresh()
        {
            ThisMenu.pageItems.Clear();
            using (List<SerializeRPC>.Enumerator enumerator = RPCList.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    SerializeRPC RPC = enumerator.Current;
                    string Name = "";
                    string Object = "";
                    if (RPC.ParameterString != null)
                    {
                        if (RPC.ParameterString.Contains("Udon")) Name = "Udon";
                        else if (RPC.ParameterString.Contains("Teleport")) Name = "Teleport";
                        else if (RPC.ParameterString.ToLower().Contains("Voice")) Name = "Voice";
                        else if (RPC.ParameterString.ToLower().Contains("uspeak")) Name = "USpeak";
                        else if (RPC.ParameterString.Contains("BadConnection")) Name = "Bad Connection";
                        else Name = RPC.ParameterString;
                    }
                    if (RPC.ParameterObject != null)
                    {
                        Object = RPC.ParameterObject.name;
                        if (RPC.ParameterObject.name.Contains("R2V0SXNFdm9sdmVk")) return;
                    }
                    ThisMenu.pageItems.Insert(0, new PageItem($"{Name}\n{Object}\n<color=magenta>(Send)</color>", () =>
                    {
                        if (BroadcastType == "Default") Exploits.Exploits.SendRPC(RPC.EventType, RPC.Name, RPC.ParameterObject, RPC.ParameterInt, RPC.ParameterFloat, RPC.ParameterString, RPC.ParameterBoolOp, RPC.Broadcast);
                        else if (BroadcastType == "Local") Exploits.Exploits.SendRPC(RPC.EventType, RPC.Name, RPC.ParameterObject, RPC.ParameterInt, RPC.ParameterFloat, RPC.ParameterString, RPC.ParameterBoolOp, VrcBroadcastType.Local);
                        else if (BroadcastType == "Always") Exploits.Exploits.SendRPC(RPC.EventType, RPC.Name, RPC.ParameterObject, RPC.ParameterInt, RPC.ParameterFloat, RPC.ParameterString, RPC.ParameterBoolOp, VrcBroadcastType.Always);
                        else if (BroadcastType == "Owner") Exploits.Exploits.SendRPC(RPC.EventType, RPC.Name, RPC.ParameterObject, RPC.ParameterInt, RPC.ParameterFloat, RPC.ParameterString, RPC.ParameterBoolOp, VrcBroadcastType.Owner);
                    }, $"Name: {Name}, Object: {Object}, Broadcast Type: {RPC.Broadcast}"));
                    Text2.SetText($"Total RPC: {RPCList.Count}");
                }
            }
        }
    }
}
