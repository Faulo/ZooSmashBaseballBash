using UnityEngine;
using ZSBB.BehaviorTree;

namespace ZSBB.AnimalBT {
    public class CheckIsGrounded : Node {
        readonly Transform _transform;
        readonly Rigidbody _rigidbody;
        readonly Collider _collider;

        readonly float distanceToGround = 0.1f;
        readonly LayerMask groundLayers = LayerMask.GetMask("Environment");

        public CheckIsGrounded(Transform transform, Rigidbody rigidbody, Collider collider) {
            _transform = transform;
            _rigidbody = rigidbody;
            _collider = collider;
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
            if (!Physics.Raycast(_rigidbody.worldCenterOfMass, Vector3.down, out var hit, groundLayers)) {
                return false;
            }

            if (!Physics.ComputePenetration(
                _collider, _transform.position, _transform.rotation,
                hit.collider, hit.transform.position, hit.transform.rotation,
                out var direction, out float distance)) {
                return false;
            }

            return distance < distanceToGround;
        }
    }
}