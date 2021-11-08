using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Evolve.Modules
{
    class Lewd
    {
        internal static List<string> TermsToToggleOff = new List<string>
        {
            "cloth",
            "shirt",
            "pant",
            "under",
            "undi",
            "jacket",
            "top",
            "bra",
            "skirt",
            "jean",
            "trouser",
            "boxers",
            "hoodi",
            "bottom",
            "dress",
            "bandage",
            "bondage",
            "sweat",
            "cardig",
            "corset",
            "tiddy",
            "pastie",
            "suit",
            "stocking",
            "jewel",
            "frill",
            "gauze",
            "cover",
            "pubic",
            "sfw",
            "harn",
            "biki",
            "off",
            "disable"
        };

        internal static List<string> TermsToToggleOn = new List<string>
        {
            "penis",
            "dick",
            "cock",
            "futa",
            "dildo",
            "strap",
            "shlong",
            "dong",
            "vibrat",
            "lovense",
            "sex",
            "toy",
            "butt",
            "plug",
            "whip",
            "cum",
            "sperm",
            "facial",
            "nude",
            "naked",
            "nsfw"
        };
        public static void Lewdify(GameObject avatar, bool ForceLewdify)
        {
            foreach (SkinnedMeshRenderer skinnedMeshRenderer in avatar.GetComponentsInChildren<SkinnedMeshRenderer>(true))
            {
                if (skinnedMeshRenderer.transform.parent != null && skinnedMeshRenderer.transform.parent.parent != null && skinnedMeshRenderer.transform.parent.parent.parent != null && skinnedMeshRenderer.transform.parent.parent.parent.parent != null)
                {
                    foreach (string value in TermsToToggleOn)
                    {
                        if ((skinnedMeshRenderer.transform.parent.parent.parent.parent.name.ToLower().Contains(value) || (skinnedMeshRenderer.transform.parent.parent.parent.parent.parent != null && skinnedMeshRenderer.transform.parent.parent.parent.parent.parent.name.ToLower().Contains(value))) && skinnedMeshRenderer.transform.parent.gameObject != null)
                        {
                            skinnedMeshRenderer.gameObject.SetActive(true);
                            skinnedMeshRenderer.transform.parent.gameObject.SetActive(true);
                            skinnedMeshRenderer.transform.parent.parent.gameObject.SetActive(true);
                            skinnedMeshRenderer.transform.parent.parent.parent.gameObject.SetActive(true);
                            skinnedMeshRenderer.transform.parent.parent.parent.parent.gameObject.SetActive(true);
                            if (skinnedMeshRenderer.transform.parent.parent.parent.parent.parent != null)
                            {
                                skinnedMeshRenderer.transform.parent.parent.parent.parent.parent.gameObject.SetActive(true);
                            }
                            if (skinnedMeshRenderer.transform.parent.parent.parent.parent.GetComponent<Animator>() != null)
                            {
                                skinnedMeshRenderer.transform.parent.parent.parent.parent.GetComponent<Animator>().enabled = false;
                            }
                            if (skinnedMeshRenderer.GetComponent<Animator>() != null)
                            {
                                skinnedMeshRenderer.GetComponent<Animator>().enabled = false;
                            }
                        }
                    }
                    foreach (string value2 in TermsToToggleOff)
                    {
                        if (skinnedMeshRenderer.transform.parent.parent.parent.parent.name.ToLower().Replace("nsfw", "").Contains(value2) && skinnedMeshRenderer.transform.parent.gameObject != null)
                        {
                            if (skinnedMeshRenderer.transform.parent.parent.parent.parent.GetComponent<Animator>() != null)
                            {
                                skinnedMeshRenderer.transform.parent.parent.parent.parent.GetComponent<Animator>().enabled = false;
                            }
                            if (skinnedMeshRenderer.GetComponent<Animator>() != null)
                            {
                                skinnedMeshRenderer.GetComponent<Animator>().enabled = false;
                            }
                            if (ForceLewdify)
                            {
                                UnityEngine.Object.Destroy(skinnedMeshRenderer.transform.parent.gameObject);
                            }
                            else
                            {
                                skinnedMeshRenderer.transform.parent.gameObject.SetActive(false);
                            }
                        }
                    }
                }
            }
            foreach (MeshRenderer meshRenderer in avatar.GetComponentsInChildren<MeshRenderer>(true))
            {
                if (meshRenderer.transform.parent != null && meshRenderer.transform.parent.parent != null && meshRenderer.transform.parent.parent.parent != null && meshRenderer.transform.parent.parent.parent.parent != null)
                {
                    foreach (string value3 in TermsToToggleOn)
                    {
                        if ((meshRenderer.transform.parent.parent.parent.parent.name.ToLower().Contains(value3) || (meshRenderer.transform.parent.parent.parent.parent.parent != null && meshRenderer.transform.parent.parent.parent.parent.parent.name.ToLower().Contains(value3))) && meshRenderer.transform.parent.gameObject != null)
                        {
                            meshRenderer.gameObject.SetActive(true);
                            meshRenderer.transform.parent.gameObject.SetActive(true);
                            meshRenderer.transform.parent.parent.gameObject.SetActive(true);
                            meshRenderer.transform.parent.parent.parent.gameObject.SetActive(true);
                            meshRenderer.transform.parent.parent.parent.parent.gameObject.SetActive(true);
                            if (meshRenderer.transform.parent.parent.parent.parent.parent != null)
                            {
                                meshRenderer.transform.parent.parent.parent.parent.parent.gameObject.SetActive(true);
                            }
                            if (meshRenderer.transform.parent.parent.parent.parent.GetComponent<Animator>() != null)
                            {
                                meshRenderer.transform.parent.parent.parent.parent.GetComponent<Animator>().enabled = false;
                            }
                            if (meshRenderer.GetComponent<Animator>() != null)
                            {
                                meshRenderer.GetComponent<Animator>().enabled = false;
                            }
                        }
                    }
                    foreach (string value4 in TermsToToggleOff)
                    {
                        if (meshRenderer.transform.parent.parent.parent.parent.name.ToLower().Replace("nsfw", "").Contains(value4) && meshRenderer.transform.parent.gameObject != null)
                        {
                            if (meshRenderer.transform.parent.parent.parent.parent.GetComponent<Animator>() != null)
                            {
                                meshRenderer.transform.parent.parent.parent.parent.GetComponent<Animator>().enabled = false;
                            }
                            if (meshRenderer.GetComponent<Animator>() != null)
                            {
                                meshRenderer.GetComponent<Animator>().enabled = false;
                            }
                            if (ForceLewdify)
                            {
                                UnityEngine.Object.Destroy(meshRenderer.transform.parent.gameObject);
                            }
                            else
                            {
                                meshRenderer.transform.parent.gameObject.SetActive(false);
                            }
                        }
                    }
                }
            }
        }
    }
}
