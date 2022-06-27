using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class FadeManager : SingletonMonoBehaviour<FadeManager>
{
    // 有効かを示すフラグ
    bool isActive = false;

    // リセットされてから経過した時間
    float elapsedTime = 0f;

    // 各アニメーションの所要時間
    float fadeInTime = 0f;
    float waitTime = 0f;
    float fadeOutTime = 0f;

    // フェードに使う黒い画像
    [SerializeField] Image FadeImage;

    // コールバック関数の入れ物
    [System.NonSerialized] public UnityEvent onFadeInComplete = new UnityEvent();
    [System.NonSerialized] public UnityEvent onWaitComplete = new UnityEvent();
    [System.NonSerialized] public UnityEvent onFadeOutComplete = new UnityEvent();


    /// <summary>
    /// アクティブ中はアニメーション処理を行う。
    /// </summary>
    void Update()
    {
        if (isActive)
        {
            elapsedTime += Time.deltaTime;

            // フェードイン中の処理
            if (elapsedTime < fadeInTime)
            {
                FadeImage.color = new Color(0, 0, 0, 1 * (elapsedTime / fadeInTime));
            }
            // フェードイン完了後の待機時間中の処理
            else if(elapsedTime < fadeInTime + waitTime)
            {
                onFadeInComplete.Invoke();
            }
            // フェードアウト中の処理
            else if (elapsedTime < fadeInTime + waitTime + fadeOutTime)
            {
                FadeImage.color = new Color(0, 0, 0, 1 - (1 * (elapsedTime / (fadeInTime + waitTime + fadeOutTime))));
                onWaitComplete.Invoke();
            }
            // 全ての流れが終了したときの処理
            else
            {
                onFadeOutComplete.Invoke();
                isActive = false;
            }
        }
    }


    /// <summary>
    /// フェードを始める関数。各アニメーションの時間を指定することも可。
    /// </summary>
    /// <param name="fadeInTime"></param>
    /// <param name="waitTime"></param>
    /// <param name="fadeOutTime"></param>
    public void Fade(float fadeInTime = 0.5f, float waitTime = 0.25f, float fadeOutTime = 0.5f)
    {
        // 時間関係のリセット
        elapsedTime = 0f;
        this.fadeInTime = fadeInTime;
        this.waitTime = waitTime;
        this.fadeOutTime = fadeOutTime;

        isActive = true;
    }
}
