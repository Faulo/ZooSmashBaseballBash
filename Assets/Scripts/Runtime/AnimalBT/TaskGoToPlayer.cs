using UnityEngine;
using UnityEngine.AI;
using ZSBB.BehaviorTree;
using ZSBB.Level;

namespace ZSBB.AnimalBT {
    sealed class TaskGoToPlayer : Node {
        readonly Transform _transform;
        readonly NavMeshAgent _navMeshAgent;
        readonly Rigidbody _rigidbody;
        readonly Animator _animator;

        readonly float minDistanceToPlayerBeforeGameOver = 5f;

        public TaskGoToPlayer(Transform transform, NavMeshAgent navMeshAgent, Rigidbody rigidbody, Animator animator) {
            _transform = transform;
            _navMeshAgent = navMeshAgent;
            _rigidbody = rigidbody;
            _animator = animator;
        }

        float velocitySmoothing = 0.5f;
        Vector3 acceleration;
        float torque;

        public override NodeState Evaluate() {
            if (_navMeshAgent.hasPath) {
                if (Tower.instance) {
                    var player = Tower.instance.transform;
                    var desiredVelocity = Vector3.zero;
                    if (_navMeshAgent.remainingDistance > minDistanceToPlayerBeforeGameOver) {
                        desiredVelocity = _navMeshAgent.desiredVelocity;
                        desiredVelocity *= AnimalBehavior.speed;
                        _animator.PlayInFixedTime(AnimationStates.Walk);
                    } else {
                        _animator.PlayInFixedTime(AnimationStates.Idle_A);
                    }
                    _rigidbody.velocity = Vector3.SmoothDamp(
                        _rigidbody.velocity,
                        desiredVelocity,
                        ref acceleration,
                        velocitySmoothing
                    );

                    LookAt(player);
                }
            }

            state = NodeState.RUNNING;
            return state;
        }

        void LookAt(Transform target) {
            var rotation = _rigidbody.rotation.eulerAngles;
            var targetRotation = Quaternion
                .LookRotation(target.position - _rigidbody.position)
                .eulerAngles;

            rotation.y = Mathf.SmoothDampAngle(rotation.y, targetRotation.y, ref torque, velocitySmoothing);

            _rigidbody.rotation = Quaternion.Euler(rotation);
        }
    }
}