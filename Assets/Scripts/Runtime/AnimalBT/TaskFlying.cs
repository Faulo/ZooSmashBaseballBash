using UnityEngine;
using ZSBB.BehaviorTree;

namespace ZSBB.AnimalBT {
    public class TaskFlying : Node {
        Animator _animator;

        public TaskFlying(Animator animator) {
            _animator = animator;
        }

        public override NodeState Evaluate() {
            _animator.PlayInFixedTime(AnimationStates.Fly);

            state = NodeState.RUNNING;
            return state;
        }
    }
}