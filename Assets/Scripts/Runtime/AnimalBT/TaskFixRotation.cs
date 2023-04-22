using UnityEngine;
using ZSBB.BehaviorTree;

namespace ZSBB.AnimalBT {
    sealed class TaskFixRotation : Node {
        readonly Rigidbody _rigidbody;

        readonly float rotationalSmoothing = 0.5f;

        Vector2 torque;

        public TaskFixRotation(Rigidbody rigidbody) {
            _rigidbody = rigidbody;
        }

        public override NodeState Evaluate() {
            FixRotation();
            state = NodeState.RUNNING;
            return state;
        }

        void FixRotation() {
            var rotation = _rigidbody.rotation.eulerAngles;

            rotation.x = Mathf.SmoothDampAngle(rotation.x, 0, ref torque.x, rotationalSmoothing);
            rotation.z = Mathf.SmoothDampAngle(rotation.z, 0, ref torque.y, rotationalSmoothing);

            _rigidbody.MoveRotation(Quaternion.Euler(rotation));
        }
    }
}