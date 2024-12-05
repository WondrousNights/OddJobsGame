using UnityEngine;

public class SoundPhysicsBody : MonoBehaviour
{
    private AudioSource audioSource;
    private Rigidbody rigidbody;

    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private bool debugLogs = false;
    [SerializeField] private float pitchRange = 0.2f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        // prevent sounds from playing on start
        if (Time.timeSinceLevelLoad > 1) PlaySound();
    }   

    private void PlaySound()
    {
        // volume based on velocity of object at the time of collision
        var volume = rigidbody.linearVelocity.sqrMagnitude * 0.01f + 0.01f;
        // pitch is random but within a range
        var pitch = Random.Range(1 - pitchRange, 1 + pitchRange);
        // pick a random clip from the list
        var clip = audioClips[Random.Range(0, audioClips.Length)];
        audioSource.pitch = pitch;
        audioSource.PlayOneShot(clip, volume);

        if (debugLogs) {
            Debug.Log($"Played clip {clip.name} with volume {volume}");
        }
    }
}
