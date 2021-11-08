using Evolve.Utils;
using ButtonApi;
using UnityEngine;
using UnityEngine.UI;

namespace Evolve.Api
{
    internal class Panels
    {
        public static QMSingleButton PanelMenu(QMNestedButton parent, float x, float y, string Txt, float rectx, float recty, string tooltip)
        {
            var NewText = new QMSingleButton(parent, x, y + 0.05f, Txt, null, tooltip);
            NewText.setIntractable(false);
            NewText.getGameObject().GetComponent<RectTransform>().sizeDelta *= new Vector2(rectx, recty);
            var TextComp = NewText.getGameObject().GetComponentInChildren<Text>();
            TextComp.fontSize = 50;
            TextComp.alignment = TextAnchor.UpperCenter;
            TextComp.color = Settings.PinkRed;

            return NewText;
        }
    }
}