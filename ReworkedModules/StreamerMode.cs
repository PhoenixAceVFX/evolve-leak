using Evolve.Utils;
using MelonLoader;
using ButtonApi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using UnityEngine;
using UnityEngine.UI;
using VRC.Core;
using TMPro;

namespace Evolve.Modules
{
    class StreamerMode
    {
        private static List<Transform> objectRoots = new List<Transform>();
        public static void SpoofName()
        {
            NameSpoofGenerator.GenerateNewName();
        }

        public static IEnumerator Loop()
        {
            objectRoots.Add(QMStuff.GetVRCUiMInstance().field_Public_GameObject_0.transform.Find("Screens/Social"));
            objectRoots.Add(QMStuff.GetVRCUiMInstance().field_Public_GameObject_0.transform.Find("Screens/WorldInfo"));
            objectRoots.Add(QMStuff.GetVRCUiMInstance().field_Public_GameObject_0.transform.Find("Screens/UserInfo"));
            while (Settings.StreamerMode)
            {
                yield return new WaitForEndOfFrame();
                try
                {
                    var nameplate = VRCPlayer.field_Internal_Static_VRCPlayer_0.field_Public_PlayerNameplate_0.gameObject.transform.Find("Contents/Main/Text Container/Name").GetComponent<TextMeshProUGUI>();
                    if (nameplate.text != NameSpoofGenerator.spoofedName) nameplate.text = NameSpoofGenerator.spoofedName;
                    foreach (Transform transform in objectRoots)
                    {
                        if (transform.gameObject.activeInHierarchy)
                        {
                            foreach (Text text in transform.GetComponentsInChildren<Text>())
                            {
                                if (text.text.Contains((APIUser.CurrentUser.displayName == "") ? APIUser.CurrentUser.username : APIUser.CurrentUser.displayName))
                                {
                                    if (text.text != NameSpoofGenerator.spoofedName) text.text = text.text.Replace((APIUser.CurrentUser.displayName == "") ? APIUser.CurrentUser.username : APIUser.CurrentUser.displayName, NameSpoofGenerator.spoofedName);
                                }
                            }
                        }
                    }

                    if (QMStuff.GetQuickMenuInstance().gameObject.activeInHierarchy)
                    {
                        foreach (Text text2 in QMStuff.GetQuickMenuInstance().gameObject.GetComponentsInChildren<Text>())
                        {
                            if (text2.text.Contains((APIUser.CurrentUser.displayName == "") ? APIUser.CurrentUser.username : APIUser.CurrentUser.displayName))
                            {
                                if (text2.text != NameSpoofGenerator.spoofedName) text2.text = text2.text.Replace((APIUser.CurrentUser.displayName == "") ? APIUser.CurrentUser.username : APIUser.CurrentUser.displayName, NameSpoofGenerator.spoofedName);
                            }
                        }
                    }
                }
                catch { }
            }
        }
    }
}
