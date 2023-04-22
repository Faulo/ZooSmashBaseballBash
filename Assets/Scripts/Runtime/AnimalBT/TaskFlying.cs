using UnityEngine;
using ZSBB.BehaviorTree;

namespace ZSBB.AnimalBT {
    public class TaskFlying : Node {
        Animator _animator;
        Transform _transform;

        public TaskFlying(Transform transform) {
            _transform = transform;
            _animator = transform.GetComponent<Animator>();
        }

        public override NodeState Evaluate() {
            _animator.Play("Base Layer." + AnimationStates.Fly);

            state = NodeState.RUNNING;
            return state;
        }
    }
}