using UnityEngine;

namespace VelocityHUD.Cache {
    public class Cache {
        public PlayerMove playerMove   { get; private set; }
        public Rigidbody playerRb      { get; private set; }
        public TimeAttack timeAttack   { get; private set; }

        public TimeUI ui               { get; private set; }

        /**
         * <summary>
         * Finds objects in the scene necessary for this mod to work.
         * </summary>
         */
        public void FindObjects() {
            playerMove = GameObject.FindObjectOfType<PlayerMove>();
            if (playerMove != null) {
                playerRb = playerMove.rigid;
            }

            timeAttack = GameObject.FindObjectOfType<TimeAttack>();
            ui = new TimeUI();
        }

        /**
         * <summary>
         * Checks whether all objects are accessible.
         * </summary>
         * <returns>True if they are, false otherwise</returns>
         */
        public bool IsComplete() {
            return playerMove != null
                && playerRb != null
                && timeAttack != null
                && ui != null
                && ui.IsComplete() == true;
        }

        /**
         * <summary>
         * Clears the cache
         * </summary>
         */
        public void Clear() {
            playerMove = null;
            playerRb = null;
            timeAttack = null;
            ui = null;
        }
    }
}
