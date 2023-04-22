using System.Linq;
using MyBox;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace ZSBB {
    [ExecuteAlways]
    sealed class Animal : MonoBehaviour {
        [Header("Art")]
        [SerializeField]
        GameObject model;
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

            animator = allAssets
                .OfType<RuntimeAnimatorController>()
                .FirstOrDefault();

            UnityEditor.EditorUtility.SetDirty(gameObject);

            SpawnAnimal();
        }
#endif

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

        void SpawnAnimal() {
            transform.Clear();
            gameObject.layer = layer;

            if (model) {
                var instance = Instantiate(model, transform);
                instance.hideFlags = HideFlags.DontSave;
                if (instance.TryGetComponent<Animator>(out var animator)) {
                    animator.runtimeAnimatorController = this.animator;
                }
                var renderer = instance.GetComponentInChildren<SkinnedMeshRenderer>();
                var bounds = renderer.bounds;

                var collider = gameObject.GetOrAddComponent<BoxCollider>();
                collider.size = bounds.size;
                collider.center = bounds.center;
                collider.material = material;
            }
            var rigidbody = gameObject.GetOrAddComponent<Rigidbody>();
            rigidbody.drag = baseDrag;
            rigidbody.mass = weight;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }
    }
}
