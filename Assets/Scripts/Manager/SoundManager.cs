using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    [SerializeField] List<AudioClip> soundList;

    List<SoundObjectController> soundObjects = new List<SoundObjectController>();

    [SerializeField] GameObject soundObjectPrefab;

    [SerializeField] Transform soundObjectParent;


    private void Start()
    {
        transform.position = Vector3.zero;
    }

    public void PlaySound(Vector3 pos, int audioIndex)
    {
        foreach (SoundObjectController s in soundObjects)
        {
            if (!s.isActive)
            {
                s.transform.position = pos;
                s.SetSoundAndStart(soundList[audioIndex]);
                return;
            }
        }
        
        SoundObjectController soc = Instantiate(soundObjectPrefab, pos, Quaternion.identity, soundObjectParent).GetComponent<SoundObjectController>();
        soc.SetSoundAndStart(soundList[audioIndex]);
        soundObjects.Add(soc);
    }


    public bool TestFunc()
    {
        return true;
    }
}
