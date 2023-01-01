using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Enemy;

public class EnemyManager : SingletonMonoBehaviour<EnemyManager>
{
    // 敵のPrefabたち
    [SerializeField] GameObject[] EnemyPrefabs;

    // 敵の親オブジェクト
    [SerializeField] Transform enemyParent;

    // 生成した敵が入るリスト
    List<Object> enemyList_toReuse = new List<Object>();

    // 現在のWave数
    int waveCount = 1;

    [System.NonSerialized] public float enemyBaseHP_toIncreaseByWave = 100;


    // TODO  最初のWave生成をGameManagerとかに移す
    private void Start()
    {
        CreateWave(waveCount);
    }


    public void CreateNormalEnemy()
    {
        CreateEnemy<NormalEnemyController>();
    }

    public void CreateWalkEnemy()
    {
        CreateEnemy<WalkEnemyController>();
    }

    public void CreateFlyEnemy()
    {
        CreateEnemy<FlyEnemyController>();
    }

    /*
        // 敵を生成・再利用する
        private void CreateEnem(int enemyIndex, float  = 0)
        {



            // 無効状態の弾があれば再利用する
            foreach (EnemyController e in enemyList_toReuse)
            {
                if (!e.isActive)
                {
                    e.Init();
                    return;
                }
            }

            // 生成する
            EnemyController tempEnemy = Instantiate(EnemyPrefabs[enemyIndex], parent: enemyParent).GetComponent<EnemyController>();

            // 経路を初期化する
            tempEnemy.Init();

            // 管理リストに登録する
            enemyList_toReuse.Add(tempEnemy);
        }
    */

    private void CreateEnemy<T>() where T : EnemyControllerBase
    {
        // enemyListからTだけのリストを作成する。
        List<T> oneTypeList_toSearchDisableEnemy = enemyList_toReuse.OfType<T>().ToList();

        // 無効状態の敵がいれば再利用する。
        foreach (T type in oneTypeList_toSearchDisableEnemy)
        {
            if (type.isActive == false)
            {
                type.Init();
                return;
            }
        }

        GameObject toInstantiateObj = null;

        // TをアタッチしたPrefabを探す。
        foreach (GameObject obj_toSearchHasTypeComponent in EnemyPrefabs)
        {
            if (obj_toSearchHasTypeComponent.TryGetComponent<T>(out T temp))
            {
                toInstantiateObj = obj_toSearchHasTypeComponent;
                break;
            }
        }

        // TをアタッチしたPrefabが見つからなかったらエラー
        if (toInstantiateObj == null)
            Debug.LogError("EnemyManager: toInstantiateObj = null");

        T typeObj_toAddList = Instantiate(toInstantiateObj, parent: enemyParent).GetComponent<T>();
        typeObj_toAddList.Init();
        enemyList_toReuse.Add(typeObj_toAddList);
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

        enemyBaseHP_toIncreaseByWave *= 1.1f;
        waveCount += 1;

        // TODO. 最終的に消す
        int thinkingMaxWaveNum = 5;

        CreateWave(waveCount % thinkingMaxWaveNum);
    }
}
