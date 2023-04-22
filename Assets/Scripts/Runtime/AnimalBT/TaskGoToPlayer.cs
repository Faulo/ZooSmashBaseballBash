using UnityEngine;
using UnityEngine.AI;
using ZSBB.BehaviorTree;

namespace ZSBB.AnimalBT {
    public class TaskGoToPlayer : Node {
        Transform _transform;
        NavMeshAgent _navMeshAgent;
        Rigidbody _rigidbody;
        Animator _animator;

        float minDistanceToPlayerBeforeGameOver = 5f;

        public TaskGoToPlayer(Transform transform, NavMeshAgent navMeshAgent, Rigidbody rigidbody, Animator animator) {
            _transform = transform;
            _navMeshAgent = navMeshAgent;
            _rigidbody = rigidbody;
            _animator = animator;
        }

        readonly NavMeshPath path = new();
        Vector3[] corners = new Vector3[1];
        public override NodeState Evaluate() {
            var player = (Transform)GetData("player");
            if (NavMesh.CalculatePath(_transform.position, player.position, -1, path)) {
                corners = path.corners;
                if (corners.Length > 0) {
                    var nextPosition = corners[0];
                }
            }

            /*
            if (Vector3.Distance(_transform.position, player.position) > minDistanceToPlayerBeforeGameOver) {
                var desiredVelocity = _navMeshAgent.desiredVelocity;
                _animator.PlayInFixedTime(AnimationStates.Walk);
                _rigidbody.AddForce(force: desiredVelocity.normalized * AnimalBehavior.speed);
            } else {
                _animator.PlayInFixedTime(AnimationStates.Idle_A);
                _rigidbody.AddForce(Vector3.zero);
            }
            //*/

            state = NodeState.RUNNING;
            return state;
        }
    }
}
