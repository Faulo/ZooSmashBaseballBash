using UnityEngine;
using ZSBB.BehaviorTree;

namespace ZSBB.AnimalBT {
    public class CheckIsLanding : Node {
        private Rigidbody _rigidbody;

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

        private bool IsLanding() {
            if ((!(_rigidbody.velocity.y < 0.7f)) && (!(_rigidbody.velocity.y > -0.7f))) {
                return true;
            }

            return false;
        }
    }
}