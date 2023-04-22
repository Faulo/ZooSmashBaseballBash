using TMPro;
using UnityEngine;

namespace ZSBB {
    sealed class GameUI : MonoBehaviour {
        [SerializeField]
        GameState state;
        [SerializeField]
        TextMeshProUGUI text;

        string template;

        void OnValidate() {
            if (!text) {
                TryGetComponent(out text);
            }
        }

        void Awake() {
            template = text.text;
        }

        void Update() {
            string content = template;
            foreach (var (name, value) in state.variables) {
                content = content.Replace(name, value);
            }
            text.text = content;
        }
    }
}
