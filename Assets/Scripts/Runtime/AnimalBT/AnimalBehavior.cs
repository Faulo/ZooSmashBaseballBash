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
        public CapsuleCollider attachedCollider;
        public Animator attachedAnimator;
        public SpeedTracker attachedTracker;

        public bool wasHit;

        Transform player;

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
                    new CheckIsHit(this),
                    new TaskEnableRotation(attachedRigidbody),
                    new TaskGettingHit(attachedAnimator, this, attachedTracker)
                ),
                new Selector(
                    new Sequence(
                        new CheckIsGrounded(transform, attachedCollider),
                        new Selector(
                            // first, if we can rotate, we must become upright and disable rotation
                            new Sequence(
                                new CheckCanRotate(attachedRigidbody),
                                new TaskFixRotation(attachedRigidbody),
                                new Selector(
                                    new Sequence(
                                        new CheckIsLyingDown(transform),
                                        new TaskStandUp(attachedAnimator)
                                    ),
                                    new Sequence(
                                        new TaskDisableRotation(attachedRigidbody)
                                    )
                                )
                            ),
                            // now we're good to go
                            new Sequence(
                                new Selector(
                                    new Sequence(
                                        new CheckForPlayer(),
                                        new TaskGoToPlayer(transform, attachedAgent, attachedRigidbody, attachedAnimator)
                                    ),
                                    // idle here!
                                    new Sequence()
                                )
                            )
                        )
                    ),
                    // if we got airborne somehow, we should start rigidbodying
                    new Sequence(
                        new TaskEnableRotation(attachedRigidbody),
                        new TaskFlying(attachedAnimator)
                    )
                )
            );

            return root;
        }
    }
}