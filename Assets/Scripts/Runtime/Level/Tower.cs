using System;
using System.Collections.Generic;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace ZSBB.Level {
    [ExecuteAlways]
    sealed class Tower : MonoBehaviour {
        public static Tower instance { get; private set; }
        [SerializeField]
        ParticleSystem damageParticlesPrefab;
        [SerializeField]
        ParticleSystem destructionParticlesPrefab;

        [SerializeField]
        GameObject segmentPrefab;
        [SerializeField, Range(0, 100)]
        int segmentCount = 0;
        [SerializeField, Range(0, 10000)]
        float hitPointsPerSegment = 100;

        [SerializeField, Range(0, 10)]
        int segmentHeight = 2;
        [SerializeField]
        GameObject topPrefab;

        ParticleSystem damageParticles;
        readonly Queue<GameObject> segments = new();
        public int maxSegmentCount => segmentCount;
        public int currentSegmentCount => segments.Count;

        float currentHitPoints;

        float damageTaken;

        public bool TakeDamage(float damage) {
            if (segments.Count > 0) {
                damageTaken += damage;
                return true;
            } else {
                return false;
            }
        }

        void FixedUpdate() {
            if (damageTaken > 0) {
                damageParticles.Emit(Mathf.CeilToInt(damageTaken));
                currentHitPoints -= damageTaken;
                damageTaken = 0;
                if (currentHitPoints <= 0) {
                    currentHitPoints = hitPointsPerSegment;
                    if (segments.TryDequeue(out var segment)) {
                        Destroy(segment);
                        Instantiate(destructionParticlesPrefab, transform);
                    } else {
                        Debug.LogError("GAME OVER");
                    }
                }
            }
        }

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
            currentHitPoints = hitPointsPerSegment;
        }
        void SpawnTower() {
            transform.Clear();
            segments.Clear();
            if (segmentPrefab) {
                for (int i = 0; i < segmentCount; i++) {
                    float y = i * segmentHeight;
                    var instance = Instantiate(segmentPrefab, transform);
                    instance.hideFlags = HideFlags.DontSave;
                    instance.transform.localPosition = Vector3.up * y;
                    segments.Enqueue(instance);
                }
            }
            if (topPrefab) {
                float y = segmentCount * segmentHeight;
                var instance = Instantiate(topPrefab, transform);
                instance.hideFlags = HideFlags.DontSave;
                instance.transform.localPosition = Vector3.up * y;
            }
            if (damageParticlesPrefab) {
                damageParticles = Instantiate(damageParticlesPrefab, transform);
            }
        }
    }
}
