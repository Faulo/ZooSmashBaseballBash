using System.Collections.Generic;
using AnimalBT;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorTree {
    sealed class AnimalBT : BTree {

        public static float speed = 1f;
        public static float weight = 10f;

        [SerializeField] NavMeshAgent agent;
        [SerializeField] Rigidbody attachedRigidbody;

        void OnValidate() {
            if (!agent) {
                TryGetComponent(out agent);
            }
            if (!attachedRigidbody) {
                TryGetComponent(out attachedRigidbody);
            }
        }

        protected override Node SetupTree() {
            Node root = new Selector(new List<Node> {
                // If Grounded, Find player and goto player
                new Sequence( new List<Node> {
                    new CheckIsGrounded(transform),
                    new CheckForPlayer(agent),
                    new TaskGoToPlayer(transform, agent, attachedRigidbody)
                }),
                new Sequence(new List<Node> {
                    new TaskFlying(transform)
                })
            });

            return root;
        }
    }
}
