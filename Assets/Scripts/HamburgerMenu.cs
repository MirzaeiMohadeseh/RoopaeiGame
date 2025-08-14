using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HamburgerMenu : MonoBehaviour
{
    [Header("References")]
    public Button hamburgerButton;
    public GameObject menuPanel;
    public Button continueButton;
    public Button restartButton;
    public Button exitButton;
    public Button shopButton;
    public GameObject ShopCanvas;

    [Header("Game References")]
    public BallController ballController;
    public ScoreCounter scoreCounter;

    [Header("Animation Settings")]
    public float animationDuration = 0.3f;
    private Vector3 shopButtonFinalPos = new Vector3(-130, -50, 0);

    void Start()
    {
        menuPanel.SetActive(false);
        shopButton.gameObject.SetActive(false);
        ShopCanvas.SetActive(false);

        hamburgerButton.onClick.AddListener(ToggleMenu);
        continueButton.onClick.AddListener(ContinueGame);
        restartButton.onClick.AddListener(RestartGame);
        exitButton.onClick.AddListener(ExitGame);
        shopButton.onClick.AddListener(OpenShop);
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
        bool willShow = !menuPanel.activeSelf;

        if (willShow)
        {
            menuPanel.SetActive(true);
            shopButton.gameObject.SetActive(true);

            StartCoroutine(AnimateShopButton(willShow));
        }
        else
        {
            StartCoroutine(AnimateShopButton(willShow, () => {
                menuPanel.SetActive(false);
                shopButton.gameObject.SetActive(false);
            }));
        }

        Time.timeScale = willShow ? 0f : 1f;
    }

    IEnumerator AnimateShopButton(bool show, System.Action onComplete = null)
    {
        RectTransform shopRect = shopButton.GetComponent<RectTransform>();
        Vector3 startPos = hamburgerButton.transform.position;
        Vector3 endPos = show ? shopButtonFinalPos : startPos;

        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            shopRect.anchoredPosition = Vector3.Lerp(
                show ? startPos : shopButtonFinalPos,
                endPos,
                elapsed / animationDuration);

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        shopRect.anchoredPosition = endPos;
        onComplete?.Invoke();
    }
    void ContinueGame()
    {
        menuPanel.SetActive(false);
        shopButton.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    void RestartGame()
    {
        // ۱. ریست امتیاز
        scoreCounter.ResetGame();

        // ۲. ریست توپ
        ballController.ResetBall();

        // ۳. بستن منو
        menuPanel.SetActive(false);
        shopButton.gameObject.SetActive(false);

        // ۴. بازنشانی زمان بازی
        Time.timeScale = 1f;

        Debug.Log("بازی با موفقیت ریست شد");
    }

    void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public void OpenShop()
    {
        ShopCanvas.SetActive(true); 
        menuPanel.SetActive(false);
        shopButton.gameObject.SetActive(false);
        Time.timeScale = 0f;
    }
}