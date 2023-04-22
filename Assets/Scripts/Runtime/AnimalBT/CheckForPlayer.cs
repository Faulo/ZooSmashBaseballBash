using UnityEngine;
using UnityEngine.AI;
using ZSBB.BehaviorTree;

namespace ZSBB.AnimalBT {
    public class CheckForPlayer : Node {
        NavMeshAgent _navMeshAgent;

        public CheckForPlayer(NavMeshAgent agent) {
            _navMeshAgent = agent;
        }
        public override NodeState Evaluate() {
            object t = GetData("player");
            if (t == null) {
                var player = GameObject.Find("P_Player");
                if (player) {
                    parent.parent.SetData("player", player.transform);
                    //_navMeshAgent.destination = player.transform.position;
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