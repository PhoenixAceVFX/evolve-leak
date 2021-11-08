using Evolve.Utils;
using Evolve.Wrappers;
using RealisticEyeMovements;
using RootMotion.FinalIK;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRC.SDKBase;
using Object = UnityEngine.Object;

namespace Evolve.Movements
{
    class Movements
    {
        //Fly
        private static VRCPlayer LocalPlayer;
        private static Transform CameraTransform;
        public static bool isInVR;

        //Jump
        public static bool InfiniteJump = false;
        public static bool RocketJump = false;

        //Blink
        public static bool ShouldSkipAction = false;
        public static GameObject Capsule;
        public static AudioClip BlinkStart;
        public static AudioClip BlinkStop;
        public static GameObject SourceStart;
        public static GameObject SourceStop;

        public static IEnumerator Initialize()
        {
            //Blink
            if (BlinkStart == null)
            {
                var Request = new WWW("https://dl.dropboxusercontent.com/s/xyr7xxelq7xa06q/BlinkStart.ogg?dl=0", null, new Il2CppSystem.Collections.Generic.Dictionary<string, string>());
                yield return Request;
                BlinkStart = Request.GetAudioClip(false, false, AudioType.OGGVORBIS);
            }
            if (BlinkStop == null)
            {
                var Request = new WWW("https://dl.dropboxusercontent.com/s/iyb9w8d8ngrh1d9/BlinkStop.ogg?dl=0", null, new Il2CppSystem.Collections.Generic.Dictionary<string, string>());
                yield return Request;
                BlinkStop = Request.GetAudioClip(false, false, AudioType.OGGVORBIS);
            }
            SourceStart = new GameObject("BlinkStart");
            SourceStart.AddComponent<AudioSource>();
            SourceStart.GetComponent<AudioSource>().clip = BlinkStart;
            SourceStart.transform.parent = Camera.main.transform;
            SourceStop = new GameObject("BlinkStop");
            SourceStop.AddComponent<AudioSource>();
            SourceStop.GetComponent<AudioSource>().clip = BlinkStop;
            SourceStop.transform.parent = Camera.main.transform;
        }

        public static void Update()
        {
            try
            {
                //Jump
                if (InfiniteJump)
                {
                    if (VRCInputManager.Method_Public_Static_VRCInput_String_0("Jump").prop_Boolean_0 && !Networking.LocalPlayer.IsPlayerGrounded())
                    {
                        var Jump = Networking.LocalPlayer.GetVelocity();
                        Jump.y = Networking.LocalPlayer.GetJumpImpulse();
                        Networking.LocalPlayer.SetVelocity(Jump);
                    }
                }
                if (RocketJump)
                {
                    if (VRCInputManager.Method_Public_Static_VRCInput_String_0("Jump").prop_Single_0 == 1)
                    {
                        var Jump = Networking.LocalPlayer.GetVelocity();
                        Jump.y = Networking.LocalPlayer.GetJumpImpulse();
                        Networking.LocalPlayer.SetVelocity(Jump);
                    }
                }

                //Blink
                if (Settings.Blink)
                {
                    if (Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f)
                    {
                        if (!ShouldSkipAction)
                        {
                            Settings.Serialize = true;
                            ShouldSkipAction = true;
                            SourceStart.GetComponent<AudioSource>().Play();
                            SourceStop.GetComponent<AudioSource>().Stop();

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
                        }
                    }
                    else if (ShouldSkipAction)
                    {
                        Settings.Serialize = false;
                        ShouldSkipAction = false;
                        Object.Destroy(Capsule);
                        SourceStart.GetComponent<AudioSource>().Stop();
                        SourceStop.GetComponent<AudioSource>().Play();
                    }
                }

                //Speed
                if (Settings.Speed)
                {
                    var Loco = VRCPlayer.field_Internal_Static_VRCPlayer_0.GetComponent<LocomotionInputController>();
                    if (Loco != null)
                    {
                        Loco.field_Public_Single_0 = Settings.SpeedValue; //Run
                        Loco.field_Public_Single_1 = Settings.SpeedValue; //Strafe
                        Loco.field_Public_Single_2 = Settings.SpeedValue / 2; //Walk
                    }
                }

                //Ray tp
                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.T))
                {
                    var playerCamera = Camera.main.transform;
                    Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
                    RaycastHit[] hits = Physics.RaycastAll(ray);
                    if (hits.Length > 0)
                    {
                        RaycastHit raycastHit = hits[0];
                        VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position = raycastHit.point;
                    }
                }

                //Fly
                if (Settings.Fly)
                {
                    if (RoomManager.field_Internal_Static_ApiWorld_0 == null) return;

                    if(LocalPlayer == null || CameraTransform == null)
                    {
                        LocalPlayer = Wrappers.Utils.LocalPlayer;
                        CameraTransform = Camera.main.transform;
                        isInVR = LocalPlayer.GetIsInVR();
                    }

                    Networking.LocalPlayer.SetVelocity(new Vector3(0f, 0f, 0f));

                    if (isInVR)
                    {
                        if (Math.Abs(Input.GetAxis("Vertical")) != 0f) LocalPlayer.transform.position += CameraTransform.forward * Settings.FlySpeed * Time.deltaTime * Input.GetAxis("Vertical");
                        if (Math.Abs(Input.GetAxis("Horizontal")) != 0f) LocalPlayer.transform.position += CameraTransform.right * Settings.FlySpeed * Time.deltaTime * Input.GetAxis("Horizontal");
                        if (Input.GetAxis("Oculus_CrossPlatform_SecondaryThumbstickVertical") < 0f) LocalPlayer.transform.position += CameraTransform.up * Settings.FlySpeed * Time.deltaTime * Input.GetAxisRaw("Oculus_CrossPlatform_SecondaryThumbstickVertical");
                        if (Input.GetAxis("Oculus_CrossPlatform_SecondaryThumbstickVertical") > 0f) LocalPlayer.transform.position += CameraTransform.up * Settings.FlySpeed * Time.deltaTime * Input.GetAxisRaw("Oculus_CrossPlatform_SecondaryThumbstickVertical");
                    }
                    else
                    {
                        if (Input.GetKeyDown((KeyCode)304)) Settings.FlySpeed *= 2f;
                        if (Input.GetKeyUp((KeyCode)304)) Settings.FlySpeed /= 2f;
                        if (Input.GetKey((KeyCode)101)) LocalPlayer.transform.position += CameraTransform.up * Settings.FlySpeed * Time.deltaTime;
                        if (Input.GetKey((KeyCode)113)) LocalPlayer.transform.position += CameraTransform.up * -1f * Settings.FlySpeed * Time.deltaTime;
                        if (Input.GetKey((KeyCode)119)) LocalPlayer.transform.position += CameraTransform.forward * Settings.FlySpeed * Time.deltaTime;
                        if (Input.GetKey((KeyCode)97)) LocalPlayer.transform.position += CameraTransform.right * -1f * Settings.FlySpeed * Time.deltaTime;
                        if (Input.GetKey((KeyCode)100)) LocalPlayer.transform.position += CameraTransform.right * Settings.FlySpeed * Time.deltaTime;
                        if (Input.GetKey((KeyCode)115)) LocalPlayer.transform.position += CameraTransform.forward * -1f * Settings.FlySpeed * Time.deltaTime;
                    }
                }
            }
            catch { }
        }
    }
}
