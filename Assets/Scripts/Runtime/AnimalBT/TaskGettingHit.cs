using UnityEngine;
using ZSBB.BehaviorTree;

namespace ZSBB.AnimalBT {
    public class TaskGettingHit : Node {
        private Animator _animator;

        public TaskGettingHit(Animator animator) {
            _animator = animator;
        }

        public override NodeState Evaluate() {
            _animator.PlayInFixedTime(AnimationStates.Death);

            state = NodeState.RUNNING;
            return state;
        }
    }
}