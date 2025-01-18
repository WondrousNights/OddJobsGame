using UnityEngine;
//using AlmenaraGames;

public class SoundPhysicsBodyMulti : MonoBehaviour
{
    //[SerializeField] private AudioObject audioObject;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        // prevent sounds from playing on start
        if (Time.timeSinceLevelLoad > 0.2) PlaySound();
    }   

    private void PlaySound()
    {
        // volume based on velocity of object at the time of collision
        var volume = rb.linearVelocity.sqrMagnitude * 0.01f + 0.01f;
        //audioObject.volume = volume;
        
        //MultiAudioManager.PlayAudioObject(audioObject, transform.position);
    }
}
