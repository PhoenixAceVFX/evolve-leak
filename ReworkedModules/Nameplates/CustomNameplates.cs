using Evolve.ConsoleUtils;
using Evolve.Patch;
using Evolve.Utils;
using Evolve.Wrappers;
using Harmony;
using Il2CppSystem.Text;
using MelonLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using TMPro;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.UI;
using VRC;
using VRC.Core;
using VRC.SDKBase;

namespace Evolve.Modules
{
    internal class CustomNameplates
    {
        #region Variables
#pragma warning disable CS0649               
        public static CustomNameplates instance;
#pragma warning restore CS0649               
#pragma warning disable CS0649               
        public static HarmonyInstance harmony;
        static Sprite nameplateBGBackup;
        static AssetBundle bundle;
        static Material npUIMaterial;
        static Sprite nameplateOutline;
        public static Color PlateUser = new Color(ColorManager.UserR, ColorManager.UserG, ColorManager.UserB);
        public static Color PlateLegend = new Color(0.30f, 1.00f, 1.00f);
        public static Color PlateKnown = new Color(ColorManager.KnownR, ColorManager.KnownG, ColorManager.KnownB);
        public static Color PlateNegative = new Color(ColorManager.NuisanceR, ColorManager.NuisanceG, ColorManager.NuisanceB);
        public static Color PlateNewUser = new Color(ColorManager.NewUserR, ColorManager.NewUserG, ColorManager.NewUserB);
        public static Color PlateVisitor = new Color(ColorManager.VisitorsR, ColorManager.VisitorsG, ColorManager.VisitorsB);
        public static Color PlateTrusted = new Color(ColorManager.TrustedR, ColorManager.TrustedG, ColorManager.TrustedB);
        public static Color PlateFriend = new Color(ColorManager.FriendR, ColorManager.FriendG, 0);
        public static Color PlateVeteran = new Color(0.90f, 0.00f, 0.75f);
        static List<string> hiddenNameplateUserIDs = new List<string>();
        public static Texture2D EvolveIcon;
        #endregion

        public static void Initialize()
        {
            ClassInjector.RegisterTypeInIl2Cpp<Nameplate>();
            LoadCustomAssets();
            MelonCoroutines.Start(Update());
        }

        public static List<string> Tags = new List<string>()
        {
            "Admin",
            "VIP",
            "Standard"
        };

        public static List<GameObject> AllNameplates = new List<GameObject>();


