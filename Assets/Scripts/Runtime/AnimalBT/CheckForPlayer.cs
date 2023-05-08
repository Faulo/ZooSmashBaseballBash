using ZSBB.BehaviorTree;
using ZSBB.Level;

namespace ZSBB.AnimalBT {
    sealed class CheckForPlayer : Node {
        public override NodeState Evaluate() {
            if (Tower.instance) {
                state = NodeState.SUCCESS;
                return state;
            }

            state = NodeState.FAILURE;
            return state;
        }
    }
}