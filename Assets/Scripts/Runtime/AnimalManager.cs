using System.Collections.Generic;
using System.Linq;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace ZSBB {
    [CreateAssetMenu]
    sealed class AnimalManager : ScriptableObject {
        [Header("Config")]
        [SerializeField]
        string prefabPath = "Assets/Prefabs/Animals";
        [SerializeField]
        PhysicMaterial defaultMaterial;
        [SerializeField, Layer]
        int defaultLayer;

        [Header("Storage")]
        [SerializeField]
        SerializableKeyValuePairs<string, GameObject> models = new();
        [SerializeField]
        SerializableKeyValuePairs<string, RuntimeAnimatorController> animators = new();
        [SerializeField]
        SerializableKeyValuePairs<string, GameObject> prefabs = new();

#if UNITY_EDITOR
        [ContextMenu(nameof(LoadAnimals))]
        public void LoadAnimals() {
            models.SetItems(GetAssets<GameObject>("_Animations"));
            animators.SetItems(GetAssets<RuntimeAnimatorController>("AC_"));
            prefabs.SetItems(GetAssets<GameObject>("P_Animal_"));
        }
        [ContextMenu(nameof(CreateMissingAnimals))]
        public void CreateMissingAnimals() {
            foreach (var (name, model) in models) {
                if (prefabs.ContainsKey(name)) {
                    continue;
                }
                var prefab = new GameObject($"P_Animal_{name}");
                var animal = prefab.AddComponent<Animal>();
                animal.model = model;
                animal.bounds = model.GetComponentInChildren<SkinnedMeshRenderer>().bounds;
                animal.animator = animators[name];
                animal.material = defaultMaterial;
                animal.layer = defaultLayer;
                UnityEditor.PrefabUtility.SaveAsPrefabAsset(prefab, $"{prefabPath}/P_Animal_{name}.prefab");
            }

            LoadAnimals();
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
