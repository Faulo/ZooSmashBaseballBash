using MyBox;
using UnityEngine;

namespace ZSBB {
    sealed class Pull : MonoBehaviour {
        [SerializeField]
        float forceMultiplier = 100;
        [SerializeField]
        ForceMode forceMode = ForceMode.Acceleration;

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

        void OnTriggerStay(Collider other) {
            if (isPulling && other.attachedRigidbody is Rigidbody rigidbody) {
                var direction = transform.position - rigidbody.position;
                direction.Normalize();
                rigidbody.AddForce(direction * forceMultiplier, forceMode);
            }
        }
    }
}
