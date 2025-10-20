using UnityEngine;
using UnityEngine.UI;

namespace VelocityHUD.Cache {
    public class TimeUI {
        public GameObject obj  { get; private set; }
        public Font font       { get; private set; }
        public Outline outline { get; private set; }
        public Text text       { get; private set; }

        public GameObject holdsObj { get; private set; }

        /**
         * <summary>
         * Constructs a new TimeUI object.
         * </summary>
         */
        public TimeUI() {
            obj = GameObject.Find("TimeAttackText");
            if (obj == null) {
                return;
            }

            Transform holdsTransform = obj.transform.Find("holds_image");
            if (holdsTransform != null) {
                holdsObj = holdsTransform.gameObject;
            }

            text = obj.GetComponent<Text>();
            outline = obj.GetComponent<Outline>();

            if (text != null) {
                font = text.font;
            }
        }

        /**
         * <summary>
         * Checks whether all UI objects are accessible.
         * </summary>
         */
        public bool IsComplete() {
            return obj != null
                && font != null
                && outline != null
                && text != null
                && holdsObj != null;
        }
    }
}
