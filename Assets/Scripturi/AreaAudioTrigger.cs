using UnityEngine;

public class AreaAudioTrigger : MonoBehaviour
{
    public AudioSource areaAudio;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!areaAudio.isPlaying)
                areaAudio.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (areaAudio.isPlaying)
                areaAudio.Stop();
        }
    }
}
