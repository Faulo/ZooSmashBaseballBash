using UnityEngine;
using ZSBB.BehaviorTree;

namespace ZSBB.AnimalBT {
    public class TaskFixRotation : Node {
        private Transform _transform;

        public TaskFixRotation(Transform transform) {
            _transform = transform;
        }

        public override NodeState Evaluate() {
            var player = (Transform)GetData("player");
            FixRotation(player);
            state = NodeState.SUCCESS;
            return state;
        }

        private void FixRotation(Transform target) {
            float smooth = 0.3f;
            float distance = 5.0f;
            float yVelocity = 0.0f;
            
            // Damp angle from current y-angle towards target y-angle
            float yAngle = Mathf.SmoothDampAngle(_transform.eulerAngles.y, target.eulerAngles.y, ref yVelocity, smooth);
            // Position at the target
            Vector3 position = target.position;
            // Then offset by distance behind the new angle
            position += Quaternion.Euler(0, yAngle, 0) * new Vector3(0, 0, -distance);
            // Apply the position
            _transform.position = position;
        }
    }
}