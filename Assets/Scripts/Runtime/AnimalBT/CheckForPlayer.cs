using UnityEngine;
using UnityEngine.AI;
using ZSBB.BehaviorTree;

namespace ZSBB.AnimalBT {
    public class CheckForPlayer : Node {
        
        public override NodeState Evaluate() {
            object t = GetData("player");
            if (t == null) {
                var player = GameObject.Find("P_Player");
                if (player) {
                    parent.parent.SetData("player", player.transform);
                    state = NodeState.SUCCESS;
                    return state;
                }

                state = NodeState.FAILURE;
                return state;
            }
            state = NodeState.SUCCESS;
            return state;
        }
    }
}