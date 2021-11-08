using System;
using UnityEngine;

namespace Evolve.Modules
{
    internal class BoneDeleteHandler : MonoBehaviour
    {
        public BoneDeleteHandler(IntPtr ptr) : base(ptr)
        {
        }

        private void OnDestroy()
        {
            foreach (var bone in GetComponents<DynamicBone>())
            {
                SolverApi.DynamicBoneOnDestroyPatch(bone.Pointer);
            }
        }
    }
}