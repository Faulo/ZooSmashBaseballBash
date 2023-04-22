using UnityEngine;
using ZSBB.AnimalBT;

namespace ZSBB {
    static class Extensions {
        public static void PlayInFixedTime(this Animator animator, AnimationStates state, float timeInSeconds = 0.2f) {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName(state.ToString())) {
                animator.PlayInFixedTime(state.ToString(), 0, timeInSeconds);
            }
        }
    }
}