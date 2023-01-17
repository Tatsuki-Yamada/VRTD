using UnityEngine;

public class HammerController : MonoBehaviour
{
    [SerializeField] ParticleSystem[] effects;
    [SerializeField] Transform effectPos;
    int effectPlayIndex = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ConstructionSite"))
        {
            SoundManager.Instance.PlaySound(transform.position, 0, true);
            effects[effectPlayIndex % 10].transform.position = effectPos.position;
            effects[effectPlayIndex % 10].Play();
            effectPlayIndex += 1;
        }
    }
}
