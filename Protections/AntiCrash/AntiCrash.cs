using Evolve.ConsoleUtils;
using Evolve.Utils;
using MelonLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using UnhollowerBaseLib;
using UnhollowerBaseLib.Runtime;
using UnityEngine;
using UnityEngine.Animations;
using VRC.Core;
using Object = UnityEngine.Object;
using ModerationManager = VRC.Management.ModerationManager;
using Evolve.Wrappers;
using Evolve.Api;
using Evolve.Exploits;
using Evolve.Protections.AntiCrash;
using Harmony;
using UnityEngine.SceneManagement;
using UnhollowerRuntimeLib.XrefScans;
using UnhollowerRuntimeLib;

namespace Evolve.Protections
{
    public class N5HF8S65K
    {
        private static List<object> ourPinnedDelegates = new List<object>();
        internal static bool CanReadAudioMixers = true;
        internal static bool CanReadBadFloats = true;
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void VoidDelegate(IntPtr thisPtr, IntPtr nativeMethodInfo);

        public static void Initialize()
        {
            ClassInjector.RegisterTypeInIl2Cpp<SOH>();
            var matchingMethods = typeof(AssetManagement).GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly).Where(Method => Method.Name.StartsWith("Method_Public_Static_Object_Object_Vector3_Quaternion_Boolean_Boolean_Boolean_") && Method.GetParameters().Length == 6).ToList();

            foreach (var matchingMethod in matchingMethods)
            {
                unsafe
                {
                    var originalMethodPointer = *(IntPtr*)(IntPtr)UnhollowerUtils.GetIl2CppMethodInfoPointerFieldForGeneratedMethod(matchingMethod).GetValue(null);

                    ObjectInstantiateDelegate originalInstantiateDelegate = null;

                    ObjectInstantiateDelegate replacement = (assetPtr, pos, rot, allowCustomShaders, isUI, validate, nativeMethodPointer) =>
                        OIPatch(assetPtr, pos, rot, allowCustomShaders, isUI, validate, nativeMethodPointer, originalInstantiateDelegate);

                    ourPinnedDelegates.Add(replacement);

                    MelonUtils.NativeHookAttach((IntPtr)(&originalMethodPointer), Marshal.GetFunctionPointerForDelegate(replacement));

                    originalInstantiateDelegate = Marshal.GetDelegateForFunctionPointer<ObjectInstantiateDelegate>(originalMethodPointer);
                }
            }

            foreach (var nestedType in typeof(VRCAvatarManager).GetNestedTypes())
            {
                var moveNext = nestedType.GetMethod("MoveNext");
                if (moveNext == null) continue;
                var avatarManagerField = nestedType.GetProperties().SingleOrDefault(it => it.PropertyType == typeof(VRCAvatarManager));
                if (avatarManagerField == null) continue;

                MelonDebug.Msg($"Patching UniTask type {nestedType.FullName}");

                var fieldOffset = (int)IL2CPP.il2cpp_field_get_offset((IntPtr)UnhollowerUtils.GetIl2CppFieldInfoPointerFieldForGeneratedFieldAccessor(avatarManagerField.GetMethod).GetValue(null));

                unsafe
                {
                    var originalMethodPointer = *(IntPtr*)(IntPtr)UnhollowerUtils.GetIl2CppMethodInfoPointerFieldForGeneratedMethod(moveNext).GetValue(null);
                    originalMethodPointer = XrefScannerLowLevel.JumpTargets(originalMethodPointer).First();
                    VoidDelegate originalDelegate = null;
                    void TaskMoveNextPatch(IntPtr taskPtr, IntPtr nativeMethodInfo)
                    {
                        var avatarManager = *(IntPtr*)(taskPtr + fieldOffset - 16);
                        using (new Cookie(new VRCAvatarManager(avatarManager)))
                            originalDelegate(taskPtr, nativeMethodInfo);
                    }
                    var patchDelegate = new VoidDelegate(TaskMoveNextPatch);
                    ourPinnedDelegates.Add(patchDelegate);
                    MelonUtils.NativeHookAttach((IntPtr)(&originalMethodPointer), Marshal.GetFunctionPointerForDelegate(patchDelegate));
                    originalDelegate = Marshal.GetDelegateForFunctionPointer<VoidDelegate>(originalMethodPointer);
                }
            }

            ReaderPatches.ApplyPatches();
            SceneManager.add_sceneLoaded(new Action<Scene, LoadSceneMode>((s, _) =>
            {
                if (s.buildIndex == -1)
                {
                    CanReadAudioMixers = false;
                    CanReadBadFloats = false;
                }
            }));

            SceneManager.add_sceneUnloaded(new Action<Scene>(s =>
            {
                if (s.buildIndex == -1)
                {
                    CanReadAudioMixers = true;
                    CanReadBadFloats = true;
                }
            }));

            OICXUISUDNR.Initialize(Patch.Patches.Instance);
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr ObjectInstantiateDelegate(IntPtr assetPtr, Vector3 pos, Quaternion rot, byte allowCustomShaders, byte isUI, byte validate, IntPtr nativeMethodPointer);

