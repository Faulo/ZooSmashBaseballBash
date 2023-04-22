using UnityEngine;
using UnityEngine.AI;
using ZSBB.BehaviorTree;

namespace ZSBB.AnimalBT {
    enum AnimationStates {
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

        public NavMeshAgent attachedAgent;
        public Rigidbody attachedRigidbody;
        public Animator attachedAnimator;

        protected override void Start() {
            attachedAgent.updatePosition = false;
            attachedAgent.Warp(attachedRigidbody.position);
            base.Start();
        }

        protected override void FixedUpdate() {
            base.FixedUpdate();
            if (attachedAgent.hasPath) {
                Debug.Log($"desiredVelocity is {attachedAgent.desiredVelocity}, warping to {attachedRigidbody.position}");
                attachedAgent.nextPosition = attachedRigidbody.position;
            }
        }

        protected override Node SetupTree() {
            Node root = new Selector(
                // If Grounded, Find player and goto player
                new Sequence(
                    new CheckIsGrounded(transform, attachedRigidbody, attachedAgent),
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
