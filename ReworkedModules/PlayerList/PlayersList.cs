using Evolve.Api;
using Evolve.Buttons;
using Evolve.Utils;
using Evolve.Wrappers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRC;
using VRC.Core;

namespace Evolve.Modules.PlayersList
{
    internal class PlayersList
    {
        public static List<string> Lines = new List<string>();
        public static List<string> LinesSM = new List<string>();
        public static List<MenuText> Logs = new List<MenuText>();
        public static List<MenuText> LogsSM = new List<MenuText>();
        public static MenuText PlayerCount;
        public static MenuText PlayerCountSM;
        public static float posx = 450;
        public static float posxSM = 450;
        public static float posy = 250;
        public static float posySM = 250;
        public static IEnumerator Initialize()
        {
            while (EvolveMenu.PlayersList == null) yield return null;
            while (LobbyMenu.PlayerList == null) yield return null;

            float num = posy;
            float num2 = posySM;
            PlayerCount = new MenuText(LobbyMenu.PlayerList.getGameObject().transform, posx + 300, 140, "");
            PlayerCountSM = new MenuText(EvolveMenu.PlayersList.getGameObject().transform, posx + 300, 140, "");

            for (int i = 0; i <= 21; i++)
            {
                MenuText item = new MenuText(LobbyMenu.PlayerList.getGameObject().transform, posx, posy, "");
                Logs.Add(item);
                posy += 70f;
            }

            for (int i = 0; i <= 21; i++)
            {
                MenuText item = new MenuText(EvolveMenu.PlayersList.getGameObject().transform, posxSM, posySM, "");
                LogsSM.Add(item);
                posySM += 70f;
            }

            posySM = num2;
            posy = num;
        }

