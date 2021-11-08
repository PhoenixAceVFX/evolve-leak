using Evolve.Utils;
using Evolve.Wrappers;
using MelonLoader;
using RealisticEyeMovements;
using RootMotion.FinalIK;
using ButtonApi;
using System.Collections;
using System.Diagnostics;
using Evolve.Modules;
using UnityEngine;
using UnityEngine.UI;
using VRC.Core;
using Evolve.Api;
using Evolve.Modules.PortableMirror;
using Evolve.Modules.NearClippingPlaneAdjuster;
using Evolve.Modules.AvatarHider;
using System.Windows.Forms;
using Button = UnityEngine.UI.Button;

namespace Evolve.Buttons
{
    internal class SelfMenu
    {
        public static QMNestedButton ThisMenu;
        public static QMSingleButton SSButton;
        public static QMSingleButton CopyUser;
        public static GameObject Capsule;
        public static QMSingleButton SelfInfos;
        public static Vector3 RotationSave;
        public static QMSingleButton NearPlane001;
        public static QMSingleButton NearPlane0001;
        public static QMSingleButton NearPlane00001;
        public static QMSingleButton Hider5;
        public static QMSingleButton Hider7;
        public static QMToggleButton AvatarHiderToggle;
        public static QMToggleButton HiderIgnoreFriends;
#pragma warning disable CS0649 // Le champ 'SelfMenu.TogglePostProcessing' n'est jamais assigné et aura toujours sa valeur par défaut null
        public static QMToggleButton TogglePostProcessing;
#pragma warning restore CS0649 // Le champ 'SelfMenu.TogglePostProcessing' n'est jamais assigné et aura toujours sa valeur par défaut null
        public static QMToggleButton MirrorToggle;
        public static QMToggleButton GrabableMirror;
        public static QMSingleButton CopyAvi;
        public static QMSingleButton InvisAvi;
        public static QMSingleButton Vrca;
        public static QMToggleButton Serialize;
        public static GameObject Slider1;
        public static GameObject Slider2;
        public static void Initialize()
        {
            ThisMenu = new QMNestedButton(EvolveMenu.ThisMenu, 1.5f, -0.25f, "Self", "Self Menu", Color.cyan, Color.magenta, Color.black, Color.yellow);

            ThisMenu.getMainButton().getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            Panels.PanelMenu(ThisMenu, 0, 0, "\nClipping \ndistance:", 1.1f, 2, "Clipping distance Menu");

            Panels.PanelMenu(ThisMenu, 5, 0, "\nAvatar \nHider", 1.1f, 2, "Avatar hider Menu");

            Panels.PanelMenu(ThisMenu, 0, 1.77f, "\nCustom\nMirror", 1.1f, 1.5f, "Mirror Menu");

            MelonCoroutines.Start(LoadSelfInfos());
            IEnumerator LoadSelfInfos()
            {
                WWW request1 = new WWW("https://i.imgur.com/Y5YQSsf.png", null, new Il2CppSystem.Collections.Generic.Dictionary<string, string>());
                yield return request1;
                SelfInfos = new QMSingleButton(ThisMenu, -2f, 1f, "", null, "SelfInfos");
                SelfInfos.getGameObject().name = "SelfInfos";
                UnityEngine.Component.Destroy(SelfInfos.getGameObject().GetComponent<Button>());
                SelfInfos.getGameObject().GetComponent<RectTransform>().sizeDelta *= new Vector2(2.4f, 4f);
                SelfInfos.getGameObject().GetComponent<Image>().sprite = Sprite.CreateSprite(request1.texture,
                    new Rect(0, 0, request1.texture.width, request1.texture.height), new Vector2(0, 0), 100 * 1000,
                    1000, SpriteMeshType.FullRect, Vector4.zero, false);
                SelfInfos.getGameObject().GetComponent<Image>().color = Color.white;
                SelfInfos.setActive(true);
            }

            MirrorToggle = new QMToggleButton(ThisMenu, 0, 2, "On", () =>
            {
                PortableMirror.ToggleMirror(true);
            }, "Off", () =>
            {
                PortableMirror.ToggleMirror(false);
            }, "Will spawn a mirror");

            GrabableMirror = new QMToggleButton(ThisMenu, 0, 2, "Can\nGrab", () =>
            {
                PortableMirror._canPickupMirror = true;
            }, "Off", () =>
            {
                PortableMirror._canPickupMirror = false;
            }, "Will make the mirror grabable", null, null, false, PortableMirror._canPickupMirror);

            MirrorToggle.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(2, 1);
            MirrorToggle.btnOn.gameObject.GetComponent<RectTransform>().sizeDelta /= new Vector2(2.7f, 1);
            MirrorToggle.btnOff.gameObject.GetComponent<RectTransform>().sizeDelta /= new Vector2(2.7f, 1);
            GrabableMirror.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(2, 1);
            GrabableMirror.btnOn.gameObject.GetComponent<RectTransform>().sizeDelta /= new Vector2(2.7f, 1);
            GrabableMirror.btnOff.gameObject.GetComponent<RectTransform>().sizeDelta /= new Vector2(2.7f, 1);

            MirrorToggle.getGameObject().GetComponent<RectTransform>().anchoredPosition -= new Vector2(105, 0);
            GrabableMirror.getGameObject().GetComponent<RectTransform>().anchoredPosition += new Vector2(105, 0);

            NearPlane001 = new QMSingleButton(ThisMenu, 0, -0.25f, "0.01f", () =>
            {
                NearClipping.ChangeNearClipPlane(.01f);
            }, "Will change the clipping plane value to 0.01f");

            NearPlane0001 = new QMSingleButton(ThisMenu, 0, 0.25f, "0.001f", () =>
            {
                NearClipping.ChangeNearClipPlane(.001f);
            }, "Will change the clipping plane value to 0.001f");

            NearPlane00001 = new QMSingleButton(ThisMenu, 0, 0.75f, "0.0001f", () =>
            {
                NearClipping.ChangeNearClipPlane(.0001f);
            }, "Will change the clipping plane value to 0.0001f");

            NearPlane001.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);
            NearPlane0001.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);
            NearPlane00001.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            AvatarHiderToggle = new QMToggleButton(ThisMenu, 5, 0, "On", () =>
            {
                Settings.m_HideAvatars = true;
                AviDistanceHide.UnHideAvatars();
            }, "Off", () =>
            {
                Settings.m_HideAvatars = false;
                AviDistanceHide.UnHideAvatars();
            }, "Will hide avatars by distance", null, null, false, Settings.m_HideAvatars);

