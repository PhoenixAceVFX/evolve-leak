using Evolve.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evolve.Buttons;
using UnityEngine;

namespace Evolve.Modules
{
    class SitOnHead
    {
        public static IEnumerator Attach(VRCPlayer Target)
        {
            while (Settings.AttachToPlayer)
            {
                try
                {
                    if (Target != null)
                    {
                        if (Input.GetAxis("Horizontal") == 0f && Input.GetAxis("Vertical") == 0f)
                        {
                            VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position = Target.field_Private_VRCPlayerApi_0.GetBonePosition(HumanBodyBones.Head);
                            MovementsMenu.Fly.setToggleState(true, true);
                            MovementsMenu.NoCliping.setToggleState(true, true);
                        }
                        else
                        {
                            MovementsMenu.Fly.setToggleState(false, true);
                            MovementsMenu.NoCliping.setToggleState(false, true);
                            Settings.AttachToPlayer = false;
                        }
                    }
                }
                catch { }
                yield return new WaitForEndOfFrame();
            }
        }

        public static IEnumerator AttachHand(VRCPlayer Target)
        {
            while (Settings.AttachToPlayerHand)
            {
                try
                {
                    if (Target != null)
                    {
                        if (Input.GetAxis("Horizontal") == 0f && Input.GetAxis("Vertical") == 0f)
                        {
                            VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position = Target.field_Private_VRCPlayerApi_0.GetBonePosition(HumanBodyBones.RightHand);
                            MovementsMenu.Fly.setToggleState(true, true);
                            MovementsMenu.NoCliping.setToggleState(true, true);
                        }
                        else
                        {
                            MovementsMenu.Fly.setToggleState(false, true);
                            MovementsMenu.NoCliping.setToggleState(false, true);
                            Settings.AttachToPlayerHand = false;
                        }
                    }
                }
                catch { }
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
