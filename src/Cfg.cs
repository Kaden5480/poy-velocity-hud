using System;
using UnityEngine;

#if BEPINEX
using BepInEx.Configuration;

#elif MELONLOADER
using MelonLoader;

#endif

namespace VelocityHUD {
    public struct Cfg {
#if BEPINEX
        public ConfigEntry<string> _toggleKeybind;
        public ConfigEntry<bool> showUI;

#elif MELONLOADER
        public MelonPreferences_Entry<string> _toggleKeybind;
        public MelonPreferences_Entry<bool> showUI;

#endif

        public KeyCode toggleKeybind {
            get => (KeyCode) Enum.Parse(typeof(KeyCode), _toggleKeybind.Value);
            set => _toggleKeybind.Value = value.ToString();
        }
    }
}
