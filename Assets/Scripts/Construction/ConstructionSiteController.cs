using UnityEngine;
using UnityEngine.Events;
using TMPro;


public class ConstructionSiteController : MonoBehaviour
{
    [SerializeField] TextMeshPro myTextMesh_toShowCount;

    [SerializeField] Animator justHitAnimator_toSetFlags;

    [SerializeField] Canvas justHitCanvas_toSwitchVisible;

    int count_toCompleteBuild;

    bool isJustHit_toMultiplyHitCount = false;

    public bool isActive_toJudgeReusable = true;

    public BulletManager.BulletType toChangeType;

    public TowerController targetTC;

    public int floorNum;

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


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hammer"))
        {
            DecreaseCountAndStartAnim();
        }
    }


    public void DecreaseCountAndStartAnim(int amountOfDecrease = 1)
    {
        justHitCanvas_toSwitchVisible.enabled = true;
        justHitAnimator_toSetFlags.SetBool("MoveStart", true);

        if (isJustHit_toMultiplyHitCount)
        {
            amountOfDecrease *= 3;
            isJustHit_toMultiplyHitCount = false;
        }

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


    public void AnimationFirstFrameFunc()
    {
        justHitAnimator_toSetFlags.SetBool("MoveStart", false);
    }


    public void AnimationEnterJustCircleFunc()
    {
        isJustHit_toMultiplyHitCount = true;
    }


    public void AnimationEndFrameFunc()
    {
        isJustHit_toMultiplyHitCount = false;
        justHitCanvas_toSwitchVisible.enabled = false;
    }


    private void Disable()
    {
        onCompleteBuildFuncs_toCallback.RemoveAllListeners();

        transform.position = new Vector3(300, 300, 300);
        isActive_toJudgeReusable = false;
    }


    public void Evolve()
    {
        switch (floorNum)
        {
            case 0:
                targetTC.ChangeBotFloor(toChangeType);
                break;
            case 1:
                targetTC.ChangeMidFloor(toChangeType);
                break;
            case 2:
                targetTC.ChangeTopFloor(toChangeType);
                break;
        }

        targetTC.SetAllFloorsIsActive(true);
    }


    private void AttatchNullCheck()
    {
        // Debug.Log("ConstructionSiteController Null Checking.");
        if (!myTextMesh_toShowCount) Debug.LogError("No attached error.");
        if (!justHitAnimator_toSetFlags) Debug.LogError("No attached error.");
        if (!justHitCanvas_toSwitchVisible) Debug.LogError("No attached error.");
    }
}
