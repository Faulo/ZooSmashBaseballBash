using Slothsoft.UnityExtensions;
using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace ZSBB.Player {
    sealed class TrackerParticles : MonoBehaviour {
        [SerializeField]
        SpeedTracker tracker;
        [SerializeField]
        ParticleSystem particles;

        [SerializeField]
        float speedMultiplier = 10;

        EmitParams emission = new();

        void OnValidate() {
            if (!tracker) {
                transform.TryGetComponentInParent(out tracker);
            }
            if (!particles) {
                transform.TryGetComponentInParent(out particles);
            }
        }

        void Update() {
            emission.velocity = tracker.velocity;
            particles.Emit(emission, Mathf.CeilToInt(tracker.speed * tracker.speed * speedMultiplier));
        }
    }
}
