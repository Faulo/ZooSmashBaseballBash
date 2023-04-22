using System.Linq;
using Slothsoft.UnityExtensions;
using UnityEngine;
using ZSBB.AnimalBT;

namespace ZSBB {
    [ExecuteAlways]
    sealed class Animal : MonoBehaviour {
        [Header("Art")]
        [SerializeField]
        GameObject model;
        [SerializeField]
        Bounds bounds = new();
        [SerializeField]
        RuntimeAnimatorController animator;

        [Header("Physics")]
        [SerializeField]
        public float weight = 100;
        [SerializeField]
        public float baseDrag = 1;
        [SerializeField]
        PhysicMaterial material;
        [SerializeField, Slothsoft.UnityExtensions.Layer]
        int layer;

        [Header("Gameplay")]
        [SerializeField]
        public float baseSpeed;

#if UNITY_EDITOR
        [ContextMenu(nameof(FindStuff))]
        void FindStuff() {
            string assetName = name.Replace("P_Animal_", "");
            var allAssets = UnityEditor.AssetDatabase.GetAllAssetPaths()
                .Where(path => path.Contains(assetName))
                .Select(UnityEditor.AssetDatabase.LoadMainAssetAtPath)
                .ToList();

            string modelName = $"{assetName}_Animations";
            model = allAssets
                .OfType<GameObject>()
                .FirstOrDefault(obj => obj.name == modelName);

            bounds = model.GetComponentInChildren<SkinnedMeshRenderer>().bounds;

            animator = allAssets
                .OfType<RuntimeAnimatorController>()
                .FirstOrDefault();

            UnityEditor.EditorUtility.SetDirty(gameObject);

            SpawnAnimal();
        }

        bool isDirty = false;
        void OnValidate() {
            isDirty = true;
        }
        void Update() {
            if (!Application.isPlaying && isDirty) {
                isDirty = false;
                SpawnAnimal();
            }
        }
#endif
        void Awake() {
            SpawnAnimal();
        }

        void SpawnAnimal() {
            transform.Clear();
            foreach (var component in GetComponents<Component>()) {
                if (component is Transform) {
                    continue;
                }
                if (component is Animal) {
                    continue;
                }
                if (Application.isPlaying) {
                    Destroy(component);
                } else {
                    DestroyImmediate(component);
                }
            }

            if (model) {
                var instance = Instantiate(model, transform);
                instance.layer = layer;
                instance.hideFlags = HideFlags.DontSave;
                if (instance.TryGetComponent<Animator>(out var animator)) {
                    animator.runtimeAnimatorController = this.animator;
                }

                var collider = instance.AddComponent<BoxCollider>();
                collider.size = bounds.size;
                collider.center = bounds.center;
                collider.material = material;

                var rigidbody = instance.AddComponent<Rigidbody>();
                rigidbody.drag = baseDrag;
                rigidbody.mass = weight;
                rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;

                var behavior = instance.AddComponent<AnimalBehavior>();
            }
        }
    }
}
