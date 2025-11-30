using UnityEngine;

public class MusicSource : MonoBehaviour
{
    
    public AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }

    
}
