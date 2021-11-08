using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Evolve.Protections
{
    class Toggles
    {
        public static void Chairs(bool State)
        {
            var Chairs = Resources.FindObjectsOfTypeAll<VRC.SDKBase.VRCStation>();
            if (Chairs != null)
            {
                foreach (var Chair in Chairs)
                {
                    Chair.gameObject.SetActive(State);
                }
            }
        }

        public static void Pickups(bool State)
        {
            foreach (var Pickup in Exploits.Exploits.AllPickups)
            {
                if (!Pickup.name.Contains("ViewFinder") && !Pickup.name.Contains("AvatarDebugConsole")) Pickup.gameObject.SetActive(State);
            }

            foreach (var Pickup in Exploits.Exploits.AllUdonPickups)
            {
                if (!Pickup.name.Contains("ViewFinder") && !Pickup.name.Contains("AvatarDebugConsole")) Pickup.gameObject.SetActive(State);
            }

            foreach (var Pickup in Exploits.Exploits.AllSyncPickups)
            {
                if (!Pickup.name.Contains("ViewFinder") && !Pickup.name.Contains("AvatarDebugConsole")) Pickup.gameObject.SetActive(State);
            }
        }
    }
}
