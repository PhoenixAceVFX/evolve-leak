using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ButtonApi;
using Evolve.Utils;
using Evolve.Wrappers;
using MelonLoader;
using UnityEngine;
using WebSocketSharp;

namespace Evolve.Bot
{
    class PhotonBots
    {
        public static QMNestedButton ThisMenu;
        public static WebSocket WSClient;
        public static void SendMessage(string Message)
        {
            WSClient.Send(Message);
        }
        public static void Connect()
        {
            WSClient = new WebSocket("ws://localhost:999/Bots");
            WSClient.Log.Level = LogLevel.Error;
            WSClient.Connect();
        }
        public static void Initialize()
        {
            ThisMenu = new QMNestedButton(BotMenu.ThisMenu, 5, 1.25f, "Photon", "Photon Bots");
            ThisMenu.getMainButton().getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            new QMToggleButton(ThisMenu, 0, 0, "Start", () =>
            {
                PopupManager.InputeText("Amount of bots", "Start", new Action<string>((Number) =>
                {
                    Discord.WebHooks.SendBot($">Photon start {Number}");
                }));
            }, "Stop", () =>
            {
                Discord.WebHooks.SendBot($">Photon stop");
            }, "Start or stop the bots");

            new QMToggleButton(ThisMenu, 0, 1, "Send 7", () =>
            {
                Settings.Send7 = true;
            }, "Stop", () =>
            {
                Settings.Send7 = false;
            }, "Send movement events");

            new QMSingleButton(ThisMenu, 1, 0, "Join", () =>
            {
                var WorldID = RoomManager.field_Internal_Static_ApiWorldInstance_0.id;
                Discord.WebHooks.SendBot($">Photon join {WorldID}");
            }, "Join me");

            new QMSingleButton(ThisMenu, 2, 0, "Join ID", () =>
            {
                PopupManager.InputeText("Amount of bots", "Start", new Action<string>((WorldID) =>
                {
                    Discord.WebHooks.SendBot($">Photon join {WorldID}");
                }));
            }, "Will make all bots join that room");

            new QMSingleButton(ThisMenu, 3, 0, "Instantiate", () =>
            {
                Discord.WebHooks.SendBot($">Photon instantiate");
            }, "Will instantiate all bots");

            new QMSingleButton(ThisMenu, 4, 0, "Players\nList", () =>
            {
                Discord.WebHooks.SendBot($">Photon playerslist");
            }, "Will log all users in the room");

            new QMSingleButton(ThisMenu, 0, 2, "Sync to\nserver", () =>
            {
                Connect();
            }, "To reconnect to the server.");

            Connect();
        }
    }
}
