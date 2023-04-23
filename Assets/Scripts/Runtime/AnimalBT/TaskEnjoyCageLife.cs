using UnityEngine;
using ZSBB.BehaviorTree;

namespace ZSBB.AnimalBT {
    sealed class TaskEnjoyCageLife : Node {
        readonly Animator _animator;

        public TaskEnjoyCageLife(Animator animator) {
            _animator = animator;
        }

        public override NodeState Evaluate() {
            _animator.PlayInFixedTime(AnimalAnimation.Spin);

            state = NodeState.RUNNING;
            return state;
        }
    }
}