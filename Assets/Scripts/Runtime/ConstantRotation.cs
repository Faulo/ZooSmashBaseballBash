using UnityEngine;

namespace ZSBB {
    sealed class ConstantRotation : MonoBehaviour {
        [SerializeField]
        Quaternion constantRotation = Quaternion.identity;
        void FixedUpdate() {
            transform.rotation = constantRotation;
        }
    }
}
