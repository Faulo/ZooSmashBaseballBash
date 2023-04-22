using MyBox;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace ZSBB {
    sealed class Relocator : MonoBehaviour {
        public static Relocator instance { get; private set; }
        [Header("MonoBehaviour configuration")]
        [SerializeField]
        Rigidbody attachedRigidbody;
        [SerializeField]
        CapsuleCollider attachedCollider;
        [SerializeField]
        SpeedTracker baseTracker;
        [SerializeField]
        public SpeedTracker topTracker;

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

        void OnEnable() {
            instance = this;
            pullInstance = Instantiate(pullPrefab);
        }

        void OnDisable() {
            instance = null;
            Destroy(pullInstance);
        }


        void FixedUpdate() {
            if (!pullInstance.isPulling) {
                UpdatePullPosition();
            }
        }

        int contactCount;
        ContactPoint[] contacts = new ContactPoint[8];

        void OnCollisionEnter(Collision collision) {
            if (collision.rigidbody is Rigidbody rigidbody) {
                contactCount = collision.GetContacts(contacts);
                rigidbody.SendMessage(nameof(IRelocationMessages.OnHit), SendMessageOptions.DontRequireReceiver);
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

        [Header("Pull")]
        [SerializeField]
        Pull pullPrefab;
        [SerializeField, ReadOnly]
        Pull pullInstance;
        [SerializeField]
        float pullCastRadius = 1;
        [SerializeField]
        float pullCastDistance = 100;
        [SerializeField]
        LayerMask pullCastLayers = default;

        Vector3 pullForward => (topTracker.position - baseTracker.position).normalized;

        int pullCastCount;
        RaycastHit[] pullCastHits = new RaycastHit[16];
        void UpdatePullPosition() {
            pullCastCount = Physics.SphereCastNonAlloc(baseTracker.position, pullCastRadius, pullForward, pullCastHits, pullCastDistance, pullCastLayers, QueryTriggerInteraction.Ignore);

            pullInstance.isPrepared = pullCastCount > 0;

            float distance = pullCastDistance;
            for (int i = 0; i < pullCastCount; i++) {
                if (distance > pullCastHits[i].distance) {
                    distance = pullCastHits[i].distance;
                    pullInstance.position = pullCastHits[i].point;
                    pullInstance.normalizedDistance = distance / pullCastDistance;
                }
            }
        }

        public void OnPull(InputValue value) {
            pullInstance.isPulling = pullInstance.isPrepared && value.isPressed;
        }
    }
}
