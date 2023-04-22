using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;

namespace AnimalBT {
    public class CheckForPlayer : Node {
        NavMeshAgent _navMeshAgent;

        public CheckForPlayer(NavMeshAgent agent) {
            _navMeshAgent = agent;
        }
        public override NodeState Evaluate() {
            object t = GetData("player");
            if (t == null) {
                var player = GameObject.Find("Player");
                if (player) {
                    parent.parent.SetData("player", player.transform);
                    _navMeshAgent.destination = player.transform.position;
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