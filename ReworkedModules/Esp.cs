using Evolve.ConsoleUtils;
using Evolve.Utils;
using Evolve.Wrappers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRC;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRCSDK2;
using VRC_Pickup = VRCSDK2.VRC_Pickup;

namespace Evolve.Modules
{
    internal class Esp
    {
        public static List<Renderer> PickupsRenderers = new List<Renderer>();
        public static List<Renderer> TriggersRenderers = new List<Renderer>();
        public static IEnumerator Loop()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.2f);
                try
                {
                    if (Settings.ItemEsp)
                    {
                        foreach (var Renderer in PickupsRenderers)
                        {
                            HighlightsFX.prop_HighlightsFX_0.Method_Public_Void_Renderer_Boolean_0(Renderer, true);
                        }
                    }

                    if (Settings.TriggersEsp)
                    {
                        foreach (var Renderer in TriggersRenderers)
                        {
                            HighlightsFX.prop_HighlightsFX_0.Method_Public_Void_Renderer_Boolean_0(Renderer, true);
                        }
                    }
                }
                catch { }
            }
        }
        public static void PlayerMeshEsp(Player Target, bool State)
        {
            try
            {
                var Renderer = Target._vrcplayer.field_Internal_GameObject_0.GetComponentsInChildren<Renderer>();
                foreach (var Mesh in Renderer) HighlightsFX.prop_HighlightsFX_0.Method_Public_Void_Renderer_Boolean_0(Mesh, State);
            }
            catch { }
        }

        public static void PlayerCapsuleEsp(Player Target, bool State)
        {
            try
            {
                var Renderer = Target.gameObject.transform.Find("SelectRegion").GetComponent<Renderer>();
                HighlightsFX.prop_HighlightsFX_0.Method_Public_Void_Renderer_Boolean_0(Renderer, State);
            }
            catch { }
        }

        public static void TriggersEsp(bool State)
        {
            try
            {
                TriggersRenderers.Clear();
                foreach (var Trigger in Exploits.Exploits.AllTriggers)
                {
                    var Renderer = Trigger.gameObject.GetComponent<Renderer>();
                    var ChildRenderers = Trigger.gameObject.GetComponentsInChildren<Renderer>();
                    HighlightsFX.prop_HighlightsFX_0.Method_Public_Void_Renderer_Boolean_0(Renderer, State);
                    foreach (var ChildRenderer in ChildRenderers)
                    {
                        HighlightsFX.prop_HighlightsFX_0.Method_Public_Void_Renderer_Boolean_0(ChildRenderer, State);
                        if (State) TriggersRenderers.Add(ChildRenderer);
                    }
                    if (State) TriggersRenderers.Add(Renderer);
                }
            }
            catch { }
        }

        public static void PickupsEsp(bool State)
        {
            try
            {
                PickupsRenderers.Clear();
                //Normal
                foreach (var Pickup in Exploits.Exploits.AllPickups)
                {
                    if (Pickup.name != "AvatarDebugConsole" && Pickup.name != "ViewFinder")
                    {
                        var Renderer = Pickup.GetComponent<Renderer>();
                        var ChildRenderers = Pickup.GetComponentsInChildren<Renderer>();
                        HighlightsFX.prop_HighlightsFX_0.Method_Public_Void_Renderer_Boolean_0(Renderer, State);
                        foreach (var ChildRenderer in ChildRenderers)
                        {
                            HighlightsFX.prop_HighlightsFX_0.Method_Public_Void_Renderer_Boolean_0(ChildRenderer, State);
                            if (State) PickupsRenderers.Add(ChildRenderer);
                        }
                        if (State) PickupsRenderers.Add(Renderer);
                    }
                }

                //Udon
                foreach (var Pickup in Exploits.Exploits.AllUdonPickups)
                {
                    if (Pickup.name != "AvatarDebugConsole" && Pickup.name != "ViewFinder")
                    {
                        var Renderer = Pickup.GetComponent<Renderer>();
                        var ChildRenderers = Pickup.GetComponentsInChildren<Renderer>();
                        HighlightsFX.prop_HighlightsFX_0.Method_Public_Void_Renderer_Boolean_0(Renderer, State);
                        foreach (var ChildRenderer in ChildRenderers)
                        {
                            HighlightsFX.prop_HighlightsFX_0.Method_Public_Void_Renderer_Boolean_0(ChildRenderer, State);
                            if (State) PickupsRenderers.Add(ChildRenderer);
                        }
                        if (State) PickupsRenderers.Add(Renderer);
                    }
                }

                //Sync
                foreach (var Pickup in Exploits.Exploits.AllSyncPickups)
                {
                    if (Pickup.name != "AvatarDebugConsole" && Pickup.name != "ViewFinder")
                    {
                        var Renderer = Pickup.GetComponent<Renderer>();
                        var ChildRenderers = Pickup.GetComponentsInChildren<Renderer>();
                        HighlightsFX.prop_HighlightsFX_0.Method_Public_Void_Renderer_Boolean_0(Renderer, State);
                        foreach (var ChildRenderer in ChildRenderers)
                        {
                            HighlightsFX.prop_HighlightsFX_0.Method_Public_Void_Renderer_Boolean_0(ChildRenderer, State);
                            if (State) PickupsRenderers.Add(ChildRenderer);
                        }
                        if (State) PickupsRenderers.Add(Renderer);
                    }
                }
            }
            catch { }
        }
    }
}