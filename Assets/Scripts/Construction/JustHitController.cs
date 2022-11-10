using UnityEngine;

public class JustHitController : MonoBehaviour
{
    [SerializeField] ConstructionSiteController myConstructionSite_toCallAnimationFunc;


    private void Awake()
    {
        AttatchNullCheck();
    }


    public void AnimationFirstFrameFunc()
    {
        myConstructionSite_toCallAnimationFunc.AnimationFirstFrameFunc();
    }


    public void AnimationEnterJustCircleFunc()
    {
        myConstructionSite_toCallAnimationFunc.AnimationEnterJustCircleFunc();
    }


    public void AnimationEndFrameFunc()
    {
        myConstructionSite_toCallAnimationFunc.AnimationEndFrameFunc();
    }


    private void AttatchNullCheck()
    {
        Debug.Log("JustHitController Null Checking.");
        if (!myConstructionSite_toCallAnimationFunc) Debug.LogError("No attached error.");
    }
}
