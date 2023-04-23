using System.Collections.Generic;
using UnityEngine;

namespace ZSBB.AnimalBT {
    enum AnimalAnimation {
        Attack,
        Bounce,
        Clicked,
        Eat,
        Idle_A,
        Idle_B,
        Idle_C,
        Sit,
        Swim,
        Roll,
        Walk,
        Fly,
        Death,
        Fear,
        Hit,
        Jump,
        Run,
        Spin
    }
    static class AnimalAnimationExtensions {
        static readonly Dictionary<AnimalAnimation, int> stateIds = new();
        public static void PlayInFixedTime(this Animator animator, AnimalAnimation state, float timeInSeconds = -1) {
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
            if (timeInSeconds < 0) {
                timeInSeconds = state.GetTransitionDuration();
            }
            animator.CrossFadeInFixedTime(id, timeInSeconds, 0);
            animator.speed = state.GetPlaybackSpeed();

        }
        public static float GetTransitionDuration(this AnimalAnimation state) => state switch {
            AnimalAnimation.Attack => 0.125f,
            _ => 0.25f,
        };
        public static float GetPlaybackSpeed(this AnimalAnimation state) => state switch {
            AnimalAnimation.Spin => 0.5f,
            _ => 1,
        };
    }
}
