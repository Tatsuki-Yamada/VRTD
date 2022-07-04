using UnityEngine;

public class HammerController : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ConstructionSite"))
        {
            SoundManager.Instance.PlaySound(transform.position, 0);
        }
    }
}
