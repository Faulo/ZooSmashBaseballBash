using UnityEngine;

namespace ZSBB {
    sealed class OnCageEnter : MonoBehaviour {
        //private AudioSource _audioSource;

        void Start() {
            //_audioSource = GetComponent<AudioSource>();
        }

        void OnTriggerEnter(Collider collision) {
            if (collision.GetComponent<Rigidbody>() is Rigidbody rigidbody) {
                rigidbody.SendMessage(
                    nameof(IRelocationMessages.OnCaged),
                    true,
                    SendMessageOptions.DontRequireReceiver
                );
            }
            // play funny sound effect.
            //_audioSource.Play();
        }

        void OnTriggerExit(Collider collision) {
            if (collision.GetComponent<Rigidbody>() is Rigidbody rigidbody) {
                rigidbody.SendMessage(
                    nameof(IRelocationMessages.OnCaged),
                    false,
                    SendMessageOptions.DontRequireReceiver
                );
            }
            // play funny sound effect.
            //_audioSource.Play();
        }
    }
}
