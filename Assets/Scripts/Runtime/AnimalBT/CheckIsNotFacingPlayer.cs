using UnityEngine;
using ZSBB.BehaviorTree;
using ZSBB.Level;

namespace ZSBB.AnimalBT {
    sealed class CheckIsNotFacingPlayer : Node {
        readonly Transform _transform;

        readonly float maxAngle = 10;

        public CheckIsNotFacingPlayer(Transform transform) {
            _transform = transform;
        }

        public override NodeState Evaluate() {
            if (Tower.instance && !IsAligned(Tower.instance.transform)) {
                state = NodeState.SUCCESS;
                return state;
            }

            state = NodeState.FAILURE;
            return state;
        }

        bool IsAligned(Transform target) {
            var rotation = Quaternion
               .LookRotation(target.position - _transform.position)
               .eulerAngles;
            return Mathf.DeltaAngle(_transform.eulerAngles.y, rotation.y) < maxAngle;
        }
    }
}