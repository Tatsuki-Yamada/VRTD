using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Text.RegularExpressions;

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


    float timeToNextWave_sec = 15f;


    /// <summary>
    /// キューに指定したWaveを登録する初期化処理
    /// </summary>
    /// <param name="waveIndex"></param>
    public void Init(int waveIndex)
    {
        switch (waveIndex)
        {
            case 1:
                AddQueue("normal : 2, wait : 2, normal : 2");
                break;
            case 2:
                AddQueue("normal : 4, wait : 2, normal : 4");
                break;
            case 3:
                AddQueue("normal : 6, wait : 2, normal : 6");
                break;
            case 4:
                AddQueue("walk : 2, wait : 2, normal : 2");
                break;
            case 5:
                AddQueue("walk : 4, wait : 2, walk : 4");
                break;
            case 6:
                AddQueue("walk : 6, wait : 2, walk : 6");
                break;
            case 7:
                AddQueue("fly : 2, wait : 2, fly : 2");
                break;
            case 8:
                AddQueue("fly : 4, wait : 2, fly : 4");
                break;
            case 9:
                AddQueue("fly : 6, wait : 2, fly : 6");
                break;
            default:
                string queueString = "";
                int maxSpawnOneTime = 4 + waveIndex / 4;

                int setAmount = Random.Range(2, 5 + waveIndex / 8);
                for (int i = 0; i < setAmount; i++)
                {
                    queueString += Utils.GetRandom<string>(new List<string> { "normal", "walk", "fly" });
                    queueString += ":";
                    queueString += Random.Range(1, maxSpawnOneTime + 1).ToString();
                    queueString += ",";
                    queueString += "wait : 2, ";
                }

                AddQueue(queueString);
                break;
        }
    }


    /// <summary>
    /// Init関数からもらったデータをデコードし、キューに登録する関数
    /// </summary>
    /// <param name="data"></param>
    void AddQueue(string data)
    {
        // Debug.Log(data);


        data += ", wait : " + timeToNextWave_sec.ToString();
        data = data.Replace(" ", "");
        string[] splitData = data.Split(",");

        int waitSecs = 2;
        foreach (string d in splitData)
        {
            if (d == "")
                continue;

            if (int.TryParse(d.Split(":")[1], out int num))
            {
                waitSecs += num;
            }
        }

        UIManager.Instance.SetWaveTimer(waitSecs);

        foreach (string d in splitData)
        {
            if (d == "")
                continue;
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
                EnemyManager.Instance.CreateNormalEnemy();
                waitTime = 1f;
                break;

            case 2:
                EnemyManager.Instance.CreateWalkEnemy();
                waitTime = 1f;
                break;

            case 3:
                EnemyManager.Instance.CreateFlyEnemy();
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
