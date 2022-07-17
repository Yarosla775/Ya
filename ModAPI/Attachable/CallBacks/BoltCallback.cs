﻿using System;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GUILayout;
using static TommoJProductions.ModApi.ModClient;

namespace TommoJProductions.ModApi.Attachable.CallBacks
{
    /// <summary>
    /// Represents a callback for bolts.
    /// </summary>
    public class BoltCallback : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        // Written, 02.07.2022

        #region Events

        /// <summary>
        /// Represents the on bolt exit event.
        /// </summary>
        public event Action<BoltCallback, PointerEventData> onMouseExit;
        /// <summary>
        /// Represents the on bolt enter event.
        /// </summary>
        public event Action<BoltCallback, PointerEventData> onMouseEnter;

        #endregion

        #region Fields

        private MeshRenderer boltRenderer;
        private Material boltMaterial;

        /// <summary>
        /// Represents the bolt that this callback is linked to.
        /// </summary>
        public Bolt bolt;

        internal static BoltCallback inspectionBolt;
        private bool inspectionSet = false;
        internal static Vector3 inspectionPosition;
        internal static Vector3 inspectionEuler;
        internal static float inspectionOffset;

        /// <summary>
        /// The bolt size for this callback.
        /// </summary>
        public Bolt.BoltSize boltSize { get; internal set; }
        /// <summary>
        /// Represents the bolt check. checks if in tool mode and that the player is holding the correct tool for the fastener.
        /// </summary>
        public bool boltCheck => !isInHandMode && getToolWrenchSize_boltSize == boltSize;

        #endregion

        #region unity runtime

        public void Awake()
        {
            boltRenderer = GetComponent<MeshRenderer>();
            boltMaterial = boltRenderer.material;
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            if (boltCheck)
            {
                if (boltRenderer)
                    boltRenderer.material = getActiveBoltMaterial;
                onMouseEnter?.Invoke(this, eventData);
            }
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            if (boltRenderer)
                boltRenderer.material = boltMaterial;
            onMouseExit?.Invoke(this, eventData);
        }
        
        void OnGUI()
        {
            // For development purposes.

            if (inspectionBolt == this)
            {
                if (!inspectionSet)
                {
                    inspectionPosition = bolt.startPosition;
                    inspectionEuler = bolt.startEulerAngles;
                    inspectionOffset = bolt.boltSettings.addNutSettings.nutOffset;
                }
                inspectionSet = true;
                using (AreaScope area = new AreaScope(new Rect(10, 10, 300, Screen.height - (10 * 2))))
                {
                    drawBoltGui();
                }
            }
            else if (inspectionSet)
            {
                inspectionSet = false;
            }
        }

        #endregion

        internal void drawBoltGui()
        {
            using (new VerticalScope("box"))
            {
                using (new VerticalScope("box"))
                {
                    drawProperty("Bolt Inspection", name);
                    Space(1);
                    drawProperty("routine", bolt.boltRoutine == null ? "null" : "active");
                    bolt.boltSettings.drawProperty("boltType");
                    drawProperty("Bolt Tightness", bolt.loadedSaveInfo.boltTightness);
                    bolt.boltSettings.drawProperty("boltSize");
                    if (bolt.boltSettings.addNut)
                    {
                        drawProperty("Nut Tightness", bolt.loadedSaveInfo.addNutTightness);
                        bolt.boltSettings.addNutSettings.drawProperty("nutSize");
                    }
                }
                Space(1);
                using (new VerticalScope("box"))
                {
                    drawPropertyVector3("start position", ref inspectionPosition);
                    drawPropertyVector3("start euler", ref inspectionEuler);
                    if (bolt.boltSettings.addNut)
                        drawPropertyEdit("Nut Offset", ref inspectionOffset);
                    if (Button("apply"))
                    {
                        bolt.startPosition = inspectionPosition;
                        bolt.startEulerAngles = inspectionEuler;
                        bolt.boltSettings.addNutSettings.nutOffset = inspectionOffset;
                        bolt.updateNutPosRot();
                    }
                }
                Space(1);
                using (new VerticalScope("box"))
                {
                    drawPropertyVector3("pos direction", ref bolt.boltSettings.posDirection);
                    drawPropertyVector3("rot direction", ref bolt.boltSettings.rotDirection);
                    drawPropertyEdit("pos step", ref bolt.boltSettings.posStep);
                    drawPropertyEdit("rot step", ref bolt.boltSettings.rotStep);
                    drawPropertyEdit("tightness step", ref bolt.boltSettings.tightnessStep);
                }
                Space(1);
                using (new HorizontalScope("box"))
                {
                    if (bolt.part)
                    {
                        if (Button((bolt.part.boltParent.activeInHierarchy ? "Deactivate" : "Activate") + " bolts"))
                            bolt.part.boltParent.SetActive(!bolt.part.boltParent.activeInHierarchy);
                    }
                    if (Button("Teleport to bolt"))
                    {
                        Camera.main.gameObject.teleport(gameObject.transform.position);
                    }
                }
            }
        }
    }
}
