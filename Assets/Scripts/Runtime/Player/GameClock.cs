using UnityEngine;

namespace ZSBB.Player {
    sealed class GameClock : MonoBehaviour {
        [SerializeField, Range(0, 60)]
        float timeLimitInMinutes = 1;
        [SerializeField]
        GameState state;

        void OnEnable() {
            state.StartTimer(timeLimitInMinutes * 60);
        }
        void Update() {
            state.UpdateTimer(Time.deltaTime);
        }
        void OnDisable() {
            state.StopTimer();
        }
    }
}
