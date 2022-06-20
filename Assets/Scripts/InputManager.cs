using UnityEngine;
using System.Linq;

public class InputManager : MonoBehaviour
{
    // 両コントローラー
    [SerializeField] GameObject rightController;
    [SerializeField] GameObject leftController;

    // コントローラーから出るレーザー
    [SerializeField] LineRenderer leftRayObject;
    [SerializeField] LineRenderer rightRayObject;

    // プレイヤー
    [SerializeField] GameObject VRPlayer;

    // 一度の右スティック入力で回転する角度
    [SerializeField] int rotateRatio = 45;

    // CompareTags関数で使用する、タイルの全タグリスト
    string[] tileTags = { "Tile", "Tile_None", "Tile_Road", "Tile_CanBuild", "Tile_EnemyBase", "Tile_PlayerBase" };

    // 同じく、タワーの全タグリスト
    string[] towerTags = { "Tower" };


    // コールバックの関係で移動時に使う一時変数
    Vector3 tempMovePos;


    void Update()
    {
        // 各ポインターオブジェクトの移動
        leftRayObject.positionCount = 2;
        leftRayObject.SetPosition(0, leftController.transform.position);
        leftRayObject.SetPosition(1, leftController.transform.position + leftController.transform.forward * 20f);

        rightRayObject.positionCount = 2;
        rightRayObject.SetPosition(0, rightController.transform.position);
        rightRayObject.SetPosition(1, rightController.transform.position + rightController.transform.forward * 20f);

        // 右人差し指ボタン
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

        // 左人差し指ボタン
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
    /// 移動処理でコールバックを使うために分離した関数。
    /// </summary>
    void PlayerMove()
    {
        VRPlayer.transform.position = tempMovePos;
    }


    /// <summary>
    /// 複数タグのどれかに一致するか調べる関数。
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
    /// PCデバッグ用のキー入力をまとめた関数。
    /// </summary>
    void DebugKeyInput()
    {
        // プレイヤーの移動周り
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
        if (Input.GetKey(KeyCode.W))
            VRPlayer.transform.Translate(0, 0, moveRatio);
        if (Input.GetKey(KeyCode.S))
            VRPlayer.transform.Translate(0, 0, -moveRatio);
        if (Input.GetKey(KeyCode.A))
            VRPlayer.transform.Translate(-moveRatio, 0, 0);
        if (Input.GetKey(KeyCode.D))
            VRPlayer.transform.Translate(moveRatio, 0, 0);
        if (Input.GetKey(KeyCode.Space) || OVRInput.Get(OVRInput.RawButton.LThumbstickUp))
            VRPlayer.transform.Translate(0, moveRatio, 0, Space.World);
        if (Input.GetKey(KeyCode.LeftShift) || OVRInput.Get(OVRInput.RawButton.LThumbstickDown))
            VRPlayer.transform.Translate(0, -moveRatio, 0, Space.World);


        // ゲームのデバッグコマンド
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
