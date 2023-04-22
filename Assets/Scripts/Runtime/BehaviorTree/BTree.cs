using UnityEngine;

namespace BehaviorTree {
    public abstract class BTree : MonoBehaviour {

        Node _root = null;

        protected void Start() {
            _root = SetupTree();
        }

        void Update() {
            if (_root != null) {
                _root.Evaluate();
            }
        }

        protected abstract Node SetupTree();

    }

}