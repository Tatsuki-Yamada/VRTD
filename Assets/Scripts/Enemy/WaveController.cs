using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveController : MonoBehaviour
{
    // Waveで生成する敵の順番が入るキュー
    Queue<int> waveQueue = new Queue<int>();

    // Waveが完了した時に呼ばれるコールバック
    [System.NonSerialized] public UnityEvent onCompleteWave = new UnityEvent();

    // 最後にキューの行動をしてから経過した時間
    float timeFromLastAction = 0f;

    // 次の行動までの待ち時間
    float waitTime = 0f;


    /// <summary>
    /// キューに指定したWaveを登録する初期化処理
    /// </summary>
    /// <param name="waveIndex"></param>
    public void Init(int waveIndex)
    {
        switch (waveIndex)
        {
            case 1:
                AddQueue("normal : 1, walk : 1, fly : 1, wait : 3, normal : 2 ");
                break;
        }
    }


    /// <summary>
    /// Init関数からもらったデータをデコードし、キューに登録する関数
    /// </summary>
    /// <param name="data"></param>
    void AddQueue(string data)
    {
        data = data.Replace(" ", "");
        string[] splitData = data.Split(",");


        foreach (string d in splitData)
        {
            string first = d.Split(":")[0];
            int second = int.Parse(d.Split(":")[1]);

            switch (first)
            {
                case "wait":
                    waveQueue.Enqueue(0);
                    waveQueue.Enqueue(second);
                    break;

                case "normal":
                    for (int i = 0; i < second; i++)
                    {
                        waveQueue.Enqueue(1);
                    }
                    break;

                case "walk":
                    for (int i = 0; i < second; i++)
                    {
                        waveQueue.Enqueue(2);
                    }
                    break;

                case "fly":
                    for (int i = 0; i < second; i++)
                    {
                        waveQueue.Enqueue(3);
                    }
                    break;
            }
        }

        // 次のwaveに行くまでの仮の待機時間
        waveQueue.Enqueue(0);
        waveQueue.Enqueue(2);

        // wave終了の登録
        waveQueue.Enqueue(-1);
    }


    void Update()
    {
        timeFromLastAction += Time.deltaTime;

        if (timeFromLastAction >= waitTime)
        {
            DoQueue(waveQueue.Dequeue());
        }
    }


    /// <summary>
    /// 引数で指定された行動を行う関数
    /// </summary>
    /// <param name="action"></param>
    void DoQueue(int action)
    {
        switch (action)
        {
            // Wave終了
            case -1:
                CompleteWave();
                break;

            // 待機
            case 0:
                waitTime = waveQueue.Dequeue();
                break;

            case 1:
                EnemyManager.Instance.CreateEnemy(0);
                waitTime = 1f;
                break;

            case 2:
                EnemyManager.Instance.CreateEnemy(1);
                waitTime = 1f;
                break;

            case 3:
                EnemyManager.Instance.CreateEnemy(2);
                waitTime = 1f;
                break;
        }

        timeFromLastAction = 0f;
    }


    /// <summary>
    /// Wave終了のコールバックを呼ぶ関数
    /// </summary>
    void CompleteWave()
    {
        onCompleteWave.Invoke();
    }
}
