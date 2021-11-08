using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ButtonApi;
using Evolve.Api;
using Evolve.Buttons;
using UnityEngine;
using VRC.Core;

namespace Evolve.Modules
{
    class EvolveMessageReader
    {
        public static IEnumerator CreateHandler()
        {
            while (VRCPlayer.field_Internal_Static_VRCPlayer_0 != true) yield return null;
            GameObject MessageObject = GameObject.Find($"Evolve message handler for {APIUser.CurrentUser.id}");
            GameObject StaffMessageObject = GameObject.Find($"Evolve Staff message handler for {APIUser.CurrentUser.id}");
            if (MessageObject == null)
            {
                MessageObject = new GameObject($"Evolve message handler for {APIUser.CurrentUser.id}");
            }
            if (StaffMessageObject == null)
            {
                StaffMessageObject = new GameObject($"Evolve Staff message handler for {APIUser.CurrentUser.id}");
            }
        }
        public static void GetMessage(string Sender, string Message)
        {
            var Decode = Convert.FromBase64String(Message);
            var String = Encoding.UTF8.GetString(Decode);
            Notifications.Notify($"New message from: <b><color=#00fff7>{Sender}</color></b>");
            Notifications.IconNotif();
            MessageMenu.NotifButton.setActive(true);
            MessageMenu.NotifButton.setButtonText($"Message\nfrom:\n<b><color=#00fff7>{Sender}</color></b>");
            MessageMenu.Message.text = $"Message from <b><color=#00fff7>{Sender}</color></b>\n\n{String}";
            var MicButton = GameObject.Find("/UserInterface/QuickMenu/MicControls");
            MicButton.GetComponent<RectTransform>().anchoredPosition -= new Vector2(400, 0);
        }

        public static void GetStaffMessage(string Sender, string Message)
        {
            var Decode = Convert.FromBase64String(Message);
            var String = Encoding.UTF8.GetString(Decode);
            Notifications.StaffNotify($"From: <b><color=#00fff7>{Sender}</color></b>\n{String}");
        }
    }
}
