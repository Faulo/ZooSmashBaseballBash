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

        public void ResolveCollision(CollisionInfo collision) {
            var instance = Instantiate(prefab, collision.position, collision.rotation);
            if (destroyAfterTimeout) {
                Destroy(instance, timeout);
            }
        }
    }
}
