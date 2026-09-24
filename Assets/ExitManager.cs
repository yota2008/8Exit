using UnityEngine;

public class ExitManager : MonoBehaviour
{
    [Header("--- 出口のオブジェクト ---")]
    // インスペクターで、表示させたい出口のゲームオブジェクト（ドアや通路、テキストなど）をセットします
    public GameObject exitObject;

    [Header("--- 消したいオブジェクト ---")]
    // 看板が9になったら【非表示】にしたいオブジェクト（ポスターなど）
    public GameObject posterObject;

    private int lastNum = -2;

    void Start()
    {
        // ゲーム開始時の初期設定
        if (exitObject != null) exitObject.SetActive(false);   // 出口は最初消しておく
        if (posterObject != null) posterObject.SetActive(true); // ポスターは最初出しておく
    }

    void Update()
    {
        // 看板の数字（Num）に変化があったときだけチェックする
        if (Stage.Num != lastNum)
        {
            lastNum = Stage.Num;
            CheckExitStatus();
        }
    }

    void CheckExitStatus()
    {
        // 看板の数字が 9 になったか判定
        if (Stage.Num == 9)
        {
            // 出口を表示し、ポスターを消す
            if (exitObject != null) exitObject.SetActive(true);
            if (posterObject != null) posterObject.SetActive(false);

            Debug.Log("クリア状態：出口を出現させ、ポスターを消しました。");
        }
        else
        {
            // 9以外のとき（間違えて0に戻ったときなど）は元の状態に戻す
            if (exitObject != null) exitObject.SetActive(false);
            if (posterObject != null) posterObject.SetActive(true);
        }
    }
}