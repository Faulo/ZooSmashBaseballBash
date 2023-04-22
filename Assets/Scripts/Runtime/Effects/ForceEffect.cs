using UnityEngine;

namespace ZSBB.Effects {
    [CreateAssetMenu]
    sealed class ForceEffect : ScriptableObject, IRelocation {
        [Header(nameof(ForceEffect))]
        [SerializeField]
        float forceMultiplier = 100;

        public void ResolveCollision(Relocator relocator, Rigidbody rigidbody, ContactPoint point) {
            var force = point.impulse * forceMultiplier;
            rigidbody.AddForceAtPosition(force, point.point);
        }
    }
}
