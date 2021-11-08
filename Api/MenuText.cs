using ButtonApi;
using UnityEngine;
using UnityEngine.UI;

namespace Evolve.Api
{
    internal class MenuText
    {
        public MenuText(QMNestedButton menuBase, float posx, float poxy, string text)
        {
            this.menuTitle = UnityEngine.Object.Instantiate<GameObject>(QMStuff.GetQuickMenuInstance().transform.Find("QuickMenu_NewElements/_InfoBar/EarlyAccessText").gameObject, menuBase.getBackButton().getGameObject().transform.parent);
            menuTitle.SetActive(true);
            this.menuTitle.GetComponent<Text>().fontStyle = FontStyle.Normal;
            this.menuTitle.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
            this.menuTitle.GetComponent<Text>().text = text;
            this.menuTitle.GetComponent<Text>().supportRichText = true;
            this.menuTitle.GetComponent<Text>().fontSize = 43;
            this.menuTitle.GetComponent<RectTransform>().anchoredPosition = new Vector2(posx, -poxy);
            this.menuTitle.GetComponent<RectTransform>().sizeDelta = new Vector2(8 * 100, 100);
            this.menuTitle.GetComponent<Text>().color = Color.white;
            this.Posx = posx;
            this.Posy = -poxy;
            this.Text = "<color=#ff006a>" + text + "</color>";
            this.menuTitle.name = string.Format("MenuText_{0}_{1}_{2}", text, posx, -this.Posy);
        }

        public MenuText(string MenuName, float posx, float poxy, string text)
        {
            this.menuTitle = UnityEngine.Object.Instantiate<GameObject>(Wrappers.Utils.QuickMenu.transform.Find("QuickMenu_NewElements/_InfoBar/EarlyAccessText").gameObject, Wrappers.Utils.QuickMenu.transform.Find(MenuName));
            menuTitle.SetActive(true);
            this.menuTitle.GetComponent<Text>().fontStyle = FontStyle.Normal;
            this.menuTitle.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
            this.menuTitle.GetComponent<Text>().text = text;
            this.menuTitle.GetComponent<Text>().supportRichText = true;
            this.menuTitle.GetComponent<Text>().fontSize = 43;
            this.menuTitle.GetComponent<RectTransform>().anchoredPosition = new Vector2(posx, -poxy);
            this.menuTitle.GetComponent<RectTransform>().sizeDelta = new Vector2(8 * 100, 100);
            this.menuTitle.GetComponent<Text>().color = Color.white;
            this.Posx = posx;
            this.Posy = -poxy;
            this.Text = "<color=#ff006a>" + text + "</color>";
            this.menuTitle.name = string.Format("MenuText_{0}_{1}_{2}", text, posx, -this.Posy);
        }

        public MenuText(Transform parent, float posx, float poxy, string text)
        {
            this.menuTitle = UnityEngine.Object.Instantiate<GameObject>(Wrappers.Utils.QuickMenu.transform.Find("QuickMenu_NewElements/_InfoBar/EarlyAccessText").gameObject, parent);
            menuTitle.SetActive(true);
            this.menuTitle.GetComponent<Text>().fontStyle = FontStyle.Normal;
            this.menuTitle.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
            this.menuTitle.GetComponent<Text>().text = text;
            this.menuTitle.GetComponent<Text>().supportRichText = true;
            this.menuTitle.GetComponent<Text>().fontSize = 43;
            this.menuTitle.GetComponent<RectTransform>().anchoredPosition = new Vector2(posx, -poxy);
            this.menuTitle.GetComponent<RectTransform>().sizeDelta = new Vector2(8 * 100, 100);
            this.menuTitle.GetComponent<Text>().color = Color.white;
            this.Posx = posx;
            this.Posy = -poxy;
            this.Text = "<color=#ff006a>" + text + "</color>";
            this.menuTitle.name = string.Format("MenuText_{0}_{1}_{2}", text, posx, -this.Posy);
        }

        public void setactive(bool value)
        {
            this.menuTitle.SetActive(value);
        }

        public void Delete()
        {
            UnityEngine.Object.Destroy(this.menuTitle);
        }

        public void SetText(string text)
        {
            this.menuTitle.GetComponent<Text>().text = text;
        }

        public void SetPos(float x, float y)
        {
            this.Posy = y;
            this.Posx = x;
            this.menuTitle.GetComponent<RectTransform>().anchoredPosition = new Vector2(this.Posx, -this.Posy);
        }

        public void SetColor(float r, float g, float b, float a)
        {
            this.menuTitle.GetComponent<Text>().color = new Color(r, g, b, a);
        }

        public void SetColor(Color color)
        {
            this.menuTitle.GetComponent<Text>().color = color;
        }

        public void SetFontSize(int size)
        {
            this.menuTitle.GetComponent<Text>().fontSize = size;
        }

        public GameObject menuTitle;

        public float Posx;

        public float Posy;

        public string Text;

#pragma warning disable CS0169 // Le champ 'MenuText.UserInfoPage' n'est jamais utilisé
        private readonly GameObject UserInfoPage;
#pragma warning restore CS0169 // Le champ 'MenuText.UserInfoPage' n'est jamais utilisé

#pragma warning disable CS0169 // Le champ 'MenuText.AvatarPage' n'est jamais utilisé
        private readonly GameObject AvatarPage;
#pragma warning restore CS0169 // Le champ 'MenuText.AvatarPage' n'est jamais utilisé

#pragma warning disable CS0169 // Le champ 'MenuText.SettingsPage' n'est jamais utilisé
        private readonly GameObject SettingsPage;
#pragma warning restore CS0169 // Le champ 'MenuText.SettingsPage' n'est jamais utilisé

#pragma warning disable CS0169 // Le champ 'MenuText.SocialPage' n'est jamais utilisé
        private readonly GameObject SocialPage;
#pragma warning restore CS0169 // Le champ 'MenuText.SocialPage' n'est jamais utilisé

#pragma warning disable CS0169 // Le champ 'MenuText.WorldsPage' n'est jamais utilisé
        private readonly GameObject WorldsPage;
#pragma warning restore CS0169 // Le champ 'MenuText.WorldsPage' n'est jamais utilisé
    }
}