using UnityEngine;

public class SignboardController : MonoBehaviour
{
    // 看板の画像を順番に入れておく配列（要素0に0の画像、要素1に1の画像をセット）
    public Sprite[] signSprites;
    // ★異変として登録する番号（例：1番の異変を「看板反転」とする）
    public int flipAnomalyNumber = 1;


    private SpriteRenderer spriteRenderer;
    private int lastNum = -2; // 前回の数値を覚えておく変数（初期値はあり得ない値）
    private int lastStrange = -1; // 異変の変化も見張るための変数
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 看板の数字（Num）か、異変（Strange）に変化があったときだけ更新する
        if (Stage.Num != lastNum || Stage.Strange != lastStrange)
        {
            lastNum = Stage.Num;
            lastStrange = Stage.Strange;

            UpdateSignboard();
        }
    }
    void UpdateSignboard()
    {
        // --- 1. 画像の更新処理 ---
        if (Stage.Num < 0)
        {
            spriteRenderer.sprite = null;
            return;
        }

        if (Stage.Num < signSprites.Length)
        {
            spriteRenderer.sprite = signSprites[Stage.Num];
        }

        // --- 2. 異変（反転）のチェック処理 ---
        // 現在の異変番号が、設定した「反転の異変番号」と一致するかどうか
        if (Stage.Strange == flipAnomalyNumber)
        {
            // 異変あり：看板の大きさを (Xをマイナス倍, Yはそのまま, Zもそのまま) にして反転させる
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else
        {
            // 異変なし（または別の異変）：看板の向きを元に戻す
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }




    void UpdateSignImage()
    {
        // -1（初期状態）のときは非表示、または専用の画像にする
        if (Stage.Num < 0)
        {
            spriteRenderer.sprite = null; // 一旦非表示
            return;
        }

        // 配列の範囲内かチェックして画像を差し替える
        if (Stage.Num < signSprites.Length)
        {
            spriteRenderer.sprite = signSprites[Stage.Num];
        }
        else
        {
            // 配列の数を超えた（例：8番出口に到達した）場合の処理
            Debug.Log("ゴールに到達、または画像が足りません！");
        }
    }
}