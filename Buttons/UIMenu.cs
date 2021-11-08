using Evolve.Api;
using Evolve.Utils;
using ButtonApi;
using UnityEngine;
using UnityEngine.UI;

namespace Evolve.Buttons
{
    internal class UIMenu
    {
        public static QMNestedButton ThisMenu;
        public static QMSingleButton Previous;
        public static QMSingleButton Apply;
        public static QMSingleButton Default;
        public static QMSingleButton Apply1;
        public static QMSingleButton Default1;

        public static Text SliderR;
        public static Text SliderG;
        public static Text SliderB;

        public static Text SliderTR;
        public static Text SliderTG;
        public static Text SliderTB;

        public static float R;
        public static float G;
        public static float B;

        public static float TR;
        public static float TG;
        public static float TB;

        public static void Initialize()
        {
            ThisMenu = new QMNestedButton(EvolveMenu.ThisMenu, 3.5f, 1.25f, "UI", "UI menu", Color.cyan, Color.magenta, Color.black, Color.yellow);
            ThisMenu.getMainButton().getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            Panels.PanelMenu(ThisMenu, 2.5f, -0.13f, "\nMain Color", 2, 1.1f, "Change the main color");
            Panels.PanelMenu(ThisMenu, 2.5f, 2f, "\n\n\n\n\n\nSecondary Color", 2, 1.12f, "Change the secondary color");

            Previous = new QMSingleButton(ThisMenu, 2.5f, 1, "Preview", null, "Preview of the color selected");
            Previous.getGameObject().GetComponent<RectTransform>().sizeDelta *= new Vector2(2, 1);
            Previous.getGameObject().name = "ColorPickPreviewButton";

            Apply = new QMSingleButton(ThisMenu, 4, -0.25f, "Apply", () =>
            {
                var NewColor = new Color(R, G, B);
                FoldersManager.Config.Ini.SetFloat("UIColor", "R", NewColor.r);
                FoldersManager.Config.Ini.SetFloat("UIColor", "G", NewColor.g);
                FoldersManager.Config.Ini.SetFloat("UIColor", "B", NewColor.b);
                Settings.UIColorHex = Settings.ColorToHex(NewColor, true);
                ColorChanger.ApplyNewColor();
            }, "Apply selected color");
            Apply.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            Default = new QMSingleButton(ThisMenu, 4, 0.25f, "Default", () =>
            {
                Settings.UIColorHex = "6a00ff";
                ColorChanger.ApplyNewColor();
            }, "Default color of Evolve");
            Default.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            Apply1 = new QMSingleButton(ThisMenu, 4, 1.75f, "Apply", () =>
            {
                var NewColor1 = new Color(TR, TG, TB);
                FoldersManager.Config.Ini.SetFloat("UISecColor", "R", NewColor1.r);
                FoldersManager.Config.Ini.SetFloat("UISecColor", "G", NewColor1.g);
                FoldersManager.Config.Ini.SetFloat("UISecColor", "B", NewColor1.b);

                Settings.UITextColorHex = Settings.ColorToHex(NewColor1, true);

                ColorChanger.ApplyNewColor();
            }, "Apply selected color");
            Apply1.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            Default1 = new QMSingleButton(ThisMenu, 4, 2.25f, "Default", () =>
            {
                Settings.UITextColorHex = "ff0055";
                ColorChanger.ApplyNewColor();
            }, "Default color of Evolve");
            Default1.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            //Sliders

            SliderR = SliderApi.Create(ThisMenu.getMenuName(), 2.2f, -0.25f, "R: 0", (float Val) =>
            {
                R = Val;
                Previous.setBackgroundColor(new Color(R, G, B));
                SliderR.text = $"R: {R}";
            }, 0f);

            SliderG = SliderApi.Create(ThisMenu.getMenuName(), 2.2f, 0, "G: 0", (float Val) =>
            {
                G = Val;
                Previous.setBackgroundColor(new Color(R, G, B));
                SliderG.text = $"G: {G}";
            }, 0f);

            SliderB = SliderApi.Create(ThisMenu.getMenuName(), 2.2f, 0.25f, "B: 0", (float Val) =>
            {
                B = Val;
                Previous.setBackgroundColor(new Color(R, G, B));
                SliderB.text = $"B: {B}";
            }, 0f);

            SliderTR = SliderApi.Create(ThisMenu.getMenuName(), 2.2f, 1.75f, "R: 0", (float Val) =>
            {
                TR = Val;
                Previous.setTextColor(new Color(TR, TG, TB));
                SliderTR.text = $"R: {TR}";
            }, 0f);

            SliderTG = SliderApi.Create(ThisMenu.getMenuName(), 2.2f, 2, "G: 0", (float Val) =>
            {
                TG = Val;
                Previous.setTextColor(new Color(TR, TG, TB));
                SliderTG.text = $"G: {TG}";
            }, 0f);

            SliderTB = SliderApi.Create(ThisMenu.getMenuName(), 2.2f, 2.25f, "B: 0", (float Val) =>
            {
                TB = Val;
                Previous.setTextColor(new Color(TR, TG, TB));
                SliderTB.text = $"B: {TB}";
            }, 0f);

            new QMToggleButton(ThisMenu, 4, 1, "Save", () =>
            {
                Settings.SaveUIColor = true;
                FoldersManager.Config.Ini.SetBool("UIColor", "SaveUIColor", true);
            }, "Disabled", () =>
            {
                Settings.SaveUIColor = false;
                FoldersManager.Config.Ini.SetBool("UIColor", "SaveUIColor", false);
            }, "Will save this color profile and will automatically apply it the next time the game start", null, null, false, Settings.SaveUIColor);
        }
    }
}