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

        float velocitySmoothing = 1;
        Vector3 acceleration;

        public override NodeState Evaluate() {
            if (_navMeshAgent.hasPath) {
                var player = (Transform)GetData("player");
                var desiredVelocity = Vector3.zero;
                if (_navMeshAgent.remainingDistance > minDistanceToPlayerBeforeGameOver) {
                    desiredVelocity = _navMeshAgent.desiredVelocity;
                    desiredVelocity *= AnimalBehavior.speed;
                    _animator.PlayInFixedTime(AnimationStates.Walk);
                } else {
                    _animator.PlayInFixedTime(AnimationStates.Idle_A);
                }
                _transform.LookAt(player);
                _rigidbody.velocity = Vector3.SmoothDamp(
                    _rigidbody.velocity,
                    desiredVelocity,
                    ref acceleration,
                    velocitySmoothing
                );
            }

            state = NodeState.RUNNING;
            return state;
        }
    }
}