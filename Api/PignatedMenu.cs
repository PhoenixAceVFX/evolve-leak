using Evolve.ConsoleUtils;
using ButtonApi;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Evolve.Api.PageItem;

namespace Evolve.Api
{
    internal class PaginatedMenu
    {
        public QMSingleButton menuEntryButton { get; set; }

        public int currentPage { get; set; }

        public PaginatedMenu(string parentPath, float x, float y, string menuName, string menuTooltip, Color? buttonColor)
        {
            this.menuBase = new QMNestedButton(parentPath, x, y, menuName, "", null, null, null, null);
            this.menuBase.getMainButton().DestroyMe();
            this.menuEntryButton = new QMSingleButton(parentPath, x, y, menuName, new Action(this.OpenMenu), menuTooltip, buttonColor, null);
            this.previousPageButton = new QMSingleButton(this.menuBase, 4, 0, "", delegate ()
            {
                if (this.currentPage != 0)
                {
                    int currentPage = this.currentPage;
                    this.currentPage = currentPage - 1;
                }
                this.UpdateMenu();
            }, "Move to the previous page", buttonColor, null);
            this.menuTitle = UnityEngine.Object.Instantiate<GameObject>(QMStuff.GetQuickMenuInstance().transform.Find("QuickMenu_NewElements/_InfoBar/EarlyAccessText").gameObject, this.menuBase.getBackButton().getGameObject().transform.parent);
            this.menuTitle.GetComponent<Text>().fontStyle = FontStyle.Normal;
            this.menuTitle.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            this.menuTitle.GetComponent<Text>().text = "";
            this.menuTitle.GetComponent<RectTransform>().anchoredPosition += new Vector2(580f, -440f);
            this.previousPageButton.getGameObject().GetComponent<Image>().sprite = QMStuff.GetQuickMenuInstance().transform.Find("EmojiMenu/PageUp").GetComponent<Image>().sprite;
            this.pageCount = new QMSingleButton(this.menuBase, 4, 1, "Page\n0/0", null, "Indicates the page you are on", null, null);
            UnityEngine.Object.DestroyObject(this.pageCount.getGameObject().GetComponentInChildren<ButtonReaction>());
            UnityEngine.Object.DestroyObject(this.pageCount.getGameObject().GetComponentInChildren<UiTooltip>());
            UnityEngine.Object.DestroyObject(this.pageCount.getGameObject().GetComponentInChildren<Image>());
            this.nextPageButton = new QMSingleButton(this.menuBase, 4, 2, "", delegate ()
            {
                if (this.pageItems.Count > 9)
                {
                    int currentPage = this.currentPage;
                    this.currentPage = currentPage + 1;
                }
                this.UpdateMenu();
            }, "Move to the next page", buttonColor, null);
            this.nextPageButton.getGameObject().GetComponent<Image>().sprite = QMStuff.GetQuickMenuInstance().transform.Find("EmojiMenu/PageDown").GetComponent<Image>().sprite;
            this.item1 = new QMSingleButton(this.menuBase, 1, 0, "", null, "", buttonColor, null);
            this.item2 = new QMSingleButton(this.menuBase, 2, 0, "", null, "", buttonColor, null);
            this.item3 = new QMSingleButton(this.menuBase, 3, 0, "", null, "", buttonColor, null);
            this.item4 = new QMSingleButton(this.menuBase, 1, 1, "", null, "", buttonColor, null);
            this.item5 = new QMSingleButton(this.menuBase, 2, 1, "", null, "", buttonColor, null);
            this.item6 = new QMSingleButton(this.menuBase, 3, 1, "", null, "", buttonColor, null);
            this.item7 = new QMSingleButton(this.menuBase, 1, 2, "", null, "", buttonColor, null);
            this.item8 = new QMSingleButton(this.menuBase, 2, 2, "", null, "", buttonColor, null);
            this.item9 = new QMSingleButton(this.menuBase, 3, 2, "", null, "", buttonColor, null);
            this.toggleItem1 = new QMToggleButton(this.menuBase, 1, 0, "", null, "", null, "", buttonColor, null, false, false);
            this.toggleItem2 = new QMToggleButton(this.menuBase, 2, 0, "", null, "", null, "", buttonColor, null, false, false);
            this.toggleItem3 = new QMToggleButton(this.menuBase, 3, 0, "", null, "", null, "", buttonColor, null, false, false);
            this.toggleItem4 = new QMToggleButton(this.menuBase, 1, 1, "", null, "", null, "", buttonColor, null, false, false);
            this.toggleItem5 = new QMToggleButton(this.menuBase, 2, 1, "", null, "", null, "", buttonColor, null, false, false);
            this.toggleItem6 = new QMToggleButton(this.menuBase, 3, 1, "", null, "", null, "", buttonColor, null, false, false);
            this.toggleItem7 = new QMToggleButton(this.menuBase, 1, 2, "", null, "", null, "", buttonColor, null, false, false);
            this.toggleItem8 = new QMToggleButton(this.menuBase, 2, 2, "", null, "", null, "", buttonColor, null, false, false);
            this.toggleItem9 = new QMToggleButton(this.menuBase, 3, 2, "", null, "", null, "", buttonColor, null, false, false);
        }

