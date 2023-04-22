using UnityEngine;

namespace ZSBB.Effects {
    [CreateAssetMenu]
    sealed class ForceEffect : ScriptableObject, IRelocation {
        [Header(nameof(ForceEffect))]
        [SerializeField]
        float forceMultiplier = 100;
        [SerializeField]
        ForceMode forceMode = ForceMode.VelocityChange;

        public void ResolveCollision(CollisionInfo collision) {
            var force = collision.velocity * forceMultiplier;
            collision.rigidbody.AddForceAtPosition(force, collision.position, forceMode);
        }
    }
}
