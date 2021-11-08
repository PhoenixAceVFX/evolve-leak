using System.Collections;
using Evolve.Api;
using Il2CppSystem;
using MelonLoader;
using ButtonApi;
using UnityEngine;
using UnityEngine.UI;
using Math = System.Math;
using Evolve.ConsoleUtils;

namespace Evolve.Buttons
{
    internal class MessageMenu
    {
        public static QMNestedButton ThisMenu;
#pragma warning disable CS0649 // Le champ 'MessageMenu.History' n'est jamais assigné et aura toujours sa valeur par défaut null
        public static QMSingleButton History;
#pragma warning restore CS0649 // Le champ 'MessageMenu.History' n'est jamais assigné et aura toujours sa valeur par défaut null
#pragma warning disable CS0649 // Le champ 'MessageMenu.ConsoleButton' n'est jamais assigné et aura toujours sa valeur par défaut null
        public static QMSingleButton ConsoleButton;
#pragma warning restore CS0649 // Le champ 'MessageMenu.ConsoleButton' n'est jamais assigné et aura toujours sa valeur par défaut null
        public static QMSingleButton NotifButton;
        public static Text Message;
        public static void Initialize()
        {
            ThisMenu = new QMNestedButton("ShortcutMenu", 0, 2, "EvolveMessage", "Evolve Message Menu");
            ThisMenu.getMainButton().setActive(false);
            var Path = GameObject.Find("UserInterface/QuickMenu");
            var Parent = QMStuff.GetQuickMenuInstance().transform.Find(ThisMenu.getMenuName());
            var TextPanel = UnityEngine.Object.Instantiate(Path.transform.Find("EmoteMenu/ActionMenuInfo").gameObject, Parent, false).Cast<GameObject>();
            TextPanel.name = "Evolve Notification Icon";
            TextPanel.SetActive(true);
            Message = TextPanel.GetComponentInChildren<Text>();
            Message.supportRichText = true;
            Message.text = "";

            NotifButton = new QMSingleButton("ShortcutMenu", 0, 2, "", () =>
            {
                QMStuff.ShowQuickmenuPage(ThisMenu.getMenuName());
                if (Notifications.AllIcons != null)
                {
                    foreach (var Icon in Notifications.AllIcons)
                    {
                        UnityEngine.Object.Destroy(Icon);
                    }
                }
                NotifButton.setActive(false);
                var MicButton = GameObject.Find("/UserInterface/QuickMenu/MicControls");
                MicButton.GetComponent<RectTransform>().anchoredPosition += new Vector2(400, 0);
            }, "");
            NotifButton.setActive(false);
            NotifButton.getGameObject().GetComponentInChildren<Text>().supportRichText = true;
        }
    }
}