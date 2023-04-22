using UnityEngine;
using UnityEngine.AI;
using ZSBB.BehaviorTree;

namespace ZSBB.AnimalBT {
    public enum AnimationStates {
        Attack,
        Bounce,
        Clicked,
        Eat,
        Idle_A,
        Idle_B,
        Idle_C,
        Sit,
        Swim,
        Roll,
        Walk,
        Fly,
        Death,
        Fear,
        Hit,
        Jump,
        Run,
        Spin
    }
    sealed class AnimalBehavior : BTree {

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
            Node root = new Selector(
                // If Grounded, Find player and goto player
                new Sequence(
                    new CheckIsGrounded(transform),
                    new CheckForPlayer(agent),
                    new TaskGoToPlayer(transform, agent, attachedRigidbody)
                ),
                // If in the Air, play Flying Animation
                new Sequence(
                    new TaskFlying(transform)
                )
            );

            return root;
        }
    }
}
