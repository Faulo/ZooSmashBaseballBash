using Slothsoft.UnityExtensions;
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

        NavMeshAgent attachedAgent;
        Rigidbody attachedRigidbody;
        Animator attachedAnimator;

        protected override void Start() {
            UpdateComponents();
            base.Start();
        }
        void UpdateComponents() {
            transform.TryGetComponentInChildren(out attachedAgent);
            transform.TryGetComponentInChildren(out attachedRigidbody);
            transform.TryGetComponentInChildren(out attachedAnimator);
        }

        protected override Node SetupTree() {
            Node root = new Selector(
                // If Grounded, Find player and goto player
                new Sequence(
                    new CheckIsGrounded(transform, attachedAgent),
                    new CheckForPlayer(attachedAgent),
                    new TaskGoToPlayer(transform, attachedAgent, attachedRigidbody, attachedAnimator)
                ),
                // If in the Air, play Flying Animation
                new Sequence(
                    new TaskFlying(attachedAnimator)
                )
            );

            return root;
        }
    }
}
