using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;

#if BEPINEX
using BepInEx;
using BepInEx.Configuration;

namespace VelocityHUD {
    [BepInPlugin("com.github.Kaden5480.poy-velocity-hud", "VelocityHUD", PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin {
        public void Awake() {
            config._toggleKeybind = Config.Bind(
                "General", "toggleKeybind", defaultToggleKeybind.ToString(),
                "Keybind to toggle the UI"
            );
            config.showUI = Config.Bind(
                "General", "showUI", false,
                "Whether to show the UI"
            );

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;

            CommonAwake();
        }

        public void OnDestroy() {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            CommonSceneLoad();
        }

        private void OnSceneUnloaded(Scene scene) {
            CommonSceneUnload();
        }

        public void Update() {
            CommonUpdate();
        }

#elif MELONLOADER
using MelonLoader;
using MelonLoader.Utils;

[assembly: MelonInfo(typeof(VelocityHUD.Plugin), "VelocityHUD", PluginInfo.PLUGIN_VERSION, "Kaden5480")]
[assembly: MelonGame("TraipseWare", "Peaks of Yore")]

namespace VelocityHUD {
    public class Plugin : MelonMod {
        public override void OnInitializeMelon() {
            string filePath = $"{MelonEnvironment.UserDataDirectory}/com.github.Kaden5480.poy-velocity-hud.cfg";

            MelonPreferences_Category general = MelonPreferences.CreateCategory("VelocityHUD_General");
            general.SetFilePath(filePath);

            config._toggleKeybind = general.CreateEntry<string>("toggleKeybind", defaultToggleKeybind.ToString());
            config.showUI = general.CreateEntry<bool>("showUI", false);
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName) {
            CommonSceneLoad();
        }

        public override void OnSceneWasUnloaded(int buildIndex, string sceneName) {
            CommonSceneUnload();
        }

        public override void OnUpdate() {
            CommonUpdate();
        }

#endif

        private const KeyCode defaultToggleKeybind = KeyCode.Mouse2;
        private Cfg config;
        private UI ui;

        private void CommonAwake() {
            ui = new UI(config);
        }

        private void CommonSceneLoad() {
            ui.OnSceneLoaded();
        }

        private void CommonSceneUnload() {
            ui.OnSceneUnloaded();
        }

        private void CommonUpdate() {
            ui.Update();
        }
    }
}
