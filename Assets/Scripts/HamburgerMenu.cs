using UnityEngine;
using UnityEngine.UI;

public class HamburgerMenu : MonoBehaviour
{
    [Header("References")]
    public Button hamburgerButton;
    public GameObject menuPanel;
    public Button continueButton;
    public Button exitButton;

    void Start()
    {
        menuPanel.SetActive(false);

        hamburgerButton.onClick.AddListener(ToggleMenu);
        continueButton.onClick.AddListener(ContinueGame);
        exitButton.onClick.AddListener(ExitGame);
    }
    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector2 touchPos = Input.GetTouch(0).position;
            if (RectTransformUtility.RectangleContainsScreenPoint(
                hamburgerButton.GetComponent<RectTransform>(),
                touchPos,
                null))
            {
                ToggleMenu();
            }
        }
    }

    void ToggleMenu()
    {
        bool shouldShow = !menuPanel.activeSelf;
        menuPanel.SetActive(shouldShow);

        Time.timeScale = shouldShow ? 0f : 1f;
    }

    void ContinueGame()
    {
        menuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}