            HiderIgnoreFriends = new QMToggleButton(ThisMenu, 5, 0, "Ignore\nFriends", () =>
            {
                Settings.m_IgnoreFriends = true;
                AviDistanceHide.UnHideAvatars();
            }, "Off", () =>
            {
                Settings.m_IgnoreFriends = false;
                AviDistanceHide.UnHideAvatars();
            }, "Won't affect your friends if enabled", null, null, false, Settings.m_IgnoreFriends);

            Hider5 = new QMSingleButton(ThisMenu, 5, 0.75f, "5", () =>
            {
                Settings.m_Distance = 5f;
                AviDistanceHide.UnHideAvatars();
            }, "Will change the distance to 5");

            Hider7 = new QMSingleButton(ThisMenu, 5, 0.75f, "7", () =>
            {
                Settings.m_Distance = 7f;
                AviDistanceHide.UnHideAvatars();
            }, "Will change the distance to 7");


            CopyUser = new QMSingleButton(ThisMenu, 2, 2, "Copy\nUserID", () =>
            {
                Clipboard.SetText(APIUser.CurrentUser.id);
                Notifications.Notify($"Copied your userID");
            }, "Copy your userID to the clipboard");

            AvatarHiderToggle.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(2, 1);
            AvatarHiderToggle.btnOn.gameObject.GetComponent<RectTransform>().sizeDelta /= new Vector2(2.7f, 1);
            AvatarHiderToggle.btnOff.gameObject.GetComponent<RectTransform>().sizeDelta /= new Vector2(2.7f, 1);
            HiderIgnoreFriends.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(2, 1);
            HiderIgnoreFriends.btnOn.gameObject.GetComponent<RectTransform>().sizeDelta /= new Vector2(2.7f, 1);
            HiderIgnoreFriends.btnOff.gameObject.GetComponent<RectTransform>().sizeDelta /= new Vector2(2.7f, 1);

