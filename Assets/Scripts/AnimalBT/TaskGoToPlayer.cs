using System;
using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;
using UnityObject = UnityEngine.Object;

namespace BehaviorTree {
    public class TaskGoToPlayer : Node {
        private Transform _transform;
        private NavMeshAgent _navMeshAgent;
        private Rigidbody _rigidbody;
        private Animator _animator;

        private float minDistanceToPlayerBeforeGameOver = 5f;

        public TaskGoToPlayer(Transform transform, NavMeshAgent navMeshAgent, Rigidbody rigidbody) {
            _transform = transform;
            _navMeshAgent = navMeshAgent;
            _rigidbody = rigidbody;
            _animator = transform.GetComponent<Animator>();
        }

        public override NodeState Evaluate() {
            Transform player = (Transform)GetData("player");
            
            if (Vector3.Distance(_transform.position, player.position) > minDistanceToPlayerBeforeGameOver) {
                var desiredVelocity = _navMeshAgent.desiredVelocity;
                _animator.SetBool("Walking", true);
                _rigidbody.AddForce(force: desiredVelocity.normalized * AnimalBT.speed);
            } else {
                _animator.SetBool("Walking", false);
                _rigidbody.AddForce(Vector3.zero);
            }

            state = NodeState.RUNNING;
            return state;
        }
    }
}
