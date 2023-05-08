using UnityEngine;
using ZSBB.BehaviorTree;

namespace ZSBB.AnimalBT {
    sealed class CheckIsLanding : Node {
        readonly Rigidbody _rigidbody;

        public CheckIsLanding(Rigidbody rigidbody) {
            _rigidbody = rigidbody;
        }

        public override NodeState Evaluate() {
            if (IsLanding()) {
                //_agent.enabled = true;
                state = NodeState.SUCCESS;
                return state;
            }

            //_agent.enabled = false;
            state = NodeState.FAILURE;
            return state;
        }

        bool IsLanding() {
            if (Mathf.Abs(_rigidbody.velocity.y) < 0.7f) {
                return true;
            }

            return false;
        }
    }
}