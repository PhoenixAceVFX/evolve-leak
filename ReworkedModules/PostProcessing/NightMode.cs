using UnityEngine;

namespace Evolve.Modules.PostProcessing
{
    internal class NightMode
    {
        public static GameObject PostProcessing = new GameObject();

#pragma warning disable CS0649 // Le champ 'NightMode.NightMode1Bool' n'est jamais assigné et aura toujours sa valeur par défaut false
        public static bool NightMode1Bool;
#pragma warning restore CS0649 // Le champ 'NightMode.NightMode1Bool' n'est jamais assigné et aura toujours sa valeur par défaut false
#pragma warning disable CS0649 // Le champ 'NightMode.NightMode2Bool' n'est jamais assigné et aura toujours sa valeur par défaut false
        public static bool NightMode2Bool;
#pragma warning restore CS0649 // Le champ 'NightMode.NightMode2Bool' n'est jamais assigné et aura toujours sa valeur par défaut false
#pragma warning disable CS0649 // Le champ 'NightMode.NightMode3Bool' n'est jamais assigné et aura toujours sa valeur par défaut false
        public static bool NightMode3Bool;
#pragma warning restore CS0649 // Le champ 'NightMode.NightMode3Bool' n'est jamais assigné et aura toujours sa valeur par défaut false
#pragma warning disable CS0649 // Le champ 'NightMode.NightMode3Float' n'est jamais assigné et aura toujours sa valeur par défaut 0
        public static float NightMode3Float;
#pragma warning restore CS0649 // Le champ 'NightMode.NightMode3Float' n'est jamais assigné et aura toujours sa valeur par défaut 0

        public static void ApplyNightMode()
        {
            GameObject PostProcessing = new GameObject();
            PostProcessing.layer = 8; // Setting the layer to 8 is the PostProcessing layer.
        }
    }
}