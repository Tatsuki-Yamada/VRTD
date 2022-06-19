using UnityEngine;

public class EnemyManager : SingletonMonoBehaviour<EnemyManager>
{
    [SerializeField] GameObject EnemyPrefab;


    public void CreateEnemy()
    {
        GameObject tempObj = Instantiate(EnemyPrefab, new Vector3(
            GameFieldManager.Instance.enemyBasePosX + GameFieldManager.Instance.createFieldOffsetX,
            GameFieldManager.Instance.createFieldOffsetY + 0.5f,
            GameFieldManager.Instance.enemyBasePosY + GameFieldManager.Instance.createFieldOffsetZ), Quaternion.identity
        );

        tempObj.GetComponent<EnemyController>().Reset(GameFieldManager.Instance.enemyBasePosX, GameFieldManager.Instance.enemyBasePosY, GameFieldManager.Instance.enemyPath);

    }

}
