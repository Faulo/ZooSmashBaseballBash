using UnityEngine;
using ZSBB.BehaviorTree;

namespace ZSBB.AnimalBT {
    public class TaskLanding : Node {
        Animator _animator;

        public TaskLanding(Animator animator) {
            _animator = animator;
        }

        public override NodeState Evaluate() {
            _animator.PlayInFixedTime(AnimationStates.Roll);

            state = NodeState.RUNNING;
            return state;
        }
    }
}