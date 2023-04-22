using UnityEngine;
using UnityEngine.Events;

namespace ZSBB {
    sealed class Relocator : MonoBehaviour {
        [Header("MonoBehaviour configuration")]
        [SerializeField]
        Rigidbody attachedRigidbody;
        [SerializeField]
        CapsuleCollider attachedCollider;

        [Header("Events")]
        [SerializeField]
        UnityEvent<Relocator, Rigidbody, ContactPoint> onCollision = new();

        void OnValidate() {
            if (!attachedRigidbody) {
                TryGetComponent(out attachedRigidbody);
            }
            if (!attachedCollider) {
                TryGetComponent(out attachedCollider);
            }
        }

        int contactCount;
        ContactPoint[] contacts = new ContactPoint[8];

        void OnCollisionEnter(Collision collision) {
            if (collision.rigidbody is Rigidbody rigidbody) {
                contactCount = collision.GetContacts(contacts);
                for (int i = 0; i < contactCount; i++) {
                    onCollision.Invoke(this, rigidbody, contacts[i]);
                }
            }
        }
    }
}
