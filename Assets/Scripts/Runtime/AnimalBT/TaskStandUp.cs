using UnityEngine;
using ZSBB.BehaviorTree;

namespace ZSBB.AnimalBT {
    public class TaskStandUp : Node {
        Animator _animator;
        private Transform _transform;

        public TaskStandUp(Animator animator, Transform transform) {
            _animator = animator;
            _transform = transform;
        }

        public override NodeState Evaluate() {
            _animator.PlayInFixedTime(AnimationStates.Roll);
            
            state = NodeState.RUNNING;
            return state;
        }
    }
}