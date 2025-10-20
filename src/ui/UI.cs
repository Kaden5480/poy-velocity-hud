using System;

using UnityEngine;
using UnityEngine.UI;

namespace VelocityHUD.UI {
    public class UI {
        private GameObject rootObj;
        private TextComponent compMax;
        private TextComponent compCurrent;
        private TextComponent compExtended;

        private Cache.Cache cache {
            get => Plugin.cache;
        }

        private Config config {
            get => Plugin.config;
        }

        private Tracker tracker {
            get => Plugin.tracker;
        }

        /**
         * <summary>
         * Makes all necessary UI objects.
         * </summary>
         */
        public void MakeUI() {
            // Root object
            rootObj = new GameObject("Velocity HUD");
            RectTransform rootTransform = rootObj.AddComponent<RectTransform>();
            rootTransform.SetParent(cache.ui.obj.transform);
            rootTransform.localPosition = new Vector3(-50, -10f, 0f);
            rootTransform.sizeDelta = new Vector2(600f, 70f);

            // Current and max objects
            compMax = new TextComponent(rootObj, "Max Velocity");
            compCurrent = new TextComponent(rootObj, "Current Velocity");

            if (config.showExtended.Value == true) {
                compExtended = new TextComponent(rootObj, "Current Extended");
                RectTransform transform = compExtended.transform;

                Vector2 oldDelta = transform.sizeDelta;
                Vector3 oldPosition = transform.localPosition;

                transform.sizeDelta = new Vector2(
                    2 * oldDelta.x,
                    oldDelta.y
                );
                transform.localPosition =  new Vector3(
                    oldPosition.x + 140,
                    oldPosition.y,
                    oldPosition.z
                );
            }

            // Hide by default
            rootObj.SetActive(false);
        }

        /**
         * <summary>
         * Destroys UI objects.
         * </summary>
         */
        public void DestroyUI() {
            rootObj = null;
            TextComponent.offset = 0;
        }

        /**
         * <summary>
         * Formats the velocity into a more user-friendly string.
         * </summary>
         * <param name="velocity">The velocity to format</param>
         * <returns>The formatted velocity</returns>
         */
        private string FormatVelocity(float velocity) {
            return String.Format("{0:0,0.0000}", velocity);
        }

        /**
         * <summary>
         * Whether the UI should be shown.
         * </summary>
         * <returns>Whether the UI should be shown</returns>
         */
        private bool ShouldShow() {
            return config.showUI.Value
                && cache.timeAttack.watchReady
                && cache.ui.holdsObj.activeSelf;
        }

        /**
         * <summary>
         * Executes each frame to update the state of the UI.
         * </summary>
         */
        public void Update() {
            if (rootObj == null) {
                return;
            }

            rootObj.SetActive(ShouldShow());

            compMax.SetEnabled(true);
            compCurrent.SetEnabled(!TimeAttack.receivingScore);

            if (ShouldShow() == false) {
                return;
            }

            compMax.SetText($"Max: {FormatVelocity(tracker.max)}");
            compCurrent.SetText($"Current: {FormatVelocity(tracker.current.magnitude)}");

            if (compExtended != null) {
                compExtended.SetEnabled(!TimeAttack.receivingScore);

                string x = FormatVelocity(tracker.current.x);
                string y = FormatVelocity(tracker.current.y);
                string z = FormatVelocity(tracker.current.z);
                compExtended.SetText($"Extended: ({x}, {y}, {z})");
            }
        }
    }
}
