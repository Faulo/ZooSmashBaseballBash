using ZSBB.BehaviorTree;

namespace ZSBB.AnimalBT {
    sealed class CheckIsMoving : Node {
        readonly SpeedTracker _tracker;

        public CheckIsMoving(SpeedTracker tracker) {
            _tracker = tracker;
        }

        public override NodeState Evaluate() {
            if (_tracker.isMoving) {
                state = NodeState.SUCCESS;
                return state;
            }

            state = NodeState.FAILURE;
            return state;
        }
    }
}
