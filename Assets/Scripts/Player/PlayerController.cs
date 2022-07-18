using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject trackingSpace;

    bool posSetFlag = false;

    /// <summary>
    /// PCデバッグ用に、HMDを外したときに視点を装着中と同じようにする
    /// </summary>
    void Update()
    {
        if ((OVRManager.instance == null || !OVRManager.instance.isUserPresent) && !posSetFlag)
        {
            Vector3 pos = trackingSpace.transform.position;
            Vector3 angle = transform.eulerAngles;

            trackingSpace.transform.localPosition = new Vector3(pos.x, 1.2f, pos.z);
            trackingSpace.transform.eulerAngles = new Vector3(20, angle.y, angle.z);

            posSetFlag = true;
        }

        else if ((OVRManager.instance != null && OVRManager.instance.isUserPresent) && posSetFlag)
        {
            trackingSpace.transform.localPosition = Vector3.zero;
            trackingSpace.transform.eulerAngles = Vector3.zero;
        }

    }
}
