using MyBox;
using UnityEngine;

namespace ZSBB {
    sealed class SpeedTracker : MonoBehaviour {
        record Snapshot(Vector3 position, float deltaTime);

        public Vector3 position => transform.position;

        [SerializeField, Range(0, 10)]
        float m_speedSmoothing = 1f / 32;

        [SerializeField, Range(0, 10)]
        float m_isMovingThreshold = 1f / 8;

        [Header("Runtime")]
        [SerializeField, ReadOnly]
        Vector3 m_currentVelocity;
        public Vector3 velocity => m_currentVelocity;

        [SerializeField, ReadOnly]
        Vector3 m_direction;
        public Vector3 direction => m_direction;

        [SerializeField, ReadOnly]
        float m_speed;
        public float speed => m_speed;

        [SerializeField, ReadOnly]
        bool m_isMoving;
        public bool isMoving => m_isMoving;

        Snapshot previousSnapshot;
        Snapshot currentSnapshot => new(transform.position, Time.deltaTime);

        void Awake() {
            previousSnapshot = currentSnapshot;
        }

        void FixedUpdate() {
            var nextSnapshot = currentSnapshot;

            Vector3.SmoothDamp(
                previousSnapshot.position,
                nextSnapshot.position,
                ref m_currentVelocity,
                m_speedSmoothing
            );
            m_speed = m_currentVelocity.magnitude;
            m_isMoving = m_speed > m_isMovingThreshold;
            m_direction = m_isMoving
                ? m_currentVelocity.normalized
                : Vector3.zero;

            previousSnapshot = nextSnapshot;
        }
    }
}
