using UnityEngine;

public class SoundPhysicsBody : MonoBehaviour
{
    private AudioSource audioSource;
    private Rigidbody rigidbody;

    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private bool debugLogs = false;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other) {
        var volume = rigidbody.linearVelocity.magnitude / 10f;
        var clip = audioClips[Random.Range(0, audioClips.Length)];
        audioSource.PlayOneShot(clip, volume);

        if (debugLogs) {
            Debug.Log($"Played clip {clip.name} with volume {volume}");
        }
    }   
}
