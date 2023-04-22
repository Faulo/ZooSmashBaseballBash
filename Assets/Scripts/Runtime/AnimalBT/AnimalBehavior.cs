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

        public static float speed = 5f;
        public static float weight = 10f;

        public NavMeshAgent attachedAgent;
        public Rigidbody attachedRigidbody;
        public Animator attachedAnimator;

        protected override void Start() {
            base.Start();
            attachedAgent.Warp(attachedRigidbody.position);
            attachedAgent.destination = GameObject.Find("P_Player")
                .transform.position;
        }

        protected override void FixedUpdate() {
            base.FixedUpdate();
            if (attachedAgent.hasPath) {
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
                // If tumbling, reset yourself
                new Sequence(
                    new CheckIsLyingDown(attachedRigidbody, transform),
                    new TaskLanding(attachedAnimator),
                    new TaskStandUp(attachedAnimator, transform)
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
