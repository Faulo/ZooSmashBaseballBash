using System.Collections.Generic;
using System.Linq;
using Slothsoft.UnityExtensions;
using UnityEngine;
using ZSBB.AnimalBT;

namespace ZSBB {
    [CreateAssetMenu]
    sealed class AnimalManager : ScriptableObject {
        public int totalCount => animals.Count;
        public int inCount => animals.Count(animal => animal.isCaged);
        public int outCount => totalCount - inCount;

        readonly HashSet<AnimalBehavior> animals = new();
        public void RegisterAnimal(AnimalBehavior animal) {
            animals.Add(animal);
        }
        public void UnregisterAnimal(AnimalBehavior animal) {
            animals.Remove(animal);
        }

#if UNITY_EDITOR
        [Header("Editor Config")]
        [SerializeField]
        string prefabPath = "Assets/Prefabs/Animals";
        [SerializeField]
        PhysicMaterial defaultMaterial;
        [SerializeField, Layer]
        int defaultLayer;
        [SerializeField]
        float defaultDrag = 0.5f;
        [SerializeField]
        GameObject defaultLinePrefab;

        [Header("Storage")]
        [SerializeField]
        SerializableKeyValuePairs<string, GameObject> models = new();
        [SerializeField]
        SerializableKeyValuePairs<string, RuntimeAnimatorController> animators = new();
        [SerializeField]
        SerializableKeyValuePairs<string, GameObject> prefabs = new();

        [ContextMenu(nameof(LoadAnimals))]
        public void LoadAnimals() {
            models.SetItems(GetAssets<GameObject>("_Animations"));
            animators.SetItems(GetAssets<RuntimeAnimatorController>("AC_"));
            prefabs.SetItems(GetAssets<GameObject>("P_Animal_"));
        }

        [ContextMenu(nameof(CreateMissingAnimals))]
        public void CreateMissingAnimals() {
            foreach (var (name, model) in models) {
                if (prefabs.TryGetValue(name, out var prefab)) {
                    UpdateBehavior(prefab.GetComponent<Animal>());
                    UnityEditor.EditorUtility.SetDirty(prefab);
                    continue;
                }

                prefab = new GameObject($"P_Animal_{name}");
                var animal = prefab.AddComponent<Animal>();
                animal.model = model;
                animal.bounds = model.GetComponentInChildren<SkinnedMeshRenderer>().bounds;
                animal.animator = animators[name];
                UpdateBehavior(animal);
                UnityEditor.PrefabUtility.SaveAsPrefabAsset(prefab, $"{prefabPath}/P_Animal_{name}.prefab");
            }

            LoadAnimals();
        }
        void UpdateBehavior(Animal animal) {
            animal.layer = defaultLayer;
            animal.baseDrag = defaultDrag;
            animal.manager = this;
            if (!animal.linePrefab) {
                animal.linePrefab = defaultLinePrefab;
            }

            if (!animal.material) {
                animal.material = defaultMaterial;
            }
        }
        Dictionary<string, T> GetAssets<T>(string name) where T : UnityEngine.Object {
            return UnityEditor.AssetDatabase.GetAllAssetPaths()
                .Where(path => path.Contains(name))
                .Select(UnityEditor.AssetDatabase.LoadAssetAtPath<T>)
                .OfType<T>()
                .OrderBy(asset => asset.name)
                .ToDictionary(asset => asset.name.Replace(name, ""));
        }
#endif
    }
}
