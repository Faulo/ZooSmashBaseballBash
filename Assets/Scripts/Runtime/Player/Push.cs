using System.Collections.Generic;
using UnityEngine;

namespace ZSBB.Player {
    sealed class Push : MonoBehaviour {
        Relocator relocator => Relocator.instance;

        [SerializeField]
        float relocatorMultiplier = 1;
        [SerializeField]
        float relocatorSmoothing = 1;

        record Info() {
            public Vector3 acceleration;
        }

        readonly Dictionary<Rigidbody, Info> rigidbodies = new();

        void FixedUpdate() {
            if (relocator.topTracker is SpeedTracker tracker && tracker.isMoving) {
                foreach (var (rigidbody, info) in rigidbodies) {
                    var currentVelocity = rigidbody.velocity;
                    var targetVelocity = tracker.velocity * relocatorMultiplier;

                    rigidbody.velocity = Vector3.SmoothDamp(
                        currentVelocity,
                        targetVelocity,
                        ref info.acceleration,
                        relocatorSmoothing
                    );
                }
            }
        }

        void OnTriggerEnter(Collider other) {
            if (other.attachedRigidbody is Rigidbody rigidbody) {
                rigidbodies[rigidbody] = new();
            }
        }

        void OnTriggerExit(Collider other) {
            if (other.attachedRigidbody is Rigidbody rigidbody) {
                rigidbodies.Remove(rigidbody);
            }
        }
    }
}
