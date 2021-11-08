using System.Collections;
using Evolve.Api;
using Evolve.ConsoleUtils;
using Evolve.Utils;
using MelonLoader;
using Mono.CSharp;
using ButtonApi;
using UnityEngine;
using UnityEngine.UI;

namespace Evolve.Buttons
{
    internal class KeybindsMenu
    {
        public static QMNestedButton ThisMenu;
        public static Text Slider1;
        public static Text Slider2;
        public static Text Slider3;
        public static float ComfortDef = 200f;
        public static float MouseDef = 200;
        public static float DefaultMicSensivity = 100;
        public static float MicSensivity = 100;

        public static void Initialize()
        {
            ThisMenu = new QMNestedButton(EvolveMenu.ThisMenu, 2.5f, 1.75f, "Keybinds", "Keybinds menu");
            ThisMenu.getMainButton().getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            new QMSingleButton(ThisMenu, 1, 0, "CTRL + F\n(Fly)", null, "Toggle fly");
            new QMSingleButton(ThisMenu, 3, 0, "CTRL + E\n(Esp)", null, "Toggle esp");
            new QMSingleButton(ThisMenu, 4, 0, "CTRL + S\n(Speed)", null, "Toggle speed");
            new QMSingleButton(ThisMenu, 2, 0, "CTRL + T\n(Teleport to cursor)", null, "Teleport to cursor");
            new QMSingleButton(ThisMenu, 1, 1, "CTRL + R\n(Player rotation)", null, "Use arrows for desktop");
            new QMSingleButton(ThisMenu, 2, 1, "CTRL + Tab\n(Third person)", null, "Use arrows to move the camera and the scroll wheel to zoom in or out.");

        }
    }
}