using MelonLoader;
using System.Collections;
using System.IO;
using UnityEngine;
using VRC.SDKBase;

namespace Evolve.Modules
{
    internal class LoadingPictures
    {
        private static GameObject screen, cube;
        private static Texture lastTexture;
        private static Renderer screenRender;
        private static Renderer pic;
        private static int delay = 1;
        private static bool noPics = false;
        private static readonly string folder_dir = Directory.GetCurrentDirectory() + "/Evolve/LoadingPictures";

        public static void Initialize()
        {
            if (VRCPlayer.field_Internal_Static_VRCPlayer_0 != true)
            {
                if (delay != 0)
                {
                    if (delay++ == 100)
                    {
                        screen = GameObject.Find("/UserInterface/MenuContent/Popups/LoadingPopup/3DElements/LoadingInfoPanel/InfoPanel_Template_ANIM/SCREEN/mainScreen");
                        if (screen != null)
                        {
                            setup();
                            delay = 0;
                        }
                        else delay = 1;
                    }
                    return;
                }

                if (noPics) return;

                if (lastTexture != screenRender.material.mainTexture)
                {
                    lastTexture = screenRender.material.mainTexture;
                    changePic();
                }
            }
        }

        public static void OnLevelWasInitialized()
        {
            if (noPics) setup();
        }

        public static void changePic()
        {
            Texture2D texture = new Texture2D(2, 2);
            ImageConversion.LoadImage(texture, File.ReadAllBytes(randImage()));
            pic.material.mainTexture = texture;
            if (pic.material.mainTexture.height > pic.material.mainTexture.width)
            {
                cube.transform.localScale = new Vector3(0.099f, 1, 0.175f);
                GameObject.Find("/UserInterface/MenuContent/Popups/LoadingPopup/3DElements/LoadingInfoPanel/InfoPanel_Template_ANIM/SCREEN/mainFrame").transform.localScale = new Vector3(10.80f, 19.20f, 1);
            }
            else
            {
                cube.transform.localScale = new Vector3(0.175f, 1, 0.099f);
                GameObject.Find("/UserInterface/MenuContent/Popups/LoadingPopup/3DElements/LoadingInfoPanel/InfoPanel_Template_ANIM/SCREEN/mainFrame").transform.localScale = new Vector3(19.20f, 10.80f, 1);
            }
        }

        public static void setup()
        {
            string imageLink = randImage();
            if (imageLink == null)
            {
                noPics = true;
                return;
            }
            noPics = false;

            GameObject parentScreen = GameObject.Find("/UserInterface/MenuContent/Popups/LoadingPopup/3DElements/LoadingInfoPanel/InfoPanel_Template_ANIM/SCREEN");
            screenRender = screen.GetComponent<Renderer>();
            lastTexture = screenRender.material.mainTexture;

            cube = GameObject.CreatePrimitive(PrimitiveType.Plane);
            cube.transform.SetParent(parentScreen.transform);
            cube.transform.rotation = screen.transform.rotation;
            cube.transform.localPosition = new Vector3(0, 0, -0.19f);
            cube.GetComponent<Collider>().enabled = false;
            Texture2D texture = new Texture2D(2, 2);
            ImageConversion.LoadImage(texture, File.ReadAllBytes(imageLink));
            pic = cube.GetComponent<Renderer>();
            pic.material.mainTexture = texture;
            var shader = Shader.Find("UI/Default");
            pic.material.shader = shader;

            screenRender.enabled = false;

            if (pic.material.mainTexture.height > pic.material.mainTexture.width)
            {
                cube.transform.localScale = new Vector3(0.099f, 1, 0.175f);
                GameObject.Find("/UserInterface/MenuContent/Popups/LoadingPopup/3DElements/LoadingInfoPanel/InfoPanel_Template_ANIM/SCREEN/mainFrame").transform.localScale = new Vector3(10.80f, 19.20f, 1);
            }
            else
            {
                cube.transform.localScale = new Vector3(0.175f, 1, 0.099f);
                GameObject.Find("/UserInterface/MenuContent/Popups/LoadingPopup/3DElements/LoadingInfoPanel/InfoPanel_Template_ANIM/SCREEN/mainFrame").transform.localScale = new Vector3(19.20f, 10.80f, 1);
            }

            GameObject.Find("/UserInterface/MenuContent/Popups/LoadingPopup/3DElements/LoadingInfoPanel/InfoPanel_Template_ANIM/ICON").active = false;
            GameObject.Find("/UserInterface/MenuContent/Popups/LoadingPopup/3DElements/LoadingInfoPanel/InfoPanel_Template_ANIM/TITLE").active = false;
        }

        public static string randImage()
        {
            FileInfo[] Files = new DirectoryInfo(folder_dir).GetFiles("*.png");
            if (Files.Length == 0)
            {
                string[] dirs = Directory.GetDirectories(folder_dir, "*", SearchOption.TopDirectoryOnly);
                if (dirs.Length == 0) return null;
                int randDir = new Il2CppSystem.Random().Next(0, dirs.Length);
                FileInfo[] dirFiles = new DirectoryInfo(dirs[randDir]).GetFiles("*.png");
                int randPic2 = new Il2CppSystem.Random().Next(0, Files.Length);
                return dirFiles[randPic2].ToString();
            }
            int randPic = new Il2CppSystem.Random().Next(0, Files.Length);
            return Files[randPic].ToString();
        }
    }
}