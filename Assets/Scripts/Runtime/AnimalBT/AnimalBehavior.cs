using UnityEngine;
using UnityEngine.AI;
using ZSBB.BehaviorTree;
using ZSBB.Level;

namespace ZSBB.AnimalBT {
    public enum AnimalCagePreference {
        PRISON,
        WHIMSICAL,
        PLAYGRUND,
        SLOTH_ONLY,
        NO_PREFERENCE
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

        public bool isCaged;

        public AnimalCagePreference cagePreference = AnimalCagePreference.NO_PREFERENCE;

        protected override void Start() {
            base.Start();
            attachedAgent.Warp(attachedRigidbody.position);
            if (Tower.instance) {
                attachedAgent.destination = Tower.instance.transform.position;
            }
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
                // if caged start dacing upright
                new Sequence(
                    new CheckIsCaged(this),
                    new CheckIsGrounded(transform, attachedCollider),
                    new TaskDisableRotation(attachedRigidbody),
                    fixRotation,
                    new TaskEnjoyCageLife(attachedAnimator)),
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
                                        new Selector(
                                            new Sequence(
                                                // attack! attack! attack!
                                                new CheckIsInAttackRange(transform),
                                                new TaskAttackPlayer(attachedAnimator)
                                            ),
                                            // gotta walk the walk
                                            new Sequence(
                                                new TaskGoToPlayer(
                                                    attachedAgent,
                                                    attachedRigidbody,
                                                    attachedAnimator
                                                )
                                            )
                                        )
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

        public void OnCaged(bool value) => isCaged = value;
    }
}