using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    [Header("UI References")]
    public GameObject menuPanel;
    public Button continueButton;
    public Button exitButton;

    void Start()
    {
        menuPanel.SetActive(false);
        continueButton.onClick.AddListener(ContinueGame);
        exitButton.onClick.AddListener(ExitGame);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        bool isActive = menuPanel.activeSelf;
        menuPanel.SetActive(!isActive);

        Time.timeScale = isActive ? 1f : 0f;
    }

    void ContinueGame()
    {
        Time.timeScale = 1f;
        menuPanel.SetActive(false);
    }

    void ExitGame()
    {
        PlayerPrefs.Save();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}