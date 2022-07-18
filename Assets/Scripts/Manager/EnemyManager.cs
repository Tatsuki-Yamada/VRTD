using UnityEngine;

public class EnemyManager : SingletonMonoBehaviour<EnemyManager>
{
    [SerializeField] GameObject[] EnemyPrefabs;

    [SerializeField] Transform enemyParent;

    // 現在のWave数
    int waveCount = 1;



    private void Start()
    {
        CreateWave(waveCount);
    }


    public void CreateEnemy(int enemyIndex)
    {
        GameObject tempObj = Instantiate(EnemyPrefabs[enemyIndex], new Vector3(
            GameFieldManager.Instance.enemyBasePosX + GameFieldManager.Instance.createFieldOffsetX,
            GameFieldManager.Instance.createFieldOffsetY + 0.5f,
            GameFieldManager.Instance.enemyBasePosY + GameFieldManager.Instance.createFieldOffsetZ), Quaternion.identity, enemyParent
        );

        tempObj.GetComponent<EnemyController>().Init(GameFieldManager.Instance.enemyBasePosX, GameFieldManager.Instance.enemyBasePosY, GameFieldManager.Instance.enemyPath);

    }


    void CreateWave(int wave)
    {
        wave = waveCount;

        WaveController waveController = this.gameObject.AddComponent<WaveController>();
        waveController.Init(wave);
        waveController.onCompleteWave.AddListener(CompleteWave);
    }


    void CompleteWave()
    {
        Destroy(this.gameObject.GetComponent<WaveController>());

        // TODO. ここにwaveのインクリメント処理を書く
        waveCount += 0;

        CreateWave(waveCount);
    }
}
