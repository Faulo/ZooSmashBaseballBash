using System.Collections.Generic;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace ZSBB {
    sealed class TractorBeam : MonoBehaviour {
        Relocator relocator => Relocator.instance;

        [SerializeField]
        SphereCollider attachedCollider;
        Vector3 worldCenter => transform.position + attachedCollider.center;
        [SerializeField]
        Vector2 tractorMultiplier = Vector2.one;
        [SerializeField]
        Vector2 relocatorMultiplier = Vector2.one;
        [SerializeField]
        Vector2 homingMultiplier = Vector2.one;
        [SerializeField]
        ForceMode forceMode = ForceMode.Acceleration;

        void OnValidate() {
            if (!attachedCollider) {
                TryGetComponent(out attachedCollider);
            }
        }

        readonly HashSet<Rigidbody> rigidbodies = new();
        void Add(Rigidbody rigidbody) {
            rigidbodies.Add(rigidbody);
            if (enabled) {
                rigidbody.useGravity = false;
            }
        }
        void Remove(Rigidbody rigidbody) {
            rigidbodies.Remove(rigidbody);
            rigidbody.useGravity = true;
        }

        void FixedUpdate() {
            foreach (var rigidbody in rigidbodies) {
                var force = CalculateForce(
                    (worldCenter - rigidbody.position).normalized,
                    tractorMultiplier
                );
                if (relocator.topTracker is SpeedTracker tracker && tracker.isMoving) {
                    force += CalculateForce(tracker.direction, relocatorMultiplier);
                    var homingDirection = tracker.position - rigidbody.position;
                    homingDirection.y = homingDirection.SwizzleXZ().magnitude;
                    force += CalculateForce(homingDirection.normalized, homingMultiplier * tracker.speed);
                }
                rigidbody.AddForce(force, forceMode);
            }
        }

        static Vector3 CalculateForce(Vector3 direction, Vector2 multiplier) {
            return (direction.SwizzleXZ() * multiplier.x)
                .SwizzleXZ()
                .WithY(direction.y * multiplier.y);
        }

        void OnEnable() {
            foreach (var rigidbody in rigidbodies) {
                rigidbody.useGravity = false;
            }
        }

        void OnDisable() {
            foreach (var rigidbody in rigidbodies) {
                rigidbody.useGravity = true;
            }
            rigidbodies.Clear();
        }

        void OnTriggerEnter(Collider other) {
            if (other.attachedRigidbody is Rigidbody rigidbody) {
                Add(rigidbody);
            }
        }

        void OnTriggerExit(Collider other) {
            if (other.attachedRigidbody is Rigidbody rigidbody) {
                Remove(rigidbody);
            }
        }
    }
}
