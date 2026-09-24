using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public static bool CanMove = false;
    public static int Num = -1;                // 看板の番号（現在のステージ進行度）
    public static int Strange = 0;             // 異変のバリエーション。0なら異変なし

    // すでに遭遇した異変を記録するリスト
    public static List<int> SelectedStrange = new List<int>();

    // 引数を「右（進む方向）に進んだかどうか」に変更
    public static void Change(bool isGoingRight)
    {
        // --- 1. プレイヤーの選択による看板（Num）の増減処理 ---

        // 【パターンA】部屋に「異変がない（0）」とき
        if (Strange == 0)
        {
            if (isGoingRight)
            {
                Num++; // 異変なしで右に進んだ ➔ 正解！
            }
            else
            {
                Num = 0; // 異変なしなのに左に戻った ➔ 不正解（リセット）
            }
        }
        // 【パターンB】部屋に「異変がある（0以外）」とき
        else
        {
            if (!isGoingRight)
            {
                Num++; // 異変ありで左に戻った ➔ 正解！
            }
            else
            {
                Num = 0; // 異変ありなのに右に進んでしまった ➔ 不正解（リセット）
            }
        }

        // --- 2. リストのクリアと既出の異変の記録 ---
        // ステージが0に戻ったら、これまでの異変履歴をリセット
        if (Num == 0)
        {
            SelectedStrange.Clear();
        }

        // 今回「異変あり（0以外）」だったら、遭遇済みリストに追加
        if (Strange != 0)
        {
            SelectedStrange.Add(Strange);
        }

        // --- 3. 次のステージの異変（Strange）を決定 ---
        // 50%の確率で「異変なし(0)」にする
        if (Random.Range(0, 2) == 0)
        {
            Strange = 0;
        }
        else
        {
            // 無限ループ防止：未遭遇の異変がまだ残っている場合のみ抽選
            if (SelectedStrange.Count < 7)
            {
                do
                {
                    Strange = Random.Range(1, 8); 
                }
                while (SelectedStrange.Contains(Strange));
            }
            else
            {
                // すべての異変を出した場合は強制的に異変なし
                Strange = 0;
            }
        }
        if (Num == 9) Strange = 0;
        Debug.Log("現在の看板（Num）:" + Num);
        Debug.Log("次の部屋の異変（Strange）:" + Strange);
    }
}