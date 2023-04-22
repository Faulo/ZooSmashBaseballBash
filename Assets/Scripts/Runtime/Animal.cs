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
        [SerializeField]
        int agentTypeID = 0;
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

            isDirty = true;
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
            if (Application.isPlaying) {
                SpawnAnimal();
            }
        }

        void SpawnAnimal() {
            transform.Clear();

            if (model) {
                var instance = Instantiate(model, transform);
                instance.layer = layer;
                instance.hideFlags = HideFlags.DontSave;

                var animator = instance.GetComponent<Animator>();
                animator.runtimeAnimatorController = this.animator;

                var collider = instance.AddComponent<BoxCollider>();
                collider.size = bounds.size;
                collider.center = bounds.center;
                collider.material = material;

                var rigidbody = instance.AddComponent<Rigidbody>();
                rigidbody.drag = baseDrag;
                rigidbody.mass = weight;
                rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;

                /*
                var agentObj = new GameObject(nameof(NavMeshAgent)) {
                    hideFlags = HideFlags.DontSave
                };
                agentObj.transform.parent = transform;
                //*/
                var agent = instance.GetOrAddComponent<NavMeshAgent>();
                agent.agentTypeID = agentTypeID;
                agent.updatePosition = false;
                agent.updateRotation = false;
                agent.updateUpAxis = false;
                var settings = NavMesh.GetSettingsByID(agentTypeID);
                agent.radius = bounds.size.x / 2;
                agent.height = bounds.size.y;

                var behavior = instance.AddComponent<AnimalBehavior>();
                behavior.attachedAnimator = animator;
                behavior.attachedRigidbody = rigidbody;
                behavior.attachedAgent = agent;
            }
        }
    }
}
