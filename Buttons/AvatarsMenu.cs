using System.Diagnostics;
using Evolve.Utils;
using ButtonApi;
using UnityEngine;
using System;
using Evolve.Wrappers;
using RealisticEyeMovements;
using RootMotion.FinalIK;
using MelonLoader;
using System.Collections;
using Evolve.Api;
using System.Collections.Generic;
using static Evolve.Buttons.EvolveAvatarsMenu;
using System.Windows.Forms;
using System.IO;

namespace Evolve.Buttons
{
    internal class AvatarsMenu
    {
        public static QMNestedButton ThisMenu;
        public static QMSingleButton AviMenu;
        public static MenuText Text;
        public static string BroadcastType = "Default";
        public static List<SerializeAvi> AviList = new List<SerializeAvi>();
        public static float ReuploadCD;
        public static List<GameObject> AllClones = new List<GameObject>();
        public static void Initialize()
        {
            ThisMenu = new QMNestedButton(EvolveMenu.ThisMenu, 2.5f, 0.75f, "Avatars", "Avatars Menu");
            ThisMenu.getMainButton().getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector3(1, 2);

            AviMenu = new QMSingleButton(ThisMenu, 5, 1.75f, "--->", () =>
            {
                EvolveAvatarsMenu.ThisMenu.OpenMenu();
            }, "Evolve avatars menu");
            AviMenu.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            new QMSingleButton(ThisMenu, 1, 0   , "Download\nVrca", () =>
            {
                Process.Start(Wrappers.Utils.LocalPlayer.GetApiAvatar().assetUrl);
            }, "Download the vrca.");

            new QMSingleButton(ThisMenu, 2, 0, "Copy\nID", () =>
            {
                Clipboard.SetText(Wrappers.Utils.LocalPlayer.GetApiAvatar().id);
                Notifications.Notify($"Copied your avatar's ID");
            }, "Copy the id of this avatar");

            new QMSingleButton(ThisMenu, 3, 0, "Copy\nasset link", () =>
            {
                Clipboard.SetText(Wrappers.Utils.LocalPlayer.GetApiAvatar().assetUrl);
                Notifications.Notify($"Copied your avatar's asset link");
            }, "Copy the asset link of this avatar");

            new QMSingleButton(ThisMenu, 4, 0, "Log avatar", () =>
            {
                var ApiAvatar = Wrappers.Utils.LocalPlayer.GetApiAvatar();
                string Log = "";
                Log = $"Name: {ApiAvatar.name}\nDescription: {ApiAvatar.description}\nID: {ApiAvatar.id}\nPlatform: {ApiAvatar.platform}\nRelease: {ApiAvatar.releaseStatus}\nAuthor name: {ApiAvatar.authorName}\nAuthor UserID: {ApiAvatar.authorId}\nVersion: {ApiAvatar.version}\nMade in: unity {ApiAvatar.unityVersion}\nAsset url: {ApiAvatar.assetUrl}\nThumbnail url: {ApiAvatar.imageUrl}";
                File.WriteAllText(FoldersManager.FileCheck.AvatarsFolder + $"/{ApiAvatar.name}.txt", Log);
                Notifications.Notify($"Saved avatar's info\nEvolve/Avatars/{ApiAvatar.name}.txt");
            }, "Will save this avatar informations into the Evolve/Avatars folder");

            var Clone = new QMSingleButton(ThisMenu, 5, -0.25f, "Clone", () =>
            {
                var Capsule = UnityEngine.Object.Instantiate<GameObject>(Wrappers.Utils.LocalPlayer.prop_VRCAvatarManager_0.prop_GameObject_0, null, true);
                Animator component = Capsule.GetComponent<Animator>();
                if (component != null && component.isHuman)
                {
                    Transform boneTransform = component.GetBoneTransform((HumanBodyBones)10);
                    if (boneTransform != null) boneTransform.localScale = Vector3.one;
                }
                Capsule.name = "Cloned";
                component.enabled = false;
                Capsule.GetComponent<FullBodyBipedIK>().enabled = false;
                Capsule.GetComponent<LimbIK>().enabled = false;
                Capsule.GetComponent<VRIK>().enabled = false;
                Capsule.GetComponent<LookTargetController>().enabled = false;
                Capsule.transform.position = Wrappers.Utils.LocalPlayer.transform.position;
                Capsule.transform.rotation = Wrappers.Utils.LocalPlayer.transform.rotation;
                AllClones.Add(Capsule);
            }, "Will clone your avatar at your current position.");

            var DestroyClones = new QMSingleButton(ThisMenu, 5, 0.25f, "Destroy all", () =>
            {
                foreach (var Clone in AllClones)
                {
                    UnityEngine.Object.Destroy(Clone);
                }
            }, "Will destroy all created clones");

            Clone.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);
            DestroyClones.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            new QMSingleButton(ThisMenu, 1, 1, "Change\nTo ID", () =>
            {
                Wrappers.PopupManager.InputeText("Enter avatar ID", "Confirm", new Action<string>((AviID) =>
                {
                    if (AviID.Contains("avtr_")) Functions.ChangeAvatar(AviID);
                    else Wrappers.Utils.VRCUiPopupManager.ShowAlert("Error", "Invalid ID");
                }));
            }, "Change into an avatar with the ID");

