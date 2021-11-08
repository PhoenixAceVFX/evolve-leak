using Evolve.ConsoleUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Evolve.Modules
{
    class ThirdPerson
    {
        public static GameObject CameraObject1;
        public static GameObject CameraObject2;
        public static Camera MainCamera;
        public static bool IsFirst = false;
        public static bool IsSecond = false;
        public static float Speed = 0.1f;
        public static float Time = 0.1f;
        public static bool Enabled = false;
        public static void Initiliaze()
        {
            var MyCam = GameObject.Find("Camera (eye)");
            if (MyCam == null) MyCam = GameObject.Find("CenterEyeAnchor");
            MainCamera = MyCam.GetComponent<Camera>();

            if (CameraObject1 != null) UnityEngine.Object.Destroy(CameraObject1);
            CameraObject1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            UnityEngine.Object.Destroy(CameraObject1.GetComponent<MeshRenderer>());
            CameraObject1.transform.localScale = MyCam.transform.localScale;
            CameraObject1.AddComponent<Rigidbody>();
            CameraObject1.GetComponent<Rigidbody>().isKinematic = true;
            CameraObject1.GetComponent<Rigidbody>().useGravity = false;
            if (CameraObject1.GetComponent<Collider>()) CameraObject1.GetComponent<Collider>().enabled = false;
            CameraObject1.GetComponent<Renderer>().enabled = false;
            CameraObject1.AddComponent<Camera>();
            CameraObject1.GetComponent<Camera>().fieldOfView = MyCam.GetComponent<Camera>().fieldOfView;
            CameraObject1.GetComponent<Camera>().nearClipPlane = MyCam.GetComponent<Camera>().nearClipPlane / 4f;
            CameraObject1.transform.parent = MyCam.transform;
            CameraObject1.transform.position = MyCam.transform.position;
            CameraObject1.transform.position -= CameraObject1.transform.forward * 2;
            CameraObject1.transform.rotation = MyCam.transform.rotation;
            CameraObject1.GetComponent<Camera>().enabled = false;

            if (CameraObject2 != null) UnityEngine.Object.Destroy(CameraObject2);
            CameraObject2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            UnityEngine.Object.Destroy(CameraObject2.GetComponent<MeshRenderer>());
            CameraObject2.transform.localScale = MyCam.transform.localScale;
            CameraObject2.AddComponent<Rigidbody>();
            CameraObject2.GetComponent<Rigidbody>().isKinematic = true;
            CameraObject2.GetComponent<Rigidbody>().useGravity = false;
            if (CameraObject2.GetComponent<Collider>()) CameraObject2.GetComponent<Collider>().enabled = false;
            CameraObject2.GetComponent<Renderer>().enabled = false;
            CameraObject2.AddComponent<Camera>();
            CameraObject2.GetComponent<Camera>().fieldOfView = MyCam.GetComponent<Camera>().fieldOfView;
            CameraObject2.GetComponent<Camera>().nearClipPlane = MyCam.GetComponent<Camera>().nearClipPlane / 4f;
            CameraObject2.transform.parent = MyCam.transform;
            CameraObject2.transform.position = MyCam.transform.position;
            CameraObject2.transform.position += CameraObject2.transform.forward * 2;
            CameraObject2.transform.rotation = MyCam.transform.rotation;
            CameraObject2.transform.Rotate(0f, 180f, 0f);
            CameraObject2.GetComponent<Camera>().enabled = false;
        }

        public static void First()
        {
            Enabled = true;
            IsFirst = true;
            MainCamera.enabled = false;
            CameraObject1.GetComponent<Camera>().enabled = true;
            CameraObject2.GetComponent<Camera>().enabled = false;
        }

        public static void Second()
        {
            Enabled = true;
            IsSecond = true;
            MainCamera.enabled = false;
            CameraObject1.GetComponent<Camera>().enabled = false;
            CameraObject2.GetComponent<Camera>().enabled = true;
        }

        public static void Third()
        {
            Enabled = false;
            IsFirst = false;
            IsSecond = false;
            MainCamera.enabled = true;
            CameraObject1.GetComponent<Camera>().enabled = false;
            CameraObject2.GetComponent<Camera>().enabled = false;
        }
    }
}
