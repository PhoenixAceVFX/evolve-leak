using Evolve.ConsoleUtils;
using Evolve.Utils;
using Harmony;
using MelonLoader;
using Newtonsoft.Json;
using System;
using System.Collections;
using UnityEngine;
using VRC.Core;
using WebSocketSharp;

namespace Evolve.ApiLogs
{
    internal class ApiLog
    {
        internal static WebSocket ws;
        private static WebSocketContent WebSocketData;
        private static WebSocketObject WebSocketRawData;
        public static string Cache = "";

        public static void Start()
        {
            MelonCoroutines.Start(Connect());
            MelonCoroutines.Start(GetLog());
        }

        public static string[] Protocols = new string[]
        {
            "User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36 OPR/77.0.4054.257"
        };

        private static IEnumerator Connect()
        {
            while (APIUser.CurrentUser == null) yield return null;
            while (string.IsNullOrWhiteSpace(ApiCredentials.authToken)) yield return new WaitForSeconds(0.2f);
            ws = new WebSocket("wss://pipeline.vrchat.cloud/?authToken=" + ApiCredentials.authToken);
            ws.OnMessage += Message;
            ws.OnClose += Close;
            ws.Connect();
        }

        private static void Close(object sender, CloseEventArgs e)
        {
            try
            {
                if (Settings.ApiLogs)
                {
                    EvoVrConsole.Log(EvoVrConsole.LogsType.Api, "Disconnected...");
                    EvoVrConsole.Log(EvoVrConsole.LogsType.Api, "Reconnecting in 15 seconds!");
                }
                Utilities.DelayFunction(15f, () => ws.Connect());
            }
            catch { }
        }

        private static void Message(object sender, MessageEventArgs e)
        {
            if (Settings.ApiLogs)
            {
                try
                {
                    WebSocketRawData = JsonConvert.DeserializeObject<WebSocketObject>(e.Data);
                    WebSocketData = JsonConvert.DeserializeObject<WebSocketContent>(WebSocketRawData.content);
                    string Message = WebSocketRawData.type;
                    if (Message != null) Cache = Message;
                }
                catch { }
            }
        }

        private static IEnumerator GetLog()
        {
            while (true)
            {
                if (Cache != "")
                {
                    try
                    {
                        var User = WebSocketData.user;
                        var World = WebSocketData.world;

                        if (Cache == "friend-location")
                        {
                            if (World.name.Length > 1) EvoVrConsole.Log(EvoVrConsole.LogsType.Api, $"<color=magenta>{User.displayName}</color> went to <color=magenta>{World.name}</color>");
                            else EvoVrConsole.Log(EvoVrConsole.LogsType.Api, $"<color=magenta>{User.displayName}</color> went to a private world.");
                            Cache = "";
                        }
                        else if (Cache == "friend-online")
                        {
                            APIUser.FetchUser(WebSocketData.userId, new Action<APIUser>(user =>
                            {
                                EvoVrConsole.Log(EvoVrConsole.LogsType.Api, $"<color=magenta>{user.displayName}</color> is now <color=magenta>Online</color>");
                            }), null);
                            Cache = "";
                        }
                        else if (Cache == "friend-offline")
                        {
                            APIUser.FetchUser(WebSocketData.userId, new Action<APIUser>(user =>
                            {
                                EvoVrConsole.Log(EvoVrConsole.LogsType.Api, $"<color=magenta>{user.displayName}</color> is now <color=magenta>Offline</color>");
                            }), null);
                            Cache = "";
                        }
                        else if (Cache == "friend-add")
                        {
                            EvoVrConsole.Log(EvoVrConsole.LogsType.Api, $"<color=magenta>{User.displayName}</color> is now your <color=magenta>friend</color>");
                            Cache = "";
                        }
                        else if (Cache == "friend-delete")
                        {
                            EvoVrConsole.Log(EvoVrConsole.LogsType.Api, $"<color=magenta>{User.displayName}</color> is no longer your <color=magenta>friend</color>");
                            Cache = "";
                        }
                        else if (Cache == "friend-active")
                        {
                            EvoVrConsole.Log(EvoVrConsole.LogsType.Api, $"<color=magenta>{User.displayName}</color> is connected on the website.");
                            Cache = "";
                        }
                    }
                    catch { }
                }
                yield return new WaitForEndOfFrame();
            }
        }
    }
}