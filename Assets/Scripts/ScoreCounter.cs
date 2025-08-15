using UnityEngine;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
    public static ScoreCounter Instance;

    [Header("References")]
    public BallController ball;
    public TMP_Text scoreText;
    public Transform startPosition;

    [Header("Lives")]
    public int maxLives = 3;
    private int currentLives;
    public GameObject[] lifeIcons; // آیکون‌های قلب در UI

    private int _score = 0;
    public bool _isGameActive = false;

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

    public void StartGame()
    {
        _isGameActive = true;
        _score = 0;
        currentLives = maxLives;
        UpdateScore();
        UpdateLivesUI();
        ball.ResetBall();
    }


    public void RegisterTouch()
    {
        if (!_isGameActive) return;

        _score++;
        UpdateScore();

        EnvironmentManager.Instance.UpdateEnvironment(_score);
    }


    public void HandleGroundHit()
    {
        if (!_isGameActive) return;

        currentLives--;

        UpdateLivesUI();

        if (currentLives <= 0)
        {
            _isGameActive = false;
            ResetGame();
        }
        else
        {
            ball.ResetBall(); // ادامه بازی با جان کمتر
        }
    }


    public void ResetGame()
    {
        _score = 0;
        _isGameActive = false;
        currentLives = maxLives;
        ball.ResetBall();
        scoreText.text = "...ﺪﯿﻧﺰﺑ ﻪﺑﺮﺿ ﻉﻭﺮﺷ ﯼﺍﺮﺑ";
        UpdateLivesUI();
        EnvironmentManager.Instance.ResetEnvironment();
    }
    void UpdateLivesUI()
    {
        for (int i = 0; i < lifeIcons.Length; i++)
        {
            lifeIcons[i].SetActive(i < currentLives);
        }
    }



    void UpdateScore()
    {
        scoreText.text = $"{_score} :ﺯﺎﯿﺘﻣﺍ ";
    }
}