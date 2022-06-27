// Ref: https://developers.10antz.co.jp/archives/1441

using UnityEngine;

/// <summary>
/// UIを操作する際にポインターを表示するクラスだが、必要かは不明
/// </summary>
public class UIPointer : OVRCursor
{
    [SerializeField] private GameObject _pointerObj;
    private bool _isCalledSetCursorCurrentFrame;

    void Start()
    {
        _pointerObj.SetActive(false);
    }

    private void LateUpdate()
    {
        if (!_isCalledSetCursorCurrentFrame)
        {
            _pointerObj.SetActive(false);
        }
        _isCalledSetCursorCurrentFrame = false;
    }

    public override void SetCursorRay(Transform ray)
    {

    }

    public override void SetCursorStartDest(Vector3 start, Vector3 dest, Vector3 normal)
    {
        _isCalledSetCursorCurrentFrame = true;
        _pointerObj.SetActive(true);

        transform.position = dest;
    }
}