        private static readonly PriorityQueue<OPriority> ourBfsQueue = new PriorityQueue<OPriority>(OPriority.IsActiveDepthNumChildrenComparer);
        private static void Clean(VRCAvatarManager avatarManager, GameObject go)
        {
            try
            {
                var No = long.Parse("2173162573851") ^ -1822076715 ^ 0xCCA000000000000L ^ 0x22A7DA944CL ^ -7968319096167071744L ^ -3816365552469803008L ^ 0x48E877F1F3000000L ^ -3805541685128069120L ^ -6026324156323004416L ^ 0x1C2942AACD390000L ^ -2340264320232849408L ^ 0x4230000000000000L ^ 0x3D9CF2DDDA149800L ^ 0x316991CC0L ^ 0 ^ -2145479602233933824L ^ 0x6BBF276000000000L ^ 0 ^ 0x2B98CF5627BC0000L ^ 0x1D2E9628A000000L ^ 0x2FEED00000000000L ^ 0x537F000000000000L ^ -6972949015494792192L ^ 0x6A00000000000000L ^ -279495115381760L ^ 0 ^ 0 ^ 0x269D9 ^ 0 ^ 0 ^ 0x29C1BC8F49000000L ^ 0x26420C4E67000000L ^ 0 ^ 0xBF9CC200000000L ^ 0x50575A0000000000L ^ 0x5EF87EAFC7079000L ^ 0x67909B0000000000L ^ -2319916758049226752L ^ 0x490685E592FC6400L ^ 0xFDBF8 ^ 0 ^ 0x2000000000000000L ^ 0xC0000000u ^ 0x820000 ^ 0 ^ -115975173177344L ^ 0x4922390000000000L ^ 0x578E7ABDF9000000L ^ 0x259AD71700000000L ^ 0x63BB4A0000000L ^ 0x6100000000000000L ^ -5188146770730811392L ^ 0x57FD3E0000000000L ^ -4276730796141707264L ^ -6786001544839371008L ^ 0 ^ 0x5277C5E0D600000L ^ 0 ^ -5941371953334321152L ^ -7540573888590118912L ^ 0x116302D900000000L ^ 0 ^ 0x5400000000000000L ^ -3575858104132173824L ^ 0x436191A723B70000L ^ 0x6891E861A200000L ^ -4611686018427387904L ^ 0x1F2B028600000000L ^ -8745643619089645568L ^ 0 ^ 0x7E0F430000000000L ^ 0x4924000000000L ^ 0x46B7F4BF28D26D00L ^ 0 ^ 0 ^ 0 ^ 0xD894000000L ^ 0x4A4A44376C2A0000L ^ 0 ^ 0x2A52138F90000000L ^ 0 ^ 0x52BC0 ^ 0x39682E22000000L ^ 0xAF7DC00000000L ^ 0x120BCA6F0000L ^ 0x4F034386D1BC0000L ^ 0x160840000000000L ^ 0x789278100D000000L ^ 0x5C054D0E930L ^ -17592186044416L ^ -2830512365802356736L ^ 0x26657C8418F6EFL ^ 0 ^ 0x4C00000000000000L ^ 0x46B0D08388000000L ^ -1209379031510679552L ^ 0 ^ 0x53421C1372000000L ^ 0x355E000000000000L ^ 0 ^ 0x37D128F300000000L ^ 0x30000 ^ 0x14E20C1D62C42800L ^ 0 ^ -8515242394525958144L ^ 0x63286B0000000000L ^ 0x45F9710000000000L ^ 0x40B5B5E9A1380000L ^ 0 ^ 0 ^ 0x46E1980000000000L ^ 0x552E178000000000L ^ 0x7100000000000000L ^ 0 ^ 0 ^ 0x756A490948B0EA00L ^ -4858255253784494080L ^ 0x6800000000000000L ^ 0x1040000000000000L ^ 0x38A2279F00000000L ^ 0 ^ 0 ^ 0x6C1D620000000000L ^ 0x475FF9633600L ^ 0 ^ -977125538244067328L ^ -82522213862559L ^ 0 ^ 0x65C2000000000000L ^ 0 ^ -1326310090260611072L ^ long.MinValue ^ 0x4901000000000000L ^ -8134912768680656896L ^ 0x32119EEDDL ^ 0 ^ 0x1F200000000000L ^ 0 ^ 0 ^ long.MinValue ^ 0x7056860000000000L ^ -2132450025513418752L ^ 0x295ACC538000000L ^ 0 ^ 0x72BFEAFD6C800000L ^ 0x3673E879F51A0000L ^ 0x6C4D000000000000L ^ -1785995011112828928L ^ 0x557E698D8AA28000L ^ 0 ^ 0 ^ 0 ^ 0x2481BAB400000000L ^ 0 ^ 0x229F347CCL ^ -1 ^ 0x2100000000000000L ^ 0x5E23C7EE93780000L ^ 0x43C5F8DE88F89400L ^ -3668643526L ^ 0x4F7D959CAFA20000L ^ 0x1000000000000000L ^ 0x2DC9F0CB8FC60000L ^ 0x5360000000000L ^ 0x39CEA36000000000L ^ 0 ^ -6708111644968353792L ^ 0 ^ 0x3CB438B898220000L ^ 0 ^ 0x530CDCA00EF12100L ^ 0 ^ 0x1AFC460000000000L ^ -8556839292003942400L ^ -8523625244752084992L ^ 0 ^ 0 ^ 0 ^ 0 ^ -102346541645774848L ^ 0x6C00000000000L ^ 0 ^ 0 ^ 0x3A00000000000000L ^ 0x7A40000000000000L ^ 0 ^ 0x150A9A0000L ^ 0x90EBB9BBu ^ -8796093022208L ^ 0x21B39D0000000000L ^ 0 ^ 0x5ADD2ECF5D000000L ^ 0 ^ 0 ^ 0x1D11CC1A4000000L ^ -2864289363007635456L ^ 0x1439FCC7C0000000L ^ 0x887C00000000L ^ 0 ^ -8546673436703850496L ^ 0 ^ 0x55653E3DAF000000L ^ 0x1C1DCB20FEFA5200L ^ 0x3626000000000000L ^ 0x6900000000000000L ^ 0x2A00000000000000L ^ -1637459926619565056L ^ 0x64297DB3 ^ -34468790272L ^ -35184372088832L ^ 0x6D00000000000000L ^ -4611686018427387904L ^ 0x3A8074AC00000000L ^ -215993974035316736L ^ 0x61839C0000000000L ^ 0x35D21362DE1D0000L ^ -6812843387394195456L ^ 0x41A5000000000000L ^ 0x18669D6BFC296200L ^ -1191817827951050752L ^ 0x44BD2F0000000000L ^ 0x1C5D000000000000L ^ 0 ^ 0 ^ 0x169C369637000000L ^ 0 ^ 0x5410975660600000L ^ 0 ^ -1879048192 ^ 0x6100000000000000L ^ 0x188211800000L ^ 0x1537B10000000000L ^ 0 ^ long.MinValue ^ 0xCF193E135000000L ^ 0x3A64A65800000000L ^ 0x775C1B5379000000L ^ 0 ^ 0x24E8000000000000L ^ long.MinValue ^ 0x1A4000000000L ^ 0 ^ -9119789245425254400L ^ 0x3900000000000000L ^ 0 ^ -1029424358575046656L ^ -8285429028383358976L ^ -218296301002752L ^ 0x3F69013CC790000L ^ 0x57FD57248D000000L ^ 0x1A66CAA7640CEA00L ^ 0 ^ 0x2A62DD0000000000L ^ -63350767616L ^ -3381370963363233792L ^ 0x3BB4FD8A00000L ^ -506889355919360000L ^ -4098128227564781568L ^ 0x7720D516CD000000L ^ 0 ^ -204509162766336L ^ 0x38D2CAC662060000L ^ 0x312F2F2800000000L ^ 0 ^ 0xC27EFE ^ -2933944628392493056L ^ 0 ^ -1015596900344135680L ^ 0x7100000000000000L ^ -2483813344364L ^ 0x165CEB0000000000L ^ -8918899510332751872L ^ 0 ^ -1662046838755164160L ^ -6854478632857894912L ^ -1886209865282486272L ^ 0x227074C2C7E40000L ^ 0 ^ -2480561153L ^ 0x236665289F545100L ^ 0x22FD000000000000L ^ -7208292678583189504L ^ 0 ^ 0 ^ 0x8E0000000L ^ -2515566150608224256L ^ long.MinValue ^ 0x6D4B15EC00000000L ^ 0 ^ 0x5C ^ 0x340A8949C3C0000L ^ 0 ^ 0 ^ 0 ^ -6500890165223021312L ^ 0 ^ 0x98F000000000000L ^ 0x596420EC00000000L ^ -103691894492L ^ 0x296C025682D60000L ^ 0x6C1919D2CC000000L ^ -6869221376L ^ -6946316783670263808L ^ 0x5FA2C5C57L ^ -7250736839250100992L ^ 0x2EA767F00000000L ^ -9695232 ^ 0x27E4000000000000L ^ -5856632329836953600L ^ 0x4000000000000000L ^ 0x7C35D88248EE0000L ^ 0 ^ 0x448046F25E300000L ^ 0x87CCB44C000000L ^ -54980231168L ^ 0x3DA9000000000000L ^ 0x3C30000000000000L ^ -5619446959279439872L ^ -806879232 ^ -4375209654595092480L ^ -6649990085934579712L ^ 0x375D22D3B4BF0000L ^ 0x3528F41ECDE80000L ^ 0 ^ 0xB98F27648000000L ^ 0x5D088A0858000000L ^ -4942815240896118784L ^ 0x5F33650000000000L ^ long.MinValue ^ -525805904864L ^ 0x27DCB80000000000L ^ 0 ^ 0x11E8570498000000L ^ 0x747A000000000000L ^ -2377900603251621888L ^ 0x39F0000000L ^ -2738188573441261568L ^ 0x2701194F2CD00000L ^ 0 ^ 0x2CD384C7CB230000L ^ 0xA3FECE5527D5800L ^ -2738188573441261568L ^ 0x1D5C832A0C97CB00L ^ 0x4BA5E72EA5A02L ^ 0x4EC4DE60189B0000L ^ 0x164A6E88000L ^ 0x6A00000000L ^ 0x47EF3FE000000000L ^ 0x1F49000000000000L ^ 0x25FB040000000000L ^ 0x35E1F86C ^ 0x6B63AF0C6A9C0000L ^ 0x68CDA10000000000L ^ -4250999496747515904L ^ -7133701809754865664L ^ 0x4865130000000000L ^ 0x2FFEEB2A0000000L ^ -5764607523034234880L ^ 0x8F1AD523B000000L ^ 0x87560000000L ^ 0x1E94000000000000L ^ -239286650941734912L ^ -5024187596796854272L ^ 0x794EF20000000000L ^ 0x566C154F862E0000L ^ 0x4B9A019BDF98E100L ^ 0 ^ 0x3378586140L ^ -9066553134481932288L ^ 0x1E46BCE300000000L ^ 0x98F536900000000L;
                if (!Settings.AvatarCleaning) return;
                var vrcPlayer = avatarManager.field_Private_VRCPlayer_0;
                if (vrcPlayer == null) return;

                var start = Stopwatch.StartNew();
                var scannedObjects = 0;
                var destroyedObjects = 0;
                var seenTransforms = 0;
                var seenPolys = 0;
                var seenMaterials = 0;
                var seenAudioSources = 0;
                var seenConstraints = 0;
                var seenClothVertices = 0;
                var seenColliders = 0;
                var seenRigidbodies = 0;
                var seenAnimators = 0;
                var seenLights = 0;
                var seenComponents = 0;
                var seenParticles = 0;
                var seenMeshParticleVertices = 0;
                var animator = go.GetComponent<Animator>();
                var componentList = new Il2CppSystem.Collections.Generic.List<Component>();
                var audioSourcesList = new List<AudioSource>();
                var skinnedRendererListList = new List<SkinnedMeshRenderer>();

                void Bfs(OPriority objWithPriority)
                {
                    var obj = objWithPriority.GameObject;
                    if (obj == null) return;
                    scannedObjects++;
                    if (animator?.IsBoneTransform(obj.transform) != true && seenTransforms++ >= AntiCrashSettings.MaxTransforms)
                    {
                        Object.DestroyImmediate(obj, true);
                        destroyedObjects++;
                        return;
                    }
                    if (objWithPriority.Depth >= AntiCrashSettings.MaxDepth)
                    {
                        Object.DestroyImmediate(obj, true);
                        destroyedObjects++;
                        return;
                    }
                    if (!AntiCrashSettings.AllowUiLayer && (obj.layer == 12 || obj.layer == 5)) obj.layer = 9;
                    obj.GetComponents(componentList);

                    foreach (var component in componentList)
                    {
                        if (component == null) continue;

                        component.TryCast<AudioSource>()?.VisitAudioSource(ref scannedObjects, ref destroyedObjects, ref seenAudioSources, obj, audioSourcesList, objWithPriority.IsActiveInHierarchy);
                        component.TryCast<IConstraint>()?.VisitConstraint(ref scannedObjects, ref destroyedObjects, ref seenConstraints, obj);
                        component.TryCast<Cloth>()?.VisitCloth(ref scannedObjects, ref destroyedObjects, ref seenClothVertices, obj);
                        component.TryCast<Rigidbody>()?.VisitGeneric(ref scannedObjects, ref destroyedObjects, ref seenRigidbodies, AntiCrashSettings.MaxRigidBodies);
                        component.TryCast<Collider>()?.VisitCollider(ref scannedObjects, ref destroyedObjects, ref seenColliders, obj);
                        component.TryCast<Animator>()?.VisitGeneric(ref scannedObjects, ref destroyedObjects, ref seenAnimators, AntiCrashSettings.MaxAnimators);
                        component.TryCast<Light>()?.VisitGeneric(ref scannedObjects, ref destroyedObjects, ref seenLights, AntiCrashSettings.MaxLights);
                        component.TryCast<Renderer>()?.VisitRenderer(ref scannedObjects, ref destroyedObjects, ref seenPolys, ref seenMaterials, obj, skinnedRendererListList);
                        component.TryCast<ParticleSystem>()?.VisitParticleSystem(component.GetComponent<ParticleSystemRenderer>(), ref scannedObjects, ref destroyedObjects, ref seenParticles, ref seenMeshParticleVertices, obj);
                        if (ReferenceEquals(component.TryCast<Transform>(), null)) component.VisitGeneric(ref scannedObjects, ref destroyedObjects, ref seenComponents, AntiCrashSettings.MaxComponents);
                    }

                    foreach (var child in obj.transform) ourBfsQueue.Enqueue(new OPriority(child.Cast<Transform>().gameObject, objWithPriority.Depth + 1, objWithPriority.IsActiveInHierarchy));
                }

                Bfs(new OPriority(go, 0, true, true));
                while (ourBfsQueue.Count > 0) Bfs(ourBfsQueue.Dequeue());

                ComponentAdjustment.PostprocessSkinnedRenderers(skinnedRendererListList);

                if (!AntiCrashSettings.AllowSpawnSounds) MelonCoroutines.Start(CleanAudio(go, audioSourcesList));
                if (AntiCrashSettings.EnforceDefaultSortingLayer) go.AddComponent<SOH>();
                if (Settings.AvatarLogs && destroyedObjects != 0) EvoVrConsole.Log(EvoVrConsole.LogsType.Avatar, $"Destroyed <color=white>{destroyedObjects}</color> objects in <color=white>{start.ElapsedMilliseconds}ms</color> on <color=white>{vrcPlayer.field_Private_VRCPlayerApi_0.displayName}'s</color> avatar");

                if (destroyedObjects > 5000)
                {
                    if (Settings.Reflect && !vrcPlayer.IsEvolved())
                    {
                        if (Settings.CrashLogs) EvoVrConsole.Log(EvoVrConsole.LogsType.Crash, $"<color=white>{vrcPlayer.DisplayName()}</color> crashed the lobby");
                        Notifications.Notify($"<color=#00ffee>Reflect</color>\nCrashing <color=white>{vrcPlayer.DisplayName()}</color>...");
                        string CrashType = FoldersManager.Config.Ini.GetString("Crashers", "Type");
                        if (CrashType.Length < 1)
                        {
                            FoldersManager.Config.Ini.SetString("Crashers", "Type", "Material");
                            CrashType = FoldersManager.Config.Ini.GetString("Crashers", "Type");
                        }

                        if (CrashType == "Material") MelonCoroutines.Start(AvatarCrash.TargetCrash(vrcPlayer, Settings.MaterialCrash, 6));
                        else if (CrashType == "CCD-IK") MelonCoroutines.Start(AvatarCrash.TargetCrash(vrcPlayer, Settings.CCDIK, 6));
                        else if (CrashType == "Mesh-Poly") MelonCoroutines.Start(AvatarCrash.TargetCrash(vrcPlayer, Settings.MeshPolyCrash, 10));
                        else if (CrashType == "Audio") MelonCoroutines.Start(AvatarCrash.TargetCrash(vrcPlayer, Settings.AudioCrash, 15));
                        else if (CrashType == "Custom") MelonCoroutines.Start(AvatarCrash.TargetCrash(vrcPlayer, FoldersManager.Config.Ini.GetString("Crashers", "CustomID"), 10));
                        EvoVrConsole.Log(EvoVrConsole.LogsType.Crash, $"Crashing target with <color=white>{CrashType}</color>");
                    }
                    else if (Settings.CrashLogs)
                    {
                        EvoVrConsole.Log(EvoVrConsole.LogsType.Crash, $"<color=white>{vrcPlayer.DisplayName()}</color> crashed the lobby");
                        Notifications.Notify($"<color=#ff00e6>Crash</color>\n<color=white>{vrcPlayer.DisplayName()}</color> crashed the lobby.");
                    }
                }
                else if (destroyedObjects > 1000)
                {
                    if (Settings.CrashLogs)
                    {
                        EvoVrConsole.Log(EvoVrConsole.LogsType.Crash, $"Suspicion on <color=white>{vrcPlayer.DisplayName()}</color>");
                        Notifications.Notify($"<color=#ff00e6>Crash</color>\nSuspicion on <color=white>{vrcPlayer.DisplayName()}</color>");
                    }
                }
            }
            catch { }
        }

