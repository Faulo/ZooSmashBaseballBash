using ZSBB.BehaviorTree;

namespace ZSBB.AnimalBT {
    sealed class CheckIsCaged : Node {
        readonly AnimalBehavior _behavior;

        public CheckIsCaged(AnimalBehavior behavior) {
            _behavior = behavior;
        }

        public override NodeState Evaluate() {
            // maybe set up different cage-categories and check them here.
            if (_behavior.isCaged) {
                state = NodeState.SUCCESS;
                return state;
            }

            state = NodeState.FAILURE;
            return state;
        }
    }
}