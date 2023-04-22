using UnityEngine;

namespace ZSBB.BehaviorTree {
    abstract class BTree : MonoBehaviour {
        Node _root = null;

        protected virtual void Start() {
            _root = SetupTree();
        }

        protected virtual void FixedUpdate() {
            _root.Evaluate();
        }

        protected abstract Node SetupTree();
    }
}