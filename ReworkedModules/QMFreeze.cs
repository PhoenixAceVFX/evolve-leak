using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnhollowerBaseLib.Attributes;
using UnityEngine;
using VRC.SDKBase;

namespace Evolve.Modules
{
    internal class EnableDisableListener : MonoBehaviour
    {
#nullable enable

        [method: HideFromIl2Cpp]
        public event Action? OnEnabled;

        [method: HideFromIl2Cpp]
        public event Action? OnDisabled;

#nullable disable

        public EnableDisableListener(IntPtr obj0) : base(obj0)
        {
        }

        private void OnEnable()
        {
            OnEnabled?.Invoke();
        }

        private void OnDisable()
        {
            OnDisabled?.Invoke();
        }
    }

    class QMFreeze
    {
        public static bool Frozen;
        private static Vector3 _originalGravity;
        private static Vector3 _originalVelocity;
        public static void Apply()
        {
            if (Frozen) Unfreeze();
        }

        public static void OnUI()
        {
            EnableDisableListener listener = GameObject.Find("/UserInterface/QuickMenu/MicControls").AddComponent<EnableDisableListener>();
            listener.OnEnabled += delegate { Freeze(); };
            listener.OnDisabled += delegate { Unfreeze(); };
        }

        public static void Freeze()
        {
            _originalGravity = Physics.gravity;
            _originalVelocity = Networking.LocalPlayer.GetVelocity();
            if (_originalVelocity == Vector3.zero) return;
            Physics.gravity = Vector3.zero;
            Networking.LocalPlayer.SetVelocity(Vector3.zero);
            Frozen = true;
        }

        public static void Unfreeze()
        {
            if (!Frozen) return;
            Physics.gravity = _originalGravity;
            Frozen = false;
        }
    }
}
