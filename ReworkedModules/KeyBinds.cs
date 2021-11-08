

using Evolve.Buttons;
using Evolve.ConsoleUtils;
using Evolve.Utils;
using System.IO;
using System.Linq;
using UnityEngine;
using VRC.Core;
using static UnityEngine.UI.Button;

namespace Evolve.Modules
{
    class KeyBinds
    {
        public static void Initialize()
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.F))
            {
                if (!Settings.Fly)
                {
                    MovementsMenu.Fly.setToggleState(true, true);
                    MovementsMenu.NoCliping.setToggleState(true, true);
                }
                else
                {
                    MovementsMenu.Fly.setToggleState(false, true);
                    MovementsMenu.NoCliping.setToggleState(false, true);
                }
            }

            else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.S))
            {
                if (!Settings.Speed) MovementsMenu.SpeedHack.setToggleState(true, true);
                else MovementsMenu.SpeedHack.setToggleState(false, true);
            }

            else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.E))
            {
                if (!Settings.CapsuleEsp) LobbyMenu.EspButton.setToggleState(true, true);
                else LobbyMenu.EspButton.setToggleState(false, true);
            }

            else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.R))
            {
                if (!Settings.Rotator) MovementsMenu.Rotator.setToggleState(true, true);
                else MovementsMenu.Rotator.setToggleState(false, true);
            }

            else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Tab))
            {
                if (!ThirdPerson.IsFirst) ThirdPerson.First();
                else if (ThirdPerson.IsFirst && !ThirdPerson.IsSecond) ThirdPerson.Second();
                else if (ThirdPerson.IsSecond) ThirdPerson.Third();
            }
            
            if (ThirdPerson.Enabled)
            {
                float axis = Input.GetAxis("Mouse ScrollWheel");

                if (Input.GetKey(KeyCode.UpArrow))
                {
                    ThirdPerson.CameraObject1.transform.position += ThirdPerson.CameraObject1.transform.up * 0.008f;
                    ThirdPerson.CameraObject2.transform.position += ThirdPerson.CameraObject2.transform.up * 0.008f;
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    ThirdPerson.CameraObject1.transform.position -= ThirdPerson.CameraObject1.transform.up * 0.008f;
                    ThirdPerson.CameraObject2.transform.position -= ThirdPerson.CameraObject2.transform.up * 0.008f;
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    ThirdPerson.CameraObject1.transform.position -= ThirdPerson.CameraObject1.transform.right * 0.008f;
                    ThirdPerson.CameraObject2.transform.position += ThirdPerson.CameraObject2.transform.right * 0.008f;
                }
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    ThirdPerson.CameraObject1.transform.position += ThirdPerson.CameraObject1.transform.right * 0.008f;
                    ThirdPerson.CameraObject2.transform.position -= ThirdPerson.CameraObject2.transform.right * 0.008f;
                }


                if (axis > 0f)
                {
                    ThirdPerson.CameraObject1.transform.position += ThirdPerson.CameraObject1.transform.forward * 0.08f;
                    ThirdPerson.CameraObject2.transform.position += ThirdPerson.CameraObject2.transform.forward * 0.08f;
                }
                else if (axis < 0f)
                {
                    ThirdPerson.CameraObject1.transform.position -= ThirdPerson.CameraObject1.transform.forward * 0.08f;
                    ThirdPerson.CameraObject2.transform.position -= ThirdPerson.CameraObject2.transform.forward * 0.08f;
                }
            }
        }
    }
}
