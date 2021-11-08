using Evolve.ConsoleUtils;
using Evolve.Utils;
using ButtonApi;
using UnityEngine;
using System;
using Evolve.ApiLogs;

namespace Evolve.Buttons
{
    internal class LogsMenu
    {
        public static QMNestedButton ThisMenu;
        public static QMToggleButton Logger;

        public static void Initialize()
        {
            ThisMenu = new QMNestedButton(EvolveMenu.ThisMenu, 1.5f, 1.75f, "Logs", "Logs Menu");
            ThisMenu.getMainButton().getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector3(1, 2);

            new QMToggleButton(ThisMenu, 1, 0, "Event\nReceived", () =>
            {
                Settings.EventLog = true;
                FoldersManager.Config.Ini.SetBool("Console", "EventLogs", true);
            }, "Disabled", () =>
            {
                Settings.EventLog = false;
                FoldersManager.Config.Ini.SetBool("Console", "EventLogs", false);
            }, "Enable Event logs in the console", null, null, false, Settings.EventLog);

            new QMToggleButton(ThisMenu, 3, 0, "RPC", () =>
            {
                Settings.RPCEventsLogs = true;
                FoldersManager.Config.Ini.SetBool("Console", "RPCEventLogs", true);
            }, "Disabled", () =>
            {
                Settings.RPCEventsLogs = false;
                FoldersManager.Config.Ini.SetBool("Console", "RPCEventLogs", false);
            }, "Every rpc sent by others and you", null, null, false, Settings.RPCEventsLogs);

            new QMToggleButton(ThisMenu, 2, 0, "Event\nSent", () =>
            {
                FoldersManager.Config.Ini.SetBool("Console", "MyEventLogs", true);
                Settings.MyEventsLog = true;
            }, "Disabled", () =>
            {
                FoldersManager.Config.Ini.SetBool("Console", "MyEventLogs", false);
                Settings.MyEventsLog = false;
            }, "The events you'r sending", null, null, false, Settings.MyEventsLog);

            new QMToggleButton(ThisMenu, 4, 0, "Avatar", () =>
            {
                FoldersManager.Config.Ini.SetBool("Console", "AvatarLogs", true);
                Settings.AvatarLogs = true;
            }, "Disabled", () =>
            {
                FoldersManager.Config.Ini.SetBool("Console", "AvatarLogs", false);
                Settings.AvatarLogs = false;
            }, "Anything about avatars", null, null, false, Settings.AvatarLogs);

            new QMToggleButton(ThisMenu, 1, 1, "Udon RPC", () =>
            {
                FoldersManager.Config.Ini.SetBool("Console", "UdonLogs", true);
                Settings.UdonRPCLogs = true;
            }, "Disabled", () =>
            {
                FoldersManager.Config.Ini.SetBool("Console", "UdonLogs", false);
                Settings.UdonRPCLogs = false;
            }, "Every udon rpc sent by others and you", null, null, false, Settings.UdonRPCLogs);

            new QMSingleButton(ThisMenu, 1, 2, "Clear\nLogs", () =>
            {
                for (int i = 0; i < 20; i++)
                {
                    EvoVrConsole.Log(EvoVrConsole.LogsType.Empty, "");
                }
                Console.Clear();
            }, "Clear logs");


            new QMToggleButton(ThisMenu, 2, 1, "Crashers", () =>
            {
                FoldersManager.Config.Ini.SetBool("Console", "Crashers", true);
                Settings.CrashLogs = true;
            }, "Disabled", () =>
            {
                FoldersManager.Config.Ini.SetBool("Console", "Crashers", false);
                Settings.CrashLogs = false;
            }, "Will warn you when someone crash the world", null, null, false, Settings.CrashLogs);

            new QMToggleButton(ThisMenu, 3, 1, "Moderation", () =>
            {
                FoldersManager.Config.Ini.SetBool("Console", "Moderation", true);
                Settings.ModerationLogs = true;
            }, "Disabled", () =>
            {
                FoldersManager.Config.Ini.SetBool("Console", "Moderation", false);
                Settings.ModerationLogs = false;
            }, "Will warn you when someone send moderation events on you", null, null, false, Settings.ModerationLogs);

            Logger = new QMToggleButton(ThisMenu, 4, 2, "Logger", () =>
            {
                FoldersManager.Config.Ini.SetBool("Console", "Logger", true);
                Settings.LoggerEnable = true;
            }, "Disabled", () =>
            {
                FoldersManager.Config.Ini.SetBool("Console", "Logger", false);
                Settings.LoggerEnable = false;
            }, "Dev Logger", null, null, false, Settings.LoggerEnable);
            Logger.setActive(false);

            new QMToggleButton(ThisMenu, 4, 1, "API", () =>
            {
                FoldersManager.Config.Ini.SetBool("Console", "ApiLogs", true);
                Settings.ApiLogs = true;
                if (!ApiLog.ws.IsAlive) ApiLog.ws.Connect();
            }, "Disabled", () =>
            {
                FoldersManager.Config.Ini.SetBool("Console", "ApiLogs", false);
                Settings.ApiLogs = false;
            }, "Will make a log in the console for any messages received from the api", null, null, false, Settings.ApiLogs);
        }
    }
}