using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;

using Cfg = VelocityHUD.Config;

namespace VelocityHUD {
    [BepInPlugin("com.github.Kaden5480.poy-velocity-hud", "VelocityHUD", PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin {
        public static Cfg config        { get; private set; } = new Cfg();
        public static Cache.Cache cache { get; private set; } = new Cache.Cache();
        public static Tracker tracker   { get; private set; } = new Tracker();

        private const KeyCode defaultToggleKeybind = KeyCode.Mouse2;

        private UI.UI ui;

        /**
         * <summary>
         * Executes when the plugin is being loaded.
         * </summary>
         */
        public void Awake() {
            config._toggleKeybind = Config.Bind(
                "General", "toggleKeybind", defaultToggleKeybind.ToString(),
                "Keybind to toggle the UI"
            );
            config.showUI = Config.Bind(
                "General", "showUI", false,
                "Whether to show the UI"
            );
            config.showExtended = Config.Bind(
                "General", "showExtended", false,
                "Whether to show extended velocity info"
            );

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;

            ui = new UI.UI();
        }

        /**
         * <summary>
         * Executes when the plugin is destroyed.
         * </summary>
         */
        public void OnDestroy() {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        /**
         * <summary>
         * Executes when a scene is loaded.
         * </summary>
         * <param name="scene">The scene which loaded</param>
         * <param name="mode">The mode the scene loaded with</param>
         */
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            cache.FindObjects();

            if (cache.IsComplete() == false) {
                return;
            }

            ui.MakeUI();
        }

        /**
         * <summary>
         * Executes when a scene is unloaded.
         * </summary>
         * <param name="scene">The scene which unloaded</param>
         */
        private void OnSceneUnloaded(Scene scene) {
            ui.DestroyUI();
            cache.Clear();
        }

        /**
         * <summary>
         * Plays an animation for toggling the UI.
         * </summary>
         */
        private void PlayAnimation() {
            TimeAttack timeAttack = cache.timeAttack;

            timeAttack.pocketwatchSound.volume = 0.48f;
            timeAttack.pocketwatchSound.pitch = 1.1f;
            timeAttack.pocketwatchSound.clip = timeAttack.s_stopTime;
            timeAttack.pocketwatchSound.Play();
            timeAttack.pocketWatchAnim.Play("pocketwatch_click");
        }

        /**
         * <summary>
         * Executes each frame.
         * </summary>
         */
        public void Update() {
            if (cache.IsComplete() == false) {
                return;
            }

            if (cache.timeAttack.isOpenNow == true
                && Input.GetKeyDown(config.toggleKeybind) == true
            ) {
                config.showUI.Value = !config.showUI.Value;
                PlayAnimation();
            }

            tracker.Update();
            ui.Update();
        }
    }
}
