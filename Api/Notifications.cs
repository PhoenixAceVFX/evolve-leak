using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Evolve.Api
{
    class Notifications
    {
        public static List<GameObject> AllIcons = new List<GameObject>();
        public static Image IconNotif()
        {
            //Create the sprite
            var hudRoot = GameObject.Find("UserInterface/UnscaledUI/HudContent/Hud");
            var requestedParent = hudRoot.transform.Find("NotificationDotParent");
            var indicator = UnityEngine.Object.Instantiate(hudRoot.transform.Find("NotificationDotParent/NotificationDot").gameObject, requestedParent, false).Cast<GameObject>();
            indicator.name = "Evolve Notification Icon";
            indicator.SetActive(true);
            indicator.transform.localPosition = new Vector3(200, 1000, 0);
            var image = indicator.GetComponent<Image>();
            image.color = Color.white;
            image.sprite = null;
            MelonLoader.MelonCoroutines.Start(GetNotificationsSprite());
            IEnumerator GetNotificationsSprite()
            {
                var Sprite = new Sprite();
                WWW www = new WWW("https://i.imgur.com/T6VEurj.png", null, new Il2CppSystem.Collections.Generic.Dictionary<string, string>());
                yield return www;
                {
                    Sprite = Sprite.CreateSprite(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0), 100 * 1000, 1000, SpriteMeshType.FullRect, Vector4.zero, false);
                    image.sprite = Sprite;
                    image.GetComponent<RectTransform>().sizeDelta *= new Vector3(1.3f, 1.3f);
                    indicator.transform.localPosition = new Vector3(0, 0, 0);
                    AllIcons.Add(indicator);
                }
            }
            return image;
        }

        public static bool IsRunning = false;
        public static Image Notify(string Text)
        {
            //Create the sprite
            var hudRoot = GameObject.Find("UserInterface/UnscaledUI/HudContent/Hud");
            var requestedParent = hudRoot.transform.Find("NotificationDotParent");
            var indicator = UnityEngine.Object.Instantiate(hudRoot.transform.Find("NotificationDotParent/NotificationDot").gameObject, requestedParent, false).Cast<GameObject>();
            indicator.name = "Evolve Notification";
            indicator.SetActive(true);
            indicator.transform.localPosition = new Vector3(200, 1000, 0);
            var image = indicator.GetComponent<Image>();
            image.color = Color.white;
            image.sprite = null;
            MelonLoader.MelonCoroutines.Start(GetNotificationsSprite());
            IEnumerator GetNotificationsSprite()
            {
                while (IsRunning) yield return null;
                IsRunning = true;
                var Sprite = new Sprite();
                WWW www = new WWW("https://i.imgur.com/7Cymd2I.png", null, new Il2CppSystem.Collections.Generic.Dictionary<string, string>());
                yield return www;
                {
                    Sprite = Sprite.CreateSprite(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0), 100 * 1000, 1000, SpriteMeshType.FullRect, Vector4.zero, false);
                    image.sprite = Sprite;
                    image.GetComponent<RectTransform>().sizeDelta *= new Vector3(8, 1.5f);

                    //Create and parent the text
                    var gameObject = new GameObject("Evolve Notification Text");
                    gameObject.AddComponent<Text>();
                    gameObject.transform.SetParent(image.transform, false);
                    gameObject.transform.localScale = Vector3.one;
                    gameObject.transform.localPosition = new Vector3(0, -20 ,0);
                    var text = gameObject.GetComponent<Text>();
                    text.fontStyle = FontStyle.Normal;
                    text.horizontalOverflow = HorizontalWrapMode.Overflow;
                    text.verticalOverflow = VerticalWrapMode.Overflow;
                    text.alignment = TextAnchor.MiddleCenter;
                    text.fontSize = 30;
                    text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                    text.supportRichText = true;
                    text.text = "<color=#ff006a>" + Text + "</color>";
                    gameObject.SetActive(true);
                    MelonLoader.MelonCoroutines.Start(Animation(indicator));

                    while (VRCPlayer.field_Internal_Static_VRCPlayer_0 != true) yield return null;
                    GameObject AudioObject = new GameObject();
                    AudioObject.transform.position = VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position;
                    AudioObject.AddComponent<AudioSource>();
                    var audioLoader = new WWW(string.Format("file://{0}", string.Concat(Directory.GetCurrentDirectory(), "/Evolve/Sounds/HUDNotifications.ogg")).Replace("\\", "/"), null, new Il2CppSystem.Collections.Generic.Dictionary<string, string>());
                    yield return audioLoader;
                    audioLoader.GetAudioClip().name = "Evolve Notifications Sound";
                    AudioObject.GetComponent<AudioSource>().clip = audioLoader.GetAudioClip(false, false, AudioType.OGGVORBIS);
                    AudioObject.GetComponent<AudioSource>().volume = 0.7f;
                    AudioObject.GetComponent<AudioSource>().Play();
                    yield return new WaitForSeconds(2); //Wait till the sound is done and delete it
                    UnityEngine.Object.Destroy(AudioObject);
                }
            }

            IEnumerator Animation(GameObject Object)
            {
                for (int I = 0; I < 20; I++)
                {
                    yield return new WaitForSeconds(0.03f);
                    Object.transform.localPosition -= new Vector3(0, 25, 0);
                }
                yield return new WaitForSeconds(4);
                for (int I = 0; I < 20; I++)
                {
                    yield return new WaitForSeconds(0.03f);
                    Object.transform.localPosition += new Vector3(0, 25, 0);
                }
                UnityEngine.Object.Destroy(Object);
                IsRunning = false;
            }
            return image;
        }


        public static void StaffNotify(string Text)
        {
            //Create the sprite
            var hudRoot = GameObject.Find("UserInterface/UnscaledUI/HudContent/Hud");
            var requestedParent = hudRoot.transform.Find("NotificationDotParent");
            var indicator = UnityEngine.Object.Instantiate(hudRoot.transform.Find("NotificationDotParent/NotificationDot").gameObject, requestedParent, false).Cast<GameObject>();
            indicator.SetActive(true);
            indicator.transform.localPosition = new Vector3(200, 1000, 0);
            var image = indicator.GetComponent<Image>();
            image.color = Color.white;
            MelonLoader.MelonCoroutines.Start(GetNotificationsSprite());
            IEnumerator GetNotificationsSprite()
            {
                var Sprite = new Sprite();
                WWW www = new WWW("https://i.imgur.com/IbNHDnG.png", null, new Il2CppSystem.Collections.Generic.Dictionary<string, string>());
                yield return www;
                {
                    Sprite = Sprite.CreateSprite(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0), 100 * 1000, 1000, SpriteMeshType.FullRect, Vector4.zero, false);
                    image.sprite = Sprite;
                    image.GetComponent<RectTransform>().sizeDelta *= new Vector3(8, 1.5f);

                    //Create and parent the text
                    var gameObject = new GameObject("Evolve Notification Text");
                    gameObject.AddComponent<Text>();
                    gameObject.transform.SetParent(image.transform, false);
                    gameObject.transform.localScale = Vector3.one;
                    gameObject.transform.localPosition = new Vector3(0, -20, 0);
                    var text = gameObject.GetComponent<Text>();
                    text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                    text.fontStyle = FontStyle.Normal;
                    text.horizontalOverflow = HorizontalWrapMode.Overflow;
                    text.verticalOverflow = VerticalWrapMode.Overflow;
                    text.alignment = TextAnchor.MiddleCenter;
                    text.fontSize = 30;
                    text.supportRichText = true;
                    text.text = "<color=#ff006a>" + Text + "</color>";
                    gameObject.SetActive(true);
                    MelonLoader.MelonCoroutines.Start(Animation(indicator));

                    while (VRCPlayer.field_Internal_Static_VRCPlayer_0 != true) yield return null;
                    GameObject AudioObject = new GameObject();
                    AudioObject.transform.position = VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position;
                    AudioObject.AddComponent<AudioSource>();
                    var audioLoader = new WWW(string.Format("file://{0}", string.Concat(Directory.GetCurrentDirectory(), "/Evolve/Sounds/ServerNotification.ogg")).Replace("\\", "/"), null, new Il2CppSystem.Collections.Generic.Dictionary<string, string>());
                    yield return audioLoader;
                    audioLoader.GetAudioClip().name = "Evolve Notifications Sound";
                    AudioObject.GetComponent<AudioSource>().clip = audioLoader.GetAudioClip(false, false, AudioType.OGGVORBIS);
                    AudioObject.GetComponent<AudioSource>().volume = 0.7f;
                    AudioObject.GetComponent<AudioSource>().Play();
                    yield return new WaitForSeconds(2);
                    AudioObject.GetComponent<AudioSource>().Pause();
                    yield return new WaitForSeconds(1.7f);
                    AudioObject.GetComponent<AudioSource>().UnPause();
                    yield return new WaitForSeconds(2);
                    UnityEngine.Object.Destroy(AudioObject);
                }
            }

            IEnumerator Animation(GameObject Object)
            {
                for (int I = 0; I < 20; I++)
                {
                    yield return new WaitForSeconds(0.03f);
                    Object.transform.localPosition -= new Vector3(0, 25, 0);
                }
                yield return new WaitForSeconds(4);
                for (int I = 0; I < 20; I++)
                {
                    yield return new WaitForSeconds(0.03f);
                    Object.transform.localPosition += new Vector3(0, 25, 0);
                }
                UnityEngine.Object.Destroy(Object);
            }
        }
    }
}
