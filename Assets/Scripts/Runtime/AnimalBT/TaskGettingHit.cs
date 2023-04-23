using UnityEngine;
using ZSBB.BehaviorTree;

namespace ZSBB.AnimalBT {
    sealed class TaskGettingHit : Node {
        readonly Animator _animator;
        readonly AnimalBehavior _behavior;
        readonly SpeedTracker _tracker;

        public TaskGettingHit(Animator animator, AnimalBehavior behavior, SpeedTracker tracker) {
            _animator = animator;
            _behavior = behavior;
            _tracker = tracker;
        }

        public override NodeState Evaluate() {
            if (_tracker.isMoving) {
                _animator.PlayInFixedTime(AnimalAnimation.Death);
            } else {
                _behavior.wasHit = false;
            }

            state = NodeState.RUNNING;
            return state;
        }
    }
}