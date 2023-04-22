using UnityEngine;
using ZSBB.BehaviorTree;

namespace ZSBB.AnimalBT {
    public class CheckIsHit : Node {
        private Transform _transform;

        public CheckIsHit(Transform transform) {
            _transform = transform;
        }

        public override NodeState Evaluate() {
            var relocator = GameObject.Find("P_Relocator");

            if (Vector3.Distance(_transform.position, relocator.transform.position) < 0.2f) {
                state = NodeState.SUCCESS;
                return state;
            }

            state = NodeState.FAILURE;
            return state;
        }
    }
}