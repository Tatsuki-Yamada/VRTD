using UnityEngine;

public class EnemyManager : SingletonMonoBehaviour<EnemyManager>
{
    [SerializeField] GameObject EnemyPrefab;

    [SerializeField] Transform enemyParent;

    // 最後にスポーンしてから経過した時間
    float timeFromLastSpawn = 0f;

    // 次のスポーンまでの間隔
    float spawnIntervalTime = 2f;



    private void Awake()
    {
        timeFromLastSpawn += 2;
    }


    void Update()
    {
        timeFromLastSpawn += Time.deltaTime;


        if (timeFromLastSpawn > spawnIntervalTime)
        {
            CreateEnemy();
            timeFromLastSpawn = 0f;
        }
        
    }


    public void CreateEnemy()
    {
        GameObject tempObj = Instantiate(EnemyPrefab, new Vector3(
            GameFieldManager.Instance.enemyBasePosX + GameFieldManager.Instance.createFieldOffsetX,
            GameFieldManager.Instance.createFieldOffsetY + 0.5f,
            GameFieldManager.Instance.enemyBasePosY + GameFieldManager.Instance.createFieldOffsetZ), Quaternion.identity, enemyParent
        );

        tempObj.GetComponent<EnemyController>().Reset(GameFieldManager.Instance.enemyBasePosX, GameFieldManager.Instance.enemyBasePosY, GameFieldManager.Instance.enemyPath);

    }
}
