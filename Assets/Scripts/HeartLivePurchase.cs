using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using Bazaar.Poolakey.Data;
using Bazaar.Data;

public class HeartLivePurchase : MonoBehaviour
{
    [SerializeField] private string productId = "your_product_id_from_bazaar"; // ID محصول از بازار
    private PurchaseManager purchaseManager;
    private Button purchaseButton;
    [SerializeField] private GameObject purchaseFullMessage; // پیام "جون‌ها کامل هستند"

    private async void Start()
    {
        // پیدا کردن PurchaseManager
        purchaseManager = FindObjectOfType<PurchaseManager>();
        if (purchaseManager == null)
        {
            Debug.LogError("PurchaseManager not found in scene!");
            enabled = false;
            return;
        }

        // مقداردهی اولیه
        bool isInitialized = await purchaseManager.Init();
        if (!isInitialized)
        {
            Debug.LogError("Failed to initialize payment service");
            enabled = false;
            return;
        }

        // تنظیم دکمه خرید
        purchaseButton = GetComponent<Button>();
        if (purchaseButton != null)
        {
            purchaseButton.onClick.AddListener(OnPurchaseClicked);
            UpdatePurchaseButtonState(); // بررسی وضعیت دکمه در شروع
        }
        else
        {
            Debug.LogError("Purchase button not found!");
        }
    }

    private void UpdatePurchaseButtonState()
    {
        PlayerLives playerLives = PlayerLives.Instance;
        if (playerLives != null)
        {
            // همیشه دکمه را فعال نگه دارید (حتی وقتی جان‌ها پر هستند)
            purchaseButton.interactable = true;

            // نمایش یا پنهان کردن پیام "کامل است" (اختیاری)
            if (purchaseFullMessage != null)
            {
                purchaseFullMessage.SetActive(playerLives.CurrentLives >= playerLives.maxLives);
            }
        }
    }

    private async void OnPurchaseClicked()
    {
        // غیرفعال کردن دکمه هنگام پردازش خرید
        if (purchaseButton != null)
        {
            purchaseButton.interactable = false;
        }

        // انجام خرید
        var purchaseResult = await purchaseManager.Purchase(productId);

        if (purchaseResult.status == Status.Success)
        {
            Debug.Log("Purchase successful! Product: " + purchaseResult.data.productId);

            // مصرف محصول (برای محصولات مصرفی)
            var consumeResult = await purchaseManager.Consume(purchaseResult.data.purchaseToken);

            if (consumeResult.status == Status.Success)
            {
                Debug.Log("Product consumed successfully");
                // افزایش جان بازیکن
                IncreasePlayerLives();
            }
            else
            {
                Debug.LogError("Failed to consume product: " + consumeResult.message);
                // می‌توانید اینجا به کاربر اطلاع دهید که مشکلی پیش آمده
            }
        }
        else if (purchaseResult.status == Status.Canceled)
        {
            Debug.Log("Purchase canceled by user");
        }
        else
        {
            Debug.LogError("Purchase failed: " + purchaseResult.message);
            // می‌توانید اینجا به کاربر اطلاع دهید که مشکلی پیش آمده
        }

        // به‌روزرسانی وضعیت دکمه پس از خرید
        UpdatePurchaseButtonState();
    }

    private void IncreasePlayerLives()
    {
        // پیدا کردن PlayerLives در صحنه
        PlayerLives playerLives = PlayerLives.Instance;
        if (playerLives != null)
        {
            // اضافه کردن یک جان (حتی اگر پر باشد)
            playerLives.AddExtraLife();
            Debug.Log("Life added. Current lives: " + playerLives.CurrentLives);
        }
        else
        {
            Debug.LogError("PlayerLives instance not found!");
        }
    }

    private void OnDestroy()
    {
        // حذف listener هنگام از بین رفتن آبجکت
        if (purchaseButton != null)
        {
            purchaseButton.onClick.RemoveListener(OnPurchaseClicked);
        }
    }
}