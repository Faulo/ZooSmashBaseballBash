using System.Linq;
using MyBox;
using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.AI;
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
        [SerializeField, ReadOnly]
        int agentTypeID;
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

            agentTypeID = 0;
            int settingsCount = NavMesh.GetSettingsCount();
            for (int i = 0; i < settingsCount; i++) {
                var settings = NavMesh.GetSettingsByIndex(i);
                string name = NavMesh.GetSettingsNameFromID(settings.agentTypeID);
                if (name == assetName) {
                    agentTypeID = settings.agentTypeID;
                    break;
                }
            }

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
        void Start() {
            SpawnAnimal();
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

                var collider = gameObject.GetOrAddComponent<BoxCollider>();
                collider.size = bounds.size;
                collider.center = bounds.center;
                collider.material = material;

                var agent = gameObject.GetOrAddComponent<NavMeshAgent>();
                agent.agentTypeID = agentTypeID;
                var settings = NavMesh.GetSettingsByID(agentTypeID);
                agent.radius = bounds.size.x / 2;
                agent.height = bounds.size.y;
            }
            var rigidbody = gameObject.GetOrAddComponent<Rigidbody>();
            rigidbody.drag = baseDrag;
            rigidbody.mass = weight;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;

            var behavior = gameObject.GetOrAddComponent<AnimalBehavior>();
        }
    }
}
