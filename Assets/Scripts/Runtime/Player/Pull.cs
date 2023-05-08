using MyBox;
using UnityEngine;

namespace ZSBB {
    sealed class Pull : MonoBehaviour {
        [SerializeField]
        TractorBeam beam;
        [SerializeField]
        ParticleSystem selecting;
        [SerializeField]
        ParticleSystem pulling;

        [SerializeField, ReadOnly]
        public bool isPulling;
        [SerializeField, ReadOnly]
        public bool isPrepared;
        [SerializeField, Range(0, 1), ReadOnly]
        public float normalizedDistance;

        public Vector3 position {
            get => transform.position;
            set => transform.position = value;
        }

        void OnValidate() {
            if (!beam) {
                TryGetComponent(out beam);
            }
        }

        void FixedUpdate() {
            beam.enabled = isPrepared && isPulling;
        }

        void Update() {
            if (selecting.emission is ParticleSystem.EmissionModule sEmission) {
                sEmission.enabled = isPrepared && !isPulling;
            }

            if (pulling.emission is ParticleSystem.EmissionModule pEmission) {
                pEmission.enabled = isPrepared && isPulling;
            }
        }
    }
}
