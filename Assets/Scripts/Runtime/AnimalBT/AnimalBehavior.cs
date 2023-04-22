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
        public Collider attachedCollider;
        public Animator attachedAnimator;

        private Transform player;

        protected override void Start() {
            base.Start();
            attachedAgent.Warp(attachedRigidbody.position);
            player = GameObject.Find("P_Player").transform;
            attachedAgent.destination = player.position;
        }

        protected override void FixedUpdate() {
            base.FixedUpdate();
            if (attachedAgent.hasPath) {
                attachedAgent.nextPosition = attachedRigidbody.position;
            }
        }

        protected override Node SetupTree() {
            Node root = new Selector(
                // Gets hit
                new Sequence(
                    new CheckIsHit(transform),
                    new TaskGettingHit(attachedAnimator)
                ),
                new Sequence(
                    new CheckForPlayer(),
                    new CheckIsNotFacingPlayer(transform),
                    new TaskFixRotation(transform)),
                // If Grounded, Find player and goto player
                new Sequence(
                    new CheckIsGrounded(transform, attachedRigidbody, attachedCollider),
                    new CheckForPlayer(),
                    new TaskGoToPlayer(transform, attachedAgent, attachedRigidbody, attachedAnimator)
                ),
                // If tumbling, reset yourself
                new Sequence(
                    new CheckIsLanding(attachedRigidbody),
                    new TaskLanding(attachedAnimator)
                ),
                new Sequence(
                    new CheckIsGrounded(transform, attachedRigidbody, attachedCollider),
                    new CheckIsLyingDown(transform),
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