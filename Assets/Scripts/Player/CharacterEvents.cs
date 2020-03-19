using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Assets.Scripts.Player
{
    public sealed class CharacterEvents : MonoBehaviour
    {
        [Header("Events")]
        #region Events
        public BoolEvent OnGroundedEvent;
        public BoolEvent OnSlideEvent;
        public UnityEvent OnDoubleJumpEvent;
        public BoolEvent OnGlideEvent;
        public UnityEvent OnAttackEndEvent;
        public UnityEvent OnThrowEndEvent;
        #endregion

        private void Awake()
        {
            // Initialize events
            if (OnGroundedEvent == null)
                OnGroundedEvent = new BoolEvent();

            if (OnSlideEvent == null)
                OnSlideEvent = new BoolEvent();

            if (OnDoubleJumpEvent == null)
                OnDoubleJumpEvent = new UnityEvent();

            if (OnGlideEvent == null)
                OnGlideEvent = new BoolEvent();

            if (OnAttackEndEvent == null)
                OnAttackEndEvent = new UnityEvent();

            if (OnThrowEndEvent == null)
                OnThrowEndEvent = new UnityEvent();
        }
    }

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool>
    {

    }
}
