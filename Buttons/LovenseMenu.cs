using MelonLoader;
using Newtonsoft.Json.Linq;
using ButtonApi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.WebPages;
using System.Windows.Forms;
using Evolve.Api;
using Evolve.Buttons;
using Evolve.ConsoleUtils;
using Evolve.Modules;
using Evolve.Utils;
using UnityEngine;
using Evolve.Wrappers;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using System.Threading;

namespace Evolve.Module
{
    internal class LovenseRemote
    {

        public static ArrayList Toys = new ArrayList();
        static string findButton = null;
        static bool lockSpeed = false;
        static bool requireHold = false;
        public static bool Waves = false;
        public static QMNestedButton ThisMenu;
        public static QMSingleButton LockButtonUI;
        public static float Speed = 0;
        public static float WaveLenght = 1;
        public static GameObject Slider1;
        public static GameObject Slider2;
        static QMSingleButton LockKeyBind;
        static QMSingleButton HoldButtonUI;
        static QMSingleButton HoldKeyBind;
        static QMSingleButton addButtonUI;
        static QMToggleButton holdToggle;
        private static QMToggleButton WavesButton;
        public static string ControlType = "Hands";

        public static QMSingleButton ClearButton;
        static KeyCode lockButton;//button to lock speed
        static KeyCode holdButton;//button to hold with other controll to use toy (if enabled)

        public static IEnumerator WaveLoop()
        {
            while (Waves)
            {
                yield return new WaitForSeconds(WaveLenght);
                if (Settings.LovenseRemote && ControlType == "Sliders" && Toys != null)
                {
                    foreach (Toy Toy in Toys)
                    {
                        Toy.setSpeed(0);
                        yield return new WaitForSeconds(1.5f);
                        Toy.setSpeed(Speed);
                    }
                }
            }
            yield break;
        }
        public static void Initialiaze()
        {
            ThisMenu = new QMNestedButton(TouchMenu.ThisMenu, 5, 1.75f, "Lovense", "Lovense remote");
            ThisMenu.getMainButton().getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);
            Panels.PanelMenu(ThisMenu, 0, 0.3f, "\nToys:", 1.1f, 2.4F, "Toys");
            Panels.PanelMenu(ThisMenu, 5, 0.3f, "\nControls Type:", 1.1f, 2.4F, "Control Type");
            MenuText Text = new MenuText(ThisMenu, 900, 300, "Connected toys: 0 / 8");
            Text.SetFontSize(70);

            WavesButton = new QMToggleButton(ThisMenu, 1, 2, "Waves", () =>
            {
                Waves = true;
                MelonCoroutines.Start(WaveLoop());
            }, "Disabled", () =>
            {
                Waves = false;

            }, "Toggle waves mode");
            WavesButton.setActive(false);

            new QMSingleButton(ThisMenu, 5, 0, "Hands", new System.Action(() =>
            {
                ControlType = "Hands";
                LockButtonUI.setActive(true);
                LockKeyBind.setActive(true);
                holdToggle.setActive(true);
                Slider1.gameObject.SetActive(false);
                Slider2.gameObject.SetActive(false);
                WavesButton.setActive(false);
                if (Toy.Buttons == null) return;
                foreach (var Button in Toy.Buttons)
                {
                    Button.SetActive(true);
                }
            }), "Control toys with your hands", null, null);

            new QMSingleButton(ThisMenu, 5, 1, "Sliders", new System.Action(() =>
            {
                ControlType = "Sliders";
                LockButtonUI.setActive(false);
                LockKeyBind.setActive(false);
                HoldButtonUI.setActive(false);
                HoldKeyBind.setActive(false);
                holdToggle.setActive(false);
                Slider1.gameObject.SetActive(true);
                Slider2.gameObject.SetActive(true);
                WavesButton.setActive(true);
                if (Toy.Buttons == null) return;
                foreach (var Button in Toy.Buttons)
                {
                    Button.SetActive(false);
                }
            }), "Control toys with the menu's sliders", null, null);

