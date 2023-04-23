using UnityEngine;
using ZSBB.BehaviorTree;

namespace ZSBB.AnimalBT {
    sealed class TaskFlying : Node {
        readonly Animator _animator;

        public TaskFlying(Animator animator) {
            _animator = animator;
        }

        public override NodeState Evaluate() {
            _animator.PlayInFixedTime(AnimalAnimation.Fly);

            state = NodeState.RUNNING;
            return state;
        }
    }
}