using UnityEngine;
using ZSBB.BehaviorTree;

namespace ZSBB.AnimalBT {
    sealed class CheckIsGrounded : Node {
        readonly Transform _transform;
        readonly CapsuleCollider _collider;

        readonly float maxDistance = 8f;
        readonly float distanceToGround = 0.125f;
        readonly LayerMask groundLayers = LayerMask.GetMask("Environment");

        public CheckIsGrounded(Transform transform, CapsuleCollider collider) {
            _transform = transform;
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
            if (!Physics.Raycast(_transform.position + _collider.center, Vector3.down, out var hit, maxDistance, groundLayers)) {
                return false;
            }

            float distance = Vector3.Distance(
                _collider.ClosestPoint(hit.point),
                hit.collider.ClosestPoint(hit.point)
            );

            return distance < distanceToGround;
        }
    }
}