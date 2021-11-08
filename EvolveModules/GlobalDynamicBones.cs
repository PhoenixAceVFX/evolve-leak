using Evolve.Utils;
using System.Collections.Generic;
using System.Linq;
using Evolve.ConsoleUtils;
using UnityEngine;
using VRC.SDKBase;
using VRC.Core;

namespace Evolve.Modules
{
    internal class GlobalDynamicBones
    {
        public static List<string> Users = new List<string>();

        public static void ProcessDynamicBones(VRCAvatarManager avatarManager, GameObject AvatarObject = null)
        {
            try
            {
                var Player = avatarManager.field_Private_VRCPlayer_0;
                if (!Users.Contains(Player._player.field_Private_APIUser_0.id)) Users.Add(Player._player.field_Private_APIUser_0.id);

                GameObject Avatar;
                if (avatarManager != null) Avatar = avatarManager.prop_GameObject_0;
                else Avatar = AvatarObject;

                if (Users.Contains(Player.prop_Player_0.prop_APIUser_0.id) && Avatar != null)
                {
                    //Bones
                    if (Settings.HeadBones)
                    {
                        foreach (DynamicBone item2 in Avatar.GetComponentInChildren<Animator>().GetBoneTransform(HumanBodyBones.Head).GetComponentsInChildren<DynamicBone>())
                        {
                            GlobalDynamicBones.currentWorldDynamicBones.Add(item2);
                        }
                    }

                    if (Settings.ChestBones)
                    {
                        foreach (DynamicBone item2 in Avatar.GetComponentInChildren<Animator>().GetBoneTransform(HumanBodyBones.Chest).GetComponentsInChildren<DynamicBone>())
                        {
                            GlobalDynamicBones.currentWorldDynamicBones.Add(item2);
                        }
                    }

                    if (Settings.HipBones)
                    {
                        foreach (DynamicBone item2 in Avatar.GetComponentInChildren<Animator>().GetBoneTransform(HumanBodyBones.Hips).GetComponentsInChildren<DynamicBone>())
                        {
                            GlobalDynamicBones.currentWorldDynamicBones.Add(item2);
                        }
                    }

                    //Colliders
                    if (Settings.HandColliders)
                    {
                        foreach (DynamicBoneCollider dynamicBoneCollider in Avatar.GetComponentInChildren<Animator>().GetBoneTransform(HumanBodyBones.LeftHand).GetComponentsInChildren<DynamicBoneCollider>())
                        {
                            if (dynamicBoneCollider.m_Bound != DynamicBoneCollider.EnumNPublicSealedvaOuIn3vUnique.Inside)
                            {
                                GlobalDynamicBones.currentWorldDynamicBoneColliders.Add(dynamicBoneCollider);
                            }
                        }
                        foreach (DynamicBoneCollider dynamicBoneCollider2 in Avatar.GetComponentInChildren<Animator>().GetBoneTransform(HumanBodyBones.RightHand).GetComponentsInChildren<DynamicBoneCollider>())
                        {
                            if (dynamicBoneCollider2.m_Bound != DynamicBoneCollider.EnumNPublicSealedvaOuIn3vUnique.Inside)
                            {
                                GlobalDynamicBones.currentWorldDynamicBoneColliders.Add(dynamicBoneCollider2);
                            }
                        }
                    }
                    if (Settings.FeetColliders)
                    {
                        foreach (DynamicBoneCollider dynamicBoneCollider3 in Avatar.GetComponentInChildren<Animator>().GetBoneTransform(HumanBodyBones.LeftFoot).GetComponentsInChildren<DynamicBoneCollider>())
                        {
                            if (dynamicBoneCollider3.m_Bound != DynamicBoneCollider.EnumNPublicSealedvaOuIn3vUnique.Inside)
                            {
                                GlobalDynamicBones.currentWorldDynamicBoneColliders.Add(dynamicBoneCollider3);
                            }
                        }
                        foreach (DynamicBoneCollider dynamicBoneCollider4 in Avatar.GetComponentInChildren<Animator>().GetBoneTransform(HumanBodyBones.RightFoot).GetComponentsInChildren<DynamicBoneCollider>())
                        {
                            if (dynamicBoneCollider4.m_Bound != DynamicBoneCollider.EnumNPublicSealedvaOuIn3vUnique.Inside)
                            {
                                GlobalDynamicBones.currentWorldDynamicBoneColliders.Add(dynamicBoneCollider4);
                            }
                        }
                    }

                    foreach (DynamicBone dynamicBone in GlobalDynamicBones.currentWorldDynamicBones.ToList<DynamicBone>())
                    {
                        if (dynamicBone == null)
                        {
                            GlobalDynamicBones.currentWorldDynamicBones.Remove(dynamicBone);
                        }
                        else
                        {
                            foreach (DynamicBoneCollider dynamicBoneCollider5 in GlobalDynamicBones.currentWorldDynamicBoneColliders.ToList<DynamicBoneCollider>())
                            {
                                if (dynamicBoneCollider5 == null)
                                {
                                    GlobalDynamicBones.currentWorldDynamicBoneColliders.Remove(dynamicBoneCollider5);
                                }
                                else if (dynamicBone.m_Colliders.IndexOf(dynamicBoneCollider5) == -1)
                                {
                                    dynamicBone.m_Colliders.Add(dynamicBoneCollider5);
                                }
                            }
                        }
                    }
                }
            }
            catch { }
        }

        public static List<DynamicBone> currentWorldDynamicBones = new List<DynamicBone>();

        public static List<DynamicBoneCollider> currentWorldDynamicBoneColliders = new List<DynamicBoneCollider>();
    }
}