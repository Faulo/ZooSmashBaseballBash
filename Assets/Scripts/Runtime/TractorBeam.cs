using System.Collections.Generic;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace ZSBB {
    sealed class TractorBeam : MonoBehaviour {
        [SerializeField]
        SphereCollider attachedCollider;
        Vector3 worldCenter => transform.position + attachedCollider.center;
        [SerializeField]
        Vector2 forceMultiplier = Vector2.one;
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
            rigidbody.useGravity = false;
        }
        void Remove(Rigidbody rigidbody) {
            rigidbodies.Remove(rigidbody);
            rigidbody.useGravity = true;
        }

        void FixedUpdate() {
            foreach (var rigidbody in rigidbodies) {
                var direction = worldCenter - rigidbody.position;
                direction.Normalize();
                var force = (direction.SwizzleXZ() * forceMultiplier.x)
                    .SwizzleXZ()
                    .WithY(direction.y * forceMultiplier.y);
                rigidbody.AddForce(force, forceMode);
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
