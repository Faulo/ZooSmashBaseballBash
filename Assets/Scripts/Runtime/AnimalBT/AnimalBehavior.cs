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

    sealed class AnimalBehavior : BTree, IRelocationMessages {
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
            var fixRotation = new TaskFixRotation(attachedRigidbody);

            Node root = new Selector(
                // if we were hit, we gotta feel the pain
                new Sequence(
                    new CheckIsHit(this),
                    new TaskGettingHit(attachedAnimator, this, attachedTracker)
                ),
                new Selector(
                    new Sequence(
                        // if we're grounded, we wanna stop this physics nonsense and start rotating us ourselves
                        new CheckIsGrounded(transform, attachedCollider),
                        new TaskDisableRotation(attachedRigidbody),
                        fixRotation,
                        new Selector(
                            // first, if we can rotate, we must become upright
                            new Sequence(
                                new CheckIsLyingDown(transform),
                                new TaskStandUp(attachedAnimator)
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
                        new Selector(
                            // if we're airborne and moving, we should keep flying
                            new Sequence(
                                new CheckIsMoving(attachedTracker),
                                new TaskEnableRotation(attachedRigidbody),
                                new TaskFlying(attachedAnimator)
                            ),
                            // otherwise, let's try to get grounded
                            new Sequence(
                                new TaskDisableRotation(attachedRigidbody),
                                fixRotation
                            )
                        )
                    )
                )
            );

            return root;
        }

        public void OnHit() {
            attachedRigidbody.freezeRotation = false;
            wasHit = true;
        }
    }
}