using UnityEngine;
using ZSBB.BehaviorTree;
using ZSBB.Level;

namespace ZSBB.AnimalBT {
    sealed class CheckIsInAttackRange : Node {
        readonly Transform _transform;

        readonly float minDistance = 3;

        public CheckIsInAttackRange(Transform transform) {
            _transform = transform;
        }

        public override NodeState Evaluate() {
            if (Tower.instance && IsInRange(Tower.instance.transform)) {
                state = NodeState.SUCCESS;
                return state;
            }

            state = NodeState.FAILURE;
            return state;
        }

        bool IsInRange(Transform target) {
            return Vector3.Distance(_transform.position, target.position) < minDistance;
        }
    }
}