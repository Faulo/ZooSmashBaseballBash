using ZSBB.BehaviorTree;

namespace ZSBB.AnimalBT {
    sealed class CheckIsHit : Node {
        readonly AnimalBehavior _behavior;

        public CheckIsHit(AnimalBehavior behavior) {
            _behavior = behavior;
        }

        public override NodeState Evaluate() {
            if (_behavior.wasHit) {
                state = NodeState.SUCCESS;
                return state;
            }

            state = NodeState.FAILURE;
            return state;
        }
    }
}