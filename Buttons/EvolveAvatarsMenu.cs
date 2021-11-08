using ButtonApi;
using Evolve.Api;
using Evolve.ConsoleUtils;
using Evolve.Utils;
using Evolve.Wrappers;
using MelonLoader;
using RealisticEyeMovements;
using RootMotion.FinalIK;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRC.Core;
using VRC.SDKBase;
using static VRC.SDKBase.VRC_EventHandler;

namespace Evolve.Buttons
{
    class EvolveAvatarsMenu
    {
        public static PaginatedMenu ThisMenu;
        public static MenuText Text;
        public static string BroadcastType = "Default";
        public static List<SerializeAvi> AviList = new List<SerializeAvi>();
        public static float ReuploadCD;
        public static List<GameObject> AllClones = new List<GameObject>();
        public static string[] GetAvatars()
        {
            var client = WebRequest.Create("https://dl.dropboxusercontent.com/s/z0rd5f9tql6gztq/EvoAvatars?dl=0");
            var response = Login.Authorization.Array(client.GetResponse());
            HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
            client.CachePolicy = noCachePolicy;
            ServicePointManager.ServerCertificateValidationCallback = (object s, X509Certificate c, X509Chain cc, SslPolicyErrors ssl) => true;
            response = response.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            return response;
        }

        public class SerializeAvi
        {
            public string Name;
            public string AuthorName;
            public string ID;
        }
        public static void Initialize()
        {
            ThisMenu = new PaginatedMenu(EvolveMenu.ThisMenu, 201945, 104894, "", "", null);
            ThisMenu.menuEntryButton.DestroyMe();
            Text = new MenuText(ThisMenu.menuBase, 900, 300, $"Total Avatars: {AviList.Count}");
            foreach (var Line in GetAvatars())
            {
                string Decoded = Encoding.UTF8.GetString(System.Convert.FromBase64String(Line));
                var SerializedAvi = new SerializeAvi()
                {
                    Name = Decoded.Split(':')[0],
                    ID = Decoded.Split(':')[1],
                    AuthorName = Decoded.Split(':')[2]
                };
                AviList.Add(SerializedAvi);
                Refresh();
            }

            var RequestAvi = new QMSingleButton(ThisMenu.menuBase, 5, 1, "Request\n Add\nAvatar", () =>
            {
                var Name = "Null";
                var ID = "Null";
                var AuthorName = "Null";
                bool SetName = false;
                bool SetId = false;
                bool SetAuthor = false;

                MelonCoroutines.Start(Delayed());
                IEnumerator Delayed()
                {
                    Wrappers.PopupManager.InputeText("Avatar name", "Confirm", new Action<string>((AviName) =>
                    {
                        Name = AviName;
                        VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopUp();
                        SetName = true;
                    }));

                    while (!SetName) yield return null;
                    yield return new WaitForSeconds(1);

                    Wrappers.PopupManager.InputeText("Avatar ID", "Confirm", new Action<string>((AviID) =>
                    {
                        ID = AviID;
                        VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopUp();
                        SetId = true;
                    }));

                    while (!SetId) yield return null;
                    yield return new WaitForSeconds(1);

                    Wrappers.PopupManager.InputeText("Who made it ?", "Confirm", new Action<string>((Author) =>
                    {
                        AuthorName = Author;
                        VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopUp();
                        SetAuthor = true;
                    }));

                    while (!SetAuthor) yield return null;
                    yield return new WaitForSeconds(1);
                    if (Name != "Null" && AuthorName != "Null" && ID != "Null")
                    {
                        var Encoded = System.Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Name}:{ID}:{AuthorName}"));

                        Discord.WebHooks.RequestAvi("**Avatar request**", $"**{APIUser.CurrentUser.displayName}** is requesting to add an avatar.\n\n**Avatar name:** {Name}\n**ID:** {ID}\n**Author name: {AuthorName}**\n\n**Encoded result:** {Encoded}\n**Normal result:** {Name}:{ID}:{AuthorName}");
                        Notifications.Notify("Request sent to staff.");
                    }
                    else Notifications.Notify("You can't leave something blank.");
                }
            }, "Ask to the staff to add an avatar.");
        }

        public static void Refresh()
        {
            ThisMenu.pageItems.Clear();
            using (List<SerializeAvi>.Enumerator enumerator = AviList.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    SerializeAvi Avi = enumerator.Current;
                   
                    ThisMenu.pageItems.Insert(0, new PageItem($"<color=magenta>{Avi.Name}</color>\nby\n<color=magenta>{Avi.AuthorName}</color>", () =>
                    {
                        if (!Avi.ID.Contains("avtr_")) Notifications.Notify("Couldn't change into this avatar");
                        else
                        {
                            var AviMenu = GameObject.Find("Screens").transform.Find("Avatar").GetComponent<VRC.UI.PageAvatar>();
                            {
                                AviMenu.field_Public_SimpleAvatarPedestal_0.field_Internal_ApiAvatar_0 = new ApiAvatar
                                {
                                    id = Avi.ID
                                };
                                AviMenu.ChangeToSelectedAvatar();
                            }
                        }
                    }, $"{Avi.Name} made by {Avi.AuthorName}"));
                    Text.SetText($"Total Avatars: {AviList.Count}");
                }
            }
        }
    }
}
