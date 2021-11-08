using System;
using TMPro;
using UnhollowerBaseLib.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace Evolve.Modules
{
    public class Nameplate : MonoBehaviour
    {
        public Graphic IconBackground;
        public RawImage Icon;
        public GameObject IconContainer;
        public GameObject Content;
        public GameObject QuickStats;
        public ImageThreeSlice NameBackground;
        public ImageThreeSlice QuickStatsBackground;
        public TextMeshProUGUI Name;
        public ImageThreeSlice NamePulse;
        public Image IconPulse;
        public ImageThreeSlice NameGlow;
        public Image IconGlow;
        public TextMeshProUGUI IconText;
        private PlayerNameplate PN = null;
        private Color NameColor;
        private Color NameColor2;
        public bool IsEvolved = false;
        private bool SetColor;
        private bool Evolved;
        public Nameplate(IntPtr ptr) : base(ptr) { }

        [HideFromIl2Cpp]
        public void SN(PlayerNameplate nameplate)
        {
            this.PN = nameplate;
        }

        [HideFromIl2Cpp]
        public void SNC(Color color)
        {
            this.NameColor = color;
            SetColor = true;
        }

        [HideFromIl2Cpp]
        public void Reset()
        {
            SetColor = false;
            Evolved = false;
        }

        [HideFromIl2Cpp]
        public void ORbld()
        {
            if (PN != null) Name.color = NameColor;
        }

        public void Update()
        {
            Name.color = NameColor;

            if (IsEvolved)
            {
                IconBackground.enabled = true;
                Icon.enabled = true;
                Icon.gameObject.SetActive(true);
                IconContainer.SetActive(true);
                IconContainer.SetActive(true);
                Icon.texture = CustomNameplates.EvolveIcon;
                IconText.gameObject.SetActive(false);

            }
        }

    }
}
