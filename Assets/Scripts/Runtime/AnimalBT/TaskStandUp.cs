using UnityEngine;
using ZSBB.BehaviorTree;

namespace ZSBB.AnimalBT {
    public class TaskStandUp : Node {
        Animator _animator;
        private Transform _transform;

        public TaskStandUp(Animator animator, Transform transform) {
            _animator = animator;
            _transform = transform;
        }

        public override NodeState Evaluate() {
            _animator.PlayInFixedTime(AnimationStates.Sit);
            var targetAngle = Vector3.zero;
            var currentAngle = new Vector3(
                Mathf.LerpAngle(_transform.position.x, targetAngle.x, Time.deltaTime),
                Mathf.LerpAngle(_transform.position.y, targetAngle.y, Time.deltaTime),
                Mathf.LerpAngle(_transform.position.z, targetAngle.z, Time.deltaTime));
 
            _transform.eulerAngles = currentAngle;
            
            state = NodeState.RUNNING;
            return state;
        }
    }
}