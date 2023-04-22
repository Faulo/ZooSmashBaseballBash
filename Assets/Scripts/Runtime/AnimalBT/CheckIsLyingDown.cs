using UnityEngine;
using ZSBB.BehaviorTree;

namespace ZSBB.AnimalBT {
    sealed class CheckIsLyingDown : Node {
        readonly Transform _transform;

        readonly float maxAngle = 0.125f;

        public CheckIsLyingDown(Transform transform) {
            _transform = transform;
        }

        public override NodeState Evaluate() {
            if (IsUpright()) {
                state = NodeState.FAILURE;
                return state;
            }

            state = NodeState.SUCCESS;
            return state;
        }

        bool IsUpright() {
            var rotation = _transform.eulerAngles;
            return Mathf.DeltaAngle(rotation.x, 0) < maxAngle
                && Mathf.DeltaAngle(rotation.z, 0) < maxAngle;
        }
    }
}