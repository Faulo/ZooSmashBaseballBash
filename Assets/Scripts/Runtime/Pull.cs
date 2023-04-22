using UnityEngine;

namespace ZSBB {
    sealed class Pull : MonoBehaviour {
        [SerializeField]
        float forceMultiplier = 100;
        [SerializeField]
        ForceMode forceMode = ForceMode.Acceleration;

        void OnTriggerStay(Collider other) {
            if (other.attachedRigidbody is Rigidbody rigidbody) {
                var direction = transform.position - rigidbody.position;
                direction.Normalize();
                rigidbody.AddForce(direction * forceMultiplier, forceMode);
            }
        }
    }
}
