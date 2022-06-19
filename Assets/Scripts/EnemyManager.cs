using UnityEngine;

public class EnemyManager : SingletonMonoBehaviour<EnemyManager>
{
    [SerializeField] GameObject EnemyPrefab;

    [SerializeField] Transform enemyParent;

    // ŽËŒ‚‚µ‚Ä‚©‚çŒo‰ß‚µ‚½ŽžŠÔ
    float timeFromLastSpawn = 0f;

    // •‘•‚ÌƒŠƒ[ƒh‚É‚©‚©‚éŽžŠÔ
    float spawnIntervalTime = 5f;


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
