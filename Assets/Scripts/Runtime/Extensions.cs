using System.Collections.Generic;
using UnityEngine;
using ZSBB.AnimalBT;

namespace ZSBB {
    static class Extensions {
        static readonly Dictionary<AnimationStates, int> stateIds = new();
        public static void PlayInFixedTime(this Animator animator, AnimationStates state, float timeInSeconds = 1f / 8) {
            if (!stateIds.TryGetValue(state, out int id)) {
                stateIds[state] = id = Animator.StringToHash("Base Layer." + state);
            }
            if (!animator.HasState(0, stateIds[state])) {
                return;
            }
            if (animator.GetCurrentAnimatorStateInfo(0).fullPathHash == id) {
                return;
            }
            if (animator.GetNextAnimatorStateInfo(0).fullPathHash == id) {
                return;
            }
            animator.PlayInFixedTime(id, 0, timeInSeconds);
        }
    }
}