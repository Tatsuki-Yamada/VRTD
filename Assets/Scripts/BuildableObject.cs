using UnityEngine;
using TMPro;

public class BuildableObject : MonoBehaviour
{
    [SerializeField] protected TextMeshPro buildCounter;
    protected BuildCounterController bcc;

    protected bool isBuilding = false;

    public virtual void Awake()
    {
        bcc = buildCounter.gameObject.GetComponent<BuildCounterController>();
    }


    public virtual void StartBuild() 
    {
        if (!isBuilding)
            isBuilding = true;
    }


    public virtual void CompleteBuild() 
    {
        isBuilding = false;
    }


    void OnTriggerEnter(Collider other)
    {
        if (!isBuilding)
            return;

        if (other.CompareTag("Hammer"))
        {
            buildCounter.gameObject.GetComponent<BuildCounterController>().DecreaseCount();
        }
    }
}