        private static IEnumerator CleanAudio(GameObject Object, List<AudioSource> Sources)
        {
            var No = long.Parse("2173162573851") ^ -1822076715 ^ 0xCCA000000000000L ^ 0x22A7DA944CL ^ -7968319096167071744L ^ -3816365552469803008L ^ 0x48E877F1F3000000L ^ -3805541685128069120L ^ -6026324156323004416L ^ 0x1C2942AACD390000L ^ -2340264320232849408L ^ 0x4230000000000000L ^ 0x3D9CF2DDDA149800L ^ 0x316991CC0L ^ 0 ^ -2145479602233933824L ^ 0x6BBF276000000000L ^ 0 ^ 0x2B98CF5627BC0000L ^ 0x1D2E9628A000000L ^ 0x2FEED00000000000L ^ 0x537F000000000000L ^ -6972949015494792192L ^ 0x6A00000000000000L ^ -279495115381760L ^ 0 ^ 0 ^ 0x269D9 ^ 0 ^ 0 ^ 0x29C1BC8F49000000L ^ 0x26420C4E67000000L ^ 0 ^ 0xBF9CC200000000L ^ 0x50575A0000000000L ^ 0x5EF87EAFC7079000L ^ 0x67909B0000000000L ^ -2319916758049226752L ^ 0x490685E592FC6400L ^ 0xFDBF8 ^ 0 ^ 0x2000000000000000L ^ 0xC0000000u ^ 0x820000 ^ 0 ^ -115975173177344L ^ 0x4922390000000000L ^ 0x578E7ABDF9000000L ^ 0x259AD71700000000L ^ 0x63BB4A0000000L ^ 0x6100000000000000L ^ -5188146770730811392L ^ 0x57FD3E0000000000L ^ -4276730796141707264L ^ -6786001544839371008L ^ 0 ^ 0x5277C5E0D600000L ^ 0 ^ -5941371953334321152L ^ -7540573888590118912L ^ 0x116302D900000000L ^ 0 ^ 0x5400000000000000L ^ -3575858104132173824L ^ 0x436191A723B70000L ^ 0x6891E861A200000L ^ -4611686018427387904L ^ 0x1F2B028600000000L ^ -8745643619089645568L ^ 0 ^ 0x7E0F430000000000L ^ 0x4924000000000L ^ 0x46B7F4BF28D26D00L ^ 0 ^ 0 ^ 0 ^ 0xD894000000L ^ 0x4A4A44376C2A0000L ^ 0 ^ 0x2A52138F90000000L ^ 0 ^ 0x52BC0 ^ 0x39682E22000000L ^ 0xAF7DC00000000L ^ 0x120BCA6F0000L ^ 0x4F034386D1BC0000L ^ 0x160840000000000L ^ 0x789278100D000000L ^ 0x5C054D0E930L ^ -17592186044416L ^ -2830512365802356736L ^ 0x26657C8418F6EFL ^ 0 ^ 0x4C00000000000000L ^ 0x46B0D08388000000L ^ -1209379031510679552L ^ 0 ^ 0x53421C1372000000L ^ 0x355E000000000000L ^ 0 ^ 0x37D128F300000000L ^ 0x30000 ^ 0x14E20C1D62C42800L ^ 0 ^ -8515242394525958144L ^ 0x63286B0000000000L ^ 0x45F9710000000000L ^ 0x40B5B5E9A1380000L ^ 0 ^ 0 ^ 0x46E1980000000000L ^ 0x552E178000000000L ^ 0x7100000000000000L ^ 0 ^ 0 ^ 0x756A490948B0EA00L ^ -4858255253784494080L ^ 0x6800000000000000L ^ 0x1040000000000000L ^ 0x38A2279F00000000L ^ 0 ^ 0 ^ 0x6C1D620000000000L ^ 0x475FF9633600L ^ 0 ^ -977125538244067328L ^ -82522213862559L ^ 0 ^ 0x65C2000000000000L ^ 0 ^ -1326310090260611072L ^ long.MinValue ^ 0x4901000000000000L ^ -8134912768680656896L ^ 0x32119EEDDL ^ 0 ^ 0x1F200000000000L ^ 0 ^ 0 ^ long.MinValue ^ 0x7056860000000000L ^ -2132450025513418752L ^ 0x295ACC538000000L ^ 0 ^ 0x72BFEAFD6C800000L ^ 0x3673E879F51A0000L ^ 0x6C4D000000000000L ^ -1785995011112828928L ^ 0x557E698D8AA28000L ^ 0 ^ 0 ^ 0 ^ 0x2481BAB400000000L ^ 0 ^ 0x229F347CCL ^ -1 ^ 0x2100000000000000L ^ 0x5E23C7EE93780000L ^ 0x43C5F8DE88F89400L ^ -3668643526L ^ 0x4F7D959CAFA20000L ^ 0x1000000000000000L ^ 0x2DC9F0CB8FC60000L ^ 0x5360000000000L ^ 0x39CEA36000000000L ^ 0 ^ -6708111644968353792L ^ 0 ^ 0x3CB438B898220000L ^ 0 ^ 0x530CDCA00EF12100L ^ 0 ^ 0x1AFC460000000000L ^ -8556839292003942400L ^ -8523625244752084992L ^ 0 ^ 0 ^ 0 ^ 0 ^ -102346541645774848L ^ 0x6C00000000000L ^ 0 ^ 0 ^ 0x3A00000000000000L ^ 0x7A40000000000000L ^ 0 ^ 0x150A9A0000L ^ 0x90EBB9BBu ^ -8796093022208L ^ 0x21B39D0000000000L ^ 0 ^ 0x5ADD2ECF5D000000L ^ 0 ^ 0 ^ 0x1D11CC1A4000000L ^ -2864289363007635456L ^ 0x1439FCC7C0000000L ^ 0x887C00000000L ^ 0 ^ -8546673436703850496L ^ 0 ^ 0x55653E3DAF000000L ^ 0x1C1DCB20FEFA5200L ^ 0x3626000000000000L ^ 0x6900000000000000L ^ 0x2A00000000000000L ^ -1637459926619565056L ^ 0x64297DB3 ^ -34468790272L ^ -35184372088832L ^ 0x6D00000000000000L ^ -4611686018427387904L ^ 0x3A8074AC00000000L ^ -215993974035316736L ^ 0x61839C0000000000L ^ 0x35D21362DE1D0000L ^ -6812843387394195456L ^ 0x41A5000000000000L ^ 0x18669D6BFC296200L ^ -1191817827951050752L ^ 0x44BD2F0000000000L ^ 0x1C5D000000000000L ^ 0 ^ 0 ^ 0x169C369637000000L ^ 0 ^ 0x5410975660600000L ^ 0 ^ -1879048192 ^ 0x6100000000000000L ^ 0x188211800000L ^ 0x1537B10000000000L ^ 0 ^ long.MinValue ^ 0xCF193E135000000L ^ 0x3A64A65800000000L ^ 0x775C1B5379000000L ^ 0 ^ 0x24E8000000000000L ^ long.MinValue ^ 0x1A4000000000L ^ 0 ^ -9119789245425254400L ^ 0x3900000000000000L ^ 0 ^ -1029424358575046656L ^ -8285429028383358976L ^ -218296301002752L ^ 0x3F69013CC790000L ^ 0x57FD57248D000000L ^ 0x1A66CAA7640CEA00L ^ 0 ^ 0x2A62DD0000000000L ^ -63350767616L ^ -3381370963363233792L ^ 0x3BB4FD8A00000L ^ -506889355919360000L ^ -4098128227564781568L ^ 0x7720D516CD000000L ^ 0 ^ -204509162766336L ^ 0x38D2CAC662060000L ^ 0x312F2F2800000000L ^ 0 ^ 0xC27EFE ^ -2933944628392493056L ^ 0 ^ -1015596900344135680L ^ 0x7100000000000000L ^ -2483813344364L ^ 0x165CEB0000000000L ^ -8918899510332751872L ^ 0 ^ -1662046838755164160L ^ -6854478632857894912L ^ -1886209865282486272L ^ 0x227074C2C7E40000L ^ 0 ^ -2480561153L ^ 0x236665289F545100L ^ 0x22FD000000000000L ^ -7208292678583189504L ^ 0 ^ 0 ^ 0x8E0000000L ^ -2515566150608224256L ^ long.MinValue ^ 0x6D4B15EC00000000L ^ 0 ^ 0x5C ^ 0x340A8949C3C0000L ^ 0 ^ 0 ^ 0 ^ -6500890165223021312L ^ 0 ^ 0x98F000000000000L ^ 0x596420EC00000000L ^ -103691894492L ^ 0x296C025682D60000L ^ 0x6C1919D2CC000000L ^ -6869221376L ^ -6946316783670263808L ^ 0x5FA2C5C57L ^ -7250736839250100992L ^ 0x2EA767F00000000L ^ -9695232 ^ 0x27E4000000000000L ^ -5856632329836953600L ^ 0x4000000000000000L ^ 0x7C35D88248EE0000L ^ 0 ^ 0x448046F25E300000L ^ 0x87CCB44C000000L ^ -54980231168L ^ 0x3DA9000000000000L ^ 0x3C30000000000000L ^ -5619446959279439872L ^ -806879232 ^ -4375209654595092480L ^ -6649990085934579712L ^ 0x375D22D3B4BF0000L ^ 0x3528F41ECDE80000L ^ 0 ^ 0xB98F27648000000L ^ 0x5D088A0858000000L ^ -4942815240896118784L ^ 0x5F33650000000000L ^ long.MinValue ^ -525805904864L ^ 0x27DCB80000000000L ^ 0 ^ 0x11E8570498000000L ^ 0x747A000000000000L ^ -2377900603251621888L ^ 0x39F0000000L ^ -2738188573441261568L ^ 0x2701194F2CD00000L ^ 0 ^ 0x2CD384C7CB230000L ^ 0xA3FECE5527D5800L ^ -2738188573441261568L ^ 0x1D5C832A0C97CB00L ^ 0x4BA5E72EA5A02L ^ 0x4EC4DE60189B0000L ^ 0x164A6E88000L ^ 0x6A00000000L ^ 0x47EF3FE000000000L ^ 0x1F49000000000000L ^ 0x25FB040000000000L ^ 0x35E1F86C ^ 0x6B63AF0C6A9C0000L ^ 0x68CDA10000000000L ^ -4250999496747515904L ^ -7133701809754865664L ^ 0x4865130000000000L ^ 0x2FFEEB2A0000000L ^ -5764607523034234880L ^ 0x8F1AD523B000000L ^ 0x87560000000L ^ 0x1E94000000000000L ^ -239286650941734912L ^ -5024187596796854272L ^ 0x794EF20000000000L ^ 0x566C154F862E0000L ^ 0x4B9A019BDF98E100L ^ 0 ^ 0x3378586140L ^ -9066553134481932288L ^ 0x1E46BCE300000000L ^ 0x98F536900000000L;
            if (Sources.Count == 0) yield break;
            var endTime = Time.time + 5f;
            while (Object != null && !Object.activeInHierarchy && Time.time < endTime) yield return null;
            if (Object == null || !Object.activeInHierarchy) yield break;
            foreach (var Source in Sources)
            {
                if (Source != null && Source.isPlaying)
                {
                    Source.Stop();
                    EvoVrConsole.Log(EvoVrConsole.LogsType.Avatar, "Removed spawning sound effects.");
                }
            }
        }


