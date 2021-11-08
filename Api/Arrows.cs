using ButtonApi;
using System;
using System.Linq;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Evolve.Api
{
    internal class ArrowsApi
    {
        public static QMSingleButton ArrowUp;
        public static QMSingleButton ArrowDown;
        public static void AddArrowUp(string CurrentMenu, string MenuToShow)
        {

            ArrowUp =  new QMSingleButton(CurrentMenu, 5, 0, "", () =>
            {
                QMStuff.ShowQuickmenuPage(MenuToShow);
            }, "");
            ArrowUp.getGameObject().GetComponent<Image>().sprite = QMStuff.GetQuickMenuInstance().transform.Find("EmojiMenu/PageUp").GetComponent<Image>().sprite;
        }

        public static void AddArrowDown(string CurrentMenu, string MenuToShow)
        {
            ArrowDown = new QMSingleButton(CurrentMenu, 5, 1, "", () =>
            {
                QMStuff.ShowQuickmenuPage(MenuToShow);
            }, "");
            ArrowDown.getGameObject().GetComponent<Image>().sprite = QMStuff.GetQuickMenuInstance().transform.Find("EmojiMenu/PageDown").GetComponent<Image>().sprite;
        }
    }
}