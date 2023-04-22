using UnityEngine;

namespace ZSBB {
    interface IRelocation {
        void ResolveCollision(Relocator relocator, Rigidbody rigidbody, ContactPoint point);
    }
}
