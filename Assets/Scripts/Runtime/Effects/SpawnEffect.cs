using MyBox;
using UnityEngine;

namespace ZSBB.Effects {
    [CreateAssetMenu]
    sealed class SpawnEffect : ScriptableObject, IRelocation {
        [Header(nameof(SpawnEffect))]
        [SerializeField]
        GameObject prefab;

        [Header("Destruction")]
        [SerializeField]
        bool destroyAfterTimeout = false;
        [SerializeField, ConditionalField(nameof(destroyAfterTimeout))]
        float timeout = 1;

        public void ResolveCollision(Relocator relocator, Rigidbody rigidbody, ContactPoint point) {
            var rotation = Quaternion.LookRotation(point.impulse);
            var instance = Instantiate(prefab, point.point, rotation);
            if (destroyAfterTimeout) {
                Destroy(instance, timeout);
            }
        }
    }
}
