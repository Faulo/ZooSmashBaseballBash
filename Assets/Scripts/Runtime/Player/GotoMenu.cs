using MyBox;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace ZSBB.Player {
    sealed class GotoMenu : MonoBehaviour {
        [SerializeField]
        SceneReference menuScene = new();

        public void OnMenu(InputValue value) {
            if (value.isPressed) {
                var currentScene = SceneManager.GetActiveScene();
                if (currentScene.name == menuScene.SceneName) {
                    Application.Quit();
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#endif
                } else {
                    menuScene.LoadScene();
                }
            }
        }
    }
}
