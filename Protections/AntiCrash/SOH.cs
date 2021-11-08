using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnhollowerBaseLib;
using UnityEngine;

namespace Evolve.Protections
{
    internal class SOH : MonoBehaviour
    {
        private Renderer[] myRenderersToHammer;

        public SOH(IntPtr ptr) : base(ptr)
        {
        }

        private void Start()
        {
            myRenderersToHammer = gameObject.GetComponentsInChildren<Renderer>(true);
        }

        private void LateUpdate()
        {
            for (var i = 0; i < myRenderersToHammer.Length; i++)
            {
                if (ReferenceEquals(myRenderersToHammer[i], null)) continue;

                try
                {
                    myRenderersToHammer[i].sortingOrder = 0;
                }
                catch (Il2CppException) // this would imply a deleted renderer
                {
                    myRenderersToHammer[i] = null;
                }
            }
        }
    }
}
