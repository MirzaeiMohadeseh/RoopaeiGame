using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PlayerLives : MonoBehaviour
{
    public static PlayerLives Instance { get; private set; }

    [Header("Settings")]
    public int maxLives = 3;
    public float respawnDelay = 2f;

    [Header("UI References")]
    public Image heartIcon; // آیکون قلب
    public TMP_Text livesText; // متن نمایش تعداد جان‌ها

    private int currentLives;
    private bool isRespawning;

    public int CurrentLives => currentLives;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ResetGame();
    }

    public void AddExtraLife()
    {
        if (currentLives < maxLives)
        {
            currentLives++;
            UpdateLifeUI();
        }
    }

    public void LoseLife()
    {
        if (isRespawning) return;

        if (GameStatus.Instance != null && GameStatus.Instance.IsInfiniteModeActive())
        {
            StartCoroutine(RespawnBall());
            return;
        }

        currentLives--;
        UpdateLifeUI();

        if (currentLives <= 0)
        {
            GameOver();
            ScoreCounter.Instance.ResetGame();
        }
        else
        {
            StartCoroutine(RespawnBall());
        }
    }

    IEnumerator RespawnBall()
    {
        isRespawning = true;
        yield return new WaitForSeconds(respawnDelay);

        BallController ball = FindObjectOfType<BallController>();
        if (ball != null)
        {
            ball.ResetBall();
        }

        isRespawning = false;
    }

    void UpdateLifeUI()
    {
        // نمایش آیکون قلب و تعداد جان‌ها
        if (heartIcon != null)
            heartIcon.enabled = currentLives > 0;
        
        if (livesText != null)
            livesText.text = currentLives.ToString();
    }

    void GameOver()
    {
        Time.timeScale = 0f;
        Debug.Log("Game Over!");
        
    }

    public void ResetGame()
    {
        currentLives = maxLives;
        UpdateLifeUI();
        Time.timeScale = 1f;

        BallController ball = FindObjectOfType<BallController>();
        if (ball != null)
        {
            ball.ResetBall();
        }
    }

    public void FillAllLives()
    {
        currentLives = maxLives;
        UpdateLifeUI();
    }
}