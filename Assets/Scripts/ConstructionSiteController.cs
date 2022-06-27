using UnityEngine;

public class ConstructionSiteController : MonoBehaviour
{
    BuildCounterController targetBcc;
    int count = 0;

    public void SetBcc(BuildCounterController bcc, int needCount)
    {
        targetBcc = bcc;
        count = needCount;
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hammer"))
        {
            targetBcc.DecreaseCount();
            count--;
            if (count <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
