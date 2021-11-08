using UnityEngine;
using VRCSDK2;

namespace Evolve.Modules.PortableMirror
{
    internal class PortableMirror
    {
        public static void ToggleMirror(bool State)
        {
            if (!State && _mirror != null)
            {
                UnityEngine.Object.Destroy(_mirror);
                _mirror = null;
            }
            else if (State)
            {
                VRCPlayer player = Utils.GetVRCPlayer();
                Vector3 pos = player.transform.position + player.transform.forward;
                pos.y += _mirrorScaleY / 1.7f;
                GameObject mirror = GameObject.CreatePrimitive(PrimitiveType.Quad);
                mirror.transform.position = pos;
                mirror.transform.rotation = player.transform.rotation;
                mirror.transform.localScale = new Vector3(_mirrorScaleX, _mirrorScaleY, 1f);
                mirror.name = "PortableMirror";
                UnityEngine.Object.Destroy(mirror.GetComponent<Collider>());
                mirror.GetOrAddComponent<BoxCollider>().size = new Vector3(1f, 1f, 0.05f);
                mirror.GetOrAddComponent<BoxCollider>().isTrigger = true;
                mirror.GetOrAddComponent<MeshRenderer>().material.shader = Shader.Find("FX/MirrorReflection");
                mirror.GetOrAddComponent<VRC_MirrorReflection>().m_ReflectLayers = new LayerMask
                {
                    value = _optimizedMirror ? 263680 : -1025
                };
                mirror.GetOrAddComponent<VRC_Pickup>().proximity = 0.3f;
                mirror.GetOrAddComponent<VRC_Pickup>().pickupable = _canPickupMirror;
                mirror.GetOrAddComponent<VRC_Pickup>().allowManipulationWhenEquipped = false;
                mirror.GetOrAddComponent<Rigidbody>().useGravity = false;
                mirror.GetOrAddComponent<Rigidbody>().isKinematic = true;
                _mirror = mirror;
            }
        }

        public static GameObject _mirror;
        public static float _mirrorScaleX = 3;
        public static float _mirrorScaleY = 2.5f;
        public static bool _optimizedMirror = false;
        public static bool _canPickupMirror = true;
    }
}