using System.Collections;
using UnityEngine;

public class GameStatus : MonoBehaviour
{
    public static GameStatus Instance { get; private set; }

    private bool infiniteMode = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ActivateInfiniteMode(float duration)
    {
        StartCoroutine(EnableInfiniteMode(duration));
    }

    IEnumerator EnableInfiniteMode(float duration)
    {
        infiniteMode = true;
        yield return new WaitForSeconds(duration);
        infiniteMode = false;
    }

    public bool IsInfiniteModeActive()
    {
        return infiniteMode;
    }
}
