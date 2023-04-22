using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;

namespace AnimalBT {
    public class CheckIsGrounded : Node {
        private Transform _transform;
        private Animator _animator;

        private float distanceToGround = 1f;

        public CheckIsGrounded(Transform transform) {
            _transform = transform;
            _animator = transform.GetComponent<Animator>();
        }

        public override NodeState Evaluate() {
            if (IsGrounded()) {
                state = NodeState.SUCCESS;
                _animator.SetBool("Grounded", true);
                return state;
            }

            state = NodeState.FAILURE;
            _animator.SetBool("Grounded", false);
            return state;
        }

        private bool IsGrounded() {
            return Physics.Raycast(_transform.position, -Vector3.up, distanceToGround + 0.1f);
        }
    }
}