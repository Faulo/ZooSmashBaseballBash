﻿using UnityEngine;
using ZSBB.BehaviorTree;

namespace ZSBB.AnimalBT {
    public class CheckIsLyingDown : Node {
        private Transform _transform;

        public CheckIsLyingDown(Transform transform) {
            _transform = transform;
        }

        public override NodeState Evaluate() {
            if (IsLyingDown()) {
                //_agent.enabled = true;
                state = NodeState.SUCCESS;
                return state;
            }

            //_agent.enabled = false;
            state = NodeState.FAILURE;
            return state;
        }

        private bool IsLyingDown() {
            return _transform.rotation.eulerAngles != Vector3.zero;
        }
    }
}