using Evolve.Api;
using Evolve.ConsoleUtils;
using Evolve.Utils;
using Evolve.Wrappers;
using ButtonApi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;

namespace Evolve.Buttons
{
    public class SerializedWorld
    {
        public string WorldID;
        public string WorldName;
        public string WorldImageURL;
    }
    internal class InstanceHistoryMenu
    {
        public static PaginatedMenu baseMenu;
        private static QMSingleButton clearInstances;
        private static List<SerializedWorld> History;

        public static void Initialize()
        {
            baseMenu = new PaginatedMenu(EvolveMenu.ThisMenu, 201945, 104894, "Instance\nHistory", "", null);
            baseMenu.menuEntryButton.DestroyMe();
            clearInstances = new QMSingleButton(InstanceHistoryMenu.baseMenu.menuBase, 5, 1, "Clear", delegate ()
            {
                Wrappers.Utils.VRCUiPopupManager.AlertV2("Evolve Engine", "Are you sure you want to clear the world history?", "Yes", delegate ()
                {
                    InstanceHistoryMenu.History.Clear();
                    SaveInstances();
                    Refresh();
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopUp();
                }, "No", delegate ()
                {
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopUp();
                });
            }, "Clears the world history", null, null);
            if (!File.Exists($"{Directory.GetCurrentDirectory()}\\Evolve\\Worlds\\History.txt"))
            {
                File.WriteAllText($"{Directory.GetCurrentDirectory()}\\Evolve\\Worlds\\History.txt", JsonConvert.SerializeObject(InstanceHistoryMenu.History));
                return;
            }
            string History = File.ReadAllText($"{Directory.GetCurrentDirectory()}\\Evolve\\Worlds\\History.txt");
            InstanceHistoryMenu.History = JsonConvert.DeserializeObject<List<SerializedWorld>>(History);
        }

        public static void Refresh()
        {
            baseMenu.pageItems.Clear();
            using (List<SerializedWorld>.Enumerator enumerator = History.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    SerializedWorld pastInstance = enumerator.Current;
                    var Button = new PageItem(pastInstance.WorldName + "\n" + InstanceIDUtilities.GetInstanceID(pastInstance.WorldID.Split(':')[1]), () =>
                    {
                        Functions.ForceJoin(pastInstance.WorldID);
                    }, $"Press to join this instance of {pastInstance.WorldName}");
                    baseMenu.pageItems.Insert(0, Button);
                }
            }
        }

        public static void SaveInstances()
        {
            File.WriteAllText($"{Directory.GetCurrentDirectory()}\\Evolve\\Worlds\\History.txt", JsonConvert.SerializeObject(History));
        }

        public static IEnumerator EnteredWorld()
        {
            while (RoomManager.field_Internal_Static_ApiWorld_0 == null) yield return null;
            try
            {
                SerializedWorld NewWorld = new SerializedWorld
                {
                    WorldID = RoomManager.field_Internal_Static_ApiWorldInstance_0.id,
                    WorldName = RoomManager.field_Internal_Static_ApiWorld_0.name,
                    WorldImageURL = RoomManager.field_Internal_Static_ApiWorld_0.imageUrl
                };
                foreach (var World in History)
                {
                    if (World.WorldID == NewWorld.WorldID) History.Remove(World);
                }
                History.Add(NewWorld);
                SaveInstances();
                Refresh();
                yield break;
            }
            catch { yield break; }
        }
    }
}