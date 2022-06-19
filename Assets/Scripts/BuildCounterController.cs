using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class BuildCounterController : MonoBehaviour
{
    // 表示するテキスト
    TextMeshPro textMesh;

    // 残りカウント
    int counter;

    // 有効かを示すフラグ
    bool isActive = false;

    // カウンターが0になった時に呼ばれるコールバック
    [System.NonSerialized] public UnityEvent onCompleteBuild = new UnityEvent();

    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
    }


    /// <summary>
    /// カウンターをリセットする関数。
    /// </summary>
    /// <param name="num"></param>
    public void SetCount(int num)
    {
        // カウンターのリセットとテキスト表示
        counter = num;
        textMesh.text = counter.ToString();
        GetComponent<Renderer>().enabled = true;

        isActive = true;
    }


    /// <summary>
    /// カウンターを減らす関数。指定しない場合は1ずつ減らす。
    /// </summary>
    /// <param name="num"></param>
    public void DecreaseCount(int num = 1)
    {
        if (isActive)
        {
            counter -= num;
            textMesh.text = counter.ToString();

            if (counter <= 0)
            {
                onCompleteBuild.Invoke();
                GetComponent<Renderer>().enabled = false;
                isActive = false;
            }
        }


    }
}
