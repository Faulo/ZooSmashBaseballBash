using UnityEngine;
using ZSBB.BehaviorTree;

namespace ZSBB.AnimalBT {
    sealed class TaskEnableRotation : Node {
        readonly Rigidbody _rigidbody;

        public TaskEnableRotation(Rigidbody rigidbody) {
            _rigidbody = rigidbody;
        }

        public override NodeState Evaluate() {
            _rigidbody.freezeRotation = false;

            state = NodeState.SUCCESS;
            return state;
        }
    }
}
