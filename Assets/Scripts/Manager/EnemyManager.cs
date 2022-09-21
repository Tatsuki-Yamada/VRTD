using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : SingletonMonoBehaviour<EnemyManager>
{
    // 敵のPrefabたち
    [SerializeField] GameObject[] EnemyPrefabs;

    // 敵の親オブジェクト
    [SerializeField] Transform enemyParent;

    // 生成した敵が入るリスト
    List<EnemyController> enemies = new List<EnemyController>();

    // 現在のWave数
    int waveCount = 1;


    // TODO  最初のWave生成をGameManagerとかに移す
    private void Start()
    {
        CreateWave(waveCount);
    }


    // 敵を生成・再利用する
    public void CreateEnemy(int enemyIndex)
    {
        // 無効状態の弾があれば再利用する
        foreach (EnemyController e in enemies)
        {
            if (!e.isActive)
            {
                e.transform.position = GameFieldManager.Instance.GetEnemySpawnPos();
                e.Init(GameFieldManager.Instance.enemyBasePosX, GameFieldManager.Instance.enemyBasePosY, GameFieldManager.Instance.enemyPath);
                return;
            }
        }

        // 生成する
        EnemyController tempEnemy = Instantiate(EnemyPrefabs[enemyIndex], GameFieldManager.Instance.GetEnemySpawnPos(), Quaternion.identity, enemyParent).GetComponent<EnemyController>();

        // 経路を初期化する
        tempEnemy.Init(GameFieldManager.Instance.enemyBasePosX, GameFieldManager.Instance.enemyBasePosY, GameFieldManager.Instance.enemyPath);

        // 管理リストに登録する
        enemies.Add(tempEnemy);
    }


    // Waveコントローラーを生成して設定する
    void CreateWave(int wave)
    {
        wave = waveCount;

        WaveController waveController = this.gameObject.AddComponent<WaveController>();
        waveController.Init(wave);
        waveController.onCompleteWave.AddListener(CompleteWave);
    }


    // Waveが終了したときに呼ばれるコールバック
    void CompleteWave()
    {
        Destroy(this.gameObject.GetComponent<WaveController>());

        // TODO. ここにwaveのインクリメント処理を書く
        waveCount += 0;

        CreateWave(waveCount);
    }
}
