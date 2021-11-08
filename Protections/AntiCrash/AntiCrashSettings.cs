namespace Evolve.Protections
{
    internal static class AntiCrashSettings
    {
        public static bool AllowSpawnSounds = false;
        public static bool AllowGlobalSounds = false;
        public static bool AllowUiLayer = false;
        public static int MaxPolygons = 500000;
        public static int MaxTransforms = 1000;
        public static int MaxConstraints = 200;
        public static int MaxMaterialSlots = 70;
        public static int MaxAudioSources = 8;
        public static int MaxClothVertices = 5000;
        public static int MaxColliders = 10;
        public static int MaxRigidBodies = 32;
        public static int MaxAnimators = 64;
        public static int MaxLights = 2;
        public static int MaxComponents = 4000;
        public static int MaxDepth = 50;
        public static int MaxParticles = 10000;
        public static int MaxMeshParticleVertices = 50000;
        public static bool AllowReadingBadFloats = false;
        public static bool HeuristicallyRemoveScreenSpaceBullshit = true;
        public static bool HidePortalsFromBlockedUsers = true;
        public static bool HidePortalsFromNonFriends = false;
        public static bool HidePortalsCreatedTooClose = true;
        public static bool AllowCustomMixers = false;
        public static bool AllowReadingMixers = false;
        public static int MaxMaterialSlotsOverSubmeshCount = 5;
        public static bool EnforceDefaultSortingLayer = true;
        public static bool AllowNonDefaultSortingLayers = false;
    }
}