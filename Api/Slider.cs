using ButtonApi;
using System;
using System.Linq;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Evolve.Api
{
    internal class SliderApi
    {
        public static Text Create(string parentPath, float x, float y, string Text, Action<float> evt, float defaultValue = 0f)
        {
            var NewText = UnityEngine.Object.Instantiate<GameObject>(QMStuff.GetQuickMenuInstance().transform.Find("QuickMenu_NewElements/_InfoBar/EarlyAccessText").gameObject, QMStuff.GetQuickMenuInstance().transform.Find(parentPath)).gameObject;
            NewText.SetActive(true);

            var TextComponent = NewText.GetComponent<Text>();
            TextComponent.fontSize = 45;
            TextComponent.fontStyle = FontStyle.Normal;
            TextComponent.text = Text;
            TextComponent.enabled = true;
            TextComponent.GetComponent<RectTransform>().sizeDelta = new Vector2(TextComponent.fontSize * Text.Count(), 100);
            TextComponent.alignment = TextAnchor.MiddleCenter;

            basePosition = new QMSingleButton(parentPath, x, y, "", null, "", null, null);
            basePosition.setActive(false);
            slider = UnityEngine.Object.Instantiate<Transform>(QMStuff.GetVRCUiMInstance().field_Public_GameObject_0.transform.Find("Screens/Settings/MousePanel/SensitivitySlider"), QMStuff.GetQuickMenuInstance().transform.Find(parentPath)).gameObject;
            slider.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            slider.transform.localPosition = basePosition.getGameObject().transform.localPosition;
            slider.GetComponentInChildren<RectTransform>().anchorMin += new Vector2(0.06f, 0f);
            slider.GetComponentInChildren<RectTransform>().anchorMax += new Vector2(0.1f, 0f);
            slider.GetComponentInChildren<Slider>().onValueChanged = new Slider.SliderEvent();
            slider.GetComponentInChildren<Slider>().value = defaultValue;
            slider.GetComponentInChildren<Slider>().onValueChanged.AddListener(DelegateSupport.ConvertDelegate<UnityAction<float>>(evt));
            TextComponent.transform.localPosition = slider.transform.localPosition;
            return TextComponent;
        }


        public static GameObject CreateCustom(string parentPath, float x, float y, string Text, Action<float> evt, float defaultValue = 0f, float MinValue = 0f, float MaxValue = 1f)
        {
            basePosition = new QMSingleButton(parentPath, x, y, "", null, "", null, null);
            basePosition.setActive(false);
            slider = UnityEngine.Object.Instantiate<Transform>(QMStuff.GetVRCUiMInstance().field_Public_GameObject_0.transform.Find("Screens/Settings/MousePanel/SensitivitySlider"), QMStuff.GetQuickMenuInstance().transform.Find(parentPath)).gameObject;
            slider.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            slider.transform.localPosition = basePosition.getGameObject().transform.localPosition;
            slider.GetComponentInChildren<RectTransform>().anchorMin += new Vector2(0.06f, 0f);
            slider.GetComponentInChildren<RectTransform>().anchorMax += new Vector2(0.1f, 0f);
            slider.GetComponentInChildren<Slider>().onValueChanged = new Slider.SliderEvent();
            slider.GetComponentInChildren<Slider>().value = defaultValue;
            slider.GetComponentInChildren<Slider>().maxValue = MaxValue;
            slider.GetComponentInChildren<Slider>().minValue = MinValue;
            slider.GetComponentInChildren<Slider>().onValueChanged.AddListener(DelegateSupport.ConvertDelegate<UnityAction<float>>(evt));
            var NewText = UnityEngine.Object.Instantiate<GameObject>(
                QMStuff.GetQuickMenuInstance().transform.Find("QuickMenu_NewElements/_InfoBar/EarlyAccessText").gameObject,
                slider.transform);
            NewText.SetActive(true);

            var TextComponent = NewText.GetComponent<Text>();
            TextComponent.fontSize = 38;
            TextComponent.fontStyle = FontStyle.Normal;
            TextComponent.text = Text;
            TextComponent.enabled = true;
            TextComponent.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 50);
            TextComponent.alignment = TextAnchor.MiddleCenter;
            TextComponent.GetComponent<RectTransform>().anchoredPosition = new Vector3(210,5,0);
            return slider;
        }

        public static GameObject CreateCustomInt(string parentPath, float x, float y, string Text, Action<int> evt, float defaultValue = 0f, int MinValue = 0, int MaxValue = 1)
        {
            basePosition = new QMSingleButton(parentPath, x, y, "", null, "", null, null);
            basePosition.setActive(false);
            slider = UnityEngine.Object.Instantiate<Transform>(QMStuff.GetVRCUiMInstance().field_Public_GameObject_0.transform.Find("Screens/Settings/MousePanel/SensitivitySlider"), QMStuff.GetQuickMenuInstance().transform.Find(parentPath)).gameObject;
            slider.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            slider.transform.localPosition = basePosition.getGameObject().transform.localPosition;
            slider.GetComponentInChildren<RectTransform>().anchorMin += new Vector2(0.06f, 0f);
            slider.GetComponentInChildren<RectTransform>().anchorMax += new Vector2(0.1f, 0f);
            slider.GetComponentInChildren<Slider>().onValueChanged = new Slider.SliderEvent();
            slider.GetComponentInChildren<Slider>().value = defaultValue;
            slider.GetComponentInChildren<Slider>().maxValue = MaxValue;
            slider.GetComponentInChildren<Slider>().minValue = MinValue;
            slider.GetComponentInChildren<Slider>().onValueChanged.AddListener(DelegateSupport.ConvertDelegate<UnityAction<float>>(evt));
            var NewText = UnityEngine.Object.Instantiate<GameObject>(
                QMStuff.GetQuickMenuInstance().transform.Find("QuickMenu_NewElements/_InfoBar/EarlyAccessText").gameObject,
                slider.transform);
            NewText.SetActive(true);

            var TextComponent = NewText.GetComponent<Text>();
            TextComponent.fontSize = 38;
            TextComponent.fontStyle = FontStyle.Normal;
            TextComponent.text = Text;
            TextComponent.enabled = true;
            TextComponent.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 50);
            TextComponent.alignment = TextAnchor.MiddleCenter;
            TextComponent.GetComponent<RectTransform>().anchoredPosition = new Vector3(210, 5, 0);
            return slider;
        }
        public static QMSingleButton basePosition;
        public static GameObject slider;
    }
}