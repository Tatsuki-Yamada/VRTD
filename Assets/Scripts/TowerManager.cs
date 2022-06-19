using UnityEngine;

public class TowerManager : SingletonMonoBehaviour<TowerManager>
{
    [SerializeField] GameObject towerPrefab;
    float buildOffsetY = 0.5f;


    public void CreateTower(Vector3 tilePos)
    {
        Instantiate(towerPrefab, new Vector3(tilePos.x, tilePos.y + buildOffsetY, tilePos.z), Quaternion.identity);

    }
}