            Hider5.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(2, 2);
            Hider7.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(2, 2);
            Hider5.getGameObject().GetComponent<RectTransform>().anchoredPosition -= new Vector2(105, 0);
            Hider7.getGameObject().GetComponent<RectTransform>().anchoredPosition += new Vector2(105, 0);
            AvatarHiderToggle.getGameObject().GetComponent<RectTransform>().anchoredPosition -= new Vector2(105, 0);
            HiderIgnoreFriends.getGameObject().GetComponent<RectTransform>().anchoredPosition += new Vector2(105, 0);

            SSButton = new QMSingleButton(EvolveMenu.ThisMenu, 2.5f, -0.25f, "User Interact", () =>
            {
                Wrappers.Utils.QuickMenu.SelectPlayer(Wrappers.Utils.LocalPlayer.prop_Player_0);
            }, "User Interact menu");
            SSButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            new QMToggleButton(ThisMenu, 4, 0, "Streamer Mode", () =>
            {
                Settings.StreamerMode = true;
                NameSpoofGenerator.GenerateNewName();
                MelonCoroutines.Start(StreamerMode.Loop());
            }, "Disabled", () =>
            {
                Settings.StreamerMode = false;
            }, "Will spoof your name", null, null, false, Settings.StreamerMode);


            new QMToggleButton(ThisMenu, 2, 0, "Loud-Mic", () =>
            {
                USpeaker.field_Internal_Static_Single_1 = float.MaxValue;
            }, "Disabled", () =>
            {
                USpeaker.field_Internal_Static_Single_1 = 1;
            }, "Your mic will be loud", null, null, false, Settings.LoudMic);

            new QMToggleButton(ThisMenu, 1, 0, "Force-Clone", () =>
            {
                Settings.ForceClone = true;
                FoldersManager.Config.Ini.SetBool("Self", "ForceClone", true);
            }, "Disabled", () =>
            {
                Settings.ForceClone = false;
                FoldersManager.Config.Ini.SetBool("Self", "ForceClone", false);
            }, "Enable Force-Clone", null, null, false, Settings.ForceClone);

            Serialize =  new QMToggleButton(ThisMenu, 3, 0, "Serialize", () =>
            {
                Settings.Serialize = true;
                Capsule = Object.Instantiate<GameObject>(Wrappers.Utils.LocalPlayer.prop_VRCAvatarManager_0.prop_GameObject_0, null, true);
                Animator component = Capsule.GetComponent<Animator>();
                if (component != null && component.isHuman)
                {
                    Transform boneTransform = component.GetBoneTransform((HumanBodyBones) 10);
                    if (boneTransform != null) boneTransform.localScale = Vector3.one;
                }
                Capsule.name = "Serialize Capsule";
                component.enabled = false;
                Capsule.GetComponent<FullBodyBipedIK>().enabled = false;
                Capsule.GetComponent<LimbIK>().enabled = false;
                Capsule.GetComponent<VRIK>().enabled = false;
                Capsule.GetComponent<LookTargetController>().enabled = false;
                Capsule.transform.position = Wrappers.Utils.LocalPlayer.transform.position;
                Capsule.transform.rotation = Wrappers.Utils.LocalPlayer.transform.rotation;

            }, "Disabled", () =>
            {
                Settings.Serialize = false;
                Object.Destroy(Capsule);
            }, "Enable Serialize, nobody will see you moving around", null, null, false, Settings.LoudMic);

