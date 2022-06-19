using UnityEngine;

public class TowerRangeController : MonoBehaviour
{
    // 親のスクリプト
    TowerFloorController towerFloorController;

    // タワーが有効になっているか
    public bool isTowerActive = false;


    void Awake()
    {
        towerFloorController = transform.parent.GetComponent<TowerFloorController>();
    }


    void OnTriggerEnter(Collider other)
    {
        if (isTowerActive)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                towerFloorController.SetTargetEnemy(other.gameObject);
                Debug.Log("Enter");
            }
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (isTowerActive)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                towerFloorController.RemoveTargetEnemy(other.gameObject);
                Debug.Log("Exit");
            }
        }
    }
}
