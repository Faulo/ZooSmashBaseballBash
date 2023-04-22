using UnityEngine;
using ZSBB.BehaviorTree;

namespace ZSBB.AnimalBT {
    public class CheckIsGrounded : Node {
        Transform _transform;

        float distanceToGround = 1f;

        public CheckIsGrounded(Transform transform) {
            _transform = transform;
        }

        public override NodeState Evaluate() {
            if (IsGrounded()) {
                state = NodeState.SUCCESS;
                return state;
            }

            state = NodeState.FAILURE;
            return state;
        }

        bool IsGrounded() {
            return Physics.Raycast(_transform.position, -Vector3.up, distanceToGround + 0.1f);
        }
    }
}