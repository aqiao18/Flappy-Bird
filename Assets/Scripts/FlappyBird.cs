using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;

public class FlappyBird : MonoBehaviour
{
    
    private PlayerInput inputActions; 
    private Rigidbody2D rb;
    private bool isGameOver = false;
    private float minY, maxY;
    private Vector3 startPosition;
    [SerializeField] private Sprite birdUp;
    [SerializeField] private Sprite birdMid;
    [SerializeField] private Sprite birdDown;
    private SpriteRenderer sr;
    [SerializeField] private float flyForce = 5f;
    [SerializeField] private float flyTorque = 100f;
    private float smoothRotateSpeed = 5f;
    private float tiltAngle = 20f;

    private void Awake()
    {
        inputActions = new PlayerInput();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();


        // Bind fly action to the onFly method
        inputActions.Gameplay.Fly.performed += OnFly;
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0));

        minY = bottomLeft.y;
        maxY = topLeft.y;  
    }

    private void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // if (!UIManager.Instance.GameStarted) return;
        float tilt = (rb.velocity.y > 0) ? tiltAngle : (rb.velocity.y < 0 ? -tiltAngle : 0f);

        // Change sprite based on vertical velocity
        if (rb.velocity.y > 1f)
        {
            sr.sprite = birdUp;
        }
        else if (rb.velocity.y < -1f)
        {
            sr.sprite = birdDown;
        }
        else
        {
            sr.sprite = birdMid;
        }

        Quaternion targetRotation = Quaternion.Euler(0, 0, tilt);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * smoothRotateSpeed);

        if (!isGameOver && (transform.position.y - 0.7f <= minY || transform.position.y + 0.7f >= maxY))
        {
            SoundManager.Instance.PlayCollisionSound();
            BackgroundLooper.pauseScrolling = true;
            EndGame("Bird hit the edge.");
        }
        
    }

    private void EndGame(string reason)
    {
        Debug.Log($"Game Over! {reason}");
        Time.timeScale = 0f;
        isGameOver = true;
        int finalScore = ScoreManager.Instance.GetScore();
        UIManager.Instance.ShowGameOver(finalScore);
        ScoreManager.Instance.HideScore();
    }
    
    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void OnFly(InputAction.CallbackContext context)
    {
        if (isGameOver) return;
        if (UIManager.Instance.IsMainMenuOpen()) return;

        if (!UIManager.Instance.GameStarted)
        {
            UIManager.Instance.UnfreezeTime();
            UIManager.Instance.MarkGameStarted();
            BackgroundLooper.pauseScrolling = false;
        }

        SoundManager.Instance.PlayFlapSound();
        rb.AddForce(Vector2.up * flyForce, ForceMode2D.Impulse);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Pipes"))
        {
            SoundManager.Instance.PlayCollisionSound();
            BackgroundLooper.pauseScrolling = true;
            EndGame("Bird hit the pipe.");
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            SoundManager.Instance.PlayCollisionSound();
            BackgroundLooper.pauseScrolling = true;
            EndGame("Bird hit the ground.");
        }
    }

    public void ResetPlayer()
    {
        isGameOver = false;
        rb.velocity = Vector2.zero;
        transform.position = startPosition;
        transform.rotation = Quaternion.identity;
    }
}
