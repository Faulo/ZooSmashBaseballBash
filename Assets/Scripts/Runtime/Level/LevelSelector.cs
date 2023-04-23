using MyBox;
using Slothsoft.UnityExtensions;
using UnityEngine;
using ZSBB.AnimalBT;

namespace ZSBB.Level {
    sealed class LevelSelector : MonoBehaviour, IRelocationMessages {
        [SerializeField]
        SerializableKeyValuePairs<AnimalCagePreference, SceneReference> scenes = new();
        public void OnHit() {
        }
        public void OnCageEnter(AnimalCagePreference cage) {
            if (scenes.TryGetValue(cage, out var scene)) {
                scene.LoadScene();
            }
        }
        public void OnCageExit(AnimalCagePreference cage) {
        }
    }
}
