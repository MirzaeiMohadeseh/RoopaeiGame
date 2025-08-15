using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public static EnvironmentManager Instance;

    [Header("Environment Objects")]
    public GameObject[] environments; 

    [Header("Ball Sprites")]
    public Sprite[] ballSprites;

    private int currentStage = -1;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void UpdateEnvironment(int score)
    {
        int stage = (score / 20) % environments.Length;

        if (stage != currentStage)
        {
            currentStage = stage;
            SetEnvironment(environments[stage], ballSprites[stage]);
        }
    }

    private void SetEnvironment(GameObject activeEnv, Sprite ballSprite)
    {
        foreach (GameObject env in environments)
        {
            env.SetActive(false);
        }

        activeEnv.SetActive(true);

        if (ScoreCounter.Instance.ball != null)
        {
            SpriteRenderer sr = ScoreCounter.Instance.ball.GetComponent<SpriteRenderer>();
            if (sr != null)
                sr.sprite = ballSprite;
        }
    }

    public void ResetEnvironment()
    {
        currentStage = -1;
        SetEnvironment(environments[0], ballSprites[0]);
    }
}
