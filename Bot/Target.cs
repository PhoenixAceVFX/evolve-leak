using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Evolve.Bot
{
    class Target
    {
        public static VRCPlayer VRCPlayer;
        public static string FollowType = "";
        public static IEnumerator Postion()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.001f);
                if (VRCPlayer.field_Internal_Static_VRCPlayer_0 == true)
                {
                    try
                    {
                        switch (FollowType)
                        {
                            case "Orbit":
                                if (VRCPlayer != null) Server.SendVector3(new Vector3(VRCPlayer.transform.position.x, VRCPlayer.field_Private_VRCPlayerApi_0.GetBonePosition(HumanBodyBones.Chest).y, VRCPlayer.transform.position.z));
                                break;
                            case "Pickup":
                                if (BotMenu.SphereObject != null) Server.SendVector3(BotMenu.SphereObject.transform.position);
                                break;
                        }
                    }
                    catch { }
                }
            }
        }
    }
}
