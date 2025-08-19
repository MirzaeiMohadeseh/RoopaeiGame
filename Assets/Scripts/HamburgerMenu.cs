using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HamburgerMenu : MonoBehaviour
{
    public static bool isMenuOpen = false;
    [Header("References")]
    public Button hamburgerButton;
    public GameObject menuPanel;
    public Button continueButton;
    public Button restartButton;
    public Button exitButton;
    public Button shopButton;
    public GameObject ShopCanvas;
    public Button backBtn;

    [Header("Game References")]
    public BallController ballController;
    public ScoreCounter scoreCounter;


    void Start()
    {
        menuPanel.SetActive(false);
        shopButton.gameObject.SetActive(false);
        ShopCanvas.SetActive(false);
        MusicManager.Instance.PlayMainMusic();

        hamburgerButton.onClick.AddListener(ToggleMenu);
        continueButton.onClick.AddListener(ContinueGame);
        restartButton.onClick.AddListener(RestartGame);
        exitButton.onClick.AddListener(ExitGame);
        shopButton.onClick.AddListener(OpenShop);
        backBtn.onClick.AddListener(CloseShop);
    }

    void Update()
    {

    }

    void ToggleMenu()
{
    bool willShow = !menuPanel.activeSelf;
    menuPanel.SetActive(willShow);
    shopButton.gameObject.SetActive(willShow);
    Time.timeScale = willShow ? 0f : 1f;
    isMenuOpen = willShow;

    if (willShow)
        MusicManager.Instance.PauseMusic(); 
    else
        MusicManager.Instance.PlayMainMusic();
}

void ContinueGame()
{
    menuPanel.SetActive(false);
    shopButton.gameObject.SetActive(false);
    Time.timeScale = 1f;
    isMenuOpen = false;
    MusicManager.Instance.PlayMainMusic();
}

public void OpenShop()
{
    ShopCanvas.SetActive(true);
    menuPanel.SetActive(false);
    shopButton.gameObject.SetActive(false);
    Time.timeScale = 0f;
    isMenuOpen = true;
    MusicManager.Instance.PlayShopMusic();
}

public void CloseShop()
{
    ShopCanvas.SetActive(false);
    Time.timeScale = 1f;
    isMenuOpen = false;
    MusicManager.Instance.PlayMainMusic();
}
void RestartGame() { 
    // ۱. ریست امتیاز 
    scoreCounter.ResetGame(); 
    // ۲. ریست توپ 
    ballController.ResetBall(); 
    // ۳. بستن منو 
    menuPanel.SetActive(false); shopButton.gameObject.SetActive(false);
    // ۴. بازنشانی زمان بازی 
    Time.timeScale = 1f; MusicManager.Instance.PlayMainMusic(); Debug.Log("بازی با موفقیت ریست شد");
    ContinueGame(); 
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