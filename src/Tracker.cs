using UnityEngine;

namespace VelocityHUD {
    public class Tracker {
        private Cfg config;

        private PlayerMove playerMove = null;
        private Rigidbody playerRb = null;
        public TimeAttack timeAttack = null;

        private float _max = 0f;

        public float current {
            get => playerRb.velocity.magnitude;
        }
        public float max {
            get => _max;
        }

        public Tracker(Cfg config) {
            this.config = config;
        }

        public void OnSceneLoaded() {
            playerMove = GameObject.FindObjectOfType<PlayerMove>();
            timeAttack = GameObject.FindObjectOfType<TimeAttack>();
            if (playerMove == null) {
                return;
            }

            playerRb = playerMove.rigid;
        }

        public void OnSceneUnloaded() {
            playerMove = null;
            timeAttack = null;
            playerRb = null;
        }

        private void PlayAnimation() {
            timeAttack.pocketwatchSound.volume = 0.48f;
            timeAttack.pocketwatchSound.pitch = 1.1f;
            timeAttack.pocketwatchSound.clip = timeAttack.s_stopTime;
            timeAttack.pocketwatchSound.Play();
            timeAttack.pocketWatchAnim.Play("pocketwatch_click");
        }

        public void Update() {
            if (playerMove == null
                || playerRb == null
                || timeAttack == null
            ) {
                return;
            }

            if (timeAttack.isOpenNow == true) {
                if (Input.GetKeyDown(config.toggleKeybind) == true) {
                    config.showUI.Value = !config.showUI.Value;
                    PlayAnimation();
                }
            }

            if (playerMove.IsGrounded() == true
                && timeAttack.isInColliderActivationRange == true
            ) {
                _max = 0f;
            }

            if (current > _max) {
                _max = current;
            }
        }
    }
}
