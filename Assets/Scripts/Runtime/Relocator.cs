using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace ZSBB {
    sealed class Relocator : MonoBehaviour {
        [Header("MonoBehaviour configuration")]
        [SerializeField]
        Rigidbody attachedRigidbody;
        [SerializeField]
        CapsuleCollider attachedCollider;
        [SerializeField]
        SpeedTracker baseTracker;
        [SerializeField]
        SpeedTracker topTracker;
        [SerializeField]
        GameObject pullObject;

        Vector3 GetVelocityAt(Vector3 position) {
            float t = InverseLerp(baseTracker.position, topTracker.position, position);
            return Vector3.Lerp(baseTracker.velocity, topTracker.velocity, t);
        }

        static float InverseLerp(Vector3 a, Vector3 b, Vector3 value) {
            var ab = b - a;
            var av = value - a;
            return Vector3.Dot(av, ab) / ab.sqrMagnitude;
        }

        [Header("Events")]
        [SerializeField]
        UnityEvent<CollisionInfo> onCollision = new();

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
                    var position = contacts[i].point;
                    var rotation = Quaternion.LookRotation(contacts[i].normal);
                    var velocity = GetVelocityAt(position);
                    onCollision.Invoke(new(
                        rigidbody,
                        position,
                        rotation,
                        velocity
                    ));
                }
            }
        }

        public void OnPull(InputValue value) {
            pullObject.SetActive(value.isPressed);
        }
    }
}
