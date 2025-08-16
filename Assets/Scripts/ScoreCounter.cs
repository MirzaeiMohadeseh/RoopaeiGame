using UnityEngine;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
    public static ScoreCounter Instance;

    [Header("References")]
    public BallController ball;
    public TMP_Text scoreText;
    private int _highScore = 0;
    public Transform startPosition;
    private bool doubleScore = false;

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
        _highScore = PlayerPrefs.GetInt("HighScore", 0);
        ResetGame();
    }

    public void StartGame()
    {
        _isGameActive = true;
        _score = 0;
        UpdateScore();
        PlayerLives.Instance.ResetGame();
        ball.ResetBall();
    }

    public void RegisterTouch()
    {
        if (!_isGameActive) return;

        _score += doubleScore ? 2 : 1;

        UpdateScore();

        if (_score > _highScore)
        {
            _highScore = _score;
            PlayerPrefs.SetInt("HighScore", _highScore);
        }

        EnvironmentManager.Instance.UpdateEnvironment(_score);
    }

    public void SetDoubleScore(bool state)
    {
        doubleScore = state;
    }

    public void HandleGroundHit()
    {
        if (!_isGameActive) return;
        ball.ResetBall();
        // استفاده از سیستم PlayerLives برای کاهش جان
        PlayerLives.Instance.LoseLife();
    }

    public void ResetGame()
    {
        _score = 0;
        _isGameActive = false;
        ball.ResetBall();
        PlayerLives.Instance.ResetGame();
        scoreText.text = "...ﺪﯿﻧﺰﺑ ﻪﺑﺮﺿ ﻉﻭﺮﺷ ﯼﺍﺮﺑ";
        EnvironmentManager.Instance.ResetEnvironment();
        
    }

    void UpdateScore()
    {
        scoreText.text = $" {_score} :ﺯﺎﯿﺘﻣﺍ\n {_highScore} :ﺩﺭﻮﮐﺭ";
    }
}