using Evolve.ConsoleUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Evolve.Bot
{
    internal class Handler : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs Message)
        {
            try
            {
                var LogType = Message.Data.Split('/')[0];
                var Log = Message.Data.Split('/')[1];
                switch (LogType)
                {
                    case "Log":
                        EvoConsole.Log(Log);
                        break;
                    case "Warn":
                        EvoConsole.LogWarn(Log);
                        break;
                    case "Success":
                        EvoConsole.LogSuccess(Log);
                        break;
                    case "Error":
                        EvoConsole.LogError(Log);
                        break;
                }
            }
            catch { }
        }
        protected override void OnError(ErrorEventArgs e)
        {
            EvoConsole.Log(e.Message);
        }
        protected override void OnOpen()
        {
            EvoConsole.Log("A bot connected to the server.");
        }

        protected override void OnClose(CloseEventArgs e)
        {
            EvoConsole.Log($"A bot disconnected from the server: {e.Reason}");
        }

        public static IEnumerator MessageHandler(string Message)
        {
            EvoVrConsole.Log(EvoVrConsole.LogsType.Bot, Message);
            yield return null;
        }
    }
    class Server
    {
        public static WebSocketServer WSServer;
        public static void Initialize()
        {
            //Normals
            
            WSServer = new WebSocketServer("ws://localhost:9999");
            WSServer.AddWebSocketService<Handler>("/Bot");
            EvoConsole.LogSuccess("Starting bots server.");
            WSServer.Start();
        }

        public static void SendMessage(string Message)
        {
            WSServer.WebSocketServices.Broadcast(Message);
        }

        public static void SendVector3(Vector3 Position)
        {
            SendMessage($"TargetPosition/{Position.x},{Position.y},{Position.z}");
        }
    }
}
