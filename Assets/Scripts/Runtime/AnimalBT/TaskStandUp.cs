using UnityEngine;
using ZSBB.BehaviorTree;

namespace ZSBB.AnimalBT {
    sealed class TaskStandUp : Node {
        readonly Animator _animator;

        public TaskStandUp(Animator animator) {
            _animator = animator;
        }

        public override NodeState Evaluate() {
            _animator.PlayInFixedTime(AnimationStates.Roll);

            state = NodeState.RUNNING;
            return state;
        }
    }
}