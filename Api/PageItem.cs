using System;
using UnityEngine;

namespace Evolve.Api
{
    internal class PageItem
    {
        public PageItem()
        {
        }

        public enum PageItems
        {
            Button = 1,
            Toggle
        }

        public PageItem(string name, Action action, string tooltip, bool active = true)
        {
            this.Name = name;
            this.Action = action;
            this.Tooltip = tooltip;
            this.Active = active;
            this.type = PageItems.Button;
        }

        public PageItem(string onName, Action onAction, string offName, Action offAction, string tooltip, bool active = true, bool defaultState = true)
        {
            this.onName = onName;
            this.offName = offName;
            this.OnAction = onAction;
            this.OffAction = offAction;
            this.Tooltip = tooltip;
            this.Active = active;
            this.ToggleState = defaultState;
            this.type = PageItems.Toggle;
        }

        public static PageItem Space
        {
            get
            {
                return new PageItem("", null, "", false);
            }
        }

        public void SetToggleState(bool newBool, bool triggerAction = false)
        {
            if (this.type == PageItems.Button)
            {
                return;
            }
            this.ToggleState = newBool;
            if (triggerAction)
            {
                this.ButtonAction();
            }
        }

        public void ButtonAction()
        {
            this.SetToggleState(!this.ToggleState, false);
            if (this.ToggleState)
            {
                this.OnAction();
                return;
            }
            if (!this.ToggleState)
            {
                this.OffAction();
            }
        }

        public string Name = "";

        public Action Action;

        public string Tooltip = "";

#pragma warning disable CS0649 // Le champ 'PageItem.buttonSprite' n'est jamais assigné et aura toujours sa valeur par défaut null
        public Sprite buttonSprite;
#pragma warning restore CS0649 // Le champ 'PageItem.buttonSprite' n'est jamais assigné et aura toujours sa valeur par défaut null

        public bool Active = true;

        public string onName = "";

        public string offName = "";

        public Action OnAction;

        public Action OffAction;

        public bool ToggleState = true;

        public PageItems type = PageItems.Button;
    }
}