        private static IntPtr OIPatch(IntPtr assetPtr, Vector3 pos, Quaternion rot, byte allowCustomShaders, byte isUI, byte validate, IntPtr nativeMethodPointer, ObjectInstantiateDelegate originalInstantiateDelegate)
        {
            var No = long.Parse("2173162573851") ^ -1822076715 ^ 0xCCA000000000000L ^ 0x22A7DA944CL ^ -7968319096167071744L ^ -3816365552469803008L ^ 0x48E877F1F3000000L ^ -3805541685128069120L ^ -6026324156323004416L ^ 0x1C2942AACD390000L ^ -2340264320232849408L ^ 0x4230000000000000L ^ 0x3D9CF2DDDA149800L ^ 0x316991CC0L ^ 0 ^ -2145479602233933824L ^ 0x6BBF276000000000L ^ 0 ^ 0x2B98CF5627BC0000L ^ 0x1D2E9628A000000L ^ 0x2FEED00000000000L ^ 0x537F000000000000L ^ -6972949015494792192L ^ 0x6A00000000000000L ^ -279495115381760L ^ 0 ^ 0 ^ 0x269D9 ^ 0 ^ 0 ^ 0x29C1BC8F49000000L ^ 0x26420C4E67000000L ^ 0 ^ 0xBF9CC200000000L ^ 0x50575A0000000000L ^ 0x5EF87EAFC7079000L ^ 0x67909B0000000000L ^ -2319916758049226752L ^ 0x490685E592FC6400L ^ 0xFDBF8 ^ 0 ^ 0x2000000000000000L ^ 0xC0000000u ^ 0x820000 ^ 0 ^ -115975173177344L ^ 0x4922390000000000L ^ 0x578E7ABDF9000000L ^ 0x259AD71700000000L ^ 0x63BB4A0000000L ^ 0x6100000000000000L ^ -5188146770730811392L ^ 0x57FD3E0000000000L ^ -4276730796141707264L ^ -6786001544839371008L ^ 0 ^ 0x5277C5E0D600000L ^ 0 ^ -5941371953334321152L ^ -7540573888590118912L ^ 0x116302D900000000L ^ 0 ^ 0x5400000000000000L ^ -3575858104132173824L ^ 0x436191A723B70000L ^ 0x6891E861A200000L ^ -4611686018427387904L ^ 0x1F2B028600000000L ^ -8745643619089645568L ^ 0 ^ 0x7E0F430000000000L ^ 0x4924000000000L ^ 0x46B7F4BF28D26D00L ^ 0 ^ 0 ^ 0 ^ 0xD894000000L ^ 0x4A4A44376C2A0000L ^ 0 ^ 0x2A52138F90000000L ^ 0 ^ 0x52BC0 ^ 0x39682E22000000L ^ 0xAF7DC00000000L ^ 0x120BCA6F0000L ^ 0x4F034386D1BC0000L ^ 0x160840000000000L ^ 0x789278100D000000L ^ 0x5C054D0E930L ^ -17592186044416L ^ -2830512365802356736L ^ 0x26657C8418F6EFL ^ 0 ^ 0x4C00000000000000L ^ 0x46B0D08388000000L ^ -1209379031510679552L ^ 0 ^ 0x53421C1372000000L ^ 0x355E000000000000L ^ 0 ^ 0x37D128F300000000L ^ 0x30000 ^ 0x14E20C1D62C42800L ^ 0 ^ -8515242394525958144L ^ 0x63286B0000000000L ^ 0x45F9710000000000L ^ 0x40B5B5E9A1380000L ^ 0 ^ 0 ^ 0x46E1980000000000L ^ 0x552E178000000000L ^ 0x7100000000000000L ^ 0 ^ 0 ^ 0x756A490948B0EA00L ^ -4858255253784494080L ^ 0x6800000000000000L ^ 0x1040000000000000L ^ 0x38A2279F00000000L ^ 0 ^ 0 ^ 0x6C1D620000000000L ^ 0x475FF9633600L ^ 0 ^ -977125538244067328L ^ -82522213862559L ^ 0 ^ 0x65C2000000000000L ^ 0 ^ -1326310090260611072L ^ long.MinValue ^ 0x4901000000000000L ^ -8134912768680656896L ^ 0x32119EEDDL ^ 0 ^ 0x1F200000000000L ^ 0 ^ 0 ^ long.MinValue ^ 0x7056860000000000L ^ -2132450025513418752L ^ 0x295ACC538000000L ^ 0 ^ 0x72BFEAFD6C800000L ^ 0x3673E879F51A0000L ^ 0x6C4D000000000000L ^ -1785995011112828928L ^ 0x557E698D8AA28000L ^ 0 ^ 0 ^ 0 ^ 0x2481BAB400000000L ^ 0 ^ 0x229F347CCL ^ -1 ^ 0x2100000000000000L ^ 0x5E23C7EE93780000L ^ 0x43C5F8DE88F89400L ^ -3668643526L ^ 0x4F7D959CAFA20000L ^ 0x1000000000000000L ^ 0x2DC9F0CB8FC60000L ^ 0x5360000000000L ^ 0x39CEA36000000000L ^ 0 ^ -6708111644968353792L ^ 0 ^ 0x3CB438B898220000L ^ 0 ^ 0x530CDCA00EF12100L ^ 0 ^ 0x1AFC460000000000L ^ -8556839292003942400L ^ -8523625244752084992L ^ 0 ^ 0 ^ 0 ^ 0 ^ -102346541645774848L ^ 0x6C00000000000L ^ 0 ^ 0 ^ 0x3A00000000000000L ^ 0x7A40000000000000L ^ 0 ^ 0x150A9A0000L ^ 0x90EBB9BBu ^ -8796093022208L ^ 0x21B39D0000000000L ^ 0 ^ 0x5ADD2ECF5D000000L ^ 0 ^ 0 ^ 0x1D11CC1A4000000L ^ -2864289363007635456L ^ 0x1439FCC7C0000000L ^ 0x887C00000000L ^ 0 ^ -8546673436703850496L ^ 0 ^ 0x55653E3DAF000000L ^ 0x1C1DCB20FEFA5200L ^ 0x3626000000000000L ^ 0x6900000000000000L ^ 0x2A00000000000000L ^ -1637459926619565056L ^ 0x64297DB3 ^ -34468790272L ^ -35184372088832L ^ 0x6D00000000000000L ^ -4611686018427387904L ^ 0x3A8074AC00000000L ^ -215993974035316736L ^ 0x61839C0000000000L ^ 0x35D21362DE1D0000L ^ -6812843387394195456L ^ 0x41A5000000000000L ^ 0x18669D6BFC296200L ^ -1191817827951050752L ^ 0x44BD2F0000000000L ^ 0x1C5D000000000000L ^ 0 ^ 0 ^ 0x169C369637000000L ^ 0 ^ 0x5410975660600000L ^ 0 ^ -1879048192 ^ 0x6100000000000000L ^ 0x188211800000L ^ 0x1537B10000000000L ^ 0 ^ long.MinValue ^ 0xCF193E135000000L ^ 0x3A64A65800000000L ^ 0x775C1B5379000000L ^ 0 ^ 0x24E8000000000000L ^ long.MinValue ^ 0x1A4000000000L ^ 0 ^ -9119789245425254400L ^ 0x3900000000000000L ^ 0 ^ -1029424358575046656L ^ -8285429028383358976L ^ -218296301002752L ^ 0x3F69013CC790000L ^ 0x57FD57248D000000L ^ 0x1A66CAA7640CEA00L ^ 0 ^ 0x2A62DD0000000000L ^ -63350767616L ^ -3381370963363233792L ^ 0x3BB4FD8A00000L ^ -506889355919360000L ^ -4098128227564781568L ^ 0x7720D516CD000000L ^ 0 ^ -204509162766336L ^ 0x38D2CAC662060000L ^ 0x312F2F2800000000L ^ 0 ^ 0xC27EFE ^ -2933944628392493056L ^ 0 ^ -1015596900344135680L ^ 0x7100000000000000L ^ -2483813344364L ^ 0x165CEB0000000000L ^ -8918899510332751872L ^ 0 ^ -1662046838755164160L ^ -6854478632857894912L ^ -1886209865282486272L ^ 0x227074C2C7E40000L ^ 0 ^ -2480561153L ^ 0x236665289F545100L ^ 0x22FD000000000000L ^ -7208292678583189504L ^ 0 ^ 0 ^ 0x8E0000000L ^ -2515566150608224256L ^ long.MinValue ^ 0x6D4B15EC00000000L ^ 0 ^ 0x5C ^ 0x340A8949C3C0000L ^ 0 ^ 0 ^ 0 ^ -6500890165223021312L ^ 0 ^ 0x98F000000000000L ^ 0x596420EC00000000L ^ -103691894492L ^ 0x296C025682D60000L ^ 0x6C1919D2CC000000L ^ -6869221376L ^ -6946316783670263808L ^ 0x5FA2C5C57L ^ -7250736839250100992L ^ 0x2EA767F00000000L ^ -9695232 ^ 0x27E4000000000000L ^ -5856632329836953600L ^ 0x4000000000000000L ^ 0x7C35D88248EE0000L ^ 0 ^ 0x448046F25E300000L ^ 0x87CCB44C000000L ^ -54980231168L ^ 0x3DA9000000000000L ^ 0x3C30000000000000L ^ -5619446959279439872L ^ -806879232 ^ -4375209654595092480L ^ -6649990085934579712L ^ 0x375D22D3B4BF0000L ^ 0x3528F41ECDE80000L ^ 0 ^ 0xB98F27648000000L ^ 0x5D088A0858000000L ^ -4942815240896118784L ^ 0x5F33650000000000L ^ long.MinValue ^ -525805904864L ^ 0x27DCB80000000000L ^ 0 ^ 0x11E8570498000000L ^ 0x747A000000000000L ^ -2377900603251621888L ^ 0x39F0000000L ^ -2738188573441261568L ^ 0x2701194F2CD00000L ^ 0 ^ 0x2CD384C7CB230000L ^ 0xA3FECE5527D5800L ^ -2738188573441261568L ^ 0x1D5C832A0C97CB00L ^ 0x4BA5E72EA5A02L ^ 0x4EC4DE60189B0000L ^ 0x164A6E88000L ^ 0x6A00000000L ^ 0x47EF3FE000000000L ^ 0x1F49000000000000L ^ 0x25FB040000000000L ^ 0x35E1F86C ^ 0x6B63AF0C6A9C0000L ^ 0x68CDA10000000000L ^ -4250999496747515904L ^ -7133701809754865664L ^ 0x4865130000000000L ^ 0x2FFEEB2A0000000L ^ -5764607523034234880L ^ 0x8F1AD523B000000L ^ 0x87560000000L ^ 0x1E94000000000000L ^ -239286650941734912L ^ -5024187596796854272L ^ 0x794EF20000000000L ^ 0x566C154F862E0000L ^ 0x4B9A019BDF98E100L ^ 0 ^ 0x3378586140L ^ -9066553134481932288L ^ 0x1E46BCE300000000L ^ 0x98F536900000000L;
            if (Cookie.CurrentManager == null || assetPtr == IntPtr.Zero)
                return originalInstantiateDelegate(assetPtr, pos, rot, allowCustomShaders, isUI, validate, nativeMethodPointer);

            var avatarManager = Cookie.CurrentManager;
            var vrcPlayer = avatarManager.field_Private_VRCPlayer_0;
            if (vrcPlayer == null) return originalInstantiateDelegate(assetPtr, pos, rot, allowCustomShaders, isUI, validate, nativeMethodPointer);

            if (vrcPlayer == VRCPlayer.field_Internal_Static_VRCPlayer_0) // never apply to self
                return originalInstantiateDelegate(assetPtr, pos, rot, allowCustomShaders, isUI, validate, nativeMethodPointer);

            var go = new Object(assetPtr).TryCast<GameObject>();
            if (go == null)
                return originalInstantiateDelegate(assetPtr, pos, rot, allowCustomShaders, isUI, validate, nativeMethodPointer);

            var wasActive = go.activeSelf;
            go.SetActive(false);
            var result = originalInstantiateDelegate(assetPtr, pos, rot, allowCustomShaders, isUI, validate, nativeMethodPointer);
            go.SetActive(wasActive);
            if (result == IntPtr.Zero) return result;
            var instantiated = new GameObject(result);
            try
            {
                Clean(Cookie.CurrentManager, instantiated);
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"Exception when cleaning avatar: {ex}");
            }
            return result;
        }

