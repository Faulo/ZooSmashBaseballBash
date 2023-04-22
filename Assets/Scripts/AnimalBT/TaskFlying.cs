﻿using BehaviorTree;
using UnityEngine;

namespace AnimalBT {
    public class TaskFlying: Node {
        private Animator _animator;
        private Transform _transform;

        public TaskFlying(Transform transform) {
            _transform = transform;
            _animator = transform.GetComponent<Animator>();
        }

        public override NodeState Evaluate() {
            _animator.SetBool("Grounded", false);
            _animator.SetBool("Walking", false);
            
            state = NodeState.RUNNING;
            return state;
        }
    }
}