        public static IEnumerator Update()
        {
            while (true)
            {
                if (VRCPlayer.field_Internal_Static_VRCPlayer_0 != true) yield return null; 
                yield return new WaitForSeconds(2);
                try
                {
                    foreach (var Player in Wrappers.Utils.PlayerManager.AllPlayers())
                    {
                        var Contents = Player._vrcplayer.field_Public_PlayerNameplate_0.gameObject.transform.Find("Contents");
                        var Stats = Contents.Find("Quick Stats");
                        var Rank = Player.prop_APIUser_0.GetRank().ToLower();
                        var plateColor = new Color();
                        bool ShouldStop = false;

                        if (Player._vrcplayer.IsEvolved())
                        {
                            var Tag = Player._vrcplayer.GetEvolveSubscription();
                            if (!Tags.Contains(Tag)) UpdateT(0, Stats, Contents, Color.black, $"<color=#ff0062>{Tag}</color>");
                            else UpdateT(0, Stats, Contents, Color.black, $"<color=#ff0062>Evolve {Tag}</color>");
                            ShouldStop = true;
                        }

                        if (!ShouldStop)
                        {
                            if (!APIUser.IsFriendsWith(Player.UserID()))
                            {
                                switch (Rank)
                                {
                                    case "user":
                                        plateColor = PlateUser;
                                        break;
                                    case "legend":
                                        plateColor = PlateLegend;
                                        break;
                                    case "known":
                                        plateColor = PlateKnown;
                                        break;
                                    case "negativetrust":
                                        plateColor = PlateNegative;
                                        break;
                                    case "new user":
                                        plateColor = PlateNewUser;
                                        break;
                                    case "verynegativetrust":
                                        plateColor = PlateNegative;
                                        break;
                                    case "visitor":
                                        plateColor = PlateVisitor;
                                        break;
                                    case "trusted":
                                        plateColor = PlateTrusted;
                                        break;
                                    case "veteran":
                                        plateColor = PlateVeteran;
                                        break;
                                }
                            }
                            else plateColor = PlateFriend;
                            if (Player.GetIsMaster()) UpdateT(0, Stats, Contents, plateColor, $"<color=#f2d600>[M]</color> F: {Player._vrcplayer.GetFramesColored()} P: {Player._vrcplayer.GetPingColored()}");
                            else UpdateT(0, Stats, Contents, plateColor, "F: " + Player._vrcplayer.GetFramesColored() + " P: " + Player._vrcplayer.GetPingColored());
                        }
                    }
                }
                catch { }
            }
        }
        public static void Update(VRCPlayer Player)
        {
            if (Player == null || Player.isActiveAndEnabled == false || Player.field_Internal_Animator_0 == null || Player.field_Internal_GameObject_0 == null || Player.field_Internal_GameObject_0.name.IndexOf("Avatar_Utility_Base_") == 0);
            var Nameplate = Player.field_Public_PlayerNameplate_0.GetComponent<Nameplate>();
            if (Nameplate == null)
            {
                Nameplate = Player.field_Public_PlayerNameplate_0.gameObject.AddComponent<Nameplate>();
                Nameplate.SN(Player.field_Public_PlayerNameplate_0);
                Nameplate.Content = Player.field_Public_PlayerNameplate_0.gameObject.transform.Find("Contents").gameObject;
                Nameplate.IconBackground = Player.field_Public_PlayerNameplate_0.gameObject.transform.Find("Contents/Icon/Background").GetComponent<Image>();
                Nameplate.Icon = Player.field_Public_PlayerNameplate_0.gameObject.transform.Find("Contents/Icon/User Image").GetComponent<RawImage>();
                Nameplate.IconContainer = Player.field_Public_PlayerNameplate_0.gameObject.transform.Find("Contents/Icon").gameObject;
                Nameplate.NameBackground = Player.field_Public_PlayerNameplate_0.gameObject.transform.Find("Contents/Main/Background").GetComponent<ImageThreeSlice>();
                Nameplate.QuickStatsBackground = Player.field_Public_PlayerNameplate_0.gameObject.transform.Find("Contents/Quick Stats").GetComponent<ImageThreeSlice>();
                Nameplate.QuickStats = Player.field_Public_PlayerNameplate_0.gameObject.transform.Find("Contents/Quick Stats").gameObject;
                Nameplate.Name = Player.field_Public_PlayerNameplate_0.gameObject.transform.Find("Contents/Main/Text Container/Name").GetComponent<TextMeshProUGUI>();
                Nameplate.IconPulse = Player.field_Public_PlayerNameplate_0.gameObject.transform.Find("Contents/Icon/Pulse").GetComponent<Image>();
                Nameplate.NamePulse = Player.field_Public_PlayerNameplate_0.gameObject.transform.Find("Contents/Main/Pulse").GetComponent<ImageThreeSlice>();
                Nameplate.IconGlow = Player.field_Public_PlayerNameplate_0.gameObject.transform.Find("Contents/Icon/Glow").GetComponent<Image>();
                Nameplate.NameGlow = Player.field_Public_PlayerNameplate_0.gameObject.transform.Find("Contents/Main/Glow").GetComponent<ImageThreeSlice>();
                Nameplate.IconText = Player.field_Public_PlayerNameplate_0.gameObject.transform.Find("Contents/Icon/Initials").GetComponent<TextMeshProUGUI>();
            }
            Reset(Player.field_Public_PlayerNameplate_0);

            var Image = Nameplate.NameBackground;
            if (Image != null)
            {
                if (nameplateBGBackup == null) nameplateBGBackup = Image._sprite;
                Image._sprite = nameplateOutline;
            }

            Color? plateColor = null;
            Color? textColor = null;
            bool resetMaterials = false;
            var Stats = Nameplate.QuickStats.transform;
            int num = 0;
            ST(ref num, Stats, Nameplate.Content.transform, Color.white, "F: " + Player.GetFramesColored() + " P: " + Player.GetPingColored());
            Stats.localPosition = new Vector3(0, 66, 0);

            if (Player._player.GetVRCPlayer().IsEvolved())
            {
                MelonCoroutines.Start(GetEvolveIcon());
                IEnumerator GetEvolveIcon()
                {
                    Color Red;
                    ColorUtility.TryParseHtmlString("#ff0062", out Red);
                    textColor = Red;
                    plateColor = Color.black;
                    SetIcon(EvolveIcon, Player.field_Public_PlayerNameplate_0, Nameplate, Player._player);
                    Nameplate.IconBackground.gameObject.transform.localScale -= new Vector3(0.3f, 0.3f, 0.3f);
                    Nameplate.IconBackground.gameObject.SetActive(false);
                    SetColor(Nameplate, plateColor, plateColor, textColor, null, resetMaterials, true);
                    WWW Request = new WWW("https://i.imgur.com/7SO12RH.png", null, new Il2CppSystem.Collections.Generic.Dictionary<string, string>());
                    yield return Request;
                    yield return new WaitForSeconds(0.5f);
                    EvolveIcon = Request.texture;
                }
                return;
            }

            var Rank = Player.GetAPIUser().GetRank().ToLower();
            if (!APIUser.IsFriendsWith(Player.UserID()))
            {
                switch (Rank)
                {
                    case "user":
                        plateColor = PlateUser;
                        textColor = PlateUser;
                        break;
                    case "legend":
                        plateColor = PlateLegend;
                        textColor = PlateLegend;
                        break;
                    case "known":
                        plateColor = PlateKnown;
                        textColor = PlateKnown;
                        break;
                    case "negativetrust":
                        plateColor = PlateNegative;
                        textColor = PlateNegative;
                        break;
                    case "new user":
                        plateColor = PlateNewUser;
                        textColor = PlateNewUser;
                        break;
                    case "verynegativetrust":
                        plateColor = PlateNegative;
                        textColor = PlateNegative;
                        break;
                    case "visitor":
                        plateColor = PlateVisitor;
                        textColor = PlateVisitor;
                        break;
                    case "trusted":
                        plateColor = PlateTrusted;
                        textColor = PlateTrusted;
                        break;
                    case "veteran":
                        plateColor = PlateVeteran;
                        textColor = PlateVeteran;
                        break;
                }
            }
            else
            {
                plateColor = PlateFriend;
                textColor = PlateFriend;
            }

            SetColor(Nameplate, plateColor, plateColor, textColor, null, resetMaterials);
        }

