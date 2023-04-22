using UnityEngine;

namespace ZSBB {
    record CollisionInfo(Rigidbody rigidbody, Vector3 position, Quaternion rotation, Vector3 velocity);
}
