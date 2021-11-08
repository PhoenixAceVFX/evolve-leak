using UnityEngine;

namespace Evolve.Modules.PortableMirror
{
    internal static class Utils
    {
        public static VRCPlayer GetVRCPlayer()
        {
            return VRCPlayer.field_Internal_Static_VRCPlayer_0;
        }

        public static bool GetKey(KeyCode key, bool control = false, bool shift = false)
        {
            bool controlFlag = !control;
            bool shiftFlag = !shift;
            if (control && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
            {
                controlFlag = true;
            }
            if (shift && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
            {
                shiftFlag = true;
            }
            return controlFlag && shiftFlag && Input.GetKey(key);
        }

        public static bool GetKeyDown(KeyCode key, bool control = false, bool shift = false)
        {
            bool controlFlag = !control;
            bool shiftFlag = !shift;
            if (control && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
            {
                controlFlag = true;
            }
            if (shift && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
            {
                shiftFlag = true;
            }
            return controlFlag && shiftFlag && Input.GetKeyDown(key);
        }

        public static bool GetKeyUp(KeyCode key, bool control = false, bool shift = false)
        {
            bool controlFlag = !control;
            bool shiftFlag = !shift;
            if (control && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
            {
                controlFlag = true;
            }
            if (shift && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
            {
                shiftFlag = true;
            }
            return controlFlag && shiftFlag && Input.GetKeyUp(key);
        }

        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            if (component == null)
            {
                return gameObject.AddComponent<T>();
            }
            return component;
        }
    }
}