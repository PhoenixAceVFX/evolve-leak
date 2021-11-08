using UnityEngine;
using UnityEngine.UI;

namespace Evolve.Api
{
    internal class ScreenAPI
    {
        public static void WorldScreen(float xpos, float ypos, string txt, System.Action listener)
        {
            var ting = GameObject.Find("Screens").transform.Find("WorldInfo");
            var BtnObj = GameObject.Find("UserInterface/MenuContent/Screens/UserInfo/User Panel/Moderator/Actions/Warn").gameObject;
            var Btn = UnityEngine.Object.Instantiate(BtnObj, BtnObj.transform, true).GetComponent<Button>();
            Btn.transform.localPosition = new Vector3(Btn.transform.localPosition.x - 275 + xpos, Btn.transform.localPosition.y - 50 + ypos, Btn.transform.localPosition.z);
            Btn.GetComponentInChildren<Text>().text = txt;
            Btn.onClick = new Button.ButtonClickedEvent();
            Btn.enabled = true;
            Btn.gameObject.SetActive(true);
            Btn.GetComponentInChildren<Image>().color = Color.white;
            Btn.GetComponent<RectTransform>().sizeDelta += new Vector2(25, 0);
            Btn.GetComponent<RectTransform>().sizeDelta -= new Vector2(0, 10);
            Btn.onClick.AddListener(listener);
            Btn.transform.SetParent(ting.transform);
        }

        public static Button UserScreen(float XPos, float YPos, string Text, System.Action Action)
        {
            var FavoriteButton = GameObject.Find("UserInterface/MenuContent/Screens/UserInfo/Buttons/RightSideButtons/RightUpperButtonColumn/FavoriteButton");
            var Button = UnityEngine.Object.Instantiate(FavoriteButton, FavoriteButton.transform.parent);
            Button.GetComponentInChildren<Text>().text = Text;
            //Button.transform.localPosition = new Vector3(Button.transform.localPosition.x + XPos, Button.transform.localPosition.y + YPos, Button.transform.localPosition.z);
            Button.GetComponentInChildren<Button>().onClick = new Button.ButtonClickedEvent();
            Button.GetComponentInChildren<Button>().onClick.AddListener(Action);
            Button.GetComponentInChildren<Button>().enabled = true;
            Button.GetComponentInChildren<Button>().interactable = true;
            Button.gameObject.SetActive(true);
            return Button.GetComponentInChildren<Button>(); 
        }
    }
}