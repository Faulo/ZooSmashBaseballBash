using UnityEngine;
using ZSBB.BehaviorTree;
using ZSBB.Level;

namespace ZSBB.AnimalBT {
    sealed class TaskAttackPlayer : Node {
        readonly Animator _animator;

        public TaskAttackPlayer(Animator animator) {
            _animator = animator;
        }

        public override NodeState Evaluate() {
            if (!Tower.instance) {
                state = NodeState.FAILURE;
                return state;
            }

            if (Tower.instance.TakeDamage(Time.deltaTime)) {
                _animator.PlayInFixedTime(AnimationStates.Attack);
            } else {
                _animator.PlayInFixedTime(AnimationStates.Spin);
            }

            state = NodeState.RUNNING;
            return state;
        }
    }
}
