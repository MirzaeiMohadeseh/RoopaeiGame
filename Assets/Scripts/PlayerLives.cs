using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerLives : MonoBehaviour
{
    public static PlayerLives Instance { get; private set; }

    [Header("Settings")]
    public int maxLives = 3;
    public float respawnDelay = 2f;

    [Header("UI References")]
    public Image[] lifeIcons; // آرایه‌ای از تصاویر قلب‌ها در UI
    public GameObject gameOverPanel;

    private int currentLives;
    private bool isRespawning;

    // 🔓 دسترسی فقط خواندنی به تعداد جان‌ها از بیرون
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
        for (int i = 0; i < lifeIcons.Length; i++)
        {
            lifeIcons[i].enabled = i < currentLives;
        }
    }

    void GameOver()
    {
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
        Debug.Log("Game Over!");
    }

    public void ResetGame()
    {
        currentLives = maxLives;
        UpdateLifeUI();
        gameOverPanel.SetActive(false);
        Time.timeScale = 1f;

        BallController ball = FindObjectOfType<BallController>();
        if (ball != null)
        {
            ball.ResetBall();
        }
    }
}
