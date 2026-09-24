using UnityEngine;

public class PosterAnomaly : MonoBehaviour
{
    [Header("--- 異変設定 ---")]
    // ★このポスターを反転させる異変の番号（例：2番の異変にする場合）
    public int myAnomalyNumber = 2;

    private int lastStrange = -1; // 前回の異変番号を覚えておく変数

    void Update()
    {
        // Stageクラスの異変（Strange）に変化があったときだけ実行する
        if (Stage.Strange != lastStrange)
        {
            lastStrange = Stage.Strange;
            CheckAnomaly();
        }
    }

    void CheckAnomaly()
    {
        // 現在選ばれている異変番号が、このポスターの異変番号と一致するか
        if (Stage.Strange == myAnomalyNumber)
        {
            // 【異変発生】左右反転（Xのスケールをマイナスにする）
            transform.localScale = new Vector3(-0.48f, 0.48f, 0.48f);
            Debug.Log($"異変発生：ポスター（異変番号 {myAnomalyNumber}）が反転しました。");
        }
        else
        {
            // 【異変なし、または別の異変】元の向き（1倍）に戻す
            transform.localScale = new Vector3(0.48f, 0.48f, 0.48f);
        }
    }
  
}