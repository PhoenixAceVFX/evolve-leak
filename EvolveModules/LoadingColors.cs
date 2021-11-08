using Evolve.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boo.Lang.Useful.Collections;
using Evolve.ConsoleUtils;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Evolve.Modules
{
    class LoadingColors
    {
        public static IEnumerator Particles()
        {
            if (VRCPlayer.field_Internal_Static_VRCPlayer_0 != true)
            {
                var Border = GameObject.Find("/UserInterface/MenuContent/Popups/LoadingPopup/3DElements/LoadingInfoPanel/InfoPanel_Template_ANIM/SCREEN/mainFrame").GetComponent<MeshRenderer>();
                var PointLight = GameObject.Find("/UserInterface/MenuContent/Popups/LoadingPopup/3DElements/LoadingBackground_TealGradient/_Lighting (1)/Point light").GetComponent<Light>();
                var ReflectionProbe1 = GameObject.Find("/UserInterface/MenuContent/Popups/LoadingPopup/3DElements/LoadingBackground_TealGradient/_Lighting (1)/Reflection Probe").GetComponent<ReflectionProbe>();
                while (Border == null) yield return null;
                while (PointLight == null) yield return null;
                while (ReflectionProbe1 == null) yield return null;
                ReflectionProbe1.mode = ReflectionProbeMode.Realtime;
                ReflectionProbe1.backgroundColor = new Color(0.4006691f, 0, 1, 0);
                Material BorderMaterial = new Material(Shader.Find("Standard"));
                Border.material = BorderMaterial;
                Border.material.color = Color.black;
                Border.material.SetFloat("_Metallic", 1);
                Border.material.SetFloat("_SmoothnessTextureChar", 1);
                PointLight.color = Color.white;

                var Snow = GameObject.Find("/UserInterface/LoadingBackground_TealGradient_Music/_FX_ParticleBubbles/FX_snow").GetComponent<ParticleSystem>();
                var Snow2 = GameObject.Find("/UserInterface/LoadingBackground_TealGradient_Music/_FX_ParticleBubbles/FX_snow").GetComponent<ParticleSystemRenderer>();
                while (Snow == null) yield return null;
                while (Snow2 == null) yield return null;

                Snow.gameObject.SetActive(false);
                Snow.gameObject.transform.position -= new Vector3(0, 5, 0);
                Material TrailMaterial = new Material(Shader.Find("UI/Default"));
                TrailMaterial.color = Color.white;
                Snow2.trailMaterial = TrailMaterial;
                Snow2.material.color = Color.white;

                //Trail
                Snow.trails.enabled = true;
                Snow.trails.mode = ParticleSystemTrailMode.PerParticle;
                Snow.trails.ratio = 1;
                Snow.trails.lifetime = 0.04f;
                Snow.trails.minVertexDistance = 0;
                Snow.trails.worldSpace = false;
                Snow.trails.dieWithParticles = true;
                Snow.trails.textureMode = ParticleSystemTrailTextureMode.Stretch;
                Snow.trails.sizeAffectsWidth = true;
                Snow.trails.sizeAffectsLifetime = false;
                Snow.trails.inheritParticleColor = false;
                Snow.trails.colorOverLifetime = Color.white;
                Snow.trails.widthOverTrail = 0.1f;
                Snow.trails.colorOverTrail = new Color(0.02987278f, 0, 0.3962264f, 0.5f);

                //MainParticle
                Snow.shape.scale = new Vector3(1,1,1.82f);
                Snow.main.startColor.mode = ParticleSystemGradientMode.Color;
                Snow.colorOverLifetime.enabled = false;
                Snow.main.prewarm = false;
                Snow.playOnAwake = true;
                Snow.startColor = new Color(1, 0, 0.282353f, 1);
                Snow.noise.frequency = 1;
                Snow.noise.strength = 0.5f;
                Snow.maxParticles = 250;
                Snow.gameObject.SetActive(true);

                var CloseParticles = GameObject.Find("/UserInterface/LoadingBackground_TealGradient_Music/_FX_ParticleBubbles/FX_CloseParticles");
                while (CloseParticles == null) yield return null;
                CloseParticles.GetComponent<ParticleSystem>().startColor = Settings.TextColor();

                var Floor = GameObject.Find("/UserInterface/LoadingBackground_TealGradient_Music/_FX_ParticleBubbles/FX_floor");
                while (Floor == null) yield return null;
                Floor.GetComponent<ParticleSystem>().startColor = Settings.TextColor();

                var Snow3 = GameObject.Find("/UserInterface/MenuContent/Popups/LoadingPopup/3DElements/LoadingBackground_TealGradient/_FX_ParticleBubbles/FX_snow").GetComponent<ParticleSystem>();
                var Snow4 = GameObject.Find("/UserInterface/MenuContent/Popups/LoadingPopup/3DElements/LoadingBackground_TealGradient/_FX_ParticleBubbles/FX_snow").GetComponent<ParticleSystemRenderer>();
                while (Snow3 == null) yield return null;
                while (Snow4 == null) yield return null;

                Snow3.gameObject.SetActive(false);
                Snow3.gameObject.transform.position -= new Vector3(0, 5, 0);
                TrailMaterial.color = Color.white;
                Snow4.trailMaterial = TrailMaterial;
                Snow4.material.color = Color.white;
                //Snow2.material.mainTexture = new Texture();

                //Trail
                Snow3.trails.enabled = true;
                Snow3.trails.mode = ParticleSystemTrailMode.PerParticle;
                Snow3.trails.ratio = 1;
                Snow3.trails.lifetime = 0.04f;
                Snow3.trails.minVertexDistance = 0;
                Snow3.trails.worldSpace = false;
                Snow3.trails.dieWithParticles = true;
                Snow3.trails.textureMode = ParticleSystemTrailTextureMode.Stretch;
                Snow3.trails.sizeAffectsWidth = true;
                Snow3.trails.sizeAffectsLifetime = false;
                Snow3.trails.inheritParticleColor = false;
                Snow3.trails.colorOverLifetime = Color.white;
                Snow3.trails.widthOverTrail = 0.1f;
                Snow3.trails.colorOverTrail = new Color(0.02987278f, 0, 0.3962264f, 0.5f);

                //MainParticle
                Snow3.shape.scale = new Vector3(1, 1, 1.82f);
                Snow3.main.startColor.mode = ParticleSystemGradientMode.Color;
                Snow3.colorOverLifetime.enabled = false;
                Snow3.main.prewarm = false;
                Snow3.playOnAwake = true;
                Snow3.startColor = new Color(1, 0, 0.282353f, 1);
                Snow3.noise.frequency = 1;
                Snow3.noise.strength = 0.5f;
                Snow3.maxParticles = 250;
                Snow3.gameObject.SetActive(true);

                var CloseParticles1 = GameObject.Find("/UserInterface/MenuContent/Popups/LoadingPopup/3DElements/LoadingBackground_TealGradient/_FX_ParticleBubbles/FX_CloseParticles");
                while (CloseParticles1 == null) yield return null;
                CloseParticles1.GetComponent<ParticleSystem>().startColor = Settings.TextColor();

                var Floor1 = GameObject.Find("/UserInterface/MenuContent/Popups/LoadingPopup/3DElements/LoadingBackground_TealGradient/_FX_ParticleBubbles/FX_floor");
                while (Floor1 == null) yield return null;
                Floor1.GetComponent<ParticleSystem>().startColor = Settings.TextColor();

                var LoadingBar = GameObject.Find("/UserInterface/MenuContent/Popups/LoadingPopup/ProgressPanel/Parent_Loading_Progress/Loading Elements/LOADING_BAR_BG").GetComponent<Image>();
                while (LoadingBar == null) yield return null;

                LoadingBar.color = Settings.TextColor();

                var LoadingBar1 = GameObject.Find("/UserInterface/MenuContent/Popups/LoadingPopup/ProgressPanel/Parent_Loading_Progress/Loading Elements/LOADING_BAR").GetComponent<Image>();
                while (LoadingBar1 == null) yield return null;

                LoadingBar1.color = Settings.TextColor() * 5;

                var LoadingBarPanel = GameObject.Find("/UserInterface/MenuContent/Popups/LoadingPopup/ProgressPanel/Parent_Loading_Progress/Panel_Backdrop").GetComponent<Image>();
                while (LoadingBarPanel == null) yield return null;

                LoadingBarPanel.color = Settings.TextColor() * 5;

                var LoadingBarPanelRight = GameObject.Find("/UserInterface/MenuContent/Popups/LoadingPopup/ProgressPanel/Parent_Loading_Progress/Decoration_Right").GetComponent<Image>();
                while (LoadingBarPanelRight == null) yield return null;

                LoadingBarPanelRight.color = Settings.TextColor() * 5;

                var LoadingBarPanelLeft = GameObject.Find("/UserInterface/MenuContent/Popups/LoadingPopup/ProgressPanel/Parent_Loading_Progress/Decoration_Left").GetComponent<Image>();
                while (LoadingBarPanelLeft == null) yield return null;

                LoadingBarPanelLeft.color = Settings.TextColor() * 5;
            }
        }
    }
}
