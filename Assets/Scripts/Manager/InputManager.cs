using UnityEngine;
using System.Linq;
using TMPro;

public class InputManager : MonoBehaviour
{
    // 左右のコントローラー
    [SerializeField] GameObject rightController;
    [SerializeField] GameObject leftController;

    // 左右のコントローラーから出るLineRenderer
    [SerializeField, Space(10)] LineRenderer rightLineRenderer;
    [SerializeField] LineRenderer leftLineRenderer;

    // 左右のRayの先端オブジェクト
    [SerializeField, Space(10)] GameObject rightLinePointerObject;
    [SerializeField] GameObject leftLinePointerObject;

    // 左右の選択先タイルを示すオブジェクト
    [SerializeField, Space(10)] GameObject rightSelector;
    [SerializeField] GameObject leftSelector;

    // 移動先を示すLineRenderer
    [SerializeField, Space(10)] LineRenderer moveLineRenderer;

    // Rayが当たるレイヤー
    [SerializeField, Space(10)] LayerMask rayLayerMask;

    // プレイヤーオブジェクト
    [SerializeField, Space(10)] GameObject VRPlayer;

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
        CheckKey();
        DebugKeyInput();

        DrawRay();
    }


    /// <summary>
    /// コールバックで呼ばれる関数
    /// </summary>
    void PlayerMove()
    {
        VRPlayer.transform.position = tempMovePos;
    }


    void CheckKey()
    {
        // 右人差し指ボタン
        if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
        {
            RaycastHit hit;

            if (Physics.Raycast(rightController.transform.position, rightController.transform.forward, out hit, 100f, rayLayerMask))
            {
                if (hit.collider.CompareTag("Tile_None") || hit.collider.CompareTag("Tile_CanBuild"))
                {
                    hit.collider.GetComponent<TileController>().StartBuild();
                }
                else if (hit.collider.CompareTag("Tower"))
                {
                    hit.collider.GetComponent<TowerController>().OnSelected();
                }
            }
        }

        // 左人差し指ボタン
        if (OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger))
        {
            RaycastHit hit;

            if (Physics.Raycast(leftController.transform.position, leftController.transform.forward, out hit, 100f, rayLayerMask))
            {
                if (Utils.CompareTags(hit.collider.tag, tileTags))
                {
                    if (!hit.collider.GetComponent<TileController>().isMovable)
                        return;

                    tempMovePos = hit.collider.GetComponent<TileController>().GetPos();
                    FadeManager.Instance.onFadeInComplete.AddListener(PlayerMove);
                    FadeManager.Instance.Fade();
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
    }


    /// <summary>
    /// コントローラーから出るLineRendererを描画する関数
    /// </summary>
    void DrawRay()
    {
        RaycastHit hit;

        // 右手側の処理↓↓↓
        if (Physics.Raycast(rightController.transform.position, rightController.transform.forward, out hit, 100f, rayLayerMask))
        {
            // LineRendererとLineの先端を表示する
            rightLineRenderer.enabled = true;
            rightLinePointerObject.SetActive(true);

            // LineRendererの描画点を設定する
            rightLineRenderer.SetPosition(0, rightController.transform.position);
            rightLineRenderer.SetPosition(1, hit.point);

            // Lineの先端を移動する
            rightLinePointerObject.transform.position = hit.point;

            // Rayが当たった先がタイルなら
            if (Utils.CompareTags(hit.collider.tag, tileTags))
            {
                rightSelector.SetActive(true);
                rightSelector.transform.position = hit.collider.GetComponent<TileController>().GetPos(0);
            }
            else
            {
                rightSelector.SetActive(false);
            }
        }
        else
        {
            // LineRendererとLineの先端を非表示にする
            rightLineRenderer.enabled = false;
            rightLinePointerObject.SetActive(false);
        }

        hit = new RaycastHit();

        // 左手側の処理↓↓↓
        if (Physics.Raycast(leftController.transform.position, leftController.transform.forward, out hit, 100f, rayLayerMask))
        {
            // LineRendererとLineの先端を表示する
            leftLineRenderer.enabled = true;
            leftLinePointerObject.SetActive(true);

            // LineRendererの描画点を設定する
            leftLineRenderer.SetPosition(0, leftController.transform.position);
            leftLineRenderer.SetPosition(1, hit.point);

            // Lineの先端を移動する
            leftLinePointerObject.transform.position = hit.point;

            // Rayが当たった先がタイルなら
            if (Utils.CompareTags(hit.collider.tag, tileTags) && hit.collider.GetComponent<TileController>().isMovable)
            {
                leftSelector.SetActive(true);
                leftSelector.transform.position = hit.collider.GetComponent<TileController>().GetPos(0);
                DrawMoveLine(hit.collider.gameObject);
            }
            else
            {
                leftSelector.SetActive(false);
                moveLineRenderer.enabled = false;
            }
        }
    }


    /// <summary>
    /// 移動先を示す曲線のラインを表示する関数
    /// </summary>
    /// <param name="targetTile"></param>
    void DrawMoveLine(GameObject targetTile)
    {
        moveLineRenderer.enabled = true;

        int middlePoints = 10;

        Vector3 endPos = targetTile.GetComponent<TileController>().GetPos();

        Vector3 control = (VRPlayer.transform.position + endPos) / 2 + Vector3.up;

        int totalPoints = middlePoints + 2;
        moveLineRenderer.positionCount = totalPoints;

        moveLineRenderer.SetPosition(0, VRPlayer.transform.position);
        for (int i = 1; i <= middlePoints; i++)
        {
            float t = (float)i / (float)(totalPoints - 1);
            var mpos = SampleCurve(VRPlayer.transform.position, endPos, control, t);
            moveLineRenderer.SetPosition(i, mpos);
        }
        moveLineRenderer.SetPosition(totalPoints - 1, endPos);

    }


    /// <summary>
    /// Ref: https://scrapbox.io/oculusgogo/LineRenderer%E3%82%92%E3%83%99%E3%82%B8%E3%82%A7%E6%9B%B2%E7%B7%9A%E3%81%A7%E6%8F%8F%E7%94%BB%E3%81%99%E3%82%8B
    /// 曲線の点を計算する関数
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="control"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    Vector3 SampleCurve(Vector3 start, Vector3 end, Vector3 control, float t)
    {
        // Interpolate along line S0: control - start;
        Vector3 Q0 = Vector3.Lerp(start, control, t);
        // Interpolate along line S1: S1 = end - control;
        Vector3 Q1 = Vector3.Lerp(control, end, t);
        // Interpolate along line S2: Q1 - Q0
        Vector3 Q2 = Vector3.Lerp(Q0, Q1, t);
        return Q2; // Q2 is a point on the curve at time t
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

        if (Input.GetKeyDown(KeyCode.L))
        {
            SoundManager.Instance.PlaySound(transform.position, 1);
        }
    }
}
