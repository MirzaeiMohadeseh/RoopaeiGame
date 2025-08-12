using UnityEngine;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
    public static ScoreCounter Instance;

    [Header("References")]
    public BallController ball;
    public TMP_Text scoreText;
    public Transform startPosition;

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
        UpdateScore();
        ball.ResetBall();
    }

    public void RegisterTouch()
    {
        if (!_isGameActive) return;

        _score++;
        UpdateScore();
    }

    public void HandleGroundHit()
    {
        if (!_isGameActive) return;

        _isGameActive = false;
        ResetGame();
    }

    void ResetGame()
    {
        ball.ResetBall();
        scoreText.text = "...ﺪﯿﻧﺰﺑ ﻪﺑﺮﺿ ﻉﻭﺮﺷ ﯼﺍﺮﺑ";
    }

    void UpdateScore()
    {
        scoreText.text = $"{_score} :ﺯﺎﯿﺘﻣﺍ ";
    }
}