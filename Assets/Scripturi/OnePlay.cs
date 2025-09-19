using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class OneTimeAudioTrigger : MonoBehaviour
{
    private bool hasPlayed = false;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        // Make sure the audio source is NOT set to "Play On Awake"
        audioSource.playOnAwake = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!hasPlayed && other.CompareTag("Player")) // Make sure your player has the "Player" tag
        {
            audioSource.Play();
            hasPlayed = true;
        }
    }
}