            new QMToggleButton(ThisMenu, 1, 2, "Head Flipper", () =>
            {
                RotationSave = VRCVrCamera.field_Private_Static_VRCVrCamera_0.GetComponentInChildren<NeckMouseRotator>().field_Public_Vector3_0;
                VRCVrCamera.field_Private_Static_VRCVrCamera_0.GetComponentInChildren<NeckMouseRotator>().field_Public_Vector3_0 = new Vector3(float.MinValue, 0f, float.MaxValue);
            }, "Disabled", () =>
            {
                VRCVrCamera.field_Private_Static_VRCVrCamera_0.GetComponentInChildren<NeckMouseRotator>().field_Public_Vector3_0 = RotationSave;
            }, "Will spoof your name", null, null, false, Settings.StreamerMode);

            new QMToggleButton(ThisMenu, 2, 1, "Offline\nSpoof", () =>
            {
                Settings.OfflineSpoof = true;
                FoldersManager.Config.Ini.SetBool("Self", "OfflineSpoof", true);
                if(VRCPlayer.field_Internal_Static_VRCPlayer_0 == true) Wrappers.Utils.VRCUiPopupManager.ShowAlert("Evolve Engine", $"If you wanna appear offline, please restart your game");
            }, "Disabled", () =>
            {
                Settings.OfflineSpoof = false;
                FoldersManager.Config.Ini.SetBool("Self", "OfflineSpoof", false);
            }, "Offline spoof", null, null, false, Settings.OfflineSpoof);

            var PingSpoof = new QMToggleButton(ThisMenu, 3, 1, "Ping\nSpoof", () =>
            {
                Settings.PingSpoof = true;
                FoldersManager.Config.Ini.SetBool("Self", "PingSpoof", true);
            }, "Disabled", () =>
            {
                Settings.PingSpoof = false;
                FoldersManager.Config.Ini.SetBool("Self", "PingSpoof", false);
            }, "Ping spoof", null, null, false, Settings.PingSpoof);

            new QMToggleButton(ThisMenu, 4, 1, "Frames\nSpoof", () =>
            {
                Settings.FramesSpoof = true;
                FoldersManager.Config.Ini.SetBool("Self", "ToggleFrameSpoof", true);
            }, "Disabled", () =>
            {
                Settings.FramesSpoof = false;
                FoldersManager.Config.Ini.SetBool("Self", "ToggleFrameSpoof", false);
            }, "Frames spoof", null, null, false, Settings.FramesSpoof);

            var QuestSpoof =new QMToggleButton(ThisMenu, 1, 1, "Quest\nSpoof", () =>
            {
                Settings.QuestSpoof = true;
                FoldersManager.Config.Ini.SetBool("Self", "QuestSpoof", true);
                if (VRCPlayer.field_Internal_Static_VRCPlayer_0 == true) Wrappers.Utils.VRCUiPopupManager.ShowAlert("Evolve Engine", $"WARNING: This feature is risky and might lead you to a permanent ban.\nMake sure your home world is quest compatible then restart your game.");
            }, "Disabled", () =>
            {
                Settings.QuestSpoof = false;
                FoldersManager.Config.Ini.SetBool("Self", "QuestSpoof", false);
            }, "Quest spoof", null, null, false, Settings.QuestSpoof);
            QuestSpoof.setIntractable(false);

            Slider1 = SliderApi.CreateCustom(ThisMenu.getMenuName(), 3.2f, 1.75f, $"Frames: {Settings.Frames}", (float Val) =>
            {
                Settings.Frames = Val;
                Slider1.GetComponentInChildren<Text>().text = $"Frames: {Settings.Frames}";
                FoldersManager.Config.Ini.SetFloat("Self", "FramesSpoof", Settings.Frames);
            }, Settings.Frames, 0f, 500f);

            Slider2 = SliderApi.CreateCustom(ThisMenu.getMenuName(), 3.2f, 2.25f, $"Ping: {Settings.Ping}", (float Val) =>
            {
                Settings.Ping = (int) Val;
                Slider2.GetComponentInChildren<Text>().text = $"Ping: {Settings.Ping}";
                FoldersManager.Config.Ini.SetFloat("Self", "Ping", Settings.Ping);
            }, Settings.Ping, -999, 999);
        }
    }
}