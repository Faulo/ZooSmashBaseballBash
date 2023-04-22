using UnityEngine;
using ZSBB.AnimalBT;

namespace ZSBB {
    static class Extensions {
        public static void PlayInFixedTime(this Animator animator, AnimationStates state, float timeInSeconds = 0.2f) {
            animator.PlayInFixedTime(state.ToString(), 0, timeInSeconds);
        }
    }
}
