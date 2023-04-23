using System.Collections;
using MyBox;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace ZSBB.Level {
    sealed class LevelController : MonoBehaviour {
        [SerializeField]
        GameState state;

        [Space]
        [SerializeField]
        SceneReference mainMenu = new();

        bool isDone;

        void FixedUpdate() {
            if (isDone) {
                return;
            }
            if (state.hasWon) {
                isDone = true;
                StartCoroutine(WinRoutine());
            }
            if (state.hasLost) {
                isDone = true;
                StartCoroutine(LoseRoutine());
            }
        }

        IEnumerator WinRoutine() {
            // play win stuff here

            yield return null;

            // or here

            yield return Wait.forSeconds[10];

            // or here

            yield return mainMenu.LoadSceneAsync();
        }

        IEnumerator LoseRoutine() {
            // play lose stuff here

            yield return null;

            // or here

            yield return Wait.forSeconds[10];

            // or here

            yield return mainMenu.LoadSceneAsync();
        }
    }
}
