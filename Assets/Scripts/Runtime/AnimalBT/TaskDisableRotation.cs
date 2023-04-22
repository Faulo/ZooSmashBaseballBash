using UnityEngine;
using ZSBB.BehaviorTree;

namespace ZSBB.AnimalBT {
    sealed class TaskDisableRotation : Node {
        readonly Rigidbody _rigidbody;

        public TaskDisableRotation(Rigidbody rigidbody) {
            _rigidbody = rigidbody;
        }

        public override NodeState Evaluate() {
            _rigidbody.freezeRotation = true;

            state = NodeState.SUCCESS;
            return state;
        }
    }
}
