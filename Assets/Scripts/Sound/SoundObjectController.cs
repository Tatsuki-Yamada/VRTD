using UnityEngine;

public class SoundObjectController : MonoBehaviour
{
    AudioSource audioSource;
    public bool isActive = false;


    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }


    void Update()
    {
        if (!isActive)
            return;

        if (!audioSource.isPlaying)
        {
            End();
        }
    }


    void End()
    {
        transform.position = new Vector3(200, 200, 200);

        isActive = false;
    }


    public void SetSoundAndStart(AudioClip sound)
    {
        audioSource.clip = sound;
        audioSource.Play();

        isActive = true;
    }
}
