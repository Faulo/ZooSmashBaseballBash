using UnityEngine;
using ZSBB.AnimalBT;

namespace ZSBB {
    sealed class OnCageEnter : MonoBehaviour {
        AudioSource _audioSource;
        [SerializeField] AnimalCagePreference thisCage;

        void Start() {
            _audioSource = GetComponent<AudioSource>();
        }

        void OnTriggerEnter(Collider collision) {
            if (collision.attachedRigidbody is Rigidbody rigidbody) {
                rigidbody.SendMessage(
                    nameof(IRelocationMessages.OnCageEnter),
                    thisCage,
                    SendMessageOptions.DontRequireReceiver
                );
            }
            // play funny sound effect.
            //_audioSource.Play();
        }

        void OnTriggerExit(Collider collision) {
            if (collision.attachedRigidbody is Rigidbody rigidbody) {
                rigidbody.SendMessage(
                    nameof(IRelocationMessages.OnCageExit),
                    thisCage,
                    SendMessageOptions.DontRequireReceiver
                );
            }
            // play funny sound effect.
            _audioSource.Play();
        }
    }
}