        #region Nameplate Functions

        internal static Transform MT(Transform STS, int I)
        {
            Transform transform = UnityEngine.Object.Instantiate<Transform>(STS, STS.parent, false);
            transform.name = string.Format("EvolveTags:{0}", I);
            transform.localPosition = new Vector3(0f, 33, 0f);
            transform.gameObject.active = true;
            Transform result = null;
            for (int i = transform.childCount; i > 0; i--)
            {
                Transform child = transform.GetChild(i - 1);
                if (child.name == "Trust Text") result = child;
                else UnityEngine.Object.Destroy(child.gameObject);
            }
            return result;
        }

        internal static void ST(ref int ID, Transform STS, Transform ObjectT, Color Color, string Text)
        {
            Transform transform = ObjectT.Find(string.Format("EvolveTags:{0}", ID));
            Transform transform2;
            if (transform == null) transform2 = CustomNameplates.MT(STS, ID);

            else
            {
                transform.gameObject.SetActive(true);
                transform2 = transform.Find("Trust Text");
            }
            TextMeshProUGUI component = transform2.GetComponent<TextMeshProUGUI>();
            component.color = Color;
            component.text = Text;
            ID++;
        }

        internal static void UpdateT(int ID, Transform STS, Transform ObjectT, Color Color, string Text)
        {
            Transform transform = ObjectT.Find(string.Format("EvolveTags:{0}", ID));
            Transform transform2;
            if (transform == null) transform2 = MT(STS, ID);

            else
            {
                transform.gameObject.SetActive(true);
                transform2 = transform.Find("Trust Text");
            }
            TextMeshProUGUI component = transform2.GetComponent<TextMeshProUGUI>();
            var Background = transform.GetComponent<ImageThreeSlice>();
            Background.color = Color;
            component.color = Color;
            component.richText = true;
            component.text = Text;
        }


