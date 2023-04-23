using System;
using UnityEngine;
using ZSBB.AnimalBT;
using UnityObject = UnityEngine.Object;

namespace ZSBB {
    sealed class OnCageEnter : MonoBehaviour {
        //private AudioSource _audioSource;

        private void Start() {
            //_audioSource = GetComponent<AudioSource>();
        }

        private void OnTriggerEnter(Collider collision) {
            if (collision.GetComponent<Rigidbody>() is Rigidbody rigidbody) {
                rigidbody.SendMessage(nameof(IRelocationMessages.OnCaged), SendMessageOptions.DontRequireReceiver);
            }
            // play funny sound effect.
            //_audioSource.Play();
        }
    }
}
