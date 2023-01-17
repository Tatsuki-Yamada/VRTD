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


    public void SetSoundAndStart(AudioClip sound, bool RandamizeFlag = false)
    {
        audioSource.clip = sound;
        if (RandamizeFlag)
            audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.Play();

        isActive = true;
    }
}