        public static void Reset(PlayerNameplate nameplate)
        {
            var Nameplate = nameplate.gameObject.GetComponent<Nameplate>();
            if (Nameplate != null) Nameplate.Reset();
            if (nameplateBGBackup != null && Nameplate != null)
            {
                ImageThreeSlice bgImage = Nameplate.NameBackground.GetComponent<ImageThreeSlice>();
                if (bgImage != null) bgImage._sprite = nameplateBGBackup;
            }
            SetColor(Nameplate, Color.white, Color.white, null, null, true);
        }

        public static void Refresh(PlayerNameplate nameplate)
        {
            Update(nameplate.field_Private_VRCPlayer_0);
            Nameplate helper = nameplate.gameObject.GetComponent<Nameplate>();
            if (helper != null) helper.Reset();
        }

        private static void SetColor(Nameplate Nameplate, Color? Color = null, Color? IconColor = null, Color? TextColor = null, Color? Lerp = null, bool Reset = false, bool IsEvolveUser = false)
        {
            if (Nameplate == null) return;


            if (!Reset)
            {
                Nameplate.NameBackground.material = npUIMaterial;
                Nameplate.QuickStatsBackground.material = npUIMaterial;
                Nameplate.IconBackground.material = npUIMaterial;
            }
            else
            {
                Nameplate.NameBackground.material = null;
                Nameplate.QuickStatsBackground.material = null;
                Nameplate.IconBackground.material = null;
            }

            Color oldBGColor = Nameplate.NameBackground.color;
            Color oldIconBGColor = Nameplate.IconBackground.color;
            Color oldQSBGColor = Nameplate.QuickStatsBackground.color;
            Color oldTextColor = Nameplate.Name.faceColor;

            if (Color.HasValue)
            {
                Color bgColor2 = Color.Value;
                Color quickStatsBGColor = Color.Value;
                bgColor2.a = oldBGColor.a;
                quickStatsBGColor.a = oldQSBGColor.a;
                Nameplate.NameBackground.color = bgColor2 / 2;
                Nameplate.QuickStatsBackground.color = quickStatsBGColor;
            }

            if (IconColor.HasValue)
            {
                Color iconBGColor2 = Color.Value;
                iconBGColor2.a = oldIconBGColor.a;
                Nameplate.IconBackground.color = iconBGColor2;
            }

            if (TextColor.HasValue && !Lerp.HasValue)
            {
                Color textColor2 = TextColor.Value;

                textColor2.a = oldTextColor.a;

                Nameplate.SNC(textColor2);
                Nameplate.ORbld();
            }

            if (TextColor.HasValue && Lerp.HasValue)
            {
                Color textColor2 = TextColor.Value;
                Color textColorLerp2 = Lerp.Value;
                textColor2.a = oldTextColor.a;
                textColorLerp2.a = oldTextColor.a;
            }

            if (IsEvolveUser)
            {
                Nameplate.IsEvolved = true;
                Color Red;
                ColorUtility.TryParseHtmlString("#ff0062", out Red);
                Nameplate.IconPulse.color = Red;
                Nameplate.IconGlow.color = UnityEngine.Color.black * 2;
                Nameplate.NamePulse.color = Red;
                Nameplate.NameGlow.color = UnityEngine.Color.black * 2;
            }
        }


        #endregion

        internal static void LoadCustomAssets()
        {
            using (var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Evolve.Assets"))
            {
                using (var tempStream = new MemoryStream((int)assetStream.Length))
                {
                    assetStream.CopyTo(tempStream);

                    bundle = AssetBundle.LoadFromMemory_Internal(tempStream.ToArray(), 0);
                    bundle.hideFlags |= HideFlags.DontUnloadUnusedAsset;
                }
            }

            if (bundle != null)
            {
                npUIMaterial = bundle.LoadAsset_Internal("NameplateMat", Il2CppType.Of<Material>()).Cast<Material>();
                npUIMaterial.hideFlags |= HideFlags.DontUnloadUnusedAsset;
                nameplateOutline = bundle.LoadAsset_Internal("NameplateOutline", Il2CppType.Of<Sprite>()).Cast<Sprite>();
                nameplateOutline.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            }
        }

        private static void SetIcon(Texture texture, PlayerNameplate nameplate, Nameplate helper, Player player)
        {
            helper.IconBackground.enabled = true;
            helper.Icon.enabled = true;
            helper.IconContainer.SetActive(true);
            helper.Icon.texture = texture;
        }
    }
}
