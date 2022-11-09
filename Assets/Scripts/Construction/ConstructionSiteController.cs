using UnityEngine;
using UnityEngine.Events;
using TMPro;


public class ConstructionSiteController : MonoBehaviour
{
    [SerializeField] TextMeshPro myTextMesh_toShowCount;

    int count_toCompleteBuild;

    bool isEnableJustHitAnim = false;

    public bool isActive_toJudgeReusable = true;

    // このEventに登録された関数が、カウンターが0になったとき呼ばれる。
    public UnityEvent onCompleteBuildFuncs_toCallback = new UnityEvent();


    private void Awake()
    {
        AttatchNullCheck();
    }


    public void Init(Vector3 pos_toSetThisPosition, int needCount_toSetCounter)
    {
        transform.position = pos_toSetThisPosition;
        count_toCompleteBuild = needCount_toSetCounter;
        UpdateText();

        isActive_toJudgeReusable = true;
    }


    private void Update()
    {
        if (!isEnableJustHitAnim)
            return;

        // TODO. ジャストヒットのアニメーション処理を書く。
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hammer"))
        {
            DecreaseCountAndStartAnim();
        }
    }


    public void DecreaseCountAndStartAnim(int amountOfDecrease = 1)
    {
        if (!isEnableJustHitAnim)
            isEnableJustHitAnim = true;

        count_toCompleteBuild -= amountOfDecrease;
        UpdateText();

        if (count_toCompleteBuild <= 0)
        {
            onCompleteBuildFuncs_toCallback.Invoke();
            Disable();
        }
    }

    private void UpdateText()
    {
        myTextMesh_toShowCount.text = count_toCompleteBuild.ToString();
    }


    private void Disable()
    {
        onCompleteBuildFuncs_toCallback.RemoveAllListeners();

        transform.position = new Vector3(300, 300, 300);
        isActive_toJudgeReusable = false;
    }


    private void AttatchNullCheck()
    {
        Debug.Log("ConstructionSiteController Null Checking.");
        if (!myTextMesh_toShowCount) Debug.LogError("No attached error.");
    }
}
