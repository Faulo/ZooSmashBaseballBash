using UnityEngine;
using ZSBB.BehaviorTree;

namespace ZSBB.AnimalBT {
    public class CheckIsLyingDown : Node {
        private Rigidbody _rigidbody;
        private Transform _transform;

        public CheckIsLyingDown(Rigidbody rigidbody, Transform transform) {
            _rigidbody = rigidbody;
            _transform = transform;
        }

        public override NodeState Evaluate() {
            if (!IsLyingDown()) {
                //_agent.enabled = true;
                state = NodeState.SUCCESS;
                return state;
            }

            //_agent.enabled = false;
            state = NodeState.FAILURE;
            return state;
        }

        private bool IsLyingDown() {
            if ((!(_rigidbody.velocity.y < 0.7f)) && (!(_rigidbody.velocity.y > -0.7f))) {
                return false;
            }

            return _transform.rotation.eulerAngles != Vector3.zero;
        }
    }
}