            Slider1 = SliderApi.CreateCustom(ThisMenu.getMenuName(), 2.2f, 0.5f, "Vibrations: 0 / 10", (float Val) =>
            {
                Speed = String.Format("{0:0.0}", Val / 5).AsFloat();
                Slider1.GetComponentInChildren<Text>().text = $"Vibrations: {String.Format("{0:0.0}", Val).AsFloat()} / 10"; 
            }, 0, 0f, 10);
            Slider1.GetComponent<RectTransform>().sizeDelta *= new Vector2(1.5f, 1.5f);
            Slider1.gameObject.SetActive(false);
            Slider1.GetComponentInChildren<Text>().transform.position += new Vector3(0.05f, 0, 0);

            Slider2 = SliderApi.CreateCustom(ThisMenu.getMenuName(), 2.2f, 1, "Wave length: 1 / 5", (float Val) =>
            {
                WaveLenght = String.Format("{0:0.0}", Val).AsFloat();
                Slider2.GetComponentInChildren<Text>().text = $"Wave length: {String.Format("{0:0.0}", Val).AsFloat()} / 5";
            }, 0, 1f, 5);
            Slider2.GetComponent<RectTransform>().sizeDelta *= new Vector2(1.5f, 1.5f);
            Slider2.gameObject.SetActive(false);
            Slider2.GetComponentInChildren<Text>().transform.position += new Vector3(0.05f, 0, 0);

            new QMToggleButton(ThisMenu, 0, 1, "Enable Remote", delegate ()
            {
                Settings.LovenseRemote = true;
            }, "Disabled", delegate () {
                Settings.LovenseRemote = false;
                foreach (Toy toy in Toys)
                {
                    toy.setSpeed(0);
                }
            }, "Enable remote control, if this is disabled nothing will be working ^^");

            LockButtonUI = new QMSingleButton(ThisMenu, 2, 2, "Lock Speed\nButton", delegate () {
                if (findButton == "lockButton")
                {
                    lockButton = KeyCode.None;
                    findButton = null;
                    LockButtonUI.setButtonText("Lock Speed\nButton\nCleared");
                    return;
                }
                findButton = "lockButton";
                LockButtonUI.setButtonText("Press Now");
            }, "Click than press button on controller to set button to lock vibraton speed", null, null);

            // LockKey keybind 
            LockKeyBind = new QMSingleButton(ThisMenu, 2, 2.75f, "none", new System.Action(() =>
            {

            }), "Shows current Lock Speed Button keybind", null, null);
            LockKeyBind.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1f, 2);
            LockKeyBind.setIntractable(false);

            HoldButtonUI = new QMSingleButton(ThisMenu, 3, 2, "Hold\nButton", delegate () {
                if (findButton == "holdButton")
                {
                    holdButton = KeyCode.None;
                    findButton = null;
                    HoldButtonUI.setButtonText("Hold\nButton\nCleared");
                    return;
                }
                findButton = "holdButton";
                HoldButtonUI.setButtonText("Press Now");
            }, "Click than press button on controller to set button to hold to use toy", null, null);

