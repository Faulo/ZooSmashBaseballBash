using UnityEngine;
using ZSBB.BehaviorTree;

namespace ZSBB.AnimalBT {
    public class CheckIsNotFacingPlayer : Node {
        private Transform _transform;

        public CheckIsNotFacingPlayer(Transform transform) {
            _transform = transform;
        }

        public override NodeState Evaluate() {
            if (!IsAligned()) {
                state = NodeState.SUCCESS;
                return state;
            }

            state = NodeState.FAILURE;
            return state;
        }

        private bool IsAligned() {
            var player = (Transform)GetData("player");
            float angle = 10f;
            if (Vector3.Angle(player.transform.forward, _transform.position - player.transform.position) < angle) {
                return true;
            }

            return false;
        }
    }
}