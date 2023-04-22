using UnityEngine;
using UnityEngine.AI;

namespace BehaviorTree {
    public class TaskGoToPlayer : Node {
        Transform _transform;
        NavMeshAgent _navMeshAgent;
        Rigidbody _rigidbody;
        Animator _animator;

        float minDistanceToPlayerBeforeGameOver = 5f;

        public TaskGoToPlayer(Transform transform, NavMeshAgent navMeshAgent, Rigidbody rigidbody) {
            _transform = transform;
            _navMeshAgent = navMeshAgent;
            _rigidbody = rigidbody;
            _animator = transform.GetComponent<Animator>();
        }

        public override NodeState Evaluate() {
            var player = (Transform)GetData("player");

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