            new QMSingleButton(ThisMenu, 0, 0, "<color=#ffb700>Reupload\navatar</color>", () =>
            {
                if (Login.Authorization.AccessLevel > 1)
                {
                    if (Time.time < ReuploadCD)
                    {
                        Wrappers.Utils.VRCUiPopupManager.ShowAlert("Evolve Engine", $"Function is still in cooldown !");
                        return;
                    }
                    ReuploadCD = Time.time + 60;

                    bool SetName = false;
                    bool SetDesc = false;
                    bool SetThumbnailUrl = false;

                    var Name = VRCPlayer.field_Internal_Static_VRCPlayer_0.GetApiAvatar().name;
                    var Desc = VRCPlayer.field_Internal_Static_VRCPlayer_0.GetApiAvatar().description;
                    var Thumbnail = VRCPlayer.field_Internal_Static_VRCPlayer_0.GetApiAvatar().imageUrl;

                    MelonCoroutines.Start(Start());
                    IEnumerator Start()
                    {
                        Wrappers.PopupManager.InputeText("Set name (Leave blank for same name)", "Confirm", new Action<string>((AviName) =>
                        {
                            if (AviName.Length >= 1) Name = AviName;
                            VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopUp();
                            SetName = true;
                        }));

                        while (!SetName) yield return null;
                        yield return new WaitForSeconds(1);

                        Wrappers.PopupManager.InputeText("Set description (Leave blank for same description)", "Confirm", new Action<string>((AviDesc) =>
                        {
                            if (AviDesc.Length >= 1) Desc = AviDesc;
                            VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopUp();
                            SetDesc = true;
                        }));

                        while (!SetDesc) yield return null;
                        yield return new WaitForSeconds(1);


                        Wrappers.PopupManager.InputeText("Set thumbnail URL (Leave blank for same thumbnail)", "Confirm", new Action<string>((Url) =>
                        {
                            if (Url.Length > 10) Thumbnail = Url;
                            VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopUp();
                            SetThumbnailUrl = true;
                        }));

                        while (!SetThumbnailUrl) yield return null;
                        yield return new WaitForSeconds(1);

                        Yoink.Yoinker.ReuploadAvatar(VRCPlayer.field_Internal_Static_VRCPlayer_0.GetApiAvatar(), Name, Desc, Thumbnail);
                    }
                }
                else Wrappers.Utils.VRCUiPopupManager.ShowAlert("Evolve Engine", "This feature is reserved for VIP users or require an access level of 2");
            }, "Will reupload the avatar your are in on your account");

            new QMToggleButton(ThisMenu, 0, 1, "Public", () =>
            {
                Yoink.Yoinker.Status = "public";
            }, "Private", () =>
            {
                Yoink.Yoinker.Status = "private";
            }, "Choose between public or private when reuploading an avatar");
        }
    }
}