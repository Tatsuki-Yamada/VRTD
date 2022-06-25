using UnityEngine;
using System.Linq;

public class InputManager : MonoBehaviour
{
    // 左右のコントローラー
    [SerializeField] GameObject rightController;
    [SerializeField] GameObject leftController;

    // 左右のコントローラーから出るLineRenderer
    [SerializeField] LineRenderer leftRayObject;
    [SerializeField] LineRenderer rightRayObject;

    // プレイヤーオブジェクト
    [SerializeField] GameObject VRPlayer;

    // 右スティック左右で回転する角度
    [SerializeField] int rotateRatio = 45;

    // CompareTagsで比較対象になるタイルのタグリスト
    string[] tileTags = { "Tile", "Tile_None", "Tile_Road", "Tile_CanBuild", "Tile_EnemyBase", "Tile_PlayerBase" };

    // CompareTagsで比較対象になるタワーのタグリスト
    string[] towerTags = { "Tower" };


    // 移動でコールバックを使うための一時変数
    Vector3 tempMovePos;


    void Update()
    {
        // Rayを移動する
        leftRayObject.positionCount = 2;
        leftRayObject.SetPosition(0, leftController.transform.position);
        leftRayObject.SetPosition(1, leftController.transform.position + leftController.transform.forward * 20f);

        rightRayObject.positionCount = 2;
        rightRayObject.SetPosition(0, rightController.transform.position);
        rightRayObject.SetPosition(1, rightController.transform.position + rightController.transform.forward * 20f);

        // 右人差し指トリガー
        if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
        {
            RaycastHit[] hits;
            hits = Physics.RaycastAll(rightController.transform.position, rightController.transform.forward, 100.0f);

            foreach (RaycastHit hit in hits)
            {
                if (!(CompareTags(tileTags, hit.collider.tag) || CompareTags(towerTags, hit.collider.tag)))
                    continue;

                if (hit.collider.CompareTag("Tile_None") || hit.collider.CompareTag("Tile_CanBuild"))
                {
                    hit.collider.GetComponent<TileController>().StartBuild();
                }
                else if (hit.collider.CompareTag("Tower"))
                {
                    hit.collider.GetComponent<TowerFloorController>().StartBuild();
                }

                break;
            }
        }

        // 左人差し指トリガー
        if (OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger))
        {
            RaycastHit[] hits;
            hits = Physics.RaycastAll(leftController.transform.position, leftController.transform.forward, 100.0f);
            foreach (RaycastHit hit in hits)
            {
                if (CompareTags(tileTags, hit.collider.tag))
                {
                    tempMovePos = hit.collider.GetComponent<TileController>().GetPos();
                    FadeManager.Instance.onFadeInComplete.AddListener(PlayerMove);
                    FadeManager.Instance.Fade();

                    break;
                }
            }
        }

        // 右スティック左右
        if (OVRInput.GetDown(OVRInput.RawButton.RThumbstickRight) || OVRInput.GetDown(OVRInput.RawButton.RThumbstickLeft))
        {
            Vector2 temp = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);
            if (temp.x > 0)
                temp.x = 1f;
            else
                temp.x = -1f;

            VRPlayer.transform.Rotate(new Vector3(0f, temp.x * rotateRatio, 0f));
        }



        DebugKeyInput();
        
    }


    /// <summary>
    /// コールバックで呼ばれる関数
    /// </summary>
    void PlayerMove()
    {
        VRPlayer.transform.position = tempMovePos;
    }


    /// <summary>
    /// 複数タグのどれかに一致するか調べる関数
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    bool CompareTags(string[] tags, string targetTag)
    {
        foreach (string t in tags)
        {
            if ((targetTag) == t)
                return true;
        }

        return false;

    }


    /// <summary>
    /// PCデバッグ用のキー入力をまとめた関数
    /// </summary>
    void DebugKeyInput()
    {
        // デバッグ移動の移動量と回転量
        float moveRatio = 0.025f;
        float rotRatio = 1f;

        if (Input.GetKey(KeyCode.RightArrow))
            VRPlayer.transform.Rotate(new Vector3(0, rotRatio, 0), Space.World);
        if (Input.GetKey(KeyCode.LeftArrow))
            VRPlayer.transform.Rotate(new Vector3(0, -rotRatio, 0), Space.World);
        if (Input.GetKey(KeyCode.UpArrow))
            VRPlayer.transform.Rotate(new Vector3(-rotRatio, 0, 0));
        if (Input.GetKey(KeyCode.DownArrow))
            VRPlayer.transform.Rotate(new Vector3(rotRatio, 0, 0));
        if (Input.GetKey(KeyCode.W) || OVRInput.Get(OVRInput.RawButton.LThumbstickUp))
            VRPlayer.transform.Translate(0, 0, moveRatio);
        if (Input.GetKey(KeyCode.S) || OVRInput.Get(OVRInput.RawButton.LThumbstickDown))
            VRPlayer.transform.Translate(0, 0, -moveRatio);
        if (Input.GetKey(KeyCode.A) || OVRInput.Get(OVRInput.RawButton.LThumbstickLeft))
            VRPlayer.transform.Translate(-moveRatio, 0, 0);
        if (Input.GetKey(KeyCode.D) || OVRInput.Get(OVRInput.RawButton.LThumbstickRight))
            VRPlayer.transform.Translate(moveRatio, 0, 0);
        if (Input.GetKey(KeyCode.Space) || OVRInput.Get(OVRInput.RawButton.Y))
            VRPlayer.transform.Translate(0, moveRatio, 0, Space.World);
        if (Input.GetKey(KeyCode.LeftShift) || OVRInput.Get(OVRInput.RawButton.X))
            VRPlayer.transform.Translate(0, -moveRatio, 0, Space.World);


        // フェードのテスト
        if (Input.GetKeyDown(KeyCode.F))
        {
            FadeManager.Instance.Fade();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            EnemyManager.Instance.CreateEnemy();
        }


        if (OVRInput.GetDown(OVRInput.RawButton.B))
        {
            EnemyManager.Instance.CreateEnemy();
        }

        
    }

}
