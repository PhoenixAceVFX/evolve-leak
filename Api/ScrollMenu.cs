using ButtonApi;
using System;
using System.Collections.Generic;

namespace Evolve.Api
{
    internal class ScrollMenu
    {
        public ScrollMenu(QMNestedButton Base)
        {
            BaseMenu = Base;
        }

        public void SetAction(Action Open)
        {
            BaseMenu.getMainButton().setAction(delegate
            {
                Open();
                QMStuff.ShowQuickmenuPage(BaseMenu.getMenuName());
            });
        }

        public void Clear()
        {
            try
            {
                if (List2 != null)
                {
                    foreach (QMSingleButton qmsingleButton in List2)
                    {
                        UnityEngine.Object.Destroy(qmsingleButton.getGameObject());
                    }
                    Posx = 1;
                    Posy = 0;
                    Pos = 0;
                    if (NextPageScroll != null)
                    {
                        NextPageScroll.Clear();
                    }
                    if (Pages == 1)
                    {
                        UnityEngine.Object.Destroy(NextPage.getMainButton().getGameObject());
                        UnityEngine.Object.Destroy(NextPage.getBackButton().getGameObject());
                        NextPage = null;
                        NextPageScroll = null;
                    }
                    Pages = 0;
                    List2.Clear();
                }
            }
            catch (Exception value)
            {
                Console.WriteLine(value);
            }
        }

        public void Add(QMSingleButton Button)
        {
            if (Pages == 0)
            {
                if (Posx < 6)
                {
                    Posx++;
                }
                if (Posx > 5 && Posy < 3)
                {
                    Posx = 2;
                    Posy++;
                }
                Button.getGameObject().transform.SetParent(QMStuff.GetQuickMenuInstance().transform.Find(BaseMenu.getMenuName()));
                Button.setLocation(Posx, Posy);
                Pos++;
                if (Pos == 12)
                {
                    Pages = 1;
                }
            }
            else
            {
                if (Pages == 1)
                {
                    if (NextPage == null || NextPageScroll == null)
                    {
                        NextPage = new QMNestedButton(BaseMenu, 5f, 1f, "Next\nPage", "Go to Next Page");
                        NextPageScroll = new ScrollMenu(NextPage);
                        Pos = 0;
                    }
                    Button.getGameObject().transform.SetParent(QMStuff.GetQuickMenuInstance().transform.Find(NextPage.getMenuName()));
                    NextPageScroll.Add(Button);
                    Pos++;
                }
            }
            try
            {
                List2.Add(Button);
            }
            catch (Exception value)
            {
                Console.WriteLine(value);
            }
        }

        public void Add(QMToggleButton toggleButton)
        {
            try
            {
                List3.Add(toggleButton);
            }
            catch (Exception value)
            {
                Console.WriteLine(value);
            }
            if (Pages == 0)
            {
                if (Posx < 6)
                {
                    Posx++;
                }
                if (Posx > 5 && Posy < 3)
                {
                    Posx = 2;
                    Posy++;
                }
                toggleButton.getGameObject().transform.SetParent(QMStuff.GetQuickMenuInstance().transform.Find(BaseMenu.getMenuName()));
                toggleButton.setLocation(Posx, Posy);
                Pos++;
                if (Pos == 12)
                {
                    Pages = 1;
                }
            }
            else
            {
                if (Pages == 1)
                {
                    if (NextPage == null || NextPageScroll == null)
                    {
                        NextPage = new QMNestedButton(BaseMenu, 5f, 1f, "Next\nPage", "Go to Next Page");
                        NextPageScroll = new ScrollMenu(NextPage);
                        Pos = 0;
                    }
                    toggleButton.getGameObject().transform.SetParent(QMStuff.GetQuickMenuInstance().transform.Find(NextPage.getMenuName()));
                    NextPageScroll.Add(toggleButton);
                    Pos++;
                }
            }
        }

        public QMNestedButton BaseMenu;

        public ScrollMenu NextPageScroll;

        public QMNestedButton NextPage;

#pragma warning disable CS0649 // Le champ 'ScrollMenu.List' n'est jamais assigné et aura toujours sa valeur par défaut null
        public QMSingleButton[] List;
#pragma warning restore CS0649 // Le champ 'ScrollMenu.List' n'est jamais assigné et aura toujours sa valeur par défaut null

        public static List<QMSingleButton> List2 = new List<QMSingleButton>();

        public static List<QMToggleButton> List3 = new List<QMToggleButton>();

        public int Pos = 0;

        public int Pages = 0;

        public int Posx = 1;

        public int Posy = 0;
    }
}