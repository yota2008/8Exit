using UnityEngine;
using UnityEngine.UI; // UI（Image）を扱うために必要
using System.Collections; // コルーチン（IEnumerator）を使うために必要

public class GenericAnomaly : MonoBehaviour
{
    public int myAnomalyNumber = 1;

    public enum AnomalyType
    {
        None,
        HideObject,       // オブジェクトを消す
        FlipX,            // 左右反転
        ScaleUp,          // 巨大化
        ChangeColor,      // 色を赤くする
        FlipY,            // 上下反転（ポスター逆さま）
        
    }
    public AnomalyType anomalyAction;

    [Header("--- 画面点滅用の設定 ---")]
    // ★ヒエラルキーで作った FlickerPanel をここにドラッグ＆ドロップします
    public Image flickerPanel;

    // 元の状態を記憶しておくための変数
    private Vector3 defaultScale;
    private Color defaultColor;
    private SpriteRenderer spriteRenderer;

    // 点滅コルーチンがすでに動いているかチェックするフラグ
    private bool isFlickering = false;

    void Start()
    {
        // （Startの内容は元のままです）
        defaultScale = transform.localScale;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            defaultColor = spriteRenderer.color;
        }

        // ゲーム開始時、パネルは完全に透明にしておく
        if (flickerPanel != null)
        {
            flickerPanel.color = new Color(0, 0, 0, 0);
        }
    }

    void Update()
    {
        if (Stage.Strange == myAnomalyNumber)
        {
            TriggerAnomaly();
        }
        else
        {
            ResetObject();
        }
    }

    // 異変を起こす処理
    void TriggerAnomaly()
    {
        switch (anomalyAction)
        {
            case AnomalyType.HideObject:
                if (spriteRenderer != null) spriteRenderer.enabled = false;
                break;

            case AnomalyType.FlipX:
                transform.localScale = new Vector3(-defaultScale.x, defaultScale.y, defaultScale.z);
                break;

            case AnomalyType.FlipY:
                transform.localScale = new Vector3(defaultScale.x, -defaultScale.y, defaultScale.z);
                break;

            case AnomalyType.ScaleUp:
                transform.localScale = defaultScale * 2f;
                break;

            case AnomalyType.ChangeColor:
                if (spriteRenderer != null) spriteRenderer.color = Color.red;
                break;

         
        }
    }

    // ★点滅のタイマー処理（コルーチン）
    IEnumerator FlickerCoroutine()
    {
        if (flickerPanel == null) yield break;
        isFlickering = true;

        // 異変が終わるまでループ
        while (Stage.Strange == myAnomalyNumber)
        {
            // 画面を暗くする（黒パネルのアルファを0.3〜0.7のランダムにする）
            flickerPanel.color = new Color(0, 0, 0, Random.Range(0.3f, 0.7f));
            // 0.05〜0.1秒待つ（高速点滅）
            yield return new WaitForSeconds(Random.Range(0.05f, 0.1f));

            // 一瞬完全に透明に戻す（明るくする）
            flickerPanel.color = new Color(0, 0, 0, 0);
            // 0.1〜0.3秒待つ
            yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
        }

        isFlickering = false;
    }

    // ★正常な状態に戻す処理
    void ResetObject()
    {
        // 1〜3. （元のResetObjectの内容はそのまま）
        if (spriteRenderer != null) spriteRenderer.enabled = true;
        transform.localScale = defaultScale;
        if (spriteRenderer != null) spriteRenderer.color = defaultColor;

        // 4. ★画面点滅パネルを完全に透明に戻す
        if (flickerPanel != null && !isFlickering) // コルーチンが止まっている時だけ
        {
            flickerPanel.color = new Color(0, 0, 0, 0);
        }
    }
}