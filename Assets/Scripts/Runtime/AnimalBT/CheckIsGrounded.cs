using UnityEngine;
using UnityEngine.AI;
using ZSBB.BehaviorTree;

namespace ZSBB.AnimalBT {
    public class CheckIsGrounded : Node {
        readonly Transform _transform;
        readonly Rigidbody _rigidbody;
        readonly NavMeshAgent _agent;

        float distanceToGround = 1f;

        public CheckIsGrounded(Transform transform, Rigidbody rigidbody, NavMeshAgent agent) {
            _transform = transform;
            _rigidbody = rigidbody;
            _agent = agent;
        }

        public override NodeState Evaluate() {
            if (IsGrounded()) {
                //_agent.enabled = true;
                state = NodeState.SUCCESS;
                return state;
            }

            //_agent.enabled = false;
            state = NodeState.FAILURE;
            return state;
        }

        bool IsGrounded() {
            return Physics.Raycast(_transform.position, Vector3.down, distanceToGround + 0.1f);
        }
    }
}