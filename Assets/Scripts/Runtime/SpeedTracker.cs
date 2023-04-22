using System.Collections.Generic;
using MyBox;
using UnityEngine;

namespace ZSBB {
    sealed class SpeedTracker : MonoBehaviour {
        [SerializeField, ReadOnly]
        float m_currentSpeed;
        public float currentSpeed => m_currentSpeed;

        [SerializeField]
        int bufferSize = 64;
        Queue<(Vector3 position, float deltaTime)> positionBuffer = new();

        void FixedUpdate() {
            // Füge die aktuelle Position in den Puffer hinzu
            positionBuffer.Enqueue((transform.position, Time.deltaTime));

            // Wenn der Puffer voll ist, entferne die älteste Position
            if (positionBuffer.Count > bufferSize) {
                positionBuffer.Dequeue();
            }

            // Berechne die durchschnittliche Geschwindigkeit basierend auf der Distanz zwischen allen aufeinanderfolgenden Positionen im Puffer
            float totalDistance = 0f;
            float totalTime = 0f;
            var positions = positionBuffer.ToArray();
            for (int i = 1; i < positions.Length; i++) {
                float distance = Vector3.Distance(positions[i].position, positions[i - 1].position);
                totalDistance += distance / i;
                totalTime += positions[i].deltaTime / i;
            }
            m_currentSpeed = totalDistance / totalTime;
        }
    }
}
