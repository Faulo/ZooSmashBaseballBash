using UnityEngine;
using ZSBB.BehaviorTree;

namespace ZSBB.AnimalBT {
    sealed class TaskLanding : Node {
        readonly Animator _animator;

        public TaskLanding(Animator animator) {
            _animator = animator;
        }

        public override NodeState Evaluate() {
            _animator.PlayInFixedTime(AnimalAnimation.Roll);

            state = NodeState.RUNNING;
            return state;
        }
    }
}