        public static IEnumerator UpdatePlayerList()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.8f);
                if (LobbyMenu.ThisMenu.getBackButton().getGameObject().active == false) yield return null;
                Lines.Clear();
                LinesSM.Clear();
                try
                {
                    foreach (Player Player in Wrappers.Utils.PlayerManager.AllPlayers().ToArray())
                    {
                        string Text = "";
                        string Rank = Player.GetAPIUser().GetRank().ToLower();
                        string VrOrPc = "<color=#a6a6a6>[PC]</color>";
                        string Bot = "";
                        string Master = "";
                        string Evolve = "";
                        if (Player.GetVRCPlayer().GetIsInVR()) VrOrPc = "<color=#6f00ff>[VR]</color>";
                        if (Player.GetVRCPlayer().GetIsQuest()) VrOrPc = "<color=#1eff00>[Q]</color>";
                        if (Player.GetVRCPlayer().GetIsBot()) Bot = "<color=#00ffe1>[BOT]</color>";
                        if (Player._vrcplayer.IsEvolved()) Evolve = "<color=#fc035e>[EVO]</color>";
                        if (Player.GetIsMaster()) Master = "<color=#f2d600>[M]</color>";

                        if (!APIUser.IsFriendsWith(Player.UserID()))
                        {
                            switch (Rank)
                            {
                                case "user":
                                    Text = $"<color=#{ConversionManager.User}>[U]</color> {Bot} {Evolve} {Master} {VrOrPc} {Player.DisplayName()} <color=#c9c9c9>[P]</color> {Player.GetVRCPlayer().GetPingColored()} <color=#c9c9c9>[F]</color> {Player.GetVRCPlayer().GetFramesColored()}";
                                    break;
                                case "legend":
                                    Text = $"<color=#{ConversionManager.Legend}>[L]</color> {Bot} {Evolve} {Master} {VrOrPc} {Player.DisplayName()} <color=#c9c9c9>[P]</color> {Player.GetVRCPlayer().GetPingColored()} <color=#c9c9c9>[F]</color> {Player.GetVRCPlayer().GetFramesColored()}";
                                    break;
                                case "known":
                                    Text = $"<color=#{ConversionManager.Known}>[K]</color> {Bot} {Evolve} {Master} {VrOrPc} {Player.DisplayName()} <color=#c9c9c9>[P]</color> {Player.GetVRCPlayer().GetPingColored()} <color=#c9c9c9>[F]</color> {Player.GetVRCPlayer().GetFramesColored()}";
                                    break;
                                case "negativetrust":
                                    Text = $"<color=#{ConversionManager.Nuisance}>[NS]</color> {Bot} {Evolve} {Master} {VrOrPc} {Player.DisplayName()} <color=#c9c9c9>[P]</color> {Player.GetVRCPlayer().GetPingColored()} <color=#c9c9c9>[F]</color> {Player.GetVRCPlayer().GetFramesColored()}";
                                    break;
                                case "new user":
                                    Text = $"<color=#{ConversionManager.NewUser}>[N]</color> {Bot} {Evolve} {Master} {VrOrPc} {Player.DisplayName()} <color=#c9c9c9>[P]</color> {Player.GetVRCPlayer().GetPingColored()} <color=#c9c9c9>[F]</color> {Player.GetVRCPlayer().GetFramesColored()}";
                                    break;
                                case "verynegativetrust":
                                    Text = $"<color=#{ConversionManager.Nuisance}>[NS]</color> {Bot} {Evolve} {Master} {VrOrPc} {Player.DisplayName()} <color=#c9c9c9>[P]</color> {Player.GetVRCPlayer().GetPingColored()} <color=#c9c9c9>[F]</color> {Player.GetVRCPlayer().GetFramesColored()}";
                                    break;
                                case "visitor":
                                    Text = $"<color=#{ConversionManager.Visitors}>[V]</color> {Bot} {Evolve} {Master} {VrOrPc} {Player.DisplayName()} <color=#c9c9c9>[P]</color> {Player.GetVRCPlayer().GetPingColored()} <color=#c9c9c9>[F]</color> {Player.GetVRCPlayer().GetFramesColored()}";
                                    break;
                                case "trusted":
                                    Text = $"<color=#{ConversionManager.Trusted}>[T]</color> {Bot} {Evolve} {Master} {VrOrPc} {Player.DisplayName()} <color=#c9c9c9>[P]</color> {Player.GetVRCPlayer().GetPingColored()} <color=#c9c9c9>[F]</color> {Player.GetVRCPlayer().GetFramesColored()}";
                                    break;
                                case "veteran":
                                    Text = $"<color=#ff08c1>[VT]</color> {Bot} {Evolve} {Master} {VrOrPc} {Player.DisplayName()} <color=#c9c9c9>[P]</color> {Player.GetVRCPlayer().GetPingColored()} <color=#c9c9c9>[F]</color> {Player.GetVRCPlayer().GetFramesColored()}";
                                    break;
                            }
                        }
                        else Text = $"<color=#{ConversionManager.isFriend}>[F]</color> {Bot} {Evolve} {Master} {VrOrPc} {Player.DisplayName()} <color=#c9c9c9>[P]</color> {Player.GetVRCPlayer().GetPingColored()} <color=#c9c9c9>[F]</color> {Player.GetVRCPlayer().GetFramesColored()}";

                        Lines.Insert(0, Text);
                        LinesSM.Insert(0, Text);
                        UpdateText();
                        PlayerCount.SetText($"<color=#5500ff><b>      Players list\n</b></color><color=#ff0055>{LinesSM.Count} </color><color=#5500ff>in current room</color>");
                        PlayerCountSM.SetText($"<color=#5500ff><b>      Players list\n</b></color><color=#ff0055>{LinesSM.Count} </color><color=#5500ff>in current room</color>");
                    }
                }
                catch { }
            }
        }

        public static void UpdateText()
        {
            try
            {
                if (RoomManager.field_Internal_Static_ApiWorldInstance_0 != null)
                {
                    for (int i = 0; i <= Logs.Count; i++)
                    {
                        try
                        {
                            if (Lines[i] != null) Logs[i].SetText(Lines[i]);
                            if (LinesSM[i] != null) LogsSM[i].SetText(LinesSM[i]);

                        }
                        catch
                        {
                            Logs[i].SetText("");
                            LogsSM[i].SetText("");

                        }
                    }
                }
            }
            catch { }
        }
    }
}