        public static bool IsShown(string userId)
        {
            var moderationsDict = ModerationManager.prop_ModerationManager_0.field_Private_Dictionary_2_String_List_1_ApiPlayerModeration_0;
            if (!moderationsDict.ContainsKey(userId)) return false;
            foreach (var playerModeration in moderationsDict[userId])
            {
                if (playerModeration.moderationType == ApiPlayerModeration.ModerationType.ShowAvatar)return true;
            }
            return false;
        }

        private readonly struct Cookie : IDisposable
        {
            internal static VRCAvatarManager CurrentManager;
            private readonly VRCAvatarManager myLastManager;

            public Cookie(VRCAvatarManager avatarManager)
            {
                myLastManager = CurrentManager;
                CurrentManager = avatarManager;
            }
            public void Dispose()
            {
                CurrentManager = myLastManager;
            }
        }

        private readonly struct OPriority
        {
            public readonly GameObject GameObject;
            public readonly bool IsActive;
            public readonly bool IsActiveInHierarchy;
            public readonly int NumChildren;
            public readonly int Depth;

            public OPriority(GameObject go, int depth, bool parentActive, bool enforceActive = false)
            {
                GameObject = go;
                Depth = depth;
                IsActive = go.activeSelf || enforceActive;
                IsActiveInHierarchy = IsActive && parentActive;
                NumChildren = go.transform.childCount;
            }

            public int Priority => Depth + NumChildren;

            private sealed class IsActiveDepthNumChildrenRelationalComparer : IComparer<OPriority>
            {
                public int Compare(OPriority x, OPriority y)
                {
                    var isActiveComparison = -x.IsActive.CompareTo(y.IsActive);
                    if (isActiveComparison != 0) return isActiveComparison;
                    return x.Priority.CompareTo(y.Priority);
                }
            }

            public static IComparer<OPriority> IsActiveDepthNumChildrenComparer { get; } = new IsActiveDepthNumChildrenRelationalComparer();
        }
    }
}