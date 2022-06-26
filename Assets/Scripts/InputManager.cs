using UnityEngine;
using System.Linq;
using TMPro;

public class InputManager : MonoBehaviour
{
    // 左右のコントローラー
    [SerializeField] GameObject rightController;
    [SerializeField] GameObject leftController;

    // 左右のコントローラーから出るLineRenderer
    [SerializeField] LineRenderer rightLineRenderer;
    [SerializeField] LineRenderer leftLineRenderer;

    // 左右のRayの先端オブジェクト
    [SerializeField] GameObject rightLinePointerObject;
    [SerializeField] GameObject leftLinePointerObject;

    // 移動先を示すLineRenderer
    [SerializeField] LineRenderer moveLineRenderer;


    // プレイヤーオブジェクト
    [SerializeField] GameObject VRPlayer;

    // 右スティック左右で回転する角度
    [SerializeField] int rotateRatio = 45;

    // CompareTagsで比較対象になるタイルのタグリスト
    string[] tileTags = { "Tile", "Tile_None", "Tile_Road", "Tile_CanBuild", "Tile_EnemyBase", "Tile_PlayerBase" };

    // CompareTagsで比較対象になるタワーのタグリスト
    string[] towerTags = { "Tower" };

    // Rayを透過するオブジェクトのタグリスト
    string[] canThroughTags = { "Enemy", "Hammer", "AttackRange" };

    // 移動でコールバックを使うための一時変数
    Vector3 tempMovePos;

    // 
    [SerializeField] GameObject constructionSite;



    void Update()
    {
        /*
        // Rayを移動する
        leftRayObject.positionCount = 2;
        leftRayObject.SetPosition(0, leftController.transform.position);
        leftRayObject.SetPosition(1, leftController.transform.position + leftController.transform.forward * 20f);

        rightRayObject.positionCount = 2;
        rightRayObject.SetPosition(0, rightController.transform.position);
        rightRayObject.SetPosition(1, rightController.transform.position + rightController.transform.forward * 20f);
        */

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
                    hit.collider.GetComponent<TowerController>().OnSelected();
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

        CheckRayHit();

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


    void CheckRayHit()
    {
        // 右手
        RaycastHit[] hits;
        hits = Physics.RaycastAll(rightController.transform.position, rightController.transform.forward, 100.0f);

        foreach (RaycastHit hit in hits)
        {
            if (!(CompareTags(tileTags, hit.collider.tag) || CompareTags(towerTags, hit.collider.tag)))
                continue;

            if (hit.collider.CompareTag("Tile_CanBuild"))
            {
                Vector3 pos = hit.collider.GetComponent<TileController>().GetPos() + new Vector3(0, 0.25f, 0);
                constructionSite.transform.position = pos;
                constructionSite.SetActive(true);
            }
            else
            {
                constructionSite.SetActive(false);
            }

            break;
        }


        // 左手
        hits = Physics.RaycastAll(leftController.transform.position, leftController.transform.forward, 100.0f);

        foreach (RaycastHit hit in hits)
        {
            if (!(CompareTags(tileTags, hit.collider.tag) || CompareTags(towerTags, hit.collider.tag)))
                continue;

            if (hit.collider.CompareTag("Tile_CanBuild"))
            {
                Vector3 pos = hit.collider.GetComponent<TileController>().GetPos() + new Vector3(0, 0.25f, 0);
                constructionSite.transform.position = pos;
                constructionSite.SetActive(true);
            }
            else
            {
                constructionSite.SetActive(false);
            }


            break;
        }
    }


    /// <summary>
    /// コントローラーから出るLineRendererを描画する関数
    /// </summary>
    void DrawRay()
    {
        RaycastHit[] hits;
        Vector3[] lineStartEndPos = new Vector3[2];
        bool hitFlag = false;

        // 右手
        hits = Physics.RaycastAll(rightController.transform.position, rightController.transform.forward, 100.0f);

        // Rayのhitを近いものから順に走査する
        foreach (RaycastHit hit in hits)
        {
            // Rayを貫通するオブジェクトならcontinue
            if (CheckRayThroughObject(hit.collider))
                continue;


            // Rayが当たった位置をLineRendererの終点にする
            lineStartEndPos[1] = hit.point;

            // Rayがオブジェクトに当たったことを示す
            hitFlag = true;

            break;
        }

        // Rayがオブジェクトに当たったなら
        if (hitFlag)
        {
            // LineRendererとLineの先端を表示する
            rightLineRenderer.enabled = true;
            rightLinePointerObject.SetActive(true);

            // LineRendererの始点をコントローラーにする
            lineStartEndPos[0] = rightController.transform.position;

            // LineRendererの描画点を設定する
            rightLineRenderer.SetPositions(lineStartEndPos);

            // Lineの先端を移動する
            rightLinePointerObject.transform.position = lineStartEndPos[1];
        }
        else
        {
            // LineRendererとLineの先端を非表示にする
            rightLineRenderer.enabled = false;
            rightLinePointerObject.SetActive(false);
        }

        // 左手
        hits = Physics.RaycastAll(leftController.transform.position, leftController.transform.forward, 100.0f);
        lineStartEndPos = new Vector3[2];
        hitFlag = false;

        // Rayのhitを近いものから順に走査する
        foreach (RaycastHit hit in hits)
        {
            // Rayを貫通するオブジェクトならcontinue
            if (CheckRayThroughObject(hit.collider))
                continue;

            // Rayが当たった位置をLineRendererの終点にする
            lineStartEndPos[1] = hit.point;

            // Rayがオブジェクトに当たったことを示す
            hitFlag = true;
            Debug.Log("Hit");
            Debug.Log(hit.collider.name);
            Debug.Log(hit.collider.tag);
            Debug.Log(CompareTags(tileTags, hit.collider.tag));

            // Rayが当たった先が移動可能なタイルなら
            if (CompareTags(tileTags, hit.collider.tag))
            {

                DrawMoveLine(hit.collider.gameObject);
                Debug.Log("Able");
            }
            else
            {
                moveLineRenderer.enabled = false;
                Debug.Log("No");
            }

            break;
        }

        // Rayがオブジェクトに当たったなら
        if (hitFlag)
        {
            // LineRendererとLineの先端を表示する
            leftLineRenderer.enabled = true;
            leftLinePointerObject.SetActive(true);

            // LineRendererの始点をコントローラーにする
            lineStartEndPos[0] = leftController.transform.position;

            // LineRendererの描画点を設定する
            leftLineRenderer.SetPositions(lineStartEndPos);

            // Lineの先端を移動する
            leftLinePointerObject.transform.position = lineStartEndPos[1];
        }
        else
        {
            // LineRendererとLineの先端を非表示にする
            leftLineRenderer.enabled = false;
            leftLinePointerObject.SetActive(false);
        }
    }


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
    /// Rayが貫通するオブジェクトかを調べる関数
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    bool CheckRayThroughObject(Collider col)
    {
        foreach (string t in canThroughTags)
        {
            if (col.tag == t)
                return true;

            if (col.name == "Model")
                return true;
        }

        return false;
    }

}