            // LockKey keybind 
            HoldKeyBind = new QMSingleButton(ThisMenu, 3, 2.75f, "none", new System.Action(() =>
            {

            }), "Shows current Hold Button keybind", null, null);
            HoldKeyBind.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1f, 2);
            HoldKeyBind.setIntractable(false);
            HoldKeyBind.setActive(false);

            addButtonUI = new QMSingleButton(ThisMenu, 0, -0.25f, "Add", delegate ()
            {
                Wrappers.PopupManager.InputeText("Example: https://c.lovense.com/c/dsf8px", "Enter", new Action<string>((a) =>
                {
                    string token = getToken(a);
                    string[] idName = getIDandName(token);//name, id
                    if (token == null || idName == null)
                    {
                        Wrappers.Utils.VRCUiPopupManager.HideCurrentPopUp();
                        Wrappers.Utils.VRCUiPopupManager.ShowAlert("Evolve Engine", $"Failed to add toy, please generate a new link");
                        EvoVrConsole.Log(EvoVrConsole.LogsType.Info, "Failed to add toy, please generate a new link");
                    }
                    if (Toys.Count == 8)
                    {
                        Text.SetText($"Connected toys: {Toys.Count} / 8");
                        Wrappers.Utils.VRCUiPopupManager.HideCurrentPopUp();
                        Wrappers.Utils.VRCUiPopupManager.ShowAlert("Evolve Engine", $"Too much toys connected (8 max)");
                        EvoVrConsole.Log(EvoVrConsole.LogsType.Info, "Too much toys connected (8 max)");
                    }
                    else
                    {
                        new Toy(idName[0], token, idName[1]);
                        Text.SetText($"Connected toys: {Toys.Count} / 8");
                    }
                }));

            }, "Enter the long distance control link", null, null);
            addButtonUI.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            ClearButton = new QMSingleButton(ThisMenu, 0, 0.25f, "Clear", () =>
            {
                foreach (var Button in Toy.Buttons)
                {
                    UnityEngine.Object.Destroy(Button);
                }
                foreach (Toy Toy in Toys)
                {
                    Toy.setSpeed(0);
                }
                Toys.Clear();
                Text.SetText($"Connected toys: 0 / 8");
                Toy.x = 0;
                Toy.y = 0;
            }, "Remove all toys");
            ClearButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);


            holdToggle = new QMToggleButton(ThisMenu, 1, 2, "Hold", delegate ()
            {
                HoldButtonUI.setActive(true);
                HoldKeyBind.setActive(true);
                requireHold = true;
            }, "Disabled", delegate () {
                HoldButtonUI.setActive(false);
                HoldKeyBind.setActive(false);
                requireHold = false;

            }, "Require holding a button to use toy?");

            holdToggle.setToggleState(requireHold);
            HoldButtonUI.setActive(requireHold);

        }

        public static void OnUpdate()
        {
            if (!Settings.LovenseRemote || Toys == null) return;
            if (ControlType == "Hands")
            {
                if (HoldKeyBind != null)
                {
                    HoldKeyBind.setButtonText(holdButton.ToString());
                }

                if (LockKeyBind != null)
                {
                    LockKeyBind.setButtonText(lockButton.ToString());
                }

                if (findButton != null) getButton();

                if (Input.GetKeyDown(lockButton))
                {
                    if (lockSpeed) lockSpeed = false;
                    else lockSpeed = true;
                }

                if (lockSpeed) return;

                if (requireHold)
                {
                    if (!Input.GetKey(holdButton))
                    {
                        foreach (Toy toy in Toys)
                        {
                            toy.setSpeed(0);
                        }
                        return;
                    }
                }

                foreach (Toy toy in Toys)
                {
                    float speed = 0;
                    switch (toy.hand)
                    {
                        case "none":
                            break;
                        case "left":
                            speed = Input.GetAxis("Oculus_CrossPlatform_PrimaryIndexTrigger");
                            break;
                        case "right":
                            speed = Input.GetAxis("Oculus_CrossPlatform_SecondaryIndexTrigger");
                            break;
                        case "either":
                            float left = Input.GetAxis("Oculus_CrossPlatform_PrimaryIndexTrigger");
                            float right = Input.GetAxis("Oculus_CrossPlatform_SecondaryIndexTrigger");
                            if (left > right) speed = left;
                            else speed = right;
                            break;
                    }
                    toy.setSpeed(speed);
                }
            }

            if (ControlType == "Sliders")
            {
                if (Waves) return;
                foreach (Toy toy in Toys)
                {
                    toy.setSpeed(Speed);
                }
            }
        }



        public static void getButton()
        {
            //A-Z
            for (int i = 97; i <= 122; i++)
                if (Input.GetKey((KeyCode) i))
                {
                    setButton((KeyCode) i);
                    return;
                }

            //left vr controller buttons
            if (Input.GetKey(KeyCode.JoystickButton0)) setButton(KeyCode.JoystickButton0);
            else if (Input.GetKey(KeyCode.JoystickButton1)) setButton(KeyCode.JoystickButton1);
            else if (Input.GetKey(KeyCode.JoystickButton2)) setButton(KeyCode.JoystickButton2);
            else if (Input.GetKey(KeyCode.JoystickButton3)) setButton(KeyCode.JoystickButton3);
            else if (Input.GetKey(KeyCode.JoystickButton8)) setButton(KeyCode.JoystickButton8);
            else if (Input.GetKey(KeyCode.JoystickButton9)) setButton(KeyCode.JoystickButton9);

            //right vr controller buttons
            else if (Input.GetKey(KeyCode.Joystick1Button0)) setButton(KeyCode.Joystick1Button0);
            else if (Input.GetKey(KeyCode.Joystick1Button1)) setButton(KeyCode.Joystick1Button1);
            else if (Input.GetKey(KeyCode.Joystick1Button2)) setButton(KeyCode.Joystick1Button2);
            else if (Input.GetKey(KeyCode.Joystick1Button3)) setButton(KeyCode.Joystick1Button3);
            else if (Input.GetKey(KeyCode.Joystick1Button8)) setButton(KeyCode.Joystick1Button8);
            else if (Input.GetKey(KeyCode.Joystick1Button9)) setButton(KeyCode.Joystick1Button9);
        }

        public static void setButton(KeyCode button)
        {
            if (findButton.Equals("lockButton"))
            {
                lockButton = button;
                LockButtonUI.setButtonText("Lock Speed\nButton Set");
#pragma warning disable CS0618 // 'MelonPrefs' est obsolète : 'MelonPrefs is obsolete. Please use MelonPreferences instead.'
#pragma warning disable CS0618 // 'MelonPrefs.SetInt(string, string, int)' est obsolète : 'MelonPrefs.SetInt is obsolete. Please use MelonPreferences.SetEntryInt instead.'
                MelonPrefs.SetInt("LovenseRemote", "lockButton", button.GetHashCode());
#pragma warning restore CS0618 // 'MelonPrefs.SetInt(string, string, int)' est obsolète : 'MelonPrefs.SetInt is obsolete. Please use MelonPreferences.SetEntryInt instead.'
#pragma warning restore CS0618 // 'MelonPrefs' est obsolète : 'MelonPrefs is obsolete. Please use MelonPreferences instead.'
            }
            else if (findButton.Equals("holdButton"))
            {
                holdButton = button;
                HoldButtonUI.setButtonText("Hold\nButton Set");
#pragma warning disable CS0618 // 'MelonPrefs' est obsolète : 'MelonPrefs is obsolete. Please use MelonPreferences instead.'
#pragma warning disable CS0618 // 'MelonPrefs.SetInt(string, string, int)' est obsolète : 'MelonPrefs.SetInt is obsolete. Please use MelonPreferences.SetEntryInt instead.'
                MelonPrefs.SetInt("LovenseRemote", "holdButton", button.GetHashCode());
#pragma warning restore CS0618 // 'MelonPrefs.SetInt(string, string, int)' est obsolète : 'MelonPrefs.SetInt is obsolete. Please use MelonPreferences.SetEntryInt instead.'
#pragma warning restore CS0618 // 'MelonPrefs' est obsolète : 'MelonPrefs is obsolete. Please use MelonPreferences instead.'
            }
            findButton = null;
        }

        static string[] getIDandName(string token)
        {
            if (token == null) return null;
            var url = "https://c.lovense.com/app/ws2/play/" + token;
            var httpRequest = (HttpWebRequest) WebRequest.Create(url);
            httpRequest.Headers["authority"] = "c.lovense.com";
            httpRequest.Headers["sec-ch-ua"] = "\"Google Chrome\";v=\"87\", \" Not; A Brand\";v=\"99\", \"Chromium\";v=\"87\"";
            httpRequest.Headers["sec-ch-ua-mobile"] = "?0";
            httpRequest.Headers["upgrade-insecure-requests"] = "1";
            httpRequest.Headers["sec-fetch-site"] = "same-origin";
            httpRequest.Headers["sec-fetch-mode"] = "navigate";
            httpRequest.Headers["sec-fetch-user"] = "?1";
            httpRequest.Headers["sec-fetch-dest"] = "document";
            httpRequest.Headers["accept-language"] = "en-US,en;q=0.9";
            httpRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36";
            httpRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            httpRequest.Referer = "https://c.lovense.com/app/ws/play/" + token;
            var httpResponse = (HttpWebResponse) httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                int start = result.IndexOf("JSON.parse('") + 12;
                int end = result.IndexOf("')");
                if (end == -1) return null;
                JObject json = JObject.Parse(result.Substring(start, end - start));
                if (json.Count == 0)
                {
                    return null;
                }
                else
                {
                    string id = (string) json.First.First["id"];
                    string name = (string) json.First.First["name"];
                    name = char.ToUpper(name[0]) + name.Substring(1);//make first letter uppercase
                    return new string[] { name, id };
                }
            }
        }

        public static string getToken(string Input)
        {
            if (!Input.Contains("https://c.lovense.com/c/")) return null;
            HttpWebResponse resp = null;
            try
            {
                HttpWebRequest req = (HttpWebRequest) HttpWebRequest.Create(Input);
                req.Method = "HEAD";
                req.AllowAutoRedirect = false;
                resp = (HttpWebResponse) req.GetResponse();
                Input = resp.Headers["Location"];
            }
#pragma warning disable CS0168 // La variable 'e' est déclarée, mais jamais utilisée
            catch (Exception e)
#pragma warning restore CS0168 // La variable 'e' est déclarée, mais jamais utilisée
            {
                return null;
            }
            finally
            {
                if (resp != null) resp.Close();
            }
            int pos = Input.LastIndexOf("/") + 1;
            return Input.Substring(pos, Input.Length - pos);
        }

    }

    internal class Toy
    {
        public static List<GameObject> Buttons = new List<GameObject>();
        public string hand = "none";
        public static int x = 0;
        public static int y = 0;
        public QMSingleButton button;
        private float lastSpeed;
        private string name;
        private string token;
        private string id;

        public Toy(string name, string token, string id)
        {
            this.token = token;
            this.id = id;
            this.name = name;
            x++;
            if (x == 5 && y == 0)
            {
                x = 1;
                y = 1;
            }
            button = new QMSingleButton(LovenseRemote.ThisMenu, x, y, name + "\n(Press)", delegate ()
            {
                changeHand();
            }, "Press to set control mode", null, null);
            button.getGameObject().GetComponent<RectTransform>().sizeDelta *= new Vector2(1, 2);
            button.getGameObject().GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, 315);
            Buttons.Add(button.getGameObject());
            if (LovenseRemote.ControlType == "Sliders")
            {
                button.setActive(false);
            }
            LovenseRemote.Toys.Add(this);
        }

        public void setSpeed(float speed)
        {
            speed = (int) (speed * 10);
            if (speed != lastSpeed)
            {
                lastSpeed = speed;
                send((int) speed);
            }
        }

        public void changeHand()
        {
            switch (hand)
            {
                case "none":
                    hand = "left";
                    button.setButtonText(name + "\nLeft Hand");
                    //LovenseRemote.LockButtonUI.setActive(true);//in case this was disabled
                    break;
                case "left":
                    hand = "right";
                    button.setButtonText(name + "\nRight Hand");
                    break;
                case "right":
                    hand = "either";
                    button.setButtonText(name + "\nBoth Hands");
                    break;
                case "either":
                    hand = "none";
                    button.setButtonText(name + "\nClick to\nSet");
                    LovenseRemote.LockButtonUI.setActive(true);//in case this was disabled
                    break;
            }
        }

        private void send(int speed)
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                var httpRequest = (HttpWebRequest) WebRequest.Create("https://c.lovense.com/app/ws/command/" + token);
                httpRequest.Method = "POST";
                httpRequest.ContentType = "application/x-www-form-urlencoded";
                using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                {
                    streamWriter.Write("order=%7B%22cate%22%3A%22id%22%2C%22id%22%3A%7B%22" + id + "%22%3A%7B%22v%22%3A" + speed + "%2C%22p%22%3A-1%2C%22r%22%3A-1%7D%7D%7D");
                }
                var httpResponse = (HttpWebResponse) httpRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    //Console.WriteLine(result);
                }
                //Console.WriteLine(httpResponse.StatusCode);
            }).Start();
        }
    }
}