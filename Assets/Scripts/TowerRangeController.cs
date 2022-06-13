using UnityEngine;

public class TowerRangeController : MonoBehaviour
{
    // 親のスクリプト
    TowerController towerController;

    // タワーが有効になっているか
    public bool isTowerActive = false;


    void Awake()
    {
        towerController = transform.parent.GetComponent<TowerController>();
    }


    void OnTriggerEnter(Collider other)
    {
        if (isTowerActive)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                towerController.SetTargetEnemy(other.gameObject);
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
                towerController.RemoveTargetEnemy(other.gameObject);
                Debug.Log("Exit");
            }
        }
    }
}
