using System;
using System.Collections.Generic;
using AnimalBT;
using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;
using Tree = BehaviorTree.Tree;
using UnityObject = UnityEngine.Object;

namespace BehaviorTree {
    sealed class AnimalBT : Tree {

        public static float speed = 1f;
        public static float weight = 10f;

        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Rigidbody rigidbody;
        protected override Node SetupTree() {
            Node root = new Selector(new List<Node> {
                // If Grounded, Find player and goto player
                new Sequence( new List<Node> {
                    new CheckIsGrounded(transform),
                    new CheckForPlayer(agent),
                    new TaskGoToPlayer(transform, agent, rigidbody)
                }),
                new Sequence(new List<Node> {
                    new TaskFlying(transform)
                })
            });

            return root;
        }
    }
}
