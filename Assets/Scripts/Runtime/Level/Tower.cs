using Slothsoft.UnityExtensions;
using UnityEngine;

namespace ZSBB.Level {
    [ExecuteAlways]
    sealed class Tower : MonoBehaviour {
        public static Tower instance { get; private set; }

        [SerializeField]
        GameObject segmentPrefab;
        [SerializeField, Range(0, 100)]
        int segmentCount = 0;
        [SerializeField, Range(0, 10)]
        int segmentHeight = 2;
        [SerializeField]
        GameObject topPrefab;

#if UNITY_EDITOR
        bool isDirty = false;
        void OnValidate() {
            isDirty = true;
        }
        void Update() {
            if (!Application.isPlaying && isDirty) {
                isDirty = false;
                SpawnTower();
            }
        }
#endif
        void OnEnable() {
            instance = this;
        }
        void OnDisable() {
            instance = null;
        }
        void Awake() {
            if (Application.isPlaying) {
                SpawnTower();
            }
        }
        void SpawnTower() {
            transform.Clear();
            if (segmentPrefab) {
                for (int i = 0; i < segmentCount; i++) {
                    float y = i * segmentHeight;
                    var instance = Instantiate(segmentPrefab, transform);
                    instance.hideFlags = HideFlags.DontSave;
                    instance.transform.localPosition = Vector3.up * y;
                }
            }
            if (topPrefab) {
                float y = segmentCount * segmentHeight;
                var instance = Instantiate(topPrefab, transform);
                instance.hideFlags = HideFlags.DontSave;
                instance.transform.localPosition = Vector3.up * y;
            }
        }
    }
}