        public PaginatedMenu(QMNestedButton menuButton, int x, int y, string menuName, string menuTooltip, Color? buttonColor) : this(menuButton.getMenuName(), x, y, menuName, menuTooltip, buttonColor)
        {
        }

        public void OpenMenu()
        {
            this.currentPage = 0;
            this.UpdateMenu();
            QMStuff.ShowQuickmenuPage(this.menuBase.getMenuName());
        }

        public void UpdateMenu()
        {
            this.pageCount.setActive(false);
            QMSingleButton[] array = new QMSingleButton[]
            {
                this.item1,
                this.item2,
                this.item3,
                this.item4,
                this.item5,
                this.item6,
                this.item7,
                this.item8,
                this.item9
            };
            QMToggleButton[] array2 = new QMToggleButton[]
            {
                this.toggleItem1,
                this.toggleItem2,
                this.toggleItem3,
                this.toggleItem4,
                this.toggleItem5,
                this.toggleItem6,
                this.toggleItem7,
                this.toggleItem8,
                this.toggleItem9
            };
            QMSingleButton[] array3 = array;
            for (int i = 0; i < array3.Length; i++)
            {
                array3[i].setActive(false);
            }
            QMToggleButton[] array4 = array2;
            for (int i = 0; i < array4.Length; i++)
            {
                array4[i].setActive(false);
            }
            int num = (int) Math.Ceiling(pageItems.Count / 9.0);
            num--;
            if (this.currentPage < 0)
            {
                this.currentPage = 0;
            }
            if (this.currentPage > num)
            {
                this.currentPage = num;
            }
            if (this.pageItems.Count > 9)
            {
                this.pageCount.setActive(true);
                this.pageCount.setButtonText("Page\n" + (this.currentPage + 1).ToString() + " of " + ((int) Math.Ceiling(pageItems.Count / 9.0)).ToString());
            }
            List<PageItem> range = this.pageItems.GetRange(this.currentPage * 9, Math.Abs(this.currentPage * 9 - this.pageItems.Count));
            if (range == null)
            {

            }
            else if (range.Count > 0)
            {
                if (range[0].type == PageItems.Button)
                {
                    this.item1.setButtonText(range[0].Name);
                    this.item1.setAction(range[0].Action);
                    this.item1.setToolTip(range[0].Tooltip);
                    if (range[0].Active)
                    {
                        this.item1.setActive(true);
                    }
                }
                else if (range[0].type == PageItems.Toggle)
                {
                    this.toggleItem1.setOnText(range[0].onName);
                    this.toggleItem1.setOffText(range[0].offName);
                    this.toggleItem1.setAction(new Action(range[0].ButtonAction), new Action(range[0].ButtonAction));
                    this.toggleItem1.setToggleState(range[0].ToggleState, false);
                    this.toggleItem1.setToolTip(range[0].Tooltip);
                    if (range[0].Active)
                    {
                        this.toggleItem1.setActive(true);
                    }
                }
            }
            if (range.Count > 1)
            {
                if (range[1].type == PageItems.Button)
                {
                    this.item2.setButtonText(range[1].Name);
                    this.item2.setAction(range[1].Action);
                    this.item2.setToolTip(range[1].Tooltip);
                    if (range[1].Active)
                    {
                        this.item2.setActive(true);
                    }
                }
                else if (range[1].type == PageItems.Toggle)
                {
                    this.toggleItem2.setOnText(range[1].onName);
                    this.toggleItem2.setOffText(range[1].offName);
                    this.toggleItem2.setAction(new Action(range[1].ButtonAction), new Action(range[1].ButtonAction));
                    this.toggleItem2.setToggleState(range[1].ToggleState, false);
                    this.toggleItem2.setToolTip(range[1].Tooltip);
                    if (range[1].Active)
                    {
                        this.toggleItem2.setActive(true);
                    }
                }
            }
            if (range.Count > 2)
            {
                if (range[2].type == PageItems.Button)
                {
                    this.item3.setButtonText(range[2].Name);
                    this.item3.setAction(range[2].Action);
                    this.item3.setToolTip(range[2].Tooltip);
                    if (range[2].Active)
                    {
                        this.item3.setActive(true);
                    }
                }
                else if (range[2].type == PageItems.Toggle)
                {
                    this.toggleItem3.setOnText(range[2].onName);
                    this.toggleItem3.setOffText(range[2].offName);
                    this.toggleItem3.setAction(new Action(range[2].ButtonAction), new Action(range[2].ButtonAction));
                    this.toggleItem3.setToggleState(range[2].ToggleState, false);
                    this.toggleItem3.setToolTip(range[2].Tooltip);
                    if (range[2].Active)
                    {
                        this.toggleItem3.setActive(true);
                    }
                }
            }
            if (range.Count > 3)
            {
                if (range[3].type == PageItems.Button)
                {
                    this.item4.setButtonText(range[3].Name);
                    this.item4.setAction(range[3].Action);
                    this.item4.setToolTip(range[3].Tooltip);
                    if (range[3].Active)
                    {
                        this.item4.setActive(true);
                    }
                }
                else if (range[3].type == PageItems.Toggle)
                {
                    this.toggleItem4.setOnText(range[3].onName);
                    this.toggleItem4.setOffText(range[3].offName);
                    this.toggleItem4.setAction(new Action(range[3].ButtonAction), new Action(range[3].ButtonAction));
                    this.toggleItem4.setToggleState(range[3].ToggleState, false);
                    this.toggleItem4.setToolTip(range[3].Tooltip);
                    if (range[3].Active)
                    {
                        this.toggleItem4.setActive(true);
                    }
                }
            }
            if (range.Count > 4)
            {
                if (range[4].type == PageItems.Button)
                {
                    this.item5.setButtonText(range[4].Name);
                    this.item5.setAction(range[4].Action);
                    this.item5.setToolTip(range[4].Tooltip);
                    if (range[4].Active)
                    {
                        this.item5.setActive(true);
                    }
                }
                else if (range[4].type == PageItems.Toggle)
                {
                    this.toggleItem5.setOnText(range[4].onName);
                    this.toggleItem5.setOffText(range[4].offName);
                    this.toggleItem5.setAction(new Action(range[4].ButtonAction), new Action(range[4].ButtonAction));
                    this.toggleItem5.setToggleState(range[4].ToggleState, false);
                    this.toggleItem5.setToolTip(range[4].Tooltip);
                    if (range[4].Active)
                    {
                        this.toggleItem5.setActive(true);
                    }
                }
            }
            if (range.Count > 5)
            {
                if (range[5].type == PageItems.Button)
                {
                    this.item6.setButtonText(range[5].Name);
                    this.item6.setAction(range[5].Action);
                    this.item6.setToolTip(range[5].Tooltip);
                    if (range[5].Active)
                    {
                        this.item6.setActive(true);
                    }
                }
                else if (range[5].type == PageItems.Toggle)
                {
                    this.toggleItem6.setOnText(range[5].onName);
                    this.toggleItem6.setOffText(range[5].offName);
                    this.toggleItem6.setAction(new Action(range[5].ButtonAction), new Action(range[5].ButtonAction));
                    this.toggleItem6.setToggleState(range[5].ToggleState, false);
                    this.toggleItem6.setToolTip(range[5].Tooltip);
                    if (range[5].Active)
                    {
                        this.toggleItem6.setActive(true);
                    }
                }
            }
            if (range.Count > 6)
            {
                if (range[6].type == PageItems.Button)
                {
                    this.item7.setButtonText(range[6].Name);
                    this.item7.setAction(range[6].Action);
                    this.item7.setToolTip(range[6].Tooltip);
                    if (range[6].Active)
                    {
                        this.item7.setActive(true);
                    }
                }
                else if (range[6].type == PageItems.Toggle)
                {
                    this.toggleItem7.setOnText(range[6].onName);
                    this.toggleItem7.setOffText(range[6].offName);
                    this.toggleItem7.setAction(new Action(range[6].ButtonAction), new Action(range[6].ButtonAction));
                    this.toggleItem7.setToggleState(range[6].ToggleState, false);
                    this.toggleItem7.setToolTip(range[6].Tooltip);
                    if (range[6].Active)
                    {
                        this.toggleItem7.setActive(true);
                    }
                }
            }
            if (range.Count > 7)
            {
                if (range[7].type == PageItems.Button)
                {
                    this.item8.setButtonText(range[7].Name);
                    this.item8.setAction(range[7].Action);
                    this.item8.setToolTip(range[7].Tooltip);
                    if (range[7].Active)
                    {
                        this.item8.setActive(true);
                    }
                }
                else if (range[7].type == PageItems.Toggle)
                {
                    this.toggleItem8.setOnText(range[7].onName);
                    this.toggleItem8.setOffText(range[7].offName);
                    this.toggleItem8.setAction(new Action(range[7].ButtonAction), new Action(range[7].ButtonAction));
                    this.toggleItem8.setToggleState(range[7].ToggleState, false);
                    this.toggleItem8.setToolTip(range[7].Tooltip);
                    if (range[7].Active)
                    {
                        this.toggleItem8.setActive(true);
                    }
                }
            }
            if (range.Count > 8)
            {
                if (range[8].type == PageItems.Button)
                {
                    this.item9.setButtonText(range[8].Name);
                    this.item9.setAction(range[8].Action);
                    this.item9.setToolTip(range[8].Tooltip);
                    if (range[8].Active)
                    {
                        this.item9.setActive(true);
                    }
                }
                else if (range[8].type == PageItems.Toggle)
                {
                    this.toggleItem9.setOnText(range[8].onName);
                    this.toggleItem9.setOffText(range[8].offName);
                    this.toggleItem9.setAction(new Action(range[8].ButtonAction), new Action(range[8].ButtonAction));
                    this.toggleItem9.setToggleState(range[8].ToggleState, false);
                    this.toggleItem9.setToolTip(range[8].Tooltip);
                    if (range[8].Active)
                    {
                        this.toggleItem9.setActive(true);
                    }
                }
            }
            if ((this.currentPage + 1 > this.pageTitles.Count || this.pageTitles.Count <= 0) && this.menuTitle != null)
            {
                this.menuTitle.GetComponent<Text>().text = "";
                return;
            }
            if (this.menuTitle != null)
            {
                this.menuTitle.GetComponent<Text>().text = this.pageTitles[this.currentPage];
            }
        }

#pragma warning disable CS0649 // Le champ 'PaginatedMenu.menuName' n'est jamais assigné et aura toujours sa valeur par défaut null
        public string menuName;
#pragma warning restore CS0649 // Le champ 'PaginatedMenu.menuName' n'est jamais assigné et aura toujours sa valeur par défaut null

        public List<string> pageTitles = new List<string>();

        public List<PageItem> pageItems = new List<PageItem>();

        public QMNestedButton menuBase;

        private readonly QMSingleButton previousPageButton;

        private readonly QMSingleButton pageCount;

        private readonly QMSingleButton nextPageButton;

        private readonly QMSingleButton item1;

        private readonly QMSingleButton item2;

        private readonly QMSingleButton item3;

        private readonly QMSingleButton item4;

        private readonly QMSingleButton item5;

        private readonly QMSingleButton item6;

        private readonly QMSingleButton item7;

        private readonly QMSingleButton item8;

        private readonly QMSingleButton item9;

        private readonly QMToggleButton toggleItem1;

        private readonly QMToggleButton toggleItem2;

        private readonly QMToggleButton toggleItem3;

        private readonly QMToggleButton toggleItem4;

        private readonly QMToggleButton toggleItem5;

        private readonly QMToggleButton toggleItem6;

        private readonly QMToggleButton toggleItem7;

        private readonly QMToggleButton toggleItem8;

        private readonly QMToggleButton toggleItem9;

        private readonly GameObject menuTitle;
    }
}