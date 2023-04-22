using System;
using System.Collections.Generic;

namespace ZSBB.BehaviorTree {
    public enum NodeState {
        RUNNING,
        SUCCESS,
        FAILURE
    }
    abstract class Node {
        protected NodeState state;

        public Node parent;
        protected readonly Node[] children;

        Dictionary<string, object> _dataContext = new Dictionary<string, object>();

        public Node() {
            parent = null;
            children = Array.Empty<Node>();
        }
        public Node(params Node[] children) {
            foreach (var child in children) {
                child.parent = this;
            }
            this.children = children;
        }

        public virtual NodeState Evaluate() => NodeState.FAILURE;

        public void SetData(string key, object value) {
            _dataContext[key] = value;
        }

        public object GetData(string key) {
            if (_dataContext.TryGetValue(key, out object value)) {
                return value;
            }

            var node = parent;
            while (node != null) {
                value = node.GetData(key);
                if (value != null) {
                    return value;
                }

                node = node.parent;
            }
            return null;
        }

        public bool ClearData(string key) {
            if (_dataContext.ContainsKey(key)) {
                _dataContext.Remove(key);
                return true;
            }

            var node = parent;
            while (node != null) {
                bool cleared = node.ClearData(key);
                if (cleared) {
                    return true;
                }

                node = node.parent;
            }
            return false;
        }
    }
}
