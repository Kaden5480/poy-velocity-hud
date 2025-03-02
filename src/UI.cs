using System;

using UnityEngine;
using UnityEngine.UI;

namespace VelocityHUD {
    public class UI {
        private Cfg config;
        private Tracker tracker;

        private GameObject holdsObj;
        private GameObject rootObj;

        private GameObject maxObj;
        private Text maxText;

        private GameObject currentObj;
        private Text currentText;

        public UI(Cfg config) {
            this.config = config;
            tracker = new Tracker(config);
        }

        private string FormatVelocity(float velocity) {
            return String.Format("{0:0,0.0000}", velocity);
        }

        private void MakeUI() {
            const int fontSize = 25;

            GameObject timeAttackObj = GameObject.Find("TimeAttackText");
            if (timeAttackObj == null) {
                return;
            }

            holdsObj = timeAttackObj.transform.Find("holds_image").gameObject;

            Text timeAttackText = timeAttackObj.GetComponent<Text>();
            if (timeAttackText == null) {
                return;
            }

            Outline timeAttackOutline = timeAttackObj.GetComponent<Outline>();
            if (timeAttackOutline == null) {
                return;
            }

            Font gameFont = timeAttackText.font;

            // Root object
            rootObj = new GameObject("Velocity HUD");
            RectTransform rootTransform = rootObj.AddComponent<RectTransform>();
            rootTransform.SetParent(timeAttackObj.transform);
            rootTransform.localPosition = new Vector3(-50, -10f, 0f);
            rootTransform.sizeDelta = new Vector2(600f, 70f);

            // Max magnitude
            maxObj = new GameObject("Max Velocity");
            RectTransform maxTransform = maxObj.AddComponent<RectTransform>();
            maxTransform.SetParent(rootObj.transform);
            maxTransform.localPosition = Vector3.zero;
            maxTransform.sizeDelta = new Vector2(200f, 70f);

            maxText = maxObj.AddComponent<Text>();
            maxText.font = gameFont;
            maxText.fontSize = fontSize;

            Outline maxOutline = maxObj.AddComponent<Outline>();
            maxOutline.effectColor = timeAttackOutline.effectColor;
            maxOutline.effectDistance = timeAttackOutline.effectDistance;

            // Current magnitude
            currentObj = new GameObject("Current Velocity");
            RectTransform currentTransform = currentObj.AddComponent<RectTransform>();
            currentTransform.SetParent(rootObj.transform);
            currentTransform.localPosition = new Vector3(160f, 0f, 0f);
            currentTransform.sizeDelta = new Vector2(200f, 70f);

            currentText = currentObj.AddComponent<Text>();
            currentText.font = gameFont;
            currentText.fontSize = fontSize;

            Outline currentOutline = currentObj.AddComponent<Outline>();
            currentOutline.effectColor = timeAttackOutline.effectColor;
            currentOutline.effectDistance = timeAttackOutline.effectDistance;

            rootObj.SetActive(false);
        }

        private bool IsUIActive() {
            if (holdsObj == null || tracker.timeAttack == null) {
                return false;
            }

            return config.showUI.Value
                && tracker.timeAttack.watchReady
                && holdsObj.activeSelf;
        }

        public void OnSceneLoaded() {
            MakeUI();
            tracker.OnSceneLoaded();
        }

        public void OnSceneUnloaded() {
            tracker.OnSceneUnloaded();

            rootObj = null;
            maxText = null;
            currentText = null;
        }

        public void Update() {
            tracker.Update();

            if (maxText == null
                || currentText == null
                || tracker.timeAttack == null
            ) {
                return;
            }

            rootObj.SetActive(IsUIActive());

            maxObj.SetActive(true);
            currentObj.SetActive(!TimeAttack.receivingScore);

            if (IsUIActive() == false) {
                return;
            }

            string max = FormatVelocity(tracker.max);
            string current = FormatVelocity(tracker.current);

            maxText.text = $"Max: {max}";
            currentText.text = $"Current: {current}";
        }
    }
}
