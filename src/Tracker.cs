using UnityEngine;

namespace VelocityHUD {
    public class Tracker {
        public Vector3 current {
            get => Plugin.cache.playerRb.velocity;
        }

        private float _max = 0f;
        public float max {
            get => _max;
        }

        public void Update() {
            if (Plugin.cache.playerMove.IsGrounded() == true
                && Plugin.cache.timeAttack.isInColliderActivationRange == true
            ) {
                _max = 0f;
            }

            float magnitude = current.magnitude;
            if (magnitude > _max) {
                _max = magnitude;
            }
        }
    }
}
