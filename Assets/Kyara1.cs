using UnityEngine;
using UnityEngine.InputSystem;

public class Kyara1 : MonoBehaviour
{
    public enum CharacterState
    {
        Idle,
        Blink, // 瞬き状態を追加
        Walk
    }

    [Header("--- アニメーション画像 ---")]
    public Sprite[] idleSprites;    // 通常の待機（呼吸など）
    public Sprite[] blinkSprites;   // 瞬きのアニメ（1〜2枚の目を閉じた画像）
    public Sprite[] walkSprites;    // 歩き

    [Header("--- 設定 ---")]
    public float moveSpeed = 5.0f;

    [Header("--- アニメーション速度（値が大きいほど遅い） ---")]
    public int idleAnimationSpeed = 10;  // 待機アニメの速度
    public int blinkAnimationSpeed = 20; // 瞬きアニメの速度（★ここを大きくするとゆっくり瞬きします）
    public int walkAnimationSpeed = 2;   // 歩きアニメの速度（元の設定値をキープ）

    [Header("--- 瞬きの設定 ---")]
    public float blinkInterval = 4.0f; // 何秒ごとに瞬きするか
    private float blinkTimer;

    private CharacterState currentState = CharacterState.Idle;
    int currentFrame;
    int animationCounter;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        blinkTimer = blinkInterval; // タイマー初期化
    }

    void Update()
    {
        Vector3 moveDirection = Vector3.zero;

        // --- 1. 入力と状態の決定 ---
        if (Keyboard.current.leftArrowKey.isPressed)
        {
            moveDirection = Vector3.left;
            SetState(CharacterState.Walk);
            // スプライトを左に向ける（反転させる）
            spriteRenderer.flipX = true;

        }
        else if (Keyboard.current.rightArrowKey.isPressed)
        {
            moveDirection = Vector3.right;
            SetState(CharacterState.Walk);
            //スプライトを右に向ける（元に戻す）
            spriteRenderer.flipX = false;
        }
        else
        {
            // キーが押されていない時
            // 現在が「瞬き」じゃない場合のみ、通常の待機にする
            if (currentState != CharacterState.Blink)
            {
                SetState(CharacterState.Idle);
            }
        }

        // --- 2. 待機中の瞬きタイマー処理 ---
        if (currentState == CharacterState.Idle)
        {
            blinkTimer -= Time.deltaTime;
            if (blinkTimer <= 0)
            {
                SetState(CharacterState.Blink);
            }
        }

        // --- 3. 移動処理 ---
        if (moveDirection != Vector3.zero)
        {
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        }

        // --- 4. アニメーションの更新 ---
        UpdateAnimation();
    }

    void SetState(CharacterState newState)
    {
        if (currentState == newState) return;

        currentState = newState;
        animationCounter = 0;
        currentFrame = 0;
    }

    void UpdateAnimation()
    {
        Sprite[] targetSprites = null;

        switch (currentState)
        {
            case CharacterState.Idle:
                targetSprites = idleSprites;
                break;
            case CharacterState.Blink:
                targetSprites = blinkSprites;
                break;
            case CharacterState.Walk:
                targetSprites = walkSprites;
                break;
        }

        if (targetSprites == null || targetSprites.Length == 0) return;

        // アニメーションのコマ進め
        animationCounter++;
        if (animationCounter >= walkAnimationSpeed)
        {
            animationCounter = 0;
            currentFrame++;

            // アニメーションが最後まで再生された時の処理
            if (currentFrame >= targetSprites.Length)
            {
                if (currentState == CharacterState.Blink)
                {
                    // 瞬きが終わったら通常待機に戻り、タイマーをリセット
                    blinkTimer = blinkInterval + Random.Range(-1.0f, 1.0f); // 少しランダム性を持たせると自然になります
                    SetState(CharacterState.Idle);
                    return;
                }
                else
                {
                    // 通常のループアニメーション
                    currentFrame = 0;
                }
            }

            spriteRenderer.sprite = targetSprites[currentFrame];
        }
    }
}