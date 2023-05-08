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
        public GameObject model;
        [SerializeField]
        public Bounds bounds = new();
        [SerializeField]
        public RuntimeAnimatorController animator;
        [SerializeField]
        public GameObject linePrefab;

        [Header("Physics")]
        [SerializeField]
        public float weight = 100;
        [SerializeField]
        public float baseDrag = 1;
        [SerializeField]
        public PhysicMaterial material;
        [SerializeField, Slothsoft.UnityExtensions.Layer]
        public int layer;

        [Header("Gameplay")]
        [SerializeField]
        public AnimalManager manager;
        [SerializeField]
        int agentTypeID = 0;
        [SerializeField]
        public float baseSpeed = 5;

        [SerializeField] AnimalCagePreference _cagePreference;

        AnimalBehavior behavior;

#if UNITY_EDITOR
        [ContextMenu(nameof(FindStuff))]
        public void FindStuff() {
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

        void OnEnable() {
            if (Application.isPlaying) {
                manager.RegisterAnimal(behavior);
            }
        }
        void OnDisable() {
            if (Application.isPlaying) {
                manager.UnregisterAnimal(behavior);
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

                var collider = instance.AddComponent<CapsuleCollider>();
                (collider.direction, collider.radius, collider.height) = GetCapsule(bounds);
                collider.center = new Vector3(0, bounds.size.y * 0.5f, 0);
                collider.material = material;

                var rigidbody = instance.AddComponent<Rigidbody>();
                rigidbody.automaticCenterOfMass = false;
                rigidbody.centerOfMass = collider.center;
                rigidbody.drag = baseDrag;
                rigidbody.mass = weight;
                rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;

                var agent = instance.GetOrAddComponent<NavMeshAgent>();
                agent.agentTypeID = agentTypeID;
                agent.updatePosition = false;
                agent.updateRotation = false;
                agent.updateUpAxis = false;
                var settings = NavMesh.GetSettingsByID(agentTypeID);
                agent.radius = bounds.size.x / 2;
                agent.height = bounds.size.y;

                var tracker = instance.AddComponent<SpeedTracker>();

                if (linePrefab) {
                    Instantiate(linePrefab, instance.transform);
                }

                behavior = instance.AddComponent<AnimalBehavior>();
                behavior.attachedAnimator = animator;
                behavior.attachedRigidbody = rigidbody;
                behavior.attachedCollider = collider;
                behavior.attachedAgent = agent;
                behavior.attachedTracker = tracker;
                behavior.cagePreference = _cagePreference;
            }
        }
        static (int direction, float radius, float height) GetCapsule(in Bounds bounds) {
            var size = bounds.size;
            if (size.x > size.y && size.x > size.z) {
                return (0, GetRadius(size.SwizzleYZ()), size.x);
            }

            if (bounds.size.y > bounds.size.z) {
                return (1, GetRadius(size.SwizzleXZ()), size.y);
            }

            return (2, GetRadius(size.SwizzleXY()), size.z);
        }
        static float GetRadius(Vector2 size) {
            return (size.x + size.y) * 0.25f;
        }
    }
}
