using UnityEngine;
using UnityEngine.AI;
using ZSBB.BehaviorTree;
using ZSBB.Level;

namespace ZSBB.AnimalBT {
    sealed class TaskGoToPlayer : Node {
        readonly NavMeshAgent _navMeshAgent;
        readonly Rigidbody _rigidbody;
        readonly Animator _animator;

        public TaskGoToPlayer(NavMeshAgent navMeshAgent, Rigidbody rigidbody, Animator animator) {
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
                    LookAt(Tower.instance.transform);

                    _animator.PlayInFixedTime(AnimationStates.Walk);

                    var desiredVelocity = _navMeshAgent.desiredVelocity;
                    desiredVelocity *= AnimalBehavior.speed;

                    _rigidbody.velocity = Vector3.SmoothDamp(
                        _rigidbody.velocity,
                        desiredVelocity,
                        ref acceleration,
                        velocitySmoothing
                    );
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