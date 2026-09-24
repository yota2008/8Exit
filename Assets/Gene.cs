using System.Collections.Generic;
using UnityEngine;

public class Gene : MonoBehaviour
{
    [Header("BG生成設定")]
    [Tooltip("生成する背景プレハブ")]
    public GameObject haikei;

    [Tooltip("中心から左右にいくつずつ生成するか（合計は 1 + 2*count）")]
    public int count = 3;

    [Tooltip("背景パネル同士の間隔（X方向）")]
    public float spacing = 18.46f;

    [Tooltip("Y/Z のオフセット")]
    public Vector3 offset = new Vector3(0f, 0.19535f, 0f);

    [Header("プレイヤーテレポート設定")]
    [Tooltip("テレポートさせるプレイヤーの Transform（未設定ならシーンから自動検索）")]
    public Transform player;

    [Tooltip("エッジに近づいたと判定するマージン（ワールド単位）")]
    public float edgeMargin = 0.5f;

    [Tooltip("テレポート直後の連続テレポートを防ぐクールダウン（秒）")]
    public float teleportCooldown = 0.3f;

    [Header("BG通過でのテレポート")]
    [Tooltip("通り過ぎたときに移動させたい場所（BG1 オブジェクトの Transform をセット）")]
    public Transform bg1Target;

    [Tooltip("BG を通り過ぎた判定に使うマージン（中心を越したとみなす閾値）")]
    public float passMargin = 0.0f;

    [Tooltip("通過で一度だけテレポートするか（true の場合一度だけ）")]
    public bool teleportOnceOnPass = true;

    int n = 0;
    float lastTeleportTime = -999f;
    int loopCounter = 0;
    bool finalTeleported = false;

    // 生成したパネルのリスト（位置判定用）
    private List<Transform> spawnedPanels = new List<Transform>();

    // 前フレームのプレイヤー X（通過判定用）
    private float prevPlayerX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [System.Obsolete]
    void Start()
    {
        // 一度だけ背景を生成し、生成物をリスト保存
        for (int i = 0; i <= count; i++)
        {
            Vector3 posR = new Vector3(spacing * i, offset.y, offset.z);
            var goR = Instantiate(haikei, posR, Quaternion.identity);
            spawnedPanels.Add(goR.transform);

            if (i != 0)
            {
                Vector3 posL = new Vector3(-spacing * i, offset.y, offset.z);
                var goL = Instantiate(haikei, posL, Quaternion.identity);
                spawnedPanels.Add(goL.transform);
            }
        }

        // プレイヤーが Inspector にセットされていなければ Kyara1 を探す
        if (player == null)
        {
            var kyara = FindObjectOfType<Kyara1>();
            if (kyara != null) player = kyara.transform;
        }

        if (player != null) prevPlayerX = player.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (finalTeleported) return; // 最終テレポート済みなら何もしない
        if (player == null) return;
        if (count <= 0) return;

        float now = Time.time;

        // 背景の左右境界（中心が 0 の並びを想定）
        float leftEdge = -spacing * count;
        float rightEdge = spacing * count;

        // 画面外ワープ（変更点：絶対座標へテレポート）
        float span = (rightEdge - leftEdge); // = 2 * spacing * count

        if (player.position.x > rightEdge - edgeMargin && now - lastTeleportTime > teleportCooldown)
        {
            // 右端を超えたら左端の近くに瞬間移動させる（leftEdge + edgeMargin）
            player.position = new Vector3(leftEdge + edgeMargin, player.position.y, player.position.z);
            lastTeleportTime = now;
            CountLoopAndMaybeFinalTeleport();
        }
        else if (player.position.x < leftEdge + edgeMargin && now - lastTeleportTime > teleportCooldown)
        {
            // 左端を超えたら右端の近くに瞬間移動させる（rightEdge - edgeMargin）
            player.position = new Vector3(rightEdge - edgeMargin, player.position.y, player.position.z);
            lastTeleportTime = now;
            CountLoopAndMaybeFinalTeleport();
        }

        // 生成したパネルの「中心」を通り過ぎたら bg1Target にテレポートさせる判定
        if (bg1Target != null && now - lastTeleportTime > teleportCooldown)
        {
            float currX = player.position.x;

            // 各パネルについて、前フレームと今フレームで中心を横切ったか判定
            foreach (var panel in spawnedPanels)
            {
                if (panel == null) continue;
                float panelX = panel.position.x;

                // 右へ通過（左->右）
                if (prevPlayerX <= panelX - passMargin && currX > panelX + passMargin)
                {
                    TeleportToBG1();
                    break;
                }
                // 左へ通過（右->左）
                if (prevPlayerX >= panelX + passMargin && currX < panelX - passMargin)
                {
                    TeleportToBG1();
                    break;
                }
            }

            prevPlayerX = currX;
        }
        else
        {
            if (player != null) prevPlayerX = player.position.x;
        }
    }

    // ループ回数をカウントし、指定回数で最終テレポートする
    [Header("ループでの最終テレポート")]
    [Tooltip("ループ（背景端を越えてワープ）を何回繰り返したら最終テレポートするか")]
    public int loopsToTeleport = 3;
    [Tooltip("ループ回数到達後にプレイヤーを移動させるワールド座標")]
    public Vector3 finalTeleportPosition = Vector3.zero;

    private void CountLoopAndMaybeFinalTeleport()
    {
        loopCounter++;

        if (loopCounter >= loopsToTeleport)
        {
            if (player != null)
            {
                player.position = finalTeleportPosition;
            }
            finalTeleported = true;
        }
    }

    private void TeleportToBG1()
    {
        if (bg1Target == null || player == null) return;

        player.position = new Vector3(bg1Target.position.x, bg1Target.position.y, bg1Target.position.z);
        lastTeleportTime = Time.time;

        if (teleportOnceOnPass)
        {
            // 通過テレポートを一度だけにするなら spawnedPanels をクリアして二度目以降を防ぐ
            spawnedPanels.Clear();
        }
    }
}