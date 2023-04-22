using UnityEngine;
using ZSBB.BehaviorTree;

namespace ZSBB.AnimalBT {
    public class TaskLanding : Node {
        Animator _animator;

        public TaskLanding(Transform transform) {
            _animator = transform.GetComponent<Animator>();
        }

        public override NodeState Evaluate() {
            _animator.Play("Base Layer." + AnimationStates.Roll);

            state = NodeState.RUNNING;
            return state;
        }
    }
}